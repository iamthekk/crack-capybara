using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsAddMonsterGroup : BaseEventArgs
	{
		public bool isMember { get; private set; }

		public int targetId { get; private set; }

		public int atkUpgrade { get; private set; }

		public int hpUpgrade { get; private set; }

		public Action onComplete { get; private set; }

		public void SetGroupData(int groupId, int atkUp, int hpUp, Action onFinish)
		{
			this.targetId = groupId;
			this.atkUpgrade = atkUp;
			this.hpUpgrade = hpUp;
			this.onComplete = onFinish;
			this.isMember = false;
		}

		public void SetMemberData(int id, int atkUp, int hpUp, MemberEnterBattleMode mode, Action onFinish)
		{
			this.targetId = id;
			this.atkUpgrade = atkUp;
			this.hpUpgrade = hpUp;
			this.enterMode = mode;
			this.onComplete = onFinish;
			this.isMember = true;
		}

		public override void Clear()
		{
		}

		public MemberEnterBattleMode enterMode;
	}
}
