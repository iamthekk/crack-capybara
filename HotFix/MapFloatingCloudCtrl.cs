using System;
using UnityEngine;

namespace HotFix
{
	public class MapFloatingCloudCtrl : MapFloatingCtrl
	{
		protected override void OnInit()
		{
			this.cloudObj.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				MapFloatingCloudGroup component = this.items[i].GetComponent<MapFloatingCloudGroup>();
				if (component)
				{
					component.DeInit();
				}
			}
			base.ClearAll();
		}

		protected override void CreateItems()
		{
			base.CreateItems();
			for (int i = 0; i < this.items.Count; i++)
			{
				MapFloatingCloudGroup component = this.items[i].GetComponent<MapFloatingCloudGroup>();
				if (component)
				{
					component.Init();
				}
			}
		}

		public void SetTime(int random, float duration)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].GetComponent<MapFloatingCloudGroup>().SetTime(random, duration);
			}
		}

		public GameObject cloudObj;
	}
}
