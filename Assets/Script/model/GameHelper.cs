using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class GameHelper
	{
		private static Dictionary<GameType,GameHelper> _map;
		public static GameHelper getHelper(GameType gt=GameType.UNDEFINE){
			if (_map == null) {
				_map = new Dictionary<GameType, GameHelper> ();
				_map.Add (GameType.JI_PING_HU, new JPHHelper ());
				_map.Add (GameType.HUA_SHUI, new HSHelper ());
				_map.Add (GameType.ZHUAN_ZHUAN, new ZZHelper ());
			}
			if (gt == GameType.UNDEFINE) {
				gt = GlobalData.getInstance ().roomVO.roomType;
			}
			return _map [gt];
		}
		public static int getInitRemainCardNum (RoomVO rvo)
		{

			int remainCardNum = 0;
			var type = rvo.roomType;
			if (type == GameType.ZHUAN_ZHUAN) {
				remainCardNum = 108;
				if (rvo.hong) {
					remainCardNum = 112;
				}
			} else if (type == GameType.HUA_SHUI) {
				remainCardNum = 108;
				if (rvo.addWordCard) {
					remainCardNum = 136;
				}
			} else if (type == GameType.JI_PING_HU) {
				remainCardNum = 136;
			}

			return remainCardNum - 53;
		}


		protected string name;
		public string getName(){
			return name;
		}
		virtual public string getHuString(THuInfo hu){
			string type = hu.type;
			if (type == "d_other") {
				return "点炮";
			}
			if (type == "zi_common") {
				return "自摸";
			} else if (type == "d_self") {
				return "接炮";
			} else if (type == "qiyise") {
				return "清一色";
			} else if (type == "zi_qingyise") {
				return "自摸清一色";
			} else if (type == "qixiaodui") {
				return "七小对";
			} else if (type == "self_qixiaodui") {
				return "自摸七小对";
			} else if (type == "gangshangpao") {
				return "杠上炮";
			} else if (type == "gangshanghua") {
				return "杠上花";
			}
			return "";
		}
		public bool isHu(string type){
			if (string.IsNullOrEmpty (type)) {
				return false;
			} else {
				return type != "d_other";
			}
		}
		public string getRuleStr(RoomVO rvo){
			string str = "房间号：\n" + rvo.roomId + "\n";
			str += "圈数：" + rvo.roundNumber + "\n";

			str += name;
			if (rvo.roomType == GameType.JI_PING_HU) {

			} else {

				if (rvo.ziMo == 1) {
					str += "只能自摸\n";
				} else {
					str += "可抢杠胡\n";
				}
				if (rvo.sevenDouble && rvo.roomType != GameType.HUA_SHUI) {
					str += "可胡七对\n";
				}

				if (rvo.addWordCard) {
					str += "有风牌\n";
				}
				if (rvo.xiaYu > 0) {
					str += "下鱼数：" + rvo.xiaYu + "";
				}

				if (rvo.ma > 0) {
					str += "抓码数：" + rvo.ma + "";
				}
			}
			if (rvo.magnification > 0) {
				str += "倍率：" + rvo.magnification + "";
			}
			return str;
		}
		public string getInviteRuleStr(RoomVO rvo){
			string str = "";

			str += name;

			str += "大战" + rvo.roundNumber + "局,";
			if (rvo.roomType == GameType.JI_PING_HU) {

			} else {
				if (rvo.ziMo == 1) {
					str += "只能自摸,";
				} else {
					str += "可抢杠胡,";
				}
				if (rvo.addWordCard) {
					str += "有风牌,";
				}

				if (rvo.xiaYu > 0) {
					str += "下鱼" + rvo.xiaYu + "条,";
				}

				if (rvo.ma > 0) {
					str += "抓" + rvo.ma + "个码,";
				}
			}
			if (rvo.magnification > 0) {
				str += "倍率" + rvo.magnification;
			}

			str += "有胆，你就来！";

			return str;
		}

	}
	class JPHHelper:GameHelper{
		public JPHHelper(){
			name = "鸡平胡";
		}
		override public string getHuString (THuInfo hu)
		{
			string str = "  ";
			if (hu.jphType > 0) {
				switch ((JPHType) hu.jphType) {
				case JPHType.JH:
					str += "鸡胡";
					break;
				case JPHType.PH:
					str += "平胡";
					break;
				case JPHType.PPH:
					str += "碰碰胡";
					break;
				case JPHType.HYS:
					str += "混一色";
					break;
				case JPHType.QYS:
					str += "清一色";
					break;
				case JPHType.HP:
					str += "混碰";
					break;
				case JPHType.QP:
					str += "清碰";
					break;
				case JPHType.HYJ:
					str += "混幺九";
					break;
				case JPHType.XSY:
					str += "小三元";
					break;
				case JPHType.XSX:
					str += "小四喜";
					break;
				case JPHType.ZYS:
					str += "字一色";
					break;
				case JPHType.QYJ:
					str += "清幺九";
					break;
				case JPHType.DSY:
					str += "大三元";
					break;
				case JPHType.DSX:
					str += "大四喜";
					break;
				case JPHType.JLBD:
					str += "九莲宝灯";
					break;
				case JPHType.SSY:
					str += "十三幺";
					break;
				}
			}
			return base.getHuString (hu) + str;
		}
	}
	class HSHelper:GameHelper{
		public HSHelper(){
			name = "划水麻将";
		}
	}
	class ZZHelper:GameHelper{
		public ZZHelper(){
			name = "转转麻将";
		}
	}
}

