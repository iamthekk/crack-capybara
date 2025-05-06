using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using HotFix;
using Proto.LeaderBoard;

public class TalentLegacyRank : CustomBehaviour
{
	protected override void OnInit()
	{
		for (int i = 0; i < this.m_rankTopItemList.Count; i++)
		{
			this.m_rankTopItemList[i].Init();
		}
		this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
		this.Button_Preview.m_onClick = new Action(this.OnClickPreview);
		this.Button_Inherit.m_onClick = new Action(this.OnClickInherit);
		RedPointController.Instance.RegRecordChange("Talent.TalentLegacy.TalentLegacyNode", new Action<RedNodeListenData>(this.OnRefreshInheritRedPoint));
		this.helpButton.Init();
	}

	protected override void OnDeInit()
	{
		this.Button_Preview.m_onClick = null;
		this.Button_Inherit.m_onClick = null;
		for (int i = 0; i < this.m_rankTopItemList.Count; i++)
		{
			this.m_rankTopItemList[i].DeInit();
		}
		this.helpButton.DeInit();
		RedPointController.Instance.UnRegRecordChange("Talent.TalentLegacy.TalentLegacyNode", new Action<RedNodeListenData>(this.OnRefreshInheritRedPoint));
	}

	private void OnClickInherit()
	{
		if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
		{
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("legacy_Unlock"));
			return;
		}
		GameApp.Event.Dispatch(null, LocalMessageName.CC_UIOpenTalentLegacyMain, null);
	}

	private void OnClickPreview()
	{
		GameApp.View.OpenView(ViewName.TalentEvolutionPreviewViewModule, null, 1, null, null);
	}

	public void OnShow()
	{
		if (this.m_talentLegacyModule == null)
		{
			return;
		}
		for (int i = 0; i < this.m_rankTopItemList.Count; i++)
		{
			this.m_rankTopItemList[i].OnShow();
		}
		GuideController.Instance.DelTarget("Button_Inherit");
		GuideController.Instance.AddTarget("Button_Inherit", this.Button_Inherit.transform);
		this.OnRefreshView();
		GameTGAExtend.OnViewOpen("TalentLegacyRank");
	}

	private void OnRefreshView()
	{
		this.m_topUserList = this.m_talentLegacyModule.Top3User;
		if (this.m_topUserList == null)
		{
			return;
		}
		this.Text_Time.gameObject.SetActiveSafe(false);
		NewWorldDataModule dataModule = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
		this.m_type = (dataModule.IsEnterNewWorld ? 2 : 1);
		this.Text_Inherit.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_rank_btnName2");
		if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
		{
			this.Button_Inherit.GetComponent<UIGrays>().Recovery();
		}
		else
		{
			this.Button_Inherit.GetComponent<UIGrays>().SetUIGray();
		}
		for (int i = 0; i < this.m_rankTopItemList.Count; i++)
		{
			if (i >= this.m_topUserList.Count)
			{
				this.m_rankTopItemList[i].SetData(RankType.NewWorld, null, string.Format("TalentLegacyPlayer_{0}", i + 1));
			}
			else
			{
				this.m_rankTopItemList[i].SetData(RankType.NewWorld, this.m_topUserList[i], string.Format("TalentLegacyPlayer_{0}", i + 1));
				this.m_rankTopItemList[i].ShowCustomNewWorld();
			}
		}
	}

	public void OnClose()
	{
		for (int i = 0; i < this.m_rankTopItemList.Count; i++)
		{
			this.m_rankTopItemList[i].OnHide();
		}
		GameTGAExtend.OnViewClose("TalentLegacyRank");
	}

	private void OnRefreshInheritRedPoint(RedNodeListenData obj)
	{
		this.Red_Inherit.Value = obj.m_count;
	}

	public CustomButton Button_Preview;

	public CustomButton Button_Inherit;

	public CustomText Text_Inherit;

	public RedNodeOneCtrl Red_Inherit;

	public CustomLanguageText Text_Time;

	public List<UIRankTopItem> m_rankTopItemList = new List<UIRankTopItem>();

	public UIHelpButton helpButton;

	private RepeatedField<RankUserDto> m_topUserList = new RepeatedField<RankUserDto>();

	private TalentLegacyDataModule m_talentLegacyModule;

	private int m_type = 1;
}
