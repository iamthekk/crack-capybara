using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix.CrossArenaRewardsUI
{
	public class CrossArenaRewardsViewCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.Scroll.Init();
			this.MyRank.Init();
			this.BottomButtons.Init();
			this.BottomButtons.OnSwitch = new Action<CrossArenaRewardsKind>(this.OnSwitchRewardsTab);
			this.DanButtons.Init();
			this.DanButtons.OnSwitchDan = new Action<int>(this.OnSwitchDan);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			CrossArenaMyArenaInfo myRank = this.MyRank;
			if (myRank == null)
			{
				return;
			}
			myRank.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnDeInit()
		{
			CrossArenaRewardsScroll scroll = this.Scroll;
			if (scroll != null)
			{
				scroll.DeInit();
			}
			CrossArenaMyArenaInfo myRank = this.MyRank;
			if (myRank != null)
			{
				myRank.DeInit();
			}
			CrossArenaRewardsBottomTabButtons bottomButtons = this.BottomButtons;
			if (bottomButtons != null)
			{
				bottomButtons.DeInit();
			}
			CrossArenaRewardsDanTabButtons danButtons = this.DanButtons;
			if (danButtons == null)
			{
				return;
			}
			danButtons.DeInit();
		}

		public void SetView(CrossArenaRewardsViewModule view)
		{
			this.mView = view;
		}

		public void RefreshOnOpen()
		{
			this.BuildAllData();
			this.DanButtons.SetData(this.mDataList);
			this.DanButtons.RefreshUI();
			int num = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule).Dan;
			if (num < 1)
			{
				num = 1;
			}
			this.DanButtons.SwitchDan(num);
			this.BottomButtons.SwitchTab(CrossArenaRewardsKind.Daily);
		}

		private void OnSwitchDan(int dan)
		{
			this.mCurDan = dan;
			this.RefreshAllUI();
		}

		private void OnSwitchRewardsTab(CrossArenaRewardsKind kind)
		{
			this.mCurRewardsKind = kind;
			this.RefreshAllUI();
		}

		public void RefreshAllUI()
		{
			if (this.mCurDan == 0 || this.mCurRewardsKind == (CrossArenaRewardsKind)0)
			{
				return;
			}
			this.mCurRewards = null;
			for (int i = 0; i < this.mDataList.Count; i++)
			{
				if (this.mDataList[i].Dan == this.mCurDan)
				{
					this.mCurRewards = this.mDataList[i];
					break;
				}
			}
			if (this.mCurRewards == null)
			{
				return;
			}
			CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.Text_MyRank.text = Singleton<LanguageManager>.Instance.GetInfoByID("15013", new object[] { dataModule.Rank.ToString() });
			string text = "";
			CrossArenaRewardsKind crossArenaRewardsKind = this.mCurRewardsKind;
			if (crossArenaRewardsKind != CrossArenaRewardsKind.Daily)
			{
				if (crossArenaRewardsKind == CrossArenaRewardsKind.Season)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("15011") + "\r\n" + Singleton<LanguageManager>.Instance.GetInfoByID("15012");
				}
			}
			else
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID("15010") + "\r\n" + Singleton<LanguageManager>.Instance.GetInfoByID("15012");
			}
			this.Text_BottomTips.text = text;
			this.Scroll.SetData(this.mCurRewards, this.mCurRewardsKind);
			CrossArenaRankRewards crossArenaRankRewards = this.CetMyCurRewards();
			this.MyRank.SetData(crossArenaRankRewards);
			this.MyRank.SwitchShowKind(this.mCurRewardsKind);
		}

		private void BuildAllData()
		{
			if (this.mDataList.Count > 0)
			{
				return;
			}
			IList<CrossArena_CrossArenaLevel> allElements = GameApp.Table.GetManager().GetCrossArena_CrossArenaLevelModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				CrossArena_CrossArenaLevel crossArena_CrossArenaLevel = allElements[i];
				this.mDataList.Add(CrossArenaRewards.Create(crossArena_CrossArenaLevel.Level));
			}
		}

		private CrossArenaRankRewards CetMyCurRewards()
		{
			if (this.mCurRewards == null)
			{
				return null;
			}
			List<CrossArenaRankRewards> rankRewards = this.mCurRewards.RankRewards;
			int rank = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule).Rank;
			CrossArenaRankRewards crossArenaRankRewards = null;
			for (int i = 0; i < rankRewards.Count; i++)
			{
				CrossArenaRankRewards crossArenaRankRewards2 = rankRewards[i];
				if (crossArenaRankRewards2.RankStart <= rank && crossArenaRankRewards2.RankEnd >= rank)
				{
					crossArenaRankRewards = crossArenaRankRewards2;
					break;
				}
			}
			if (crossArenaRankRewards == null)
			{
				crossArenaRankRewards = rankRewards[rankRewards.Count - 1];
			}
			return crossArenaRankRewards;
		}

		private void OnCloseThis()
		{
			if (this.mView != null)
			{
				GameApp.View.CloseView(this.mView.GetName(), null);
			}
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnCloseThis();
		}

		public UIPopCommon PopCommon;

		public CustomText Text_MyRank;

		public CustomText Text_BottomTips;

		public CrossArenaMyArenaInfo MyRank;

		public CrossArenaRewardsScroll Scroll;

		public CrossArenaRewardsBottomTabButtons BottomButtons;

		public CrossArenaRewardsDanTabButtons DanButtons;

		private CrossArenaRewardsViewModule mView;

		private int mCurDan;

		private CrossArenaRewardsKind mCurRewardsKind;

		private List<CrossArenaRewards> mDataList = new List<CrossArenaRewards>();

		private CrossArenaRewards mCurRewards;
	}
}
