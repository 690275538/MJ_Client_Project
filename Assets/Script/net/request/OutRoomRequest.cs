using System;

namespace AssemblyCSharp
{
	public class OutRoomRequest :ClientRequest
	{
		
		public OutRoomRequest (string sendMsg)
		{
			headCode = APIS.OUT_ROOM_REQUEST;
			msg = sendMsg;
		}
	}
}

