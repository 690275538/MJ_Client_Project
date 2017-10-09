using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
/**
 * 单人投票结果
*/
namespace AssemblyCSharp
{
	public class VoteResultItemView :MonoBehaviour
	{
		public Text nameTf;
		public Text resultTf;
		public VoteResultItemView ()
		{
		}

		public void  setInitVal(string namestr,string resultstr){
			nameTf.text = namestr;
			resultTf.text = resultstr;
		}

		public void changeResult(string resultstr){
			resultTf.text = resultstr;
		}
			
	}
}

