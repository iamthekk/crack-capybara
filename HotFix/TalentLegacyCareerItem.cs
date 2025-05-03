using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using HotFix.EventArgs;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyCareerItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Button_Self.m_onClick = new Action(this.OnClickSelf);
			this.Button_Switch.m_onClick = new Action(this.OnClickSwitch);
			this.Button_Enabled.m_onClick = new Action(this.OnClickEnabled);
		}

		protected override void OnDeInit()
		{
			this.Button_Self.m_onClick = null;
			this.Button_Switch.m_onClick = null;
			this.Button_Enabled.m_onClick = null;
		}

		public void OnShow()
		{
		}

		public void OnClose()
		{
		}

		private void OnClickEnabled()
		{
			if (this.m_careerCfg == null)
			{
				return;
			}
			GuideController.Instance.CustomizeStringTrigger("ClickCareerEnabled");
			NetworkUtils.TalentLegacy.DoTalentLegacySelectCareerRequest(this.m_careerCfg.id, null);
		}

		private void OnClickSwitch()
		{
			if (this.m_careerCfg == null)
			{
				return;
			}
			TalentLegacySkillSelectViewModule.OpenData openData = default(TalentLegacySkillSelectViewModule.OpenData);
			openData.CareerId = this.m_careerCfg.id;
			openData.OriginType = 1;
			GameApp.View.OpenView(ViewName.TalentLegacySkillSelectViewModule, openData, 1, null, null);
		}

		private void OnClickSelf()
		{
			if (this.m_careerCfg == null)
			{
				return;
			}
			if (!this.m_isClick)
			{
				return;
			}
			if (this.m_careerCfg.isOpen == 0)
			{
				return;
			}
			if (!this.m_isUnlock)
			{
				if (this.m_careerCfg != null)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID(this.m_careerCfg.unLockTips));
				}
				return;
			}
			EventArgsOpenTree eventArgsOpenTree = new EventArgsOpenTree(this.m_careerCfg.id, -1);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_UIOpenTalentLegacyTree, eventArgsOpenTree);
		}

		public void PlayAni()
		{
			if (this.m_talentLegacyModule.OnGetTalentLegacyInfo().SelectCareerId == this.m_careerId)
			{
				this.Obj_NodeAni.SetBool("isPlay", true);
				return;
			}
			this.Obj_NodeAni.SetBool("isPlay", false);
		}

		public void SetData(bool isClick = true, bool isShowRed = false, int originType = 1)
		{
			this.m_isClick = isClick;
			this.m_isShowRed = isShowRed;
			int num = int.Parse(base.name.Split('_', StringSplitOptions.None)[1]);
			this.m_careerId = num;
			this.m_careerCfg = GameApp.Table.GetManager().GetTalentLegacy_career(num);
			if (this.m_careerCfg == null)
			{
				return;
			}
			this.PlayAni();
			this.Ctrl_Red.gameObject.SetActiveSafe(false);
			this.Ctrl_EnabledRed.gameObject.SetActiveSafe(false);
			this.Button_Self.enabled = this.m_careerCfg.isOpen != 0;
			if (this.m_careerCfg.isOpen == 0)
			{
				this.Obj_Empty.SetActiveSafe(true);
				this.Obj_Have.SetActiveSafe(false);
				this.Obj_Switch.SetActiveSafe(false);
				return;
			}
			this.Obj_Empty.SetActiveSafe(false);
			this.Obj_Have.SetActiveSafe(true);
			this.m_isUnlock = this.m_talentLegacyModule.IsUnlockTalentLegacyCareer(num);
			string text;
			int num2;
			if (!this.m_isUnlock)
			{
				text = "img_talentLegacyExpectDi";
				num2 = 160;
				this.Obj_Unlock.SetActiveSafe(true);
				this.Obj_Switch.SetActiveSafe(false);
			}
			else
			{
				text = "img_talentLegacyCareerDi";
				num2 = 162;
				this.Obj_Unlock.SetActiveSafe(false);
				this.Obj_Switch.SetActiveSafe(true);
				TalentLegacyDataModule.TalentLegacyInfo talentLegacyInfo = this.m_talentLegacyModule.OnGetTalentLegacyInfo();
				if (talentLegacyInfo != null)
				{
					if (talentLegacyInfo.SelectCareerId <= 0)
					{
						this.Button_Enabled.gameObject.SetActiveSafe(true);
						this.Button_Switch.gameObject.SetActiveSafe(false);
						this.Obj_Enabled.SetActiveSafe(false);
					}
					else if (talentLegacyInfo.SelectCareerId == num)
					{
						this.Button_Enabled.gameObject.SetActiveSafe(false);
						this.Button_Switch.gameObject.SetActiveSafe(false);
						this.Obj_Enabled.SetActiveSafe(true);
					}
					else
					{
						this.Button_Enabled.gameObject.SetActiveSafe(false);
						this.Button_Switch.gameObject.SetActiveSafe(true);
						this.Obj_Enabled.SetActiveSafe(false);
					}
					this.Ctrl_EnabledRed.gameObject.SetActiveSafe(talentLegacyInfo.SelectCareerId <= 0);
				}
				if (isShowRed)
				{
					this.Ctrl_Red.gameObject.SetActiveSafe(this.m_talentLegacyModule.OnGetTalentLegacyCareerRed(num, -1, false) == 1);
				}
			}
			this.Image_Bg.SetImage(num2, text);
			TalentLegacy_talentLegacyNode showTalentLegacyNodeByCareerId = this.m_talentLegacyModule.GetShowTalentLegacyNodeByCareerId(num);
			if (showTalentLegacyNodeByCareerId != null)
			{
				this.Text_SkillName.text = Singleton<LanguageManager>.Instance.GetInfoByID(showTalentLegacyNodeByCareerId.name);
				this.Image_Icon.SetImage(showTalentLegacyNodeByCareerId.iconId, showTalentLegacyNodeByCareerId.icon);
				this.Image_Unlock.SetImage(showTalentLegacyNodeByCareerId.iconId, showTalentLegacyNodeByCareerId.icon);
			}
			string careerTotalFinishProgress = this.m_talentLegacyModule.GetCareerTotalFinishProgress(num);
			this.Text_CareerProgress.text = HLog.StringBuilder(new string[] { Singleton<LanguageManager>.Instance.GetInfoByID(this.m_careerCfg.nameID) });
			this.Image_Stage.SetActiveSafe(!string.IsNullOrEmpty(careerTotalFinishProgress));
			this.Text_Stage.text = careerTotalFinishProgress;
		}

		public GameObject Obj_Have;

		public GameObject Obj_Empty;

		public CustomImage Image_Bg;

		public CustomImage Image_Icon;

		public GameObject Obj_Unlock;

		public CustomImage Image_Unlock;

		public CustomText Text_SkillName;

		public CustomText Text_CareerProgress;

		public CustomButton Button_Self;

		public GameObject Obj_Switch;

		public CustomButton Button_Switch;

		public CustomButton Button_Enabled;

		public GameObject Obj_Enabled;

		public GameObject Image_Stage;

		public CustomText Text_Stage;

		public RedNodeOneCtrl Ctrl_Red;

		public RedNodeOneCtrl Ctrl_EnabledRed;

		public Animator Obj_NodeAni;

		private int m_careerId;

		private TalentLegacyDataModule m_talentLegacyModule;

		private TalentLegacy_career m_careerCfg;

		private bool m_isClick = true;

		private bool m_isShowRed;

		private bool m_isUnlock;
	}
}
