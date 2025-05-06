using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class PetBoxFree : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.isInit = true;
			if (this.m_redNode != null)
			{
				this.m_redNode.gameObject.SetActive(false);
			}
			this.m_btnItem.onClick.AddListener(new UnityAction(this.OnBtnItemClick));
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			this.btnItemClickCallback = null;
			this.m_btnItem.onClick.RemoveListener(new UnityAction(this.OnBtnItemClick));
		}

		private void Update()
		{
			if (!this.isInit)
			{
				return;
			}
			this.tempTime += Time.deltaTime;
			if (this.tempTime >= PetBoxFree.RefreshTime)
			{
				this.RefreshCdTime();
			}
		}

		public void Refresh(PetDataModule petDataModule, EPetBoxType petBoxType)
		{
			this.petDataModule = petDataModule;
			this.petBoxType = petBoxType;
			AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.adData = dataModule.GetAdData(8);
			base.gameObject.SetActiveSafe(dataModule.CheckCloudDataAdOpen());
			this.m_txtDrawTimes.text = string.Format("x{0}", this.petDataModule.AdvertisementResultCount);
			this.m_txtCost.text = this.adData.GetRemainTimes().ToString() + "/" + this.adData.watchCountMax.ToString();
			this.m_redNode.gameObject.SetActive(this.adData.GetRemainTimes() > 0);
			this.RefreshCdTime();
		}

		public void SetCallback(Action<PetBoxFree> cb)
		{
			this.btnItemClickCallback = cb;
		}

		private void OnBtnItemClick()
		{
			Action<PetBoxFree> action = this.btnItemClickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		private void RefreshCdTime()
		{
		}

		public CustomButton m_btnItem;

		public CustomText m_txtDrawTimes;

		public CustomImage m_imgCostIcon;

		public CustomText m_txtCost;

		public CustomText m_txtTimer;

		public RedNodeOneCtrl m_redNode;

		public EPetBoxType petBoxType;

		[NonSerialized]
		public AdData adData;

		private PetDataModule petDataModule;

		private Action<PetBoxFree> btnItemClickCallback;

		private bool isInit;

		private static float RefreshTime = 0.5f;

		private float tempTime;
	}
}
