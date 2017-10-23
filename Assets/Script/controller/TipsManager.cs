using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace AssemblyCSharp
{
	public class TipsManager  {

		private static TipsManager _instance;

		public static TipsManager getInstance(){
			if (_instance == null) {
				_instance = new TipsManager ();
			}
			return _instance;
		}

		/** Stage Transform **/
		Transform parent;
		public void init(Transform parent){
			this.parent = parent;
		}
		public void setTips(string str){
			GameObject temp = GameObject.Instantiate (Resources.Load ("Prefab/W_TipsUI") as GameObject);
			temp.transform.SetParent( parent);
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition =new Vector3 (0,-300);
			temp.GetComponent<TipsView> ().show (str);

		}


		public void loadDialog(string titlestr,string msgstr,OnClick yesCallBack,OnClick noCallBack){
			GameObject temp = GameObject.Instantiate (Resources.Load ("Prefab/W_DialogUI") as GameObject);
			temp.transform.SetParent(parent);
			temp.transform.localScale = Vector3.one;
			temp.transform.localPosition = Vector3.zero;
			temp.GetComponent<BaseDialogView> ().setContent (titlestr,msgstr,true,yesCallBack,noCallBack);
		}


	}
}