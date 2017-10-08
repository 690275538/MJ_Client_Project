using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class TableView : MonoBehaviour
	{
		public Text TimeLeft;
		public Text roomRemark;
		public Text LeavedCastNumText;//剩余牌的张数
		public Text LeavedRoundNumText;//剩余局数
		public Image remainCard;
		public Image remainRound;
		public Image centerImage;
		public GameObject genZhuang;
		public Text versionText;

		private float timer = 0;
		public List<GameObject> dirGameList;
		public TableView ()
		{
		}

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
		void UpateTimeReStart ()
		{
			timer = 16;
		}
	}
}

