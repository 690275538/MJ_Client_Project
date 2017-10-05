using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{

	public class GamingData
	{
		public int myIndex;
		/**本轮庄家**/
		public int bankerIndex;
		public int remainCardNum;
		/**自己的手牌**/
		public List<List<int>> paiArray;
		List<AvatarVO> avatarList;

		public bool isReEnter = false;

		public int putoutIndex;
		/**因摸牌，碰，吃，杠牌，而要出牌的人**/
		public int pickIndex;
		public int putoutPoint;
		public int pickPoint;

		public string[] gangPaiList;
		public int pgcCardPoint;

		public List<AvatarVO> AvatarList{
			set{
				avatarList = value;
				AvatarVO myAVO = GlobalData.getInstance ().myAvatarVO;
				for (int i = 0; i < avatarList.Count; i++) {
					AvatarVO avo = avatarList [i];
					if (avo.account.uuid == myAVO.account.uuid || avo.account.openid == myAVO.account.openid) {
						myAVO.account.uuid = avo.account.uuid;
						myIndex = i;
						paiArray = ToList (avo.paiArray);
					}
				}
			}
			get{
				return avatarList;
			}
		}

		GameView host;
		public GamingData ()
		{
			
		}

		public void init(GameView host){
			this.host = host;
		}

		public Direction toGameDir(int avatarIndex){
			int i = (avatarIndex + 4 - myIndex) % 4;
			return (Direction)i;
		}
		public Direction toGameDir2(int uuid){
			int i = toAvatarIndex (uuid);
			return toGameDir (i);
		}
		public int toAvatarIndex (int uuid)
		{
			var avatarList = host.avatarList;
			if (avatarList != null) {
				for (int i = 0; i < avatarList.Count; i++) {
					if (avatarList [i].account != null) {
						if (avatarList [i].account.uuid == uuid) {
							return i;
						}
					}
				}
			}
			return 0;
		}
		public static List<List<int>> ToList (int[][] param)
		{
			List<List<int>> list = new List<List<int>> ();
			for (int i = 0; i < param.Length; i++) {
				List<int> subList = new List<int> ();
				for (int j = 0; j < param [i].Length; j++) {
					subList.Add (param [i] [j]);
				}
				list.Add (subList);
			}
			return list;
		}
	}
}

