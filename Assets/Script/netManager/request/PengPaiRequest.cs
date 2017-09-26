using System;
using LitJson;
namespace AssemblyCSharp
{
	public class PengPaiRequest : ClientRequest
	{
		public PengPaiRequest (CardVO cardvo)
		{
			headCode = APIS.PENGPAI_REQUEST;
			messageContent = JsonMapper.ToJson (cardvo);;
		}
	}
}

