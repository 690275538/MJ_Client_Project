using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


public class UIStage : MonoBehaviour {
//	static InitializationConfigScritp instance;
	SocketEventHandle eventCenter;

	private GameManager gameMgr;

	void Start () {
//		instance = this;
		GameObject root = GameObject.Find ("RootContainer") as GameObject;
		GameObject login = GameObject.Find ("Panel_Login_View") as GameObject;
		GlobalData.getInstance ().init (gameObject,root,login);

		eventCenter = SocketEventHandle.getInstance ();


		MicroPhoneInput.getInstance ();

		ChatSocket.getInstance ().Connect ();
		SoundCtrl.getInstance ();

		UpdateScript update = new UpdateScript ();
		StartCoroutine (update.updateCheck ());
		Screen.sleepTimeout = SleepTimeout.NeverSleep;


		gameMgr = GameManager.getInstance ();
		gameMgr.Server.connect ();
	}
	void FixedUpdate(){
		gameMgr.FixedUpdate ();
		eventCenter.FixedUpdate ();
	}




}
