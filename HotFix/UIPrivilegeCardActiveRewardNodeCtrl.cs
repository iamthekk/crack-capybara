using System;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Shop.Arena;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIPrivilegeCardActiveRewardNodeCtrl : UIPrivilegeCardNodeCtrlBase
	{
		protected override void OnInit()
		{
			this.buttonGet.onClick.AddListener(new UnityAction(this.OnClickGet));
		}

		protected override void OnDeInit()
		{
			this.buttonGet.onClick.AddListener(new UnityAction(this.OnClickGet));
		}

		public override void Refresh()
		{
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(GameConfig.Privilege_Card_Frame_Item);
			if (item_Item != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(item_Item.atlasID);
				this.imageFrame.SetImage(atlasPath, item_Item.icon);
			}
			int headActiveState = GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.HeadActiveState;
			this.redNode.SetActiveSafe(false);
			if (headActiveState == 2)
			{
				this.textActive.text = Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_active");
				this.textGet.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_collected");
				this.btnGray.SetUIGray();
			}
			else
			{
				this.textActive.text = Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_inactive");
				this.textGet.text = Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_inactive");
				this.btnGray.SetUIGray();
				if (headActiveState == 1)
				{
					this.btnGray.Recovery();
					this.textGet.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_collect");
					this.redNode.SetActiveSafe(true);
				}
			}
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_freecard_active");
		}

		private void OnClickGet()
		{
			NetworkUtils.Shop.DoHeadFrameActiveRequest(delegate(bool result, HeadFrameActiveResponse response)
			{
				if (result)
				{
					this.Refresh();
					if (response != null && response.CommonData.Reward != null && response.CommonData.Reward.Count > 0)
					{
						DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, null, true);
					}
				}
			});
		}

		public CustomImage imageFrame;

		public CustomText textActive;

		public CustomText textInfo;

		public CustomButton buttonGet;

		public CustomText textGet;

		public UIGrays btnGray;

		public GameObject redNode;
	}
}
