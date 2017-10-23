using UnityEngine;
using System.Collections;
using DG.Tweening;
using LitJson;

namespace AssemblyCSharp
{
	/**控制声音，申请解散房间，打开快捷信息面板**/
	public class TRMenuBarView : MonoBehaviour
	{
		private GameView _host;
		public GameObject soundOnBtn;
		public GameObject soundOffBtn;
		public GameObject showMenuBarBtn;
		private bool _isMenuBarVisible = false;


		private GameObject _dissoDialog;
		private bool _isDissoliving = false;
		private int _disagreeCount = 0;
		// Use this for initialization
		public void init(GameView host)
		{
			_host = host;
			GlobalData.getInstance ().SoundToggleChange += onSoundToggleChange;
			GameManager.getInstance ().Server.onResponse += onResponse;
			onSoundToggleChange ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void onSoundOnBtn ()
		{
			GlobalData.getInstance ().SoundToggle = true;
		}

		public void onSoundOffBtn ()
		{
			GlobalData.getInstance ().SoundToggle = false;
		}

		private void onSoundToggleChange ()
		{
		
			soundOnBtn.SetActive (!GlobalData.getInstance ().SoundToggle);
			soundOffBtn.SetActive (GlobalData.getInstance ().SoundToggle);
		}

		public void tabVisible ()
		{
			if (!_isMenuBarVisible) {
				gameObject.transform.DOLocalMove (new Vector3 (65, -5), 0.4f);
//				showMenuBarBtn.transform.Rotate( new Vector3 (0, 0, -180));
			} else {
				gameObject.transform.DOLocalMove (new Vector3 (65, 94), 0.4f);
//				showMenuBarBtn.transform.Rotate ( new Vector3 (0, 0, 180));
			}

			_isMenuBarVisible = !_isMenuBarVisible;
		}

		void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.DISSOLIVE_ROOM_RESPONSE:
				dissoliveRoomResponse (response);
				break;
			case APIS.OFFLINE_NOTICE://离线通知
				offlineNotice (response);
				break;
			}
		}
		/**用户离线回调**/
		public void offlineNotice (ClientResponse response)
		{
			int uuid = int.Parse (response.message);
			int index = _host.Data.toAvatarIndex (uuid);
			//申请解散房间过程中，有人掉线，直接不能解散房间
			if (_isDissoliving) {
				closeDialog ();
				TipsManager.getInstance ().setTips ("由于" + _host.Data.AvatarList [index].account.nickname + "离线，系统不能解散房间。");
			}
		}
		/**申请或同意解散房间请求**/ 
		public void showDissoliveDialog ()
		{
			if (GlobalData.getInstance ().gameStatus == GameStatus.GAMING) {
				TipsManager.getInstance ().loadDialog ("申请解散房间", "你确定要申请解散房间？", onOK, onCancle);
			} else {
				TipsManager.getInstance ().setTips ("还没有开始游戏，不能申请退出房间");
			}

		}

		public void  onOK ()
		{
			DissoliveRoomRequestVo vo = new DissoliveRoomRequestVo ();
			vo.roomId = GlobalData.getInstance ().roomVO.roomId;
			vo.type = "0";
			GameManager.getInstance ().Server.requset (APIS.DISSOLIVE_ROOM_REQUEST, JsonMapper.ToJson (vo));
			_isDissoliving = true;
		}



		/**申请解散房间回调**/
		public void dissoliveRoomResponse (ClientResponse response)
		{
			DissoliveRoomResponseVo dvo = JsonMapper.ToObject<DissoliveRoomResponseVo> (response.message);
			string plyerName = dvo.accountName;
			if (dvo.type == "0") {
				_isDissoliving = true;
				_dissoDialog = SceneManager.getInstance().loadPerfab ("Prefab/Panel_Apply_Exit");
				_dissoDialog.GetComponent<VoteView> ().iniUI (plyerName);
			} else if (dvo.type == "3") {

				_isDissoliving = false;
				GlobalData.isOverByPlayer = true;
				closeDialog ();

			} else if (dvo.type == "2") {
				_disagreeCount += 1;
				if (_disagreeCount >= 2) {
					_isDissoliving = false;
					TipsManager.getInstance ().setTips ("同意解散房间申请人数不够，本轮投票结束，继续游戏");
					closeDialog ();
				}
			}
		}

		public void closeDialog ()
		{
			if (_dissoDialog != null) {
				GameObject.Destroy (_dissoDialog);
				_dissoDialog = null;
			}
		}


		private void onCancle ()
		{

		}
		void OnDestroy(){
			GameManager.getInstance ().Server.onResponse -= onResponse;
		}
	}
}