using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;

public class HomeView : MonoBehaviour,ISceneView
{
	public Image headIconImg;
	private string headIcon;
	public Text nickNameText;
	public Text cardCountText;
	public Text noticeText;

	public Text IpText;

	public Text contactInfoContent;


	public GameObject contactInfoPanel;

	private GameObject _dialog;

	private bool startFlag = false;
	private int showNum = 0;

	#region ISceneView implementation

	public void open (object data = null)
	{
		transform.SetSiblingIndex (0);
	}

	public void close (object data = null)
	{
		Destroy (gameObject);
	}

	#endregion

	void Start ()
	{
		initUI ();
		GameManager.getInstance ().Server.onResponse += onResponse;
		GlobalData.getInstance ().noticeChange += onNoticeChange;
	}


	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) { 
			if (_dialog != null) {
				Destroy (_dialog);
				_dialog = null;
			} else {
				exitApp ();
			}
		}

	}
	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.CARD_CHANGE_NOTIFY://房卡数据变化
			onCardChangeNotify (response);
			break;
		case APIS.CONTACT_INFO_RESPONSE://联系方式回调
			onContactInfoResponse (response);
			break;
		}
	}


	//房卡变化处理
	private void onCardChangeNotify (ClientResponse response)
	{
		cardCountText.text = response.message;
		GlobalData.getInstance ().myAvatarVO.account.roomcard = int.Parse (response.message);
	}

	private void onNoticeChange ()
	{
		showNum = 0;
		if (!startFlag) {
			startFlag = true;
			setNoticeTextMessage ();
		}
	}

	void setNoticeTextMessage ()
	{
		var noticeMsgs = GlobalData.getInstance ().NoticeMsgs;
		if (noticeMsgs.Count != 0) {
			noticeText.transform.localPosition = new Vector3 (500, noticeText.transform.localPosition.y);
			noticeText.text = noticeMsgs [showNum];
			float time = noticeText.text.Length * 0.5f + 422f / 56f;

			Tweener tweener = noticeText.transform.DOLocalMove (new Vector3 (-noticeText.text.Length * 28, noticeText.transform.localPosition.y), time);
			tweener.OnComplete (moveCompleted);
			tweener.SetEase (Ease.Linear);
			//tweener.SetLoops(-1);
		}

	}

	void moveCompleted ()
	{
		showNum++;
		if (showNum == GlobalData.getInstance ().NoticeMsgs.Count) {
			showNum = 0;
		}
		setNoticeTextMessage ();
	}
  
	/***
	 *初始化显示界面 
	 */
	private void initUI ()
	{
		var avo = GlobalData.getInstance ().myAvatarVO;
		if (avo != null) {
			headIcon = avo.account.headicon;
			cardCountText.text = avo.account.roomcard.ToString ();
			nickNameText.text = avo.account.nickname;
			IpText.text = "ID:" + avo.account.uuid;
		}
		StartCoroutine (LoadImg ());

	}

	public void showUserInfoPanel ()
	{
		SceneManager.getInstance ().showUserInfoPanel (GlobalData.getInstance ().myAvatarVO);
	}


	public void showContactInfoPanel ()
	{
		GameManager.getInstance ().Server.requset (APIS.CONTACT_INFO_REQUEST);

	}

	private void onContactInfoResponse (ClientResponse response)
	{
		contactInfoContent.text = response.message;
		contactInfoPanel.SetActive (true);
	}

	public void closeContactInfoPanel ()
	{
		contactInfoPanel.SetActive (false);
	}



	/***
	 * 打开创建房间的对话框
	 * 
	 */
	public void openCreateRoomDialog ()
	{
		if (GlobalData.getInstance ().roomVO.roomId == 0) {
			loadPerfab ("Prefab/home/Panel_CreateRoomUI");
		} else {
		
			TipsManager.getInstance ().setTips ("当前正在房间状态，无法创建房间");
		}

	}

	/***
	 * 打开进入房间的对话框
	 * 
	 */
	public void openEnterRoomDialog ()
	{
		
		if (GlobalData.getInstance ().roomVO == null || GlobalData.getInstance ().roomVO.roomId == 0) {
			loadPerfab ("Prefab/home/Panel_EnterRoomUI");

		} else {
			TipsManager.getInstance ().setTips ("当前正在房间状态，无法加入新的房间");
		}
	}

	/**
	 * 打开游戏规则对话框
	 */ 
	public void onRuleBtnClick ()
	{
		
		loadPerfab ("Prefab/home/Panel_RuleUI");
	}

	/**
	 * 打开排行榜
	 */ 
	public void onRankBtnClick ()
	{
		loadPerfab ("Prefab/home/Panel_RankUI");
	}


	/**
	 * 打开抽奖对话框
	 * 
	*/
	public void onLotteryBtnClick ()
	{
		loadPerfab ("Prefab/home/Panel_LotteryUI");
	}

	public void onRecordBtnClick ()
	{
		loadPerfab ("Prefab/home/Panel_RecordUI");
	}

	private void  loadPerfab (string perfabName)
	{
		if (_dialog != null) {
			Destroy (_dialog);
		}
		_dialog = Instantiate (Resources.Load (perfabName)) as GameObject;
		_dialog.transform.SetParent( gameObject.transform);
		_dialog.transform.localScale = Vector3.one;
		//panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
		_dialog.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
		_dialog.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
	}


	private IEnumerator LoadImg ()
	{ 
		if (!string.IsNullOrEmpty(headIcon)) {
			WWW www = new WWW (headIcon);
			yield return www;

			if(string.IsNullOrEmpty(www.error)) {
				Texture2D texture2D = www.texture;
				headIconImg.sprite = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));

			}
		}
	}



	public void exitApp ()
	{
		SceneManager.getInstance ().showExitPanel ();
	}
	void OnDestroy(){
		GameManager.getInstance ().Server.onResponse -= onResponse;
		GlobalData.getInstance ().noticeChange -= onNoticeChange;
	}
}
