using System;
using Framework.Logic.Component;

namespace HotFix
{
	public class MapFloatingCloudGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				this.items[i].Init();
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				this.items[i].DeInit();
			}
		}

		public void SetTime(int random, float duration)
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				this.items[i].SetTime(random, duration);
			}
		}

		public MapFloatingCloudItem[] items;
	}
}
