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
		public double lastTime;
		public Text Number;
		public Text roomRemark;
		public Image headIconImg;
		public int otherGangCard;
		public List<Transform> parentList;
		//过时
		public List<Transform> outparentList;
		//过时

		public List<Transform> PGCParents;
		public List<Transform> HandParents;
		public List<Transform> TableParents;
		public List<PlayerItemView> PlayerItemViews;

		public List<GameObject> dirGameList;
		public Text LeavedCastNumText;
		//剩余牌的张数
		public Text LeavedRoundNumText;
		//剩余局数
		//public int StartRoundNum;
		public Transform pengGangParenTransformB;
//过时
		public Transform pengGangParenTransformL;
//过时
		public Transform pengGangParenTransformR;
//过时
		public Transform pengGangParenTransformT;
//过时
		public List<AvatarVO> avatarList;
		public Image centerTitle;
		public Button inviteFriendButton;
		public  Button ExitRoomButton;

		public Image remainCard;
		public Image remainRound;
		public Image centerImage;
		public GameObject genZhuang;
		public Text versionText;

		//======================================
		private int uuid;
		private float timer = 0;
		private int LeavedCardsNum;


		/**抓码动态面板**/
		private GameObject zhuamaPanel;


		/// <summary>
		/// 手牌数组，0自己，1-右边。2-上边。3-左边
		/// </summary>
		public List<List<GameObject>> handCardList;
