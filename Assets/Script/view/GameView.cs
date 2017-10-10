using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;
using LitJson;

namespace AssemblyCSharp
{

	public class GameView : MonoBehaviour ,ISceneView
	{
		public List<Transform> PGCParents;
		public List<Transform> HandParents;
		public List<Transform> TableParents;
		public List<PlayerItemView> PlayerItemViews;

		public List<AvatarVO> avatarList;


		//======================================


		/**抓码动态面板**/
		private GameObject zhuamaPanel;



		private bool isFirstOpen = true;

		private UIHelper _uiHelper;

		public UIHelper UIHelper {
			get {
				return	_uiHelper;
			}
		}
		private ActionEffectHelper _actionHlpr;
		private TableView _tableView;

		private GamingData _data;

		public GamingData Data {
			get {
				return	_data;
			}
		}

		#region ISceneView implementation

		public void open (object data = null)
		{
			GlobalData.getInstance ().gamingData = _data;
		}

		public void close (object data = null)
		{
			GlobalData.getInstance ().myAvatarVO.resetData ();//复位房间数据
			GlobalData.getInstance ().roomVO.roomId = 0;
			GlobalData.getInstance().SoundToggle = true;
			_uiHelper.cleanCardInTable ();

			SoundManager.getInstance ().playBGM ();


			for (int i = 0; i < 4; i++) {
				PlayerItemViews [i].destroy_all ();
				Destroy (PGCParents[i].gameObject);
				Destroy (HandParents[i].gameObject);
				Destroy (TableParents[i].gameObject);

			}
			PlayerItemViews.Clear ();
			PGCParents.Clear ();
			HandParents.Clear ();
			TableParents.Clear ();


			if (zhuamaPanel != null) {
				Destroy (zhuamaPanel.GetComponent<ZhuMaScript> ());
				Destroy (zhuamaPanel);
			}

			Destroy (this);
			Destroy (gameObject);
		}

		#endregion

		public GameView ()
		{
			_uiHelper = new UIHelper ();
			_data = new GamingData ();
		}

		void Start ()
		{
			
			SoundManager.getInstance ().stopBGM ();
			//===========================================================================================

			//===========================================================================================
			_actionHlpr = gameObject.GetComponent<ActionEffectHelper> ();
			_tableView = gameObject.GetComponent<TableView> ();
			_actionHlpr.addListener (this);

			_uiHelper.init (this);

			gameObject.GetComponentInChildren<TRMenuBarView> ().init (this);
			gameObject.GetComponentInChildren<QuickMsgView> ().init (this);
			gameObject.GetComponentInChildren<MicrophoneView> ().init (this);

			GameManager.getInstance ().Server.onResponse += onResponse;

			cleanTable ();
			_tableView.setTableVisible (false);
			try {
				if (_data.isReEnter) {
					_data.isReEnter = false;

					reEnterRoom ();
				
				} else {
					joinToRoom ();
				}
			} catch (Exception e) {
				Debug.Log (e.ToString ());
			}
		}

		void cleanTable ()
		{
			_uiHelper.cleanCardInTable ();
			_actionHlpr.cleanBtnShow ();
		}

