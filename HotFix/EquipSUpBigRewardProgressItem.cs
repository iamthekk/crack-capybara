using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class EquipSUpBigRewardProgressItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnReward.m_onClick = new Action(this.OnBtnRewardClick);
			this.btnInfo.m_onClick = new Action(this.OnBtnInfoClick);
			this.goBoxGlow.SetActive(false);
			this.redNodeBox.Value = 0;
			this.animBox.StopPlayback();
		}

		protected override void OnDeInit()
		{
			this.btnReward.m_onClick = null;
			this.btnInfo.m_onClick = null;
		}

		public void SetData(object obj)
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.curProgress = dataModule.GetEquipSUpPoolDrawCount();
			this.maxProgress = Singleton<GameConfig>.Instance.ShopEquipSUpExchangeCount;
			this.UpdateProgress(this.curProgress, this.maxProgress);
		}

		public void UpdateProgress(int curProgress, int maxProgress)
		{
			this.curProgress = curProgress;
			this.maxProgress = maxProgress;
			this.txtProgress.text = string.Format("{0}/{1}", curProgress, maxProgress);
			this.sliderProgress.value = (float)curProgress / (float)maxProgress;
			int num = curProgress / maxProgress;
			this.redNodeBox.Value = num;
			bool flag = curProgress >= maxProgress;
			if (flag != this.canReward)
			{
				this.canReward = flag;
				if (this.canReward)
				{
					this.goBoxGlow.SetActive(true);
					this.animBox.Play("Shake");
					return;
				}
				this.goBoxGlow.SetActive(false);
				this.animBox.Play("Idle");
			}
		}

		private void OnBtnRewardClick()
		{
			GameApp.View.OpenView(ViewName.ShopActivitySUpBigRewardSelectViewModule, null, 1, null, null);
		}

		private void OnBtnInfoClick()
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
			{
				m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID("suppool_help_title_1"),
				m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID("suppool_help_desc_1")
			};
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		public RectTransform fg;

		public CustomButton btnReward;

		public GameObject goBoxGlow;

		public RedNodeOneCtrl redNodeBox;

		public Animator animBox;

		public CustomText txtProgress;

		public Slider sliderProgress;

		public CustomButton btnInfo;

		private int curProgress;

		private int maxProgress = 120;

		private bool canReward;
	}
}
