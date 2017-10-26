using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class TGangInfo
	{
		public int cardPoint;
		/**出牌人**/
		public string uuid;
		/**明暗**/
		public string type;
	}

	public class TPengInfo{
		public int cardPoint;
	}

	public class THuInfo
	{
		public int cardPiont;
		/**出牌人**/
		public string uuid;
		public string type;
		public int jphType = 0;
	}

	public class TChiInfo
	{
		public string uuid;
		public int[] cardPionts;
	}

	[Serializable]
	public class TotalInfo
	{
		public string gang;
		public string peng;
		public string chi;
		public string hu;
		public string genzhuang;
		public TotalInfo ()
		{

		}
		public TGangInfo[] getGangInfos(){
			List<TGangInfo> infos = new List<TGangInfo> ();
			string str;
			string[] ss;
			string[] s;
			str = gang;
			if (!string.IsNullOrEmpty (str)) {
				ss = str.Split (new char[1]{ ',' });
				for (int i = 0; i < ss.Length; i++) {
					TGangInfo info = new TGangInfo ();
					s = ss [i].Split (new char[1]{ ':' });
					info.uuid = s [0];
					info.cardPoint = int.Parse (s [1]);
					info.type = s [2];

					infos.Add (info);
				}
			}

			return infos.ToArray ();
		}
		public TPengInfo[] getPengInfos(){
			List<TPengInfo> infos = new List<TPengInfo> ();
			string str;
			string[] ss;
			str = peng;
			if (!string.IsNullOrEmpty (str)) {
				ss = str.Split (new char[1]{ ',' });
				for (int i = 0; i < ss.Length; i++) {
					TPengInfo info = new TPengInfo ();
					info.cardPoint = int.Parse (ss [i]);
					infos.Add (info);
				}
			}

			return infos.ToArray ();
		}
		public TChiInfo[] getChiInfos(){
			List<TChiInfo> infos = new List<TChiInfo> ();
			string str;
			string[] ss;
			string[] s;
			str = chi;
			if (!string.IsNullOrEmpty (str)) {
				ss = str.Split (new char[1]{ ',' });
				for (int i = 0; i < ss.Length; i++) {
					TChiInfo info = new TChiInfo ();
					s = ss [i].Split (new char[1]{ ':' }); 
					info.uuid = s [0];
					info.cardPionts = new int[3];
					info.cardPionts [0] = int.Parse(s [1]);
					info.cardPionts [1] = int.Parse(s [2]);
					info.cardPionts [2] = int.Parse(s [3]);
					infos.Add (info);
				}
			}

			return infos.ToArray ();
		}
		public THuInfo getHuInfo(){
			THuInfo info = null;
			string str;
			string[] ss;
			str = hu;
			if (!string.IsNullOrEmpty (str)) {
				ss = str.Split (new char[1]{ ':' });
				info = new THuInfo ();
				info.uuid = ss [0];
				info.cardPiont = int.Parse (ss [1]);
				info.type = ss [2];
				if (ss.Length == 4)
					info.jphType = int.Parse (ss [3]);
			}
			return info;
		}
	}
}

