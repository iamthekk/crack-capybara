using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix.VIPUI
{
	public class VIPNodeUI : CustomBehaviour
	{
		public VIPDataModule.VIPDatas Data
		{
			get
			{
				return this.mCurShowVIPData;
			}
		}

		protected override void OnInit()
		{
			this.BenefitsUI.Init();
			this.RewardsUI.Init();
		}

		protected override void OnDeInit()
		{
			VIPBenefitsUI benefitsUI = this.BenefitsUI;
			if (benefitsUI != null)
			{
				benefitsUI.DeInit();
			}
			VIPRewardsUI rewardsUI = this.RewardsUI;
			if (rewardsUI == null)
			{
				return;
			}
			rewardsUI.DeInit();
		}

		public void SetData(VIPDataModule.VIPDatas data)
		{
			this.mCurShowVIPData = data;
		}

		public void RefreshUI()
		{
			this.InternalRefreshUI();
		}

		private void InternalRefreshUI()
		{
			if (this.mCurShowVIPData == null)
			{
				return;
			}
			this.TextTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("8801", new object[] { this.mCurShowVIPData.m_id });
			this.BenefitsUI.SetData(this.mCurShowVIPData);
			this.BenefitsUI.RefreshUI();
			this.RewardsUI.SetData(this.mCurShowVIPData);
			this.RewardsUI.RefreshUI();
		}

		public CustomText TextTitle;

		public VIPBenefitsUI BenefitsUI;

		public VIPRewardsUI RewardsUI;

		private VIPDataModule.VIPDatas mCurShowVIPData;
	}
}
