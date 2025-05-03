using System;
using Framework.Logic.UI;
using UnityEngine.UI;

namespace HotFix
{
	public class UIItemSlider : UIItemBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetSliderValue(uint currentCount, uint maxCount)
		{
			if (this.m_sliderImage != null)
			{
				this.m_sliderImage.fillAmount = currentCount * 1f / maxCount;
			}
			if (this.m_sliderTxt != null)
			{
				this.m_sliderTxt.text = string.Format("{0}/{1}", currentCount, maxCount);
			}
		}

		public Image m_sliderImage;

		public CustomText m_sliderTxt;
	}
}
