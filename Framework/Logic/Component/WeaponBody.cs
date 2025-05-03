using System;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class WeaponBody : MonoBehaviour
	{
		public Transform GetTransform(WeaponBodyPosType posType)
		{
			Transform transform = this.m_center;
			switch (posType)
			{
			case WeaponBodyPosType.Center:
				transform = this.m_center;
				break;
			case WeaponBodyPosType.LocalCenter:
				transform = this.m_localCenter;
				break;
			case WeaponBodyPosType.Head:
				transform = this.m_head;
				break;
			case WeaponBodyPosType.Tail:
				transform = this.m_tail;
				break;
			}
			return transform;
		}

		public void PlayAnimation(string triggerName)
		{
			if (this.m_animator == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(triggerName))
			{
				return;
			}
			this.m_animator.SetTrigger(triggerName);
		}

		public void SetAnimatorSpeed(float speed)
		{
			if (this.m_animator == null)
			{
				return;
			}
			this.m_animator.speed = speed;
		}

		public void StopAnimation()
		{
			if (this.m_animator == null)
			{
				return;
			}
			this.m_animator.StopPlayback();
		}

		public void ResetAnimation(string triggerName)
		{
			if (this.m_animator == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(triggerName))
			{
				return;
			}
			this.m_animator.ResetTrigger(triggerName);
		}

		public Transform m_center;

		public Transform m_localCenter;

		public Transform m_head;

		public Transform m_tail;

		public Animator m_animator;
	}
}
