using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


public class InitializationConfigScritp : MonoBehaviour {
	static InitializationConfigScritp instance;
	ServiceErrorListener seriveError = null;

	void Start () {
		instance = this;
		MicroPhoneInput.getInstance ();
		GlobalDataScript.getInstance ();
		//CustomSocket.getInstance().Connect();
		//ChatSocket.getInstance ();
		TipsManagerScript.getInstance ().parent = gameObject.transform;
		SoundCtrl.getInstance ();

		UpdateScript update = new UpdateScript ();
		StartCoroutine (update.updateCheck ());
		seriveError = new ServiceErrorListener();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//heartbeatTimer ();
		heartbeatThread();
	}

   void	Awake(){
		SocketEventHandle.getInstance().disConnetNotice += disConnetNotice;
		SocketEventHandle.getInstance ().hostUpdateDrawResponse += hostUpdateDrawResponse;
		SocketEventHandle.getInstance ().otherTeleLogin += otherTeleLogin;
	}




	private void  disConnetNotice(){
		if (GlobalDataScript.isonLoginPage) {
			//CustomSocket.getInstance ().Connect();
			//ChatSocket.getInstance ().Connect ();
		} else {
			SocketEventHandle.getInstance ().clearListener ();
			PrefabManage.loadPerfab ("Prefab/Panel_Start");

		}
	}

	private void otherTeleLogin(ClientResponse response){
	//	TipsManagerScript.getInstance ().setTips ("你的账号在其他设备登录");
		disConnetNotice ();
	}



	System.Timers.Timer t;
	private  void heartbeatTimer(){
		t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
		t.Elapsed += new System.Timers.ElapsedEventHandler(doSendHeartbeat); //到达时间的时候执行事件；   
		t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
		t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

	}

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

	public  void doSendHeartbeat( object source, System.Timers.ElapsedEventArgs e){
		CustomSocket.getInstance ().sendHeadData ();
	}


	private void hostUpdateDrawResponse(ClientResponse response){
		int giftTimes =int.Parse(response.message);
		GlobalDataScript.loginResponseData.account.prizecount = giftTimes;
		if(CommonEvent.getInstance().prizeCountChange !=null){
			CommonEvent.getInstance ().prizeCountChange();
		}
	}

}
