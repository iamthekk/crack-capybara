using System;
using Framework.Logic.Component;

namespace HotFix
{
	public class UnlockInfoSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.skillIIcontem.Init();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void Refresh(int skillBuildId)
		{
			this.skillIIcontem.Refresh(skillBuildId);
		}

		public UIGameEventSkillIconItem skillIIcontem;
	}
}
