using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;

namespace HotFix
{
	public class ShopActivitySUpPoolPreviewViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.txtScrollView.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("suppool_preview_desc"), true);
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this._shopActivityData = dataModule.GetShopSUpActivityData();
			if (this._shopActivityData != null)
			{
				int hours = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
				DateTime dateTime = DxxTools.Time.UnixTimestampToServerLocalDateTime((double)this._shopActivityData.startTimestamp);
				DateTime dateTime2 = DxxTools.Time.UnixTimestampToServerLocalDateTime((double)this._shopActivityData.endTimestamp);
				int num = hours - DxxTools.Time.Timezone;
				dateTime = dateTime.AddHours((double)num);
				dateTime2 = dateTime2.AddHours((double)num);
				this.txtTime.text = dateTime.ToString("yyyy/MM/dd HH:mm") + "-" + dateTime2.ToString("yyyy/MM/dd HH:mm");
				return;
			}
			this.txtTime.text = "";
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this._shopActivityData == null)
			{
				GameApp.View.CloseView(ViewName.ShopActivitySUpPoolPreviewViewModule, null);
				return;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			if (this._shopActivityData.endTimestamp <= serverTimestamp)
			{
				GameApp.View.CloseView(ViewName.ShopActivitySUpPoolPreviewViewModule, null);
				return;
			}
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.m_uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.btnGo.m_onClick = new Action(this.OnBtnGoClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.m_uiPopCommon.OnClick = null;
			this.btnGo.m_onClick = null;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnBtnCloseClick();
			}
		}

		private void OnBtnGoClick()
		{
			Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainShop_EquipShop, null);
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.ShopActivitySUpPoolPreviewViewModule, null);
		}

		public UIPopCommon m_uiPopCommon;

		public CustomTextScrollView txtScrollView;

		public CustomText txtTime;

		public CustomButton btnGo;

		private IAPShopActivityData _shopActivityData;
	}
}
