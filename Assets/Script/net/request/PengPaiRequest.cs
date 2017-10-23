using System;
using LitJson;
namespace AssemblyCSharp
{
	public class PengPaiRequest : ClientRequest
	{
		public PengPaiRequest (CardVO cardvo)
		{
			headCode = APIS.PENGPAI_REQUEST;
			msg = JsonMapper.ToJson (cardvo);;
		}
	}
}

