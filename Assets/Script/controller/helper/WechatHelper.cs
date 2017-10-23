using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using AssemblyCSharp;
using System.IO;
using UnityEngine.UI;
using System;
using LitJson;

namespace AssemblyCSharp
{
	/**
 * 微信操作
 */ 
	public class WechatHelper : MonoBehaviour
	{
		public ShareSDK shareSdk;
		private string picPath;
		//
		void Start ()
		{
			if (shareSdk != null) {
				shareSdk.showUserHandler = getUserInforCallback;
				shareSdk.shareHandler = onShareCallBack;
			}

		}


		/**
	 * 登录，提供给button使用
	 * 
	 */ 
		public void login ()
		{
			shareSdk.GetUserInfo (PlatformType.WeChat);
		}

		/**
	 * 获取微信个人信息成功回调,登录
	 *
	 */ 
		public void getUserInforCallback (int reqID, ResponseState state, PlatformType type, Hashtable data)
		{
			//TipsManagerScript.getInstance ().setTips ("获取个人信息成功");

			if (data != null) {
				MyDebug.Log (data.toJson ());
				LoginVO lvo = new LoginVO ();
				try {

					lvo.openId = (string)data ["openid"];
					lvo.nickName = (string)data ["nickname"];
					lvo.headIcon = (string)data ["headimgurl"];
					lvo.unionid = (string)data ["unionid"];
					lvo.province = (string)data ["province"];
					lvo.city = (string)data ["city"];
					string sex = data ["sex"].ToString ();
					lvo.sex = int.Parse (sex);
					lvo.IP = GameManager.getInstance ().getIpAddress ();
					String msg = JsonMapper.ToJson (lvo);

					GameManager.getInstance ().Server.requset (APIS.LOGIN_REQUEST,msg);

					AvatarVO avo = new AvatarVO ();
					avo.account = new Account ();
					avo.account.city = lvo.city;
					avo.account.openid = lvo.openId;
					avo.account.nickname = lvo.nickName;
					avo.account.headicon = lvo.headIcon;
					avo.account.unionid = lvo.city;
					avo.account.sex = lvo.sex;
					avo.IP = lvo.IP;
					GlobalData.getInstance ().myAvatarVO = avo;

					MyDebug.Log (" loginvo.nickName:" + lvo.nickName);

				} catch (Exception e) {
					MyDebug.Log ("微信接口有变动！" + e.Message);
					TipsManager.getInstance ().setTips ("请先打开你的微信客户端");
					return;
				}
			} else {
				TipsManager.getInstance ().setTips ("微信登录失败");
			}




		}


		/***
	 * 分享战绩成功回调
	 */
		public void onShareCallBack (int reqID, ResponseState state, PlatformType type, Hashtable result)
		{
			if (state == ResponseState.Success) {
				TipsManager.getInstance ().setTips ("分享成功");

			} else if (state == ResponseState.Fail) {
				MyDebug.Log ("shar fail :" + result ["error_msg"]);
			}
		}

		/**
	 * 分享战绩、战绩
	 */ 
		public void shareAchievementToWeChat (PlatformType platformType)
		{
			StartCoroutine (GetCapture (platformType));
		}

		/**
	 * 执行分享到朋友圈的操作
	 */ 
		private void shareAchievement (PlatformType platformType)
		{
			ShareContent s = new ShareContent ();
			s.SetText ("");
			s.SetImagePath (picPath);
			s.SetShareType (ContentType.Image);
			s.SetObjectID ("");
			s.SetShareContentCustomize (platformType, s);
			shareSdk.ShareContent (platformType, s);
		}

		/**
	 * 截屏
	 * 
	 * 
	 */ 
		private IEnumerator GetCapture (PlatformType platformType)
		{
			yield return new WaitForEndOfFrame ();
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
				picPath = Application.persistentDataPath;
			else if (Application.platform == RuntimePlatform.WindowsPlayer)
				picPath = Application.dataPath;
			else if (Application.platform == RuntimePlatform.WindowsEditor) {  
				picPath = Application.dataPath;  
				picPath = picPath.Replace ("/Assets", null);  
			}   

			picPath = picPath + "/screencapture.png";

			MyDebug.Log ("picPath:" + picPath);

			int width = Screen.width;
			int height = Screen.height;
			Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);
			tex.ReadPixels (new Rect (0, 0, width, height), 0, 0, true);
			tex.Apply ();
			byte[] imagebytes = tex.EncodeToPNG ();//转化为png图
			tex.Compress (false);//对屏幕缓存进行压缩
			MyDebug.Log ("imagebytes:" + imagebytes);
			if (File.Exists (picPath)) {
				File.Delete (picPath);
			}
			File.WriteAllBytes (picPath, imagebytes);//存储png图
			Destroy (tex);
			shareAchievement (platformType);
		}




		public void inviteFriend ()
		{
			RoomVO rvo = GlobalData.getInstance ().roomVO;
			if (rvo != null) {
				


				string title = "铁脚麻将    " + "房间号：" + rvo.roomId;
				ShareContent s = new ShareContent ();
				s.SetTitle (title);
				s.SetText (GameHelper.getHelper ().getInviteRuleStr (rvo));
				s.SetUrl (Constants.Download_URL);
				s.SetImageUrl (Constants.ImgUrl + "icon96.png");
				s.SetShareType (ContentType.Webpage);
				s.SetObjectID ("");
				shareSdk.ShowShareContentEditor (PlatformType.WeChat, s);
			}
		}


		public void testLogin (string uin)
		{
			LoginVO lvo = new LoginVO ();
			try {

				lvo.openId = "" + uin;
				lvo.nickName = "" + uin;
				lvo.headIcon = "";
				lvo.unionid = "" + uin;
				lvo.province = "广东省";
				lvo.city = "深圳";
				lvo.sex = 1;
				lvo.IP = GameManager.getInstance ().getIpAddress ();
				String msg = JsonMapper.ToJson (lvo);

				GameManager.getInstance ().Server.requset (APIS.LOGIN_REQUEST,msg);

				AvatarVO avo = new AvatarVO ();
				avo.account = new Account ();
				avo.account.city = lvo.city;
				avo.account.openid = lvo.openId;
				avo.account.nickname = lvo.nickName;
				avo.account.headicon = lvo.headIcon;
				avo.account.unionid = lvo.city;
				avo.account.sex = lvo.sex;
				avo.IP = lvo.IP;
				GlobalData.getInstance ().myAvatarVO = avo;
			} catch (Exception e) {
				Debug.Log (e.ToString ());
				TipsManager.getInstance ().setTips ("请先打开你的微信客户端");
				return;
			}

			//GameManager.getInstance().Server.requset(new LoginRequest(null));
		}

	}
}