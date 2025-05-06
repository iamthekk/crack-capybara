using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class NewWorldRankViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.rankPanel.SetContentVisible(false);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnCloseSelf));
			this.buttonRank.onClick.AddListener(new UnityAction(this.OnClickRank));
			this.buttonReward.onClick.AddListener(new UnityAction(this.OnClickReward));
		}

		public override void OnOpen(object data)
		{
			this.rankOpenData = new RankOpenData();
			this.rankOpenData.RankType = RankType.NewWorld;
			this.rankPanel.Init();
			this.OnClickRank();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.rankPanel.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.rankPanel.DeInit();
		}

		public override void OnDelete()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.buttonRank.onClick.RemoveListener(new UnityAction(this.OnClickRank));
			this.buttonReward.onClick.RemoveListener(new UnityAction(this.OnClickReward));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnOpedRankPanel(bool success)
		{
			if (success)
			{
				this.rankPanel.SetContentVisible(true);
				return;
			}
			GameApp.View.CloseView(ViewName.NewWorldRankViewModule, null);
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.NewWorldRankViewModule, null);
		}

		private void OnClickRank()
		{
			this.buttonRank.SetSelect(true);
			this.buttonReward.SetSelect(false);
			this.rankPanel.Open(RankViewType.Order, this.rankOpenData, new Action<bool>(this.OnOpedRankPanel));
		}

		private void OnClickReward()
		{
			this.buttonRank.SetSelect(false);
			this.buttonReward.SetSelect(true);
			this.rankPanel.Open(RankViewType.Reward, this.rankOpenData, new Action<bool>(this.OnOpedRankPanel));
		}

		public CustomButton buttonClose;

		public CustomChooseButton buttonRank;

		public CustomChooseButton buttonReward;

		public MainRankPanel rankPanel;

		private RankOpenData rankOpenData;
	}
}
