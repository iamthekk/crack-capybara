using System;

namespace Server
{
	public class SMemberRound : SMemberBase
	{
		public override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SMemberRound.Data>(parameters) : new SMemberRound.Data());
		}

		public override void OnInit()
		{
			this.OnBattleStart = (Action)Delegate.Combine(this.OnBattleStart, new Action(this.InitMemberRound_WaitRoundCount));
			this.OnRoleRoundEnd = (Action)Delegate.Combine(this.OnRoleRoundEnd, new Action(this.MemberRound_RoleRoundEnd));
		}

		public override void OnDeInit()
		{
			this.OnBattleStart = (Action)Delegate.Remove(this.OnBattleStart, new Action(this.InitMemberRound_WaitRoundCount));
			this.OnRoleRoundEnd = (Action)Delegate.Remove(this.OnRoleRoundEnd, new Action(this.MemberRound_RoleRoundEnd));
		}

		protected override void RoleRoundFighting()
		{
			this.curIgnoreRount++;
			if (this.curIgnoreRount > this.m_data.WaitRoundCount)
			{
				this.curIgnoreRount = 0;
				this.ShowWaitRoundCount(this.curIgnoreRount);
				base.RoleRoundFighting();
			}
		}

		private void InitMemberRound_WaitRoundCount()
		{
			base.ReportWaitRoundCount(this.m_data.WaitRoundCount);
		}

		private void MemberRound_RoleRoundEnd()
		{
			int num = this.m_data.WaitRoundCount - this.curIgnoreRount;
			this.ShowWaitRoundCount(num);
		}

		private void ShowWaitRoundCount(int count)
		{
			base.ReportWaitRoundCount(count);
		}

		public SMemberRound.Data m_data;

		private int curIgnoreRount;

		public class Data
		{
			public int WaitRoundCount;
		}
	}
}
