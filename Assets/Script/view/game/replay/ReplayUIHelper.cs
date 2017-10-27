using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class ReplayUIHelper
	{
		public ReplayUIHelper ()
		{
		}
		private GameObject _pointerGO;
		/**自己摸到的牌**/
		//		private GameObject pickCardItem;
		public GameObject lastCardOnTable;
		private GameObject otherPickCardItem;
		private ReplayData _data;
		private List<RCardGOs> allCardGOs;
		ReplayView _host;

		public void init (ReplayView host)
		{
			_host = host;
			_data = _host.Data;
			allCardGOs = new List<RCardGOs> (4);
			for (int i = 0; i < 4; i++) {
				RCardGOs cgo = new RCardGOs ();
				cgo.HandParent = host.HandParents [i];
				cgo.TableParent = host.TableParents [i];
				cgo.PGCParent = host.PGCParents [i];
				cgo.PlayerItem = host.PlayerItemViews [i];
				allCardGOs.Add (cgo);
			}
		}
		public void start (){
			for (int i = 0; i < _data.AvatarList.Count; i++) {
				var cgo = getCardGOs (i);
				Direction dir = _data.toGameDir(i);
				//avatar
				ReplayAvatarVO rvo = _data.AvatarList [i];
				cgo.PlayerItem.setAvatarVo (rvo);


				int[] tempPai = rvo.getPaiArray ();
				for (int a = 0; a <tempPai.Length; a++) {
					if (tempPai [a] > 0) {
						GameObject temp = null;
						for (int b = 0; b < tempPai [a]; b++) {
							temp = newGameObject ("Prefab/playBack/HandCard_"+ dir.ToString(), cgo.HandParent, Vector3.one);
							temp.GetComponent<PutoutCardView> ().setPoint (a ,_data.toGameDir(i) );

							if (dir == Direction.R) {
								temp.transform.SetSiblingIndex (0);
							}
							cgo.Hand.Add (temp);
						}
					}
				}

			}
		}
		public void rangeHandCard(){
			for (int i = 0; i < 4; i++) {
				var cgo = allCardGOs [i];
				int len = cgo.Hand.Count;
				for (int a = 0; a <len; a++) {
					int tempNum = len - a-1;
					switch (i) {
					case 0:
						cgo.Hand[a].transform.localPosition = new Vector3 (410 - tempNum * 79, 0);
						break;
					case 1:
						cgo.Hand[a].transform.localPosition = new Vector3 (0, 200 - tempNum * 32);
						cgo.Hand[a].transform.SetSiblingIndex (0);
						break;
					case 2:
						cgo.Hand[a].transform.localPosition = new Vector3 (-190 + tempNum * 55, 0);
						break;
					case 3:
						cgo.Hand[a].transform.localPosition = new Vector3 (0,-140+tempNum*32);
						break;
					}
				}
			}
		}
		public void huCard (int avatarIndex, int cardPoint)
		{
			var cgo = getCardGOs (avatarIndex);
			cgo.PlayerItem.showHuEffect ();
			SoundManager.getInstance ().playSoundByAction ("hu", cgo.PlayerItem.getSex ());
		}
		public void gangCard (int avatarIndex, int cardPoint, int gangType)
		{
			var cgo = getCardGOs (avatarIndex);

			cgo.PlayerItem.showGangEffect ();
			SoundManager.getInstance ().playSoundByAction ("gang", cgo.PlayerItem.getSex ());

			if (gangType == 4) {
				removeHandCard (avatarIndex, cardPoint, 4);
			} else if (gangType == 1) {//明杠别人
				addPGCCards (avatarIndex, cardPoint, 2);
				removeLastPutoutCard ();
				removeHandCard (avatarIndex, cardPoint, 3);
			} else if (gangType == 3) {//先碰后杠
				addPGCCards (avatarIndex, cardPoint, 5);
				removeHandCard (avatarIndex, cardPoint, 1);
			} else {//暗
				addPGCCards (avatarIndex, cardPoint, 3);
				removeHandCard (avatarIndex, cardPoint, 4);
			}
			rangeHandCard ();
		}
		public void pengCard (int avatarIndex, int cardPoint)//其他人碰牌
		{
			var cgo = getCardGOs (avatarIndex);

			cgo.PlayerItem.showPengEffect ();
			SoundManager.getInstance ().playSoundByAction ("peng", cgo.PlayerItem.getSex ());


			removeLastPutoutCard ();
			removeHandCard (avatarIndex, cardPoint, 2);
			rangeHandCard ();


			addPGCCards (avatarIndex, cardPoint, 1);


		}
		/**type:1碰2明杠3暗杠4吃5先碰后杠**/
		private void addPGCCards (int avatarIndex, int cardPoint, int type)
		{
			Direction dir = _data.toGameDir (avatarIndex);
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
				int index = findIndexInPGC (avatarIndex, cardPoint);
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
				points = new int[]{ cardPoint , cardPoint + 1, cardPoint + 2 };
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
				if (i < 3 && dir == Direction.R) {
					card.transform.SetSiblingIndex (0);
				}
				cards.Add (card);
			}
			cgo.PGC.Add (cards);

		}
		public int findIndexInPGC(int avatarIndex,int cardPoint){
			var PGC = getCardGOs (avatarIndex).PGC;
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
				position = new Vector3 (251 - count * 120f + k * 37, j * 13);
			} else if (dir == Direction.L) {
				position = new Vector3 (0f, 122 - count * 95f - k * 28f + j * 15);
			} else if (dir == Direction.R) {
				position = new Vector3 (0, -122 + count * 95 + k * 28f + j * 13);
			}

