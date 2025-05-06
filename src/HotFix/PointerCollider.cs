using System;
using UnityEngine;

namespace HotFix
{
	public class PointerCollider : MonoBehaviour
	{
		public void SetColliderEnable(bool enable)
		{
			this._colliderEnable = enable;
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (!this._colliderEnable)
			{
				return;
			}
			Action onPointerColliderEnter = this.OnPointerColliderEnter;
			if (onPointerColliderEnter == null)
			{
				return;
			}
			onPointerColliderEnter();
		}

		public void Clear()
		{
			this.OnPointerColliderEnter = null;
		}

		public Collider2D pointCollider;

		private bool _colliderEnable;

		public Action OnPointerColliderEnter;
	}
}
