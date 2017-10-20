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
		titleText.text = GameHelper.getHelper (gt).getName ();

		RectTransform ctf = (RectTransform)ruleContent.rectTransform;
		RectTransform ptf = (RectTransform)ruleContent.rectTransform.parent;

		if (gt == GameType.JI_PING_HU) {
			ctf.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 2600);
			ptf.GetComponent<ScrollRect> ().scrollSensitivity = 8;
		}else if (gt == GameType.ZHUAN_ZHUAN) {
			ctf.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 644);
			ptf.GetComponent<ScrollRect> ().scrollSensitivity = 1;
		}else if (gt == GameType.HUA_SHUI) {
			ctf.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 644);
			ptf.GetComponent<ScrollRect> ().scrollSensitivity = 1;
		}
		var delta = ptf.rect.height - ctf.rect.height;
		ctf.localPosition = new Vector3 (0, delta, 0);
	}

}
