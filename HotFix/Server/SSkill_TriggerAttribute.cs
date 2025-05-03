using System;

namespace Server
{
	public class SSkill_TriggerAttribute : SSkillBase
	{
		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			base.OnInit(skillFactory, skillData);
			SMemberData memberData = base.Owner.memberData;
			memberData.m_onChangeHP = (Action<FP>)Delegate.Combine(memberData.m_onChangeHP, new Action<FP>(this.OnChangeHP));
			SMemberBase owner = base.Owner;
			owner.OnBattleEnd = (Action)Delegate.Combine(owner.OnBattleEnd, new Action(this.OnGameOver));
			this.TriggerAttr();
		}

		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.LitJson_ToObjectFp<SSkill_TriggerAttribute.Data>(parameters) : new SSkill_TriggerAttribute.Data());
		}

		protected override void OnAddBaseAttributes(bool isReverse = false)
		{
		}

		private void OnChangeHP(FP hp)
		{
			this.TriggerAttr();
		}

		private void TriggerAttr()
		{
			if (base.Owner.memberData.HPPercent < this.m_data.hpPercent)
			{
				if (!this.isAddAttributes)
				{
					this.isAddAttributes = true;
					base.OnAddBaseAttributes(false);
					base.ReportChangeAttribute();
					return;
				}
			}
			else if (this.isAddAttributes)
			{
				this.isAddAttributes = false;
				base.OnAddBaseAttributes(true);
				base.ReportChangeAttribute();
			}
		}

		private void OnGameOver()
		{
			SMemberData memberData = base.Owner.memberData;
			memberData.m_onChangeHP = (Action<FP>)Delegate.Remove(memberData.m_onChangeHP, new Action<FP>(this.OnChangeHP));
			SMemberBase owner = base.Owner;
			owner.OnBattleEnd = (Action)Delegate.Remove(owner.OnBattleEnd, new Action(this.OnGameOver));
			if (this.isAddAttributes)
			{
				this.isAddAttributes = false;
				base.OnAddBaseAttributes(true);
			}
		}

		public SSkill_TriggerAttribute.Data m_data;

		private bool isAddAttributes;

		public class Data
		{
			public FP hpPercent;
		}
	}
}
