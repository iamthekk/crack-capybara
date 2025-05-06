using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class RogueDungeonRankViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.rankPanel.SetContentVisible(false);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnCloseSelf));
		}

		public override void OnOpen(object data)
		{
			RankOpenData rankOpenData = new RankOpenData();
			rankOpenData.RankType = RankType.RogueDungeon;
			this.rankPanel.Init();
			this.rankPanel.Open(RankViewType.Order, rankOpenData, new Action<bool>(this.OnOpedRankPanel));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.rankPanel.DeInit();
		}

		public override void OnDelete()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
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
			GameApp.View.CloseView(ViewName.RogueDungeonRankViewModule, null);
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.RogueDungeonRankViewModule, null);
		}

		public CustomButton buttonClose;

		public MainRankPanel rankPanel;
	}
}
