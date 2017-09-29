using System;

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


		ServerProxy server;
		public ServerProxy Server {
			get {
				return server;
			}
		}

		DataManager dataMgr;
//		public DataManager DataMgr {
//			get {
//				return dataMgr;
//			}
//		}
		public GameManager ()
		{
			server = new ServerProxy ();
			server.init ();
			server.onDisconnect += disConnetNotice;
			server.onResponse += onResponse;


			dataMgr = new DataManager ();
			server.onResponse += dataMgr.onResponse;
		}

		void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.OTHER_TELE_LOGIN:
				disConnetNotice ();
				TipsManager.getInstance ().setTips ("你的账号在其他设备登录");
				break;
			case APIS.ERROR_RESPONSE:
				TipsManager.getInstance ().setTips (response.message);
				break;
			case APIS.HOST_UPDATEDRAW_RESPONSE:
				updatePrizeCount (response);
				break;
			}
				
		}
		private void updatePrizeCount(ClientResponse response){
			int prizecount =int.Parse(response.message);
			GlobalData.myAvatarVO.account.prizecount = prizecount;
			if(CommonEvent.getInstance().prizeCountChange !=null){
				CommonEvent.getInstance ().prizeCountChange();
			}
		}
		public void FixedUpdate(){
			server.FixedUpdate ();
		}

		private void  disConnetNotice(){
			SceneManager.getInstance ().changeToScene (SceneType.LOGIN);
			SocketEventHandle.getInstance ().clearListener ();
		}
	}
}

