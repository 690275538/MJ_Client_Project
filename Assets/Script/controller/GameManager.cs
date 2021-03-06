﻿//#define TEST
using System;
using UnityEngine;
using LitJson;

namespace AssemblyCSharp
{
	
	public class GameManager
	{
		private static GameManager _instance;
		public static GameManager getInstance(){
			if (_instance == null) {
				_instance = new GameManager();
			}
			return _instance;
		}

		
		public WechatHelper wechatAPI;
		/**微信接口**/
		public WechatHelper WechatAPI {
			get {
				return wechatAPI;
			}
		}

		private GameObject stage;
		/** UI Stage **/
		public GameObject Stage {
			get {
				return stage;
			}
		}


		private GameObject root;
		/** Game Root inside the Stage **/
		public GameObject Root {
			get {
				return root;
			}
		}

		#if TEST
		ServerProxyTest server;
		public ServerProxyTest Server {
			get {
				return server;
			}
		}
		#else
		ServerProxy server;
		public ServerProxy Server {
			get {
				return server;
			}
		}
		#endif
		DataManager dataMgr;
		public DataManager DataMgr {
			get {
				return dataMgr;
			}
		}

		private UIStage uiStage;
		private UpdateHelper updateHelper;
		public GameManager ()
		{
			#if TEST
			server = new ServerProxyTest ();
			server.init ();
			#else
			server = new ServerProxy ();
			server.init ();
			#endif
			server.onDisconnect += disConnetNotice;
			server.onResponse += onResponse;


			dataMgr = new DataManager ();
			server.onResponse += dataMgr.onResponse;
		}
		public void init(UIStage uiStage,GameObject root,GameObject login){
			this.uiStage = uiStage;
			this.stage = uiStage.gameObject;
			this.root = root;
			wechatAPI = stage.GetComponent<WechatHelper>();

			TipsManager.getInstance ().init (stage.transform);
			SceneManager.getInstance ().init (root.transform,login);

			MicrophoneManager.getInstance ().init ();

			SoundManager.getInstance ();//TODO 后面改

			updateHelper = new UpdateHelper ();
			this.uiStage.StartCoroutine (updateHelper.updateCheck());
		}
		void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.OTHER_TELE_LOGIN:
				disConnetNotice ();
				TipsManager.getInstance ().setTips ("你的账号在其他设备登录");
				break;
			case APIS.TIP_MESSAGE:
				TipsManager.getInstance ().setTips (response.message);
				break;
			case APIS.CLOSE_RESPONSE:
				TipsManager.getInstance ().setTips ("服务器关闭了");
				break;
			case APIS.ERROR_RESPONSE:
				TipsManager.getInstance ().setTips (response.message);
				break;
			case APIS.HOST_UPDATEDRAW_RESPONSE:
				dataMgr.parsePrizeCount (response.message);
				break;
			case APIS.GAME_BROADCAST:
				dataMgr.parseBroadCast (response.message);
				break;
			case APIS.HUPAIALL_RESPONSE://全局结束
				GlobalData.getInstance ().gamingData.finalGameEndVo = JsonMapper.ToObject<FinalGameEndVo> (response.message);

				SceneManager.getInstance ().showGameOverView (1, GlobalData.getInstance().gamingData);
				GlobalData.getInstance ().resetDataForNewRoom ();
				SceneManager.getInstance ().changeToScene (SceneType.HOME);
				break;
			}
				
		}
		public void FixedUpdate(){
			server.FixedUpdate ();
		}

		private void  disConnetNotice(){
			SceneManager.getInstance ().changeToScene (SceneType.LOGIN);
		}

		public string getIpAddress()
		{
			string tempip = "";
			//		try
			//		{
			//			WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
			//			Stream s = wr.GetResponse().GetResponseStream();
			//			StreamReader sr = new StreamReader(s, Encoding.Default);
			//			string all = sr.ReadToEnd(); //读取网站的数据
			//
			//			int start = all.IndexOf("[")+1;
			//		    int end = all.IndexOf("]");
			//		    int count = end-start;
			//			tempip = all.Substring(start,count);
			//			sr.Close();
			//			s.Close();
			//		}
			//		catch
			//		{
			//		}
			return tempip;
		}
	}
}

