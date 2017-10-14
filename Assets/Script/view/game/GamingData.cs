using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{

	public class GamingData
	{
		public event Change RemainCardNumChange;
		public event Change PickIndexChange;

		public int myIndex;
		/**本轮庄家**/
		public int bankerIndex;

		private int _remainCardNum;
		public int remainCardNum{
			set{
				if (_remainCardNum != value) {
					_remainCardNum = value;
					if (RemainCardNumChange != null) {
						RemainCardNumChange ();
					}
				}
			}
			get{
				return _remainCardNum;
			}

		}
		/**自己的手牌**/
		public int[][] paiArray;

		List<AvatarVO> _avatarList;

		/**断线重进房间**/
		public bool isReEnter = false;
		/**上一个出牌的人**/
		public int putoutIndex;

		private int _pickIndex;
		/**因摸牌，碰，吃，杠牌，而将要出牌的人**/
		public int pickIndex{
			set{
				if (_pickIndex != value) {
					_pickIndex = value;
					if (PickIndexChange != null) {
						PickIndexChange ();
					}
				}
			}
			get{
				return _pickIndex;
			}
		}
		/**上一个打出的牌**/
		public int putoutPoint;
		/**摸到的牌**/
		public int pickPoint;

		/**服务器下发的可杠的牌**/
		public string[] gangPaiList;
		/**被碰吃杠的牌,杠可以在手牌中有多组，胡牌时可以自摸，可以胡别人**/
		public int pgcCardPoint;

		public int[] chiPoints;

		public bool isQiangHu = false;
		public int BankerUuid {
			get {
				if (_avatarList != null) {
					var avo = _avatarList [bankerIndex];
					return avo.account.uuid;
				}
				return -1;
			}
		}
		public List<AvatarVO> AvatarList{
			set{
				_avatarList = value;
				AvatarVO myAVO = GlobalData.getInstance ().myAvatarVO;
				for (int i = 0; i < _avatarList.Count; i++) {
					AvatarVO avo = _avatarList [i];
					if (avo.account.uuid == myAVO.account.uuid || avo.account.openid == myAVO.account.openid) {
						myAVO.account.uuid = avo.account.uuid;
						myIndex = i;
						paiArray = avo.paiArray;
					}
				}
			}
			get{
				return _avatarList;
			}
		}

		public HupaiResponseVo hupaiResponseVO;
		public FinalGameEndVo finalGameEndVo;

		public GamingData ()
		{
			
		}
		static public Direction ToGameDir(int avatarIndex,int refAvatarIndex){
			int i = (avatarIndex + 4 - refAvatarIndex) % 4;
			return (Direction)i;
		}
		public Direction toGameDir(int avatarIndex){
			return ToGameDir (avatarIndex, myIndex);
		}
		public Direction toGameDir2(int uuid){
			int i = toAvatarIndex (uuid);
			return toGameDir (i);
		}
		public int toAvatarIndex (int uuid)
		{
			if (_avatarList != null) {
				for (int i = 0; i < _avatarList.Count; i++) {
					var avo = _avatarList [i];
					if (avo.account != null) {
						if (avo.account.uuid == uuid) {
							return i;
						}
					}
				}
			}
			return 0;
		}
		public AvatarVO getAvatarVO(int uuid){
			if (_avatarList != null) {
				for (int i = 0; i < _avatarList.Count; i++) {
					var avo = _avatarList [i];
					if (avo.account != null) {
						if (avo.account.uuid == uuid) {
							return avo;
						}
					}
				}
			}
			return null;
		}
	}
}

