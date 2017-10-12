using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class FinalGameEndItemVo
	{
		public int uuid;
		public int zimo;
		public int jiepao;
		public int dianpao;
		public int minggang;
		public int angang;
		public int scores;

		private string nickname;
		private string icon;
		private bool isWiner = false;
		private bool isPaosshou = false;
		private bool isMain = false;
		public FinalGameEndItemVo ()
		{
			
		}

		public void  setIsWiner(bool val){
			isWiner = val;
		}
		public bool  getIsWiner(){
			return isWiner;
		}

		public bool getIsPaoshou(){
			return isPaosshou;
		}
		public void setIsPaoshou(bool val){
			isPaosshou = val;
		}

		public string  getNickname(){
			return nickname ;
		}
		public void  setNickname(string val){
			nickname = val;
		}

		public string getIcon(){
			return icon;
		}
		public void setIcon(string val){
			icon = val;
		}

		public bool getIsMain(){
			return isMain;
		}
		public void setIsMain(bool  val){
			isMain = val;
		}

	}
}

