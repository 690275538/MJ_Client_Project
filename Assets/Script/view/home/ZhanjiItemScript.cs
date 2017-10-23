using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class ZhanjiItemScript: MonoBehaviour
	{
		public Text indexText;
		public Text timeText;
		public List<Text> scoresText;
		private DetailRecordVO mZhanjiDataItemVo;

		public ZhanjiItemScript ()
		{
		}

		public void setUI(DetailRecordVO zhanjiDataItemVo,int index){
			mZhanjiDataItemVo = zhanjiDataItemVo;
			indexText.text = index + "";
			timeText.text = parseDateSpan (mZhanjiDataItemVo.createtime);
			string content = mZhanjiDataItemVo.content;
			pareseContent (content);

		}
		private void pareseContent(string content){
			if (content != null && content != "") {
				string[] infoList = content.Split (new char[1]{','});
				for (int i = 0; i < infoList.Length-1; i++) {
					string score = infoList [i].Split (new char[1]{':'})[1];
					scoresText [i].text = score;
				}
			}
		}
		private string parseDateSpan(long timenumber){
			
			DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			dateTimeStart=dateTimeStart.AddMilliseconds (timenumber);
			string month =(dateTimeStart.Month>9)? dateTimeStart.Month.ToString ():"0"+dateTimeStart.Month.ToString ();
			string day =(dateTimeStart.Day>9)? dateTimeStart.Day.ToString ():"0"+dateTimeStart.Day.ToString ();
			string hour = dateTimeStart.Hour.ToString ();
			string minute = dateTimeStart.Minute.ToString ();
			return month + "-" + day + "  " + hour + ":" + minute;
		}


		public void  openGamePlay(){
			string id = mZhanjiDataItemVo.id + "";
			GameManager.getInstance().Server.requset (new GameBackPlayRequest (id));
			SceneManager.getInstance().loadPerfab ("Prefab/Panel_GamePlayBack");

		}
	}
}

