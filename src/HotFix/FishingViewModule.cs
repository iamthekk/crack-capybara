using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine.Events;

namespace HotFix
{
	public class FishingViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.fishingGameCtrl.Init();
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnClickBack));
		}

		public override async void OnOpen(object data)
		{
			if (data != null)
			{
				FishingViewModule.OpenData openData = data as FishingViewModule.OpenData;
				if (openData != null)
				{
					GameEventFishingFactory fishingFactory = Singleton<GameEventController>.Instance.GetFishingFactory();
					Fishing_fishing baseConfig = fishingFactory.baseConfig;
					this.fishingGameCtrl.SetData(baseConfig, fishingFactory.GetBestRod(), openData.seed);
					await this.LoadAreaSprite(baseConfig.area);
				}
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.fishingGameCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.fishingGameCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIFishingResult_Close, new HandlerEvent(this.OnEventCloseResult));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIFishingResult_Close, new HandlerEvent(this.OnEventCloseResult));
		}

		private void OnEventCloseResult(object sender, int type, BaseEventArgs eventargs)
		{
			this.fishingGameCtrl.ResetState();
			if (Singleton<GameEventController>.Instance.GetFishingFactory().baitNum <= 0)
			{
				DxxTools.UI.OpenPopCommonNoCancle(Singleton<LanguageManager>.Instance.GetInfoByID("UIFishing_NoBait"), delegate(int result)
				{
					if (result == 1)
					{
						GameApp.View.CloseView(ViewName.FishingViewModule, null);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIFishing_Close, null);
					}
				});
			}
		}

		private void OnClickBack()
		{
			DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("UIFishing_BackTip"), delegate(int result)
			{
				if (result == 1)
				{
					GameApp.View.CloseView(ViewName.FishingViewModule, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIFishing_Close, null);
				}
			});
		}

		private Task LoadAreaSprite(int area)
		{
			FishingViewModule.<LoadAreaSprite>d__13 <LoadAreaSprite>d__;
			<LoadAreaSprite>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadAreaSprite>d__.<>4__this = this;
			<LoadAreaSprite>d__.area = area;
			<LoadAreaSprite>d__.<>1__state = -1;
			<LoadAreaSprite>d__.<>t__builder.Start<FishingViewModule.<LoadAreaSprite>d__13>(ref <LoadAreaSprite>d__);
			return <LoadAreaSprite>d__.<>t__builder.Task;
		}

		public CustomImage imageFishingBg;

		public FishingGameCtrl fishingGameCtrl;

		public CustomButton buttonBack;

		public class OpenData
		{
			public int seed;
		}
	}
}
