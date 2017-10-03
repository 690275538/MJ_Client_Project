using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CardGOs{
		public List<GameObject> Hand;
		public List<GameObject> Peng;
		public List<GameObject> Gang;
		public List<GameObject> Chi;
		public List<GameObject> PGC;
		public List<GameObject> Table;

		public Transform HandParent;
		public Transform PGCParent;
		public Transform TableParent;

		public CardGOs(){
			Hand = new List<GameObject> ();
			Peng = new List<GameObject> ();
			Gang = new List<GameObject> ();
			Chi = new List<GameObject> ();
			PGC = new List<GameObject> ();
			Table = new List<GameObject> ();
		}
	}
	public class GamingData
	{
		public int myIndex;
		private List<CardGOs> allCardGOs;
		public GamingData ()
		{
			allCardGOs = new List<CardGOs> (4);
			for (int i = 0; i < 4; i++) {
				allCardGOs.Add (new CardGOs ());
			}
		}
		public CardGOs getCardGOs(Direction dir){
			return allCardGOs [(int)dir];
		}
		public void init(GameView host){
			for (int i = 0; i < 4; i++) {
				CardGOs cgo = allCardGOs [i];
				cgo.HandParent = host.HandParents[i];
				cgo.TableParent = host.TableParents[i];
				cgo.PGCParent = host.PGCParents[i];
			}
		}
		public void setMyAvatarIndex(int index){
			myIndex = index;
//			dir = (Direction)index;
//			dirStr = Enum.GetName (typeof(Direction), index);
		}
		public Direction toGameDir(int avatarIndex){
			int i = (avatarIndex + 4 - myIndex) % 4;
			return (Direction)i;
		}

	}
}

