using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
	public class MainShopEquipPackGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].Init();
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 4;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].SetData();
			}
		}

		public override int PlayAnimation(float startTime, int index)
		{
			this.titleFg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index, 10024);
			this.contentFg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + 1, 10024);
			return index + 2;
		}

		public RectTransform contentFg;

		public List<MainShopEquipChestBase> items = new List<MainShopEquipChestBase>();
	}
}