//			switch (avaIndex) {
//			case 0:
//				tempvector3 = new Vector3 (-380 + pengGangLists [avaIndex].Count * 200 + i * 60, 0);
//				break;
//			case 1:
//				tempvector3 = new Vector3 (0, -116 + pengGangLists [avaIndex].Count * 90 + i * 26f);
//				break;
//			case 2:
//				tempvector3 = new Vector3 (231 - pengGangLists [avaIndex].Count * 120f + i * 37, 0, 0);
//				break;
//			case 3:
//				tempvector3 = new Vector3 (0, 142 - pengGangLists [avaIndex].Count * 90f - i * 26f, 0);
//				break;
//			}
			return position;
		}



		private void removeHandCard(int avatarIndex, int cardPoint,int num){
			var Hand = getCardGOs (avatarIndex).Hand;
			for (int i = 0; i < Hand.Count; i++) {
				GameObject card = Hand [i];
				int point = card.GetComponent<PutoutCardView> ().getPoint ();
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
//			if (putOutCard != null) {
//				GameObject.Destroy (putOutCard);
//			}
		}
		public void pickCard (int avatarIndex, int cardPoint)
		{
			var dir = _data.toGameDir (avatarIndex);
			var cgo = getCardGOs (avatarIndex);

			GameObject card = newGameObject ("Prefab/playBack/HandCard_" + dir.ToString(), cgo.HandParent, Vector3.one);
			card.GetComponent<PutoutCardView> ().setPoint (cardPoint,dir);

			switch (dir) {
			case Direction.B:
				card.transform.localPosition = new Vector3 (520, 0);
				break;
			case Direction.R:
				card.transform.localPosition = new Vector3 (0, 250);
				break;
			case Direction.T:
				card.transform.localPosition = new Vector3 (-260, 0);
				break;
			case Direction.L:
				card.transform.localPosition = new Vector3 (0, -190);
				break;
			}

			cgo.Hand.Add (card);
		}
		public void qiangGangHu (int avatarIndex, int cardPoint)
		{
			pickCard (avatarIndex, cardPoint);

			var cgo = getCardGOs (avatarIndex);
			cgo.PlayerItem.showHuEffect ();
			SoundManager.getInstance ().playSoundByAction ("hu", cgo.PlayerItem.getSex ());

		}
		public void putoutCard (int avatarIndex, int cardPoint)
		{
			var cgo = getCardGOs (avatarIndex);
			List<GameObject> Hand = cgo.Hand;
			GameObject lastCard = Hand [Hand.Count - 1];
			int lastPoint = lastCard.GetComponent<PutoutCardView> ().getPoint ();
			if (cardPoint == lastPoint) {
				Hand.Remove (lastCard);
				GameObject.Destroy (lastCard);
			} else {
				for (int i = 0; i < Hand.Count; i++) {
					int tempPoint = Hand [i].GetComponent<PutoutCardView> ().getPoint ();
					if (tempPoint == cardPoint) {
						GameObject card = Hand [i];
						Hand.RemoveAt (i);
						GameObject.Destroy (card);
						break;
					}
				}
				paiSort (Hand);
				rangeHandCard ();
			}
			addPutoutCardEffect (avatarIndex, cardPoint);

			lastCardOnTable = addCardToTable (avatarIndex, cardPoint);
		}
		private void paiSort (List<GameObject> Hand)
		{
			GameObject lastCard = Hand [Hand.Count - 1];
			Hand.Remove (lastCard);
			int lastPoint = lastCard.GetComponent<PutoutCardView> ().getPoint ();
			for (int i = 0; i < Hand.Count; i++) {
				int curPoint = Hand [i].GetComponent<PutoutCardView> ().getPoint ();
				if (lastPoint <= curPoint) {
					Hand.Insert (i, lastCard);
					return;
				}
			}
			Hand.Add (lastCard);
		}

		/**提示用，打出的牌，1秒后会自动销毁**/
		private void addPutoutCardEffect (int avatarIndex,int cardPoint)
		{
			var cgo = getCardGOs (avatarIndex);
			SoundManager.getInstance ().playSound (cardPoint, cgo.PlayerItem.getSex ());

			Vector3 position = new Vector3 (0, 0);
			Direction dir = _data.toGameDir (avatarIndex);
			switch (dir) {
			case Direction.T: //上
				position = new Vector3 (0, -180f);
				break;
			case Direction.L: //左
				position = new Vector3 (100, 0f);
				break;
			case Direction.R: //右
				position = new Vector3 (-100, 0f);
				break;
			case Direction.B:
				position = new Vector3 (0, 180);
				break;
			}


			GameObject card = newGameObject ("Prefab/card/PutOutCard", cgo.HandParent, position);
			card.name = "putOutCard";
			card.transform.localScale = Vector3.one;
			card.GetComponent<PutoutCardView> ().setPoint (cardPoint);

			GameObject.Destroy (card, 1f);



		}
		/**往桌子上打出一张牌**/
		public GameObject addCardToTable (int avatarIndex, int cardpoint)//
		{
			Direction dir = _data.toGameDir(avatarIndex);
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


		public RCardGOs getCardGOs (Direction dir)
		{
			return allCardGOs [(int)dir];
		}

		public RCardGOs getCardGOs (int avatarIndex)
		{
			var dir = _data.toGameDir (avatarIndex);
			return getCardGOs (dir);
		}
		private GameObject newGameObject(string path,Transform parent,Vector3 position)
		{
			GameObject obj = GameObject.Instantiate(Resources.Load(path)) as GameObject;
			obj.transform.SetParent (parent);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = position;
			return obj;
		}
	}


	public class RCardGOs
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

		public ReplayPlayerView PlayerItem;

		public RCardGOs ()
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

