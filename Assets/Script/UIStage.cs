using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


public class UIStage : MonoBehaviour {

	private GameManager gameMgr;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		GameObject root = GameObject.Find ("RootContainer") as GameObject;
		GameObject login = GameObject.Find ("Panel_Login_View") as GameObject;



		gameMgr = GameManager.getInstance ();
		gameMgr.init (this,root,login);
		gameMgr.Server.connect ();
	}
	void FixedUpdate(){
		if (gameMgr != null )
			gameMgr.FixedUpdate ();

	}




}
