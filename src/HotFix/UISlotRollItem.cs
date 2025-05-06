using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UISlotRollItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.skillItem.Init();
		}

		protected override void OnDeInit()
		{
			this.skillItem.DeInit();
		}

		public void SetData(PaySlotRewardData data)
		{
			if (data == null)
			{
				return;
			}
			if (data.IsSkill)
			{
				this.skillItem.gameObject.SetActive(true);
				this.imageIcon.gameObject.SetActive(false);
				this.skillItem.Refresh(data.SkillBuildData, false, false);
				return;
			}
			this.skillItem.gameObject.SetActive(false);
			this.imageIcon.gameObject.SetActive(true);
			string atlasPath = GameApp.Table.GetAtlasPath(data.Config.atlas);
			this.imageIcon.SetImage(atlasPath, data.Config.icon);
		}

		public void AutoScale(Vector3 centerPos)
		{
			float num = Vector3.Distance(this.RTFScaleRoot.position, centerPos);
			float num2 = 0.7f;
			float num3 = num2 - Mathf.Abs(num);
			num3 = Mathf.Clamp(num3, 0.6f, num2);
			if (num3 >= 0.68f)
			{
				num3 = num2;
			}
			this.RTFScaleRoot.localScale = new Vector3(num3, num3, 1f);
		}

		public RectTransform RTFScaleRoot;

		public CustomImage imageIcon;

		public UIGameEventSkillItem skillItem;
	}
}
