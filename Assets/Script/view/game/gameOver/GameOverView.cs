using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AssemblyCSharp;
using cn.sharesdk.unity3d;
using System.IO;
using System.Collections.Generic;

public class GameOverView : MonoBehaviour {
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

	/**分享单局结束战绩按钮**/
	public GameObject shareCurButton;

	/**继续游戏按钮**/
	public GameObject continueButton;

	/**分享全局结束战绩按钮**/
	public GameObject shareEndButton;

	public GameObject closeButton;

	public Text titleText;




	private List<int> mas_0;
	private List<int> mas_1;
	private List<int> mas_2;
	private List<int> mas_3;
	private  List<List<int>> allMasList;


	GamingData _data;
	FinalGameEndVo _fvo;
	HupaiResponseVo _hvo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/**
	 * 设置面板的显示内容
	 * dispalyFlag:0------>表示单据结束； 1--------->全局结束
		List<AvatarVO> personList,string allMas,List<int> validMas
	 */ 
	public void setDisplaContent(int dispalyFlag,GamingData data){

		_data = data;
		_hvo = data.hupaiResponseVO;
		_fvo = data.finalGameEndVo;
		timeText.text = DateTime.Now.ToString ("yyyy-MM-dd");
		roomText.text = "房间号：" + GlobalData.getInstance().roomVO.roomId;
		if (GlobalData.getInstance().roomVO.roomType == GameType.ZHUAN_ZHUAN) {//转转麻将
			titleText.text = "转转麻将";
		} else if (GlobalData.getInstance().roomVO.roomType == GameType.HUA_SHUI) {//划水麻将
			titleText.text = "划水麻将";
		} else if (GlobalData.getInstance().roomVO.roomType == GameType.GI_PING_HU) {
			titleText.text = "鸡平胡";
		}
		roundText.text = "局数：" + (GlobalData.getInstance().roomVO.roundNumber - GlobalData.getInstance().remainRoundCount) + "/" + GlobalData.getInstance().roomVO.roundNumber;
		if (dispalyFlag == 0) {
			allMasList = new List<List<int>> ();
			for (int i = 0; i < 4; i++) {
				allMasList.Add (new List<int> ());
			}

			curContainer.SetActive (true);
			endContainer.SetActive (false);
			continueButton.SetActive (true);
			shareEndButton.SetActive (false);
			closeButton.SetActive (false);
			if (GlobalData.getInstance().remainRoundCount == 0 || GlobalData.isOverByPlayer) {
				shareCurButton.GetComponent<Image> ().color =Color.white;
			} else {
				shareCurButton.GetComponent<Image> ().color = new Color32 (200, 200, 200, 128); 
			}

			getMas (_hvo.allMas);
			setCurentRoundContent ();
		} else if (dispalyFlag == 1) {
			curContainer.SetActive (false);
			endContainer.SetActive (true);
			shareCurButton.SetActive (false);
			continueButton.SetActive (false);
			shareEndButton.SetActive (true);
			closeButton.SetActive (true);
			setFinalScoreContent ();
		}


	}



	private void  getMas(string mas){
		List<string> paiList = new List<string> ();
		if (mas != null && mas != "") {
			string uuid = mas.Split (new char[1]{ ':' }) [0];
			string[] paiArray = mas.Split (new char[1]{ ':' });
			paiList = new List<string> (paiArray);
			paiList.RemoveAt (0);
			int referIndex = _data.toAvatarIndex (int.Parse (uuid));
//			List<int> temp = new List<int> (); 

			int resultIndex = 0;
			for (int i = 0; i < paiList.Count; i++) {
				int cardPoint = int.Parse (paiList [i]);
				int positionIndex = (cardPoint + 1) % 9;
//			string resultPonsition ="";
				if (cardPoint != 31) {
					switch (positionIndex) {
					case 1:
					case 5:
					case 0:
						resultIndex = referIndex;
						break;
					case 2:
					case 6:

						if ((referIndex + 1) == 4) {
							resultIndex = 0;

						} else {
							resultIndex = referIndex + 1;
						}
						break;
					case 4:
					case 8:
						if ((referIndex - 1) < 0) {
							resultIndex = 3;
						} else {
							resultIndex = referIndex - 1;
						}
						break;
					case 3:
					case 7:
						if ((referIndex + 2) == 4) {	
							resultIndex = 0;
						} else if ((referIndex + 2) > 4) {
							resultIndex = 1;
						} else if ((referIndex + 2) < 4) {
							resultIndex = referIndex + 2;	
						}
						break;	
					}
				} else {
					resultIndex = referIndex;
				}
				allMasList [resultIndex].Add (cardPoint);
			}
		}
	}


