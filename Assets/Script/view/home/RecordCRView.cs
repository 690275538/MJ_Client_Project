using System;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;


public class RecordCRView :MonoBehaviour
{
	
	private RecordVO _data;
	public Text indexText;
	public Text roomIdText;
	public Text timeText;
	public List<Text> names;
	public List<Text> scores;


	public RecordCRView ()
	{
	}

	public void setUI (RecordVO  data, int index)
	{
		_data = data;
		indexText.text = index.ToString ();
		roomIdText.text = _data.roomId.ToString ();
		timeText.text = formatDate (_data.data.createtime);

		string content = _data.data.content;
		if (!string.IsNullOrEmpty(content)) {
			string[] p = content.Split (new char[1]{ ',' });
			for (int i = 0; i < p.Length - 1; i++) {
				var arr = p [i].Split (new char[1]{ ':' });
				names [i].text = arr [0];
				scores [i].text = arr [1];
			}
		}

	}

	private string formatDate (long time)
	{
		
		DateTime date = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
		date = date.AddMilliseconds (time);
		string month = (date.Month > 9) ? date.Month.ToString () : "0" + date.Month.ToString ();
		string day = (date.Day > 9) ? date.Day.ToString () : "0" + date.Day.ToString ();
		string hour = date.Hour.ToString ();
		string minute = date.Minute.ToString ();
		return month + "-" + day + "  " + hour + ":" + minute;
	
	}

	public void onClick ()
	{
		GameManager.getInstance ().Server.requset (new ClientRequest (APIS.RECORD_REQUEST, _data.data.id.ToString ()));
	}

}


