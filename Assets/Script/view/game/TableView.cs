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
			string str = "房间号：\n" + rvo.roomId + "\n";
			str += "圈数：" + rvo.roundNumber + "\n";

			if (rvo.roomType == GameType.JI_PING_HU) {
				str += "鸡平胡\n";
			} else {

				if (rvo.roomType == GameType.ZHUAN_ZHUAN) {
					if (rvo.hong) {
						str += "红中麻将\n";
					} else {
						str += "转转麻将\n";
					}

				} else if (rvo.roomType == GameType.HUA_SHUI) {
					str += "划水麻将\n";
				}
				if (rvo.ziMo == 1) {
					str += "只能自摸\n";
				} else {
					str += "可抢杠胡\n";
				}
				if (rvo.sevenDouble && rvo.roomType != GameType.HUA_SHUI) {
					str += "可胡七对\n";
				}

				if (rvo.addWordCard) {
					str += "有风牌\n";
				}
				if (rvo.xiaYu > 0) {
					str += "下鱼数：" + rvo.xiaYu + "";
				}

				if (rvo.ma > 0) {
					str += "抓码数：" + rvo.ma + "";
				}
			}
			if (rvo.magnification > 0) {
				str += "倍率：" + rvo.magnification + "";
			}
			roomRemarkText.text = str;
		}
	}
}

