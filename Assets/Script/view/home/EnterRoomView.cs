using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using LitJson;

public class EnterRoomView : MonoBehaviour{


	public Button button_sure,button_delete;//确认删除按钮

	private List<String> inputChars;//输入的字符
	public List<Text> inputTexts;

	public List<GameObject> btnList;

	// Use this for initialization
	void Start () {
		
		SocketEventHandle.getInstance().JoinRoomCallBack += onJoinRoomCallBack;
		inputChars = new List<String>();
		for (int i = 0; i < btnList.Count; i++) {
			GameObject gobj = btnList [i];
			btnList [i].GetComponent<Button> ().onClick.AddListener(delegate() {
				this.OnClickHandle(gobj); 
			});
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickHandle (GameObject gobj){
		//if(eventData.button)
		MyDebug.Log(gobj);
		clickNumber (gobj.GetComponentInChildren<Text>().text);
	}



	private void clickNumber(string number){

		MyDebug.Log (number.ToString ());
		if (inputChars.Count >=6) {
			return;
		}
		inputChars.Add(number);
		int index = inputChars.Count;
		inputTexts [index-1].text = number.ToString();

	}

	public void deleteNumber(){
		if (inputChars != null && inputChars.Count > 0) {
			inputChars.RemoveAt (inputChars.Count -1);
			inputTexts [inputChars.Count].text = "";
		}
	}

	public void closeDialog(){
		MyDebug.Log ("closeDialog");
		//GlobalDataScript.homePanel.SetActive (false);
		removeListener ();
		Destroy (this);
		Destroy (gameObject);
	}

	private void removeListener(){
		SocketEventHandle.getInstance().JoinRoomCallBack -= onJoinRoomCallBack;
	}

	public void sureRoomNumber(){
		if (inputChars.Count != 6) {
			MyDebug.Log ("请先完整输入房间号码！");
			TipsManager.getInstance ().setTips ("请先完整输入房间号码！");
			return;
		}

		String roomNumber = inputChars[0]+inputChars[1]+inputChars[2]+inputChars[3]+inputChars[4]+inputChars[5];
		MyDebug.Log (roomNumber);
		RoomJoinVo roomJoinVo = new  RoomJoinVo ();
		roomJoinVo.roomId =int.Parse(roomNumber);
		string sendMsg = JsonMapper.ToJson (roomJoinVo);
		GameManager.getInstance().Server.requset(new JoinRoomRequest(sendMsg));

	}

	public void onJoinRoomCallBack(ClientResponse response){
		MyDebug.Log (response);

		if (response.status == 1) {
			RoomJoinResponseVo vo = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);
			GameManager.getInstance ().DataMgr.updateRoomVO (vo);

			SceneManager.getInstance ().changeToScene (SceneType.GAME);
			closeDialog ();
		} else {
			TipsManager.getInstance ().setTips (response.message);
		}

	}

}
