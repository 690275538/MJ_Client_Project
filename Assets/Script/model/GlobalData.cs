using System;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public class GlobalData
{
	public delegate void Change();
	public event Change noticeChange;
	public event Change prizeCountChange;

	private static GlobalData _instance;

	public static GlobalData getInstance ()
	{
		if (_instance == null) {
			_instance = new GlobalData ();
		}
		return _instance;
	}

	/**是否到自己出牌**/
	public static bool isDrag = false;
	/**单局游戏结束服务器返回数据**/
	public static HupaiResponseVo hupaiResponseVo;
	/**全局游戏结束服务器返回数据**/
	public static FinalGameEndVo finalGameEndVo;

	public static int mainUuid;


	/// <summary>
	/// 声音开关
	/// </summary>
	public static bool soundToggle = true;

	/// <summary>
	/// 单局结算面板
	/// </summary>
	public static List<GameObject> singalGameOverList = new List<GameObject> ();



	//public SocketEventHandle socketEventHandle;
	/// <summary>
	/// 抽奖数据
	/// </summary>
	public static List<LotteryData> lotteryDatas;


	/**是否处于申请解散房间状态**/
	public static bool isDissoliving = false;
	/**是否由用用户选择退出而退出的游戏**/
	public static bool isOverByPlayer = false;



	/**房间游戏规则信息**/
	public RoomVO roomVO;
	/**玩家个人信息**/
	public AvatarVO myAvatarVO;
	/**其他玩家个人信息**/
	public List<AvatarVO> playerList;

	/**重返房间**/
	public bool isReEnter = false;

	/**麻将剩余局数**/
	public int remainRoundCount = 0;

	/**游戏公告**/
	List<String> noticeMsgs = new List<string> ();
	public List<String> NoticeMsgs{
		get{
			return noticeMsgs;
		}
		set{
			noticeMsgs = value;
			if (noticeChange != null) {
				noticeChange ();
			}
		}

	}


	public int PrizeCount{
		get{
			if (myAvatarVO == null)
				return 0;
			return myAvatarVO.account.prizecount;
		}
		set{
			if (myAvatarVO != null) {
				myAvatarVO.account.prizecount = value;
				if (prizeCountChange != null) {
					prizeCountChange ();
				}
			} else {
				Debug.Log ("数据下发时机错误");
			}
		}

	}

	/**游戏状态**/
	public GameStatus gameStatus;

	/**
	 * 重新初始化数据
	*/
	public void reinitData ()
	{
		roomVO = new RoomVO (); 
		myAvatarVO = null;
		playerList = null;
		isReEnter = false;
		remainRoundCount = 0;
		gameStatus = GameStatus.UNDEFINED;

		isDrag = false;
		finalGameEndVo = null;
		singalGameOverList = new List<GameObject> ();
		lotteryDatas = null;
		isDissoliving = false;
		isOverByPlayer = false;
	}
	public void resetDataForNewRoom()
	{
		roomVO = new RoomVO (); 
		playerList = null;
		isReEnter = false;

		gameStatus = GameStatus.UNDEFINED;
	}






}
	
