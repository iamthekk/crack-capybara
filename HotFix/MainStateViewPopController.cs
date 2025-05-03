using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class MainStateViewPopController
	{
		public void Init()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ViewPopCheck_ReCheck, new HandlerEvent(this.OnReCheck));
		}

		public void DeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ViewPopCheck_ReCheck, new HandlerEvent(this.OnReCheck));
			this.ClearCheckerList();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			MainState state = GameApp.State.GetState(StateName.MainState);
			if (state != null && !state.CheckPopEnabled)
			{
				return;
			}
			while (this.mCheckerList.Count > 0)
			{
				BaseViewPopChecker baseViewPopChecker = this.mCheckerList[0];
				if (baseViewPopChecker == null)
				{
					this.mCheckerList.RemoveAt(0);
				}
				else
				{
					if (baseViewPopChecker.Check())
					{
						break;
					}
					this.mCheckerList.RemoveAt(0);
				}
			}
		}

		public void OnLoadFinished()
		{
			this.ClearCheckerList();
			this.mCheckerList.Add(new FunctionOpenChecker(this));
			this.mCheckerList.Add(new ShopActivityPreviewChecker());
		}

		public void ClearCheckerList()
		{
			for (int i = 0; i < this.mCheckerList.Count; i++)
			{
				this.mCheckerList[i] = null;
			}
		}

		private void OnReCheck(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnLoadFinished();
		}

		private readonly List<BaseViewPopChecker> mCheckerList = new List<BaseViewPopChecker>();
	}
}
