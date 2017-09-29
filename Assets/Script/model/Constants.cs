using System;

namespace AssemblyCSharp
{
	public class Constants
	{
		public static float GAME_DEFALUT_AGREE_TIME = 200.0f;
	}
	public enum GameType{
		ZHUAN_ZHUAN=1,
		HUA_SHUI=2,
		GI_PING_HU=3
	}
	public enum SceneType{
		LOGIN,
		HOME,
		GAME,
		SCORE
	}
	public enum SocketStatus{
		UNDEFINE,
		DISCONNECT,
		CONNECTING,
		CONNECTED
	}
}

