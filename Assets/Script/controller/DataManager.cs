using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class DataManager
	{
		public DataManager ()
		{
		}
		public void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.OTHER_TELE_LOGIN:
				
				break;
			}

		}
		public void updateRoomVO(RoomJoinResponseVo vo){
			RoomVO myRVO = GlobalData.getInstance ().roomVO;
			AvatarVO myAVO = GlobalData.getInstance ().myAvatarVO;
			myRVO.addWordCard = vo.addWordCard;
			myRVO.hong = vo.hong;
			myRVO.ma = vo.ma;
			myRVO.name = vo.name;
			myRVO.roomId = vo.roomId;
			myRVO.roomType = vo.roomType;
			myRVO.roundNumber = vo.roundNumber;
			myRVO.sevenDouble = vo.sevenDouble;
			myRVO.xiaYu = vo.xiaYu;
			myRVO.ziMo = vo.ziMo;
			myRVO.magnification = vo.magnification;

			GlobalData.getInstance ().remainRoundCount = vo.roundNumber;


			GlobalData.getInstance ().playerList = vo.playerList;

			for (int i = 0; i < vo.playerList.Count; i++) {
				AvatarVO avo =	vo.playerList [i];
				if (avo.account.openid == myAVO.account.openid) {
					myAVO.account.uuid = avo.account.uuid;
					myAVO.account.roomcard = avo.account.roomcard;
					break;
				}
			}
		}

		public void parseBroadCast(string msg){
			string[] noticeList = msg.Split (new char[1]{ '*' });
			if (noticeList != null)
			{
//				List<String> list = GlobalData.getInstance ().NoticeMsgs;
				List<String> list = new List<string>();
				for (int i=0 ;i<noticeList.Length ;i++){
					
					list.Add (noticeList[i]);
				}
				GlobalData.getInstance ().NoticeMsgs = list;
			}
		}

		public void parsePrizeCount(string msg){
			int prizecount =int.Parse(msg);
			GlobalData.getInstance().PrizeCount = prizecount;

		}
	}
}

