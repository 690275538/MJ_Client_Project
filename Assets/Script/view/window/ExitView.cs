using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ExitView: MonoBehaviour {


	public void exit(){
		GameManager.getInstance().Server.requset(APIS.QUITE_LOGIN,GlobalData.getInstance().myAvatarVO.account.uuid.ToString());
		Application.Quit ();

	}

	public void cancle(){

		Destroy (gameObject);
	}
}
