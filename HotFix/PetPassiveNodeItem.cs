using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class PetPassiveNodeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.petPassiveItems.Count; i++)
			{
				this.petPassiveItems[i].Init();
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.petPassiveItems.Count; i++)
			{
				this.petPassiveItems[i].DeInit();
			}
		}

		public void SetData(List<int> passiveIds, List<int> passiveValues)
		{
			for (int i = 0; i < this.petPassiveItems.Count; i++)
			{
				PetPassiveItem petPassiveItem = this.petPassiveItems[i];
				int num = ((i < passiveIds.Count) ? passiveIds[i] : (-1));
				int num2 = ((i < passiveValues.Count) ? passiveValues[i] : (-1));
				petPassiveItem.SetData(num, num2);
			}
		}

		public int GetActiveCount()
		{
			int num = 0;
			for (int i = 0; i < this.petPassiveItems.Count; i++)
			{
				if (this.petPassiveItems[i].gameObject.activeSelf)
				{
					num++;
				}
			}
			return num;
		}

		public CustomText textTitle;

		public CustomButton btnInfo;

		public CustomText textTraining;

		public List<PetPassiveItem> petPassiveItems = new List<PetPassiveItem>();
	}
}
