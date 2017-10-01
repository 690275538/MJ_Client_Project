using System;

namespace AssemblyCSharp
{
	public class GamingData
	{
		public int myIndex;
		public GamingData ()
		{
		}
		public void setMyAvatarIndex(int index){
			myIndex = index;
//			dir = (Direction)index;
//			dirStr = Enum.GetName (typeof(Direction), index);
		}
		public Direction toGameDir(int avatarIndex){
			int i = (avatarIndex + 4 - myIndex) % 4;
			return (Direction)i;
		}

	}
}

