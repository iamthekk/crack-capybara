using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class WheelSymbolReward : MonoBehaviour
	{
		public void SetData(WheelSymbolData data)
		{
			this.textIndex.text = "";
			int num = int.Parse(data.cfg.param[0]);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(data.cfg.textId);
			this.textStyle.text = string.Concat(new string[]
			{
				"<color=",
				data.cfg.textColor,
				">",
				infoByID,
				"</color>"
			});
			this.imgSkillFrame.enabled = num == 2;
			this.imgSkillIcon.enabled = num == 2;
			this.imgItemIcon.enabled = num == 1;
			if (num == 2)
			{
				this.imgSkillIcon.SetImage(data.cfg.atlas, data.cfg.icon);
				return;
			}
			if (num == 1)
			{
				this.imgItemIcon.SetImage(data.cfg.atlas, data.cfg.icon);
			}
		}

		public void PlaySelectEfx()
		{
			this.selectParticle.Play();
			this.enterParticle.Play();
		}

		public CustomText textStyle;

		public CustomText textIndex;

		public CustomImage imgSkillFrame;

		public CustomImage imgSkillIcon;

		public CustomImage imgItemIcon;

		public ParticleSystem enterParticle;

		public ParticleSystem selectParticle;
	}
}
