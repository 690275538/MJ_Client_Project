using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AssemblyCSharp;
using cn.sharesdk.unity3d;
using System.IO;
using System.Collections.Generic;

public class GameOverView : MonoBehaviour
{
	/**时间显示条**/
	public Text timeText;

	/**房间号**/
	public Text roomText;

	/**局数**/
	public Text roundText;

	/***单局面板**/
	public GameObject curContainer;

	/***全局面板**/
	public GameObject endContainer;

	/**打开总全局面板**/
	public GameObject openFinalButton;

	/**继续游戏按钮**/
	public GameObject continueButton;

	/**分享全局结束战绩按钮**/
	public GameObject shareEndButton;

	public GameObject closeButton;

	public Text titleText;



	private  List<List<int>> allMasList;


	GamingData _data;
	FinalGameEndVo _fvo;
	HupaiResponseVo _hvo;


	/**
	 * 设置面板的显示内容
	 * dispalyFlag:0本回合结算，1全局结算
	 */ 
	public void setDisplaContent (int dispalyFlag, GamingData data)
	{

		_data = data;
		_hvo = data.hupaiResponseVO;
		_fvo = data.finalGameEndVo;
		timeText.text = DateTime.Now.ToString ("yyyy-MM-dd");
		roomText.text = "房间号：" + GlobalData.getInstance ().roomVO.roomId;
		if (GlobalData.getInstance ().roomVO.roomType == GameType.ZHUAN_ZHUAN) {//转转麻将
			titleText.text = "转转麻将";
		} else if (GlobalData.getInstance ().roomVO.roomType == GameType.HUA_SHUI) {//划水麻将
			titleText.text = "划水麻将";
		} else if (GlobalData.getInstance ().roomVO.roomType == GameType.GI_PING_HU) {
			titleText.text = "鸡平胡";
		}
		roundText.text = "局数：" + (GlobalData.getInstance ().roomVO.roundNumber - GlobalData.getInstance ().remainRoundCount) + "/" + GlobalData.getInstance ().roomVO.roundNumber;
		if (dispalyFlag == 0) {
			curContainer.SetActive (true);
			endContainer.SetActive (false);
			continueButton.SetActive (true);
			shareEndButton.SetActive (false);
			closeButton.SetActive (false);
			if (GlobalData.getInstance ().remainRoundCount == 0 || GlobalData.isOverByPlayer) {
				openFinalButton.GetComponent<Image> ().color = Color.white;
			} else {
				openFinalButton.GetComponent<Image> ().color = new Color32 (200, 200, 200, 128); 
			}

			getMas (_hvo.allMas);
			for (int i = 0; i < _hvo.avatarList.Count; i++) {
				HupaiResponseItem itemdata = _hvo.avatarList [i];
				itemdata.setMaPoints (allMasList [i]);

				GameObject cell = Instantiate (Resources.Load ("Prefab/Panel_GCurCellRenderUI")) as GameObject;
				cell.transform.SetParent (curContainer.transform);
				cell.transform.localScale = Vector3.one;
				cell.GetComponent<GCurCellRenderView> ().setUI (itemdata, _hvo.validMas, _data.BankerUuid);
			}
		} else if (dispalyFlag == 1) {
			curContainer.SetActive (false);
			endContainer.SetActive (true);
			openFinalButton.SetActive (false);
			continueButton.SetActive (false);
			shareEndButton.SetActive (true);
			closeButton.SetActive (true);
			setFinalScoreContent ();
		}


	}



	private void  getMas (string mas)
	{
		allMasList = new List<List<int>> ();
		for (int i = 0; i < 4; i++) {
			allMasList.Add (new List<int> ());
		}

		List<string> paiList = new List<string> ();
		if (!string.IsNullOrEmpty (mas)) {
			string[] paiArray = mas.Split (new char[1]{ ':' });
			string uuid = paiArray [0];
			paiList = new List<string> (paiArray);
			paiList.RemoveAt (0);
			int referIndex = _data.toAvatarIndex (int.Parse (uuid));

			int resultIndex = 0;
			for (int i = 0; i < paiList.Count; i++) {
				int cardPoint = int.Parse (paiList [i]);
				int positionIndex = (cardPoint + 1) % 9;

				if (cardPoint != 31) {
					switch (positionIndex) {
					case 1:
					case 5:
					case 0:
						resultIndex = referIndex;
						break;
					case 2:
					case 6:
						resultIndex = referIndex + 1;
						break;
					case 4:
					case 8:
						resultIndex = referIndex + 3;
						break;
					case 3:
					case 7:
						resultIndex = referIndex + 2;
						break;	
					}
				} else {
					resultIndex = referIndex;
				}
				resultIndex = resultIndex % 4;
				allMasList [resultIndex].Add (cardPoint);
			}
		}
	}


	private void setFinalScoreContent ()
	{
		if (_fvo != null && _fvo.totalInfo.Count > 0) {
			int owerUuid = _fvo.theowner;

			int lastTopIndex = 0;
			int lastPaoshouIndex = 0;

			int topScore = 0;
			int topPaoshou = 0;
			for (int i = 0; i < _fvo.totalInfo.Count; i++) {
				var item = _fvo.totalInfo [i];
				if (topScore < item.scores) {
					lastTopIndex = i;
					topScore = item.scores;
				}
				if (topPaoshou < item.dianpao && lastTopIndex != i) {
					topPaoshou = item.dianpao;
					lastPaoshouIndex = i;
				}
			
				int uuid = item.uuid;
				Account account = _data.getAvatarVO (uuid).account;
				if (account != null) {
					item.setIcon (account.headicon);
					item.setNickname (account.nickname);
				}
				item.setIsMain (owerUuid == uuid);

			}
			_fvo.totalInfo [lastTopIndex].setIsWiner (true);
			_fvo.totalInfo [lastPaoshouIndex].setIsPaoshou (true);


			for (int i = 0; i < _fvo.totalInfo.Count; i++) {
				GameObject cell = Instantiate (Resources.Load ("Prefab/Panel_GEndCellRenderUI")) as GameObject;
				cell.transform.parent = endContainer.transform;
				cell.transform.localScale = Vector3.one;
				cell.GetComponent<GEndCellRenderView> ().setUI (_fvo.totalInfo [i]);
			}

		}

	

	}

	public void reStratGame ()
	{
		if (GlobalData.isOverByPlayer) {
			TipsManager.getInstance ().setTips ("房间已解散，不能重新开始游戏");
			return;
		}

		if (GlobalData.getInstance ().remainRoundCount > 0) {
			GameManager.getInstance ().Server.requset (new GameReadyRequest ());
			//TODO 这个后面要补一下 gameview.markselfReadyGame
			closeDialog ();

		} else {
			TipsManager.getInstance ().setTips ("游戏局数已经用完，无法重新开始游戏");
		}

	}


	public void closeDialog ()
	{
		
		Destroy (gameObject);

	}

	public void doShare ()
	{
		GameManager.getInstance ().WechatAPI.shareAchievementToWeChat (PlatformType.WeChat);
	}

	public void openFinalOverPanl ()
	{
		if (_fvo != null && _fvo.totalInfo != null && _fvo.totalInfo.Count > 0) {
			SceneManager.getInstance ().showGameOverView (1, _data);

			GlobalData.getInstance ().resetDataForNewRoom ();
			SceneManager.getInstance ().changeToScene (SceneType.HOME);

		}

	}


}
