using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AssemblyCSharp;
using cn.sharesdk.unity3d;
using System.IO;
using System.Collections.Generic;

public class GameOverScript : MonoBehaviour {
	/**时间显示条**/
	public Text timeText;

	/**房间号**/
	public Text roomNoText;

	/**局数**/
	public Text jushuText;

	/**标题显示**/
	//public Text TitleText;
  
	/***单局面板**/
	public GameObject signalEndPanel;

	/***全局面板**/
	public GameObject finalEndPanel;

	/**分享单局结束战绩按钮**/
	public GameObject shareSiganlButton;

	/**继续游戏按钮**/
	public GameObject continueGame;

	/**分享全局结束战绩按钮**/
	public GameObject shareFinalButton;

	public Button closeButton;

	public Text title;




	private List<AvatarVO> mAvatarvoList;
	private List<int> mas_0;
	private List<int> mas_1;
	private List<int> mas_2;
	private List<int> mas_3;
	private  List<List<int>> allMasList;
	private List<int> mValidMas;

	/**0表示打开单局结束模板，1表示全局结束模板**/
//	private int mDispalyFlag;

	private string picPath;//图片存储路径


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/**
	 * 设置面板的显示内容
	 * dispalyFlag:0------>表示单据结束； 1--------->全局结束
	 */ 
	public void setDisplaContent(int dispalyFlag,List<AvatarVO> personList,string allMas,List<int> validMas){
		mAvatarvoList = personList;
//		mDispalyFlag = dispalyFlag;
		mValidMas = validMas; 
		initRoomBaseInfo ();
		jushuText.text = "局数：" + (GlobalData.totalTimes - GlobalData.surplusTimes) + "/" + GlobalData.totalTimes;
		if (dispalyFlag == 0) {
			allMasList = new List<List<int>> ();
			mas_0 = new List<int> ();
			mas_1 = new List<int> ();
			mas_2 = new List<int> ();
			mas_3 = new List<int> ();
			signalEndPanel.SetActive (true);
			finalEndPanel.SetActive (false);
			continueGame.SetActive (true);
			shareFinalButton.SetActive (false);
			closeButton.transform.gameObject.SetActive (false);
			if (GlobalData.surplusTimes == 0 || GlobalData.isOverByPlayer) {
				shareSiganlButton.GetComponent<Image> ().color =Color.white;
			} else {
				shareSiganlButton.GetComponent<Image> ().color = new Color32 (200, 200, 200, 128); 
			}

			getMas (allMas);
			setSignalContent ();
		} else if (dispalyFlag == 1) {
			signalEndPanel.SetActive (false);
			finalEndPanel.SetActive (true);
			shareSiganlButton.SetActive (false);
			continueGame.SetActive (false);
			shareFinalButton.SetActive (true);
			closeButton.transform.gameObject.SetActive (true);
			setFinalContent ();
		}


	}



	private void  getMas(string mas){
		List<string> paiList = new List<string>();
		if (mas != null && mas!="") {
			string uuid = mas.Split (new char[1]{ ':' })[0];
			string[] paiArray = mas.Split (new char[1]{ ':' });
			paiList = new List<string> (paiArray);
			paiList.RemoveAt(0);
			int referIndex = getIndex (int.Parse(uuid));
//			List<int> temp = new List<int> (); 

		int resultIndex=0;
		for (int i = 0; i < paiList.Count; i++) {
			int cardPoint =int.Parse(paiList [i]);
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
			
			switch (resultIndex) {
			case 0:
				mas_0.Add (cardPoint);
				break;
			case 1:
				mas_1.Add (cardPoint);
				break;
			case 2:
				mas_2.Add (cardPoint);
				break;
			case 3:
				mas_3.Add (cardPoint);
				break;

			}
		}
		allMasList.Add (mas_0);
		allMasList.Add (mas_1);
		allMasList.Add (mas_2);
		allMasList.Add (mas_3);
		}
	}



	private int getIndex(int uuid){
		if (mAvatarvoList != null) {
			for (int i = 0; i < mAvatarvoList.Count; i++) {
				if (mAvatarvoList[i].account.uuid ==uuid) {
						return i;
					}
				}
			}
			return 0;
		}

