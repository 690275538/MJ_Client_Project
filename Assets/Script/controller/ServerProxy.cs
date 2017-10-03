using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ServerProxy
	{
		public delegate void OnResponse (ClientResponse response);

		public delegate void OnDisconnect ();

		public event OnResponse onResponse;
		public event OnDisconnect onDisconnect;

		CustomSocket _socket;
		bool _isDisconnet;
		private List<ClientResponse> _cache;

		public ServerProxy ()
		{
			
		}

		public void init ()
		{
			_socket = new CustomSocket (onSocketData, onSocketStatus);
			_cache = new List<ClientResponse> ();

			heartbeatThread ();//开一个线程维持与服务的心跳
		}

		public void connect ()
		{
			_socket.Connect ();

			ChatSocket.getInstance ().Connect ();
		}

		public bool Connected {
			get {
				return _socket.Status == SocketStatus.CONNECTED;
			}

		}

		public void requset (ClientRequest q)
		{
			Debug.Log("req: "+q.headCode.ToString ("x8")+" , "+q.messageContent);
			_socket.sendMsg (q);
		}

		private void onSocketData (ClientResponse response)
		{
			_cache.Add (response);
			SocketEventHandle.getInstance ().addResponse (response);
		}

		public void FixedUpdate ()
		{
			while (_cache.Count > 0) {
				try {
					onResponse (_cache [0]);
				} catch (Exception e) {
					Debug.Log ("命令出错：" + _cache [0].headCode.ToString ("x8") + " " + _cache [0].message);
					Debug.Log (e.ToString ());
				}

				Debug.Log("res: "+_cache [0].headCode.ToString ("x8")+" , "+_cache [0].message);
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
			_socket.sendMsg (new HeadRequest ());
			Thread.Sleep (20000);//20秒发一次心跳
			sendHeartbeat ();
		}


	}
}

