using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


public class InitializationConfigScritp : MonoBehaviour {
//	static InitializationConfigScritp instance;

	void Start () {
//		instance = this;
		MicroPhoneInput.getInstance ();
		GameObject root = GameObject.Find ("container") as GameObject;
		GameObject login = GameObject.Find ("Panel_Login_View") as GameObject;
		GlobalData.getInstance ().init (gameObject,root,login);


		CustomSocket.getInstance ().Connect ();
		ChatSocket.getInstance ().Connect ();
		SoundCtrl.getInstance ();

		UpdateScript update = new UpdateScript ();
		StartCoroutine (update.updateCheck ());
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//heartbeatTimer ();
		heartbeatThread();
	}

   void	Awake(){
		SocketEventHandle.getInstance().disConnetNotice += disConnetNotice;
		SocketEventHandle.getInstance ().hostUpdateDrawResponse += hostUpdateDrawResponse;
		SocketEventHandle.getInstance ().otherTeleLogin += otherTeleLogin;
		SocketEventHandle.getInstance().serviceErrorNotice += serviceErrorNotice;
	}

	public void serviceErrorNotice(ClientResponse response){
		TipsManager.getInstance().setTips(response.message);
	}


	private void  disConnetNotice(){
		SceneManager.getInstance ().changeToScene (SceneType.LOGIN);
		SocketEventHandle.getInstance ().clearListener ();
	}

	private void otherTeleLogin(ClientResponse response){
		disConnetNotice ();
		TipsManager.getInstance ().setTips ("你的账号在其他设备登录");
	}



//	System.Timers.Timer t;
//	private  void heartbeatTimer(){
//		t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
//		t.Elapsed += new System.Timers.ElapsedEventHandler(doSendHeartbeat); //到达时间的时候执行事件；   
//		t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
//		t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
//
//	}

//	public  void doSendHeartbeat( object source, System.Timers.ElapsedEventArgs e){
//		CustomSocket.getInstance ().sendHeadData ();
//	}


	private void heartbeatThread(){
		Thread thread = new Thread (sendHeartbeat);
		thread.IsBackground = true;
		thread.Start();
	}


	private static void sendHeartbeat(){
		CustomSocket.getInstance ().sendHeadData ();
		Thread.Sleep (20000);
		sendHeartbeat ();
	}

	private void hostUpdateDrawResponse(ClientResponse response){
		int prizecount =int.Parse(response.message);
		GlobalData.myAvatarVO.account.prizecount = prizecount;
		if(CommonEvent.getInstance().prizeCountChange !=null){
			CommonEvent.getInstance ().prizeCountChange();
		}
	}

}
