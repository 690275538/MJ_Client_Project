using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class bottomScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    private int cardPoint;
    private Vector3 oldPosition;
	private bool dragFlag = false;
    //==================================================
    public Image image;
    //
    public delegate void EventHandler(GameObject obj);
    public event EventHandler onMyHandCardPutout;
	public event EventHandler onMyHandCardSelectedChange;
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
        if (GlobalData.isDrag)
        {
			dragFlag = true;
            GetComponent<RectTransform>().pivot.Set(0, 0);
            transform.position = Input.mousePosition;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
		if (GlobalData.isDrag) {
			if (selected == false) {
				selected = true;
				oldPosition = transform.localPosition;
			} else {
				sendObjectToCallBack ();
			}
		}

    }

    public void OnPointerUp(PointerEventData eventData)
    {
		if (GlobalData.isDrag) {
			if (transform.localPosition.y > -122f) {
				sendObjectToCallBack ();
			} else {
				if (dragFlag) {
					transform.localPosition = oldPosition;
				} else {
					reSetPoisitonCallBack ();
				}
			}
			dragFlag = false;
		}
    }

	private void sendObjectToCallBack(){
		if (onMyHandCardPutout != null)     //发送消息
		{
			onMyHandCardPutout(gameObject);//发送当前游戏物体消息
		}
	}

	private void reSetPoisitonCallBack(){
		if (onMyHandCardSelectedChange != null) {
			onMyHandCardSelectedChange (gameObject);
		}
	}

    public void setPoint(int _cardPoint)
    {
        cardPoint = _cardPoint;//设置所有牌指针
		image.sprite = Resources.Load("Cards/Big/b"+cardPoint,typeof(Sprite)) as Sprite;

    }

    public int getPoint()
    {
        return cardPoint;
    }

    private void destroy()
    {
       // Destroy(this.gameObject);
    }

}
