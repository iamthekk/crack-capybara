using System;
using Framework.Logic.Component;

namespace HotFix
{
	public class UnlockInfoLayoutItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void Refresh(int lineIndex)
		{
			this.index = lineIndex;
		}

		public int GetIndex()
		{
			return this.index;
		}

		private int index;
	}
}
