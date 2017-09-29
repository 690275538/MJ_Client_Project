using System;

namespace AssemblyCSharp
{
	public class DataManager
	{
		public DataManager ()
		{
		}
		public void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.OTHER_TELE_LOGIN:
				
				break;
			}

		}
	}
}

