using System;
using UnityEngine;

/**
 * prefab 管理器
 */ 
namespace AssemblyCSharp{
public class PrefabManage : MonoBehaviour
{
	public PrefabManage ()
	{
	}
	public static GameObject loadPerfab(string perfabName){

		GameObject panelCreateDialog = Instantiate (Resources.Load(perfabName)) as GameObject;
		panelCreateDialog.transform.parent = GameManager.getInstance ().Root.transform;;
		panelCreateDialog.transform.localScale = Vector3.one;
		panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		return panelCreateDialog;
	}
}
}