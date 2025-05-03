using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainHero_HeroBottomGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_dataModule = GameApp.Data.GetDataModule(DataName.HeroLevelUpDataModule);
			this.m_levelUpBtn.onClick.AddListener(new UnityAction(this.OnClickLevelUpBtn));
			this.m_evolutionUpBtn.onClick.AddListener(new UnityAction(this.OnClickEvolutionUpBtn));
			this.m_costGroup.Init();
		}

		protected override void OnDeInit()
		{
			if (this.m_levelUpBtn != null)
			{
				this.m_levelUpBtn.onClick.RemoveListener(new UnityAction(this.OnClickLevelUpBtn));
			}
			if (this.m_evolutionUpBtn != null)
			{
				this.m_evolutionUpBtn.onClick.RemoveListener(new UnityAction(this.OnClickEvolutionUpBtn));
			}
			if (this.m_costGroup != null)
			{
				this.m_costGroup.DeInit();
			}
		}

		public void RefreshCostGroup(List<ItemData> costDatas)
		{
			if (this.m_costGroup == null)
			{
				return;
			}
			this.m_costGroup.RefreshUI(costDatas);
		}

		public void SetLevelUpGray(bool isGray)
		{
			this.m_isLevelUpGray = isGray;
			if (isGray)
			{
				this.m_levelUpGrays.SetUIGray();
				return;
			}
			this.m_levelUpGrays.Recovery();
		}

		public void SetEvolutionUpGray(bool isGray)
		{
			this.m_isEvolutionUpGray = isGray;
			if (isGray)
			{
				this.m_evolutionUpGrays.SetUIGray();
				return;
			}
			this.m_evolutionUpGrays.Recovery();
		}

		public void SetActiveForLevelUpBtn(bool active)
		{
			if (this.m_levelUpBtn == null)
			{
				return;
			}
			this.m_levelUpBtn.gameObject.SetActive(active);
			this.m_levelUpBtnActive = active;
		}

		public void SetActiveForEvolutionUpBtn(bool active)
		{
			if (this.m_evolutionUpBtn == null)
			{
				return;
			}
			this.m_evolutionUpBtn.gameObject.SetActive(active);
			this.m_evolutionUpBtnActive = active;
		}

		private void OnClickLevelUpBtn()
		{
			if (this.OnReqLevelUp == null)
			{
				return;
			}
			this.OnReqLevelUp();
		}

		private void OnClickEvolutionUpBtn()
		{
			if (this.m_isEvolutionUpGray)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("100001807"));
				return;
			}
			HeroLevelupNextEvolutionViewModule.OpenData openData = new HeroLevelupNextEvolutionViewModule.OpenData();
			openData.m_tableID = this.m_dataModule.Level;
			openData.m_cardData = this.m_dataModule.MainPlayCardData;
			GameApp.View.OpenView(ViewName.HeroLevelupNextEvolutionViewModule, openData, 1, null, null);
		}

		[SerializeField]
		private CustomButton m_levelUpBtn;

		[SerializeField]
		private UIGrays m_levelUpGrays;

		[SerializeField]
		private CustomButton m_evolutionUpBtn;

		[SerializeField]
		private UIGrays m_evolutionUpGrays;

		[SerializeField]
		private UICostCtrl m_costGroup;

		[HideInInspector]
		public bool m_isLevelUpGray;

		private bool m_isEvolutionUpGray;

		private bool m_levelUpBtnActive = true;

		private bool m_evolutionUpBtnActive = true;

		private HeroLevelUpDataModule m_dataModule;

		public Action OnReqLevelUp;
	}
}
