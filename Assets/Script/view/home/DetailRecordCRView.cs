using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class DetailRecordCRView: MonoBehaviour
	{
		public Text indexText;
		public Text timeText;
		public List<Text> scoresText;
		private DetailRecordVO dvo;

		public DetailRecordCRView ()
		{
		}

		public void setUI(DetailRecordVO dvo,int index){
			this.dvo = dvo;
			indexText.text = index + "";

			DateTime date = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
			date = date.AddMilliseconds (dvo.createtime);
			timeText.text = date.ToString (@"yy-MM-dd hh\:mm");


			string content = dvo.content;
			if (content != null && content != "") {
				string[] p = content.Split (new char[1]{','});
				for (int i = 0; i < p.Length-1; i++) {
					string score = p [i].Split (new char[1]{':'})[1];
					scoresText [i].text = score;
				}
			}

		}

		public void  openGamePlay(){
			string id = dvo.id + "";
			GameManager.getInstance().Server.requset (new GameBackPlayRequest (id));
			SceneManager.getInstance().loadPerfab ("Prefab/Panel_GamePlayBack");

		}
	}
}

