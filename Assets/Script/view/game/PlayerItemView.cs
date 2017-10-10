using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class PlayerItemView: MonoBehaviour
	{

		static List<string> messageBoxContents = new List<string> ();

		static void initMessageBox ()
		{
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
		/**说话提示**/
		public GameObject chatAction;
		/**离线图片**/
		public Image offlineImage;
		public Text chatMessage;
		public GameObject chatMc;
		public GameObject HuFlag;

		private AvatarVO avatarVO;
		private int _showTime;
		private int _showChatTime;


		// Use this for initialization
		void Start ()
		{

		}
	
		// Update is called once per frame
		void Update ()
		{
			if (_showTime > 0) {
				_showTime--;
				if (_showTime == 0) {
					chatMc.SetActive (false);
				}
			}

			if (_showChatTime > 0) {
				_showChatTime--;
				if (_showChatTime == 0) {
					chatAction.SetActive (false);
				}
			}
		}

		public void setAvatarVo (AvatarVO value)
		{
			if (value != null) {
				avatarVO = value;
				readyImg.enabled = avatarVO.isReady;
				bankerImg.enabled = avatarVO.main;
				nameText.text = avatarVO.account.nickname;
				scoreText.text = avatarVO.scores + "";
				offlineImage.transform.gameObject.SetActive (!avatarVO.isOnLine);
				StartCoroutine (LoadImg ());

			} else {
				nameText.text = "";
				readyImg.enabled = false;
				bankerImg.enabled = false;
				scoreText.text = "";
				readyImg.enabled = false;

				headerIcon.sprite = Resources.Load ("Image/morentouxiang", typeof(Sprite)) as Sprite;
			}
		}

		/// <summary>
		/// 加载头像
		/// </summary>
		/// <returns>The image.</returns>
		private IEnumerator LoadImg ()
		{ 
			//开始下载图片
			WWW www = new WWW (avatarVO.account.headicon);
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



		public void setBankerIconVisible (bool flag)
		{
			bankerImg.enabled = flag;

		}

		public void showChatAction ()
		{
			_showChatTime = 120;
			chatAction.SetActive (true);
		}

		public int getUuid ()
		{
			int result = -1;
			if (avatarVO != null) {
				result = avatarVO.account.uuid;
			}
			return result;
		}

		public void destroy_all ()
		{
			Destroy (headerIcon.gameObject);
			Destroy (bankerImg.gameObject);
			Destroy (nameText.gameObject);
			Destroy (readyImg.gameObject);
			Destroy (scoreText);
			Destroy (chatAction);
			Destroy (offlineImage);
			Destroy (HuFlag);
			Destroy (gameObject);
			Destroy (this);
		}

		/**设置游戏玩家离线**/
		public void setPlayerOffline ()
		{

			offlineImage.transform.gameObject.SetActive (true);
		}

		/**设置游戏玩家上线**/
		public void setPlayerOnline ()
		{
			offlineImage.transform.gameObject.SetActive (false);
		}

		public void showChatMessage (int index)
		{
			_showTime = 200;
			index = index - 1001;
			chatMessage.text = messageBoxContents [index];
			chatMc.SetActive (true);
		}


		public void displayAvatorIp ()
		{
			//userInfoPanel.SetActive (true);
			GameObject obj = PrefabManage.loadPerfab ("Prefab/userInfo");
			obj.GetComponent<ShowUserInfoScript> ().setUIData (avatarVO);
		}

		public void setHuFlagDisplay ()
		{
			HuFlag.SetActive (true);
		}

		public void setHuFlagHidde ()
		{
			HuFlag.SetActive (false);
		}

	}
}