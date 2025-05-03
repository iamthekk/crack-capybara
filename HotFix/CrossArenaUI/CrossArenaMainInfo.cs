using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix.CrossArenaUI
{
	public class CrossArenaMainInfo : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.moduleCurrencyUI.Init();
			this.moduleCurrencyUI.SetStyle(EModuleId.CrossArena, new List<int> { 10 });
			this.RefreshUI();
			this.OnUpdate(0f, 0f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.moduleCurrencyUI.OnUpdate(deltaTime, unscaledDeltaTime);
			this.timer += unscaledDeltaTime;
			if (this.timer > 1f)
			{
				this.timer -= (float)((int)this.timer);
				this.RefreshTime();
			}
		}

		protected override void OnDeInit()
		{
			this.moduleCurrencyUI.DeInit();
		}

		public void RefreshUI()
		{
			CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.BuildProgress();
			this.Text_Dan.text = CrossArenaDataModule.GetCrossArenaDanName(dataModule.Dan);
			this.RefreshTime();
		}

		private void BuildProgress()
		{
			this.mProgress = new CrossArenaProgress();
			this.mProgress.BuildProgress();
		}

		private void RefreshTime()
		{
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = this.mProgress.DailyCloseTime;
			long num2 = this.mProgress.DailyOpenTime;
			if (serverTimestamp > this.mProgress.DailyCloseTime)
			{
				this.BuildProgress();
				num = this.mProgress.DailyCloseTime;
				num2 = this.mProgress.DailyOpenTime;
			}
			long num3;
			if (serverTimestamp < num2)
			{
				num3 = Utility.Math.Max(num2 - serverTimestamp, 0L);
				this.Text_Time.text = Singleton<LanguageManager>.Instance.GetInfoByID("arena_countdown_daily_open", new object[] { DxxTools.FormatTime(num3) });
				return;
			}
			num3 = Utility.Math.Max(num - serverTimestamp, 0L);
			this.Text_Time.text = Singleton<LanguageManager>.Instance.GetInfoByID("arena_countdown_daily_close", new object[] { DxxTools.FormatTime(num3) });
		}

		private void OnClickAddTicket()
		{
			CommonTicketBuyTipModule.OpenData openData = default(CommonTicketBuyTipModule.OpenData);
			openData.SetData(UserTicketKind.CrossArena);
			GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData, 1, null, null);
		}

		private void OnClickOpenTips()
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData();
			openData.m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID("15037");
			openData.m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID("15038");
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		public ModuleCurrencyCtrl moduleCurrencyUI;

		public CustomText Text_Time;

		public CustomText Text_Dan;

		private CrossArenaProgress mProgress;

		private float timer;
	}
}
