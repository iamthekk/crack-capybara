using System;
using UnityEngine;

namespace HotFix
{
	public class PetDeployGroup : MonoBehaviour
	{
		public void ResetUIParent()
		{
			Transform[] array = this.deploySlot;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetParent(this.petSlotParent);
			}
			this.board.SetParent(base.transform);
		}

		public Transform petSlotParent;

		public Transform[] deploySlot;

		public Transform board;
	}
}
