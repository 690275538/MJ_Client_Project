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
		
		inputChars = new List<String>();
		for (int i = 0; i < btnList.Count; i++) {
			GameObject gobj = btnList [i];
			btnList [i].GetComponent<Button> ().onClick.AddListener(delegate() {
				this.OnClickHandle(gobj); 
			});
		}

		GameManager.getInstance ().Server.onResponse += onResponse;
	}
	
	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.JOIN_ROOM_RESPONSE://加入房间
			onJoinRoomResponse (response);
			break;
		}
	}

	public void OnClickHandle (GameObject gobj){
		

		clickNumber (gobj.GetComponentInChildren<Text>().text);
	}



	private void clickNumber(string number){


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
		
		Destroy (gameObject);
	}

	public void sureRoomNumber(){
		if (inputChars.Count != 6) {
			
			TipsManager.getInstance ().setTips ("请先完整输入房间号码！");
			return;
		}

		String roomNumber = inputChars[0]+inputChars[1]+inputChars[2]+inputChars[3]+inputChars[4]+inputChars[5];

		RoomJoinVo roomJoinVo = new  RoomJoinVo ();
		roomJoinVo.roomId =int.Parse(roomNumber);
		string sendMsg = JsonMapper.ToJson (roomJoinVo);
		GameManager.getInstance().Server.requset(new JoinRoomRequest(sendMsg));

	}

	public void onJoinRoomResponse(ClientResponse response){
		

		if (response.status == 1) {
			RoomJoinResponseVo vo = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);
			GameManager.getInstance ().DataMgr.updateRoomVO (vo);

			SceneManager.getInstance ().changeToScene (SceneType.GAME);
			closeDialog ();
		} else {
			TipsManager.getInstance ().setTips (response.message);
		}

	}
	void OnDestroy(){
		GameManager.getInstance ().Server.onResponse -= onResponse;
	}

}
