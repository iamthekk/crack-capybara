using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyPreViewItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData()
		{
			int num = int.Parse(base.name.Split('_', StringSplitOptions.None)[1]);
			this.m_careerCfg = GameApp.Table.GetManager().GetTalentLegacy_career(num);
			if (this.m_careerCfg == null)
			{
				return;
			}
			if (this.m_careerCfg.isOpen == 0)
			{
				this.Obj_Empty.SetActiveSafe(true);
				this.Obj_Have.SetActiveSafe(false);
				return;
			}
			this.Obj_Empty.SetActiveSafe(false);
			this.Obj_Have.SetActiveSafe(true);
			this.Image_Icon.SetImage(this.m_careerCfg.previewIconId, this.m_careerCfg.previewIcon);
		}

		public GameObject Obj_Have;

		public CustomImage Image_Icon;

		public GameObject Obj_Empty;

		private TalentLegacy_career m_careerCfg;

		private TalentLegacyDataModule m_talentLegacyModule;
	}
}