	private void initRoomBaseInfo(){
		timeText.text=DateTime.Now.ToString("yyyy-MM-dd");
		roomNoText.text = "房间号：" + GlobalData.roomVO.roomId;
		if (GlobalData.roomVO.roomType == GameType.ZHUAN_ZHUAN) {//转转麻将
			title.text = "转转麻将";
		} else if (GlobalData.roomVO.roomType == GameType.HUA_SHUI) {//划水麻将
			title.text = "划水麻将";
		} else if (GlobalData.roomVO.roomType == GameType.GI_PING_HU) {
			title.text = "鸡平胡";
		}

		/**
		if (mDispalyFlag == 1) {
			TitleText.text = "棋牌结束";
		}
		*/

	}

	private Account getAcount(int uuid){
		if (mAvatarvoList != null && mAvatarvoList.Count > 0) {
			for (int i = 0; i < mAvatarvoList.Count; i++) {
				if (mAvatarvoList [i].account.uuid == uuid) {
					return mAvatarvoList [i].account;
				}
			}
		}
		return null;
	}

	/*
	private AvatarVO getAvatar(int uuid){
		if (mAvatarvoList != null && mAvatarvoList.Count > 0) {
			for (int i = 0; i < mAvatarvoList.Count; i++) {
				if (mAvatarvoList [i].account.uuid == uuid) {
					return mAvatarvoList [i];
				}
			}
		}
		return null;
	}
*/

	private void setFinalContent(){
		GlobalData.finalGameEndVo.totalInfo [0].setIsWiner (true);
		GlobalData.finalGameEndVo.totalInfo [0].setIsPaoshou (true);
		int topScore = GlobalData.finalGameEndVo.totalInfo [0].scores;
		int topPaoshou =  GlobalData.finalGameEndVo.totalInfo[0].dianpao;

		int uuid0= GlobalData.finalGameEndVo.totalInfo [0].uuid;
		int owerUuid = GlobalData.finalGameEndVo.theowner;

		Account account0 = getAcount (uuid0);

		//AvatarVO avatarVO0 = getAvatar (uuid0);

		string iconstr = account0.headicon;
		string nickName = account0.nickname;
		GlobalData.finalGameEndVo.totalInfo [0].setIcon (iconstr);
		GlobalData.finalGameEndVo.totalInfo [0].setNickname (nickName);
		if (owerUuid == uuid0) {
			GlobalData.finalGameEndVo.totalInfo [0].setIsMain (true);
		} else {
			GlobalData.finalGameEndVo.totalInfo [0].setIsMain (false);
		}
	//	GlobalDataScript.finalGameEndVo.totalInfo [0].setIsMain (avatarVO0.main);
		int lastTopIndex = 0;
		int lastPaoshouIndex = 0;
		if (GlobalData.finalGameEndVo != null && GlobalData.finalGameEndVo.totalInfo.Count > 0) {
			
			for (int i = 1; i < GlobalData.finalGameEndVo.totalInfo.Count; i++) {
				if (topScore < GlobalData.finalGameEndVo.totalInfo [i].scores) {
					GlobalData.finalGameEndVo.totalInfo [lastTopIndex].setIsWiner (false);
					GlobalData.finalGameEndVo.totalInfo [i].setIsWiner (true);
					lastTopIndex = i;
					topScore = GlobalData.finalGameEndVo.totalInfo[i].scores;
				}
				if (topPaoshou < GlobalData.finalGameEndVo.totalInfo [i].dianpao && !GlobalData.finalGameEndVo.totalInfo [i].getIsWiner()) {
					topPaoshou =GlobalData.finalGameEndVo.totalInfo[i].dianpao;
					GlobalData.finalGameEndVo.totalInfo [i].setIsPaoshou (true);
					GlobalData.finalGameEndVo.totalInfo [lastPaoshouIndex].setIsPaoshou (false);
					lastPaoshouIndex = i;
				}
			

				int uuid = GlobalData.finalGameEndVo.totalInfo [i].uuid;
				Account account = getAcount (uuid);
				if (account != null) {
					GlobalData.finalGameEndVo.totalInfo [i].setIcon (account.headicon);
					GlobalData.finalGameEndVo.totalInfo [i].setNickname (account.nickname);

				}
				if (owerUuid == uuid) {
					GlobalData.finalGameEndVo.totalInfo [i].setIsMain (true);
				} else {
					GlobalData.finalGameEndVo.totalInfo [i].setIsMain (false);
				}


			}

			for (int i = 0; i < GlobalData.finalGameEndVo.totalInfo.Count; i++) {
				FinalGameEndItemVo itemdata = GlobalData.finalGameEndVo.totalInfo [i];
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_Final_Item")) as GameObject;
				itemTemp.transform.parent = finalEndPanel.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<finalOverItem>().setUI(itemdata);
			}

		}

	

	}
	private void setSignalContent(){


		if (GlobalData.hupaiResponseVo != null && GlobalData.hupaiResponseVo.avatarList.Count > 0) {
			for (int i = 0; i < GlobalData.hupaiResponseVo.avatarList.Count; i++) {
				HupaiResponseItem itemdata = GlobalData.hupaiResponseVo.avatarList [i];
				if (allMasList != null && allMasList.Count != 0) {
					itemdata.setMaPoints (allMasList[i]);
				}
				GameObject itemTemp = Instantiate (Resources.Load("Prefab/Panel_Current_Item")) as GameObject;
				itemTemp.transform.parent = signalEndPanel.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.GetComponent<SignalOverItemScript>().setUI(itemdata,mValidMas,getMainuuid());
			}
		} 
	}

