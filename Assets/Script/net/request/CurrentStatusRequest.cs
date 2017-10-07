using System;

namespace AssemblyCSharp
{
	public class CurrentStatusRequest:ClientRequest
	{
		public CurrentStatusRequest ()
		{
			headCode = APIS.CURRENT_STATUS_REQUEST;
			messageContent = "dd";
		}
	}
}

