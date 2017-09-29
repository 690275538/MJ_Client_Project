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
	private static GlobalData _instance;
	public static GlobalData getInstance(){
		if (_instance == null) {
			_instance = new GlobalData ();
		}
		return _instance;
	}

	/**是否到自己出牌**/
    public static bool isDrag = false;
	/**登陆返回数据**/
	public static AvatarVO myAvatarVO;
	/**加入房间返回数据**/
	public static RoomJoinResponseVo roomJoinResponseData;
	/**房间游戏规则信息**/
	public static RoomVO roomVO = new RoomVO ();
	/**单局游戏结束服务器返回数据**/
	public static HupaiResponseVo hupaiResponseVo;
	/**全局游戏结束服务器返回数据**/
	public static FinalGameEndVo finalGameEndVo;

	public static int mainUuid;
	/**房间成员信息**/
	public static List<AvatarVO> roomAvatarVoList;

//	public static Dictionary<int, Account > palyerBaseInfo = new Dictionary<int, Account> (); 


	/**麻将剩余局数**/
	public static int surplusTimes ;
	/**总局数**/
	public static int totalTimes;
	/**重新加入房间的数据**/
	public static RoomJoinResponseVo reEnterRoomData;

	/// <summary>
	/// 声音开关
	/// </summary>
	public static bool soundToggle = true;

	/// <summary>
	/// 单局结算面板
	/// </summary>
	public static List<GameObject> singalGameOverList = new List<GameObject>();



	//public SocketEventHandle socketEventHandle;
	/// <summary>
	/// 抽奖数据
	/// </summary>
	public static List<LotteryData> lotteryDatas;
	public static bool isonApplayExitRoomstatus = false;//是否处于申请解散房间状态
	public static bool isOverByPlayer = false;//是否由用用户选择退出而退出的游戏
	public static LoginVo loginVo;//登录数据
	public static List<String> noticeMegs = new List<string>();



	private GameObject stage;
	/** UI Stage **/
	public GameObject Stage {
		get {
			return stage;
		}
	}


	private GameObject root;
	/** Game Root inside the Stage **/
	public GameObject Root {
		get {
			return root;
		}
	}

	/**微信接口**/
	public WechatOperateScript wechatOperate;
	/**
	 * 重新初始化数据
	*/
	public static void reinitData(){
		isDrag = false;
		myAvatarVO = null;
		roomJoinResponseData = null;
		roomVO=new RoomVO(); 
		hupaiResponseVo = null;
		finalGameEndVo = null;
		roomAvatarVoList = null;
		surplusTimes = 0;
		totalTimes = 0;
		reEnterRoomData = null;
		singalGameOverList =   new List<GameObject>();
		lotteryDatas = null;
		isonApplayExitRoomstatus = false;
		isOverByPlayer = false;
		loginVo = null;
	}


	public void init(GameObject stage,GameObject root,GameObject login){
		
		this.stage = stage;
		this.root = root;
		wechatOperate = stage.GetComponent<WechatOperateScript>();

		TipsManager.getInstance ().init (stage.transform);
		SceneManager.getInstance ().init (root.transform,login);
	}


	public string getIpAddress()
	{
		string tempip = "";
//		try
//		{
//			WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
//			Stream s = wr.GetResponse().GetResponseStream();
//			StreamReader sr = new StreamReader(s, Encoding.Default);
//			string all = sr.ReadToEnd(); //读取网站的数据
//
//			int start = all.IndexOf("[")+1;
//		    int end = all.IndexOf("]");
//		    int count = end-start;
//			tempip = all.Substring(start,count);
//			sr.Close();
//			s.Close();
//		}
//		catch
//		{
//		}
		return tempip;
	}



}
	
