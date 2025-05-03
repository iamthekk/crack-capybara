using System;
using System.Text;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class WorldBossRankPanel : BaseViewModule
	{
		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnCreate(object data)
		{
			if (this.script_LayoutTest != null)
			{
				this.script_LayoutTest.enabled = false;
			}
			this.rankPanel.SetContentVisible(false);
			this.closeBtn.onClick.AddListener(new UnityAction(this.OnClickClose));
			if (this.infoBtn != null)
			{
				this.infoBtn.onClick.AddListener(new UnityAction(this.OnClickInfo));
			}
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.btn_RankOrder.onClick.AddListener(new UnityAction(this.OnClickRankOrder));
			this.btn_RankReward.onClick.AddListener(new UnityAction(this.OnClickRankReward));
		}

		public override void OnDelete()
		{
			if (this.infoBtn != null)
			{
				this.infoBtn.onClick.RemoveAllListeners();
			}
			this.buttonMask.onClick.RemoveAllListeners();
			this.btn_RankOrder.onClick.RemoveAllListeners();
			this.btn_RankReward.onClick.RemoveAllListeners();
		}

		public override void OnOpen(object data)
		{
			this.RefreshTitleInfo();
			this.rankPanel.Init();
			this.OpenRankPanel(RankViewType.Order, true);
		}

		private void OnOpedRankPanel(bool success)
		{
			if (success)
			{
				this.rankPanel.SetContentVisible(true);
				return;
			}
			GameApp.View.CloseView(ViewName.WorldBossRankViewModule, null);
		}

		private void RefreshTitleInfo()
		{
			WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			int rankLevel = dataModule.RankLevel;
			if (dataModule.Id >= 1 && rankLevel > 0)
			{
				this.backText.gameObject.SetActiveSafe(true);
				WorldBoss_Subsection worldBoss_Subsection = GameApp.Table.GetManager().GetWorldBoss_Subsection(rankLevel);
				int promotionRank = worldBoss_Subsection.PromotionRank;
				this.backText.text = ((promotionRank > 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_PromotionWillUp", new object[] { worldBoss_Subsection.PromotionRank }) : Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_PromotionWillDown", new object[] { worldBoss_Subsection.DemoteRank }));
				return;
			}
			this.backText.gameObject.SetActiveSafe(false);
		}

		private void OnClickInfo()
		{
			int num = GameApp.Data.GetDataModule(DataName.WorldBossDataModule).RankLevel;
			if (num <= 0)
			{
				num = 1;
			}
			StringBuilder stringBuilder = new StringBuilder();
			WorldBoss_Subsection worldBoss_Subsection = GameApp.Table.GetManager().GetWorldBoss_Subsection(num);
			stringBuilder.AppendLine(Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_Desc1"));
			if (worldBoss_Subsection.PromotionRank > 0)
			{
				stringBuilder.AppendLine(Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_Desc3", new object[] { worldBoss_Subsection.PromotionRank }));
			}
			if (worldBoss_Subsection.DemoteRank < 99999)
			{
				stringBuilder.AppendLine(Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_Desc4", new object[] { worldBoss_Subsection.DemoteRank }));
			}
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
			{
				m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_InfoTitle"),
				m_contextInfo = stringBuilder.ToString()
			};
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.WorldBossRankViewModule, null);
		}

		private void OnClickRankOrder()
		{
			this.OpenRankPanel(RankViewType.Order, false);
		}

		private void OnClickRankReward()
		{
			this.OpenRankPanel(RankViewType.Reward, false);
		}

		private void OpenRankPanel(RankViewType viewType, bool force = false)
		{
			if (this.m_RankViewType == viewType && !force)
			{
				return;
			}
			this.m_RankViewType = viewType;
			this.btn_RankOrder.SetSelect(viewType == RankViewType.Order);
			this.btn_RankReward.SetSelect(viewType == RankViewType.Reward);
			RankOpenData rankOpenData = new RankOpenData();
			rankOpenData.RankType = RankType.WorldBoss;
			this.rankPanel.Open(viewType, rankOpenData, new Action<bool>(this.OnOpedRankPanel));
		}

		public override void OnClose()
		{
			this.rankPanel.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public MonoBehaviour script_LayoutTest;

		public CustomButton buttonMask;

		public Button closeBtn;

		public CustomText backText;

		public CustomButton infoBtn;

		public MainRankPanel rankPanel;

		public CustomChooseButton btn_RankOrder;

		public CustomChooseButton btn_RankReward;

		private RankViewType m_RankViewType;
	}
}
