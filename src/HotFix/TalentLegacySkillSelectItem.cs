using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacySkillSelectItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Button_Self.m_onClick = new Action(this.OnClickSelf);
		}

		private void OnClickSelf()
		{
			int num = 0;
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyModule.OnGetTalentLegacySkillInfo(this.m_careerId, this.m_talentLegacyId);
			if (GameApp.Table.GetManager().GetTalentLegacy_career(this.m_careerId) == null)
			{
				return;
			}
			if (talentLegacySkillInfo != null)
			{
				num = talentLegacySkillInfo.Level;
			}
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(this.m_talentLegacyId);
			if (talentLegacy_talentLegacyNode == null)
			{
				return;
			}
			TalentLegacy_talentLegacyEffect talentLegacySkillCfgByLevel = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_talentLegacyId, num);
			if (talentLegacySkillCfgByLevel == null)
			{
				return;
			}
			InfoTipViewModule.InfoTipData infoTipData = new InfoTipViewModule.InfoTipData();
			infoTipData.m_name = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name);
			if (num <= 0)
			{
				infoTipData.m_info = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacySkillCfgByLevel.unlockDesc);
			}
			else
			{
				infoTipData.m_info = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacySkillCfgByLevel.desc);
			}
			RectTransform component = base.GetComponent<RectTransform>();
			Vector3 position = component.position;
			float num2 = component.sizeDelta.y / 2f;
			new Vector3(component.position.x, component.position.y, component.position.z);
			infoTipData.m_position = component.position;
			infoTipData.m_offsetX = 76f;
			infoTipData.m_offsetY = 174f;
			infoTipData.Open();
		}

		protected override void OnDeInit()
		{
			this.Button_Self.m_onClick = null;
		}

		public void OnShow()
		{
		}

		public void OnClose()
		{
		}

		public void SetData(int careerId, int talentLegacyNodeId)
		{
			if (this.m_talentLegacyModule == null)
			{
				return;
			}
			this.m_careerId = careerId;
			this.m_talentLegacyId = talentLegacyNodeId;
			int num = 0;
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyModule.OnGetTalentLegacySkillInfo(careerId, talentLegacyNodeId);
			if (talentLegacySkillInfo != null)
			{
				num = talentLegacySkillInfo.Level;
			}
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(talentLegacyNodeId);
			if (talentLegacy_talentLegacyNode == null)
			{
				return;
			}
			if (this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(talentLegacyNodeId, num) == null)
			{
				return;
			}
			bool flag = this.m_talentLegacyModule.IsUnlockTalentLegacyNode(talentLegacy_talentLegacyNode.id);
			bool flag2 = talentLegacySkillInfo != null && talentLegacySkillInfo.Level >= 1;
			bool flag3 = flag && flag2;
			if (talentLegacy_talentLegacyNode.type == 1 || talentLegacy_talentLegacyNode.type == 2)
			{
				this.Obj_Small.SetActiveSafe(true);
				this.Obj_Middle.SetActiveSafe(false);
				this.Obj_Big.SetActiveSafe(false);
				this.Image_SmallSkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
			}
			else if (talentLegacy_talentLegacyNode.type == 4)
			{
				this.Obj_Small.SetActiveSafe(false);
				this.Obj_Middle.SetActiveSafe(true);
				this.Obj_Big.SetActiveSafe(false);
				this.Image_MiddleSkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
			}
			else if (talentLegacy_talentLegacyNode.type == 3)
			{
				this.Obj_Small.SetActiveSafe(false);
				this.Obj_Middle.SetActiveSafe(false);
				this.Obj_Big.SetActiveSafe(true);
				this.Image_BigSkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
			}
			this.Text_Level.gameObject.SetActiveSafe(flag3);
			this.Text_Level.text = HLog.StringBuilder("Lv.", num.ToString());
			if (flag3)
			{
				this.Obj_Small.GetComponent<UIGrays>().Recovery();
				this.Obj_Big.GetComponent<UIGrays>().Recovery();
				this.Obj_Middle.GetComponent<UIGrays>().Recovery();
				return;
			}
			this.Obj_Small.GetComponent<UIGrays>().SetUIGray();
			this.Obj_Big.GetComponent<UIGrays>().SetUIGray();
			this.Obj_Middle.GetComponent<UIGrays>().SetUIGray();
		}

		[Header("小节点")]
		public GameObject Obj_Small;

		public CustomImage Image_SmallSkillIcon;

		[Header("中节点")]
		public GameObject Obj_Middle;

		public CustomImage Image_MiddleSkillIcon;

		[Header("大节点")]
		public GameObject Obj_Big;

		public CustomImage Image_BigProgressBg;

		public CustomImage Image_BigSkillIcon;

		public CustomButton Button_Self;

		public CustomText Text_Level;

		private TalentLegacyDataModule m_talentLegacyModule;

		private int m_careerId;

		private int m_talentLegacyId;
	}
}
