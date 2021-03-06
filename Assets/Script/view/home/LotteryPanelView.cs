﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;
using System;

using LitJson;

using cn.sharesdk.unity3d;


public class LotteryPanelView : MonoBehaviour
{
	public  Text curTime;
	// Use this for initialization
	bool action = false;
	bool callBack = false;
	private int x = 0;
	public Text choujiangNum;
	private int prizecount;
	float end = 270;
	public Image turnImage;
	public float smoothing;
	// public float endFlag=20;
	public List<LotteryCRView> lotteryItems;
	public Text Congratulations;
	public int StopIndex = 1;


	public GameObject rulePanel;
//活动说明对话框
	public Text ruleText;
//活动说明文字

	private GiftList list;

	void Start ()
	{
		GlobalData.getInstance ().prizeCountChange += prizeCountChange;
		choujiangNum.text = GlobalData.getInstance ().myAvatarVO.account.prizecount + "";

		GameManager.getInstance ().Server.onResponse += onResponse;
		GameManager.getInstance ().Server.requset (APIS.LOTTERY_REQUEST, "0");
	}

	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.LOTTERY_RESPONSE://奖品
			giftResponse (response);
			break;
		}
	}

	private void prizeCountChange ()
	{
		TipsManager.getInstance ().setTips ("您的抽奖次数已经更新");
		choujiangNum.text = GlobalData.getInstance ().myAvatarVO.account.prizecount + "";
	}

	public class Drawl
	{
		public  string type;
		public int data;
	}


	// Update is called once per frame
	void Update ()
	{
		curTime.text = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm");


	}

	void FixedUpdate ()
	{
		if (action) {
			int b = 0;
			if (callBack) {
				x -= 30;
				int a = x / -360;
				b = 22 - a;
			} else {
				b = 22;
			}
			if (b > 2) {
				turnImage.transform.Rotate (new Vector3 (0, 0, -end), b);
			} else {
				float result = Math.Abs (turnImage.transform.localRotation.eulerAngles.z - end);
				if (result < 2) {
					action = false;
					callBack = false;
					end = 0;
				} else {
					//float lerp=Mathf.Lerp(b, 0, Time.deltaTime*smoothing);
					turnImage.transform.Rotate (new Vector3 (0, 0, -end), 2f);
					Invoke ("turnOver", 3);
					//turnOver();
				}
			}
		}
	}

	public void turnOver ()
	{
		Congratulations.text = "恭喜你！你抽到了" + lotteryItems [StopIndex - 1].nameTxt.text;
	}

	public void giftResponse (ClientResponse response)
	{
		callBack = true;
		JsonData data = JsonMapper.ToObject<JsonData> (response.message);
		if (int.Parse (data ["type"].ToString ()) == 2) {
			TipsManager.getInstance ().setTips ("抽奖活动暂时没有开放，3秒后将关闭对话框");
			Invoke ("closeDialog", 3f);
		} else {
			try { 
				list = JsonMapper.ToObject<GiftList> (response.message);
				if (list.type == "0") {
					for (int i = 0; i < list.data.Count; i++) {
						GiftItemVO gvo = list.data [i];
						lotteryItems [i].setData (gvo);
					}

				}
			} catch (Exception e) {
				Debug.Log (e.ToString ());
				if (GlobalData.getInstance ().myAvatarVO.account.prizecount > 0) {
					GlobalData.getInstance ().myAvatarVO.account.prizecount--;
					choujiangNum.text = GlobalData.getInstance ().myAvatarVO.account.prizecount + "";
				}
				Drawl returndata = JsonMapper.ToObject<Drawl> (response.message);
				StopIndex = returndata.data;
				MyDebug.Log ("StopIndex" + StopIndex);
				if (action == false) {
					float a = UnityEngine.Random.Range (-2, 2f);
					end = Math.Abs (StopIndex * 36 - 34 + a);
					MyDebug.Log ("end = " + end);
					x = 0;
					action = true;
					callBack = true;
				}
			}
		}



	

	}

	public void shareToWeChat ()
	{
		GameManager.getInstance ().WechatAPI.shareAchievementToWeChat (PlatformType.WeChatMoments);
	}

	/***
	 *关闭对话框 
	 */
	public void closeDialog ()
	{
		Destroy (gameObject);
	}


	public void startTurn ()
	{
		if (GlobalData.getInstance ().myAvatarVO.account.prizecount > 0) {
			GameManager.getInstance ().Server.requset (APIS.LOTTERY_REQUEST, "1");
		} else {
			TipsManager.getInstance ().setTips ("对不起，抽奖次数不足");
		}
	}

	/*显示活动规则*/
	public void showRule ()
	{
		rulePanel.SetActive (true);
	}

	/*关闭规则对话框*/
	public void closeRule ()
	{
		rulePanel.SetActive (false);
	}

	void OnDestroy ()
	{
		GameManager.getInstance ().Server.onResponse -= onResponse;
	}
}
