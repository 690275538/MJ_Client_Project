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

		public UIHelper ()
		{
		}

		public GameObject addPutoutCard(int cardPoint,Direction dir,Transform parent){
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

			GameObject card = newGameObject ("Prefab/card/PutOutCard", parent, position);
			card.name = "putOutCard";
			card.transform.localScale = Vector3.one;
			card.GetComponent<PutoutCardView> ().setPoint (cardPoint);

			if (card != null) {
				GameObject.Destroy (card, 1f);
			}

			putOutCard = card;
			return card;
		}

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
		public GameObject addCardToTable (int cardpoint, Direction dir, List<List<GameObject>> outChildren, List<Transform> outParents)//
		{
			GameObject card = null;
			String path = "";
			Vector3 position = Vector3.one;

			if (dir == Direction.B) {
				path = "Prefab/ThrowCard/TopAndBottomCard";
				position = new Vector3 (-261 + outChildren [0].Count % 14 * 37, (int)(outChildren [0].Count / 14) * 67f);
			} else if (dir == Direction.R) {
				path = "Prefab/ThrowCard/ThrowCard_R";
				position = new Vector3 ((int)(-outChildren [1].Count / 13 * 54f), -180f + outChildren [1].Count % 13 * 28);
			} else if (dir == Direction.T) {
				path = "Prefab/ThrowCard/TopAndBottomCard";
				position = new Vector3 (289f - outChildren [2].Count % 14 * 37, -(int)(outChildren [2].Count / 14) * 67f);
			} else if (dir == Direction.L) {
				path = "Prefab/ThrowCard/ThrowCard_L";
				position = new Vector3 (outChildren [3].Count / 13 * 54f, 152f - outChildren [3].Count % 13 * 28);
			}

			card = newGameObject (path, outParents [(int)dir], position);
			if (dir == Direction.R || dir == Direction.L) {
				card.GetComponent<PutoutCardView> ().setLefAndRightPoint (cardpoint);
			} else {
				card.GetComponent<PutoutCardView> ().setPoint (cardpoint);
			}

			outChildren [(int)dir].Add (card);
			card.GetComponent<PutoutCardView> ().setOwnerList (outChildren [(int)dir]);
			if (dir == Direction.R) {
				card.transform.SetSiblingIndex (0);
			}

			return card;
		}
		public void removeCardFromTable(){
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

			
		public void clean(){
			if (putOutCard != null) {
				GameObject.Destroy (putOutCard);
				putOutCard = null;
			}

			if (lastCardOnTable != null) {
				lastCardOnTable = null;
			}
		}





		public void updateRule (Text tf)
		{
			RoomVO roomvo = GlobalData.getInstance ().roomVO;
			string str = "房间号：\n" + roomvo.roomId + "\n";
			str += "圈数：" + roomvo.roundNumber + "\n";

			if (roomvo.roomType == GameType.GI_PING_HU) {
				str += "鸡平胡\n";
			} else {

				if (roomvo.roomType == GameType.ZHUAN_ZHUAN) {
					if (roomvo.hong) {
						str += "红中麻将\n";
					} else {
						str += "转转麻将\n";
					}

				} else if (roomvo.roomType == GameType.HUA_SHUI) {
					str += "划水麻将\n";
				}
				if (roomvo.ziMo == 1) {
					str += "只能自摸\n";
				} else {
					str += "可抢杠胡\n";
				}
				if (roomvo.sevenDouble && roomvo.roomType != GameType.HUA_SHUI) {
					str += "可胡七对\n";
				}

				if (roomvo.addWordCard) {
					str += "有风牌\n";
				}
				if (roomvo.xiaYu > 0) {
					str += "下鱼数：" + roomvo.xiaYu + "";
				}

				if (roomvo.ma > 0) {
					str += "抓码数：" + roomvo.ma + "";
				}
			}
			if (roomvo.magnification > 0) {
				str += "倍率：" + roomvo.magnification + "";
			}
			tf.text = str;
		}
	}
}