		void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.JOIN_ROOM_NOTIFY:
				onOtherJointRoom (response);
				break;
			case APIS.START_GAME_NOTIFY://开始游戏
				onStartGame (response);
				break;
			case APIS.MY_PICK_NOTIFY://自己摸牌
				onMyPickCard (response);
				break;
			case APIS.OTHER_PICK_NOTIFY://别人摸牌通知
				onOtherPickCard (response);
				break;
			case APIS.OTHER_PUTOUT_NOTIFY://出牌通知
				onOtherPutOutCard (response);
				break;
			case APIS.PENGPAI_NOTIFY://碰牌回调
				onPengNotify (response);
				break;
			case APIS.CHIPAI_NOTIFY://吃牌回调
				onChiNotify (response);
				break;
			case APIS.GANGPAI_RESPONSE://杠牌回调
				onMyGangResponse (response);
				break;
			case APIS.OTHER_GANGPAI_NOTIFY:
				onOtherGangNotify (response);
				break;
			case APIS.ACTION_BUTTON_NOTIFY://碰杠行为按钮显示
				onActionBtnNotify (response);
				break;
			case APIS.HUPAI_RESPONSE://胡牌回调
				onHuNotify (response);
				break;
			case APIS.OUT_ROOM_RESPONSE://退出房间回调
				outRoomCallbak (response);
				break;
			case APIS.PrepareGame_MSG_RESPONSE://准备游戏通知返回
				gameReadyNotice (response);
				break;
			case APIS.OFFLINE_NOTICE://离线通知
				offlineNotice (response);
				break;
			case APIS.ONLINE_NOTICE://上线通知
				onlineNotice (response);
				break;
			case APIS.CURRENT_STATUS_RESPONSE:
				onCurStatusResponse (response);
				break;
			case APIS.Game_FollowBander_Notice://跟庄
				_actionHlpr.showEffect (ActionType.GEN_ZHUANG);
				break;
			default:
				return;	

			}

		}


		public void onStartGame (ClientResponse response)
		{
			StartGameVO svo = JsonMapper.ToObject<StartGameVO> (response.message);
			_data.bankerIndex = svo.bankerId;
			GlobalData.getInstance ().gameStatus = GameStatus.GAMING;
			cleanGameplayUI ();//开始游戏后不显示
			MyDebug.Log ("startGame");
			GlobalData.getInstance ().remainRoundCount--;

			_tableView.remainRoundText.text = GlobalData.getInstance ().remainRoundCount + "";//刷新剩余圈数
			if (!isFirstOpen) {
				isFirstOpen = false;
				cleanTable ();
				avatarList [_data.bankerIndex].main = true;
			}


			GlobalData.isOverByPlayer = false;
			MyHandCardView.isPutout = _data.bankerIndex == _data.myIndex;


			_uiHelper.getBankerCGO ().PlayerItem.setBankerIconVisible (true);
			_data.pickIndex = _data.bankerIndex;
			_data.paiArray = svo.paiArray;

			_tableView.resetTimer ();
			_uiHelper.hideReadyIcon ();
			//创建手牌
			_uiHelper.addMyHandCards (_data.paiArray [0]);
			_uiHelper.addOtherHandCards (Direction.L, 13);
			_uiHelper.addOtherHandCards (Direction.R, 13);
			_uiHelper.addOtherHandCards (Direction.T, 13);
			//突出摸到的牌
			if (_data.bankerIndex == _data.myIndex) {
				_uiHelper.rangeMyHandCard (true);//设置位置
			} else {
				_uiHelper.rangeMyHandCard (false);
				_uiHelper.addPickCard (_data.bankerIndex);
			}
			//
			_uiHelper.initRemainCardNum ();

		}

		private void cleanGameplayUI ()
		{
			_actionHlpr.liujuEffect.SetActive (false);
			_tableView.setTableVisible (true);
		}

		/// <summary>
		/// 别人摸牌通知
		/// </summary>
		/// <param name="response">Response.</param>
		public void onOtherPickCard (ClientResponse response)
		{
			_tableView.resetTimer ();
			JsonData json = JsonMapper.ToObject (response.message);
			_data.pickIndex = (int)json ["avatarIndex"];
			_uiHelper.addPickCard (_data.pickIndex);
			_uiHelper.updateRemainCardNum ();
		}


		/// <summary>
		/// 自己摸牌
		/// </summary>
		/// <param name="response">Response.</param>
		public void onMyPickCard (ClientResponse response)
		{

			_tableView.resetTimer ();
			CardVO cardvo = JsonMapper.ToObject<CardVO> (response.message);
			int cardPoint = cardvo.cardPoint;
			_data.pickIndex = _data.myIndex;
			_data.pickPoint = cardPoint;
			_data.paiArray [0] [cardPoint]++;
			_uiHelper.addPickCard (_data.pickIndex, _data.pickPoint);
			_uiHelper.updateRemainCardNum ();
			MyHandCardView.isPutout = true;

			MyDebug.Log ("摸牌:" + cardPoint);
		}

		/// <summary>
		/// 胡，杠，碰，吃，pass按钮显示.
		/// </summary>
		/// <param name="response">Response.</param>
		public void onActionBtnNotify (ClientResponse response)
		{
			string[] strs = response.message.Split (new char[1]{ ',' });


			for (int i = 0; i < strs.Length; i++) {
				if (strs [i].Equals ("hu")) {
					_actionHlpr.showBtn (ActionType.HU);

				} else if (strs [i].Contains ("qianghu")) {

					_data.pgcCardPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [1]);

					_actionHlpr.showBtn (ActionType.HU);
					_data.isQiangHu = true;
				} else if (strs [i].Contains ("peng")) {
					_actionHlpr.showBtn (ActionType.PENG);
					//_data.putoutPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [2]); //可以不用管


				} else if (strs [i].Contains ("chi")) {
					_actionHlpr.showBtn (ActionType.CHI);
					//_data.putoutPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [2]); //可以不用管
					_uiHelper.showChiCard ();
				}
				if (strs [i].Contains ("gang")) {
				
					_actionHlpr.showBtn (ActionType.GANG);
					string[] gangPaiList = strs [i].Split (new char[1]{ ':' });
					List<string> gangPaiListTemp = gangPaiList.ToList ();
					gangPaiListTemp.RemoveAt (0);

					_data.gangPaiList = gangPaiListTemp.ToArray ();
				}
			}
		}


		public void onOtherPutOutCard (ClientResponse response)
		{
		
			JsonData json = JsonMapper.ToObject (response.message);
			_data.putoutPoint = (int)json ["cardIndex"];
			_data.putoutIndex = (int)json ["curAvatarIndex"];
			_uiHelper.removePickCard ();
			createPutOutCardAndPlayAction (_data.putoutPoint, _data.putoutIndex);
		}

		private void createPutOutCardAndPlayAction (int cardPoint, int avatarIndex)
		{
			SoundManager.getInstance ().playSound (_data.putoutPoint, avatarList [_data.putoutIndex].account.sex);

			_uiHelper.addPutoutCardEffect (_data.putoutIndex, _data.putoutPoint);

			_uiHelper.lastCardOnTable = _uiHelper.addCardToTable (_data.putoutIndex, _data.putoutPoint);
			_uiHelper.addPointer (_uiHelper.lastCardOnTable);
		}


		private void onChiNotify (ClientResponse response)//吃牌处理
		{
			_tableView.resetTimer ();
			ChiPaiNotifyVO pvo = JsonMapper.ToObject<ChiPaiNotifyVO> (response.message);

			_data.pickIndex = pvo.avatarId;
			int cardPoint = Math.Min ( Math.Min (pvo.cardPoint, pvo.onePoint), pvo.twoPoint);
			_actionHlpr.showEffect (ActionType.CHI);
			SoundManager.getInstance ().playSoundByAction ("chi", avatarList [_data.pickIndex].account.sex);

			_uiHelper.removeLastPutoutCard ();


			if (_data.pickIndex == _data.myIndex) {  //自己吃牌
				_data.paiArray [0] [pvo.cardPoint]++;
//				_data.paiArray [1] [cardPoint] |= PaiArrayType.CHI;
//				_data.paiArray [1] [cardPoint+1] |= PaiArrayType.CHI;
//				_data.paiArray [1] [cardPoint+2] |= PaiArrayType.CHI;
				_uiHelper.removeMyHandCard (pvo.onePoint, 1);
				_uiHelper.removeMyHandCard (pvo.twoPoint, 1);
				_uiHelper.rangeMyHandCard (true);
				MyHandCardView.isPutout = true;

			} else {//其他人吃牌
				_uiHelper.removeOtherHandCard (_data.pickIndex, 2);
				_uiHelper.rangeOtherHandCard (_data.pickIndex, 1);
			}
			_uiHelper.addPGCCards (_data.pickIndex, cardPoint, 4);
		}
		private void onPengNotify (ClientResponse response)//碰牌处理
		{
			_tableView.resetTimer ();
			OtherPengGangBackVO pvo = JsonMapper.ToObject<OtherPengGangBackVO> (response.message);

			_data.pickIndex = pvo.avatarId;
			int cardPoint = pvo.cardPoint;
			_actionHlpr.showEffect (ActionType.PENG);
			SoundManager.getInstance ().playSoundByAction ("peng", avatarList [_data.pickIndex].account.sex);

			_uiHelper.removeLastPutoutCard ();


			if (_data.pickIndex == _data.myIndex) {  //自己碰牌
				_data.paiArray [0] [cardPoint]++;
//				_data.paiArray [1] [cardPoint] |= (int)PaiArrayType.PENG;
				_uiHelper.removeMyHandCard (cardPoint, 2);
				_uiHelper.rangeMyHandCard (true);
				MyHandCardView.isPutout = true;
		
			} else {//其他人碰牌
				_uiHelper.removeOtherHandCard (_data.pickIndex, 2);
				_uiHelper.rangeOtherHandCard (_data.pickIndex, 1);

			}
			_uiHelper.addPGCCards (_data.pickIndex, cardPoint, 1);
		}


		private void onOtherGangNotify (ClientResponse response) //其他人杠牌
		{

			GangNoticeVO gvo = JsonMapper.ToObject<GangNoticeVO> (response.message);
			int cardPoint = gvo.cardPoint;
			_data.pickIndex = gvo.avatarId;
			_actionHlpr.showEffect (ActionType.GANG);
			SoundManager.getInstance ().playSoundByAction ("gang", avatarList [_data.pickIndex].account.sex);

			if (_uiHelper.findIndexInPGC (_data.pickIndex, cardPoint) == -1) {//判断有没有碰过了，
				_uiHelper.removeOtherHandCard(_data.pickIndex,3);//消耗3只牌
				_uiHelper.rangeMyHandCard (false);
				_uiHelper.rangeOtherHandCard (_data.pickIndex, 2);

				if (gvo.type == 0) {//明杠别人
					_uiHelper.removeLastPutoutCard ();
					_uiHelper.addPGCCards (_data.pickIndex, cardPoint, 2);
				} else {//暗杠
					_uiHelper.removePickCard ();//消耗一只摸的牌
					_uiHelper.addPGCCards (_data.pickIndex, cardPoint, 3);//杠4只到桌面
				}

			} else {
				_uiHelper.removePickCard ();
				_uiHelper.addPGCCards (_data.pickIndex, cardPoint, 5);//明杠1只到桌面

			}

		}

		/**客户端主动打一只牌**/
		public void putoutCard (int cardPoint)//
		{
			_data.paiArray [0] [cardPoint]--;
			_data.putoutIndex = _data.myIndex;
			_data.putoutPoint = cardPoint;
			createPutOutCardAndPlayAction (_data.putoutPoint, _data.putoutIndex);
			//========================================================================
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = cardPoint;
			GameManager.getInstance ().Server.requset (new PutOutCardRequest (cardvo));
		}

		/// <summary>
		/// 点击放弃按钮
		/// </summary>
		public void myPassBtnClick ()
		{
			_actionHlpr.cleanBtnShow ();

			if (_data.pickIndex == _data.myIndex) {
				MyHandCardView.isPutout = true;
			}
			GameManager.getInstance ().Server.requset (new GaveUpRequest ());
		}

		public void myPengBtnClick ()
		{
			MyHandCardView.isPutout = true;
			_tableView.resetTimer ();
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = _data.putoutPoint;
			GameManager.getInstance ().Server.requset (new PengPaiRequest (cardvo));
			_actionHlpr.cleanBtnShow ();
		}

		public void myChiBtnClick ()
		{
			MyHandCardView.isPutout = true;
			MyHandCardView.isChi = false;
			_tableView.resetTimer ();
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = _data.putoutPoint;
			cardvo.onePoint = _data.chiPoints[0];
			cardvo.twoPoint = _data.chiPoints[1];
			GameManager.getInstance ().Server.requset (new ChiPaiRequest (cardvo));
			_actionHlpr.cleanBtnShow ();
		}


		public void onMyGangResponse (ClientResponse response)
		{
			_tableView.resetTimer ();
			GangBackVO gvo = JsonMapper.ToObject<GangBackVO> (response.message);
			int gangKind = gvo.type;
			if (gvo.cardList.Count == 0) {
				int cardPoint = _data.pgcCardPoint;
//				_data.paiArray[1][cardPoint] |= PaiArrayType.GANG;
				if (gangKind == 0) {//明杠
					
					/**杠牌点数**/
					if (_uiHelper.findIndexInPGC (_data.myIndex,cardPoint) == -1) {//杠别人
						_uiHelper.removeLastPutoutCard ();
						_uiHelper.removeMyHandCard (cardPoint, 3);
						_uiHelper.addPGCCards (_data.myIndex, cardPoint, 2);
					} else {//摸牌杠
						_uiHelper.removeMyHandCard(cardPoint,1);
						_uiHelper.addPGCCards(_data.myIndex,cardPoint,5);
					}

				} else if (gangKind == 1) { //暗杠

					_uiHelper.removeMyHandCard (cardPoint, 4);

					_uiHelper.addPGCCards(_data.myIndex,cardPoint,3);

				}
			}
			_uiHelper.rangeMyHandCard (false);
		}


		public void myGangBtnClick ()
		{
			//TODO 多张可以杠的牌
			_data.pgcCardPoint = int.Parse (_data.gangPaiList [0]);
			_data.gangPaiList = null;
			GameManager.getInstance ().Server.requset (new GangPaiRequest (_data.pgcCardPoint, 0));
			SoundManager.getInstance ().playSoundByAction ("gang", GlobalData.getInstance ().myAvatarVO.account.sex);
			_actionHlpr.cleanBtnShow ();
			_actionHlpr.showEffect (ActionType.GANG);

			MyHandCardView.isPutout = true;
		}

			

		/**进入房间**/
		public void joinToRoom ()
		{
			avatarList = GlobalData.getInstance ().playerList;
			_data.AvatarList = GlobalData.getInstance ().playerList;
			for (int i = 0; i < avatarList.Count; i++) {
				_uiHelper.getCardGOs (i).PlayerItem.setAvatarVo (avatarList [i]);
			}
			_tableView.updateRule ();
			GlobalData.getInstance ().gameStatus = GameStatus.READYING;
			GameManager.getInstance ().Server.requset (new GameReadyRequest ());
			markselfReadyGame ();
		}


		public void onOtherJointRoom (ClientResponse response)
		{
			AvatarVO avatar = JsonMapper.ToObject<AvatarVO> (response.message);
			_uiHelper.getCardGOs (avatarList.Count).PlayerItem.setAvatarVo (avatar);
			avatarList.Add (avatar);
		}


		/**
	 * 胡牌请求
	 */ 
		public void myHuBtnClick ()
		{
			if (_data.pgcCardPoint != -1) {
				CardVO requestVo = new CardVO ();

				if (_data.isQiangHu) {
					requestVo.type = "qianghu";
					_data.isQiangHu = false;
					requestVo.cardPoint = _data.pgcCardPoint;
				}
				string sendMsg = JsonMapper.ToJson (requestVo);
				GameManager.getInstance ().Server.requset (new HupaiRequest (sendMsg));
				_actionHlpr.cleanBtnShow ();
			}
		}



		/**
	 * 胡牌请求回调
	 */ 
		private void onHuNotify (ClientResponse response)
		{
			HupaiResponseVo hvo = JsonMapper.ToObject<HupaiResponseVo> (response.message);
			_data.hupaiResponseVO = hvo; 
			string scores = hvo.currentScore;
			hupaiCoinChange (scores);


			if (hvo.type == "0") {
				SoundManager.getInstance ().playSoundByAction ("hu", GlobalData.getInstance ().myAvatarVO.account.sex);
				_actionHlpr.showEffect (ActionType.HU);

				for (int i = 0; i < hvo.avatarList.Count; i++) {
					if (checkAvarHupai (hvo.avatarList [i]) == 1) {//胡
						_uiHelper.getCardGOs (i).PlayerItem.setHuFlagDisplay ();
						SoundManager.getInstance ().playSoundByAction ("hu", avatarList [i].account.sex);
					} else if (checkAvarHupai (hvo.avatarList [i]) == 2) {
						_uiHelper.getCardGOs (i).PlayerItem.setHuFlagDisplay ();
						SoundManager.getInstance ().playSoundByAction ("zimo", avatarList [i].account.sex);
					} else {
						_uiHelper.getCardGOs (i).PlayerItem.setHuFlagHidde ();
					}

				}

				string allMas = hvo.allMas;
				if (GlobalData.getInstance ().roomVO.roomType == GameType.ZHUAN_ZHUAN || GlobalData.getInstance ().roomVO.roomType == GameType.GI_PING_HU) {//只有转转麻将才显示抓码信息
					if (GlobalData.getInstance ().roomVO.ma > 0 && allMas != null && allMas.Length > 0) {
						zhuamaPanel = PrefabManage.loadPerfab ("prefab/Panel_ZhuaMa");
						zhuamaPanel.GetComponent<ZhuMaScript> ().init (_data);
						zhuamaPanel.GetComponent<ZhuMaScript> ().arrageMas (allMas, hvo.validMas);
						Invoke ("openGameOverView", 7);
					} else {
						Invoke ("openGameOverView", 3);
					}

				} else {
					Invoke ("openGameOverView", 3);
				}


			} else if (hvo.type == "1") {

				SoundManager.getInstance ().playSoundByAction ("liuju", GlobalData.getInstance ().myAvatarVO.account.sex);
				_actionHlpr.showEffect (ActionType.LIUJU);
				Invoke ("openGameOverView", 3);
			} else {
				Invoke ("openGameOverView", 3);
			}



		}

		/**
	 *检测某人是否胡牌 
	 */
		public int checkAvarHupai (HupaiResponseItem itemData)
		{
			string hupaiStr = itemData.totalInfo.hu;
			HuipaiObj hupaiObj = new HuipaiObj ();
			if (hupaiStr != null && hupaiStr.Length > 0) {
				hupaiObj.uuid = hupaiStr.Split (new char[1]{ ':' }) [0];
				hupaiObj.cardPiont = int.Parse (hupaiStr.Split (new char[1]{ ':' }) [1]);
				hupaiObj.type = hupaiStr.Split (new char[1]{ ':' }) [2];
				//增加判断是否是自己胡牌的判断

				if (hupaiStr.Contains ("d_other")) {//排除一炮多响的情况
					return 0;
				} else if (hupaiObj.type == "zi_common") {
					return 2;

				} else if (hupaiObj.type == "d_self") {
					return 1;
				} else if (hupaiObj.type == "qiyise") {
					return 1;
				} else if (hupaiObj.type == "zi_qingyise") {
					return 2;
				} else if (hupaiObj.type == "qixiaodui") {
					return 1;
				} else if (hupaiObj.type == "self_qixiaodui") {
					return 2;
				} else if (hupaiObj.type == "gangshangpao") {
					return 1;
				} else if (hupaiObj.type == "gangshanghua") {
					return 2;
				}


			}
			return 0;
		}




		private void hupaiCoinChange (string scores)
		{
			string[] scoreList = scores.Split (new char[1]{ ',' });
			if (scoreList != null && scoreList.Length > 0) {
				for (int i = 0; i < scoreList.Length - 1; i++) {
					string itemstr = scoreList [i];
					int uuid = int.Parse (itemstr.Split (new char[1]{ ':' }) [0]);
					int score = int.Parse (itemstr.Split (new char[1]{ ':' }) [1]) + 1000;
					int avatarIndex = _data.toAvatarIndex (uuid);
					_uiHelper.getCardGOs (avatarIndex).PlayerItem.scoreText.text = score + "";
					avatarList [avatarIndex].scores = score;
				}
			}

		}


		private void openGameOverView ()
		{//单局结算
			_actionHlpr.liujuEffect.SetActive (false);
			_uiHelper.hideHuIcon ();
			if (zhuamaPanel != null) {
				Destroy (zhuamaPanel);
			}

			avatarList [_data.bankerIndex].main = false;
			_uiHelper.getBankerCGO ().PlayerItem.setBankerIconVisible (false);

			cleanTable ();

			SceneManager.getInstance ().showGameOverView (0, _data);
		}



		/**退出房间请求**/
		public void quiteRoom ()
		{
			OutRoomRequestVo vo = new OutRoomRequestVo ();
			vo.roomId = GlobalData.getInstance ().roomVO.roomId;
			string sendMsg = JsonMapper.ToJson (vo);
			GameManager.getInstance ().Server.requset (new OutRoomRequest (sendMsg));
		}

		public void outRoomCallbak (ClientResponse response)
		{
			OutRoomResponseVo responseMsg = JsonMapper.ToObject<OutRoomResponseVo> (response.message);
			if (responseMsg.status_code == "0") {
				if (responseMsg.type == "0") {

					int uuid = responseMsg.uuid;
					if (uuid != GlobalData.getInstance ().myAvatarVO.account.uuid) {
						int index = _data.toAvatarIndex (uuid);
						avatarList.RemoveAt (index);
						_data.AvatarList = GlobalData.getInstance ().playerList;
						int i = 0;
						for (; i < avatarList.Count; i++) {
							var avo = avatarList [i];
							_uiHelper.getCardGOs (i).PlayerItem.setAvatarVo (avo);
						}
						for (; i < 4; i++) {
							_uiHelper.getCardGOs (i).PlayerItem.setAvatarVo (avatarList [i]);
						}
						markselfReadyGame ();
					} else {
						exitOrDissoliveRoom ();
					}

				} else {
					exitOrDissoliveRoom ();
				}

			} else {
				TipsManager.getInstance ().setTips ("退出房间失败：" + responseMsg.error);
			}
		}


		public void exitOrDissoliveRoom ()
		{
			SceneManager.getInstance ().changeToScene (SceneType.HOME);

		}

		public void gameReadyNotice (ClientResponse response)
		{

			//===============================================
			JsonData json = JsonMapper.ToObject (response.message);
			int avatarIndex = Int32.Parse (json ["avatarIndex"].ToString ());
			_uiHelper.getCardGOs (avatarIndex).PlayerItem.readyImg.enabled = true;
			avatarList [avatarIndex].isReady = true;
		}


		/*************************断线重连*********************************/
		private void reEnterRoom ()
		{
			_tableView.updateRule ();
			//设置座位

			avatarList = GlobalData.getInstance ().playerList;
			_data.AvatarList = GlobalData.getInstance ().playerList;
			for (int i = 0; i < avatarList.Count; i++) {
				_uiHelper.getCardGOs (i).PlayerItem.setAvatarVo (avatarList [i]);
			}


			if (_data.paiArray.Count == 0) {//游戏还没有开始
				GlobalData.getInstance ().gameStatus = GameStatus.READYING;

			} else {//牌局已开始
				GlobalData.getInstance ().gameStatus = GameStatus.GAMING;
				_uiHelper.hideReadyIcon ();
				cleanGameplayUI ();

				for (int i = 0; i < _data.AvatarList.Count; i++) {
					AvatarVO avo = avatarList [i];
					//HandCard
					_uiHelper.addOtherHandCards (i, avo.commonCards);
					//TableCard
					int[] chupai = avo.chupais;
					if (chupai != null && chupai.Length > 0) {
						for (int j = 0; j < chupai.Length; j++) {
							_uiHelper.addCardToTable (i, chupai [j]);
						}
					}
					//GangCard
					List<int> paiArrayType = _data.paiArray [1];
					if (checkPaiArrayContainType(PaiArrayType.GANG)) {
						string gangString = avo.huReturnObjectVO.totalInfo.gang;
						if (gangString != null) {
							string[] gangArray = gangString.Split (new char[1]{ ',' });
							for (int j = 0; j < gangArray.Length; j++) {
								string item = gangArray [j];
								string[] vals = item.Split (new char[1]{ ':' });
								GangpaiObj gangpaiObj = new GangpaiObj ();
								gangpaiObj.uuid = vals [0];
								gangpaiObj.cardPiont = int.Parse (vals [1]);
								gangpaiObj.type = vals [2];
								//增加判断是否为自己的杠牌的操作
								_data.paiArray [0] [gangpaiObj.cardPiont] -= 4;
								if (gangpaiObj.type == "an") {
									_uiHelper.addPGCCards (i, gangpaiObj.cardPiont, 3);

								} else {
									_uiHelper.addPGCCards (i, gangpaiObj.cardPiont, 2);

								}
							}
						}
					}
					//PengCard
					if (checkPaiArrayContainType(PaiArrayType.PENG)) {
						for (int j = 0; j < paiArrayType.Count; j++) {
							if (paiArrayType [j] == 1 && _data.paiArray [0] [j] > 0) {
								_data.paiArray [0] [j] -= 3;
								_uiHelper.addPGCCards (i, j, 1);
							}
						}
					}
					//ChiCard
					if (checkPaiArrayContainType(PaiArrayType.CHI)) {
						string chiString = avo.huReturnObjectVO.totalInfo.chi;
						if (chiString != null) {
							string[] chiArray = chiString.Split (new char[1]{ ',' });
							for (int j = 0; j < chiArray.Length; j++) {
								string item = chiArray [j];
								string[] vals = item.Split (new char[1]{ ':' });
								int a = int.Parse (vals [1]);
								int b = int.Parse (vals [2]);
								int c = int.Parse (vals [3]);

								//增加判断是否为自己的杠牌的操作
								_data.paiArray [0] [a] -= 1;
								_data.paiArray [0] [b] -= 1;
								_data.paiArray [0] [c] -= 1;
								_uiHelper.addPGCCards (i, a, 4);
							}
						}
					}
				}
				_uiHelper.addMyHandCards (_data.paiArray [0]);
				GameManager.getInstance ().Server.requset (new CurrentStatusRequest ());
			}

		} 
		private bool checkPaiArrayContainType(PaiArrayType type){
			List<int> paiArrayType = _data.paiArray [1];
			for (int i = 0; i < paiArrayType.Count; i++) {
				if ((paiArrayType [i] & (int)type) == (int)type) {
					return true;
				}
			}
			return false;
		}


		public void inviteFriend ()
		{
			GameManager.getInstance ().WechatAPI.inviteFriend ();
		}



		/**用户离线回调**/
		public void offlineNotice (ClientResponse response)
		{
			int uuid = int.Parse (response.message);
			int index = _data.toAvatarIndex (uuid);
			_uiHelper.getCardGOs (index).PlayerItem.setPlayerOffline ();
		}

		/**用户上线提醒**/
		public void onlineNotice (ClientResponse  response)
		{
			int uuid = int.Parse (response.message);
			var avatarIndex = _data.toAvatarIndex (uuid);
			_uiHelper.getCardGOs (avatarIndex).PlayerItem.setPlayerOnline ();

		}
			

		/*显示自己准备*/
		private void markselfReadyGame ()
		{
			_uiHelper.getCardGOs (Direction.B).PlayerItem.readyImg.transform.gameObject.SetActive (true);
		}



		public void onCurStatusResponse (ClientResponse response)
		{
			//1.显示剩余牌的张数和圈数
			JsonData msgData = JsonMapper.ToObject (response.message);
			string surplusCards = msgData ["surplusCards"].ToString ();
			_data.remainCardNum = int.Parse (surplusCards) + 1;
			_uiHelper.updateRemainCardNum ();
			int gameRound = int.Parse (msgData ["gameRound"].ToString ());
			_tableView.remainRoundText.text = gameRound + "";
			GlobalData.getInstance ().remainRoundCount = gameRound;

			try {
				_data.putoutIndex = int.Parse (msgData ["curAvatarIndex"].ToString ());
				_data.putoutPoint = int.Parse (msgData ["putOffCardPoint"].ToString ());

				_data.pickIndex = int.Parse (msgData ["pickAvatarIndex"].ToString ()); 
				if (msgData.Keys.Contains ("currentCardPoint")) {
					_data.pickPoint = int.Parse (msgData ["currentCardPoint"].ToString ());
				}

			} catch (Exception e) {
				Debug.Log (e.ToString ());
			}


			if (_data.pickIndex == _data.myIndex) {//把摸的牌放对位置
				var Hand = _uiHelper.getCardGOs (Direction.B).Hand;
				if (_data.pickPoint == -2) {
					var lastCard = Hand [Hand.Count - 1];
					_data.pickPoint = lastCard.GetComponent<MyHandCardView> ().getPoint ();
					_uiHelper.rangeMyHandCard (true);

					MyDebug.Log ("自己摸牌");

				} else {
					if ((Hand.Count) % 3 != 1) {
						MyDebug.Log ("摸牌" + _data.pickPoint);
						_uiHelper.removeMyHandCard (_data.pickPoint, 1);
						_uiHelper.rangeMyHandCard (false);
						_uiHelper.addPickCard (_data.myIndex, _data.pickPoint);
					}
				}
				MyHandCardView.isPutout = true;	 
			} else if (_data.pickIndex == _data.putoutIndex && _data.toGameDir(_data.putoutIndex) == Direction.L){//上家出牌，先试一下能不能吃
				if (_uiHelper.checkChi ()) {
					_actionHlpr.showBtn (ActionType.CHI);
					_uiHelper.showChiCard ();
				}
			}
			//光标指向打牌人

			var Table = _uiHelper.getCardGOs (_data.putoutIndex).Table;
			if (Table.Count == 0) {//刚启动

			} else {
				_uiHelper.lastCardOnTable = Table [Table.Count - 1];
				_uiHelper.addPointer (_uiHelper.lastCardOnTable);
			}
		}
		void OnDestroy(){
			GameManager.getInstance ().Server.onResponse -= onResponse;
		}
	}

}