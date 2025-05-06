using System;
using Framework;

namespace HotFix
{
	public class MainShopEquipSUpPoolActivityGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			this._iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.equipSUpBigRewardProgressItem.Init();
			this.equipSUpSmallRewardPreviewItem.Init();
			this.equipSUpActivityPackItem.Init();
		}

		protected override void OnDeInit()
		{
			this.equipSUpBigRewardProgressItem.DeInit();
			this.equipSUpSmallRewardPreviewItem.DeInit();
			this.equipSUpActivityPackItem.DeInit();
			this._iapDataModule = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			if (this._iapShopSUpPoolActivityData == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			if (this._iapShopSUpPoolActivityData.endTimestamp <= serverTimestamp)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.equipSUpSmallRewardPreviewItem.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 13;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			IAPShopActivityData shopSUpActivityData = this._iapDataModule.GetShopSUpActivityData();
			this._iapShopSUpPoolActivityData = shopSUpActivityData;
			if (shopSUpActivityData != null)
			{
				this._isActivityOpen = true;
				base.gameObject.SetActive(true);
				this.equipSUpBigRewardProgressItem.SetData(shopSUpActivityData);
				this.equipSUpSmallRewardPreviewItem.SetData(shopSUpActivityData);
				this.equipSUpActivityPackItem.SetData(shopSUpActivityData);
				return;
			}
			this._isActivityOpen = false;
			base.gameObject.SetActive(false);
		}

		public override int PlayAnimation(float startTime, int index)
		{
			this.titleFg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index, 10024);
			this.equipSUpBigRewardProgressItem.fg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + 1, 10024);
			this.equipSUpSmallRewardPreviewItem.fg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + 2, 10024);
			this.equipSUpActivityPackItem.fg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + 3, 10024);
			if (this._isActivityOpen)
			{
				return index + 4;
			}
			return index + 1;
		}

		public EquipSUpBigRewardProgressItem equipSUpBigRewardProgressItem;

		public EquipSUpSmallRewardPreviewItem equipSUpSmallRewardPreviewItem;

		public EquipSUpActivityPackItem equipSUpActivityPackItem;

		private IAPDataModule _iapDataModule;

		private IAPShopActivityData _iapShopSUpPoolActivityData;

		private bool _isActivityOpen;
	}
}
