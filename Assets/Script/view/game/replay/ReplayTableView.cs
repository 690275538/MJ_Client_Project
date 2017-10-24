using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class ReplayTableView : MonoBehaviour
	{
		private ReplayData _data;
		public Text TimeLeft;
		public Text roomRemarkText;
		public Text remainCardText;
		public Text remainRoundText;



		private float timer = 0;
		public List<GameObject> dirGameList;


		void Update ()
		{
			timer -= Time.deltaTime;
			if (timer < 0) {
				timer = 0;
				//UpateTimeReStart();
			}
			TimeLeft.text = Math.Floor (timer) + "";

		}
		/// <summary>
		/// 重新开始计时
		/// </summary>
		public void resetTimer ()
		{
			timer = 16;
		}
		public void init (ReplayData data){
			_data = data;
			_data.RemainCardNumChange += onRemainCardNumChange;
			_data.PickIndexChange += onPickIndexChange;

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

		/**左上角显示房间规则**/
		public void updateRule ()
		{
			RoomVO rvo = _data.replayVO.roomvo;
			roomRemarkText.text = GameHelper.getHelper(rvo.roomType).getRuleStr(rvo);
		}
	}
}

