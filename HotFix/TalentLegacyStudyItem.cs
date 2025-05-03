using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyStudyItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Ctrl_NodeItem.Init();
			this.Button_Speed.m_onClick = new Action(this.OnClickAddSpeed);
		}

		protected override void OnDeInit()
		{
			this.Button_Speed.m_onClick = null;
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnStartCountDown));
			this.Ctrl_NodeItem.DeInit();
		}

		public void OnShow()
		{
			this.Ctrl_NodeItem.OnShow();
		}

		public void OnClose()
		{
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnStartCountDown));
			this.Ctrl_NodeItem.OnClose();
		}

		private void OnClickAddSpeed()
		{
			TalentLegacyStudyViewModule.OpenData openData = new TalentLegacyStudyViewModule.OpenData();
			openData.CareerId = this.m_careerId;
			openData.TalentLagcyNodeId = this.m_talentLegacyId;
			GameApp.View.OpenView(ViewName.TalentLegacyStudyViewModule, openData, 1, null, null);
		}

		public void SetData(int careerId, int talentLegacyNodeId)
		{
			if (this.m_talentLegacyModule == null)
			{
				return;
			}
			this.m_careerId = careerId;
			this.m_talentLegacyId = talentLegacyNodeId;
			this.m_skillInfoData = this.m_talentLegacyModule.OnGetTalentLegacySkillInfo(careerId, talentLegacyNodeId);
			if (this.m_skillInfoData == null)
			{
				return;
			}
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(this.m_skillInfoData.TalentLegacyNodeId);
			if (talentLegacy_talentLegacyNode == null)
			{
				return;
			}
			TalentLegacy_talentLegacyEffect talentLegacySkillCfgByLevel = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_skillInfoData.TalentLegacyNodeId, this.m_skillInfoData.Level + 1);
			if (talentLegacySkillCfgByLevel == null)
			{
				return;
			}
			this.Text_SkillName.text = HLog.StringBuilderFormat("{0}Lv.{1}", new object[]
			{
				Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name),
				this.m_skillInfoData.Level + 1
			});
			if (string.IsNullOrEmpty(talentLegacySkillCfgByLevel.desc))
			{
				List<MergeAttributeData> mergeAttributeData = talentLegacySkillCfgByLevel.attributes.GetMergeAttributeData();
				string text = "";
				for (int i = 0; i < mergeAttributeData.Count; i++)
				{
					Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData[i].Header);
					if (elementById != null)
					{
						string text2 = "";
						if (mergeAttributeData[i].Header.Contains("%"))
						{
							text2 = "%";
						}
						string text3 = DxxTools.FormatNumber((long)mergeAttributeData[i].Value);
						text = HLog.StringBuilder(text, Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId), "<color=#298744>", " +", text3, text2, "</color>", (i == mergeAttributeData.Count - 1) ? "" : ",");
					}
				}
				this.Text_SkillDesc.text = text;
			}
			else
			{
				this.Text_SkillDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacySkillCfgByLevel.desc);
			}
			this.Ctrl_NodeItem.SetData(careerId, talentLegacyNodeId, false, false, true);
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnStartCountDown));
			long num = this.m_skillInfoData.LevelUpTime - DxxTools.Time.ServerTimestamp;
			if (num > 0L)
			{
				this.Obj_Time.SetActiveSafe(true);
				this.Text_Time.text = DxxTools.FormatFullTimeWithDay(num);
				DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.OnStartCountDown));
			}
			else
			{
				this.Obj_Time.SetActiveSafe(false);
			}
			this.Ctrl_Red.gameObject.SetActiveSafe(this.m_talentLegacyModule.OnGetTalentLegacyCareerRed(careerId, talentLegacyNodeId, false) == 1);
		}

		private void OnStartCountDown()
		{
			if (this.m_skillInfoData != null)
			{
				long num = this.m_skillInfoData.LevelUpTime - DxxTools.Time.ServerTimestamp;
				this.Obj_Time.SetActiveSafe(num > 0L);
				this.Text_Time.text = DxxTools.FormatFullTimeWithDay(num);
			}
		}

		public CustomText Text_SkillName;

		public CustomText Text_SkillDesc;

		public GameObject Obj_Time;

		public CustomText Text_Time;

		public CustomButton Button_Speed;

		public TalentLegacyNodeItem Ctrl_NodeItem;

		public RedNodeOneCtrl Ctrl_Red;

		private TalentLegacyDataModule m_talentLegacyModule;

		private TalentLegacyDataModule.TalentLegacySkillInfo m_skillInfoData;

		private int m_careerId;

		private int m_talentLegacyId;
	}
}
