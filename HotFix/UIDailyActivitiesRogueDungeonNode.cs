using System;
using System.Runtime.CompilerServices;
using Framework;
using Framework.Logic.UI;
using Proto.Tower;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIDailyActivitiesRogueDungeonNode : UIDailyActivitiesNodeBase
	{
		public override FunctionID FunctionOpenID
		{
			get
			{
				return FunctionID.RogueDungeon;
			}
		}

		public override UIDailyActivitiesType DailyType
		{
			get
			{
				return UIDailyActivitiesType.RogueDungeon;
			}
		}

		protected override void OnInit()
		{
			this.isInit = true;
			this.mDataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			this.buttonNode.onClick.AddListener(new UnityAction(this.OnClickNode));
			RedPointController.Instance.RegRecordChange("DailyActivity.ChallengeTag.RogueDungeon", new Action<RedNodeListenData>(base.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			this.buttonNode.onClick.RemoveListener(new UnityAction(this.OnClickNode));
			RedPointController.Instance.UnRegRecordChange("DailyActivity.ChallengeTag.RogueDungeon", new Action<RedNodeListenData>(base.OnRedPointChange));
		}

		protected override void OnShow()
		{
			this.textFloor.text = Singleton<LanguageManager>.Instance.GetInfoByID("roguedungon_progress", new object[] { this.mDataModule.CurrentFloorID });
			this.textRank.text = "--";
			if (this.mDataModule.PlayerRank <= 0)
			{
				NetworkUtils.RogueDungeon.DoHellRankRequest(1, false, false, delegate(int page, bool isNextPage, bool res, HellRankResponse response)
				{
					if (res)
					{
						this.<OnShow>g__SetRankInfo|12_1();
					}
				});
			}
			else
			{
				this.<OnShow>g__SetRankInfo|12_1();
			}
			DelayCall.Instance.CallOnce(10, delegate
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.desRT);
			});
		}

		protected override void OnHide()
		{
		}

		private void OnClickNode()
		{
			if (this.mIsLock)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.RogueDungeonViewModule, null, 1, null, null);
		}

		[CompilerGenerated]
		private void <OnShow>g__SetRankInfo|12_1()
		{
			this.textRank.text = ((this.mDataModule.PlayerRank <= 0) ? "--" : this.mDataModule.PlayerRank.ToString());
		}

		[SerializeField]
		private CustomText textFloor;

		[SerializeField]
		private CustomText textRank;

		[SerializeField]
		private CustomButton buttonNode;

		[SerializeField]
		private RectTransform desRT;

		private RogueDungeonDataModule mDataModule;

		private bool isInit;
	}
}