//过时

		/**所有的抓码数据字符串**/
		private string allMas;

		private bool isFirstOpen = true;

		/**是否为抢胡 游戏结束时需置为false**/
		private bool isQiangHu = false;

		private UIHelper _uiHelper;
		private DissoliveHelper _dissoliveHlpr;
		private ActionEffectHelper _actionHlpr;

		private GamingData _data;

		public GamingData Data {
			get {
				return	_data;
			}
		}

		public DissoliveHelper DissoliveHlpr {
			get {
				return _dissoliveHlpr;
			}
		}

		#region ISceneView implementation

		public void open (object data = null)
		{
		
		}

		public void close (object data = null)
		{
			GlobalData.getInstance ().myAvatarVO.resetData ();//复位房间数据
			GlobalData.getInstance ().roomVO.roomId = 0;
			GlobalData.soundToggle = true;
			_uiHelper.clean ();
			removeListener ();

			SoundCtrl.getInstance ().playBGM ();


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
			_dissoliveHlpr = new DissoliveHelper ();
			_data = new GamingData ();
		}

		void Start ()
		{
			
			SoundCtrl.getInstance ().stopBGM ();
			//===========================================================================================
			versionText.text = "V" + Application.version;
			//===========================================================================================
			_actionHlpr = gameObject.GetComponent<ActionEffectHelper> ();
			_actionHlpr.addListener (this);

			_data.init (this);
			_uiHelper.init (_data, this);
			_dissoliveHlpr.init (this);


			addListener ();
			cleanTable ();

			if (_data.isReEnter) {
				_data.isReEnter = false;
				reEnterRoom ();
			} else {
				joinToRoom ();
			}
		}

		void cleanTable ()
		{
			_uiHelper.clean ();
			_actionHlpr.cleanBtnShow ();
		}

		public void addListener ()
		{
			GameManager.getInstance ().Server.onResponse += onResponse;

			SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
			SocketEventHandle.getInstance ().micInputNotice += micInputNotice;

		}

		private void removeListener ()
		{
			GameManager.getInstance ().Server.onResponse -= onResponse;

			SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
			SocketEventHandle.getInstance ().micInputNotice -= micInputNotice;
		}

		void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.STARTGAME_RESPONSE_NOTICE://开始游戏
				startGame (response);
				break;
			case APIS.PICKCARD_RESPONSE://自己摸牌
				pickCard (response);
				break;
			case APIS.OTHER_PICKCARD_RESPONSE_NOTICE://别人摸牌通知
				otherPickCard (response);
				break;
			case APIS.CHUPAI_RESPONSE://出牌通知
				otherPutOutCard (response);
				break;
			case APIS.JOIN_ROOM_NOICE:
				otherUserJointRoom (response);
				break;
			case APIS.PENGPAI_RESPONSE://碰牌回调
				onPeng (response);
				break;
			case APIS.GANGPAI_RESPONSE://杠牌回调
				onMyGang (response);
				break;
			case APIS.OTHER_GANGPAI_NOICE:
				onOtherGang (response);
				break;
			case APIS.ACTION_BUTTON_NOTICE://碰杠行为按钮显示
				onActionBtnNotice (response);
				break;
			case APIS.HUPAI_RESPONSE://胡牌回调
				hupaiCallBack (response);
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
			case APIS.RETURN_ONLINE_RESPONSE:
				returnGameResponse (response);
				break;
			case APIS.Game_FollowBander_Notice://跟庄
				gameFollowBanderNotice (response);
				break;
			default:
				return;	

			}
			Debug.Log ("下发：" + response.headCode.ToString ("x8") + " " + response.message);

		}



		/// <summary>
		/// 开始游戏
		/// </summary>
		/// <param name="response">Response.</param>
		public void startGame (ClientResponse response)
		{
			//GlobalDataScript.surplusTimes -= 1;
			StartGameVO svo = JsonMapper.ToObject<StartGameVO> (response.message);
			_data.bankerIndex = svo.bankerId;
			GlobalData.getInstance ().gameStatus = GameStatus.GAMING;
			cleanGameplayUI ();
			//开始游戏后不显示
			MyDebug.Log ("startGame");
			GlobalData.getInstance ().remainRoundCount--;

			LeavedRoundNumText.text = GlobalData.getInstance ().remainRoundCount + "";//刷新剩余圈数
			if (!isFirstOpen) {
				isFirstOpen = false;
				cleanTable ();
				avatarList [_data.bankerIndex].main = true;
			}


			GlobalData.finalGameEndVo = null;
			GlobalData.isOverByPlayer = false;
			GlobalData.mainUuid = avatarList [_data.bankerIndex].account.uuid;
			GlobalData.isDrag = _data.bankerIndex == _data.myIndex;


			_uiHelper.getBankerCGO ().PlayerItem.setbankImgEnable (true);
			_uiHelper.setDirectPointer (_data.bankerIndex);

			_data.paiArray = svo.paiArray;

			UpateTimeReStart ();
			_uiHelper.hideReadyIcon ();
			//创建手牌
			_uiHelper.addMyHandCards (_data.paiArray [0]);
			_uiHelper.addOtherHandCards (Direction.L, 13);
			_uiHelper.addOtherHandCards (Direction.R, 13);
			_uiHelper.addOtherHandCards (Direction.T, 13);
			//突出摸到的牌
			if (_data.bankerIndex == _data.myIndex) {
				_uiHelper.rangeMyHandCard (true);//设置位置
				MyDebug.Log ("初始化数据自己为庄家");
			} else {
				_uiHelper.rangeMyHandCard (false);
				_uiHelper.addPickCard (_data.bankerIndex);
			}
			//
			_uiHelper.initRemainCardNum ();

		}

		private void cleanGameplayUI ()
		{
			centerTitle.transform.gameObject.SetActive (false);
			inviteFriendButton.transform.gameObject.SetActive (false);
			ExitRoomButton.transform.gameObject.SetActive (false);
			remainCard.transform.gameObject.SetActive (true);
			remainRound.transform.gameObject.SetActive (true);
			centerImage.transform.gameObject.SetActive (true);
			_actionHlpr.liujuEffect.SetActive (false);
		}

		/// <summary>
		/// 别人摸牌通知
		/// </summary>
		/// <param name="response">Response.</param>
		public void otherPickCard (ClientResponse response)
		{
			UpateTimeReStart ();
			JsonData json = JsonMapper.ToObject (response.message);
			//下一个摸牌人的索引
			int avatarIndex = (int)json ["avatarIndex"];
			MyDebug.Log ("otherPickCard avatarIndex = " + avatarIndex);
			_uiHelper.addPickCard (avatarIndex);
			_uiHelper.setDirectPointer (avatarIndex);
			_uiHelper.updateRemainCardNum ();
		}


		/// <summary>
		/// 自己摸牌
		/// </summary>
		/// <param name="response">Response.</param>
		public void pickCard (ClientResponse response)
		{

			UpateTimeReStart ();
			CardVO cardvo = JsonMapper.ToObject<CardVO> (response.message);
			int cardPoint = cardvo.cardPoint;
			_data.pickIndex = _data.myIndex;
			_data.pgcCardPoint = cardPoint;
			MyDebug.Log ("摸牌:" + cardPoint);
			putCardIntoMineList (cardPoint);
			_uiHelper.addPickCard (_data.myIndex, cardvo.cardPoint);
			_uiHelper.setDirectPointer (_data.myIndex);
			_uiHelper.updateRemainCardNum ();
			GlobalData.isDrag = true;
		}

		/// <summary>
		/// 胡，杠，碰，吃，pass按钮显示.
		/// </summary>
		/// <param name="response">Response.</param>
		public void onActionBtnNotice (ClientResponse response)
		{
			GlobalData.isDrag = false;
			string[] strs = response.message.Split (new char[1]{ ',' });


			for (int i = 0; i < strs.Length; i++) {
				if (strs [i].Equals ("hu")) {
					_actionHlpr.showBtn (ActionType.HU);

				} else if (strs [i].Contains ("qianghu")) {
				
					try {
						_data.pgcCardPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [1]);
					} catch (Exception e) {
						Debug.Log (e.ToString ());
					}

					_actionHlpr.showBtn (ActionType.HU);
					isQiangHu = true;
				} else if (strs [i].Contains ("peng")) {
					_actionHlpr.showBtn (ActionType.PENG);
					_data.putoutPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [2]);


				} else if (strs [i].Contains ("chi")) {
					_actionHlpr.showBtn (ActionType.CHI);
					_data.putoutPoint = int.Parse (strs [i].Split (new char[1]{ ':' }) [2]);
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


		public void putCardIntoMineList (int cardPoint)
		{
			if (_data.paiArray [0] [cardPoint] < 4) {
				_data.paiArray [0] [cardPoint]++;
			}
		}

		public void pushOutFromMineList (int cardPoint)
		{

			if (_data.paiArray [0] [cardPoint] > 0) {
				_data.paiArray [0] [cardPoint]--;
			}
		}

		/// <summary>
		/// 接收到其它人的出牌通知
		/// </summary>
		/// <param name="response">Response.</param>
		public void otherPutOutCard (ClientResponse response)
		{
		
			JsonData json = JsonMapper.ToObject (response.message);
			int cardPoint = (int)json ["cardIndex"];
			int curAvatarIndex = (int)json ["curAvatarIndex"];
			_data.pgcCardPoint = cardPoint;
			_uiHelper.removePickCard ();
			createPutOutCardAndPlayAction (cardPoint, curAvatarIndex);
		}

		/// <summary>
		/// 创建打来的的牌对象，并且开始播放动画
		/// </summary>
		/// <param name="cardPoint">Card point.</param>
		/// <param name="curAvatarIndex">Current avatar index.</param>
		private void createPutOutCardAndPlayAction (int cardPoint, int avatarIndex)
		{
			MyDebug.Log ("put out cardPoint" + cardPoint);
			SoundCtrl.getInstance ().playSound (cardPoint, avatarList [avatarIndex].account.sex);

			_data.putoutPoint = cardPoint;

			_uiHelper.addPutoutCard (cardPoint, _data.toGameDir (avatarIndex));

			_uiHelper.lastCardOnTable = _uiHelper.addCardToTable (_data.toGameDir (avatarIndex), cardPoint);

			_uiHelper.addPointer (_uiHelper.lastCardOnTable);


		}


		private void onPeng (ClientResponse response)//碰牌处理
		{
			UpateTimeReStart ();
			OtherPengGangBackVO pvo = JsonMapper.ToObject<OtherPengGangBackVO> (response.message);

			_data.pickIndex = pvo.avatarId;
			int cardPoint = pvo.cardPoint;
			_uiHelper.setDirectPointer (_data.pickIndex);
			_actionHlpr.pengGangHuEffectCtrl (ActionType.PENG);
			SoundCtrl.getInstance ().playSoundByAction ("peng", avatarList [_data.pickIndex].account.sex);

			_uiHelper.removeLastPutoutCard ();


			if (_data.pickIndex == _data.myIndex) {  //自己碰牌
				_data.paiArray [0] [cardPoint]++;
				_data.paiArray [1] [cardPoint] = 2;
				_uiHelper.removeMyHandCard (cardPoint, 2);
				_uiHelper.rangeMyHandCard (true);

				_uiHelper.addPGCCards (Direction.B, cardPoint, 1);
				GlobalData.isDrag = true;
		
			} else {//其他人碰牌
				List<GameObject> Hand = _uiHelper.getCardGOs (_data.pickIndex).Hand;
				string path = "Prefab/PengGangCard/PengGangCard_" + _data.toGameDir (_data.pickIndex).ToString ();
				Direction dir = _data.toGameDir (_data.pickIndex);
				_uiHelper.removeOtherHandCard (dir, 2);
				_uiHelper.rangeOtherHandCard (dir, 1);

				_uiHelper.addPGCCards (dir, cardPoint, 1);
			}
		}


		private void onOtherGang (ClientResponse response) //其他人杠牌
		{

			GangNoticeVO gvo = JsonMapper.ToObject<GangNoticeVO> (response.message);
			int cardPoint = gvo.cardPoint;
			_data.pickIndex = gvo.avatarId;
			_actionHlpr.pengGangHuEffectCtrl (ActionType.GANG);
			_uiHelper.setDirectPointer (_data.pickIndex);
			SoundCtrl.getInstance ().playSoundByAction ("gang", avatarList [_data.pickIndex].account.sex);

			Direction dir = _data.toGameDir (_data.pickIndex);
			if (_uiHelper.findIndexInPGC (dir, cardPoint) == -1) {//判断有没有碰过了，
				_uiHelper.removeOtherHandCard(dir,3);//消耗3只牌
				_uiHelper.rangeMyHandCard (false);
				_uiHelper.rangeOtherHandCard (dir, 2);

				if (gvo.type == 0) {//明杠别人
					_uiHelper.removeLastPutoutCard ();
					_uiHelper.addPGCCards (dir, cardPoint, 2);
				} else {//暗杠
					_uiHelper.removePickCard ();//消耗一只摸的牌
					_uiHelper.addPGCCards (dir, cardPoint, 3);//杠4只到桌面
				}

			} else {
				_uiHelper.removePickCard ();
				_uiHelper.addPGCCards (dir, cardPoint, 5);//明杠1只到桌面

			}

		}

		/**客户端主动打一只牌**/
		public void putoutCard (int cardPoint)//
		{
			pushOutFromMineList (cardPoint);
			createPutOutCardAndPlayAction (cardPoint, _data.myIndex);//讲拖出牌进行第一段动画的播放
			//========================================================================
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = cardPoint;
			GameManager.getInstance ().Server.requset (new PutOutCardRequest (cardvo));
		}



		void Update ()
		{
			timer -= Time.deltaTime;
			if (timer < 0) {
				timer = 0;
				//UpateTimeReStart();
			}
			Number.text = Math.Floor (timer) + "";

		}

		/// <summary>
		/// 重新开始计时
		/// </summary>
		void UpateTimeReStart ()
		{
			timer = 16;
		}

		/// <summary>
		/// 点击放弃按钮
		/// </summary>
		public void myPassBtnClick ()
		{
			_actionHlpr.cleanBtnShow ();

			if (_data.pickIndex == _data.myIndex) {
				GlobalData.isDrag = true;
			}
			GameManager.getInstance ().Server.requset (new GaveUpRequest ());
		}

		public void myPengBtnClick ()
		{
			GlobalData.isDrag = true;
			UpateTimeReStart ();
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = _data.pgcCardPoint;
			GameManager.getInstance ().Server.requset (new PengPaiRequest (cardvo));
			_actionHlpr.cleanBtnShow ();
		}

		public void myChiBtnClick ()
		{
			GlobalData.isDrag = false;
			UpateTimeReStart ();
			CardVO cardvo = new CardVO ();
			cardvo.cardPoint = _data.pgcCardPoint;
			cardvo.onePoint = _data.pgcCardPoint;
			cardvo.twoPoint = _data.pgcCardPoint;
			GameManager.getInstance ().Server.requset (new ChiPaiRequest (cardvo));
			_actionHlpr.cleanBtnShow ();
		}


		public void onMyGang (ClientResponse response)
		{
			UpateTimeReStart ();
			GangBackVO gvo = JsonMapper.ToObject<GangBackVO> (response.message);
			int gangKind = gvo.type;
			if (gvo.cardList.Count == 0) {
				int cardPoint = _data.pgcCardPoint;
				if (gangKind == 0) {//明杠
					
					_data.paiArray[1][cardPoint] = 3;
					/**杠牌点数**/
					if (_uiHelper.findIndexInPGC (Direction.B,cardPoint) == -1) {//杠别人
						_uiHelper.removeLastPutoutCard ();
						_uiHelper.removeMyHandCard (cardPoint, 3);
						_uiHelper.addPGCCards (Direction.B, cardPoint, 2);
					} else {//摸牌杠
						_uiHelper.removeMyHandCard(cardPoint,1);
						_uiHelper.addPGCCards(Direction.B,cardPoint,5);
					}

				} else if (gangKind == 1) { //暗杠

					_data.paiArray [1] [cardPoint] = 4;
					_uiHelper.removeMyHandCard (cardPoint, 4);

					_uiHelper.addPGCCards(Direction.B,cardPoint,3);

				}
			} else if (gvo.cardList.Count == 2) {

			}
			_uiHelper.rangeMyHandCard (false);
		}


		public void myGangBtnClick ()
		{
			//TODO 多张可以杠的牌
			_data.pgcCardPoint = int.Parse (_data.gangPaiList [0]);
			_data.gangPaiList = null;
			GameManager.getInstance ().Server.requset (new GangPaiRequest (_data.pgcCardPoint, 0));
			SoundCtrl.getInstance ().playSoundByAction ("gang", GlobalData.getInstance ().myAvatarVO.account.sex);
			_actionHlpr.cleanBtnShow ();
			_actionHlpr.pengGangHuEffectCtrl (ActionType.GANG);

			GlobalData.isDrag = true;
		}

			

		/**进入房间**/
		public void joinToRoom ()
		{
			avatarList = GlobalData.getInstance ().playerList;
			_data.AvatarList = GlobalData.getInstance ().playerList;
			for (int i = 0; i < avatarList.Count; i++) {
				_uiHelper.getCardGOs (i).PlayerItem.setAvatarVo (avatarList [i]);
			}
			_uiHelper.updateRule (roomRemark);
			GlobalData.getInstance ().gameStatus = GameStatus.READYING;
			GameManager.getInstance ().Server.requset (new GameReadyRequest ());
			markselfReadyGame ();
		}


		private int getIndex (int uuid)
		{
			if (avatarList != null) {
				for (int i = 0; i < avatarList.Count; i++) {
					if (avatarList [i].account != null) {
						if (avatarList [i].account.uuid == uuid) {
							return i;
						}
					}
				}
			}
			return 0;
		}

		public void otherUserJointRoom (ClientResponse response)
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
				int cardPoint = _data.pgcCardPoint;//需修改成正确的胡牌cardpoint
				CardVO requestVo = new CardVO ();
				requestVo.cardPoint = cardPoint;
				if (isQiangHu) {
					requestVo.type = "qianghu";
					isQiangHu = false;
				}
				string sendMsg = JsonMapper.ToJson (requestVo);
				GameManager.getInstance ().Server.requset (new HupaiRequest (sendMsg));
				_actionHlpr.cleanBtnShow ();
			}
		}



		/**
	 * 胡牌请求回调
	 */ 
		private void hupaiCallBack (ClientResponse response)
		{
			//删除这句，未区分胡家是谁
			GlobalData.hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo> (response.message);

			string scores = GlobalData.hupaiResponseVo.currentScore;
			hupaiCoinChange (scores);


			if (GlobalData.hupaiResponseVo.type == "0") {
				SoundCtrl.getInstance ().playSoundByAction ("hu", GlobalData.getInstance ().myAvatarVO.account.sex);
				_actionHlpr.pengGangHuEffectCtrl (ActionType.HU);

				for (int i = 0; i < GlobalData.hupaiResponseVo.avatarList.Count; i++) {
					if (checkAvarHupai (GlobalData.hupaiResponseVo.avatarList [i]) == 1) {//胡
						_uiHelper.getCardGOs (i).PlayerItem.setHuFlagDisplay ();
						SoundCtrl.getInstance ().playSoundByAction ("hu", avatarList [i].account.sex);
					} else if (checkAvarHupai (GlobalData.hupaiResponseVo.avatarList [i]) == 2) {
						_uiHelper.getCardGOs (i).PlayerItem.setHuFlagDisplay ();
						SoundCtrl.getInstance ().playSoundByAction ("zimo", avatarList [i].account.sex);
					} else {
						_uiHelper.getCardGOs (i).PlayerItem.setHuFlagHidde ();
					}

				}

				allMas = GlobalData.hupaiResponseVo.allMas;
				if (GlobalData.getInstance ().roomVO.roomType == GameType.ZHUAN_ZHUAN || GlobalData.getInstance ().roomVO.roomType == GameType.GI_PING_HU) {//只有转转麻将才显示抓码信息
					if (GlobalData.getInstance ().roomVO.ma > 0 && allMas != null && allMas.Length > 0) {
						zhuamaPanel = PrefabManage.loadPerfab ("prefab/Panel_ZhuaMa");
						zhuamaPanel.GetComponent<ZhuMaScript> ().init (_data);
						zhuamaPanel.GetComponent<ZhuMaScript> ().arrageMas (allMas, avatarList, GlobalData.hupaiResponseVo.validMas);
						Invoke ("openGameOverPanelSignal", 7);
					} else {
						Invoke ("openGameOverPanelSignal", 3);
					}

				} else {
					Invoke ("openGameOverPanelSignal", 3);
				}


			} else if (GlobalData.hupaiResponseVo.type == "1") {

				SoundCtrl.getInstance ().playSoundByAction ("liuju", GlobalData.getInstance ().myAvatarVO.account.sex);
				_actionHlpr.pengGangHuEffectCtrl (ActionType.LIUJU);
				Invoke ("openGameOverPanelSignal", 3);
			} else {
				Invoke ("openGameOverPanelSignal", 3);
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


		private void openGameOverPanelSignal ()
		{//单局结算
			_actionHlpr.liujuEffect.SetActive (false);
			_uiHelper.hideHuIcon ();
			if (zhuamaPanel != null) {
				Destroy (zhuamaPanel.GetComponent<ZhuMaScript> ());
				Destroy (zhuamaPanel);
			}

			//GlobalDataScript.singalGameOver = PrefabManage.loadPerfab("prefab/Panel_Game_Over");
			GameObject gameOverView = PrefabManage.loadPerfab ("Prefab/Panel_Game_Over");
			avatarList [_data.bankerIndex].main = false;
			_uiHelper.getBankerCGO ().PlayerItem.setbankImgEnable (false);


			cleanTable ();
			gameOverView.GetComponent<GameOverScript> ().setDisplaContent (0, avatarList, allMas, GlobalData.hupaiResponseVo.validMas);
			GlobalData.singalGameOverList.Add (gameOverView);
			allMas = "";//初始化码牌数据为空
			//GlobalDataScript.singalGameOver.GetComponent<GameOverScript> ().setDisplaContent (0,avatarList,allMas,GlobalDataScript.hupaiResponseVo.validMas);	
		}




		private void  loadPerfab (string perfabName, int openFlag)
		{
			GameObject obj = PrefabManage.loadPerfab (perfabName);
			obj.GetComponent<GameOverScript> ().setDisplaContent (openFlag, avatarList, allMas, GlobalData.hupaiResponseVo.validMas);
		}

		/***
	 * 退出房间请求
	 */
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


		private void gameFollowBanderNotice (ClientResponse response)
		{
			genZhuang.SetActive (true);
			Invoke ("hideGenzhuang", 2f);
		}

		private void hideGenzhuang ()
		{
			genZhuang.SetActive (false);
		}

		/*************************断线重连*********************************/
		private void reEnterRoom ()
		{
			_uiHelper.updateRule (roomRemark);
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

				for (int i = 0; i < avatarList.Count; i++) {
					AvatarVO avo = avatarList [i];
					var dir = _data.toGameDir (i);
					//HandCard
					_uiHelper.addOtherHandCards (dir, avo.commonCards);
					//TableCard
					int[] chupai = avo.chupais;
					if (chupai != null && chupai.Length > 0) {
						for (int j = 0; j < chupai.Length; j++) {
							_uiHelper.addCardToTable (dir, chupai [j]);
						}
					}
					//GangCard
					int[] paiArrayType = avo.paiArray [1];
					if (paiArrayType.Contains<int> (2)) {
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
								avo.paiArray [0] [gangpaiObj.cardPiont] -= 4;
								if (gangpaiObj.type == "an") {
									_uiHelper.addPGCCards (_data.toGameDir (i), gangpaiObj.cardPiont, 3);

								} else {
									_uiHelper.addPGCCards (_data.toGameDir (i), gangpaiObj.cardPiont, 2);

								}
							}
						}
					}
					//PengCard
					if (paiArrayType.Contains<int> (1)) {
						for (int j = 0; j < paiArrayType.Length; j++) {
							if (paiArrayType [j] == 1 && avo.paiArray [0] [j] > 0) {
								avo.paiArray [0] [j] -= 3;
								_uiHelper.addPGCCards (_data.toGameDir (i), j, 1);
							}
						}
					}
				}
				_uiHelper.addMyHandCards (_data.paiArray [0]);
				GameManager.getInstance ().Server.requset (new CurrentStatusRequest ());
			}

		}

		public void myselfSoundActionPlay ()
		{
			_uiHelper.getCardGOs (Direction.B).PlayerItem.showChatAction ();
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


			//申请解散房间过程中，有人掉线，直接不能解散房间
			if (GlobalData.isDissoliving) {
				_dissoliveHlpr.closeDialog ();
				TipsManager.getInstance ().setTips ("由于" + avatarList [index].account.nickname + "离线，系统不能解散房间。");

			}
		}

		/**用户上线提醒**/
		public void onlineNotice (ClientResponse  response)
		{
			int uuid = int.Parse (response.message);
			var avatarIndex = _data.toAvatarIndex (uuid);
			_uiHelper.getCardGOs (avatarIndex).PlayerItem.setPlayerOnline ();

		}


		public void messageBoxNotice (ClientResponse response)
		{
			string[] arr = response.message.Split (new char[1]{ '|' });
			int uuid = int.Parse (arr [1]);
			var avatarIndex = _data.toAvatarIndex (uuid);
			_uiHelper.getCardGOs (avatarIndex).PlayerItem.showChatMessage (int.Parse (arr [0]));
		}


		/*显示自己准备*/
		private void markselfReadyGame ()
		{
			_uiHelper.getCardGOs (Direction.B).PlayerItem.readyImg.transform.gameObject.SetActive (true);
		}


		public void micInputNotice (ClientResponse response)
		{
			int uuid = int.Parse (response.message);
			var avatarIndex = _data.toAvatarIndex (uuid);
			_uiHelper.getCardGOs (avatarIndex).PlayerItem.showChatAction ();
		}


		public void returnGameResponse (ClientResponse response)
		{
			//1.显示剩余牌的张数和圈数
			JsonData msgData = JsonMapper.ToObject (response.message);
			string surplusCards = msgData ["surplusCards"].ToString ();
			_data.remainCardNum = int.Parse (surplusCards) + 1;
			_uiHelper.updateRemainCardNum ();
			int gameRound = int.Parse (msgData ["gameRound"].ToString ());
			LeavedRoundNumText.text = gameRound + "";
			GlobalData.getInstance ().remainRoundCount = gameRound;


			int avatarIndexPutout = -1;//当前出牌人的索引
			int avatarIndexPick = -1; //当前摸牌人的索引
			int cardPointPutout = -1;//当前打得点数
			int cardPointPick = -1;//当前摸的点数


			//不是自己摸牌
			try {

				avatarIndexPutout = int.Parse (msgData ["curAvatarIndex"].ToString ());//当前打牌人的索引
				cardPointPutout = int.Parse (msgData ["putOffCardPoint"].ToString ());//当前打得点数


				_data.pgcCardPoint = cardPointPutout;
				avatarIndexPick = int.Parse (msgData ["pickAvatarIndex"].ToString ()); //当前摸牌牌人的索引

				if (msgData.Keys.Contains ("currentCardPoint")) {
					cardPointPick = int.Parse (msgData ["currentCardPoint"].ToString ());//当前摸得的点数  
					_data.pickPoint = cardPointPick; 
				}

			} catch (Exception e) {
				Debug.Log (e.ToString ());
			}

			_uiHelper.setDirectPointer (avatarIndexPick);
			_data.putoutIndex = avatarIndexPutout;

			if (avatarIndexPick == _data.myIndex) {//TODO 这一段很有问题，删了又加，多余
				if (cardPointPick == -2) {
					int cardPoint = handCardList [0] [handCardList [0].Count - 1].GetComponent<bottomScript> ().getPoint ();

					Destroy (handCardList [0] [handCardList [0].Count - 1]);
					handCardList [0].Remove (handCardList [0] [handCardList [0].Count - 1]);
					_uiHelper.rangeMyHandCard (false);
					putCardIntoMineList (cardPoint);
					_uiHelper.addPickCard (_data.myIndex, cardPoint);

					MyDebug.Log ("自己摸牌");

				} else {
					if ((handCardList [0].Count) % 3 != 1) {
						int cardPoint = cardPointPick;
						MyDebug.Log ("摸牌" + cardPoint);
						for (int i = 0; i < handCardList [0].Count; i++) {
							if (handCardList [0] [i].GetComponent<bottomScript> ().getPoint () == cardPointPick) {
								Destroy (handCardList [0] [i]);
								handCardList [0].Remove (handCardList [0] [i]);
								break;
							}
						}
						_uiHelper.rangeMyHandCard (false);
						putCardIntoMineList (cardPoint);
						_uiHelper.addPickCard (_data.myIndex, cardPoint);
					}

				}
				GlobalData.isDrag = true;	
			} else { //别人摸牌
				
			}





			//光标指向打牌人

			var Table = _uiHelper.getCardGOs (avatarIndexPutout).Table;
			if (Table.Count == 0) {//刚启动


			} else {
				_uiHelper.lastCardOnTable = Table [Table.Count - 1];
				_uiHelper.addPointer (_uiHelper.lastCardOnTable);
			}


		}

	}

}