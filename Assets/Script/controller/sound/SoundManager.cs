using UnityEngine;
using System.Collections;


namespace AssemblyCSharp
{
	public class SoundManager
	{

		private Hashtable soudHash = new Hashtable ();

		private static SoundManager _instance;

		private static AudioSource audioS;

		public static SoundManager getInstance ()
		{
			if (_instance == null) {
				_instance = new SoundManager ();
				audioS = GameObject.Find ("MyAudio").GetComponent<AudioSource> ();
			}

			return _instance;
		}

		public void playSound (int cardPoint, int sex)
		{
			if (GlobalData.getInstance ().SoundToggle) {
				string path = null;
				if (GlobalData.lang == Language.YUEYU) {
					path = "Sounds/yueyu/";
				} else {
					path = "Sounds/putonghua/";
				}
				if (sex == 1) {
					path += "boy/" + (cardPoint + 1);
				} else {
					path += "girl/" + (cardPoint + 1);
				}
				AudioClip temp = (AudioClip)soudHash [path];
				if (temp == null) {
					temp = GameObject.Instantiate (Resources.Load (path)) as AudioClip;
					soudHash.Add (path, temp);
				}
				audioS.clip = temp;
				audioS.loop = false;
				audioS.Play ();
			}
		}

		public void playMessageBoxSound (int codeIndex)
		{
			if (GlobalData.getInstance ().SoundToggle) {
				string path = "Sounds/other/" + codeIndex;
				AudioClip temp = (AudioClip)soudHash [path];
				if (temp == null) {
					temp = GameObject.Instantiate (Resources.Load (path)) as AudioClip;
					soudHash.Add (path, temp);
				}
				audioS.clip = temp;
				audioS.Play ();
			}
		}

		public void playBGM ()
		{
//		string path = "Sounds/mjBGM";
//		AudioClip temp = (AudioClip)soudHash[path];
//		if(temp == null){
//			temp = GameObject.Instantiate(Resources.Load (path)) as AudioClip;
//			soudHash.Add (path,temp);
//		}
//		audioS.clip = temp;
//		audioS.loop = true;
//		audioS.Play ();
//		if (GlobalData.soundToggle) {
//			audioS.mute = false;
//		} else {
//			audioS.mute = true;
//		}
		}

		public void stopBGM ()
		{
			audioS.loop = false;
			audioS.Stop ();
		}

		public void playSoundByAction (string str, int sex)
		{
			string path = null;
			if (GlobalData.lang == Language.YUEYU) {
				path = "Sounds/yueyu/";
			} else {
				path = "Sounds/putonghua/";
			}
			if (sex == 1) {
				path += "boy/" + str;
			} else {
				path += "girl/" + str;
			}
			AudioClip temp = (AudioClip)soudHash [path];
			if (temp == null) {
				temp = GameObject.Instantiate (Resources.Load (path)) as AudioClip;
				soudHash.Add (path, temp);
			}
			audioS.clip = temp;
			audioS.Play ();
		}


	}
}