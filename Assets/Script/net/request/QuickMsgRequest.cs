using System;

namespace AssemblyCSharp
{
	public class QuickMsgRequest : ClientRequest
	{
		public QuickMsgRequest (int codeIndex,int uuid)
		{
			headCode = APIS.MessageBox_Request;
			msg = codeIndex + "|"+uuid;
		}
	}
}

