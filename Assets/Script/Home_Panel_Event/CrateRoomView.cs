using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine.SceneManagement;


public class CrateRoomView : MonoBehaviour {
	
	public GameObject panelZhuanzhuanSetting;
	public GameObject panelJiPingHuSetting;
	public GameObject panelHuashuiSetting;
	public GameObject panelDevoloping;

	public List<Toggle> zhuanzhuanRoomCards;//转转麻将房卡数
	public List<Toggle> jiPingHuRoomCards;//鸡平胡房卡数
	public List<Toggle> huashuiRoomCards;//划水麻将房卡数

	public List<Toggle> zhuanzhuanGameRule;//转转麻将玩法
	public List<Toggle> jiPingHuGameRule;//鸡平胡玩法
	public List<Toggle> huashuiGameRule;//划水麻将玩法

	public List<Toggle> zhuanzhuanZhuama;//转转麻将抓码个数
	public List<Toggle> jiPingHuZhuama;//鸡平胡将抓码个数
	public List<Toggle> huashuixiayu;//划水麻将下鱼条数


	private int roomCardCount;//房卡数
	private GameObject gameSence;
	private RoomCreateVo sendVo;//创建房间的信息


	public GameObject giPingHuBt;
	public GameObject zzBt;
	public GameObject huaShuiBt;
	public GameObject closeBt;
	void Start () {
		switchSetting (GameType.GI_PING_HU);
		SocketEventHandle.getInstance ().CreateRoomCallBack += onCreateRoomCallback;

		giPingHuBt.GetComponent<Button> ().onClick.AddListener (onClickBtn_GiPingHu);
		zzBt.GetComponent<Button> ().onClick.AddListener (onClickBtn_ZhuanZhuan);
		huaShuiBt.GetComponent<Button> ().onClick.AddListener (onClickBtn_Hua_Shui);
		closeBt.GetComponent<Button> ().onClick.AddListener (onClickBtn_Close);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void onClickBtn_GiPingHu(){
		switchSetting (GameType.GI_PING_HU);
	}
	private void onClickBtn_ZhuanZhuan(){
		switchSetting (GameType.ZHUAN_ZHUAN);
	}

	private void onClickBtn_Hua_Shui(){
		switchSetting (GameType.HUA_SHUI);
	}
	private void switchSetting(GameType type){
		panelZhuanzhuanSetting.SetActive (false);
		panelJiPingHuSetting.SetActive (false);
		panelHuashuiSetting.SetActive (false);
		panelDevoloping.SetActive (false);
		switch (type) {
		case GameType.GI_PING_HU:
			panelJiPingHuSetting.SetActive (true);
			break;
		case GameType.ZHUAN_ZHUAN:
			panelZhuanzhuanSetting.SetActive (true);
			break;
		case GameType.HUA_SHUI:
			panelHuashuiSetting.SetActive (true);
			break;
		default:
			panelDevoloping.SetActive (true);
			break;
		}

	}

	private void onClickBtn_Close(){
		SocketEventHandle.getInstance ().CreateRoomCallBack -= onCreateRoomCallback;
		Destroy (this);
		Destroy (gameObject);
	}

	/**
	 * 创建鸡平胡房间
	 */
	public void createJiPingHuRoom(){

		int roomCardNum = costRoomCardNum (huashuiRoomCards);

		//抓码
		int maCount = 0;
		for (int i = 0; i <jiPingHuZhuama.Count; i++) {
			if (jiPingHuZhuama [i].isOn) {
				maCount = (int)Math.Pow(2,i);
				break;
			}
		}

		sendVo = new RoomCreateVo ();
		sendVo.roomType = (int) GameType.GI_PING_HU;

		sendVo.magnification = maCount;

		createRoom (sendVo,roomCardNum);

	}

	/**
	 * 创建转转麻将房间
	 */ 
	public void createZhuanzhuanRoom(){
		
		int roomCardNum = costRoomCardNum (huashuiRoomCards);

		bool isZimo=false;//自摸
		if (zhuanzhuanGameRule [0].isOn) {
			isZimo = true;
		}
		
		bool hasHong=false;//红中赖子
		if (zhuanzhuanGameRule [2].isOn) {
			hasHong = true;
		}

		bool isSevenDoube =false;//七小对
		if (zhuanzhuanGameRule [3].isOn) {
			isSevenDoube = true;
		}

		
		int maCount = 0;
		for (int i = 0; i < zhuanzhuanZhuama.Count; i++) {
			if (zhuanzhuanZhuama [i].isOn) {
				maCount = 2 * (i + 1);
				break;
			}
		}
		sendVo = new RoomCreateVo ();
		sendVo.roomType = (int) GameType.ZHUAN_ZHUAN;

		sendVo.ma = maCount;
		sendVo.ziMo = isZimo?1:0;
		sendVo.hong = hasHong;
		sendVo.sevenDouble = isSevenDoube;

		createRoom (sendVo,roomCardNum);

	}

	/**
	 * 创建划水麻将房间
	 */
	public void createHuashuiRoom(){
		int roomCardNum = costRoomCardNum (huashuiRoomCards);

		bool isFengpai =false;//七小对
		if (huashuiGameRule [0].isOn) {
			isFengpai = true;
		}

		bool isZimo=false;//自摸
		if (huashuiGameRule [1].isOn) {
			isZimo = true;
		}
	

		int maCount = 0;
		for (int i = 0; i <huashuixiayu.Count; i++) {
			if (huashuixiayu [i].isOn) {
				maCount = 2 * (i + 1);
				break;
			}
		}

		sendVo = new RoomCreateVo ();
		sendVo.roomType = (int) GameType.HUA_SHUI;

		sendVo.xiaYu = maCount;
		sendVo.ziMo = isZimo?1:0;
		sendVo.addWordCard = isFengpai;
		sendVo.sevenDouble = true;

		createRoom (sendVo,roomCardNum);
	}
	private int costRoomCardNum(List<Toggle> list){
		for (int i = 0; i < list.Count; i++) {
			Toggle item = list [i];
			if (item.isOn) {
				return i + 1;
			}
		}
		return 1;
	}
	private void createRoom(RoomCreateVo roomVO,int roomCardNum){
		if (GlobalDataScript.loginResponseData.account.roomcard >= roomCardNum) {
			sendVo.roundNumber = roomCardNum * 8;
			string sendmsgstr = JsonMapper.ToJson (roomVO);
			CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (sendmsgstr));
		} else {
			TipsManagerScript.getInstance ().setTips ("你的房卡数量不足，不能创建房间");
		}
	}
	public void onCreateRoomCallback(ClientResponse response){
		MyDebug.Log (response.message);
		if (response.status == 1) {
			
			int roomid = Int32.Parse(response.message);
			sendVo.roomId = roomid;
			GlobalDataScript.roomVo = sendVo;
			GlobalDataScript.loginResponseData.roomId = roomid;
			//GlobalDataScript.loginResponseData.isReady = true;
			GlobalDataScript.loginResponseData.main = true;
			GlobalDataScript.loginResponseData.isOnLine = true;

			//SceneManager.LoadSceneAsync(1);

			GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePlay");

			GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript> ().createRoomAddAvatarVO (GlobalDataScript.loginResponseData);
		
			onClickBtn_Close ();

		} else {
			TipsManagerScript.getInstance ().setTips (response.message);
		}
	}

}
