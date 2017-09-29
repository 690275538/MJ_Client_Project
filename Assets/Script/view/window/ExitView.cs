using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ExitView: MonoBehaviour {


	public void exit(){
		GameManager.getInstance().Server.requset(new LoginRequest());
		Application.Quit ();

	}

	public void cancle(){

		Destroy (this);
		Destroy (gameObject);
	}
}
