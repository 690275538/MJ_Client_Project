using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;
using LitJson;

public class ReplayView : MonoBehaviour
{

	public GameObject liujuEffectGame;
	public GameObject playBtn;
	public GameObject stopBtn;
	public Text processText;
	public GameObject genZhuang;
	public GameObject exitPanel;
	//======================================

	private GameObject zhuamaPanel;

	private int timeNum = 0;
	private int curBehaviorIndex = 0;
	private GamePlayResponseVo replayVO = new GamePlayResponseVo ();
	private List<GameBehaviourVO> usedList;


	public List<Transform> PGCParents;
	public List<Transform> HandParents;
	public List<Transform> TableParents;
	public List<ReplayPlayerView> PlayerItemViews;

	private ReplayUIHelper _uiHelper;
	private ReplayData _data;
	private ReplayTableView _tableView;

	public ReplayData Data {
		get {
			return	_data;
		}
	}

	public ReplayView ()
	{
		_data = new ReplayData ();
		_uiHelper = new ReplayUIHelper ();
	}
	// Use this for initialization
	void Start ()
	{
		_tableView = gameObject.GetComponent<ReplayTableView> ();
		_tableView.init (_data);
		_uiHelper.init (this);
		usedList = new List<GameBehaviourVO> ();
		GameManager.getInstance ().Server.onResponse += onResponse;

	}

	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.REPLAY_RESPONSE://回放
			replayVO = JsonMapper.ToObject<GamePlayResponseVo> (response.message);
			_data.replayVO = replayVO;
			_tableView.updateRule ();

			_data.AvatarList = replayVO.playerItems;

			_uiHelper.start ();
			_data.isPlaying = true;
			break;
		}
	}
		

	// Update is called once per frame
	void Update ()
	{
		if (_data.isPlaying) {
			timeNum--;
			if (timeNum <= 0) {
				timeNum = 70;
				if (curBehaviorIndex < replayVO.behavieList.Count) {
					processText.text = "播放进度：" + (int)(curBehaviorIndex * 100 / replayVO.behavieList.Count) + "%";
					stepAction ();
				} else {
					processText.text = "播放进度：100%";
					_data.isPlaying = false;
				}
			}
		}
	}

	private void stepAction ()
	{
		GameBehaviourVO gvo = replayVO.behavieList [curBehaviorIndex];
		usedList.Insert (0, gvo);

		Debug.Log (">" + curBehaviorIndex + " : " + JsonMapper.ToJson (gvo));
		int avatarIndex = gvo.accountindex_id;
		int cardPoint = int.Parse (gvo.cardIndex);
		if (gvo.type == 1) {
			_uiHelper.putoutCard (avatarIndex, cardPoint);
		} else if (gvo.type == 2) {
			_data.remainCardNum--;
			_uiHelper.pickCard (avatarIndex, cardPoint);
		} else if (gvo.type == 4) {
			_uiHelper.pengCard (avatarIndex, cardPoint);
		} else if (gvo.type == 5) {
			_uiHelper.gangCard (avatarIndex, cardPoint, gvo.gangType);
		} else if (gvo.type == 6) {
			_uiHelper.huCard (avatarIndex, cardPoint);
		} else if (gvo.type == 7) {
			_uiHelper.qiangGangHu (avatarIndex, cardPoint);
		} else if (gvo.type == 8) {
			if (replayVO.roomvo.roomType == GameType.HUA_SHUI) {//划水麻将不显示码框
			} else {
				zhuama (gvo.ma, gvo.valideMa);
			}
		} else if (gvo.type == 9) {
			liuju ();
		}
		_data.pickIndex = avatarIndex;
		curBehaviorIndex++;
	}

	public void play_Click ()
	{
		_data.isPlaying = true;
		playBtn.SetActive (false);
		stopBtn.SetActive (true);
	}

	public void stop_Click ()
	{
		_data.isPlaying = false;
		playBtn.SetActive (true);
		stopBtn.SetActive (false);
	}

	public void front_Click ()
	{
		
	}


	public void next_Click ()
	{
		timeNum = 1;
	}

	public void exit_Click ()
	{
		exitPanel.SetActive (true);
	}

	public void exitSure_Click ()
	{
		Destroy (gameObject);
	}

	public void exitCancel_Click ()
	{
		exitPanel.SetActive (false);
	}


	private void zhuama (string allMas, List<int> vailedMa)
	{
		if (allMas == "" || allMas == null) {
			return;
		}
		if (zhuamaPanel == null) {
			
			zhuamaPanel = SceneManager.getInstance ().loadPerfab ("prefab/Panel_ZhuaMa");
			zhuamaPanel.GetComponent<ZhuaMaView> ().arrageMas (allMas, vailedMa);
		}
		Invoke ("destroyZhuaMa", 7);
	}
	private void destroyZhuaMa(){
		if (zhuamaPanel != null) {
			GameObject.Destroy (zhuamaPanel);
			zhuamaPanel = null;
		}
	}
	private void liuju ()
	{
		liujuEffectGame.SetActive (true);
	}

	void OnDestroy ()
	{
		GameManager.getInstance ().Server.onResponse -= onResponse;
	}
}
