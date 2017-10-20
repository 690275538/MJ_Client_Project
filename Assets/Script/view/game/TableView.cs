using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class TableView : MonoBehaviour
	{
		private GameView _host;
		private GamingData _data;
		public Text TimeLeft;
		public Text roomRemarkText;
		public Text remainCardText;
		public Text remainRoundText;
		public Text TimeText;
		public Image tableUI;
		public Image centerTitle;

		public Text versionText;

		private float timer = 0;
		public List<GameObject> dirGameList;

		public Button inviteFriendBtn;
		public Button exitRoomBtn;


		void Update ()
		{
			timer -= Time.deltaTime;
			if (timer < 0) {
				timer = 0;
				//UpateTimeReStart();
			}
			TimeLeft.text = Math.Floor (timer) + "";

			TimeText.text = DateTime.Now.ToString ("HH : mm");
		}
		/// <summary>
		/// 重新开始计时
		/// </summary>
		public void resetTimer ()
		{
			timer = 16;
		}
		public void init (GameView host){
			_host = host;
			_data = _host.Data;
			_data.RemainCardNumChange += onRemainCardNumChange;
			_data.PickIndexChange += onPickIndexChange;

			versionText.text = "V" + Application.version;
			onRemainCardNumChange ();
			onPickIndexChange ();
		}
		private void onRemainCardNumChange(){
			remainCardText.text = _data.remainCardNum.ToString();
		}
		private void onPickIndexChange(){
			for (int i = 0; i < dirGameList.Count; i++) {
				dirGameList [i].SetActive (false);
			}
			dirGameList [(int)_data.toGameDir(_data.pickIndex)].SetActive (true);
		}
		public void setTableVisible (bool isGameStart)
		{
			centerTitle.transform.gameObject.SetActive (!isGameStart);
			inviteFriendBtn.transform.gameObject.SetActive (!isGameStart);
			exitRoomBtn.transform.gameObject.SetActive (!isGameStart);
			tableUI.transform.gameObject.SetActive (isGameStart);
		}

		/**左上角显示房间规则**/
		public void updateRule ()
		{
			RoomVO rvo = GlobalData.getInstance ().roomVO;

			roomRemarkText.text = GameHelper.getHelper().getRuleStr(rvo);
		}
	}
}

