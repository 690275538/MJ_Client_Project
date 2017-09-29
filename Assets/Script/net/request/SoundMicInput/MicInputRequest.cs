using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class MicInputRequest:ChatRequest
	{
		public MicInputRequest (List<int> _userList, byte[] sound)
		{
			headCode = APIS.MicInput_Request;
			myUUid = GlobalData.myAvatarVO.account.uuid;
			userList = _userList;
			ChatSound = sound;
		}
	}
}

