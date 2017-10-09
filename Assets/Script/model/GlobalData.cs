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

	public event Change SoundToggleChange;
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

	/**单局游戏结束服务器返回数据**/
	public static HupaiResponseVo hupaiResponseVo;
	/**全局游戏结束服务器返回数据**/
	public static FinalGameEndVo finalGameEndVo;

	/// <summary>
	/// 单局结算面板
	/// </summary>
	public static List<GameObject> singalGameOverList = new List<GameObject> ();



	//public SocketEventHandle socketEventHandle;
	/// <summary>
	/// 抽奖数据
	/// </summary>
	public static List<LotteryData> lotteryDatas;


	/**是否由用用户选择退出而退出的游戏**/
	public static bool isOverByPlayer = false;



	/**房间游戏规则信息**/
	public RoomVO roomVO;
	/**玩家个人信息**/
	public AvatarVO myAvatarVO;
	/**其他玩家个人信息**/
	public List<AvatarVO> playerList;


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

	private bool _soundToggle = true;
	/**声音开关**/
	public bool SoundToggle{
		get{
			return _soundToggle;
		}
		set{
			_soundToggle = value;
			if (SoundToggleChange != null) {
				SoundToggleChange ();
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

	public GamingData gamingData;
	/**
	 * 重新初始化数据
	*/
	public void reinitData ()
	{
		roomVO = new RoomVO (); 
		myAvatarVO = null;
		playerList = null;
		remainRoundCount = 0;
		gameStatus = GameStatus.UNDEFINED;
		gamingData = null;

		finalGameEndVo = null;
		singalGameOverList = new List<GameObject> ();
		lotteryDatas = null;
		isOverByPlayer = false;
	}
	public void resetDataForNewRoom()
	{
		roomVO = new RoomVO (); 
		playerList = null;
		gamingData = null;

		gameStatus = GameStatus.UNDEFINED;
	}






}
	
