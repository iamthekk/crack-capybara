using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacySkillSlotItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Button_Self.m_onClick = new Action(this.OnClickSelf);
			this.m_talentLegacyDataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
		}

		protected override void OnDeInit()
		{
			this.Button_Self.m_onClick = null;
		}

		private void OnClickSelf()
		{
			if (this.m_state == 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("legacy_skill_slot_unlock_title1"));
				return;
			}
			int num = -1;
			if (this.m_state == 2)
			{
				TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(this.m_nodeId);
				if (talentLegacy_talentLegacyNode != null)
				{
					num = talentLegacy_talentLegacyNode.career;
				}
			}
			TalentLegacySkillViewModule.OpenData openData = default(TalentLegacySkillViewModule.OpenData);
			openData.CareerId = this.m_careerId;
			openData.Index = this.m_index;
			openData.SelectCareerId = num;
			openData.State = this.m_state;
			GameApp.View.OpenView(ViewName.TalentLegacySkillViewModule, openData, 1, null, null);
		}

		public void SetData(int state, int index, int nodeId, int careerId)
		{
			this.m_state = state;
			this.m_index = index;
			this.m_nodeId = nodeId;
			this.m_careerId = careerId;
			this.Ctrl_Red.gameObject.SetActiveSafe(false);
			this.Image_Add.gameObject.SetActiveSafe(false);
			this.Text_Level.gameObject.SetActiveSafe(false);
			string text = "img_talentLegacySkillUnlockBg";
			if (index == 0)
			{
				GuideController.Instance.DelTarget("Button_EquipCareerSlotItem");
				GuideController.Instance.AddTarget("Button_EquipCareerSlotItem", this.Button_Self.transform);
				GuideController.Instance.OpenViewTrigger(ViewName.TalentLegacySkillSelectViewModule);
			}
			if (state == 0)
			{
				this.Obj_Empty.SetActiveSafe(false);
				this.Obj_Skill.SetActiveSafe(false);
				this.Obj_Unlock.SetActiveSafe(true);
				text = "img_talentLegacySkilllockBg";
				this.Text_Name.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_skill_slot_unlock_title1");
				this.Text_Desc.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_skill_unlock");
			}
			else if (state == 1)
			{
				bool flag = false;
				IList<TalentLegacy_career> talentLegacy_careerElements = GameApp.Table.GetManager().GetTalentLegacy_careerElements();
				for (int i = 0; i < talentLegacy_careerElements.Count; i++)
				{
					if (this.m_talentLegacyDataModule.OnGetTalentLegacySkillRed(careerId, talentLegacy_careerElements[i].id, index) == 1)
					{
						flag = true;
						break;
					}
				}
				this.Ctrl_Red.gameObject.SetActiveSafe(flag);
				this.Obj_Empty.SetActiveSafe(true);
				this.Obj_Skill.SetActiveSafe(false);
				this.Obj_Unlock.SetActiveSafe(false);
				this.Text_Name.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_skill_slot_unlock_title2");
				this.Text_Desc.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_skill_slot_unlock2");
			}
			else if (state == 2)
			{
				this.Obj_Empty.SetActiveSafe(false);
				this.Obj_Skill.SetActiveSafe(true);
				this.Obj_Unlock.SetActiveSafe(false);
				this.Text_Level.gameObject.SetActiveSafe(true);
				this.Image_Add.gameObject.SetActiveSafe(true);
				TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(nodeId);
				if (talentLegacy_talentLegacyNode == null)
				{
					return;
				}
				TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyDataModule.OnGetTalentLegacySkillInfo(talentLegacy_talentLegacyNode.career, nodeId);
				int num = 0;
				if (talentLegacySkillInfo != null)
				{
					num = talentLegacySkillInfo.Level;
				}
				this.Text_Level.text = HLog.StringBuilder("Lv.", num.ToString());
				TalentLegacy_talentLegacyEffect talentLegacySkillCfgByLevel = this.m_talentLegacyDataModule.GetTalentLegacySkillCfgByLevel(nodeId, num);
				if (talentLegacy_talentLegacyNode != null && talentLegacySkillCfgByLevel != null)
				{
					this.Image_Skill.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
					this.Text_Name.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name);
					this.Text_Desc.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacySkillCfgByLevel.desc);
				}
			}
			this.Text_Level.gameObject.SetActiveSafe(false);
			this.Image_Bg.SetImage(160, text);
		}

		private TalentLegacy_talentLegacyEffect OnFindFirstAddSkillSlot()
		{
			IList<TalentLegacy_talentLegacyEffect> talentLegacy_talentLegacyEffectElements = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyEffectElements();
			for (int i = 0; i < talentLegacy_talentLegacyEffectElements.Count; i++)
			{
				if (talentLegacy_talentLegacyEffectElements[i].addSkillSlot > 0)
				{
					return talentLegacy_talentLegacyEffectElements[i];
				}
			}
			return null;
		}

		public CustomImage Image_Bg;

		public CustomText Text_Name;

		public CustomText Text_Desc;

		public CustomText Text_Level;

		public CustomImage Image_Add;

		public CustomButton Button_Self;

		public GameObject Obj_Empty;

		public GameObject Obj_Skill;

		public CustomImage Image_Skill;

		public GameObject Obj_Unlock;

		public RedNodeOneCtrl Ctrl_Red;

		private TalentLegacyDataModule m_talentLegacyDataModule;

		private int m_index;

		private int m_state;

		private int m_nodeId;

		private int m_careerId;
	}
}
