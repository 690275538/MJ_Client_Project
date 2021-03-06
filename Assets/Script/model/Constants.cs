﻿#define JM
using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public delegate void OnData(ClientResponse response);
	public delegate void OnStatus(SocketStatus status);
	public delegate void Change();
	public delegate void OnClick();
	public class Constants
	{
		public static float GAME_DEFALUT_AGREE_TIME = 200.0f;
		#if JM

		public static string IP = "whyzx.com";
		public static string UPDATE_INFO_JSON_URL = "http://"+IP+":8080/download/update.xml";
		public static string chatSocketUrl = IP;
		public static string socketUrl = IP;
		public static string PIC_PATH = "http://"+IP+":8080/";
		public static string ImgUrl = "http://"+IP+":8080/MaJiangManage/images";
		public static string Download_URL = "http://"+IP+":8080/download/index.html";
		#else
		public const string UPDATE_INFO_JSON_URL = "http://whyzx.com:8080/download/update.xml";
		public const string chatSocketUrl = "whyzx.com";
		public const string socketUrl = "whyzx.com";
		public const string PIC_PATH = "http://whyzx.com:8080/";
		public const string ImgUrl = "http://whyzx.com:8080/MaJiangManage/images";
		public const string Download_URL = "http://whyzx.com:8080/download/index.html";

		#endif


	
	}
	public enum PaiArrayType{
		PENG=1,
		GANG=2,
		CHI=4
	}
	public enum GameType{
		UNDEFINE=0,
		ZHUAN_ZHUAN=1,
		HUA_SHUI=2,
		JI_PING_HU=3
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
	public enum JPHType{
		// 鸡平胡
		JH = 1,
		PH = 2,
		PPH = 3,
		HYS = 4,
		QYS = 5,
		HP = 6,
		QP = 7,
		HYJ = 8,
		XSY = 9,
		XSX = 10,
		ZYS = 11,
		QYJ = 12,
		DSY = 13,
		DSX = 14,
		JLBD = 15,
		SSY = 16
	}
	public enum HuType{
		UNDEFINE,
		DIAN_PAO,
		JIE_PAO,
		ZI_MO
	}

}

