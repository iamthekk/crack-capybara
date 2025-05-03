using System;
using System.Runtime.CompilerServices;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Tower;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIDailyActivitiesTowerNode : UIDailyActivitiesNodeBase
	{
		public override FunctionID FunctionOpenID
		{
			get
			{
				return FunctionID.Activity_ChallengeTower;
			}
		}

		public override UIDailyActivitiesType DailyType
		{
			get
			{
				return UIDailyActivitiesType.Tower;
			}
		}

		protected override void OnInit()
		{
			this.isInit = true;
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			this.nodeButton.onClick.AddListener(new UnityAction(this.NodeButtonClick));
			RedPointController.Instance.RegRecordChange("DailyActivity.ChallengeTag.Tower", new Action<RedNodeListenData>(base.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			this.nodeButton.onClick.RemoveListener(new UnityAction(this.NodeButtonClick));
			RedPointController.Instance.UnRegRecordChange("DailyActivity.ChallengeTag.Tower", new Action<RedNodeListenData>(base.OnRedPointChange));
		}

		protected override void OnShow()
		{
			int num = this.towerDataModule.CalculateShouldChallengeLevelID(this.towerDataModule.CompleteTowerLevelId);
			TowerChallenge_Tower towerConfigByLevelId = this.towerDataModule.GetTowerConfigByLevelId(num);
			int towerConfigNum = this.towerDataModule.GetTowerConfigNum(towerConfigByLevelId);
			int levelNumByLevelId = this.towerDataModule.GetLevelNumByLevelId(num);
			this.towerLevelCountText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_progress", new object[] { string.Format("{0}-{1}", towerConfigNum, levelNumByLevelId) });
			this.<OnShow>g__SetRankInfo|12_1();
			NetworkUtils.Tower.TowerRankIndexRequest(false, delegate(bool res, TowerRankIndexResponse response)
			{
				if (!this.isInit)
				{
					return;
				}
				if (res)
				{
					this.<OnShow>g__SetRankInfo|12_1();
				}
			});
			GuideController.Instance.DelTarget("TowerNode");
			GuideController.Instance.AddTarget("TowerNode", this.nodeButton.transform);
			GuideController.Instance.OpenViewTrigger(ViewName.DailyActivitiesViewModule);
			DelayCall.Instance.CallOnce(10, delegate
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.desRT);
			});
		}

		protected override void OnHide()
		{
		}

		private void NodeButtonClick()
		{
			if (this.mIsLock)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.TowerMainViewModule, null, 1, null, null);
		}

		[CompilerGenerated]
		private void <OnShow>g__SetRankInfo|12_1()
		{
			this.rankCountText.text = ((this.towerDataModule.CurTowerRank == 0) ? "--" : this.towerDataModule.CurTowerRank.ToString());
		}

		[SerializeField]
		private CustomText towerLevelCountText;

		[SerializeField]
		private CustomText rankCountText;

		[SerializeField]
		private CustomButton nodeButton;

		[SerializeField]
		private RectTransform desRT;

		private TowerDataModule towerDataModule;

		private bool isInit;
	}
}
