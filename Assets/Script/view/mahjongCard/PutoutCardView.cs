using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;

public class PutoutCardView : MonoBehaviour {
    private int cardPoint;
	private List<GameObject> ownerList;

	public Image cardImg;

	public void setOwnerList(List<GameObject> list){
		ownerList = list;
	}
	public List<GameObject> getOwnerList(){
		return ownerList;
	}

    public void setPoint(int _cardPoint)
    {
        cardPoint = _cardPoint;//设置所有牌指针
		cardImg.sprite = Resources.Load("Cards/Small/s"+cardPoint,typeof(Sprite)) as Sprite;

    }

	public int getPoint()
	{
		return cardPoint;
	}


	//=========================================
	public void setLefAndRightPoint(int _cardPoint){
		cardPoint = _cardPoint;//设置所有牌指针
		cardImg.sprite = Resources.Load("Cards/Left&Right/lr"+cardPoint,typeof(Sprite)) as Sprite;

	}

}
