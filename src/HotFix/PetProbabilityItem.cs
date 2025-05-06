using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class PetProbabilityItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(PetProbabilityTipViewModule.ItemNodeData data)
		{
			Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(data.qualityId);
			if (elementById != null)
			{
				this.textQuality.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			}
			else
			{
				this.textQuality.text = "";
			}
			float num = (float)data.probability * 0.01f;
			this.textProbability.text = num.ToString("0.00") + "%";
			this.imgQualityBg.SetImage(105, elementById.typeTxtBg);
		}

		public CustomImage imgQualityBg;

		public CustomText textQuality;

		public CustomText textProbability;
	}
}
