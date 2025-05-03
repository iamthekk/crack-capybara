using System;
using UnityEngine;

namespace HotFix
{
	public class PetDragController : MonoBehaviour
	{
		public PetDragController.DragState GetDragState()
		{
			return this._dragState;
		}

		public void StartDragRanchPet(PetRanchEntity ranchEntity)
		{
			this._dragRanchPet = ranchEntity;
			this._dragState = PetDragController.DragState.DragRanchPet;
			this._waitFrame = 2;
		}

		public void StartDragSlotPet(PetSlotEntity entity)
		{
			this._dragSlotPet = entity;
			this._dragState = PetDragController.DragState.DragSlotPet;
			this._waitFrame = 2;
		}

		private void Update()
		{
			if (this._dragState == PetDragController.DragState.DragRanchPet || this._dragState == PetDragController.DragState.DragSlotPet)
			{
				Vector2 vector = Vector2.zero;
				bool flag = false;
				if (Input.touchCount > 0)
				{
					flag = true;
					vector = Input.GetTouch(0).position;
				}
				if (flag)
				{
					this.dragPos = vector;
					return;
				}
				if (this._waitFrame > 0)
				{
					this._waitFrame--;
					return;
				}
				if (this._dragState == PetDragController.DragState.DragRanchPet)
				{
					this._dragRanchPet.DragFinish();
					this._dragRanchPet = null;
					this._dragState = PetDragController.DragState.None;
					return;
				}
				if (this._dragState == PetDragController.DragState.DragSlotPet)
				{
					this._dragSlotPet.DragFinish();
					this._dragSlotPet = null;
					this._dragState = PetDragController.DragState.None;
				}
			}
		}

		[HideInInspector]
		public Vector2 dragPos;

		private PetDragController.DragState _dragState;

		private PetRanchEntity _dragRanchPet;

		private PetSlotEntity _dragSlotPet;

		private int _waitFrame = 2;

		public enum DragState
		{
			None,
			DragRanchPet,
			DragSlotPet
		}
	}
}
