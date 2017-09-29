using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemScript : MonoBehaviour {

	static List<string> messageBoxContents = new List<string>();
	static void initMessageBox(){
		if (messageBoxContents.Count == 0) {
			messageBoxContents.Add ("不要吵了，专心玩游戏！");
			messageBoxContents.Add ("不要走，决战到天亮");
			messageBoxContents.Add ("大家好，很高兴见到各位");
			messageBoxContents.Add ("和你合作真是太愉快了");
			messageBoxContents.Add ("快点啊，等得你花儿都谢了!");
			messageBoxContents.Add ("你的牌打得也太好了");
			messageBoxContents.Add ("交个朋友吧");
			messageBoxContents.Add ("下次再玩吧，我要走了");
		}
	}

	public Image headerIcon;
	public Image bankerImg;
	public Text nameText;
	public Image readyImg;
	public Text scoreText;
	public string dir;
	public GameObject chatAction;
	//public Text offline;//离线字样
	public Image offlineImage;//离线图片
	public Text chatMessage;
	public GameObject chatPaoPao;
	public GameObject HuFlag;

	private AvatarVO avatarvo;
	private int showTime;
	private int showChatTime;


	// Use this for initialization
	void Start () {
		//bankerImg.enabled = false;
		//readyImg.enabled = false;
//	    scoreText.text = 1000 + "";
	}
	
	// Update is called once per frame
	void Update () {
		if(showTime >0){
			showTime--;
			if (showTime == 0) {
				chatPaoPao.SetActive (false);
			}
		}

		if(showChatTime >0){
			showChatTime--;
			if (showChatTime == 0) {
				chatAction.SetActive (false);
			}
		}
	}

	public void setAvatarVo(AvatarVO value){
		if (value != null) {
			avatarvo = value;
			readyImg.enabled = avatarvo.isReady;
			bankerImg.enabled = avatarvo.main;
			nameText.text = avatarvo.account.nickname;
			scoreText.text =avatarvo.scores+"";
			offlineImage.transform.gameObject.SetActive (!avatarvo.isOnLine);
			StartCoroutine (LoadImg());

		} else {
			nameText.text = "";
			readyImg.enabled = false;
			bankerImg.enabled = false;
			scoreText.text ="";
			readyImg.enabled = false;

//			SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer> ();
//			Texture2D texture =(Texture2D)Resources.Load ("Image/gift");
//			Sprite sp = Sprite.Create (texture, spr.sprite.textureRect, new Vector2 (0.5f, 0.5f));
			headerIcon.sprite = Resources.Load("Image/morentouxiang",typeof(Sprite)) as Sprite;
		}
	}
	/// <summary>
	/// 加载头像
	/// </summary>
	/// <returns>The image.</returns>
	private IEnumerator LoadImg() { 
		//开始下载图片
		WWW www = new WWW(avatarvo.account.headicon);
		yield return www;
		//下载完成，保存图片到路径filePath
		if (www != null && www.texture != null) {
			Texture2D texture2D = www.texture;
			//byte[] bytes = texture2D.EncodeToPNG ();

			//将图片赋给场景上的Sprite
			Sprite tempSp = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));
			headerIcon.sprite = tempSp;
		} else {
			MyDebug.Log ("没有加载到图片");
		}
	}



	public void setbankImgEnable(bool flag){
		bankerImg.enabled = flag;

	}

	public void showChatAction(){
		showChatTime = 120;
		chatAction.SetActive (true);
	}

	public int getUuid(){
		int result = -1;
		if (avatarvo != null) {
			result = avatarvo.account.uuid;
		}
		return result;
	}

	public void clean(){
		Destroy(headerIcon.gameObject);
		Destroy (bankerImg.gameObject);
		Destroy (nameText.gameObject);
		Destroy (readyImg.gameObject);
	}

	/**设置游戏玩家离线**/
	public void setPlayerOffline(){

		offlineImage.transform.gameObject.SetActive (true);
	}

	/**设置游戏玩家上线**/
	public void setPlayerOnline(){
		offlineImage.transform.gameObject.SetActive (false);
	}

	public void showChatMessage(int index){
		showTime = 200;
		index = index - 1001;
		chatMessage.text = messageBoxContents[index];
		chatPaoPao.SetActive (true);
	}


	public void displayAvatorIp(){
		//userInfoPanel.SetActive (true);
		GameObject obj= PrefabManage.loadPerfab("Prefab/userInfo");
		obj.GetComponent<ShowUserInfoScript> ().setUIData (avatarvo);
	}

	public void setHuFlagDisplay(){
		HuFlag.SetActive (true);
	}

	public void setHuFlagHidde(){
		HuFlag.SetActive (false);
	}

}
