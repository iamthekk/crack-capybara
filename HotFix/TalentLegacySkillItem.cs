using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacySkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyDataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Button_Up.m_onClick = new Action(this.OnClickUp);
			this.Button_Down.m_onClick = new Action(this.OnClickDown);
		}

		protected override void OnDeInit()
		{
			this.Button_Up.m_onClick = null;
			this.Button_Down.m_onClick = null;
		}

		private void OnClickDown()
		{
			NetworkUtils.TalentLegacy.DoTalentLegacySkillSwitchRequest(this.m_originCareerId, 0, this.m_index, null);
		}

		private void OnClickUp()
		{
			NetworkUtils.TalentLegacy.DoTalentLegacySkillSwitchRequest(this.m_originCareerId, this.m_talentLegacyNodeId, this.m_index, null);
		}

		public void SetData(int originCareerId, int talentLegacyNodeId, int index)
		{
			this.m_originCareerId = originCareerId;
			this.m_talentLegacyNodeId = talentLegacyNodeId;
			this.m_index = index;
			if (this.m_talentLegacyDataModule == null)
			{
				return;
			}
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(talentLegacyNodeId);
			if (talentLegacy_talentLegacyNode == null)
			{
				return;
			}
			this.Text_Down.text = Singleton<LanguageManager>.Instance.GetInfoByID("1466");
			this.Text_Up.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_skill_up");
			this.Image_SkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
			this.Text_Name.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name);
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyDataModule.OnGetTalentLegacySkillInfo(talentLegacy_talentLegacyNode.career, talentLegacyNodeId);
			int num = 0;
			if (talentLegacySkillInfo != null)
			{
				num = talentLegacySkillInfo.Level;
			}
			this.Text_Level.gameObject.SetActiveSafe(num != 0);
			this.Text_Level.text = HLog.StringBuilderFormat("Lv.{0}", new object[] { num });
			TalentLegacy_talentLegacyEffect talentLegacySkillCfgByLevel = this.m_talentLegacyDataModule.GetTalentLegacySkillCfgByLevel(talentLegacyNodeId, num);
			if (talentLegacySkillCfgByLevel != null)
			{
				this.Text_Des.SetText(Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacySkillCfgByLevel.desc), true);
			}
			TalentLegacyDataModule.ELegacySkillEquipState skillEquipState = this.m_talentLegacyDataModule.GetSkillEquipState(originCareerId, talentLegacyNodeId, talentLegacy_talentLegacyNode.career, index);
			if (skillEquipState == TalentLegacyDataModule.ELegacySkillEquipState.Equip)
			{
				this.Button_Up.gameObject.SetActiveSafe(false);
				this.Obj_UnActive.SetActiveSafe(false);
				this.Button_Down.gameObject.SetActiveSafe(true);
				this.Obj_Finish.SetActiveSafe(false);
				return;
			}
			if (skillEquipState == TalentLegacyDataModule.ELegacySkillEquipState.UnActive)
			{
				this.Button_Up.gameObject.SetActiveSafe(false);
				this.Obj_UnActive.SetActiveSafe(true);
				this.Button_Down.gameObject.SetActiveSafe(false);
				this.Obj_Finish.SetActiveSafe(false);
				return;
			}
			if (skillEquipState == TalentLegacyDataModule.ELegacySkillEquipState.UnDown)
			{
				this.Button_Up.gameObject.SetActiveSafe(false);
				this.Obj_UnActive.SetActiveSafe(false);
				this.Button_Down.gameObject.SetActiveSafe(false);
				this.Obj_Finish.SetActiveSafe(true);
				return;
			}
			this.Button_Up.gameObject.SetActiveSafe(true);
			this.Obj_UnActive.SetActiveSafe(false);
			this.Button_Down.gameObject.SetActiveSafe(false);
			this.Obj_Finish.SetActiveSafe(false);
		}

		public CustomImage Image_SkillIcon;

		public CustomText Text_Level;

		public CustomText Text_Name;

		public CustomTextScrollView Text_Des;

		public CustomText Text_Up;

		public CustomButton Button_Up;

		public CustomText Text_Down;

		public CustomButton Button_Down;

		public GameObject Obj_UnActive;

		public GameObject Obj_Finish;

		private int m_originCareerId;

		private int m_talentLegacyNodeId;

		private int m_index;

		private TalentLegacyDataModule m_talentLegacyDataModule;
	}
}
