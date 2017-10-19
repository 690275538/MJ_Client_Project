using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;


public class UserInfoView : MonoBehaviour {
	public Image headIcon;
	public Text Ip;
	public Text ID;
	public Text NickName;

	private Texture2D texture2D;
	private string headIconPath;

	public void setUIData(AvatarVO  userInfo){
		headIconPath = userInfo.account.headicon;
		Ip.text = "IP:"+ userInfo.IP;
		ID.text = "ID:"+  userInfo.account.uuid +"";
		NickName.text ="昵称:"+ userInfo.account.nickname;
		StartCoroutine (LoadImg());

	}



	private IEnumerator LoadImg() { 
		if (headIconPath != null && headIconPath != "") {
			WWW www = new WWW(headIconPath);
			yield return www;
			if(string.IsNullOrEmpty(www.error)) {
				texture2D = www.texture;
//				byte[] bytes = texture2D.EncodeToPNG();
				Sprite tempSp = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));
				headIcon.sprite = tempSp;
			}
			
		}
	}

	public void close(){
		Destroy (gameObject);
	}
}
