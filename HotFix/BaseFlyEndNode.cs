using System;
using Framework;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public abstract class BaseFlyEndNode : CustomBehaviour
	{
		public void SetCount(long from, long to, long count)
		{
			this.m_from = from;
			this.m_to = to;
			this.m_count = count;
		}

		public void SetPos(Vector3 pos)
		{
			this.m_pos = pos;
			base.transform.position = pos;
		}

		public void SetTransform(Transform trans)
		{
			if (trans == null)
			{
				return;
			}
			this.m_transform = trans;
			this.SetPos(trans.position);
		}

		public abstract void OnItemFinished(int current, int maxCount);

		public abstract void OnFinished();

		protected override void OnInit()
		{
			this.m_showCount++;
			if (!this.m_isShow)
			{
				base.gameObject.SetActive(true);
				this.m_isShow = true;
				EventArgsCustomBehaviour eventArgsCustomBehaviour = new EventArgsCustomBehaviour();
				eventArgsCustomBehaviour.SetData(this);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_AddUpdate, eventArgsCustomBehaviour);
			}
			if (this.m_transform != null)
			{
				this.SetPos(this.m_transform.position);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (!this.m_isShow)
			{
				return;
			}
			if (this.m_transform == null)
			{
				return;
			}
			base.transform.position = this.m_transform.position;
		}

		protected override void OnDeInit()
		{
			this.m_showCount--;
			if (this.m_showCount <= 0)
			{
				base.gameObject.SetActive(false);
				this.m_isShow = false;
				EventArgsCustomBehaviour eventArgsCustomBehaviour = new EventArgsCustomBehaviour();
				eventArgsCustomBehaviour.SetData(this);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_RemoveUpdate, eventArgsCustomBehaviour);
			}
		}

		[Label]
		public Vector3 m_pos;

		[Label]
		public Transform m_transform;

		[Label]
		public long m_from;

		[Label]
		public long m_to;

		[Label]
		public long m_count;

		[Label]
		public int m_showCount;

		[Label]
		public bool m_isShow;
	}
}
