using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacySkillTabItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int careerId, int state, int targetCareerId, int index)
		{
			this.m_index = index;
			this.CareerId = careerId;
			this.m_targetCareerId = targetCareerId;
			this.m_state = state;
			TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(careerId);
			if (talentLegacy_career != null)
			{
				this.Image_Icon.SetImage(talentLegacy_career.previewIconId, talentLegacy_career.previewIcon);
				bool flag = this.m_talentLegacyModule.IsUnlockTalentLegacyCareer(careerId);
				this.Obj_Lock.SetActiveSafe(!flag);
			}
			else
			{
				this.Obj_Lock.SetActiveSafe(false);
			}
			this.OnRefreshRed();
		}

		private void OnRefreshRed()
		{
		}

		public CustomImage Image_Icon;

		public GameObject Obj_Lock;

		public RedNodeOneCtrl Ctrl_Red;

		private TalentLegacyDataModule m_talentLegacyModule;

		public int CareerId;

		private int m_targetCareerId;

		private int m_state;

		private int m_index;
	}
}
