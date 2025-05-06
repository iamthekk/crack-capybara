using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacySkillSelectViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.Ctrl_PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnClickClose);
			this.Button_Switch.m_onClick = new Action(this.OnClickSwitch);
			this.Button_SwitchChange.m_onClick = new Action(this.OnClickSwitchChange);
			this.Ctrl_SpineModelItem.Init();
			TalentLegacySkillSlotItem component = this.Obj_SlotItem.GetComponent<TalentLegacySkillSlotItem>();
			component.Init();
			this.m_skillSlotItemList.Add(component);
			TalentLegacySkillSelectItem component2 = this.Obj_SkillSelectItem.GetComponent<TalentLegacySkillSelectItem>();
			component2.Init();
			this.m_selectItemList.Add(component2);
			this.m_talentLegacyDataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = (TalentLegacySkillSelectViewModule.OpenData)data;
			this.OnRefreshView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			for (int i = 0; i < this.m_selectItemList.Count; i++)
			{
				this.m_selectItemList[i].OnClose();
			}
		}

		public override void OnDelete()
		{
			this.Ctrl_PopCommon.OnClick = null;
			this.Button_Switch.m_onClick = null;
			this.Button_SwitchChange.m_onClick = null;
			for (int i = 0; i < this.m_skillSlotItemList.Count; i++)
			{
				this.m_skillSlotItemList[i].DeInit();
			}
			for (int j = 0; j < this.m_selectItemList.Count; j++)
			{
				this.m_selectItemList[j].DeInit();
			}
			this.Ctrl_SpineModelItem.DeInit();
		}

		private void OnClickSwitchChange()
		{
			this.OnClickClose(UIPopCommon.UIPopCommonClickType.ButtonClose);
			if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.MainTalent, null, true))
			{
				UIBaseMainPageNode.OpenData openData = new UIBaseMainPageNode.OpenData();
				openData.OriginType = UIBaseMainPageNode.EOriginType.Equip;
				Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainTalent, openData);
			}
		}

		private void OnClickSwitch()
		{
			NetworkUtils.TalentLegacy.DoTalentLegacySelectCareerRequest(this.m_openData.CareerId, null);
		}

		private void OnClickClose(UIPopCommon.UIPopCommonClickType clickType)
		{
			GameApp.View.CloseView(ViewName.TalentLegacySkillSelectViewModule, null);
		}

		private void OnRefreshView()
		{
			this.m_talentLegacyInfo = this.m_talentLegacyDataModule.OnGetTalentLegacyInfo();
			if (this.m_talentLegacyInfo == null)
			{
				return;
			}
			this.Button_Switch.gameObject.SetActiveSafe(false);
			this.Obj_Enabled.gameObject.SetActiveSafe(false);
			this.Button_SwitchChange.gameObject.SetActiveSafe(false);
			if (this.m_openData.OriginType == 1)
			{
				if (this.m_talentLegacyInfo.SelectCareerId == this.m_openData.CareerId)
				{
					this.Obj_Enabled.gameObject.SetActiveSafe(true);
				}
				else
				{
					this.Button_Switch.gameObject.SetActiveSafe(true);
				}
			}
			else if (this.m_openData.OriginType == 2)
			{
				this.Button_SwitchChange.gameObject.SetActiveSafe(true);
			}
			this.Ctrl_SpineModelItem.gameObject.SetActiveSafe(false);
			TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(this.m_openData.CareerId);
			TalentLegacy_talentLegacyNode showTalentLegacyNodeByCareerId = this.m_talentLegacyDataModule.GetShowTalentLegacyNodeByCareerId(this.m_openData.CareerId);
			if (showTalentLegacyNodeByCareerId != null)
			{
				ArtMember_member artMember_member = GameApp.Table.GetManager().GetArtMember_member(showTalentLegacyNodeByCareerId.spineID);
				if (artMember_member != null)
				{
					this.Ctrl_SpineModelItem.gameObject.SetActiveSafe(true);
					this.Ctrl_SpineModelItem.ShowModel(showTalentLegacyNodeByCareerId.spineID, 0, "Idle", true);
					this.Ctrl_SpineModelItem.SetScale(artMember_member.uiScale);
				}
			}
			if (talentLegacy_career != null)
			{
				string careerTotalFinishProgress = this.m_talentLegacyDataModule.GetCareerTotalFinishProgress(this.m_openData.CareerId);
				this.Text_Name.text = HLog.StringBuilder(Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_career.nameID), careerTotalFinishProgress);
			}
			this.m_cfgList.Clear();
			this.m_cfgList.AddRange(this.m_talentLegacyDataModule.GetCareerTalentLegacyListAllCfg(this.m_openData.CareerId, 4));
			this.OnRefreshSelectSkill();
			this.OnRefreshSkill();
		}

		private void OnRefreshSelectSkill()
		{
			if (this.m_selectItemList.Count < this.m_cfgList.Count)
			{
				int num = this.m_cfgList.Count - this.m_selectItemList.Count;
				for (int i = 0; i < num; i++)
				{
					TalentLegacySkillSelectItem component = Object.Instantiate<GameObject>(this.Obj_SkillSelectItem, this.Obj_SkillSelectParent.transform).GetComponent<TalentLegacySkillSelectItem>();
					component.Init();
					this.m_selectItemList.Add(component);
				}
			}
			for (int j = 0; j < this.m_selectItemList.Count; j++)
			{
				if (j > this.m_cfgList.Count - 1)
				{
					this.m_selectItemList[j].gameObject.SetActiveSafe(false);
				}
				else
				{
					this.m_selectItemList[j].gameObject.SetActiveSafe(true);
					this.m_selectItemList[j].OnShow();
					this.m_selectItemList[j].SetData(this.m_openData.CareerId, this.m_cfgList[j].id);
				}
			}
		}

		private void OnRefreshSkill()
		{
			int num = int.Parse(GameApp.Table.GetManager().GetGameConfig_Config(8503).Value);
			if (this.m_skillSlotItemList.Count < num)
			{
				int num2 = num - this.m_skillSlotItemList.Count;
				for (int i = 0; i < num2; i++)
				{
					TalentLegacySkillSlotItem component = Object.Instantiate<GameObject>(this.Obj_SlotItem, this.Obj_Slot.transform).GetComponent<TalentLegacySkillSlotItem>();
					component.Init();
					this.m_skillSlotItemList.Add(component);
				}
			}
			TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo = this.m_talentLegacyDataModule.OnGetTalentLegacyCareerInfo(this.m_openData.CareerId);
			if (talentLegacyCareerInfo == null)
			{
				return;
			}
			for (int j = 0; j < this.m_skillSlotItemList.Count; j++)
			{
				if (this.m_talentLegacyInfo.AssemblySlotCount > j)
				{
					if (talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList.Count > j && talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList[j] != 0)
					{
						this.m_skillSlotItemList[j].SetData(2, j, talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList[j], this.m_openData.CareerId);
					}
					else
					{
						this.m_skillSlotItemList[j].SetData(1, j, -1, this.m_openData.CareerId);
					}
				}
				else
				{
					this.m_skillSlotItemList[j].SetData(0, j, -1, this.m_openData.CareerId);
				}
			}
		}

		private void OnTalentLegacySelectCareerBack(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshView();
		}

		private void OnTalentLegacySkillChange(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshView();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(470, new HandlerEvent(this.OnTalentLegacySelectCareerBack));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TalentLegacySkillChange, new HandlerEvent(this.OnTalentLegacySkillChange));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(470, new HandlerEvent(this.OnTalentLegacySelectCareerBack));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TalentLegacySkillChange, new HandlerEvent(this.OnTalentLegacySkillChange));
		}

		public UIPopCommon Ctrl_PopCommon;

		[Header("传承技")]
		public GameObject Obj_Slot;

		public GameObject Obj_SlotItem;

		public CustomText Text_Name;

		public UISpineModelItem Ctrl_SpineModelItem;

		[Header("被动技能")]
		public GameObject Obj_SkillSelectItem;

		public GameObject Obj_SkillSelectParent;

		public CustomButton Button_Switch;

		public GameObject Obj_Enabled;

		public CustomButton Button_SwitchChange;

		private TalentLegacySkillSelectViewModule.OpenData m_openData;

		private TalentLegacyDataModule m_talentLegacyDataModule;

		private TalentLegacyDataModule.TalentLegacyInfo m_talentLegacyInfo;

		private List<TalentLegacySkillSlotItem> m_skillSlotItemList = new List<TalentLegacySkillSlotItem>();

		private List<TalentLegacySkillSelectItem> m_selectItemList = new List<TalentLegacySkillSelectItem>();

		private List<TalentLegacy_talentLegacyNode> m_cfgList = new List<TalentLegacy_talentLegacyNode>();

		public struct OpenData
		{
			public int CareerId;

			public int OriginType;
		}
	}
}
