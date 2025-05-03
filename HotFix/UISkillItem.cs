using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class UISkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(GameSkill_skill skill, int quality)
		{
			if (skill == null)
			{
				return;
			}
			string text = GameApp.Table.GetAtlasPath(skill.iconAtlasID);
			this.imageIcon.SetImage(text, skill.icon);
			text = GameApp.Table.GetAtlasPath(105);
			this.imageQuality.SetImage(text, string.Format("item_frame_{0}", quality));
		}

		public CustomImage imageQuality;

		public CustomImage imageIcon;
	}
}
