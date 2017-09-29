using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;

public class HomeView : MonoBehaviour,ISceneView {
	public Image headIconImg;//头像路径
	private string headIcon;
	public Text nickNameText;//昵称
	public Text cardCountText;//房卡剩余数量
	public Text noticeText;

	public Text IpText;

	public Text contactInfoContent;


	public GameObject contactInfoPanel;
	WWW www;                     //请求

	private GameObject panelCreateDialog;//界面上打开的dialog
	/// <summary>
	/// 这个字段是作为消息显示的列表 ，如果要想通过管理后台随时修改通知信息，
	/// 请接收服务器的数据，并重新赋值给这个字段就行了。
	/// </summary>
	private bool startFlag = false;
	private int showNum = 0;

	#region ISceneView implementation
	public void open (object data = null)
	{

	}
	public void close (object data = null)
	{
		removeListener ();
	}
	#endregion		

	void Start () {
		initUI();
		checkEnterInRoom ();
		addListener ();
	}


	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Escape)){ //Android系统监听返回键，由于只有Android和ios系统所以无需对系统做判断
			MyDebug.Log ("Input.GetKey(KeyCode.Escape)");
			if(panelCreateDialog!=null){
				Destroy (panelCreateDialog);
			}
			else 
			{
				exitApp ();
			}
		}



	}

	//增加服务器返沪数据监听
	public void  addListener(){
		SocketEventHandle.getInstance ().cardChangeNotice += cardChangeNotice;
		SocketEventHandle.getInstance ().contactInfoResponse += contactInfoResponse;
			
	//	SocketEventHandle.getInstance ().gameBroadcastNotice += gameBroadcastNotice;
		CommonEvent.getInstance ().DisplayBroadcast += gameBroadcastNotice;
	}

	public void removeListener(){
		SocketEventHandle.getInstance ().cardChangeNotice -= cardChangeNotice;
		CommonEvent.getInstance ().DisplayBroadcast -= gameBroadcastNotice;
		SocketEventHandle.getInstance ().contactInfoResponse -= contactInfoResponse;
	//	SocketEventHandle.getInstance ().gameBroadcastNotice -= gameBroadcastNotice;
	}



	//房卡变化处理
	private void cardChangeNotice(ClientResponse response){
		cardCountText.text = response.message;
		GlobalData.myAvatarVO.account.roomcard =int.Parse(response.message);
	}

	private void gameBroadcastNotice(){
		showNum = 0;
		if(!startFlag){
			startFlag = true;
			setNoticeTextMessage ();
		}
	}
	void setNoticeTextMessage(){

		if (GlobalData.noticeMegs != null && GlobalData.noticeMegs.Count != 0) {
			noticeText.transform.localPosition = new Vector3 (500,noticeText.transform.localPosition.y);
			noticeText.text = GlobalData.noticeMegs [showNum];
			float time = noticeText.text.Length*0.5f+422f/56f;

			Tweener tweener=noticeText.transform.DOLocalMove(
				new Vector3(-noticeText.text.Length*28, noticeText.transform.localPosition.y), time)
				.OnComplete(moveCompleted);
			tweener.SetEase (Ease.Linear);
			//tweener.SetLoops(-1);
		}

	}

	void moveCompleted(){
		showNum++;
		if (showNum == GlobalData.noticeMegs.Count) {
			showNum = 0;
		}
		setNoticeTextMessage ();
	}
  
	/***
	 *初始化显示界面 
	 */
	private void initUI(){
		if (GlobalData.myAvatarVO != null) {
			headIcon = GlobalData.myAvatarVO.account.headicon;
			cardCountText.text = GlobalData.myAvatarVO.account.roomcard.ToString();
			nickNameText.text = GlobalData.myAvatarVO.account.nickname;
			IpText.text = "ID:" + GlobalData.myAvatarVO.account.uuid;
		}
        StartCoroutine (LoadImg());

	}

	public void showUserInfoPanel(){
		
		GameObject obj= PrefabManage.loadPerfab("Prefab/userInfo");
		obj.GetComponent<ShowUserInfoScript> ().setUIData (GlobalData.myAvatarVO);
	}


	public void showRoomCardPanel(){
		GameManager.getInstance().Server.requset (new GetContactInfoRequest ());

	}

	private void contactInfoResponse(ClientResponse response){
		contactInfoContent.text = response.message;
		contactInfoPanel.SetActive (true);
	}
	public void closeRoomCardPanel (){
		contactInfoPanel.SetActive (false);
	}

	/****
	 * 判断进入房间
	 */ 
	private void checkEnterInRoom(){
		if (GlobalData.roomVO!= null && GlobalData.roomVO.roomId != 0) {
			SceneManager.getInstance ().changeToScene (SceneType.GAME);
		}
	}


	/***
	 * 打开创建房间的对话框
	 * 
	 */ 
	public void openCreateRoomDialog(){
		if (GlobalData.myAvatarVO == null || GlobalData.myAvatarVO.roomId == 0) {
			loadPerfab ("Prefab/Panel_Create_Room_View");
		} else {
		
			TipsManager.getInstance ().setTips("当前正在房间状态，无法创建房间");
		}

	}

	/***
	 * 打开进入房间的对话框
	 * 
	 */ 
	public void openEnterRoomDialog(){
		
		if (GlobalData.roomVO == null || GlobalData.roomVO.roomId == 0) {
			loadPerfab ("Prefab/Panel_Enter_Room");

		} else {
			TipsManager.getInstance ().setTips("当前正在房间状态，无法加入新的房间");
		}
	}

	/**
	 * 打开游戏规则对话框
	 */ 
	public void onRuleBtnClick(){
		
		loadPerfab ("Prefab/Panel_Game_Rule_Dialog");
	}

	/**
	 * 打开排行榜
	 */ 
	public void onRankBtnClick(){
		loadPerfab ("Prefab/Panel_Rank_Dialog");
	}


	/**
	 * 打开抽奖对话框
	 * 
	*/
	public void onLotteryBtnClick()
	{
		loadPerfab ("Prefab/Panel_Lottery");
	}

    public void onScoreBtnClick()
    {
        loadPerfab("Prefab/Panel_Report");
    }

	private void  loadPerfab(string perfabName){
		panelCreateDialog = Instantiate (Resources.Load(perfabName)) as GameObject;
		panelCreateDialog.transform.parent = gameObject.transform;
		panelCreateDialog.transform.localScale = Vector3.one;
		//panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
		panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
	}


	private IEnumerator LoadImg() { 
		//开始下载图片
		if (headIcon != null && headIcon != "") {
			WWW www = new WWW(headIcon);
			yield return www;

			try {
				Texture2D texture2D = www.texture;
//				byte[] bytes = texture2D.EncodeToPNG();
				//将图片赋给场景上的Sprite
				headIconImg.sprite = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));

			} catch (Exception e){
				
				MyDebug.Log ("LoadImg"+e.Message);
			}
		}
	}



	public void exitApp(){
		SceneManager.getInstance ().showExitPanel ();
	}

}
