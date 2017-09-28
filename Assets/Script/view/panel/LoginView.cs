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

	public GameObject watingPanel;

	#region ISceneView implementation

	public void open (object data = null)
	{
		
	}

	public void close (object data = null)
	{
		
		removeListener ();
		Destroy (this);
		Destroy (gameObject);
	}

	#endregion

	void Start () {

		//shareSdk.showUserHandler = getUserInforCallback;//注册获取用户信息回调


		addListener ();
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

	public void login(){
		
		if (!CustomSocket.getInstance ().isConnected) {
			CustomSocket.getInstance ().Connect ();
			ChatSocket.getInstance ().Connect();
			TipsManager.getInstance ().setTips ("正在连接服务器...");
			return;
		}

		GlobalData.reinitData ();//初始化界面数据
		if (agreeToggle.isOn) {
			//GlobalDataScript.getInstance ().wechatOperate.login ();//TODO 
			//watingPanel.SetActive(true);
			if (uinInput.text != "" ) {
				GlobalData.getInstance ().wechatOperate.testLogin (uinInput.text);
				watingPanel.SetActive(true);
			}else{
				TipsManager.getInstance ().setTips ("请先输入QQ号");
			}
		} else {
			TipsManager.getInstance ().setTips ("请先同意用户使用协议");
		}



	}

	private void addListener(){
		SocketEventHandle.getInstance ().LoginCallBack += onLoginCallBack;
		SocketEventHandle.getInstance ().RoomBackResponse += onRoomBackResponse;
	}
	private void removeListener(){
		SocketEventHandle.getInstance ().LoginCallBack -= onLoginCallBack;
		SocketEventHandle.getInstance ().RoomBackResponse -= onRoomBackResponse;
	}

	private void onLoginCallBack(ClientResponse response){
		watingPanel.SetActive(false);

		SoundCtrl.getInstance ().playBGM ();
		if (response.status == 1) {
			
			GlobalData.myAvatarVO = JsonMapper.ToObject<AvatarVO> (response.message);
			ChatSocket.getInstance ().sendMsg (new LoginChatRequest (GlobalData.myAvatarVO.account.uuid));

			SceneManager.getInstance ().changeToScene (SceneType.HOME);


		} else {
			TipsManager.getInstance ().setTips (response.message);
		}
	}
	private void onRoomBackResponse(ClientResponse response){

		watingPanel.SetActive(false);


		GlobalData.reEnterRoomData = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);

		for (int i = 0; i < GlobalData.reEnterRoomData.playerList.Count; i++) {
			AvatarVO itemData =	GlobalData.reEnterRoomData.playerList [i];
			if (itemData.account.openid == GlobalData.myAvatarVO.account.openid) {
				GlobalData.myAvatarVO.account.uuid = itemData.account.uuid;
				ChatSocket.getInstance ().sendMsg (new LoginChatRequest(GlobalData.myAvatarVO.account.uuid));
				break;
			}
		}

		SceneManager.getInstance ().changeToScene (SceneType.GAME);
	
	}


}
