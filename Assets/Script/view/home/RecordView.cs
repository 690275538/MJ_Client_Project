using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class RecordView : MonoBehaviour
{
	public GameObject recordPage;
	public GameObject detailPage;

	public GameObject recordContainer;
	public GameObject detailContainer;

	public GameObject replayDialogUI;
	public InputField inputField;


	public List<Text> names;

	private List<GameObject> detailList;
	private List<GameObject> recordList;


	void Start ()
	{
		detailList = new List<GameObject> ();
		recordList = new List<GameObject> ();

		GameManager.getInstance ().Server.requset (APIS.RECORD_REQUEST, "0");
		GameManager.getInstance ().Server.onResponse += onResponse;

		inputField.characterValidation = InputField.CharacterValidation.Integer;
		inputField.keyboardType = TouchScreenKeyboardType.NumberPad;
		inputField.characterLimit = 6;
	}

	void onResponse (ClientResponse response)
	{
		switch (response.headCode) {
		case APIS.RECORD_RESPONSE://房间战绩
			onRecordResponse (response);
			break;
		case APIS.DETAIL_RECORD_REPONSE://房间详细战绩
			onDetailRecordResponse (response);
			break;
		}
	}

	public void onCloseBtn ()
	{
		Destroy (this.gameObject);

	}

	private  void onRecordResponse (ClientResponse response)
	{
		
		var vos = JsonMapper.ToObject<List<RecordVO>> (response.message);

		for (int i = 0; i < recordList.Count; i++) {
			Destroy (recordList [i]);
		}
		recordList.Clear ();


		if (vos.Count != 0) {
			recordPage.SetActive (true);
			detailPage.SetActive (false);

			for (int i = 0; i < vos.Count; i++) {
				RecordVO rvo = vos [i];
				GameObject cell = Instantiate (Resources.Load ("Prefab/home/RecordCR")) as GameObject;
				cell.transform.SetParent( recordContainer.transform);
				cell.transform.localScale = Vector3.one;
				cell.GetComponent<RecordCRView> ().setUI (rvo, i + 1);
				recordList.Add (cell);
		
			}
		}
	}


	private void onDetailRecordResponse (ClientResponse response)
	{
		recordPage.SetActive (false);
		detailPage.SetActive (true);
		var vos = JsonMapper.ToObject<List<DetailRecordVO>> (response.message);

		for (int i = 0; i < detailList.Count; i++) {
			Destroy (detailList [i]);
		}
		detailList.Clear ();

		if (vos.Count > 0) {
			string content = vos [0].content;
			if (!string.IsNullOrEmpty (content)) {
				string[] strs = content.Split (new char[1]{ ',' });
				for (int i = 0; i < strs.Length - 1; i++) {
					string name = strs [i].Split (new char[1]{ ':' }) [0];
					names [i].text = name;
				}
			}
		}
		for (int i = 0; i < vos.Count; i++) {
			DetailRecordVO dvo = vos [i];
			GameObject cell = Instantiate (Resources.Load ("Prefab/home/DetailRecordCR")) as GameObject;
			cell.transform.SetParent (detailContainer.transform);
			cell.transform.localScale = Vector3.one;
			cell.GetComponent<DetailRecordCRView> ().setUI (dvo, i + 1);
			detailList.Add (cell);
		}

	}



	public void onReplayBtn ()
	{
		replayDialogUI.SetActive (true);
	}

	public void onCancleBtn ()
	{
		replayDialogUI.SetActive (false);
	}

	public void onOKBtn ()
	{
		string inputStr = inputField.text;
		if (string.IsNullOrEmpty (inputStr) || inputStr.Length != 6) {
			MyDebug.Log ("输入的数据长度不正确");
			return;
		}

		GameManager.getInstance ().Server.requset (APIS.DETAIL_RECORD_REQUEST, inputStr);
		onCancleBtn ();
	}

	void OnDestroy ()
	{
		GameManager.getInstance ().Server.onResponse -= onResponse;
	}
}
