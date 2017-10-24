using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class ReplayPlayerView : MonoBehaviour
{
	public Image headerIcon;
	public Image bankerImg;
	public Text nameText;
	public Image readyImg;
	public Text scoreText;

	public GameObject HuFlag;
	public GameObject pengEffect;
	public GameObject gangEffect;
	public GameObject huEffect;
	public GameObject chiEffect;

	private ReplayAvatarVO data;

	public void setAvatarVo (ReplayAvatarVO value)
	{
		if (value != null) {
			data = value;
			nameText.text = data.accountName;
			scoreText.text = data.socre + "";
			StartCoroutine (LoadImg ());

		} else {
			nameText.text = "";
			readyImg.enabled = false;
			bankerImg.enabled = false;
			headerIcon.sprite = Resources.Load ("Image/morentouxiang", typeof(Sprite)) as Sprite;
		}
	}

	private IEnumerator LoadImg ()
	{ 
		WWW www = new WWW (data.headIcon);
		yield return www;

		if (string.IsNullOrEmpty (www.error)) {
			Texture2D texture2D = www.texture;
			Sprite sprite = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));
			headerIcon.sprite = sprite;
		}
	}

	public void showHuEffect ()
	{
		huEffect.SetActive (true);
		Invoke ("hideEffects", 1f);

		HuFlag.SetActive (true);
	}

	public void hideHuImage ()
	{
		HuFlag.SetActive (false);
	}

	public void showPengEffect ()
	{
		pengEffect.SetActive (true);
		Invoke ("hideEffects", 1f);
	}
	public void showChiEffect ()
	{
		chiEffect.SetActive (true);
		Invoke ("hideEffects", 1f);
	}

	public void showGangEffect ()
	{
		gangEffect.SetActive (true);
		Invoke ("hideEffects", 1f);
	}

	private void hideEffects ()
	{
		pengEffect.SetActive (false);
		gangEffect.SetActive (false);
		huEffect.SetActive (false);
		chiEffect.SetActive (false);
	}

	public int getSex ()
	{
		return data.sex;
	}
}
