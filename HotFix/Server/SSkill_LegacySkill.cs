using System;

namespace Server
{
	public class SSkill_LegacySkill : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.LitJson_ToObject<SSkill_LegacySkill.Data>(parameters) : new SSkill_LegacySkill.Data());
			if (this.m_data != null)
			{
				this.targetShieldAddDamage = this.m_data.targetShieldAddDamage;
				this.attackerHpLessAddDamage = this.m_data.attackerHpLessAddDamage;
				this.targetHpLessAddDamage = this.m_data.targetHpLessAddDamage;
			}
		}

		public override void Play()
		{
			this.PlayLegacySkill();
		}

		private void PlayLegacySkill()
		{
			this.m_CastType = SkillCastType.Attack;
			Action<SSkillBase> onPlayStart = base.m_onPlayStart;
			if (onPlayStart != null)
			{
				onPlayStart(this);
			}
			base.ResetMaxCD();
			this.OnSummonDisplay();
			base.Move(false);
			this.OnPlayBefore();
			this.OnPlaying();
		}

		private void OnSummonDisplay()
		{
			ReportTool.AddLegacySkillSummonDisplay(this.m_owner.m_controller, this.m_skillData.m_id, this.m_owner.m_instanceId, base.skillData.m_legacyAppearFrame);
		}

		protected SSkill_LegacySkill.Data m_data;

		public class Data
		{
			public SSkillTypeData_TargetShieldAddDamage targetShieldAddDamage;

			public AttackerHpLessAddDamage attackerHpLessAddDamage;

			public TargetHpLessAddDamage targetHpLessAddDamage;
		}
	}
}
