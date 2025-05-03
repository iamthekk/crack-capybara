using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyNodeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Button_Self.m_onClick = new Action(this.OnClickSelf);
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

		private void OnClickSelf()
		{
			if (!this.m_isClick)
			{
				return;
			}
			TalentLegacyStudyViewModule.OpenData openData = new TalentLegacyStudyViewModule.OpenData();
			openData.CareerId = this.m_careerId;
			openData.TalentLagcyNodeId = this.m_talentLegacyNodeId;
			GameApp.View.OpenView(ViewName.TalentLegacyStudyViewModule, openData, 1, null, null);
		}

		private void SetEffectHide()
		{
			this.Obj_SmallEffectStudying.SetActiveSafe(false);
			this.Obj_SmallEffectSpeed.SetActiveSafe(false);
			this.Obj_SmallEffectFinish.SetActiveSafe(false);
			this.Obj_MiddleEffectStudying.SetActiveSafe(false);
			this.Obj_MiddleEffectSpeed.SetActiveSafe(false);
			this.Obj_MiddleEffectFinish.SetActiveSafe(false);
			this.Obj_BigEffectStudying.SetActiveSafe(false);
			this.Obj_BigEffectStudyingBack.SetActiveSafe(false);
			this.Obj_BigEffectSpeed.SetActiveSafe(false);
			this.Obj_BigEffectFinish.SetActiveSafe(false);
		}

		public void PlayStudyFinishEffect()
		{
			this.Obj_SmallEffectFinish.SetActiveSafe(true);
			this.Obj_MiddleEffectFinish.SetActiveSafe(true);
			this.Obj_BigEffectFinish.SetActiveSafe(true);
		}

		public void PlayStudySpeedEffect()
		{
			this.Obj_SmallEffectSpeed.SetActiveSafe(true);
			this.Obj_MiddleEffectSpeed.SetActiveSafe(true);
			this.Obj_BigEffectSpeed.SetActiveSafe(true);
		}

		public void SetData(int careerId, int talentLegacyNodeId, bool isShowRed = false, bool isClick = false, bool isPlayEffect = true)
		{
			if (this.m_talentLegacyModule == null)
			{
				return;
			}
			if (talentLegacyNodeId == 10101)
			{
				GuideController.Instance.DelTarget("Button_NodeItem");
				GuideController.Instance.AddTarget("Button_NodeItem", base.transform);
			}
			base.name = careerId.ToString();
			this.SetEffectHide();
			this.Ctrl_SmallRed.gameObject.SetActiveSafe(false);
			this.Ctrl_MiddleRed.gameObject.SetActiveSafe(false);
			this.Ctrl_BigRed.gameObject.SetActiveSafe(false);
			this.m_isClick = isClick;
			this.Button_Self.enabled = isClick;
			this.m_careerId = careerId;
			this.m_talentLegacyNodeId = talentLegacyNodeId;
			int num = 0;
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyModule.OnGetTalentLegacySkillInfo(careerId, talentLegacyNodeId);
			if (talentLegacySkillInfo != null)
			{
				num = talentLegacySkillInfo.Level;
			}
			bool flag = false;
			if (talentLegacySkillInfo != null && talentLegacySkillInfo.LevelUpTime > 0L)
			{
				flag = true;
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
			List<TalentLegacy_talentLegacyEffect> talentLegacySkillListById = this.m_talentLegacyModule.GetTalentLegacySkillListById(talentLegacyNodeId);
			bool flag2 = this.m_talentLegacyModule.IsUnlockTalentLegacyNode(talentLegacy_talentLegacyNode.id);
			if (talentLegacy_talentLegacyNode.type == 1 || talentLegacy_talentLegacyNode.type == 2)
			{
				this.Obj_Small.SetActiveSafe(true);
				this.Obj_Middle.SetActiveSafe(false);
				this.Obj_Big.SetActiveSafe(false);
				this.Image_SmallSkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
				this.Text_SmallProgress.text = HLog.StringBuilderFormat("{0}/{1}", new object[] { num, talentLegacySkillListById.Count });
				this.Image_SmallProgress.fillAmount = Mathf.Clamp01((float)num / (float)talentLegacySkillListById.Count);
				if (num >= talentLegacySkillListById.Count)
				{
					this.Image_SmallProgressBg.SetImage(160, "img_talentLegacySmallFinish");
				}
				else
				{
					this.Image_SmallProgressBg.SetImage(160, "img_talentLegacySmallUnFinsh");
				}
				if (flag2)
				{
					this.Obj_Small.GetComponent<UIGrays>().Recovery();
				}
				else
				{
					this.Obj_Small.GetComponent<UIGrays>().SetUIGray();
					this.Image_SmallProgress.fillAmount = 1f;
				}
				if (flag && isPlayEffect)
				{
					this.Obj_SmallEffectStudying.SetActiveSafe(true);
				}
			}
			else if (talentLegacy_talentLegacyNode.type == 4)
			{
				this.Obj_Small.SetActiveSafe(false);
				this.Obj_Middle.SetActiveSafe(true);
				this.Obj_Big.SetActiveSafe(false);
				this.Image_MiddleSkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
				this.Text_MiddlProgress.text = HLog.StringBuilderFormat("{0}/{1}", new object[] { num, talentLegacySkillListById.Count });
				this.Image_MiddleProgress.fillAmount = Mathf.Clamp01((float)num / (float)talentLegacySkillListById.Count);
				this.Text_MiddleName.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name);
				this.Obj_MiddleStage.SetActiveSafe(!string.IsNullOrEmpty(talentLegacy_talentLegacyNode.romeNumber));
				this.Text_MiddleStage.text = talentLegacy_talentLegacyNode.romeNumber;
				if (num >= talentLegacySkillListById.Count)
				{
					this.Image_MiddleProgressBg.SetImage(160, "img_talentLegacyMiddleFinish");
				}
				else
				{
					this.Image_MiddleProgressBg.SetImage(160, "img_talentLegacyMiddleUnFinish");
				}
				if (flag2)
				{
					this.Obj_Middle.GetComponent<UIGrays>().Recovery();
				}
				else
				{
					this.Obj_Middle.GetComponent<UIGrays>().SetUIGray();
					this.Image_MiddleProgress.fillAmount = 1f;
				}
				if (flag && isPlayEffect)
				{
					this.Obj_MiddleEffectStudying.SetActiveSafe(true);
				}
			}
			else if (talentLegacy_talentLegacyNode.type == 3)
			{
				this.Obj_Small.SetActiveSafe(false);
				this.Obj_Middle.SetActiveSafe(false);
				this.Obj_Big.SetActiveSafe(true);
				this.Obj_BigUnlock.SetActiveSafe(!flag2);
				this.Image_BigSkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
				this.Image_UnlockBigSkillIcon.SetImage(talentLegacy_talentLegacyNode.iconId, talentLegacy_talentLegacyNode.icon);
				this.Image_BigProgress.fillAmount = Mathf.Clamp01((float)num / (float)talentLegacySkillListById.Count);
				this.Text_BigProgress.text = HLog.StringBuilderFormat("{0}/{1}", new object[] { num, talentLegacySkillListById.Count });
				this.Text_BigName.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name);
				this.Obj_BigStage.SetActiveSafe(!string.IsNullOrEmpty(talentLegacy_talentLegacyNode.romeNumber));
				this.Text_BigStage.text = talentLegacy_talentLegacyNode.romeNumber;
				if (flag2)
				{
					this.Image_BigProgressBg.SetImage(162, "img_talentLegacyCareerDi");
					this.Obj_Big.GetComponent<UIGrays>().Recovery();
				}
				else
				{
					this.Image_BigProgressBg.SetImage(160, "img_talentLegacyExpectDi");
					this.Obj_Big.GetComponent<UIGrays>().SetUIGray();
					this.Image_BigProgress.fillAmount = 1f;
				}
				if (flag && isPlayEffect)
				{
					this.Obj_BigEffectStudying.SetActiveSafe(true);
					this.Obj_BigEffectStudyingBack.SetActiveSafe(true);
				}
			}
			if (flag2 && isShowRed)
			{
				bool flag3 = this.m_talentLegacyModule.OnGetTalentLegacyCareerRed(careerId, talentLegacyNodeId, false) == 1;
				this.Ctrl_SmallRed.gameObject.SetActiveSafe(flag3);
				this.Ctrl_MiddleRed.gameObject.SetActiveSafe(flag3);
				this.Ctrl_BigRed.gameObject.SetActiveSafe(flag3);
			}
		}

		public GameObject Obj_Node;

		[Header("小节点")]
		public GameObject Obj_Small;

		public CustomImage Image_SmallSkillIcon;

		public CustomImage Image_SmallProgressBg;

		public CustomImage Image_SmallProgress;

		public CustomText Text_SmallProgress;

		public GameObject Obj_SmallEffectStudying;

		public GameObject Obj_SmallEffectSpeed;

		public GameObject Obj_SmallEffectFinish;

		public RedNodeOneCtrl Ctrl_SmallRed;

		[Header("中节点")]
		public GameObject Obj_Middle;

		public CustomImage Image_MiddleSkillIcon;

		public CustomImage Image_MiddleProgressBg;

		public CustomImage Image_MiddleProgress;

		public CustomText Text_MiddlProgress;

		public GameObject Obj_MiddleEffectStudying;

		public GameObject Obj_MiddleEffectSpeed;

		public GameObject Obj_MiddleEffectFinish;

		public GameObject Obj_MiddleStage;

		public CustomText Text_MiddleStage;

		public CustomText Text_MiddleName;

		public RedNodeOneCtrl Ctrl_MiddleRed;

		[Header("大节点")]
		public GameObject Obj_Big;

		public CustomImage Image_BigProgressBg;

		public CustomImage Image_BigProgressDi;

		public CustomImage Image_BigSkillIcon;

		public GameObject Obj_BigUnlock;

		public CustomImage Image_UnlockBigSkillIcon;

		public CustomImage Image_BigProgress;

		public CustomText Text_BigProgress;

		public GameObject Obj_BigEffectStudying;

		public GameObject Obj_BigEffectStudyingBack;

		public GameObject Obj_BigEffectSpeed;

		public GameObject Obj_BigEffectFinish;

		public GameObject Obj_BigStage;

		public CustomText Text_BigStage;

		public CustomText Text_BigName;

		public RedNodeOneCtrl Ctrl_BigRed;

		public CustomButton Button_Self;

		private bool m_isClick;

		private TalentLegacyDataModule m_talentLegacyModule;

		private int m_careerId = -1;

		private int m_talentLegacyNodeId = -1;
	}
}
