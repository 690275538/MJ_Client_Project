using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ExitView: MonoBehaviour {


	public void exit(){
		CustomSocket.getInstance().sendMsg(new LoginRequest());
		Application.Quit ();

	}

	public void cancle(){

		Destroy (this);
		Destroy (gameObject);
	}
}
