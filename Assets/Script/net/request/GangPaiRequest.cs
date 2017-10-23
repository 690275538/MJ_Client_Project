using System;
using LitJson;
namespace AssemblyCSharp
{
	public class GangPaiRequest : ClientRequest
	{
		public GangPaiRequest (int cardPoint,int gangType)
		{
			headCode = APIS.GANGPAI_REQUEST;
            GangRequestVO vo = new GangRequestVO();
		    vo.cardPoint = cardPoint;
		    vo.gangType = gangType;
		    msg = JsonMapper.ToJson(vo);
		}
	}
}

