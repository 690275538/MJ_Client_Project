using System;

namespace AssemblyCSharp
{
	public class CurrentStatusRequest:ClientRequest
	{
		public CurrentStatusRequest ()
		{
			headCode = APIS.RETURN_ONLINE_REQUEST;
			messageContent = "dd";
		}
	}
}

