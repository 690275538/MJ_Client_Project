using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyHandCardView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public static bool isPutout;
	public static bool isChi;
    private int _cardPoint;
    private Vector3 oldPosition;
	private bool dragFlag = false;
    //==================================================
    public Image image;
    //
    public delegate void HandCardHandler(GameObject obj);
	public event HandCardHandler onMyHandCardPutout;
	public event HandCardHandler onMyHandCardSelectedChange;
	public event HandCardHandler onMyHandCardChiChange;
	public bool selected = false;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isPutout)
        {
			dragFlag = true;
            GetComponent<RectTransform>().pivot.Set(0, 0);
            transform.position = Input.mousePosition;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
		if (isPutout) {
			if (selected == false) {
				selected = true;
				oldPosition = transform.localPosition;
			} else {
				putoutHandCard ();
			}
		}

    }

    public void OnPointerUp(PointerEventData eventData)
    {
		if (isPutout) {
			if (transform.localPosition.y > -122f) {
				putoutHandCard ();
			} else {
				if (dragFlag) {
					transform.localPosition = oldPosition;
				} else {
					selectedChange ();
				}
			}
			dragFlag = false;
		}
		if (isChi) {
			chiChange ();
		}
    }
	private void chiChange(){
		if (onMyHandCardChiChange != null)
		{
			onMyHandCardChiChange(gameObject);
		}
	}
	private void putoutHandCard(){
		if (onMyHandCardPutout != null)
		{
			onMyHandCardPutout(gameObject);
		}
	}

	private void selectedChange(){
		if (onMyHandCardSelectedChange != null) {
			onMyHandCardSelectedChange (gameObject);
		}
	}

    public void setPoint(int cardPoint)
	{
        _cardPoint = cardPoint;
		image.sprite = Resources.Load("Cards/Big/b"+_cardPoint,typeof(Sprite)) as Sprite;

    }

    public int getPoint()
    {
        return _cardPoint;
    }

    private void destroy()
    {
       // Destroy(this.gameObject);
    }

}
