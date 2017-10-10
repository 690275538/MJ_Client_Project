using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class MicrophoneView : MonoBehaviour
	{
		public float WholeTime = 10f;
		public GameObject recordBtn;
		public GameObject cdCircle;
		private GameView _host;
		private Boolean _isRecording = false;

		public void init (GameView host){
			_host = host;
			GameManager.getInstance ().Server.onResponse += onResponse;
		}
		void onResponse (ClientResponse response)
		{
			switch (response.headCode) {
			case APIS.MicInput_Response://别人说话
				micInputNotice (response);
				break;
			}
		}
		void FixedUpdate ()
		{
			if (_isRecording) {
				WholeTime -= Time.deltaTime;
				cdCircle.GetComponent<Slider> ().value = WholeTime;
				if (WholeTime <= 0) {
					OnPointerUp ();
				}
			}
		}

		public void OnPointerDown ()
		{
        
			if (getUserList ().Count > 0) {
				_isRecording = true;
				recordBtn.SetActive (true);
				MicrophoneManager.getInstance ().StartRecord ();
			} else {
				TipsManager.getInstance ().setTips ("房间里只有你一个人，不能发送语音");
			}
		}

		public void OnPointerUp ()
		{
			if (_isRecording) {
				_isRecording = false;
				recordBtn.SetActive (false);
				WholeTime = 10;
				Byte[] outData = MicrophoneManager.getInstance ().StopRecord ();
				List<int> uuidList = getUserList ();
				if (uuidList.Count > 0 && outData != null) {
					_host.UIHelper.getCardGOs (Direction.B).PlayerItem.showChatAction ();
					GameManager.getInstance().Server.requset (new MicInputRequest (uuidList, outData));
				} else {
					
				}
			}
		}

		public void micInputNotice (ClientResponse response)
		{
			int uuid = int.Parse (response.message);
			var avatarIndex = _host.Data.toAvatarIndex (uuid);
			_host.UIHelper.getCardGOs (avatarIndex).PlayerItem.showChatAction ();
			MicrophoneManager.getInstance ().PlaySound (response.bytes);
		}

		private List<int> getUserList ()
		{
			List<int> userList = new List<int> ();
			for (int i = 0; i < _host.Data.AvatarList.Count; i++) {
				if (i != _host.Data.myIndex) {
					userList.Add (_host.Data.AvatarList [i].account.uuid);
				}
			}
			return userList;
		}
		void OnDestroy() {
			GameManager.getInstance ().Server.onResponse -= onResponse;
		}
	}
}