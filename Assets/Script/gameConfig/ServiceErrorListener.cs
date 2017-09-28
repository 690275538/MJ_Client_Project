using System;
using UnityEngine;

/**
 * 服务器返回错误
 */
namespace AssemblyCSharp
{
	public class ServiceErrorListener //:  MonoBehaviour
	{
		public ServiceErrorListener ()
		{
			SocketEventHandle.getInstance().serviceErrorNotice += serviceErrorNotice;
		}

		public void serviceErrorNotice(ClientResponse response){
			TipsManager.getInstance().setTips(response.message);
		}
	}
}

