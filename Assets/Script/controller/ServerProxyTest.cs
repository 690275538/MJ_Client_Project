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
				qq (APIS.BACK_LOGIN_RESPONSE, "{\"addWordCard\":false,\"currentRound\":1,\"endStatistics\":{\"100019\":{\"minggang\":1}},\"hong\":false,\"id\":73,\"ma\":0,\"name\":\"\",\"playerList\":[{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":10,\"day\":2,\"hours\":11,\"minutes\":18,\"month\":9,\"seconds\":14,\"time\":1507605494000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":16,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"11\",\"openid\":\"11\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"11\",\"uuid\":100015},\"chiArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0],\"chupais\":[30,32,8,3,33,26,20,0,18,13],\"commonCards\":7,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{\"5\":[1,-3]},\"gangScore\":-3,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"chi\":\"100015:22:23:24\",\"peng\":\"16\"},\"totalScore\":-3,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":true,\"paiArray\":[[0,1,0,1,0,0,0,0,0,2,1,1,1,0,0,0,3,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0]],\"pengArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"roomId\":359704,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":10,\"day\":2,\"hours\":11,\"minutes\":19,\"month\":9,\"seconds\":32,\"time\":1507605572000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":17,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"22\",\"openid\":\"22\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"22\",\"uuid\":100016},\"chiArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"chupais\":[33,17,30,27,8,32,11,8],\"commonCards\":0,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"peng\":\"29\"},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,0,0,0,0,1,1,0,0,0,0,0,0,2,1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,3,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0]],\"pengArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0],\"roomId\":359704,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":10,\"day\":2,\"hours\":15,\"minutes\":37,\"month\":9,\"seconds\":48,\"time\":1507621068000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":20,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"33\",\"openid\":\"33\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":5,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"33\",\"uuid\":100019},\"chiArray\":[0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"chupais\":[32,0,17,15,6,20,24,7,23],\"commonCards\":4,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{\"5\":[1,3]},\"gangScore\":3,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"gang\":\"100015:25:diangang\",\"chi\":\"100019:8:6:7\",\"peng\":\"31\"},\"totalScore\":3,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,2,1,1,0,0,0,4,0,0,0,0,0,3,0,0],[0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,1,0,0]],\"pengArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0],\"roomId\":359704,\"scores\":1000},{\"IP\":\"\",\"account\":{\"actualcard\":5,\"city\":\"深圳\",\"createtime\":{\"date\":10,\"day\":2,\"hours\":15,\"minutes\":22,\"month\":9,\"seconds\":51,\"time\":1507620171000,\"timezoneOffset\":-480,\"year\":117},\"headicon\":\"\",\"id\":19,\"isGame\":\"1\",\"lastlogintime\":{\"date\":1,\"day\":0,\"hours\":0,\"minutes\":0,\"month\":0,\"seconds\":0,\"time\":1483200000000,\"timezoneOffset\":-480,\"year\":117},\"managerUpId\":1,\"nickname\":\"44\",\"openid\":\"44\",\"prizecount\":70,\"province\":\"广东省\",\"roomcard\":4,\"sex\":1,\"status\":\"0\",\"totalcard\":5,\"unionid\":\"44\",\"uuid\":100018},\"chiArray\":[0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"chupais\":[32,27,17,18,19,7,24,19,1],\"commonCards\":5,\"gangArray\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"hasMopaiChupai\":true,\"huReturnObjectVO\":{\"gangAndHuInfos\":{},\"gangScore\":0,\"nickname\":\"\",\"paiArray\":[],\"totalInfo\":{\"chi\":\"100018:12:10:11\",\"peng\":\"28,2\"},\"totalScore\":0,\"uuid\":0},\"huType\":0,\"isOnLine\":true,\"isReady\":false,\"main\":false,\"paiArray\":[[0,0,3,0,0,1,0,0,0,0,1,1,1,1,0,2,0,0,0,0,0,0,1,0,0,0,0,0,3,0,0,0,0,0],[0,0,1,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0]],\"pengArray\":[0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0],\"roomId\":359704,\"scores\":1000}],\"roomId\":359704,\"roomType\":3,\"roundNumber\":8,\"sevenDouble\":false,\"xiaYu\":0,\"ziMo\":0}");
				break;
			case APIS.RETURN_ONLINE_REQUEST:
				qq(APIS.RETURN_ONLINE_RESPONSE,"{\"curAvatarIndex\":2,\"gameRound\":7,\"pickAvatarIndex\":3,\"putOffCardPoint\":23,\"surplusCards\":47}");
				break;
			case APIS.CHIPAI_REQUEST:
				qq (APIS.CHIPAI_NOTIFY, "{\"cardPoint\":6,\"onePoint\":7,\"twoPoint\":8,\"avatarId\":1}");


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

