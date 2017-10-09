using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

public class QuickMsgView : MonoBehaviour {
	GameView _host;
	public void init(GameView host)
	{
		_host = host;
		GameManager.getInstance ().Server.onResponse += onResponse;
	}
	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.MessageBox_Notice:
			string[] arr = response.message.Split (new char[1]{ '|' });
			int uuid = int.Parse (arr [1]);
			int msgIndex = int.Parse (arr [0]);
			var avatarIndex = _host.Data.toAvatarIndex (uuid);
			_host.UIHelper.getCardGOs (avatarIndex).PlayerItem.showChatMessage (msgIndex);
			SoundManager.getInstance ().playMessageBoxSound (msgIndex);
			break;
		}
	}
	public void btnClick(int index){
		GameManager.getInstance().Server.requset (new QuickMsgRequest(index,GlobalData.getInstance().myAvatarVO.account.uuid));

		ClientResponse res = new ClientResponse ();
		res.message = index + "|" + GlobalData.getInstance ().myAvatarVO.account.uuid;
		onResponse (res);
		
		hidePanel ();
	}

	public void showPanel(){
		gameObject.transform.DOLocalMove (new Vector3(472,113), 0.4f);
	}

	public void hidePanel(){
		gameObject.transform.DOLocalMove (new Vector3(472,567), 0.4f);
	}


	public void Destroy(){
		GameManager.getInstance ().Server.onResponse -= onResponse;
	}
}
