using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankView : MonoBehaviour {
	public Transform itemParent;
	public Text selfRank;
	public Image selfHeaderIcon;
	public Text selfRankDes;
	public Text selfScore;
	public Button giftButton;
	// Use this for initialization
	void Start () {
		setRankData ();
	}


	/**设置排行榜中的数据
	 * 
	 */ 
	public void setRankData(){
		for (int i = 0; i < 20; i++) {
			GameObject itemTemp = Instantiate (Resources.Load("Prefab/home/RankCR")) as GameObject;
			itemTemp.transform.SetParent( itemParent);
			itemTemp.transform.localScale = Vector3.one;
		}
	}

	/**
	*关闭对话框
	*/
	public void closeDialog(){
		Destroy(this.gameObject );
	}



}
