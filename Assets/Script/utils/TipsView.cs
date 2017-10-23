using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TipsView : MonoBehaviour {

	public Text label;

	public void show(string str){
		label.text = str;
		Invoke ("onCompleted",4f);
	}

	private void move(){
		gameObject.transform.DOLocalMove (new Vector3(0,-100),0.7f).OnComplete(onCompleted);
	}

	public void onCompleted(){
		Destroy (gameObject);
	}
}
