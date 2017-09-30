using System;
using UnityEngine.UI;
using UnityEngine;
using LitJson;

namespace AssemblyCSharp
{
	public class DissoliveHelper
	{
		private string dissoliveRoomType = "0";
		public DissoliveHelper ()
		{
			
		}
		public void init(GameView host){
			Transform dissoliveBt = host.transform.Find ("TR_MENU_BAR/Button_Dissolive");
			dissoliveBt.GetComponent<Button> ().onClick.AddListener (showDialog);
			GameManager.getInstance ().Server.onResponse += onResponse;
		}
		void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.DISSOLIVE_ROOM_RESPONSE:
				dissoliveRoomResponse(response);
				break;
			}
		}
		/**
	 * 申请或同意解散房间请求
	 * 
	 */ 

		public void showDialog ()
		{
			if (GlobalData.getInstance().gameStatus == GameStatus.GAMING) {
				dissoliveRoomType = "0";
				TipsManager.getInstance ().loadDialog ("申请解散房间", "你确定要申请解散房间？", onOK, onCancle);
			} else {
				TipsManager.getInstance ().setTips ("还没有开始游戏，不能申请退出房间");
			}

		}

		public void  onOK ()
		{
			DissoliveRoomRequestVo vo = new DissoliveRoomRequestVo ();
			vo.roomId = GlobalData.getInstance ().roomVO.roomId;
			vo.type = dissoliveRoomType;
			string sendMsg = JsonMapper.ToJson (vo);
			GameManager.getInstance ().Server.requset (new DissoliveRoomRequest (sendMsg));
			GlobalData.isDissoliving = true;
		}


		/***
	 * 申请解散房间回调
	 */
		GameObject dissoDialog;

		public void dissoliveRoomResponse (ClientResponse response)
		{
			MyDebug.Log ("dissoliveRoomResponse" + response.message);
			DissoliveRoomResponseVo vo = JsonMapper.ToObject<DissoliveRoomResponseVo> (response.message);
			string plyerName = vo.accountName;
			if (vo.type == "0") {
				GlobalData.isDissoliving = true;
				dissoliveRoomType = "1";
				dissoDialog = PrefabManage.loadPerfab ("Prefab/Panel_Apply_Exit");
				dissoDialog.GetComponent<VoteScript> ().iniUI (plyerName, GlobalData.getInstance().playerList);
			} else if (vo.type == "3") {
				
				GlobalData.isDissoliving = false;
				GlobalData.isOverByPlayer = true;
				closeDialog ();

			}  
		}
		public void closeDialog(){
			if (dissoDialog != null) {
				dissoDialog.GetComponent<VoteScript> ().removeListener ();
				GameObject.Destroy (dissoDialog.GetComponent<VoteScript> ());
				GameObject.Destroy (dissoDialog);
			}
		}


		private void onCancle ()
		{

		}

	}
}

