using System;
using LitJson;
using System.Collections;

namespace AssemblyCSharp
{
	public class LoginRequest:ClientRequest
	{
		
		public LoginRequest (string data)
		{
			headCode = APIS.LOGIN_REQUEST;

			messageContent = data;

		}

		/**用于重新登录使用**/


		//退出登录
		public LoginRequest (){
			headCode = APIS.QUITE_LOGIN;
			if (GlobalData.getInstance().myAvatarVO != null) {
				messageContent = GlobalData.getInstance().myAvatarVO.account.uuid + "";
			}

		}


	}
}

