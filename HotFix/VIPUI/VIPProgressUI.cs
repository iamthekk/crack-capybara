using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.VIPUI
{
	public class VIPProgressUI : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mVIPDataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			this.ButtonRecharge.m_onClick = new Action(this.OnGotoRecharge);
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshUI()
		{
			int vipLevel = this.mVIPDataModule.VipLevel;
			this.TextVIPLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("8804", new object[] { vipLevel });
			VIPDataModule.VIPDatas vipdatas = this.mVIPDataModule.GetVIPDatas(vipLevel);
			int num = ((vipdatas != null) ? vipdatas.m_exp : 0);
			VIPDataModule.VIPDatas vipdatas2 = this.mVIPDataModule.GetVIPDatas(vipLevel + 1);
			int num2 = ((vipdatas2 != null) ? vipdatas2.m_exp : num);
			if (vipLevel == this.mVIPDataModule.MaxVIPLevel)
			{
				VIPDataModule.VIPDatas vipdatas3 = this.mVIPDataModule.GetVIPDatas(vipLevel - 1);
				num = ((vipdatas3 != null) ? vipdatas3.m_exp : 0);
				num2 = vipdatas.m_exp;
			}
			int vipExp = this.mVIPDataModule.VipExp;
			int num3 = num2 - vipExp;
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (vipExp >= num2)
			{
				this.TextExp.text = Singleton<LanguageManager>.Instance.GetInfoByID("8812");
			}
			else
			{
				this.TextExp.text = string.Format("{0}/{1}", vipExp, num2);
			}
			this.SliderExp.minValue = (float)num;
			this.SliderExp.maxValue = (float)num2;
			this.SliderExp.value = (float)vipExp;
			if (vipdatas2 != null && num3 > 0)
			{
				string text = string.Format("<color={0}>{1}</color>", "#EBCF1D", this.GetNeedRecharge(num3));
				string text2 = "<color=#EBCF1D>" + Singleton<LanguageManager>.Instance.GetInfoByID("8804", new object[] { vipLevel + 1 }) + "</color>";
				this.TextNeedToNextLevel.gameObject.SetActive(true);
				this.TextNeedToNextLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("8806", new object[] { text, text2 });
				return;
			}
			this.TextNeedToNextLevel.gameObject.SetActive(false);
		}

		private int GetNeedRecharge(int exp)
		{
			return (int)Mathf.Ceil((float)exp / 10f);
		}

		private void OnGotoRecharge()
		{
			GameApp.View.CloseView(ViewName.VIPViewModule, null);
			IAPShopViewModule.OpenData openData = new IAPShopViewModule.OpenData(IAPShopJumpTabData.CreateDiamonds(IAPDiamondsType.DiamondsPack));
			GameApp.View.OpenView(ViewName.IAPShopViewModule, openData, 1, null, null);
		}

		public CustomText TextVIPLevel;

		public CustomText TextNeedToNextLevel;

		public Slider SliderExp;

		public CustomText TextExp;

		public CustomButton ButtonRecharge;

		private VIPDataModule mVIPDataModule;

		private const string COLOR_YELLOW = "#EBCF1D";
	}
}
