#define TEST
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using cn.sharesdk.unity3d;


public class LoginView : MonoBehaviour, ISceneView {
	

	//public ShareSDK shareSdk;

	public Toggle agreeToggle;
	public Text versionText;
	public InputField uinInput;
	public InputField roomIDInput;

	public GameObject watingPanel;

	#region ISceneView implementation

	public void open (object data = null)
	{
		
	}

	public void close (object data = null)
	{
		Destroy (gameObject);
	}

	#endregion

	void Start () {

		//shareSdk.showUserHandler = getUserInforCallback;//注册获取用户信息回调

		GameManager.getInstance ().Server.onResponse += onResponse;

		versionText.text ="版本号：" +Application.version;

		#if UNITY_ANDROID
		WxPayImpl test = new WxPayImpl(gameObject);
		test.callTest ("dddddddddddddddddddddddddddd");
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape)){ //Android系统监听返回键，由于只有Android和ios系统所以无需对系统做判断
			SceneManager.getInstance().showExitPanel();
		} 

	}

	#if TEST

	public void login(){

		if (!GameManager.getInstance().Server.Connected) {
			GameManager.getInstance ().Server.connect ();
			TipsManager.getInstance ().setTips ("正在连接服务器...");
			return;
		}

		GlobalData.getInstance().reinitData ();//初始化界面数据
		if (agreeToggle.isOn) {
			if (uinInput.text != "" ) {
				GameManager.getInstance().WechatAPI.testLogin (uinInput.text);
				watingPanel.SetActive(true);
			}else{
				TipsManager.getInstance ().setTips ("请先输入QQ号");
			}
		} else {
			TipsManager.getInstance ().setTips ("请先同意用户使用协议");
		}

	}
	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.LOGIN_RESPONSE://登录回包
			if (roomIDInput.text != "") {
				RoomJoinVo roomJoinVo = new  RoomJoinVo ();
				roomJoinVo.roomId =int.Parse(roomIDInput.text);
				string sendMsg = JsonMapper.ToJson (roomJoinVo);
				GameManager.getInstance().Server.requset(new JoinRoomRequest(sendMsg));
			} else {
				onLoginResponse (response);
			}
			break;
		case APIS.BACK_LOGIN_RESPONSE://掉线登录回包
			onBackLoginResponse (response);
			break;
		case APIS.JOIN_ROOM_RESPONSE://加入房间
			if (response.status == 1) {
				RoomJoinResponseVo vo = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);
				GameManager.getInstance ().DataMgr.updateRoomVO (vo);

				SceneManager.getInstance ().changeToScene (SceneType.GAME);
			} else {
				TipsManager.getInstance ().setTips (response.message);
			}
			break;
		}
	}
	#else
	public void login(){

		if (!GameManager.getInstance().Server.Connected) {
			GameManager.getInstance ().Server.connect ();
			TipsManager.getInstance ().setTips ("正在连接服务器...");
			return;
		}

		GlobalData.getInstance().reinitData ();//初始化界面数据
		if (agreeToggle.isOn) {
			GameManager.getInstance().WechatAPI.login ();
			watingPanel.SetActive(true);
		} else {
			TipsManager.getInstance ().setTips ("请先同意用户使用协议");
		}

	}

	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.LOGIN_RESPONSE://登录回包
			onLoginResponse (response);
			break;
		case APIS.BACK_LOGIN_RESPONSE://掉线登录回包
			onBackLoginResponse (response);
			break;
		}
	}

	#endif
	private void onLoginResponse(ClientResponse response){
		watingPanel.SetActive(false);

		SoundManager.getInstance ().playBGM ();
		if (response.status == 1) {
			
			GlobalData.getInstance().myAvatarVO = JsonMapper.ToObject<AvatarVO> (response.message);
			GameManager.getInstance().Server.requset (new LoginChatRequest (GlobalData.getInstance().myAvatarVO.account.uuid));

			SceneManager.getInstance ().changeToScene (SceneType.HOME);


		} else {
			TipsManager.getInstance ().setTips (response.message);
		}
	}
	private void onBackLoginResponse(ClientResponse response){

		watingPanel.SetActive(false);


		RoomJoinResponseVo vo = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);
		GameManager.getInstance ().DataMgr.updateRoomVO (vo);
		GameManager.getInstance().Server.requset (new LoginChatRequest(GlobalData.getInstance().myAvatarVO.account.uuid));
		SceneManager.getInstance ().changeToScene (SceneType.GAME);
		SceneManager.getInstance ().CurScenePanel.GetComponent<GameView> ().Data.isReEnter = true;
	}


	void OnDestroy(){
		GameManager.getInstance ().Server.onResponse -= onResponse;
	}
}
