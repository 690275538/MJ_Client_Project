using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomVO
	{
		public GameType roomType;
		public int roomId;


		public  bool hong;
		public int ma;
		/**局数**/
		public int roundNumber;
		public bool sevenDouble;
		public int ziMo;//1：自摸胡；2、抢杠胡
		public int xiaYu;
		public string name;
		public bool addWordCard;
		public int magnification;
		public RoomVO()
		{

		}
	}
}