	private int getMainuuid (){
		for (int i = 0; i < mAvatarvoList.Count; i++) {
			if (mAvatarvoList [i].main) {
				return mAvatarvoList [i].account.uuid;
			}
		}
		return 0;
	}

	public void reStratGame(){
		if (GlobalData.isOverByPlayer) {
			TipsManager.getInstance ().setTips ("房间已解散，不能重新开始游戏");
			return;
		}

		if (GlobalData.surplusTimes > 0) {
			CustomSocket.getInstance ().sendMsg (new GameReadyRequest ());
			CommonEvent.getInstance ().readyGame ();
			closeDialog ();

		} else {
			TipsManager.getInstance ().setTips ("游戏局数已经用完，无法重新开始游戏");
		}

	}


	public void closeDialog(){
		GameOverScript self = GetComponent<GameOverScript> ();
		Destroy (self.continueGame);
		Destroy (self.finalEndPanel);
		Destroy (self.jushuText);
		Destroy (self.signalEndPanel);
		Destroy (self.finalEndPanel);
		Destroy (self.shareSiganlButton);
		Destroy (self.continueGame);
		Destroy (self.shareFinalButton);

		if (GlobalData.singalGameOverList!=null && GlobalData.singalGameOverList.Count > 0) {
			for (int i = 0; i < GlobalData.singalGameOverList.Count; i++) {
				//GlobalDataScript.singalGameOverList [i].GetComponent<GameOverScript> ().closeDialog ();
				Destroy(GlobalData.singalGameOverList[i].GetComponent<GameOverScript>());
				Destroy (GlobalData.singalGameOverList [i]);
			}
			int count = GlobalData.singalGameOverList.Count;
			for (int i = 0; i < count; i++) {
				GlobalData.singalGameOverList.RemoveAt (0);
			}
		}

		Destroy (this);
		Destroy (gameObject);

	}

	public void doShare(){
		GlobalData.getInstance ().wechatOperate.shareAchievementToWeChat (PlatformType.WeChat);
	}

	public void openFinalOverPanl(){
		if ( GlobalData.finalGameEndVo !=null && GlobalData.finalGameEndVo.totalInfo !=null && GlobalData.finalGameEndVo.totalInfo.Count>0) {
			GameObject obj = PrefabManage.loadPerfab ("prefab/Panel_Game_Over");
			obj.GetComponent<GameOverScript> ().setDisplaContent (1, GlobalData.roomAvatarVoList, null, GlobalData.hupaiResponseVo.validMas);
			obj.transform.SetSiblingIndex (2);

			if (GlobalData.singalGameOverList.Count > 0) {
				for (int i = 0; i < GlobalData.singalGameOverList.Count; i++)
				{
					//GlobalDataScript.singalGameOverList [i].GetComponent<GameOverScript> ().closeDialog ();
					Destroy(GlobalData.singalGameOverList[i].GetComponent<GameOverScript>());
					Destroy(GlobalData.singalGameOverList[i]);
				}
				//int count = GlobalDataScript.singalGameOverList.Count;
				//for (int i = 0; i < count; i++) {
				//	GlobalDataScript.singalGameOverList.RemoveAt (0);
				//}
				GlobalData.singalGameOverList.Clear();
			}
			if (CommonEvent.getInstance ().closeGamePanel != null) {
				CommonEvent.getInstance ().closeGamePanel ();
			}

		}

	

	}


}
