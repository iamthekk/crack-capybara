using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.SignIn;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class SignItemCrtl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.itemDefault.gameObject.SetActive(false);
			this.rewardBtn.m_onClick = new Action(this.OnClickSignInBtn);
			this.showItem = new ShowRewardItem();
			this.showItem.m_grid = this.gridContent;
			this.showItem.m_item = this.itemDefault;
			this.showItem.m_scrollRect = this.scrollRect;
			this.showItem.m_pool = LocalUnityObjctPool.Create(base.gameObject);
			this.showItem.Init();
			this.signDataModule = GameApp.Data.GetDataModule(DataName.SignDataModule);
		}

		protected override void OnDeInit()
		{
			if (this.showItem != null)
			{
				this.showItem.deinitItems();
			}
		}

		public void SetSigninData(SignInItemInfo signInItemInfo, SignInAwardType signInAwardType)
		{
			this.signInItemInfo = signInItemInfo;
			this.showItem.SetShowItemList(signInItemInfo.rewardDtoListDto.RewardDtos, new Action<UIItem, PropData, object>(this.OnClickRewardItem), null, 0.05f);
			this.showItem.SetScrollEnable(signInItemInfo.rewardDtoListDto.RewardDtos.Count > 5);
			bool flag = (signInItemInfo.isCanSignIn || (this.signDataModule.SignInIndex == 0U && signInItemInfo.SignInIndex == 1)) && !signInItemInfo.isSigin;
			this.imageBg.SetActive(!flag);
			this.imageCanClaimBg.SetActive(flag);
			this.imageBGMask.SetActive(signInItemInfo.isSigin);
			this.titleText.text = (flag ? string.Empty : Singleton<LanguageManager>.Instance.GetInfoByID("2302", new object[] { signInItemInfo.SignInIndex }));
			this.titleCanClaimText.text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("2301") : string.Empty);
		}

		private void OnClickSignInBtn()
		{
			if (this.signInItemInfo.isSigin)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("2304"));
				return;
			}
			if (!this.signInItemInfo.isCanSignIn)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("2307"));
				return;
			}
			this.SendMsg();
		}

		private void OnClickRewardItem(UIItem item, PropData propData, object param)
		{
			if (this.signInItemInfo.isSigin)
			{
				return;
			}
			if (!this.signInItemInfo.isCanSignIn)
			{
				if (item != null)
				{
					item.OnBtnItemClick(item, propData, param);
				}
				return;
			}
			this.SendMsg();
		}

		private void SendMsg()
		{
			NetworkUtils.SiGnIn.SignInDoSignRequest(delegate(bool result, SignInDoSignResponse resp)
			{
				if (result)
				{
					EventSignDataList instance = Singleton<EventSignDataList>.Instance;
					instance.SetData(resp.SignInData);
					GameApp.Event.DispatchNow(this, 220, instance);
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					RedPointController.Instance.ReCalc("Main.Sign", true);
				}
			});
		}

		public CustomText titleText;

		public CustomText titleCanClaimText;

		public GameObject imageBg;

		public GameObject imageCanClaimBg;

		public GameObject imageBGMask;

		public CustomButton rewardBtn;

		public GameObject itemDefault;

		public ShowRewardItem showItem;

		public GridLayoutGroup gridContent;

		public CustomScrollRect scrollRect;

		private SignInItemInfo signInItemInfo;

		private SignDataModule signDataModule;
	}
}
