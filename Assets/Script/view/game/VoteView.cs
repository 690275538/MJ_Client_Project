using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System;


/**申请解散房间投票框**/

public class VoteView : MonoBehaviour {
	public Text sponsorNameText;
	public List<VoteResultItemView> voteResultList; 
	public Button okButton;
	public Button cancleButton;
	public Text timerText;
	private string sponsorName;
	private string dissolveType;
	private List<string> playerNames;
	private float _timer = Constants.GAME_DEFALUT_AGREE_TIME;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_timer != 0) {
			_timer -= Time.deltaTime;
			if (_timer < 0)
			{
				_timer = 0;
				clickOk ();
				//UpateTimeReStart();
			}
			timerText.text = Math.Floor(_timer) + "";
		}



	}

	private void addListener(){
		
		GameManager.getInstance ().Server.onResponse += onResponse;
	}
	public void removeListener(){
		GameManager.getInstance ().Server.onResponse -= onResponse;
	}

	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.DISSOLIVE_ROOM_RESPONSE:
			dissoliveRoomResponse(response);
			break;
		}
	}
	public  void iniUI( string sponsor){
		List<AvatarVO> avatarList = GlobalData.getInstance ().playerList;
		if (GlobalData.getInstance().myAvatarVO.account.nickname == sponsor) {
			okButton.transform.gameObject.SetActive (false);
			cancleButton.transform.gameObject.SetActive (false);
		}

		sponsorName = sponsor;
		playerNames = new List<string> ();
		sponsorNameText.text = sponsorName;
		addListener ();
		for (int i = 0; i <avatarList.Count; i++) {
			string name = avatarList [i].account.nickname;
		
			if (name != sponsorName) {
				playerNames.Add (name);
			}

		}

		for (int i = 0; i < playerNames.Count; i++) {
			voteResultList [i].setInitVal (playerNames [i], "正在选择");
		}

	
	}

	private VoteResultItemView getResultItem(string name){
		for (int i = 0; i < playerNames.Count; i++) {
			if (name == playerNames[i]) {
				return voteResultList[i];
			}
		}	
		return voteResultList[0];
	}

	private void dissoliveRoomResponse(ClientResponse response){
		DissoliveRoomResponseVo dvo = JsonMapper.ToObject<DissoliveRoomResponseVo> (response.message);
		string plyerName = dvo.accountName;
		if (dvo.type == "1") {
			getResultItem (plyerName).changeResult ("同意");
		} else if (dvo.type == "2") {
			getResultItem (plyerName).changeResult ("拒绝");

		} 
	}

	private void  doDissoliveRoomRequest(){
		DissoliveRoomRequestVo dvo = new DissoliveRoomRequestVo ();
		dvo.roomId = GlobalData.getInstance().roomVO.roomId;
		dvo.type = dissolveType;
		string sendMsg = JsonMapper.ToJson (dvo);
		GameManager.getInstance().Server.requset(new DissoliveRoomRequest(sendMsg));

	}
	public void  clickOk(){
		dissolveType = "1";
		okButton.transform.gameObject.SetActive (false);
		cancleButton.transform.gameObject.SetActive (false);
		doDissoliveRoomRequest ();


	}

	public void clickCancle(){
		dissolveType = "2";
		doDissoliveRoomRequest ();
		okButton.transform.gameObject.SetActive (false);
		cancleButton.transform.gameObject.SetActive (false);
	}

}
