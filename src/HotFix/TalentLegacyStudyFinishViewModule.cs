using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyStudyFinishViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.Button_Mask.m_onClick = new Action(this.OnClickClose);
			this.Ctrl_Close.OnClose = new Action(this.OnClickClose);
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Ctrl_NodeItem.Init();
			this.m_attrItemList.Add(this.Ctrl_AttrItem.GetComponent<TalentLegacyStudyFinishAttrItem>());
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.TalentLegacyStudyFinishViewModule, null);
			GameApp.View.CloseView(ViewName.TalentLegacyBigStudyViewModule, null);
		}

		public override void OnOpen(object data)
		{
			if (this.m_talentLegacyModule == null)
			{
				this.OnClickClose();
				return;
			}
			GameApp.Sound.PlayClip(688, 1f);
			this.Ctrl_NodeItem.OnShow();
			this.m_openData = (TalentLegacyStudyFinishViewModule.OpenData)data;
			NetworkUtils.TalentLegacy.DoTalentLegacyInfoRequest(null, false);
			PlayerPrefsKeys.SetTalentLegacyNodeFinish("");
			this.Text_Bg.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("legacy_study_finish_title"));
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(this.m_openData.TalentLegacyNodeId);
			if (talentLegacy_talentLegacyNode == null)
			{
				return;
			}
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyModule.OnGetTalentLegacySkillInfo(this.m_openData.CareerId, this.m_openData.TalentLegacyNodeId);
			if (talentLegacySkillInfo == null)
			{
				return;
			}
			TalentLegacy_talentLegacyEffect talentLegacySkillCfgByLevel = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_openData.TalentLegacyNodeId, talentLegacySkillInfo.Level);
			if (talentLegacySkillCfgByLevel == null)
			{
				return;
			}
			this.Text_SkillName.text = HLog.StringBuilderFormat("{0}Lv.{1}", new object[]
			{
				Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name),
				talentLegacySkillInfo.Level
			});
			this.Ctrl_NodeItem.SetData(this.m_openData.CareerId, this.m_openData.TalentLegacyNodeId, false, false, true);
			this.Ctrl_NodeItem.PlayStudyFinishEffect();
			if (talentLegacySkillCfgByLevel.skills.Length == 0)
			{
				this.Text_UnlockTitle.gameObject.SetActiveSafe(false);
				this.Text_UnlockDes.gameObject.SetActiveSafe(false);
				if (this.Image_Line != null)
				{
					this.Image_Line.SetActiveSafe(false);
				}
			}
			else
			{
				if (this.Image_Line != null)
				{
					this.Image_Line.SetActiveSafe(true);
				}
				this.Text_UnlockTitle.gameObject.SetActiveSafe(true);
				this.Text_UnlockDes.gameObject.SetActiveSafe(true);
				this.Text_UnlockTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_skill_newunlock");
				this.Text_UnlockDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacySkillCfgByLevel.desc);
			}
			bool flag = true;
			TalentLegacy_talentLegacyEffect talentLegacy_talentLegacyEffect = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_openData.TalentLegacyNodeId, talentLegacySkillInfo.Level - 1);
			if (talentLegacySkillInfo.Level == 1)
			{
				talentLegacy_talentLegacyEffect = null;
			}
			if (talentLegacy_talentLegacyEffect != null)
			{
				List<MergeAttributeData> mergeAttributeData = talentLegacySkillCfgByLevel.attributes.GetMergeAttributeData();
				List<MergeAttributeData> mergeAttributeData2 = talentLegacy_talentLegacyEffect.attributes.GetMergeAttributeData();
				if (talentLegacy_talentLegacyEffect.attributes.Length == talentLegacySkillCfgByLevel.attributes.Length)
				{
					bool flag2 = false;
					for (int i = 0; i < mergeAttributeData.Count; i++)
					{
						if (mergeAttributeData[i].Value != mergeAttributeData2[i].Value)
						{
							flag2 = true;
							break;
						}
					}
					flag = flag2;
				}
			}
			List<MergeAttributeData> mergeAttributeData3 = talentLegacySkillCfgByLevel.attributes.GetMergeAttributeData();
			if (mergeAttributeData3.Count <= 0 || !flag)
			{
				this.Obj_Attr.SetActiveSafe(false);
				if (talentLegacy_talentLegacyNode.type == 4 || talentLegacy_talentLegacyNode.type == 3)
				{
					this.Ctrl_NodeItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -43f);
					return;
				}
			}
			else
			{
				if (talentLegacy_talentLegacyNode.type == 4 || talentLegacy_talentLegacyNode.type == 3)
				{
					this.Ctrl_NodeItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(-259f, -43f);
				}
				this.Obj_Attr.SetActiveSafe(true);
				if (this.m_attrItemList.Count < mergeAttributeData3.Count)
				{
					int num = mergeAttributeData3.Count - this.m_attrItemList.Count;
					for (int j = 0; j < num; j++)
					{
						TalentLegacyStudyFinishAttrItem component = Object.Instantiate<GameObject>(this.Ctrl_AttrItem, this.Obj_Attr.transform).GetComponent<TalentLegacyStudyFinishAttrItem>();
						component.Init();
						this.m_attrItemList.Add(component);
					}
				}
				for (int k = 0; k < this.m_attrItemList.Count; k++)
				{
					if (k > mergeAttributeData3.Count - 1)
					{
						this.m_attrItemList[k].gameObject.SetActiveSafe(false);
					}
					else
					{
						this.m_attrItemList[k].gameObject.SetActiveSafe(true);
						this.m_attrItemList[k].OnShow();
						this.m_attrItemList[k].SetData(mergeAttributeData3[k]);
					}
				}
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.Ctrl_NodeItem.OnClose();
			for (int i = 0; i < this.m_attrItemList.Count; i++)
			{
				this.m_attrItemList[i].OnClose();
			}
		}

		public override void OnDelete()
		{
			this.Button_Mask.m_onClick = null;
			this.Ctrl_Close.OnClose = null;
			this.Ctrl_NodeItem.DeInit();
			for (int i = 0; i < this.m_attrItemList.Count; i++)
			{
				this.m_attrItemList[i].DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public CustomButton Button_Mask;

		public TapToCloseCtrl Ctrl_Close;

		public TalentLegacyNodeItem Ctrl_NodeItem;

		public CustomText Text_SkillName;

		public UIBgText Text_Bg;

		public GameObject Obj_Attr;

		public GameObject Ctrl_AttrItem;

		public GameObject Image_Line;

		public CustomText Text_UnlockTitle;

		public CustomText Text_UnlockDes;

		private TalentLegacyStudyFinishViewModule.OpenData m_openData;

		private TalentLegacyDataModule m_talentLegacyModule;

		private List<TalentLegacyStudyFinishAttrItem> m_attrItemList = new List<TalentLegacyStudyFinishAttrItem>();

		public struct OpenData
		{
			public int CareerId;

			public int TalentLegacyNodeId;
		}
	}
}
