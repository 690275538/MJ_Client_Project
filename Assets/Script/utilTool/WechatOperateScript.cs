using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using AssemblyCSharp;
using System.IO;
using UnityEngine.UI;
using System;
using LitJson;

/**
 * 微信操作
 */ 
public class WechatOperateScript : MonoBehaviour {
	public ShareSDK shareSdk;
	private string picPath;
	//
	void Start () {
		if (shareSdk != null) {
			shareSdk.showUserHandler = getUserInforCallback;
			shareSdk.shareHandler = onShareCallBack;
		}

	}
	

	void Update () {
	
	}



	/**
	 * 登录，提供给button使用
	 * 
	 */ 
	public void login(){
		shareSdk.GetUserInfo(PlatformType.WeChat);
	

	}

	/**
	 * 获取微信个人信息成功回调,登录
	 *
	 */ 
	public void getUserInforCallback(int reqID, ResponseState state, PlatformType type, Hashtable data){
		//TipsManagerScript.getInstance ().setTips ("获取个人信息成功");

		if (data != null) {
			MyDebug.Log (data.toJson ());
			LoginVo loginvo = new LoginVo ();
			try {

				loginvo.openId = (string)data ["openid"];
				loginvo.nickName = (string)data ["nickname"];
				loginvo.headIcon = (string)data ["headimgurl"];
				loginvo.unionid = (string)data ["unionid"];
				loginvo.province = (string)data ["province"];
				loginvo.city = (string)data ["city"];
				string sex = data ["sex"].ToString ();
				loginvo.sex = int.Parse (sex);
				loginvo.IP = GlobalData.getInstance ().getIpAddress ();
				String msg = JsonMapper.ToJson (loginvo);

				CustomSocket.getInstance ().sendMsg (new LoginRequest (msg));

				GlobalData.loginVo = loginvo;
				GlobalData.myAvatarVO = new AvatarVO ();
				GlobalData.myAvatarVO.account = new Account ();
				GlobalData.myAvatarVO.account.city = loginvo.city;
				GlobalData.myAvatarVO.account.openid = loginvo.openId;
				MyDebug.Log(" loginvo.nickName:"+loginvo.nickName);
				GlobalData.myAvatarVO.account.nickname = loginvo.nickName;
				GlobalData.myAvatarVO.account.headicon = loginvo.headIcon;
				GlobalData.myAvatarVO.account.unionid = loginvo.city;
				GlobalData.myAvatarVO.account.sex = loginvo.sex;
				GlobalData.myAvatarVO.IP = loginvo.IP;

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
	public void onShareCallBack(int reqID,ResponseState state,PlatformType type,Hashtable result){
		if (state == ResponseState.Success) {
			TipsManager.getInstance ().setTips ("分享成功");

		} else if(state == ResponseState.Fail){
			MyDebug.Log ("shar fail :" + result ["error_msg"]);
		}
	}

	/**
	 * 分享战绩、战绩
	 */ 
	public void shareAchievementToWeChat(PlatformType platformType){
		StartCoroutine(GetCapture(platformType));
	}

	/**
	 * 执行分享到朋友圈的操作
	 */ 
	private void shareAchievement(PlatformType platformType){
		ShareContent customizeShareParams = new ShareContent();
		customizeShareParams.SetText("");
		customizeShareParams.SetImagePath(picPath);
		customizeShareParams.SetShareType(ContentType.Image);
		customizeShareParams.SetObjectID("");
		customizeShareParams.SetShareContentCustomize(platformType, customizeShareParams);
		shareSdk.ShareContent (platformType, customizeShareParams);
	}

	/**
	 * 截屏
	 * 
	 * 
	 */ 
	private IEnumerator GetCapture(PlatformType platformType)
	{
		yield return new WaitForEndOfFrame();
		if(Application.platform==RuntimePlatform.Android || Application.platform==RuntimePlatform.IPhonePlayer)  

			picPath=Application.persistentDataPath;  

		else if(Application.platform==RuntimePlatform.WindowsPlayer)  

			picPath=Application.dataPath;  

		else if(Application.platform==RuntimePlatform.WindowsEditor)

		{  
			picPath=Application.dataPath;  
			picPath= picPath.Replace("/Assets",null);  
		}   

		picPath = picPath + "/screencapture.png";

		MyDebug.Log ("picPath:" + picPath);

		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width,height,TextureFormat.RGB24,false);
		tex.ReadPixels(new Rect(0,0,width,height),0,0,true);
		tex.Apply ();
		byte[] imagebytes = tex.EncodeToPNG();//转化为png图
		tex.Compress(false);//对屏幕缓存进行压缩
		MyDebug.Log("imagebytes:"+imagebytes);
		if (File.Exists (picPath)) {
			File.Delete (picPath);
		}
		File.WriteAllBytes(picPath,imagebytes);//存储png图
		Destroy(tex);
		shareAchievement(platformType);
	}




	public void inviteFriend(){
		if(GlobalData.roomVO != null){
			RoomVO roomvo = GlobalData.roomVO;
			GlobalData.totalTimes = roomvo.roundNumber;
			GlobalData.surplusTimes = roomvo.roundNumber;
			string str="" ;

			if (roomvo.hong) {
				str += "红中麻将,";
			} else {
				if (roomvo.roomType == GameType.ZHUAN_ZHUAN) {
					str += "转转麻将,";
				} else if (roomvo.roomType == GameType.HUA_SHUI){
					str += "划水麻将,";
				}else if (roomvo.roomType == GameType.GI_PING_HU){
					str += "鸡平胡,";
				}
			}

			str += "大战"+roomvo.roundNumber+"局,";
			if (roomvo.ziMo == 1) {
				str += "只能自摸,";
			} else {
				str += "可抢杠胡,";
			}
			if (roomvo.addWordCard) {
				str += "有风牌,";
			}

			if (roomvo.xiaYu > 0) {
				str += "下鱼"+roomvo.xiaYu+"条,";
			}

			if (roomvo.ma > 0) {
				str += "抓"+roomvo.ma+"个码,";
			}
			if (roomvo.magnification > 0) {
				str += "倍率"+roomvo.magnification;
			}
			str += "有胆，你就来！";

			string title = "铁脚麻将    "+ "房间号："+roomvo.roomId;
			ShareContent customizeShareParams = new ShareContent();
			customizeShareParams.SetTitle(title);
			customizeShareParams.SetText (str);
			customizeShareParams.SetUrl (APIS.Download_URL);
			customizeShareParams.SetImageUrl(APIS.ImgUrl+"icon96.png");
			customizeShareParams.SetShareType(ContentType.Webpage);
			customizeShareParams.SetObjectID("");
			shareSdk.ShowShareContentEditor(PlatformType.WeChat, customizeShareParams);
		}
	}


	public void testLogin(string uin){
        LoginVo loginvo = new LoginVo();
        try
        {

            loginvo.openId = "" + uin;
            loginvo.nickName = "" + uin;
            loginvo.headIcon = "";
            loginvo.unionid = "" + uin;
            loginvo.province = "广东省";
            loginvo.city = "深圳";
			loginvo.sex = 1;
            loginvo.IP = GlobalData.getInstance().getIpAddress();
            String msg = JsonMapper.ToJson(loginvo);

            CustomSocket.getInstance().sendMsg(new LoginRequest(msg));

            GlobalData.loginVo = loginvo;
            GlobalData.myAvatarVO = new AvatarVO();
            GlobalData.myAvatarVO.account = new Account();
            GlobalData.myAvatarVO.account.city = loginvo.city;
            GlobalData.myAvatarVO.account.openid = loginvo.openId;
            GlobalData.myAvatarVO.account.nickname = loginvo.nickName;
            GlobalData.myAvatarVO.account.headicon = loginvo.headIcon;
            GlobalData.myAvatarVO.account.unionid = loginvo.city;
            GlobalData.myAvatarVO.account.sex = loginvo.sex;
            GlobalData.myAvatarVO.IP = loginvo.IP;

        }
        catch (Exception e)
        {
			Debug.Log(e.ToString());
            TipsManager.getInstance().setTips("请先打开你的微信客户端");
            return;
        }

        //CustomSocket.getInstance().sendMsg(new LoginRequest(null));
	}

}
