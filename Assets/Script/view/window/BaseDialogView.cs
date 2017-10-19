using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

namespace AssemblyCSharp
{
	public class BaseDialogView : MonoBehaviour
	{
		public OnClick onOK;
		public OnClick onCancle;

		public Text title;
		public Text msg;


		private void setTitle (string titleStr)
		{
			title.text = titleStr;
		}

		private void setMsg (string msgStr)
		{
			msg.text = msgStr;
		}


		public void clickOk ()
		{
			if (onOK != null)
				onOK ();
			Destroy (gameObject);
		}

		public void clickCancle ()
		{
			if (onCancle != null)
				onCancle ();
			Destroy (gameObject);
		}

		public void setContent (string titlestr, string msgstr, bool flag, OnClick ok, OnClick cancel)
		{
			setTitle (titlestr);
			setMsg (msgstr);
			if (ok != null) {
				onOK += ok;
			}
			if (cancel != null) {
				onCancle += cancel;
			}
		}

	}
}