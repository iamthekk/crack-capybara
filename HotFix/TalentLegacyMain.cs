using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyMain : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Button_TalentLegacyRank.m_onClick = new Action(this.OnClickTalentLegacyRank);
			this.Ctrl_StudyItem.Init();
			for (int i = 0; i < this.CareerItemList.Count; i++)
			{
				this.CareerItemList[i].Init();
			}
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
		}

		protected override void OnDeInit()
		{
			this.Button_TalentLegacyRank.m_onClick = null;
			this.Ctrl_StudyItem.DeInit();
			for (int i = 0; i < this.CareerItemList.Count; i++)
			{
				this.CareerItemList[i].DeInit();
			}
		}

		private void OnClickTalentLegacyRank()
		{
			GameApp.Event.DispatchNow(null, 461, null);
		}

		public void OnShow()
		{
			if (this.m_talentLegacyModule == null)
			{
				return;
			}
			this.m_talentLegacyInfo = this.m_talentLegacyModule.OnGetTalentLegacyInfo();
			GuideController.Instance.DelTarget("Button_CareerItem");
			GuideController.Instance.AddTarget("Button_CareerItem", this.CareerItemList[0].transform);
			if (this.m_talentLegacyInfo.SelectCareerId > 0)
			{
				if (!GuideController.Instance.IfGuideOver(9))
				{
					EventArgGuideOver eventArgGuideOver = new EventArgGuideOver();
					eventArgGuideOver.SetData(9, 902, true);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_GuideOver, eventArgGuideOver);
				}
			}
			else
			{
				GuideController.Instance.DelTarget("Button_CareerEnabled");
				GuideController.Instance.AddTarget("Button_CareerEnabled", this.CareerItemList[0].Button_Enabled.transform);
			}
			GameApp.Event.RegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.RegisterEvent(465, new HandlerEvent(this.OnTalentLegacyNodeLevelUpBack));
			GameApp.Event.RegisterEvent(466, new HandlerEvent(this.OnTalentLegacyNodeLevelUpSpeedBack));
			GameApp.Event.RegisterEvent(470, new HandlerEvent(this.OnTalentLegacySelectCareerBack));
			this.Ctrl_StudyItem.OnShow();
			for (int i = 0; i < this.CareerItemList.Count; i++)
			{
				this.CareerItemList[i].OnShow();
			}
			this.OnRefreshView();
			GameTGAExtend.OnViewOpen("TalentLegacyMain");
		}

		public void OnClose()
		{
			GameApp.Event.UnRegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.UnRegisterEvent(465, new HandlerEvent(this.OnTalentLegacyNodeLevelUpBack));
			GameApp.Event.UnRegisterEvent(466, new HandlerEvent(this.OnTalentLegacyNodeLevelUpSpeedBack));
			GameApp.Event.UnRegisterEvent(470, new HandlerEvent(this.OnTalentLegacySelectCareerBack));
			this.Ctrl_StudyItem.OnClose();
			for (int i = 0; i < this.CareerItemList.Count; i++)
			{
				this.CareerItemList[i].OnClose();
			}
			GameTGAExtend.OnViewClose("TalentLegacyMain");
		}

		private void OnRefreshView()
		{
			if (this.m_talentLegacyInfo == null)
			{
				return;
			}
			for (int i = 0; i < this.CareerItemList.Count; i++)
			{
				this.CareerItemList[i].SetData(true, true, 1);
			}
			ValueTuple<int, int> valueTuple = this.m_talentLegacyModule.IsHaveStudyingNode();
			if (valueTuple.Item1 == -1 && valueTuple.Item2 == -1)
			{
				this.Ctrl_StudyItem.gameObject.SetActiveSafe(false);
				return;
			}
			this.Ctrl_StudyItem.gameObject.SetActiveSafe(true);
			this.Ctrl_StudyItem.SetData(valueTuple.Item1, valueTuple.Item2);
		}

		private void OnTalentLegacySelectCareerBack(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshView();
		}

		private void OnTalentLegacyNodeLevelUpSpeedBack(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshView();
		}

		private void OnTalentLegacyNodeLevelUpBack(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshView();
		}

		private void OnTalentLegacyNodeTimeEnd(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshView();
		}

		[Header("节点")]
		public List<TalentLegacyCareerItem> CareerItemList = new List<TalentLegacyCareerItem>();

		public CustomButton Button_TalentLegacyRank;

		public TalentLegacyStudyItem Ctrl_StudyItem;

		private TalentLegacyDataModule m_talentLegacyModule;

		private TalentLegacyDataModule.TalentLegacyInfo m_talentLegacyInfo;
	}
}
