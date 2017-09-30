using System;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class UIHelper
	{
		public UIHelper ()
		{
		}
		public void updateRule(Text tf){
			RoomVO roomvo = GlobalData.getInstance ().roomVO;
			string str = "房间号：\n" + roomvo.roomId + "\n";
			str += "圈数：" + roomvo.roundNumber + "\n";

			if (roomvo.roomType == GameType.GI_PING_HU) {
				str += "鸡平胡\n";
			} else {

				if (roomvo.roomType == GameType.ZHUAN_ZHUAN) {
					if (roomvo.hong) {
						str += "红中麻将\n";
					} else {
						str += "转转麻将\n";
					}

				} else if (roomvo.roomType == GameType.HUA_SHUI) {
					str += "划水麻将\n";
				}
				if (roomvo.ziMo == 1) {
					str += "只能自摸\n";
				} else {
					str += "可抢杠胡\n";
				}
				if (roomvo.sevenDouble && roomvo.roomType != GameType.HUA_SHUI) {
					str += "可胡七对\n";
				}

				if (roomvo.addWordCard) {
					str += "有风牌\n";
				}
				if (roomvo.xiaYu > 0) {
					str += "下鱼数：" + roomvo.xiaYu + "";
				}

				if (roomvo.ma > 0) {
					str += "抓码数：" + roomvo.ma + "";
				}
			}
			if (roomvo.magnification > 0) {
				str += "倍率：" + roomvo.magnification + "";
			}
			tf.text = str;
		}
			
	}
}

