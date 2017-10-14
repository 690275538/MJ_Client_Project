using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ServerProxyTest
	{

		public event AssemblyCSharp.ServerProxy.OnResponse onResponse;
		public event AssemblyCSharp.ServerProxy.OnDisconnect onDisconnect;

		private List<ClientResponse> _cache;

		public ServerProxyTest ()
		{
			_cache = new List<ClientResponse> ();
		}

		public void init ()
		{

		}

		public void connect ()
		{
			
		}

		public bool Connected {
			get {
				return true;
			}

		}
		public void requset (ChatRequest q)
		{
			Debug.Log("req: "+q.headCode.ToString ("x8")+" , "+q.userList.ToArray());
		}


		public void requset (ClientRequest q)
		{
			Debug.Log("req: "+q.headCode.ToString ("x8")+" , "+q.messageContent);
			switch (q.headCode) {
			case APIS.LOGIN_REQUEST:
				qq (APIS.BACK_LOGIN_RESPONSE, "{\"addWordCard\":false,\"currentRound\":1,\"endStatistics\":{},\"hong\":false,\"id\":19,\"ma\":0,\"name\":\"\",\"playerList\":[{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":14,\"day\":6,\"hours\":0,\"minutes\":51,\"month\":9,\"seconds\":22,\"time\":1507913482000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":12,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"11\",\"openid\":\"11\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":4,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"11\",\"uuid\":100011},\"chiArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"chupais\":[2,24,8,1],\"commonCards\":10,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"peng\":\"30\"},\"totalScore\":0,\"uuid\":0},\"huType\":1,\"isOnLine\":true,\"isReady\":false,\"main\":true,\"paiArray\":[[0,0,0,1,1,0,0,0,0,0,0,2,0,0,0,1,1,1,0,0,1,1,1,0,0,0,0,0,0,0,3,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0]],\"pengArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0],\"roomId\":324333,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":7,\"day\":6,\"hours\":22,\"minutes\":18,\"month\":9,\"seconds\":45,\"time\":1507385925000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":9,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"22\",\"openid\":\"22\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"22\",\"uuid\":100008},\"chiArray\":[0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"chupais\":[28,32],\"commonCards\":10,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"chi\":\"100008:6:7:8\"},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,0,0,0,0,1,1,1,1,0,1,0,0,2,0,1,0,1,0,0,0,0,1,1,0,1,0,0,0,1,0,0,0,0],[0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]],\"pengArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"roomId\":324333,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":7,\"day\":6,\"hours\":22,\"minutes\":18,\"month\":9,\"seconds\":48,\"time\":1507385928000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":10,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"33\",\"openid\":\"33\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"33\",\"uuid\":100009},\"chiArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"chupais\":[27,31],\"commonCards\":13,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,1,0,0,0,1,1,0,0,0,0,0,2,1,0,0,0,0,0,0,0,0,1,1,0,1,1,0,2,0,0,0,1,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]],\"pengArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"roomId\":324333,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":7,\"day\":6,\"hours\":22,\"minutes\":18,\"month\":9,\"seconds\":50,\"time\":1507385930000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":11,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"44\",\"openid\":\"44\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"44\",\"uuid\":100010},\"chiArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"chupais\":[27,32,31,10,5],\"commonCards\":0,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"peng\":\"18,0,9\"},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[3,0,0,1,0,0,0,0,0,3,0,0,0,0,0,1,0,0,3,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0],[1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]],\"pengArray\":[1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"roomId\":324333,\"scores\":1000}],\"roomId\":324333,\"roomType\":3,\"roundNumber\":8,\"sevenDouble\":false,\"xiaYu\":0,\"ziMo\":0}");
				break;
			case APIS.RETURN_ONLINE_REQUEST:
				qq (APIS.RETURN_ONLINE_RESPONSE, "{\"curAvatarIndex\":3,\"currentCardPoint\":3,\"gameRound\":7,\"pickAvatarIndex\":3,\"putOffCardPoint\":5,\"surplusCards\":72}");
				qq (APIS.HUPAI_RESPONSE, "{\"avatarList\":[{\"gangAndHuInfos\":{\"2\":[1,1]},\"gangScore\":0,\"nickname\":\"11\",\"paiArray\":[1,0,0,1,1,0,0,0,0,0,0,2,0,0,0,1,1,1,0,0,1,1,1,0,0,0,0,0,0,0,3,0,0,0],\"totalInfo\":{\"hu\":\"100010:0:d_self:1\",\"peng\":\"30\"},\"totalScore\":1,\"uuid\":100011},{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"22\",\"paiArray\":[0,0,0,0,0,1,1,1,1,0,1,0,0,2,0,1,0,1,0,0,0,0,1,1,0,1,0,0,0,1,0,0,0,0],\"totalInfo\":{\"chi\":\"100008:6:7:8\"},\"totalScore\":0,\"uuid\":100008},{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"33\",\"paiArray\":[0,1,0,0,0,1,1,0,0,0,0,0,2,1,0,0,0,0,0,0,0,0,1,1,0,1,1,0,2,0,0,0,1,0],\"totalInfo\":{},\"totalScore\":0,\"uuid\":100009},{\"gangAndHuInfos\":{\"3\":[1,-1]},\"gangScore\":0,\"nickname\":\"44\",\"paiArray\":[3,0,0,1,0,0,0,0,0,3,0,0,0,0,0,1,0,0,3,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0],\"totalInfo\":{\"hu\":\"100011:0:d_other:1\",\"peng\":\"18,0,9\"},\"totalScore\":-1,\"uuid\":100010}],\"currentScore\":\"100011:1,100008:0,100009:0,100010:-1,\",\"type\":\"0\",\"validMas\":[]}");
				break;
			case APIS.CHIPAI_REQUEST:
				//qq (APIS.CHIPAI_NOTIFY, "{\"cardPoint\":6,\"onePoint\":7,\"twoPoint\":8,\"avatarId\":1}");


				break;
			}
		}

		//吃
//		public void requset (ClientRequest q)
//		{
//			Debug.Log("req: "+q.headCode.ToString ("x8")+" , "+q.messageContent);
//			switch (q.headCode) {
//			case APIS.LOGIN_REQUEST:
//				qq(APIS.BACK_LOGIN_RESPONSE,"{\"addWordCard\":false,\"currentRound\":1,\"endStatistics\":{},\"hong\":false,\"id\":12,\"ma\":0,\"name\":\"\",\"playerList\":[{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":1,\"day\":0,\"hours\":22,\"minutes\":4,\"month\":9,\"seconds\":22,\"time\":1506866662000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":2,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"2345\",\"openid\":\"2345\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"2345\",\"uuid\":100001},\"chupais\":[6],\"commonCards\":13,\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":true,\"paiArray\":[[0,2,0,2,1,1,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,1,1],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]],\"roomId\":211304,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":23,\"day\":6,\"hours\":21,\"minutes\":56,\"month\":8,\"seconds\":30,\"time\":1506174990000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":1,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"1234\",\"openid\":\"1234\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"1234\",\"uuid\":100000},\"chupais\":[],\"commonCards\":0,\"hasMopaiChupai\":false,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"chi\":\"2345:6:7:8\"},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,1,0,0,1,1,1,2,2,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,1,1,0,0,0,0,1,0],[0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]],\"roomId\":211304,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":1,\"day\":0,\"hours\":22,\"minutes\":5,\"month\":9,\"seconds\":26,\"time\":1506866726000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":3,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"3456\",\"openid\":\"3456\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"3456\",\"uuid\":100002},\"chupais\":[],\"commonCards\":13,\"hasMopaiChupai\":false,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,0,0,0,0,0,1,1,1,0,0,1,0,0,1,1,2,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,1],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]],\"roomId\":211304,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":1,\"day\":0,\"hours\":22,\"minutes\":5,\"month\":9,\"seconds\":44,\"time\":1506866744000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":4,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"4567\",\"openid\":\"4567\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"4567\",\"uuid\":100003},\"chupais\":[],\"commonCards\":13,\"hasMopaiChupai\":false,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,2,2,0,0,1,2,0,0,1,0,0,0,1,0,1,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]],\"roomId\":211304,\"scores\":1000}],\"roomId\":211304,\"roomType\":3,\"roundNumber\":8,\"sevenDouble\":false,\"xiaYu\":0,\"ziMo\":0}");
//				break;
//			case APIS.CURRENT_STATUS_REQUEST:
//				qq(APIS.CURRENT_STATUS_RESPONSE,"{\"curAvatarIndex\":0,\"gameRound\":7,\"pickAvatarIndex\":0,\"putOffCardPoint\":6,\"surplusCards\":84}");
//				break;
//			case APIS.CHIPAI_REQUEST:
//				qq (APIS.CHIPAI_NOTIFY, "{\"cardPoint\":6,\"onePoint\":7,\"twoPoint\":8,\"avatarId\":1}");
//
//
//				break;
//			}
//		}
//
//		//正常登录流程
//		public void requset (ClientRequest q)
//		{
//			Debug.Log("req: "+q.headCode.ToString ("x8")+" , "+q.messageContent);
//			switch (q.headCode) {
//			case APIS.LOGIN_REQUEST:
//				qq(APIS.LOGIN_RESPONSE,"{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":23,\"day\":6,\"hours\":21,\"minutes\":56,\"month\":8,\"seconds\":30,\"time\":1506174990000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":1,\"isGame\":\"0\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"1234\",\"openid\":\"1234\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"1234\",\"uuid\":100000},\"chupais\":[],\"commonCards\":0,\"hasMopaiChupai\":false,\"huReturnObjectVO\":null,\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[],\"roomId\":0,\"scores\":1000}");
//				break;
//			case APIS.CREATEROOM_REQUEST:
//				qq(APIS.CREATEROOM_RESPONSE,"361238");
//				break;
//			case APIS.PrepareGame_MSG_REQUEST:
//				qq (APIS.PrepareGame_MSG_RESPONSE, " {\"avatarIndex\":0}");
//
//				qq (APIS.JOIN_ROOM_NOICE, "{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":1,\"day\":0,\"hours\":22,\"minutes\":4,\"month\":9,\"seconds\":22,\"time\":1506866662000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":2,\"isGame\":\"0\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"2345\",\"openid\":\"2345\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"2345\",\"uuid\":100001},\"chupais\":[],\"commonCards\":0,\"hasMopaiChupai\":false,\"huReturnObjectVO\":null,\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[],\"roomId\":269240,\"scores\":1000}");
//				qq (APIS.PrepareGame_MSG_RESPONSE, "{\"avatarIndex\":1}");
//
//
//				qq (APIS.JOIN_ROOM_NOICE, "{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":1,\"day\":0,\"hours\":22,\"minutes\":5,\"month\":9,\"seconds\":26,\"time\":1506866726000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":3,\"isGame\":\"0\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"3456\",\"openid\":\"3456\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"3456\",\"uuid\":100002},\"chupais\":[],\"commonCards\":0,\"hasMopaiChupai\":false,\"huReturnObjectVO\":null,\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[],\"roomId\":269240,\"scores\":1000}");
//				qq (APIS.PrepareGame_MSG_RESPONSE, "{\"avatarIndex\":2}");
//
//
//				qq (APIS.JOIN_ROOM_NOICE, "{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":1,\"day\":0,\"hours\":22,\"minutes\":5,\"month\":9,\"seconds\":44,\"time\":1506866744000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":4,\"isGame\":\"0\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"4567\",\"openid\":\"4567\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"4567\",\"uuid\":100003},\"chupais\":[],\"commonCards\":0,\"hasMopaiChupai\":false,\"huReturnObjectVO\":null,\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[],\"roomId\":269240,\"scores\":1000}");
//				qq (APIS.PrepareGame_MSG_RESPONSE, "{\"avatarIndex\":3}");
//
//				qq (APIS.STARTGAME_RESPONSE_NOTICE, "{\"bankerId\":0,\"paiArray\":[[0,0,0,0,0,0,1,1,1,0,0,1,1,0,0,0,1,2,0,0,0,1,0,0,1,2,0,0,0,0,1,1,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]]}");
//
//
//				break;
//			}
//		}
		public void qq(int headCode,string msg){
			ClientResponse res = new ClientResponse ();
			res.headCode = headCode;
			res.message = msg;
			res.status = 1;
			onSocketData (res);
		}
		private void onSocketData (ClientResponse response)
		{
			_cache.Add (response);
			SocketEventHandle.getInstance ().addResponse (response);
		}

		public void FixedUpdate ()
		{
			while (_cache.Count > 0) {
				Debug.Log("res: "+_cache [0].headCode.ToString ("x8")+" , "+_cache [0].message);
				try {
					onResponse (_cache [0]);
				} catch (Exception e) {
					Debug.Log ("命令出错：" + _cache [0].headCode.ToString ("x8") + " " + _cache [0].message);
					Debug.Log (e.ToString ());
				}

				_cache.RemoveAt (0);
			}

		}





	}
}

