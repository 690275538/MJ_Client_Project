using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;

public class GangpaiObj
{
	public int cardPiont;
//出牌的下标
	public string uuid;
//出牌的玩家
	public string type;
}

//public class PengpaiObj{
//public string cardPoints;//出牌的玩家
//}

public class HuipaiObj
{
	public int cardPiont;
//出牌的下标
	public string uuid;
	public string type;
}

public class ChipaiObj
{
	public string[] cardPionts;
//出牌的下标

}



/**
 * 
 * 
 */ 
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



	private List<GangpaiObj> gangPaiList = new List<GangpaiObj> ();
//杠牌列表
	private string[] pengpaiList;
//碰牌列表
	private List<ChipaiObj> chipaiList = new List<ChipaiObj> ();
//吃牌列表
	private List<int> maPais;
//码牌数组

	private HuipaiObj hupaiObj = new HuipaiObj ();
//胡牌列表


	private string mdesCribe = "";
//对结果展示字符串
	private int[] paiArray;
//牌列表

	private List<int> validMas;
	private HupaiResponseItem _itemdata;


	public void setUI (HupaiResponseItem itemData, List<int> validMasParm, int mainuuid)
	{
		_itemdata = itemData;
		validMas = validMasParm;

		nickName.text = itemData.nickname;
		totalScroe.text = itemData.totalScore + "";
		gangScore.text = itemData.gangScore + "";

		paiArray = itemData.paiArray;
		huFlag.SetActive (false);
		zhongMaFlag.SetActive (false);
		if (itemData.totalInfo.genzhuang == "1" && itemData.uuid == mainuuid) {
			GenzhuangFlag.SetActive (true);
		} else {
			GenzhuangFlag.SetActive (false);
		}

		analysisPaiInfo (itemData);

	}


	TotalInfo totalInfo;

	private void analysisPaiInfo (HupaiResponseItem itemData)
	{
		string str;
		string[] ss;
		string[] s;

		totalInfo = itemData.totalInfo;
		str = totalInfo.gang;
		if (!string.IsNullOrEmpty (str)) {
			ss = str.Split (new char[1]{ ',' });

			for (int i = 0; i < ss.Length; i++) {
				GangpaiObj obj = new GangpaiObj ();
				s = ss [i].Split (new char[1]{ ':' });
				obj.uuid = s [0];
				obj.cardPiont = int.Parse (s [1]);
				obj.type = s [2];
				//增加判断是否为自己的杠牌的操作

				paiArray [obj.cardPiont] -= 4;
				gangPaiList.Add (obj);

				if (obj.type == "an") {
					mdesCribe += "暗杠  ";
				} else {
					mdesCribe += "明杠  ";
				}
			}
		}


		str = totalInfo.peng;
		if (!string.IsNullOrEmpty (str)) {
			ss = str.Split (new char[1]{ ',' });

			List<string> pengpaiListTTT = new List<string> ();
			for (int i = 0; i < ss.Length; i++) {
				int cardPoint = int.Parse (ss [i]);
				if (paiArray [cardPoint] >= 3) {
					paiArray [cardPoint] -= 3;
					pengpaiListTTT.Add (ss [i]);
				}

			}
			pengpaiList = pengpaiListTTT.ToArray ();
		}


		str = totalInfo.chi;
		if (!string.IsNullOrEmpty (str)) {
			ss = str.Split (new char[1]{ ',' });

			for (int i = 0; i < ss.Length; i++) {
				ChipaiObj chipaiObj = new ChipaiObj ();
				s = ss [i].Split (new char[1]{ ':' }); 
				List<string> list = new List<string> (s);
				list.RemoveAt (0);
				s = list.ToArray ();
				chipaiObj.cardPionts = s;
				chipaiList.Add (chipaiObj);
				paiArray [int.Parse (s [0])] -= 1;
				paiArray [int.Parse (s [1])] -= 1;
				paiArray [int.Parse (s [2])] -= 1;
			}

		}



		str = totalInfo.hu;
		if (!string.IsNullOrEmpty (str)) {
			ss = str.Split (new char[1]{ ',' });
			hupaiObj.uuid = ss [0];
			hupaiObj.cardPiont = int.Parse (ss [1]);
			hupaiObj.type = ss [2];
			//增加判断是否是自己胡牌的判断

			if (str.Contains ("d_other")) {//排除一炮多响的情况
				mdesCribe += "点炮";
			} else if (hupaiObj.type == "zi_common") {
				mdesCribe += "自摸";
				huFlag.SetActive (true);
				paiArray [hupaiObj.cardPiont] -= 1;

			} else if (hupaiObj.type == "d_self") {
				mdesCribe += "接炮";
				huFlag.SetActive (true);
				paiArray [hupaiObj.cardPiont] -= 1;
			} else if (hupaiObj.type == "qiyise") {
				mdesCribe += "清一色";
				huFlag.SetActive (true);
				paiArray [hupaiObj.cardPiont] -= 1;
			} else if (hupaiObj.type == "zi_qingyise") {
				mdesCribe += "自摸清一色";
				huFlag.SetActive (true);
				paiArray [hupaiObj.cardPiont] -= 1;
			} else if (hupaiObj.type == "qixiaodui") {
				mdesCribe += "七小对";
				huFlag.SetActive (true);
				paiArray [hupaiObj.cardPiont] -= 1;
			} else if (hupaiObj.type == "self_qixiaodui") {
				mdesCribe += "自摸七小对";
				huFlag.SetActive (true);
				paiArray [hupaiObj.cardPiont] -= 1;
			} else if (hupaiObj.type == "gangshangpao") {
				mdesCribe += "杠上炮";
			} else if (hupaiObj.type == "gangshanghua") {
				mdesCribe += "杠上花";
				huFlag.SetActive (true);
				paiArray [hupaiObj.cardPiont] -= 1;
			}
		}

		if (_itemdata.huType != null) {
			mdesCribe += _itemdata.huType;
		}
			
		resultDes.text = mdesCribe;
		maPais = itemData.getMaPoints ();
		arrangePai ();
	}


	/**整理牌**/
	private void arrangePai ()
	{
		
		float startPosition = 30f;
		GameObject card;

		if (gangPaiList != null) {
			for (int i = 0; i < gangPaiList.Count; i++) {
				GangpaiObj itemgangData = gangPaiList [i];
				for (int j = 0; j < 4; j++) {
					createCard (itemgangData.cardPiont, startPosition);
					startPosition += 36f;
				}
			}
			startPosition = startPosition + (gangPaiList.Count > 0 ? 8f : 0f);
		}



		if (pengpaiList != null) {
			for (int i = 0; i < pengpaiList.Length; i++) {
				string cardPoint = pengpaiList [i];
				for (int j = 0; j < 3; j++) {
					createCard (int.Parse(cardPoint), startPosition);
					startPosition += 36f;
				}
			}
			startPosition = startPosition + (pengpaiList.Length > 0 ? 8f : 0f);

		}



		if (chipaiList != null) {
			for (int i = 0; i < chipaiList.Count; i++) {
				ChipaiObj itemgangData = chipaiList [i];
				for (int j = 0; j < 3; j++) {
					createCard (int.Parse(itemgangData.cardPionts [j]), startPosition);
					startPosition += 36f;

				}
			}
			startPosition = startPosition + (chipaiList.Count > 0 ? 8f : 0f);
		}


		for (int i = 0; i < paiArray.Length; i++) {
			
			if (paiArray [i] > 0) {

				for (int j = 0; j < paiArray [i]; j++) {
					createCard (i, startPosition);
					startPosition += 36f;
				}

			}

		}
		startPosition += 8f;

		if (hupaiObj != null) {
			if (hupaiObj.type == "zi_common" || hupaiObj.type == "d_self" || hupaiObj.type == "qiyise" || hupaiObj.type == "zi_qingyise"
			    || hupaiObj.type == "qixiaodui" || hupaiObj.type == "self_qixiaodui" || hupaiObj.type == "gangshanghua") {

				createCard (hupaiObj.cardPiont, startPosition);
			}
			startPosition = startPosition + 36f + 52f;
		} else {
			startPosition = startPosition + 52f;
		}


		if (maPais != null && maPais.Count > 0) {
			for (int i = 0; i < maPais.Count; i++) {
				card = Instantiate (Resources.Load ("Prefab/ThrowCard/ZhongMa")) as GameObject;
				if (isMaValid (maPais [i])) {
					zhongMaFlag.SetActive (true);
				} else {
					zhongMaFlag.SetActive (false);
				}

				card.transform.parent = paiArrayPanel.transform;
				card.GetComponent<PutoutCardView> ().setPoint (maPais [i]);
				card.transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
				card.transform.localPosition = new Vector3 ((20 + i) * 36f, 0, 0);
			}
		}

		if (GlobalData.getInstance ().roomVO.roomType == GameType.HUA_SHUI) {
			card = Instantiate (Resources.Load ("Prefab/Image_yu")) as GameObject;
			card.transform.parent = paiArrayPanel.transform;
			card.GetComponent<yuSetScript> ().setCount (GlobalData.getInstance ().roomVO.xiaYu);
			card.transform.localScale = Vector3.one;
			card.transform.localPosition = new Vector3 (20 * 36f, 0, 0);
		}




	}
	private void createCard (int cardPoint, float startPosition){

		GameObject card = Instantiate (Resources.Load ("Prefab/ThrowCard/TopAndBottomCard")) as GameObject;
		card.transform.parent = paiArrayPanel.transform;
		//itemTemp.transform.localScale = new Vector3(0.8f,0.8f,1f);
		card.transform.localScale = Vector3.one;
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
