using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIMainButton_ChapterWheel : BaseUIMainButton
	{
		protected override void OnInit()
		{
			this.actModule = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
			this.wheelEnterCtrl.Init();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivityWheel_RefreshUI, new HandlerEvent(this.OnEventRefresh));
			RedPointController.Instance.RegRecordChange("Main.ChapterWheel", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			this.wheelEnterCtrl.DeInit();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivityWheel_RefreshUI, new HandlerEvent(this.OnEventRefresh));
			RedPointController.Instance.UnRegRecordChange("Main.ChapterWheel", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.wheelEnterCtrl != null)
			{
				this.wheelEnterCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override bool IsShow()
		{
			return this.actModule.IsActivityOpen();
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 2;
			subPriority = 0;
		}

		public override void OnRefresh()
		{
			base.gameObject.SetActiveSafe(this.actModule.IsActivityOpen());
			this.wheelEnterCtrl.SetData(new Action(this.OnClickChapterWheel), false);
		}

		public override void OnLanguageChange()
		{
		}

		public override void OnRefreshAnimation()
		{
		}

		private void OnClickChapterWheel()
		{
			if (this.actModule != null && this.actModule.IsActivityOpen())
			{
				GameApp.View.OpenView(ViewName.ChapterActivityWheelViewModule, null, 1, null, null);
			}
		}

		private void OnRedPointChange(RedNodeListenData redData)
		{
			if (this.redNode == null)
			{
				return;
			}
			this.redNode.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnEventRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefresh();
		}

		public UIChapterWheelEnterCtrl wheelEnterCtrl;

		public RedNodeOneCtrl redNode;

		private ChapterActivityWheelDataModule actModule;
	}
}
