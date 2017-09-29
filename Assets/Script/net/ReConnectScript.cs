using System;

namespace AssemblyCSharp{
	public class ReConnectScript 
	{
		private static ReConnectScript _instance;
		public static ReConnectScript getInstance(){
			if (_instance == null) {
				_instance = new ReConnectScript ();
				//_instance.Connect ();
			}
			return _instance;
		}
		public ReConnectScript ()
		{
		}
		/*
		public void ReConnectToServer(){
			CustomSocket.getInstance().Connect();
			if (GlobalDataScript.loginVo != null) {
				GameManager.getInstance().Server.requset (new LoginRequest(GlobalDataScript.loginVo,0));
			}

		}
*/


	}
}


