using System;
using UnityEngine;

namespace HotFix.Client
{
	public abstract class CSkillMoveTaskBase : BaseTask
	{
		protected override void OnAwake()
		{
		}

		protected override void OnStart()
		{
			this.m_currentTime = 0f;
			this.m_targetPosition = this.GetTargetPosition();
		}

		protected abstract Vector3 GetTargetPosition();

		protected override void OnEnd()
		{
		}

		public override TaskStatus OnUpdate(float deltaTime)
		{
			if (this.m_owner == null || this.m_owner.m_gameObject == null)
			{
				return TaskStatus.Success;
			}
			if (Vector3.Distance(this.m_owner.m_gameObject.transform.position, this.m_targetPosition) < 0.01f)
			{
				this.m_owner.m_gameObject.transform.position = this.m_targetPosition;
				return TaskStatus.Success;
			}
			this.m_currentTime += deltaTime;
			if (this.m_currentTime >= this.m_time)
			{
				this.m_owner.m_gameObject.transform.position = this.m_targetPosition;
				return TaskStatus.Success;
			}
			this.m_owner.m_gameObject.transform.position = Vector3.Lerp(this.m_owner.m_gameObject.transform.position, this.m_targetPosition, this.m_currentTime / this.m_time);
			return TaskStatus.Running;
		}

		public CMemberBase m_owner;

		public float m_time;

		private float m_currentTime;

		private Vector3 m_targetPosition;
	}
}
