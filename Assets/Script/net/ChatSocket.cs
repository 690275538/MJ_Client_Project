using System;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using AssemblyCSharp;
using System.IO;

public class ChatSocket{


	private OnData _onData;
	private OnStatus _onStatus;

	private TcpClient _tcpclient;

	int waitLen = 0;
	bool isWait = false;
	byte[] sources;


	private SocketStatus _status;
	public SocketStatus Status {
		get {
			return _status;
		}
	}
	private void setStatus(SocketStatus s,string msg=null,TcpClient tcp=null){
		if (_status == s)
			return;
		_status = s;

		if (s == SocketStatus.DISCONNECT) {
			
			if (tcp != null) {
				if (tcp.GetStream () != null) {
					tcp.GetStream ().Close ();
				}
				tcp.Close ();
			}
			if (msg != null) {
				Debug.Log (msg);
				ClientResponse temp = new ClientResponse ();
				temp.headCode = APIS.TIP_MESSAGE;
				temp.message = msg;
				_onData (temp);
			}
			_tcpclient = null;

		}
		if (_onStatus != null) {
			_onStatus (_status);
		}
	}

	public ChatSocket(OnData onData,OnStatus onStatus){
		this._onData = onData;
		this._onStatus = onStatus;

		this._status = SocketStatus.UNDEFINE;
	}


    public void Connect()
    {
		if (_status == SocketStatus.CONNECTING || _status == SocketStatus.CONNECTED)
			return;

		setStatus (SocketStatus.CONNECTING);
		if (_tcpclient != null) {
			if (_tcpclient.GetStream () != null) {
				_tcpclient.GetStream ().Close ();
			}
			_tcpclient.Close ();
		}
        try
        {
			_tcpclient = new TcpClient();
            //防止延迟,即时发送!
            _tcpclient.NoDelay = true;
			StateObject state = new StateObject();
			state.client = _tcpclient;
			_tcpclient.BeginConnect(APIS.chatSocketUrl, 10112, new AsyncCallback(ConnectCallback), state);
        }
        catch(Exception e)
        {
			setStatus (SocketStatus.DISCONNECT,e.ToString());
        }
    }

	public bool sendMsg(ChatRequest client){
		if (_status != SocketStatus.CONNECTED || _tcpclient == null || !_tcpclient.Connected) {
			//Debug.Log ("未建立连接，不能发送请求");
			return false;
		}
		try
		{
			byte[] data = client.ToBytes();
			NetworkStream stream = _tcpclient.GetStream();
			stream.Write(data, 0, data.Length);
		}
		catch(Exception e)
		{
			setStatus (SocketStatus.DISCONNECT, e.ToString(), _tcpclient);
			return false;
		}
		return true;
	}


    private void ConnectCallback(IAsyncResult ar)
    {
		StateObject state = (StateObject)ar.AsyncState;
		TcpClient tcp = state.client;
        try
        {
			tcp.EndConnect(ar);
			if(tcp.Connected){
				Debug.Log("聊天服务器已经连接!");
				setStatus(SocketStatus.CONNECTED);
				NetworkStream stream = tcp.GetStream();
				if(stream.CanRead){
					stream.BeginRead(state.buffer, 0, StateObject.BufferSize,new AsyncCallback(ReadCallback), state);
				}
			}else{
				setStatus(SocketStatus.DISCONNECT,"聊天服务器连接失败!");
			}
            
        }
        catch(Exception ex)
        {
			setStatus(SocketStatus.DISCONNECT,ex.ToString());
        }
    }

    private void ReadCallback(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
		TcpClient tcp = state.client;
        //主动断开时
		if (!tcp.Connected)
        {
			setStatus (SocketStatus.DISCONNECT,"聊天服务器主动断开连接!",tcp);
            return;
        }
		int l;
		NetworkStream stream = tcp.GetStream();
        l = stream.EndRead(ar);
        state.totalBytesRead += l;
        if (l > 0)
        {
            byte[] dd = new byte[l];
            Array.Copy(state.buffer,0,dd,0,l);
			if (isWait) {
				byte[] temp = new byte[sources.Length+dd.Length];
				sources.CopyTo (temp,0);
				dd.CopyTo (temp,sources.Length);
				sources = temp;
				if (sources.Length >= waitLen) {
					ReceiveCallBack (sources.Clone() as byte[]);
					isWait = false;
					waitLen = 0;
				}
			} else {
				sources = null;
				ReceiveCallBack (dd);
			}
            stream.BeginRead(state.buffer, 0, StateObject.BufferSize,new AsyncCallback(ReadCallback), state);
        }
        else
        {
			setStatus (SocketStatus.DISCONNECT,"聊天聊天服务器被动断开连接",tcp);
        }
    }
		
	public int ReadInt(byte[] intbytes)
	{
		Array.Reverse(intbytes);
		return BitConverter.ToInt32(intbytes, 0);
	}

	public short ReadShort(byte[] intbytes){
		Array.Reverse(intbytes);
		return BitConverter.ToInt16(intbytes, 0);
	}

	private void ReceiveCallBack(byte[] m_receiveBuffer)
	{
		//通知调用端接收完毕
		try {
			MemoryStream ms = new MemoryStream (m_receiveBuffer);
			BinaryReader buffers = new BinaryReader (ms, UTF8Encoding.Default);
			readBuffer (buffers);
		} catch (Exception ex) {
			
			throw new Exception (ex.Message);
		}
	}

	private void readBuffer(BinaryReader buffers){
		byte flag = buffers.ReadByte();
		int lens = ReadInt(buffers.ReadBytes(4));

		if (lens > buffers.BaseStream.Length) {
			waitLen = lens;
			isWait = true;
			buffers.BaseStream.Position = 0;
			byte[] dd = new byte[buffers.BaseStream.Length];//TODO 明显有问题，应该是lens
			byte[] temp =  buffers.ReadBytes ((int)buffers.BaseStream.Length);
			Array.Copy (temp, 0, dd, 0, (int)buffers.BaseStream.Length);
			if (sources == null) {
				sources = dd;
			} 
			return;
		}
		int headcode = ReadInt(buffers.ReadBytes(4));
		int uuid = ReadInt(buffers.ReadBytes(4));
		int soundLen = ReadInt(buffers.ReadBytes(4));
		if(flag == 1){
			byte[] sound = buffers.ReadBytes (soundLen);
			ClientResponse response = new ClientResponse();
			response.headCode = headcode;
			response.bytes = sound;
			response.message = uuid.ToString ();
			_onData (response);
		}
		if (buffers.BaseStream.Position < buffers.BaseStream.Length) {
			readBuffer (buffers);
		}
	}

}

internal class StateObject
{
	public TcpClient client = null;
	public int totalBytesRead = 0;
	public const int BufferSize = 1024*1024*2;
	public string readType = null;
	public byte[] buffer = new byte[BufferSize];
}