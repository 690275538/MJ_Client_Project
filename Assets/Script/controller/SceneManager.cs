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
		GameObject gameOverPanel;
		
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
		/** 切换场景 **/
		public void changeToScene(SceneType type){
			if (curSceneType == type)
				return;

			GameObject newScenePanel = null;
			//新场景
			switch (type) {
			case SceneType.HOME:
				newScenePanel = loadPerfab ("Prefab/Panel_Home_View");
				break;
			case SceneType.LOGIN:
				newScenePanel = loadPerfab ("Prefab/Panel_Login_View");
				break;
			case SceneType.GAME:
				newScenePanel = loadPerfab ("Prefab/Panel_Game_View");
				break;
			}
			newScenePanel.GetComponent<ISceneView> ().open ();

			//旧场景
			curScenePanel.GetComponent<ISceneView> ().close ();

			//替换
			curScenePanel = newScenePanel;
			curSceneType = type;
		}
		public GameObject loadPerfab(string perfabName){
			GameObject g = GameObject.Instantiate (Resources.Load(perfabName)) as GameObject;
			g.transform.SetParent( parent);
			g.transform.localScale = Vector3.one;
			g.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
			g.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
			return g;
		}
		/**显示退出app面板**/
		public void showExitPanel(){
			if (exitPanel == null) {
				exitPanel = GameObject.Instantiate (Resources.Load("Prefab/Panel_Exit_View")) as GameObject;
			}
			exitPanel.transform.parent = curScenePanel.transform;
			exitPanel.transform.localScale = Vector3.one;
			exitPanel.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
			exitPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		}
		/**显示用户信息面板**/
		public void showUserInfoPanel(AvatarVO avo){
			GameObject g = loadPerfab ("Prefab/userInfo");
			g.GetComponent<ShowUserInfoScript> ().setUIData (avo);
		}

		/**显示 game over 结算面板**/
		public void showGameOverView(int type,GamingData data){
			if (gameOverPanel != null) {
				GameObject.Destroy (gameOverPanel);
				gameOverPanel = null;
			}
			gameOverPanel = loadPerfab ("prefab/Panel_Game_Over_View");
			gameOverPanel.GetComponent<GameOverView> ().setDisplaContent (type, data);
			gameOverPanel.transform.SetSiblingIndex (2);
		}
	}

}

