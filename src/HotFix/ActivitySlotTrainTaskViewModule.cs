using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class ActivitySlotTrainTaskViewModule : BaseViewModule
	{
		public ViewName GetModuleId()
		{
			return ViewName.ActivitySlotTrainTaskViewModule;
		}

		public string GetModuleName()
		{
			return ViewName.ActivitySlotTrainTaskViewModule.ToString();
		}

		public override void OnCreate(object data)
		{
			this.Obj_NetLoading.SetActive(false);
			this.m_activityNode.SetActive(false);
			this._catchList = new List<ActivitySlotTrainTaskItem>();
			this.Button_Close.m_onClick = new Action(this.OnCloseSelf);
			this.buttonMask.m_onClick = new Action(this.OnCloseSelf);
			this.m_btnInfo.m_onClick = new Action(this.OnBtnInfoClick);
			this.m_btnInfo.gameObject.SetActiveSafe(false);
			this.currencyCtrl.Init();
		}

		public override void OnDelete()
		{
			for (int i = 0; i < this._catchList.Count; i++)
			{
				Object.Destroy(this._catchList[i]);
			}
			this._catchList.Clear();
			this.Button_Close.m_onClick = null;
			this.buttonMask.m_onClick = null;
			this.m_btnInfo.m_onClick = null;
			this.currencyCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ActivitySlotTrainQuestChanged, new HandlerEvent(this.OnEventQuestChanged));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ActivitySlotTrainPayChanged, new HandlerEvent(this.OnEventPayChanged));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ActivitySlotTrainQuestChanged, new HandlerEvent(this.OnEventQuestChanged));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ActivitySlotTrainPayChanged, new HandlerEvent(this.OnEventPayChanged));
		}

		public override void OnOpen(object data)
		{
			this.viewType = (ActivitySlotTrainTaskViewType)data;
			this.dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
			this.mainCfg = GameApp.Table.GetManager().GetActivityTurntable_ActivityTurntableModelInstance().GetElementById(this.dataModule.TurntableId);
			this.FreshAllView();
		}

		public override void OnClose()
		{
			this.m_pool.Clear(false);
			DxxTools.UI.RemoveServerTimeClockCallback(this.GetModuleName());
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.RefreshCountdown();
		}

		private void FreshAllView()
		{
			this.currencyCtrl.SetItemId(this.mainCfg.priceId);
			if (this.viewType == ActivitySlotTrainTaskViewType.Quest)
			{
				this.m_titleText.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_quest_title");
			}
			else
			{
				this.m_titleText.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_gift_title");
			}
			this.RefreshCountdown();
			this.FreshTaskItems(true);
		}

		private void RefreshCountdown()
		{
			if (!this.dataModule.CanShow())
			{
				this.OnCloseSelf();
				return;
			}
			this.m_leftTimePrefixText.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_lefttime_prefix");
			this.m_leftTimeText.text = DxxTools.FormatFullTimeWithDay(this.dataModule.LeftTime);
		}

		private void FreshTaskItems(bool playAnim)
		{
			ActivitySlotTrainTaskViewType activitySlotTrainTaskViewType = this.viewType;
			if (activitySlotTrainTaskViewType == ActivitySlotTrainTaskViewType.Quest)
			{
				this.ShowQuest(playAnim);
				return;
			}
			if (activitySlotTrainTaskViewType != ActivitySlotTrainTaskViewType.Pay)
			{
				return;
			}
			this.ShowPay(playAnim);
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(this.GetModuleId(), null);
		}

		private void OnBtnInfoClick()
		{
			Module_moduleInfo elementById = GameApp.Table.GetManager().GetModule_moduleInfoModelInstance().GetElementById(this.systemId);
			if (elementById != null)
			{
				InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
				{
					m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId),
					m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.infoId)
				};
				GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
			}
		}

		private void OnEventQuestChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.viewType == ActivitySlotTrainTaskViewType.Quest)
			{
				this.FreshTaskItems(false);
			}
		}

		private void OnEventPayChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.viewType == ActivitySlotTrainTaskViewType.Pay)
			{
				this.FreshTaskItems(false);
			}
		}

		private void ShowQuest(bool playAnim = false)
		{
			List<ActivityTurntable_TurntableQuest> list = (from x in GameApp.Table.GetManager().GetActivityTurntable_TurntableQuestElements()
				orderby x.ID
				select x).ToList<ActivityTurntable_TurntableQuest>();
			this.content.anchoredPosition = Vector2.zero;
			this.setNodeByCount(list.Count);
			if (playAnim)
			{
				this.PlayShowAnim();
			}
			List<ActivitySlotTrainTaskViewModule.QuestItemInfo> list2 = new List<ActivitySlotTrainTaskViewModule.QuestItemInfo>();
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				ActivitySlotTrainTaskViewModule.QuestItemInfo questItemInfo = new ActivitySlotTrainTaskViewModule.QuestItemInfo();
				list2.Add(questItemInfo);
				ActivityTurntable_TurntableQuest activityTurntable_TurntableQuest = list[i];
				questItemInfo.cfg = activityTurntable_TurntableQuest;
				questItemInfo.isFinished = this.dataModule.IsFinishedTask(activityTurntable_TurntableQuest);
				questItemInfo.score = this.dataModule.GetTaskScore(activityTurntable_TurntableQuest);
				questItemInfo.canPick = this.dataModule.CanPickTask(activityTurntable_TurntableQuest);
			}
			list2.Sort(new Comparison<ActivitySlotTrainTaskViewModule.QuestItemInfo>(ActivitySlotTrainTaskViewModule.QuestItemInfo.Compare));
			for (int j = 0; j < list2.Count; j++)
			{
				ActivitySlotTrainTaskViewModule.QuestItemInfo questItemInfo2 = list2[j];
				this._catchList[num].ShowQuest(questItemInfo2);
				num++;
			}
		}

		private void ShowPay(bool playAnim = false)
		{
			List<ActivityTurntable_TurntablePay> list = new List<ActivityTurntable_TurntablePay>();
			IEnumerable<ActivityTurntable_TurntablePay> activityTurntable_TurntablePayElements = GameApp.Table.GetManager().GetActivityTurntable_TurntablePayElements();
			bool flag = GameApp.Data.GetDataModule(DataName.AdDataModule).CheckCloudDataAdOpen();
			foreach (ActivityTurntable_TurntablePay activityTurntable_TurntablePay in activityTurntable_TurntablePayElements)
			{
				if (activityTurntable_TurntablePay.AdId <= 0 || flag)
				{
					list.Add(activityTurntable_TurntablePay);
				}
			}
			this.content.anchoredPosition = Vector2.zero;
			this.setNodeByCount(list.Count);
			if (playAnim)
			{
				this.PlayShowAnim();
			}
			List<ActivitySlotTrainTaskViewModule.PayItemInfo> list2 = new List<ActivitySlotTrainTaskViewModule.PayItemInfo>();
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				ActivitySlotTrainTaskViewModule.PayItemInfo payItemInfo = new ActivitySlotTrainTaskViewModule.PayItemInfo();
				list2.Add(payItemInfo);
				ActivityTurntable_TurntablePay activityTurntable_TurntablePay2 = list[i];
				payItemInfo.cfg = activityTurntable_TurntablePay2;
				payItemInfo.payMax = activityTurntable_TurntablePay2.objToplimit;
				payItemInfo.payCount = this.dataModule.GetPayCount(activityTurntable_TurntablePay2);
				payItemInfo.isFinished = this.dataModule.IsFinishedPay(activityTurntable_TurntablePay2);
				if (activityTurntable_TurntablePay2.AdId == 0)
				{
					payItemInfo.iapData = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(activityTurntable_TurntablePay2.PurchaseId);
				}
				payItemInfo.isFree = this.dataModule.CanFreePay(activityTurntable_TurntablePay2);
			}
			list2.Sort(new Comparison<ActivitySlotTrainTaskViewModule.PayItemInfo>(ActivitySlotTrainTaskViewModule.PayItemInfo.Compare));
			for (int j = 0; j < list2.Count; j++)
			{
				ActivitySlotTrainTaskViewModule.PayItemInfo payItemInfo2 = list2[j];
				this._catchList[num].ShowPay(payItemInfo2);
				num++;
			}
		}

		private void PlayShowAnim()
		{
			this.m_pool.Clear(false);
			Sequence sequence = this.m_pool.Get();
			for (int i = 0; i < this._catchList.Count; i++)
			{
				DxxTools.UI.DoMoveRightToScreenAnim(sequence, this._catchList[i].taskAniNode, 0f, 0.1f * (float)i, 0.2f, 9);
			}
		}

		private void setNodeByCount(int count)
		{
			for (int i = 0; i < this._catchList.Count; i++)
			{
				this._catchList[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < count; j++)
			{
				if (j < this._catchList.Count)
				{
					this._catchList[j].gameObject.SetActive(true);
				}
				else
				{
					ActivitySlotTrainTaskItem activitySlotTrainTaskItem = Object.Instantiate<ActivitySlotTrainTaskItem>(this.m_activityNode, this.content.transform);
					activitySlotTrainTaskItem.SetActive(true);
					this._catchList.Add(activitySlotTrainTaskItem);
				}
			}
		}

		public int systemId = 113;

		public CustomButton Button_Close;

		public CustomButton buttonMask;

		public CustomButton m_btnInfo;

		public UIActivityCurrency currencyCtrl;

		public CustomText m_titleText;

		public CustomText m_leftTimePrefixText;

		public CustomText m_leftTimeText;

		public GameObject Obj_NetLoading;

		private SequencePool m_pool = new SequencePool();

		private ActivitySlotTrainTaskViewType viewType;

		private ActivitySlotTrainDataModule dataModule;

		private ActivityTurntable_ActivityTurntable mainCfg;

		public ActivitySlotTrainTaskItem m_activityNode;

		private List<ActivitySlotTrainTaskItem> _catchList;

		public RectTransform content;

		public class QuestItemInfo
		{
			public static int Compare(ActivitySlotTrainTaskViewModule.QuestItemInfo a, ActivitySlotTrainTaskViewModule.QuestItemInfo b)
			{
				if (a.isFinished != b.isFinished)
				{
					return a.isFinished.CompareTo(b.isFinished);
				}
				if (a.canPick != b.canPick)
				{
					return -a.canPick.CompareTo(b.canPick);
				}
				return a.cfg.ID.CompareTo(b.cfg.ID);
			}

			public ActivityTurntable_TurntableQuest cfg;

			public bool isFinished;

			public bool canPick;

			public int score;
		}

		public class PayItemInfo
		{
			public static int Compare(ActivitySlotTrainTaskViewModule.PayItemInfo a, ActivitySlotTrainTaskViewModule.PayItemInfo b)
			{
				if (a.isFinished != b.isFinished)
				{
					return a.isFinished.CompareTo(b.isFinished);
				}
				if (a.isFree != b.isFree)
				{
					return -a.isFree.CompareTo(b.isFree);
				}
				return a.cfg.id.CompareTo(b.cfg.id);
			}

			public ActivityTurntable_TurntablePay cfg;

			public bool isFinished;

			public bool isFree;

			public int payCount;

			public int payMax;

			public IAP_Purchase iapData;
		}
	}
}
