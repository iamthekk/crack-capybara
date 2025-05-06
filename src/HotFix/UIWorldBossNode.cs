using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIWorldBossNode : UIDailyActivitiesNodeBase
	{
		public override FunctionID FunctionOpenID
		{
			get
			{
				return FunctionID.WorldBoss;
			}
		}

		public override UIDailyActivitiesType DailyType
		{
			get
			{
				return UIDailyActivitiesType.WorldBoss;
			}
		}

		protected override void OnInit()
		{
			this.isInit = true;
			this.worldBossDataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			this.nodeButton.m_onClick = new Action(this.NodeButtonClick);
			RedPointController.Instance.RegRecordChange("DailyActivity.ChallengeTag.WorldBoss", new Action<RedNodeListenData>(base.OnRedPointChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_WorldBoss_Update, new HandlerEvent(this.OnWorldBossUpdate));
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			this.nodeButton.m_onClick = null;
			RedPointController.Instance.UnRegRecordChange("DailyActivity.ChallengeTag.WorldBoss", new Action<RedNodeListenData>(base.OnRedPointChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_WorldBoss_Update, new HandlerEvent(this.OnWorldBossUpdate));
		}

		public override bool CanShow()
		{
			return this.worldBossDataModule.HasInfo();
		}

		protected override void OnShow()
		{
			this.FreshUI();
			GuideController.Instance.DelTarget("WorldBossNode");
			GuideController.Instance.AddTarget("WorldBossNode", this.nodeButton.transform);
			GuideController.Instance.OpenViewTrigger(ViewName.WorldBossViewModule);
		}

		private void FreshUI()
		{
			this.totalDamageText.text = DxxTools.FormatNumber(this.worldBossDataModule.TotalDamage);
			int num = this.worldBossDataModule.RankLevel;
			if (num <= 0)
			{
				num = 1;
			}
			WorldBoss_Subsection worldBoss_Subsection = GameApp.Table.GetManager().GetWorldBoss_Subsection(num);
			this.rankGroupIcon.SetImage(worldBoss_Subsection.atlasName, worldBoss_Subsection.atlasId);
			this.rankGroupNameText.SetText(worldBoss_Subsection.languageId);
			this.FreshTime();
			DelayCall.Instance.CallOnce(10, delegate
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.desRT);
			});
		}

		private void FreshTime()
		{
			long num = this.worldBossDataModule.GetSeasonRemainTime();
			if (num > 0L)
			{
				this.timeText.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Season_EndTime", new object[] { Singleton<LanguageManager>.Instance.GetTime(num) });
				return;
			}
			num = this.worldBossDataModule.GetNextSeasonOpenRemainTime();
			if (num > 0L)
			{
				this.timeText.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_NextBattle_AfterTime", new object[] { Singleton<LanguageManager>.Instance.GetTime(num) });
				return;
			}
			this.timeText.text = "--";
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
			if (this.worldBossDataModule.GetSeasonRemainTime() <= 0L && this.worldBossDataModule.GetNextSeasonOpenRemainTime() > 0L)
			{
				this.worldBossDataModule.ShowNextSeasonTimeTip();
				return;
			}
			GameApp.View.OpenView(ViewName.WorldBossViewModule, null, 1, null, null);
		}

		private void OnWorldBossUpdate(object sender, int type, BaseEventArgs eventargs)
		{
			this.FreshUI();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.FreshTime();
		}

		[SerializeField]
		private CustomText totalDamageText;

		[SerializeField]
		private CustomText rankGroupNameText;

		[SerializeField]
		private CustomImage rankGroupIcon;

		[SerializeField]
		private CustomText timeText;

		[SerializeField]
		private CustomButton nodeButton;

		[SerializeField]
		private RectTransform desRT;

		private WorldBossDataModule worldBossDataModule;

		private bool isInit;
	}
}
