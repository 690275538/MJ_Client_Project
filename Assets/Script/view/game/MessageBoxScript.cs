using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

public class MessageBoxScript : MonoBehaviour {
	GameView myMaj;
	// Use this for initialization
	void Start () {
		SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void btnClick(int index){
		SoundCtrl.getInstance ().playMessageBoxSound (index);
		GameManager.getInstance().Server.requset (new MessageBoxRequest(index,GlobalData.getInstance().myAvatarVO.account.uuid));
		if (myMaj == null) {
			myMaj = SceneManager.getInstance ().CurScenePanel.GetComponent<GameView> ();
		}
		if (myMaj != null) {
			myMaj.PlayerItemViews [0].showChatMessage (index);
		}
		hidePanel ();
	}

	public void showPanel(){
		gameObject.transform.DOLocalMove (new Vector3(472,113), 0.4f);
	}

	public void hidePanel(){
		gameObject.transform.DOLocalMove (new Vector3(472,567), 0.4f);
	}

	public void messageBoxNotice(ClientResponse response){
		string[] arr = response.message.Split (new char[1]{ '|' });
		int code = int.Parse(arr[0]);
		SoundCtrl.getInstance ().playMessageBoxSound (code);
	}

	public void Destroy(){
		SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
	}
}
