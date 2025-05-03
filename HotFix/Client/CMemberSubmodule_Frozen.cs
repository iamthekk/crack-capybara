using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Logic.Component;

namespace HotFix.Client
{
	public class CMemberSubmodule_Frozen : CMemberBaseSubmodule
	{
		public CMemberSubmodule_Frozen(CMemberBase member, int commonTableID, MemberBodyPosType bodyPosType)
			: base(member, commonTableID, bodyPosType)
		{
		}

		public bool IsControlled
		{
			get
			{
				return this.m_effectGuids != null && this.m_effectGuids.Count > 0;
			}
		}

		protected override void OnInit()
		{
			this.m_effectGuids.Clear();
			this.m_gameObject.SetActive(false);
		}

		protected override Task OnDeInit()
		{
			this.m_effectGuids.Clear();
			this.ForceStop();
			return Task.CompletedTask;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime, float unscaleDeltaTimePause)
		{
		}

		public override Task OnReset()
		{
			this.m_effectGuids.Clear();
			return Task.CompletedTask;
		}

		public void Play(int buffId, string guid)
		{
			this.m_owner.ShowTextHUD(EHoverTextType.Frozen);
			this.m_effectGuids.Add(guid);
			CMemberBase owner = this.m_owner;
			if (owner != null)
			{
				owner.ControlledStateChange();
			}
			this.SetEffectShow();
		}

		public void Stop(int buffId, string guid)
		{
			if (!this.m_effectGuids.Contains(guid))
			{
				return;
			}
			this.m_effectGuids.Remove(guid);
			CMemberBase owner = this.m_owner;
			if (owner != null)
			{
				owner.ControlledStateChange();
			}
			this.SetEffectShow();
		}

		public void ForceStop()
		{
			this.m_effectGuids.Clear();
			CMemberBase owner = this.m_owner;
			if (owner != null)
			{
				owner.ControlledStateChange();
			}
			this.SetEffectShow();
		}

		private void SetEffectShow()
		{
			bool flag = this.m_effectGuids.Count > 0;
			if (this.m_gameObject != null && this.m_gameObject.activeSelf != flag)
			{
				this.m_gameObject.SetActive(flag);
			}
		}

		public List<string> m_effectGuids = new List<string>();
	}
}
