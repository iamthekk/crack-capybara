using System;
using Framework.Logic.Component;
using UnityEngine.UI;

namespace HotFix
{
	public class UnlockInfoStarItem : CustomBehaviour
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

		public void Refresh(int starIndex)
		{
			this.index = starIndex;
		}

		public int GetIndex()
		{
			return this.index;
		}

		public Image image;

		private int index;
	}
}
