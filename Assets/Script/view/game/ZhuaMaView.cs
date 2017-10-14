using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class ZhuaMaView : MonoBehaviour
	{
		public List<Transform> maParents;
		public List<GameObject> zhongFlags;

		/**中间显示码牌的容器**/
		public GameObject mapaiContaner;
		/**中间显示码牌的背景框**/
		public GameObject mapaiBg;


		private List<int> validMas;
		private List<string> paiList;
		private string uuid;

		private GamingData _data;

		public void init (GamingData data)
		{
			_data = data;
		}


		public void arrageMas (string allMas, List<int> validMasParms)
		{
		
			validMas = validMasParms;
			if (string.IsNullOrEmpty (allMas)) {
				return;
			}
			string[] paiArray = allMas.Split (new char[1]{ ':' });
			paiList = new List<string> (paiArray);
			uuid = paiList [0];
			paiList.RemoveAt (0);
	
			if (paiList.Count == 0) {
				mapaiBg.SetActive (false);
				return;
			}
			float leftMargin = -(paiList.Count * 70f) / 2 + 35f;
			for (int i = 0; i < paiList.Count; i++) {
				GameObject card = Instantiate (Resources.Load ("Prefab/PengGangCard/PengGangCard_B")) as GameObject;
				int cardPoint = int.Parse (paiList [i]);
				card.GetComponent<PutoutCardView> ().setPoint (cardPoint);
				card.transform.SetParent (mapaiContaner.transform);
				card.transform.localScale = Vector3.one;
				card.transform.localPosition = new Vector3 (leftMargin + i * 70f, 0, 0);
			}


			Invoke ("doArrage", 2.5f);


		}

		private bool isMaValid (int cardPonit)
		{
			for (int i = 0; i < validMas.Count; i++) {
				if (cardPonit == validMas [i]) {
					return true;		
				}
			}
			return false;
		}

		private void doArrage ()
		{
			mapaiBg.SetActive (false);

			string[] Normal_PATHS = new string[] {
				"Prefab/PengGangCard/PengGangCard_B",
				"Prefab/PengGangCard/PengGangCard_R",
				"Prefab/PengGangCard/PengGangCard_T",
				"Prefab/PengGangCard/PengGangCard_L"
			};
			int[] positions = new int[]{ 0, 0, 0, 0 };

			int referIndex = _data.toAvatarIndex (int.Parse (uuid));
			for (int i = 0; i < paiList.Count; i++) {
				int cardPoint = int.Parse (paiList [i]);
				int positionIndex = (cardPoint + 1) % 9;
				int resultIndex = 0;
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

				Direction dir = GamingData.ToGameDir (resultIndex,referIndex);
				int gameIndex = (int)dir;
		
				GameObject card;
				if (isMaValid (cardPoint)) {
					zhongFlags [gameIndex].SetActive (true);
				}
				card = Instantiate (Resources.Load (Normal_PATHS [gameIndex])) as GameObject;
				card.GetComponent<PutoutCardView> ().setPoint (cardPoint);
				card.transform.SetParent (maParents [gameIndex]);
				card.transform.localScale = Vector3.one;
				card.transform.localPosition = _getPosition (dir, positions);
				if (dir == Direction.R) {
					card.transform.SetSiblingIndex (0);
				}
			}
		}

		private Vector3 _getPosition (Direction dir, int[] positions)
		{
			Vector3 position = Vector3.one;
			int gameIndex = (int)dir;
			if (dir == Direction.B) {
				position = new Vector3 (-149f + positions [gameIndex] * 60f, 0f, 0);
			} else if (dir == Direction.T) {
				position = new Vector3 (149f - positions [gameIndex] * 60f, 0, 0);
			} else if (dir == Direction.L) {
				position = new Vector3 (0, 140f - positions [gameIndex] * 50f, 0);
			} else if (dir == Direction.R) {
				position = new Vector3 (0f, -140f + positions [gameIndex] * 50f, 0);
			}
			positions [gameIndex]++;
			return position;
		}

	}

}
