using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine.Events;

namespace HotFix
{
	public class MiningBonusRateViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.showRateCtrl.Init();
			this.getRateCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			IList<Mining_showRate> allElements = GameApp.Table.GetManager().GetMining_showRateModelInstance().GetAllElements();
			this.showRateCtrl.SetData(allElements.ToList<Mining_showRate>(), 0);
			this.getRateCtrl.SetData(allElements.ToList<Mining_showRate>(), 1);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.showRateCtrl.DeInit();
			this.getRateCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.MiningBonusRateViewModule, null);
		}

		public UIBonusRateCtrl showRateCtrl;

		public UIBonusRateCtrl getRateCtrl;

		public CustomButton buttonClose;

		public CustomButton buttonMask;

		public const int Mode_Show = 0;

		public const int Mode_Get = 1;
	}
}
