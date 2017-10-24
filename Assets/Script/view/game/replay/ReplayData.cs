using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{

	public class ReplayData
	{
		public event Change RemainCardNumChange;
		public event Change PickIndexChange;

		public int myIndex;
		/**本轮庄家**/
		public int bankerIndex;

		private int _remainCardNum;

		public GamePlayResponseVo replayVO;

		public bool isPlaying = false;

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

		List<ReplayAvatarVO> _avatarList;

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
		/**打出的牌**/
		public int putoutPoint;
		/**摸到的牌**/
		public int pickPoint;


		public int BankerUuid {
			get {
				if (_avatarList != null) {
					var avo = _avatarList [bankerIndex];
					return avo.uuid;
				}
				return -1;
			}
		}
		public List<ReplayAvatarVO> AvatarList{
			set{
				_avatarList = value;
				AvatarVO myAVO = GlobalData.getInstance ().myAvatarVO;
				for (int i = 0; i < _avatarList.Count; i++) {
					ReplayAvatarVO avo = _avatarList [i];
					if (avo.uuid == myAVO.account.uuid ) {
						myIndex = i;
						//paiArray = avo.paiArray;
					}
				}
			}
			get{
				return _avatarList;
			}
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
					if (avo.uuid == uuid) {
						return i;
					}

				}
			}
			return 0;
		}
		public ReplayAvatarVO getAvatarVO(int uuid){
			if (_avatarList != null) {
				for (int i = 0; i < _avatarList.Count; i++) {
					var avo = _avatarList [i];
					if (avo.uuid == uuid) {
						return avo;
					}

				}
			}
			return null;
		}
	}
}

