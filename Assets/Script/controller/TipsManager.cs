using UnityEngine;
using System.Collections;
using DG.Tweening;

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
		GameObject temp = GameObject.Instantiate (Resources.Load ("Prefab/TipPanel") as GameObject);
		temp.transform.parent = GlobalData.getInstance().Stage.transform;
		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition =new Vector3 (0,-300);
		temp.GetComponent<TipPanelScript> ().setText (str);
		temp.GetComponent<TipPanelScript> ().startAction();

	}


	public void loadDialog(string titlestr,string msgstr,DialogPanelScript.ButtonOnClick yesCallBack,DialogPanelScript.ButtonOnClick noCallBack){
		GameObject temp = GameObject.Instantiate (Resources.Load ("Prefab/Image_Base_Dialog") as GameObject);
		temp.transform.parent = parent;
		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition = Vector3.zero;
		temp.GetComponent<DialogPanelScript> ().setContent (titlestr,msgstr,true,yesCallBack,noCallBack);
	}







}
