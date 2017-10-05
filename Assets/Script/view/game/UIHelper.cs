using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{ 
	public class UIHelper
	{
		private GameObject _pointerGO;
		public GameObject lastCardOnTable;
		public GameObject putOutCard;
		/**自己摸到的牌**/
		private GameObject pickCardItem;
		private GameObject otherPickCardItem;
		private GamingData _data;
		private List<CardGOs> allCardGOs;
		GameView _host;

		public void init (GamingData data, GameView host)
		{
			_data = data;
			_host = host;
			allCardGOs = new List<CardGOs> (4);
			for (int i = 0; i < 4; i++) {
				CardGOs cgo = new CardGOs ();
				cgo.HandParent = host.HandParents [i];
				cgo.TableParent = host.TableParents [i];
				cgo.PGCParent = host.PGCParents [i];
				cgo.PlayerItem = host.PlayerItemViews [i];
				allCardGOs.Add (cgo);
			}
		}

		public CardGOs getBankerCGO ()
		{
			return allCardGOs [(int)_data.toGameDir (_data.bankerIndex)];
		}

		public CardGOs getCardGOs (Direction dir)
		{
			return allCardGOs [(int)dir];
		}

		public CardGOs getCardGOs (int avatarIndex)
		{
			var dir = _data.toGameDir (avatarIndex);
			return getCardGOs (dir);
		}

		public void hideHuIcon ()
		{
			for (int i = 0; i < 4; i++) {
				allCardGOs [i].PlayerItem.setHuFlagHidde ();
			}
		}

		public void hideReadyIcon ()
		{
			for (int i = 0; i < 4; i++) {
				allCardGOs [i].PlayerItem.readyImg.enabled = false;
			}
		}

		public void addMyHandCards (List<int> cardPoints)
		{
			var cgo = getCardGOs (Direction.B);
			for (int i = 0; i < cardPoints.Count; i++) {
				for (int j = 0; j < cardPoints [i]; j++) {
					GameObject gob = GameObject.Instantiate (Resources.Load ("prefab/card/Bottom_B")) as GameObject;
					gob.transform.SetParent (cgo.HandParent);
					gob.transform.localScale = new Vector3 (1.1f, 1.1f, 1);
					gob.GetComponent<bottomScript> ().onMyHandCardPutout += onMyHandCardPutout;
					gob.GetComponent<bottomScript> ().onMyHandCardSelectedChange += onMyHandCardSelectedChange;
					gob.GetComponent<bottomScript> ().setPoint (i);        

					cgo.Hand.Add (gob);//增加游戏对象

				}
			}
			rangeMyHandCard (false);
		}

		public void onMyHandCardPutout (GameObject card)//
		{
			var Hand = getCardGOs (Direction.B).Hand;
			int handCardCount = Hand.Count - 1;
			if (handCardCount == 13 || handCardCount == 10 || handCardCount == 7 || handCardCount == 4 || handCardCount == 1) {
				GlobalData.isDrag = false;
				card.GetComponent<bottomScript> ().onMyHandCardPutout -= onMyHandCardPutout;
				card.GetComponent<bottomScript> ().onMyHandCardSelectedChange -= onMyHandCardSelectedChange;

				Hand.Remove (card);
				GameObject.Destroy (card);
				rangeMyHandCard (false);

				int putOutCardPointTemp = card.GetComponent<bottomScript> ().getPoint ();//将当期打出牌的点数传出
				_host.putoutCard (putOutCardPointTemp);
			}

		}

		public void onMyHandCardSelectedChange (GameObject card)
		{
			var Hand = getCardGOs (Direction.B).Hand;
			for (int i = 0; i < Hand.Count; i++) {
				var g = Hand [i];
				if (g == null) {
					Hand.RemoveAt (i);
					i--;
				} else {
					g.transform.localPosition = new Vector3 (g.transform.localPosition.x, -292f); //从右到左依次对齐
					g.transform.GetComponent<bottomScript> ().selected = false;
				}
			}
			if (card != null) {
				card.transform.localPosition = new Vector3 (card.transform.localPosition.x, -272f);
				card.transform.GetComponent<bottomScript> ().selected = true;
			}
		}

		public void rangeMyHandCard (bool flag)//设置位置
		{
			var Hand = getCardGOs (Direction.B).Hand;
			int startX = 594 - Hand.Count * 80;
			if (flag) {
				for (int i = 0; i < Hand.Count - 1; i++) {
					Hand [i].transform.localPosition = new Vector3 (startX + i * 80f, -292f); //从左到右依次对齐
				}
				Hand [Hand.Count - 1].transform.localPosition = new Vector3 (580f, -292f); //从左到右依次对齐

			} else {
				for (int i = 0; i < Hand.Count; i++) {
					Hand [i].transform.localPosition = new Vector3 (startX + i * 80f - 80f, -292f); //从左到右依次对齐
				}
			}
		}

		public void addOtherHandCards (Direction dir, int count)
		{
			var cgo = getCardGOs (dir);
			for (int i = 0; i < count; i++) {
				GameObject card = GameObject.Instantiate (Resources.Load ("Prefab/card/Bottom_" + dir.ToString ())) as GameObject;
				card.transform.SetParent (cgo.HandParent); //父节点
				card.transform.localScale = Vector3.one;
				switch (dir) {
				case Direction.B:

					return;
				case Direction.T:
					card.transform.localPosition = new Vector3 (-204 + 38 * i, 0); 

					break;
				case Direction.L://左
					card.transform.localPosition = new Vector3 (0, -105 + i * 30);
					card.transform.SetSiblingIndex (0);
					break;
				case Direction.R://右
					card.transform.localPosition = new Vector3 (0, 119 - i * 30);  
					break;
				}
				card.transform.localScale = Vector3.one; 
				cgo.Hand.Add (card);
			}
		}
		public void removeMyHandCard(int cardPoint,int num){
			var Hand = getCardGOs (Direction.B).Hand;
			for (int i = 0; i < Hand.Count; i++) {
				GameObject card = Hand [i];
				int point = card.GetComponent<bottomScript> ().getPoint ();
				if (point == cardPoint) {

					Hand.RemoveAt (i);
					GameObject.Destroy (card);
					i--;
					num--;
					if (num == 0) {
						break;
					}
				}
			}
		}
		public void removeOtherHandCard(Direction dir,int num){
			var Hand = getCardGOs (dir).Hand;
			for (int i = 0; i < num; i++) {
				GameObject temp = Hand [0];
				Hand.RemoveAt (0);
				GameObject.Destroy (temp);
			}

		}
		/**排列其他玩家手牌，type:1碰2杠**/
		public void rangeOtherHandCard (Direction dir,int type)//设置位置
		{
			var Hand = getCardGOs (dir).Hand;
			if (Hand.Count < 1)
				return;
			if (type == 1) {
				switch (dir) {
				case Direction.T: //上
					Hand [0].transform.localPosition = new Vector3 (-273f,0f); //位置                      
					break;
				case Direction.L: //左
					Hand [0].transform.localPosition = new Vector3 (0, -173); //位置              
					break;
				case Direction.R: //右
					Hand [0].transform.localPosition = new Vector3 (0, 180f); //位置                  
					break;
				}


				for (int i = 1; i < Hand.Count; i++) {

					switch (dir) {
					case Direction.T: //上
						Hand [i].transform.localPosition = new Vector3 (-204f + 37 * (i - 1), 0); //位置                      
						break;
					case Direction.L: //左
						Hand [i].transform.localPosition = new Vector3 (0, -105 + (i - 1) * 30); //位置              
						break;
					case Direction.R: //右
						Hand [i].transform.localPosition = new Vector3 (0, 119 - (i - 1) * 30); //位置                  
						break;
					}
				}
			} else {

				for (int i = 0; i < Hand.Count; i++) {

					switch (dir) {
					case Direction.T: //上
						Hand [i].transform.localPosition = new Vector3 (-204 + 37 * i, 0); //位置                      
						break;
					case Direction.L: //左
						Hand [i].transform.localPosition = new Vector3 (0, -105 + i * 30); //位置              
						break;
					case Direction.R: //右
						Hand [i].transform.localPosition = new Vector3 (0, 119 - i * 30); //位置                  
						break;
					}
				}
			}
		}

		/**type:1碰2明杠3暗杠4吃5先碰后杠**/
		public void addPGCCards (Direction dir, int cardPoint, int type)
		{
			String[] AN_GANG_PATHS = new string[] {
				"Prefab/PengGangCard/gangBack",
				"Prefab/PengGangCard/GangBack_L&R",
				"Prefab/PengGangCard/GangBack_T",
				"Prefab/PengGangCard/GangBack_L&R"
			};
			String[] Normal_PATHS = new string[] {
				"Prefab/PengGangCard/PengGangCard_B",
				"Prefab/PengGangCard/PengGangCard_R",
				"Prefab/PengGangCard/PengGangCard_T",
				"Prefab/PengGangCard/PengGangCard_L"
			};
			int[] points;
			var cgo = getCardGOs (dir);
			if (type == 5) {
				int index = findIndexInPGC (dir, cardPoint);
				GameObject card = newGameObject (Normal_PATHS [(int)dir], cgo.PGCParent, _getPGCPosition (dir, 1, 1, index));
				if (dir == Direction.R) {
					card.transform.SetSiblingIndex (0);
				}
				card.GetComponent<PutoutCardView> ().setPoint (cardPoint,dir);
				cgo.PGC [index].Add (card);
				return;
			}else if (type == 3 || type == 2) {
				points = new int[]{ cardPoint, cardPoint, cardPoint, cardPoint };
			} else if (type == 1) {
				points = new int[]{ cardPoint, cardPoint, cardPoint };
			} else {
				points = new int[]{ cardPoint + 1, cardPoint + 2, cardPoint + 3 };
			}

			List<GameObject> cards = new List<GameObject> ();
			for (int i = 0; i < points.Length; i++) {
				GameObject card;
				int k = i == 3 ? 1 : i;
				int j = i == 3 ? 1 : 0;
				string path = i < 3 && type == 3 ? AN_GANG_PATHS [(int)dir] : Normal_PATHS [(int)dir];

				Vector3 position = _getPGCPosition (dir, k, j, cgo.PGC.Count);

				card = newGameObject (path, cgo.PGCParent, position);
				if (!(i < 3 && type == 3)) {
					card.GetComponent<PutoutCardView> ().setPoint (points [i],dir);
				}
				if (dir == Direction.R) {
					card.transform.SetSiblingIndex (0);
				}
				cards.Add (card);
			}
			cgo.PGC.Add (cards);

		}
		public int findIndexInPGC(Direction dir,int cardPoint){
			var PGC = getCardGOs (dir).PGC;
			for (int i = 0; i < PGC.Count; i++) {
				var list = PGC [i];
				var a = list [0].GetComponent<PutoutCardView> ().getPoint ();
				var b = list [1].GetComponent<PutoutCardView> ().getPoint ();
				if (a == cardPoint && b == a) {
					return i;
				}
			}
			return -1;
		}
		private Vector3 _getPGCPosition(Direction dir,int k,int j,int count){
			Vector3 position = Vector3.one;
			if (dir == Direction.B) {
				position = new Vector3 (-370f + count * 190f + k * 60f, j * 24);
			} else if (dir == Direction.T) {
				position = new Vector3 (251 - count * 120f + k * 37, j * 20);
			} else if (dir == Direction.L) {
				position = new Vector3 (0f, 122 - count * 95f - k * 28f + j * 10);
			} else if (dir == Direction.R) {
				position = new Vector3 (0, -122 + count * 95 + k * 28f + j * 5);
			}
			return position;
		}

		
		public void addPickCard (int avatarIndex, int cardPoint = -1)
		{
			Direction dir = _data.toGameDir (avatarIndex);
			Vector3 position = new Vector3 (0, 0);
			switch (dir) {
			case Direction.B:
				var cvo = getCardGOs (dir);
				var card = GameObject.Instantiate (Resources.Load ("prefab/card/Bottom_B")) as GameObject;

				card.name = "pickCardItem";
				card.transform.SetParent (cvo.HandParent);
				card.transform.localScale = new Vector3 (1.1f, 1.1f, 1);
				card.transform.localPosition = new Vector3 (580f, -292f);
				card.GetComponent<bottomScript> ().onMyHandCardPutout += onMyHandCardPutout;
				card.GetComponent<bottomScript> ().onMyHandCardSelectedChange += onMyHandCardSelectedChange;
				card.GetComponent<bottomScript> ().setPoint (cardPoint); 

				var Hand = cvo.Hand;
				for (int i = 0; i < Hand.Count; i++) {
					int cc = Hand [i].GetComponent<bottomScript> ().getPoint ();
					if (cc >= cardPoint) {
						Hand.Insert (i, card);
						return;
					}
				}
				Hand.Add (card);

				return;
			case Direction.T://上
				position = new Vector3 (-273, 0f);
				break;
			case Direction.L://左
				position = new Vector3 (0, -173f);

				break;
			case Direction.R://右
				position = new Vector3 (0, 183f);
				break;
			}

			String path = "prefab/card/Bottom_" + dir.ToString ();
			Debug.Log ("path  = " + path);
			otherPickCardItem = newGameObject (path, getCardGOs (dir).HandParent, position);

		}

		public void removePickCard ()
		{
			if (otherPickCardItem != null) {

				GameObject.Destroy (otherPickCardItem);
				otherPickCardItem = null;

			} else {
//				int dirIndex = getIndexByDir (getDirection (curAvatarIndex));
//				GameObject obj = handCardList [dirIndex] [0];
//				handCardList [dirIndex].RemoveAt (0);
//				Destroy (obj);

			}
		}

		/**提示用，打出的牌，1秒后会自动销毁**/
		public GameObject addPutoutCard (int cardPoint, Direction dir)
		{
			Vector3 position = new Vector3 (0, 0);

			switch (dir) {
			case Direction.T: //上
				position = new Vector3 (0, 130f);
				break;
			case Direction.L: //左
				position = new Vector3 (-370, 0f);
				break;
			case Direction.R: //右
				position = new Vector3 (420f, 0f);
				break;
			case Direction.B:
				position = new Vector3 (0, -100f);
				break;
			}

			GameObject card = newGameObject ("Prefab/card/PutOutCard", _host.transform, position);
			card.name = "putOutCard";
			card.transform.localScale = Vector3.one;
			card.GetComponent<PutoutCardView> ().setPoint (cardPoint,Direction.B);

			if (card != null) {
				GameObject.Destroy (card, 1f);
			}

			putOutCard = card;
			return card;
		}

		/**在牌上加指标**/
		public void addPointer (GameObject parent)
		{
			if (parent != null) {
				if (_pointerGO == null) {
					_pointerGO = GameObject.Instantiate (Resources.Load ("Prefab/Pointer")) as GameObject;
				}
				_pointerGO.transform.SetParent (parent.transform);
				_pointerGO.transform.localScale = Vector3.one;
				_pointerGO.transform.localPosition = new Vector3 (0f, parent.transform.GetComponent<RectTransform> ().sizeDelta.y / 2 + 10);
			}
		}

		/**往桌子上打出一张牌**/
		public GameObject addCardToTable (Direction dir, int cardpoint)//
		{
			GameObject card = null;
			String path = "";
			Vector3 position = Vector3.one;
			var cgo = getCardGOs (dir);
			var count = cgo.Table.Count;
			if (dir == Direction.B) {
				path = "Prefab/ThrowCard/TopAndBottomCard";
				position = new Vector3 (-261 + count % 14 * 37, (int)(count / 14) * 67f);
			} else if (dir == Direction.R) {
				path = "Prefab/ThrowCard/ThrowCard_R";
				position = new Vector3 ((int)(-count / 13 * 54f), -180f + count % 13 * 28);
			} else if (dir == Direction.T) {
				path = "Prefab/ThrowCard/TopAndBottomCard";
				position = new Vector3 (289f - count % 14 * 37, -(int)(count / 14) * 67f);
			} else if (dir == Direction.L) {
				path = "Prefab/ThrowCard/ThrowCard_L";
				position = new Vector3 (count / 13 * 54f, 152f - count % 13 * 28);
			}

			card = newGameObject (path, cgo.TableParent, position);
			card.GetComponent<PutoutCardView> ().setPoint (cardpoint,dir);

			cgo.Table.Add (card);
			card.GetComponent<PutoutCardView> ().setOwnerList (cgo.Table);
			if (dir == Direction.R) {
				card.transform.SetSiblingIndex (0);
			}

			return card;
		}

		/**把最后出的一张牌销毁，用于别人吃碰杠胡**/
		public void removeLastPutoutCard ()
		{
			GameObject card = lastCardOnTable;
			if (card != null) {
				List<GameObject> ownerList = card.GetComponent<PutoutCardView> ().getOwnerList ();
				if (ownerList != null && ownerList.Contains (card)) {
					ownerList.Remove (card);
				}
				GameObject.Destroy (card);
			}
			if (putOutCard != null) {
				GameObject.Destroy (putOutCard);
			}
		}



		private GameObject newGameObject (string path, Transform parent, Vector3 position)
		{
			GameObject obj = GameObject.Instantiate (Resources.Load (path)) as GameObject;
			obj.transform.SetParent (parent);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = position;
			return obj;
		}

		/**清理游戏过程中产生的牌**/
		public void clean ()
		{
			if (putOutCard != null) {
				GameObject.Destroy (putOutCard);
				putOutCard = null;
			}

			if (lastCardOnTable != null) {
				lastCardOnTable = null;
			}

			for (int i = 0; i < 4; i++) {
				CardGOs cgo = allCardGOs [i];
				destroyListGO (cgo.Hand);
				destroyListGO (cgo.Table);
				destroyListListGO (cgo.PGC);
				cgo.PlayerItem.clean ();
				GameObject.Destroy (cgo.PlayerItem.gameObject);
				GameObject.Destroy (cgo.PlayerItem);
			}
//			if (mineList != null) {
//				mineList.Clear ();
//			}


			if (pickCardItem != null) {
				GameObject.Destroy (pickCardItem);
			}

			if (otherPickCardItem != null) {
				GameObject.Destroy (otherPickCardItem);
			}

		}

		private void destroyListListGO (List<List<GameObject>> list)
		{
			if (list != null) {
				while (list.Count > 0) {
					List<GameObject> tempList = list [0];
					list.RemoveAt (0);
					destroyListGO (tempList);
				}
			}
		}

		private void destroyListGO (List<GameObject> list)
		{
			if (list != null) {
				while (list.Count > 0) {
					GameObject temp = list [0];
					list.RemoveAt (0);
					GameObject.Destroy (temp);
				}
			}
		}


		public void updateRemainCardNum ()
		{
			_data.remainCardNum--;
			if (_data.remainCardNum < 0) {
				_data.remainCardNum = 0;
			}
			_host.LeavedCastNumText.text = _data.remainCardNum + "";
		}

		public void initRemainCardNum ()
		{
			
			RoomVO rvo = GlobalData.getInstance ().roomVO;

			var type = rvo.roomType;
			if (type == GameType.ZHUAN_ZHUAN) {//转转麻将
				_data.remainCardNum = 108;
				if (rvo.hong) {
					_data.remainCardNum = 112;
				}
			} else if (type == GameType.HUA_SHUI) {//划水麻将
				_data.remainCardNum = 108;
				if (rvo.addWordCard) {
					_data.remainCardNum = 136;
				}
			} else if (type == GameType.GI_PING_HU) {
				_data.remainCardNum = 136;
			}
			_data.remainCardNum = _data.remainCardNum - 53;
			_host.LeavedCastNumText.text = (_data.remainCardNum) + "";

		}
		/// <summary>
		/// 设置红色箭头的显示方向
		/// </summary>
		public void setDirectPointer (int avatarIndex) //设置方向
		{
			//UpateTimeReStart();
			for (int i = 0; i < _host.dirGameList.Count; i++) {
				_host.dirGameList [i].SetActive (false);
			}
			_host.dirGameList [(int)_data.toGameDir(avatarIndex)].SetActive (true);
		}
		/**左上角显示房间规则**/
		public void updateRule (Text tf)
		{
			RoomVO rvo = GlobalData.getInstance ().roomVO;
			string str = "房间号：\n" + rvo.roomId + "\n";
			str += "圈数：" + rvo.roundNumber + "\n";

			if (rvo.roomType == GameType.GI_PING_HU) {
				str += "鸡平胡\n";
			} else {

				if (rvo.roomType == GameType.ZHUAN_ZHUAN) {
					if (rvo.hong) {
						str += "红中麻将\n";
					} else {
						str += "转转麻将\n";
					}

				} else if (rvo.roomType == GameType.HUA_SHUI) {
					str += "划水麻将\n";
				}
				if (rvo.ziMo == 1) {
					str += "只能自摸\n";
				} else {
					str += "可抢杠胡\n";
				}
				if (rvo.sevenDouble && rvo.roomType != GameType.HUA_SHUI) {
					str += "可胡七对\n";
				}

				if (rvo.addWordCard) {
					str += "有风牌\n";
				}
				if (rvo.xiaYu > 0) {
					str += "下鱼数：" + rvo.xiaYu + "";
				}

				if (rvo.ma > 0) {
					str += "抓码数：" + rvo.ma + "";
				}
			}
			if (rvo.magnification > 0) {
				str += "倍率：" + rvo.magnification + "";
			}
			tf.text = str;
		}
	}

	public class CardGOs
	{
		public List<GameObject> Hand;
		//		public List<GameObject> Peng;
		//		public List<GameObject> Gang;
		//		public List<GameObject> Chi;
		public List<List<GameObject>> PGC;
		public List<GameObject> Table;

		public Transform HandParent;
		public Transform PGCParent;
		public Transform TableParent;

		public PlayerItemView PlayerItem;

		public CardGOs ()
		{
			Hand = new List<GameObject> ();
			//			Peng = new List<GameObject> ();
			//			Gang = new List<GameObject> ();
			//			Chi = new List<GameObject> ();
			PGC = new List<List<GameObject>> ();
			Table = new List<GameObject> ();
		}
	}
}

