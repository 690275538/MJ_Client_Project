using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;


public class RuleView : MonoBehaviour {
	private List<GameType> _rules;
	private Dictionary<GameType,string> map;
	public Text ruleContent;
	private int _curIndex = 0;
	public Text titleText;

	void Start () {
		_rules = new List<GameType> ();
		_rules.Add (GameType.JI_PING_HU);
		_rules.Add (GameType.ZHUAN_ZHUAN);
		_rules.Add (GameType.HUA_SHUI);

		map = new Dictionary<GameType,string> ();
		updateContent ();
	}

	public void closeDialog(){
		Destroy (gameObject);
	}

	public void ruleUp(){
		if (_curIndex > 0) {
			_curIndex -= 1;
			updateContent ();
		}
	}
	public void ruleDown(){
		if (_curIndex <_rules.Count-1) {
			_curIndex += 1;
			updateContent ();
		}
	}
	private void updateContent(){
		var gt = _rules [_curIndex];
		if (!map.ContainsKey (gt)) {
			string text = "";
			if (gt == GameType.JI_PING_HU) {
				text = (Resources.Load ("text/JPH") as TextAsset).text;
			}else if (gt == GameType.ZHUAN_ZHUAN) {
				text = (Resources.Load ("text/ZZ") as TextAsset).text;
			}else if (gt == GameType.HUA_SHUI) {
				text = (Resources.Load ("text/HS") as TextAsset).text;
			}
			map [gt] = text;
		}
		ruleContent.text = map [gt];
		if (gt == GameType.JI_PING_HU) {
			ruleContent.rectTransform.sizeDelta = new Vector2 (1087, 2600);
			titleText.text = "鸡平胡";
		}else if (gt == GameType.ZHUAN_ZHUAN) {
			ruleContent.rectTransform.sizeDelta = new Vector2 (1087, 644);
			titleText.text = "转转麻将";
		}else if (gt == GameType.HUA_SHUI) {
			ruleContent.rectTransform.sizeDelta = new Vector2 (1087, 644);
			titleText.text = "划水麻将";
		}
	}

}
