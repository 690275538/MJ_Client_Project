using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class GiftList
	{
		public List<GiftItemVO> data;
		public string type;

		public GiftList ()
		{
		}
	}
}

