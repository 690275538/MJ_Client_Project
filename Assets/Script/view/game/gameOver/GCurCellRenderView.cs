using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;


public class GCurCellRenderView : MonoBehaviour
{

	public Text nickName;
	public Text resultDes;
	public Text totalScroe;
	public Text fanCount;
	public Text gangScore;
	public GameObject paiArrayPanel;

	public GameObject zhongMaFlag;
	public GameObject huFlag;
	public GameObject GenzhuangFlag;


	/**码牌数组**/
	private List<int> validMas;
	private HupaiResponseItem _itemData;


	public void setUI (HupaiResponseItem itemData, List<int> validMas, int mainuuid)
	{
		_itemData = itemData;
		this.validMas = validMas;

		nickName.text = itemData.nickname;
		totalScroe.text = itemData.totalScore.ToString ();
		gangScore.text = itemData.gangScore.ToString ();
		fanCount.text = itemData.fanScore.ToString ();

		huFlag.SetActive (false);
		zhongMaFlag.SetActive (false);
		if (itemData.totalInfo.genzhuang == "1" && itemData.uuid == mainuuid) {
			GenzhuangFlag.SetActive (true);
		} else {
			GenzhuangFlag.SetActive (false);
		}

		string mdesCribe = "";
		TotalInfo totalInfo = _itemData.totalInfo;
		int[] paiArray = _itemData.paiArray;

		var gangs = totalInfo.getGangInfos ();
		for (int i = 0; i < gangs.Length; i++) {
			var info = gangs [i];
			paiArray [info.cardPoint] -= 4;
			if (info.type == "an") {
				mdesCribe += "暗杠  ";
			} else {
				mdesCribe += "明杠  ";
			}
		}

		var pengs = totalInfo.getPengInfos ();
		for (int i = 0; i < pengs.Length; i++) {
			var info = pengs [i];
			if (paiArray [info.cardPoint] >= 3) {
				paiArray [info.cardPoint] -= 3;
			} else {
				pengs [i] = null;
			}
		}

		var chis = totalInfo.getChiInfos ();
		for (int i = 0; i < chis.Length; i++) {
			var info = chis [i];
			paiArray [info.cardPionts [0]] -= 1;
			paiArray [info.cardPionts [1]] -= 1;
			paiArray [info.cardPionts [2]] -= 1;
		}
			

		var hu = totalInfo.getHuInfo ();
		if (hu != null) {

			mdesCribe += GameHelper.getHelper ().getHuString (hu.type);
			if (! hu.type.Contains ("d_other")) {
				huFlag.SetActive (true);
				paiArray [hu.cardPiont] -= 1;
			}
		}

		if (_itemData.huType != null) {
			mdesCribe += _itemData.huType;
		}
			
		resultDes.text = mdesCribe;

		//显示手牌
		float startPosition = 30f;
		GameObject card;

		for (int i = 0; i < gangs.Length; i++) {
			TGangInfo info = gangs [i];
			if (info.type == "an") {
				for (int j = 0; j < 3; j++) {
					createCard (info.cardPoint, startPosition,true);
					startPosition += 36f;
				}
				createCard (info.cardPoint, startPosition);
				startPosition += 36f;
			} else {
				for (int j = 0; j < 4; j++) {
					createCard (info.cardPoint, startPosition);
					startPosition += 36f;
				}
			}
		}
		startPosition = startPosition + (gangs.Length > 0 ? 8f : 0f);

		var ii = 0;
		for (int i = 0; i < pengs.Length; i++) {
			if (pengs [i] != null) {
				for (int j = 0; j < 3; j++) {
					createCard (pengs [i].cardPoint, startPosition);
					startPosition += 36f;
				}
			} else {
				ii++;
			}
		}
		startPosition = startPosition + (pengs.Length-ii > 0 ? 8f : 0f);

		for (int i = 0; i < chis.Length; i++) {
			TChiInfo info = chis [i];
			for (int j = 0; j < 3; j++) {
				createCard (info.cardPionts[j], startPosition);
				startPosition += 36f;

			}
		}
		startPosition = startPosition + (chis.Length > 0 ? 8f : 0f);

		for (int i = 0; i < paiArray.Length; i++) {
			
			if (paiArray [i] > 0) {

				for (int j = 0; j < paiArray [i]; j++) {
					createCard (i, startPosition);
					startPosition += 36f;
				}

			}

		}
		startPosition += 8f;

		if (hu != null) {
			if (GameHelper.getHelper().isHu(hu.type)) {

				createCard (hu.cardPiont, startPosition);
			}
			startPosition = startPosition + 36f + 52f;
		} else {
			startPosition = startPosition + 52f;
		}

		List<int> maPais = _itemData.getMaPoints ();
		bool flag = false;
		if (maPais != null && maPais.Count > 0) {
			for (int i = 0; i < maPais.Count; i++) {
				flag = flag || isMaValid (maPais [i]);

				card = Instantiate (Resources.Load ("Prefab/ThrowCard/ZhongMa")) as GameObject;
				card.transform.SetParent( paiArrayPanel.transform);
				card.GetComponent<PutoutCardView> ().setPoint (maPais [i]);
				card.transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
				card.transform.localPosition = new Vector3 ((20 + i) * 36f, 0, 0);
			}
			zhongMaFlag.SetActive (flag);
		}

		if (GlobalData.getInstance ().roomVO.roomType == GameType.HUA_SHUI) {
			card = Instantiate (Resources.Load ("Prefab/Image_yu")) as GameObject;
			card.transform.SetParent( paiArrayPanel.transform);
			card.GetComponent<yuSetScript> ().setCount (GlobalData.getInstance ().roomVO.xiaYu);
			card.transform.localScale = Vector3.one;
			card.transform.localPosition = new Vector3 (20 * 36f, 0, 0);
		}

	}

	private void createCard (int cardPoint, float startPosition,bool isback=false)
	{
		string path = isback ? "Prefab/PengGangCard/GangBack_T" : "Prefab/ThrowCard/TopAndBottomCard";
		GameObject card = Instantiate (Resources.Load (path)) as GameObject;
		card.transform.SetParent( paiArrayPanel.transform);
		//itemTemp.transform.localScale = new Vector3(0.8f,0.8f,1f);
		card.transform.localScale = Vector3.one;
		if (!isback)
			card.GetComponent<PutoutCardView> ().setPoint (cardPoint);

		card.transform.localPosition = new Vector3 (startPosition, 0, 0);
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




}
