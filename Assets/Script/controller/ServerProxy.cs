using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace AssemblyCSharp
{
	public class ServerProxy
	{
		public delegate void OnResponse (ClientResponse response);

		public delegate void OnDisconnect ();

		public event OnResponse onResponse;
		public event OnDisconnect onDisconnect;

		CustomSocket _socket;
		ChatSocket _chatSocket;
		bool _isDisconnet;
		private List<ClientResponse> _cache;

		public ServerProxy ()
		{
			
		}

		public void init ()
		{
			_socket = new CustomSocket (onSocketData, onSocketStatus);
			_chatSocket = new ChatSocket (onSocketData, null);
			_cache = new List<ClientResponse> ();

			heartbeatThread ();//开一个线程维持与服务的心跳
		}

		public void connect ()
		{
			_socket.Connect ();
			_chatSocket.Connect ();
		}

		public bool Connected {
			get {
				return _socket.Status == SocketStatus.CONNECTED;
			}
		}
		public bool ChatConnected {
			get {
				return _chatSocket.Status == SocketStatus.CONNECTED;
			}
		}

		public void requset(int cmd,string msg=""){
			
			if (cmd != APIS.HEART_REQUEST) {
				Debug.Log ("req: " + cmd.ToString ("x8") + " , " + APIS.getCmdName(cmd) + " ,\n " + msg);
			}

			_socket.sendMsg (new ClientRequest (cmd, msg));
		}
		public void requset (ChatRequest q)
		{
			Debug.Log ("req: " + q.headCode.ToString ("x8") + " , " + APIS.getCmdName(q.headCode) + " ,\n " + q.userList.ToArray ());
			_chatSocket.sendMsg (q);
		}
		private void onSocketData (ClientResponse response)
		{
			_cache.Add (response);
		}

		public void FixedUpdate ()
		{
			while (_cache.Count > 0) {
				if (_cache [0].headCode != APIS.HEART_RESPONSE)
					Debug.Log ("res: " + _cache [0].headCode.ToString ("x8") + " , " + APIS.getCmdName(_cache [0].headCode) + " ,\n " + _cache [0].message);
				try {
					onResponse (_cache [0]);
				} catch (Exception e) {
					Debug.Log ("命令出错：" + _cache [0].headCode.ToString ("x8") + " " + _cache [0].message);
					Debug.Log (e.ToString ());
				}
				_cache.RemoveAt (0);
			}
			if (_isDisconnet) {
				_isDisconnet = false;
				if (onDisconnect != null) {
					onDisconnect ();
				}
			}

		}

		private void onSocketStatus (SocketStatus status)
		{
			_isDisconnet = status == SocketStatus.DISCONNECT;
		}



		private void heartbeatThread ()
		{
			Thread thread = new Thread (sendHeartbeat);
			thread.IsBackground = true;
			thread.Start ();
		}


		private void sendHeartbeat ()
		{
			requset (APIS.HEART_REQUEST, "");
			Thread.Sleep (20000);//20秒发一次心跳
			sendHeartbeat ();
		}


	}
}

