using System;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class UIMainButton_ChapterRank : BaseUIMainButton
	{
		protected override void OnInit()
		{
			this.actModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterRank_Show, new HandlerEvent(this.OnEventShowRank));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterRank_Hide, new HandlerEvent(this.OnEventHideRank));
			this.actEnterCtrl.Init();
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterRank_Show, new HandlerEvent(this.OnEventShowRank));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterRank_Hide, new HandlerEvent(this.OnEventHideRank));
			this.actEnterCtrl.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.actEnterCtrl != null)
			{
				this.actEnterCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override bool IsShow()
		{
			return this.actModule.GetActiveActivityData(ChapterActivityKind.Rank) != null;
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 1;
			subPriority = 0;
		}

		public override void OnRefresh()
		{
			ChapterActivityData activeActivityData = this.actModule.GetActiveActivityData(ChapterActivityKind.Rank);
			if (activeActivityData != null)
			{
				base.gameObject.SetActiveSafe(true);
				this.actEnterCtrl.Show();
				this.actEnterCtrl.SetData(activeActivityData, new Action(this.OnClickChapterRank), false);
			}
		}

		public override void OnLanguageChange()
		{
		}

		public override void OnRefreshAnimation()
		{
		}

		private void OnEventShowRank(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefresh();
		}

		private void OnEventHideRank(object sender, int type, BaseEventArgs eventArgs)
		{
			this.actEnterCtrl.Hide();
			base.gameObject.SetActiveSafe(false);
		}

		private void OnClickChapterRank()
		{
			GameApp.View.OpenView(ViewName.ChapterActivityRankViewModule, null, 1, null, null);
		}

		public UIChapterRankEnterCtrl actEnterCtrl;

		private ChapterActivityDataModule actModule;
	}
}
