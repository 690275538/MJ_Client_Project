using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SceneManager
	{
		private static SceneManager _instance;

		public static SceneManager getInstance(){
			if (_instance == null) {
				_instance = new SceneManager ();
			}
			return _instance;
		}

		GameObject curScenePanel;

		public GameObject CurScenePanel {
			get {
				return curScenePanel;
			}
		}

		GameObject exitPanel;
		
		SceneType curSceneType;
		public SceneType CurSceneType {
			get {
				return curSceneType;
			}
		}



		/** Root Transform **/
		Transform parent;
		public void init(Transform parent,GameObject login){
			this.parent = parent;
			curSceneType = SceneType.LOGIN;
			curScenePanel = login;
		}

		public void changeToScene(SceneType type){
			if (curSceneType == type)
				return;

			GameObject newScenePanel = null;
			//新场景
			switch (type) {
			case SceneType.HOME:
				newScenePanel = loadPerfab ("Prefab/Panel_Home_View");
				GlobalData.homePanel = newScenePanel;
				break;
			case SceneType.LOGIN:
				newScenePanel = loadPerfab ("Prefab/Panel_Login_View");
				break;
			case SceneType.GAME:
				newScenePanel = loadPerfab ("Prefab/Panel_GamePlay");
				break;
			}
			newScenePanel.GetComponent<ISceneView> ().open ();

			//旧场景
			curScenePanel.GetComponent<ISceneView> ().close ();

			//替换
			curScenePanel = newScenePanel;
			curSceneType = type;
		}
		private GameObject loadPerfab(string perfabName){
			GameObject g = GameObject.Instantiate (Resources.Load(perfabName)) as GameObject;
			g.transform.parent = parent;
			g.transform.localScale = Vector3.one;
			g.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
			g.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
			return g;
		}
		public void showExitPanel(){
			if (exitPanel == null) {
				exitPanel = GameObject.Instantiate (Resources.Load("Prefab/Panel_Exit_View")) as GameObject;

			}
			exitPanel.transform.parent = curScenePanel.transform;
			exitPanel.transform.localScale = Vector3.one;
			exitPanel.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
			exitPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		}
	}

}

