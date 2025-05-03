using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.CrossArenaUI
{
	public class CrossArenaMainMyRank : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.Button_Rewards.onClick.AddListener(new UnityAction(this.OnOpenRewardsView));
			this.Button_Record.onClick.AddListener(new UnityAction(this.OnOpenRecordView));
			this.Text_Tips.text = "";
			this.MyRank.Init();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			CustomButton button_Rewards = this.Button_Rewards;
			if (button_Rewards != null)
			{
				button_Rewards.onClick.RemoveListener(new UnityAction(this.OnOpenRewardsView));
			}
			CustomButton button_Record = this.Button_Record;
			if (button_Record != null)
			{
				button_Record.onClick.RemoveListener(new UnityAction(this.OnOpenRecordView));
			}
			CrossArenaRankMemberItem myRank = this.MyRank;
			if (myRank == null)
			{
				return;
			}
			myRank.DeInit();
		}

		private void OnOpenRecordView()
		{
			GameApp.View.OpenView(ViewName.CrossArenaRecordViewModule, null, 1, null, null);
		}

		private void OnOpenRewardsView()
		{
			GameApp.View.OpenView(ViewName.CrossArenaRewardsViewModule, null, 1, null, null);
		}

		public void SetHide()
		{
			this.IsShow = false;
			this.AnimatorMyRank.Play("hide");
		}

		public void PlayShow()
		{
			this.RefreshMyRankShow();
			if (this.IsShow)
			{
				return;
			}
			this.IsShow = true;
			this.AnimatorMyRank.Play("show");
		}

		public void RefreshMyRankShow()
		{
			this.RefreshMyRank();
			this.RefreshDanChangeTips();
		}

		public void RefreshMyRank()
		{
			this.MyRank.SetData(this.mDataModule.Rank - 1, this.mDataModule.MyRankInfo);
			this.MyRank.RefreshUI();
			this.MyRank.RefreshUIAsMine();
		}

		public void RefreshDanChangeTips()
		{
			CrossArenaDataModule crossArenaDataModule = this.mDataModule;
			if (crossArenaDataModule.Dan == 0)
			{
				return;
			}
			CrossArena_CrossArenaLevel elementById = GameApp.Table.GetManager().GetCrossArena_CrossArenaLevelModelInstance().GetElementById(crossArenaDataModule.Dan);
			float num = Mathf.Clamp01((float)elementById.upPro / 100f);
			float num2 = Mathf.Clamp01((100f - (float)elementById.downPro) / 100f);
			int num3 = Mathf.CeilToInt(num * (float)crossArenaDataModule.TotalMemberCount);
			int num4 = Mathf.CeilToInt(num2 * (float)crossArenaDataModule.TotalMemberCount);
			if (crossArenaDataModule.Rank <= num3)
			{
				string crossArenaDanName = CrossArenaDataModule.GetCrossArenaDanName(crossArenaDataModule.Dan + 1);
				this.Text_Tips.text = Singleton<LanguageManager>.Instance.GetInfoByID("15034", new object[] { crossArenaDanName });
				return;
			}
			if (crossArenaDataModule.Rank <= num4)
			{
				string crossArenaDanName2 = CrossArenaDataModule.GetCrossArenaDanName(crossArenaDataModule.Dan);
				this.Text_Tips.text = Singleton<LanguageManager>.Instance.GetInfoByID("15035", new object[] { crossArenaDanName2 });
				return;
			}
			string crossArenaDanName3 = CrossArenaDataModule.GetCrossArenaDanName(crossArenaDataModule.Dan - 1);
			this.Text_Tips.text = Singleton<LanguageManager>.Instance.GetInfoByID("15036", new object[] { crossArenaDanName3 });
		}

		public CustomButton Button_Rewards;

		public CustomButton Button_Record;

		public CustomText Text_Tips;

		public CrossArenaRankMemberItem MyRank;

		public Animator AnimatorMyRank;

		public bool IsShow;

		private CrossArenaDataModule mDataModule;
	}
}
