using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;

public class LotteryCRView : MonoBehaviour
{
    public Image img;
	public Text nameTxt;
	private string imgUrl;

	private IEnumerator loadImg(){
		if (!string.IsNullOrEmpty(imgUrl)) {
			WWW www = new WWW(Constants.PIC_PATH+ imgUrl);
			yield return www;

			if(string.IsNullOrEmpty(www.error)) {
				Texture2D texture2D = www.texture;
				Sprite sprite = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));
				img.sprite = sprite;
				img.SetNativeSize();
			}
		}
	}

	public void setData(GiftItemVO gvo){

		nameTxt.text = gvo.prizeName;
		imgUrl = gvo.imageUrl;
		StartCoroutine (loadImg());
	}




  
}
