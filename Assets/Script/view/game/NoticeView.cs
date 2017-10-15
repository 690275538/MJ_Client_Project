using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class NoticeView : MonoBehaviour
	{

		public GameObject ui;
		public Text noticeText;


		private int showTimeNumber = 0;
		private int showNoticeNumber = 0;
		private bool timeFlag = false;

		public NoticeView ()
		{


		}
		void Start(){
			randShowTime ();
			timeFlag = true;
		}
		void Update(){
			if (timeFlag) {
				showTimeNumber--;
				if (showTimeNumber < 0) {
					timeFlag = false;
					showTimeNumber = 0;
					playNoticeAction ();
				}
			}
		}
		void randShowTime ()
		{
			showTimeNumber = (int)(UnityEngine.Random.Range (5000, 10000));
		}

		private void playNoticeAction ()
		{

			var noticeMsgs = GlobalData.getInstance ().NoticeMsgs;
			ui.SetActive (noticeMsgs.Count != 0);
			if (noticeMsgs.Count != 0) {
				noticeText.transform.localPosition = new Vector3 (500, noticeText.transform.localPosition.y);
				noticeText.text = noticeMsgs [showNoticeNumber];
				float time = noticeText.text.Length * 0.5f + 422f / 56f;

				Tweener tweener = noticeText.transform.DOLocalMove (new Vector3 (-noticeText.text.Length * 28, noticeText.transform.localPosition.y), time);
				tweener.OnComplete (moveCompleted);
				tweener.SetEase (Ease.Linear);
				//tweener.SetLoops(-1);
			}
		}

		void moveCompleted ()
		{
			showNoticeNumber++;
			if (showNoticeNumber == GlobalData.getInstance ().NoticeMsgs.Count) {
				showNoticeNumber = 0;
			}
			ui.SetActive (false);
			randShowTime ();
			timeFlag = true;
		}

	}
}

