using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AssemblyCSharp
{
	public class GEndCellRenderView :MonoBehaviour
	{
		public Text nickName;
		public Text ID;
		public Image icon;
		public GameObject winer;
		public GameObject paoshou;
		public Text zimoCount;
		public Text jiepaoCount;
		public Text dianpaoCount;
		public Text angangCount;
		public Text minggangCount;
		public Text finalScore;
		public GameObject mainImgFlag;
		private string headIcon;
		public GEndCellRenderView ()
		{
		}

		public void setUI(FinalGameEndItemVo itemData){
			if (itemData.getIsMain ()) {
				mainImgFlag.SetActive (true);
			} else {
				mainImgFlag.SetActive (false);
			}
			nickName.text =itemData.getNickname ();
			ID.text ="ID:" +itemData.uuid.ToString();
			if (itemData.getIsWiner() && itemData.scores>0) {
				winer.SetActive (true);
			}
			if (itemData.getIsPaoshou() &&  itemData.dianpao>0) {
				paoshou.SetActive (true);
			}

			zimoCount.text = itemData.zimo.ToString ();
			jiepaoCount.text = itemData.jiepao.ToString ();
			dianpaoCount.text = itemData.dianpao.ToString ();
			angangCount.text = itemData.angang.ToString ();
			minggangCount.text = itemData.minggang.ToString ();
			finalScore.text = itemData.scores.ToString ();
			headIcon = itemData.getIcon ();
			StartCoroutine (LoadImg());

		}


		private IEnumerator LoadImg() { 
			
			WWW www = new WWW(headIcon);
			yield return www;
			if (string.IsNullOrEmpty (www.error)) {
				Texture2D texture2D = www.texture;
				//byte[] bytes = texture2D.EncodeToPNG();
				Sprite tempSp = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));
				icon.sprite = tempSp;
			}

		}

	}
}

