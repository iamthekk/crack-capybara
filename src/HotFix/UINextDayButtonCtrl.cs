using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine.UI;

namespace HotFix
{
	public class UINextDayButtonCtrl : UIOneButtonCtrl
	{
		private GameEventViewModule gameEventViewModule
		{
			get
			{
				return GameApp.View.GetViewModule(ViewName.GameEventViewModule);
			}
		}

		public void SetFood(int food, int beginFood)
		{
			float num = (float)food / (float)beginFood;
			num = ((num > 1f) ? 1f : num);
			this.sliderFood.value = num;
			this.textCostFood.text = string.Format("{0}/{1}", food, beginFood);
		}

		public void SetHp(long hp, long maxHp)
		{
			float num = (float)hp / (float)maxHp;
			this.sliderHp.value = ((num > 1f) ? 1f : num);
			this.textCostHp.text = string.Format("{0}/{1}", hp, maxHp);
		}

		protected override void OnButtonDown()
		{
			if (this.gameEventViewModule.IsClickEnabled())
			{
				base.OnButtonDown();
			}
		}

		protected override void OnButtonUp()
		{
			if (this.gameEventViewModule.IsClickEnabled())
			{
				base.OnButtonUp();
			}
		}

		public Slider sliderFood;

		public CustomText textCostFood;

		public Slider sliderHp;

		public CustomText textCostHp;
	}
}
