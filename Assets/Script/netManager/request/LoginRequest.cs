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
			/**
			LoginVo loginvo = new LoginVo ();
			if (data != null) {
				MyDebug.Log (data.toJson());
				try {
					
					loginvo.openId = (string)data ["openid"];
					loginvo.nickName = (string)data ["nickname"];
					loginvo.headIcon = (string)data ["headimgurl"];
					loginvo.unionid = (string)data ["unionid"];
					loginvo.province = (string)data ["province"];
					loginvo.city = (string)data ["city"];
					string sex = data ["sex"].ToString();
					loginvo.sex = int.Parse(sex);
					loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
				} catch (Exception e) {
					MyDebug.Log ("微信接口有变动！" + e.Message);
					TipsManagerScript.getInstance ().setTips ("请先打开你的微信客户端");
					return;
				}
			} else {

			}


			MyDebug.Log ("loginvo.IP" + loginvo.IP);

**/

			if (data == null) {
				LoginVo loginvo = new LoginVo ();

				loginvo.openId = "127" ;


				loginvo.nickName = "127";
				loginvo.headIcon = "imgicon";
				loginvo.unionid = "127";
				loginvo.province = "21sfsd";
				loginvo.city = "afafsdf";
				loginvo.sex = 1;
				loginvo.IP = GlobalData.getInstance().getIpAddress();
				data = JsonMapper.ToJson (loginvo);

				GlobalData.loginVo = loginvo;
				GlobalData.myAvatarVO = new AvatarVO ();
				GlobalData.myAvatarVO.account = new Account ();
				GlobalData.myAvatarVO.account.city = loginvo.city;
				GlobalData.myAvatarVO.account.openid = loginvo.openId;
				GlobalData.myAvatarVO.account.nickname = loginvo.nickName;
				GlobalData.myAvatarVO.account.headicon = loginvo.headIcon;
				GlobalData.myAvatarVO.account.unionid = loginvo.city;
				GlobalData.myAvatarVO.account.sex = loginvo.sex;
				GlobalData.myAvatarVO.IP = loginvo.IP;
			}
			messageContent = data;

		}

		/**用于重新登录使用**/


		//退出登录
		public LoginRequest (){
			headCode = APIS.QUITE_LOGIN;
			if (GlobalData.myAvatarVO != null) {
				messageContent = GlobalData.myAvatarVO.account.uuid + "";
			}

		}


	}
}

