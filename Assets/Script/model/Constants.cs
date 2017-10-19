using System;

namespace AssemblyCSharp
{
	public delegate void OnData(ClientResponse response);
	public delegate void OnStatus(SocketStatus status);
	public delegate void Change();
	public delegate void OnClick();
	public class Constants
	{
		public static float GAME_DEFALUT_AGREE_TIME = 200.0f;
	}
	public enum PaiArrayType{
		PENG=1,
		GANG=2,
		CHI=4
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
	public enum GameStatus{
		UNDEFINED,
		READYING,
		GAMING

	}
	public enum Direction{
		B,
		R,
		T,
		L
	}
	public enum ActionType
	{
		GANG,
		PENG,
		CHI,
		HU,
		LIUJU,
		GEN_ZHUANG
	}
	public enum Language{
		YUEYU =1,
		PU_TONG_HUA=2
	}

}

