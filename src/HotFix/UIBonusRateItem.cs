using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class UIBonusRateItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(Mining_showRate showRate, int mode)
		{
			if (showRate == null)
			{
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(showRate.atlas);
			this.imageQuality.SetImage(atlasPath, showRate.icon);
			this.textName.text = this.GetColorInfo(Singleton<LanguageManager>.Instance.GetInfoByID(showRate.languageId), showRate.quality);
			int num = showRate.showRate;
			if (mode == 1)
			{
				num = showRate.getRate;
			}
			float num2 = (float)num / 10000f * 100f;
			this.textRate.text = num2.ToString("0.00") + "%";
		}

		private string GetColorInfo(string info, int quality)
		{
			switch (quality)
			{
			case 1:
				return "<color=#d9d7ef>" + info + "</color>";
			case 2:
				return "<color=#d3f24e>" + info + "</color>";
			case 3:
				return "<color=#91eef6>" + info + "</color>";
			case 4:
				return "<color=#ba7fff>" + info + "</color>";
			case 5:
				return "<color=#ff9852>" + info + "</color>";
			case 6:
				return "<color=#ff604d>" + info + "</color>";
			default:
				return info;
			}
		}

		public CustomImage imageQuality;

		public CustomText textName;

		public CustomText textRate;
	}
}
