using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	/// <summary>
	/// 消息分发类
	/// </summary>
	public class SocketEventHandle
	{

		public  delegate void ServerCallBackEvent (ClientResponse response);
		public  delegate void ServerDisconnectCallBackEvent ();
		public ServerCallBackEvent LoginCallBack;//登录回调
		public ServerCallBackEvent CreateRoomCallBack;//创建房间回调
		public ServerCallBackEvent JoinRoomCallBack;//加入房间回调

		public ServerCallBackEvent FinalGameOverCallBack;//全局结束回调

		public ServerCallBackEvent micInputNotice;
		public ServerCallBackEvent messageBoxNotice;
		public ServerCallBackEvent backLoginNotice;//玩家断线重连
		public ServerCallBackEvent RoomBackResponse;//掉线后返回房间
		public ServerCallBackEvent cardChangeNotice;//房卡数据变化
		//public ServerCallBackEvent rewardRequestCallBack;//投资请求返回
		public ServerCallBackEvent giftResponse;//奖品回调


		public ServerDisconnectCallBackEvent disConnetNotice;//断线
		public ServerCallBackEvent contactInfoResponse;//联系方式回调
		public ServerCallBackEvent zhanjiResponse;//房间战绩返回数据
		public ServerCallBackEvent zhanjiDetailResponse;//房间战绩返回数据

		public ServerCallBackEvent gameBackPlayResponse;//回放返回数据




		private List<ClientResponse> callBackResponseList;

		private bool isDisconnet = false;


		public SocketEventHandle ()
		{
			callBackResponseList = new List<ClientResponse> ();
		}

	
		private static SocketEventHandle _instance;
		public static SocketEventHandle getInstance(){
			if (_instance == null) {
                _instance = new SocketEventHandle();
			}
			return _instance;
		}

		public void FixedUpdate(){
			while(callBackResponseList.Count >0){
				ClientResponse response = callBackResponseList [0];
				callBackResponseList.RemoveAt (0);
				dispatchHandle (response);

			}

			if (isDisconnet) {
				isDisconnet = false;
				disConnetNotice ();
			}

		}

		private void dispatchHandle(ClientResponse response){
			switch(response.headCode){
			case APIS.CLOSE_RESPONSE:
				TipsManager.getInstance ().setTips ("服务器关闭了");
				//CustomSocket.getInstance ().closeSocket ();
				break;
			case APIS.LOGIN_RESPONSE:
				if (LoginCallBack != null) {
					LoginCallBack(response);
				}
				break;
			case APIS.CREATEROOM_RESPONSE:
				if (CreateRoomCallBack != null) {
					CreateRoomCallBack(response);
				}
				break;
			case APIS.JOIN_ROOM_RESPONSE:
				if (JoinRoomCallBack != null) {
					JoinRoomCallBack(response);
				}
				break;
			case APIS.HUPAIALL_RESPONSE:
				if (FinalGameOverCallBack != null) {
					FinalGameOverCallBack(response);
				}
				break;

			case APIS.headRESPONSE:
				break;
			case APIS.MicInput_Response:
				if (micInputNotice != null) {
					micInputNotice (response);
				}
				break;
			case APIS.MessageBox_Notice:
				if (messageBoxNotice != null) {
					messageBoxNotice (response);
				}
				break;
			case APIS.BACK_LOGIN_RESPONSE:
				if (RoomBackResponse != null) {
					RoomBackResponse (response);
				}

				break;
			case APIS.CARD_CHANGE:
				if (cardChangeNotice != null) {
					cardChangeNotice (response);
				}
				break;
			case APIS.PRIZE_RESPONSE:
				if (giftResponse != null) {
					giftResponse (response);
				}
				break;


			

			case APIS.CONTACT_INFO_RESPONSE:
				if (contactInfoResponse != null) {
					contactInfoResponse (response);
				}
				break;
			case APIS.ZHANJI_REPORTER_REPONSE:
				if (zhanjiResponse != null) {
					zhanjiResponse (response);
				}
				break;
			case APIS.ZHANJI_DETAIL_REPORTER_REPONSE:
				if (zhanjiDetailResponse != null) {
					zhanjiDetailResponse (response);
				}
				break;
			case APIS.GAME_BACK_PLAY_RESPONSE:
				if (gameBackPlayResponse != null) {
					gameBackPlayResponse (response);
				}
				break;
			case APIS.TIP_MESSAGE:
				TipsManager.getInstance ().setTips (response.message);
				break;
			}


        }

		public void addResponse(ClientResponse response){
			callBackResponseList.Add (response);
		}


		public void noticeDisConect(){
			isDisconnet = true;
		}

		//排除了在initConfigScript 中的
		public void clearListener(){
			if (CreateRoomCallBack != null) {
				CreateRoomCallBack = null;
			}

			if (JoinRoomCallBack != null) {
				JoinRoomCallBack = null;
			}
				


			if (messageBoxNotice != null) {
				messageBoxNotice = null;
			}



			if (backLoginNotice != null) {
				backLoginNotice = null;
			}

			if (cardChangeNotice != null) {
				cardChangeNotice = null;
			}


			if (giftResponse != null) {
				giftResponse = null;
			}

			if (contactInfoResponse != null) {
				contactInfoResponse = null;
			}

			if (zhanjiResponse != null) {
				zhanjiResponse = null;
			}



			if (zhanjiDetailResponse != null) {
				zhanjiDetailResponse = null;
			}

			if (gameBackPlayResponse != null) {
				gameBackPlayResponse = null;
			}
		}
	}
}

