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


		protected string name;
		public string getName(){
			return name;
		}
		virtual public string getHuString(string type){
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
		override public string getHuString (string type)
		{
			var i = int.Parse (type);
			if (i > 0) {
				switch ((JPHType)i) {
				case JPHType.JH:
					return "鸡胡";
				case JPHType.PH:
					return "平胡";
				case JPHType.PPH:
					return "碰碰胡";
				case JPHType.HYS:
					return "混一色";
				case JPHType.QYS:
					return "清一色";
				case JPHType.HP:
					return "混碰";
				case JPHType.QP:
					return "清碰";
				case JPHType.HYJ:
					return "混幺九";
				case JPHType.XSY:
					return "小三元";
				case JPHType.XSX:
					return "小四喜";
				case JPHType.ZYS:
					return "字一色";
				case JPHType.QYJ:
					return "清幺九";
				case JPHType.DSY:
					return "大三元";
				case JPHType.DSX:
					return "大四喜";
				case JPHType.JLBD:
					return "九莲宝灯";
				case JPHType.SSY:
					return "十三幺";
				}
			}
			return base.getHuString (type);
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