	private void setFinalScoreContent(){
		_fvo.totalInfo [0].setIsWiner (true);
		_fvo.totalInfo [0].setIsPaoshou (true);
		int topScore = _fvo.totalInfo [0].scores;
		int topPaoshou =  _fvo.totalInfo[0].dianpao;

		int uuid0= _fvo.totalInfo [0].uuid;
		int owerUuid = _fvo.theowner;

		Account account0 = _data.getAvatarVO(uuid0).account;


		string iconstr = account0.headicon;
		string nickName = account0.nickname;
		_fvo.totalInfo [0].setIcon (iconstr);
		_fvo.totalInfo [0].setNickname (nickName);
		if (owerUuid == uuid0) {
			_fvo.totalInfo [0].setIsMain (true);
		} else {
			_fvo.totalInfo [0].setIsMain (false);
		}
		int lastTopIndex = 0;
		int lastPaoshouIndex = 0;
		if (_fvo != null && _fvo.totalInfo.Count > 0) {
			
			for (int i = 1; i < _fvo.totalInfo.Count; i++) {
				if (topScore < _fvo.totalInfo [i].scores) {
					_fvo.totalInfo [lastTopIndex].setIsWiner (false);
					_fvo.totalInfo [i].setIsWiner (true);
					lastTopIndex = i;
					topScore = _fvo.totalInfo[i].scores;
				}
				if (topPaoshou < _fvo.totalInfo [i].dianpao && !_fvo.totalInfo [i].getIsWiner()) {
					topPaoshou =_fvo.totalInfo[i].dianpao;
					_fvo.totalInfo [i].setIsPaoshou (true);
					_fvo.totalInfo [lastPaoshouIndex].setIsPaoshou (false);
					lastPaoshouIndex = i;
				}
			

				int uuid = _fvo.totalInfo [i].uuid;
				Account account = _data.getAvatarVO (uuid).account;
				if (account != null) {
					_fvo.totalInfo [i].setIcon (account.headicon);
					_fvo.totalInfo [i].setNickname (account.nickname);

				}
				if (owerUuid == uuid) {
					_fvo.totalInfo [i].setIsMain (true);
				} else {
					_fvo.totalInfo [i].setIsMain (false);
				}


			}

			for (int i = 0; i < _fvo.totalInfo.Count; i++) {
				FinalGameEndItemVo itemdata = _fvo.totalInfo [i];
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_GEndCellRenderUI")) as GameObject;
				itemTemp.transform.parent = endContainer.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<GEndCellRenderView>().setUI(itemdata);
			}

		}

	

	}
	private void setCurentRoundContent(){


		if (_hvo != null && _hvo.avatarList.Count > 0) {
			for (int i = 0; i < _hvo.avatarList.Count; i++) {
				HupaiResponseItem itemdata = _hvo.avatarList [i];
				itemdata.setMaPoints (allMasList[i]);

				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_GCurCellRenderUI")) as GameObject;
				itemTemp.transform.parent = curContainer.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<GCurCellRenderView>().setUI(itemdata,_hvo.validMas,_data.AvatarList[_data.bankerIndex].account.uuid);
			}
		} 
	}

	public void reStratGame(){
		if (GlobalData.isOverByPlayer) {
			TipsManager.getInstance ().setTips ("房间已解散，不能重新开始游戏");
			return;
		}

		if (GlobalData.getInstance().remainRoundCount > 0) {
			GameManager.getInstance().Server.requset (new GameReadyRequest ());
			//TODO 这个后面要补一下 gameview.markselfReadyGame
			closeDialog ();

		} else {
			TipsManager.getInstance ().setTips ("游戏局数已经用完，无法重新开始游戏");
		}

	}


	public void closeDialog(){
		
		Destroy (gameObject);

	}

	public void doShare(){
		GameManager.getInstance().WechatAPI.shareAchievementToWeChat (PlatformType.WeChat);
	}

	public void openFinalOverPanl(){
		if ( _fvo !=null && _fvo.totalInfo !=null && _fvo.totalInfo.Count>0) {
			SceneManager.getInstance ().showGameOverView (1, _data);

			GlobalData.getInstance ().resetDataForNewRoom ();
			SceneManager.getInstance ().changeToScene (SceneType.HOME);

		}

	}


}
