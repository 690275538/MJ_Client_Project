using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class DebugView : MonoBehaviour
	{
		public InputField input;
		public InputField codeTf;
		public Button okBt;
		private int index=0;
		string[] kk;
		// Use this for initialization
		void Start ()
		{
			
			kk = new string[] {
				"00100006", "{\"cardPoint\":30,\"avatarId\":1}",
				"00100002", "{\"cardIndex\":29,\"curAvatarIndex\":1}",

				"00100014", "{\"avatarIndex\":2}",
				"00100002", "{\"cardIndex\":27,\"curAvatarIndex\":2}",

				"00100014", "{\"avatarIndex\":3}",
				"00100002", "{\"cardIndex\":27,\"curAvatarIndex\":3}",

				"00100004", "{\"cardPoint\":24,\"onePoint\":0,\"twoPoint\":0,\"type\":\"\"}", //自己摸牌

				"00100014", "{\"avatarIndex\":1}",
				"00100002", "{\"cardIndex\":25,\"curAvatarIndex\":1}",

				"00100000", "peng:1:25," //ACTION_BUTTON_NOTICE

			};
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void Click ()
		{
			try{
				GameManager.getInstance ().Server.qq (Int32.Parse (codeTf.text,System.Globalization.NumberStyles.HexNumber), input.text);
			}catch(Exception e){
				Debug.Log (e.ToString ());
			}
		}
		public void Next(){
			if (index < kk.Length) {
				codeTf.text = kk [index];
				input.text = kk [index + 1];

				index += 2;
			} else {
				index = 0;
			}
		}
		
	}
}