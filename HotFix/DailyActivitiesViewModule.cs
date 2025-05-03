using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class DailyActivitiesViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonChallenge.onClick.AddListener(new UnityAction(this.OnClickChallenge));
			this.buttonDungeon.onClick.AddListener(new UnityAction(this.OnClickDungeon));
			RedPointController.Instance.RegRecordChange("DailyActivity.ChallengeTag", new Action<RedNodeListenData>(this.OnRedPointChallengeTag));
			RedPointController.Instance.RegRecordChange("DailyActivity.DungeonTag", new Action<RedNodeListenData>(this.OnRedPointDungeonTag));
			for (int i = 0; i < this.nodeParent.childCount; i++)
			{
				GameObject gameObject = this.nodeParent.GetChild(i).gameObject;
				UIDailyActivitiesNodeBase component = gameObject.GetComponent<UIDailyActivitiesNodeBase>();
				if (component)
				{
					this.nodeList.Add(component);
				}
				else
				{
					gameObject.SetActiveSafe(false);
				}
			}
			List<int> list = new List<int>();
			list.Add(9);
			list.Add(2);
			list.Add(1);
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.DailyActivities, list);
		}

		private void InitData()
		{
			this.challengeTagItems.Clear();
			this.dungeonTagItems.Clear();
			List<UIDailyActivitiesType> list = new List<UIDailyActivitiesType>
			{
				UIDailyActivitiesType.Tower,
				UIDailyActivitiesType.WorldBoss,
				UIDailyActivitiesType.CrossArena,
				UIDailyActivitiesType.RogueDungeon,
				UIDailyActivitiesType.Mining
			};
			List<UIDailyActivitiesType> list2 = new List<UIDailyActivitiesType>();
			list2.Add(UIDailyActivitiesType.DragonLair);
			list2.Add(UIDailyActivitiesType.AstralTree);
			list2.Add(UIDailyActivitiesType.SwordIsland);
			if (LoginDataModule.IsTestB())
			{
				list2.Add(UIDailyActivitiesType.DeepSeaRuins);
			}
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				UIDailyActivitiesNodeBase uidailyActivitiesNodeBase = this.nodeList[i];
				uidailyActivitiesNodeBase.Init();
				uidailyActivitiesNodeBase.SetActive(false);
				if (uidailyActivitiesNodeBase.DailyType == UIDailyActivitiesType.Wait)
				{
					this.challengeTagItems.Add(uidailyActivitiesNodeBase);
					this.dungeonTagItems.Add(uidailyActivitiesNodeBase);
				}
				else if (list.Contains(uidailyActivitiesNodeBase.DailyType))
				{
					this.challengeTagItems.Add(uidailyActivitiesNodeBase);
				}
				else if (list2.Contains(uidailyActivitiesNodeBase.DailyType))
				{
					this.dungeonTagItems.Add(uidailyActivitiesNodeBase);
				}
			}
			this.challengeTagItems.Sort(new Comparison<UIDailyActivitiesNodeBase>(this.ComparerTag));
			this.dungeonTagItems.Sort(new Comparison<UIDailyActivitiesNodeBase>(this.ComparerTag));
		}

		public override void OnOpen(object data)
		{
			this.InitData();
			this.openData = data as DailyActivitiesViewModule.OpenData;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.challengeTagItems.Count; i++)
			{
				if (this.challengeTagItems[i].IsFunctionOpen())
				{
					flag = true;
					break;
				}
			}
			for (int j = 0; j < this.dungeonTagItems.Count; j++)
			{
				if (this.dungeonTagItems[j].IsFunctionOpen())
				{
					flag2 = true;
					break;
				}
			}
			DailyActivitiesPageType dailyActivitiesPageType;
			if (this.openData != null)
			{
				dailyActivitiesPageType = this.openData.openPageType;
			}
			else if (flag)
			{
				dailyActivitiesPageType = DailyActivitiesPageType.Challenge;
			}
			else
			{
				if (!flag2)
				{
					this.OnClickClose();
					return;
				}
				dailyActivitiesPageType = DailyActivitiesPageType.Dungeon;
			}
			if (dailyActivitiesPageType == DailyActivitiesPageType.Dungeon)
			{
				this.OnClickDungeon();
			}
			else
			{
				this.OnClickChallenge();
			}
			this.currencyCtrl.OnRefreshUI();
			GameApp.Data.GetDataModule(DataName.MiningDataModule).AsyncCachePos();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			foreach (UIDailyActivitiesNodeBase uidailyActivitiesNodeBase in this.nodeList)
			{
				if (uidailyActivitiesNodeBase.CanShow())
				{
					uidailyActivitiesNodeBase.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
			this.nodeList.ForEach(delegate(UIDailyActivitiesNodeBase item)
			{
				item.Hide();
			});
			if (this.openData != null)
			{
				Action onCloseCallback = this.openData.onCloseCallback;
				if (onCloseCallback != null)
				{
					onCloseCallback();
				}
				this.openData = null;
			}
			this.buttonDungeon.SetSelect(false);
			this.buttonChallenge.SetSelect(false);
		}

		public override void OnDelete()
		{
			RedPointController.Instance.UnRegRecordChange("DailyActivity.ChallengeTag", new Action<RedNodeListenData>(this.OnRedPointChallengeTag));
			RedPointController.Instance.UnRegRecordChange("DailyActivity.DungeonTag", new Action<RedNodeListenData>(this.OnRedPointDungeonTag));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonChallenge.onClick.RemoveListener(new UnityAction(this.OnClickChallenge));
			this.buttonDungeon.onClick.RemoveListener(new UnityAction(this.OnClickDungeon));
			this.currencyCtrl.DeInit();
			this.nodeList.ForEach(delegate(UIDailyActivitiesNodeBase item)
			{
				item.DeInit();
			});
			this.nodeList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DailyActivities_RefreshUI, new HandlerEvent(this.OnEventRefreshNode));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnEventRefreshNode));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DailyActivities_RefreshUI, new HandlerEvent(this.OnEventRefreshNode));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnEventRefreshNode));
		}

		private void OnEventRefreshNode(object sender, int type, BaseEventArgs eventArgs)
		{
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				UIDailyActivitiesNodeBase uidailyActivitiesNodeBase = this.nodeList[i];
				if (uidailyActivitiesNodeBase.CanShow())
				{
					uidailyActivitiesNodeBase.Show();
				}
			}
		}

		private void RefreshInfo(List<UIDailyActivitiesNodeBase> list)
		{
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				this.nodeList[i].SetActive(false);
			}
			float yPos = 20f;
			list.ForEach(delegate(UIDailyActivitiesNodeBase item)
			{
				if (item.CanShow())
				{
					item.SetAnchorPosY(-yPos);
					yPos += item.SizeY + 20f;
				}
			});
			RectTransform content = this.scroll.content;
			Vector2 sizeDelta = content.sizeDelta;
			sizeDelta.y = yPos;
			content.sizeDelta = sizeDelta;
			this.m_seqPool.Clear(false);
			int num = 0;
			for (int j = 0; j < list.Count; j++)
			{
				UIDailyActivitiesNodeBase uidailyActivitiesNodeBase = list[j];
				if (uidailyActivitiesNodeBase.CanShow())
				{
					uidailyActivitiesNodeBase.Show();
					uidailyActivitiesNodeBase.PlayShow(this.m_seqPool, num);
					num++;
				}
			}
		}

		private void OnClickChallenge()
		{
			if (this.buttonChallenge.IsSelected)
			{
				return;
			}
			this.buttonChallenge.SetSelect(true);
			this.buttonDungeon.SetSelect(false);
			this.RefreshInfo(this.challengeTagItems);
		}

		private void OnClickDungeon()
		{
			if (this.buttonDungeon.IsSelected)
			{
				return;
			}
			this.buttonDungeon.SetSelect(true);
			this.buttonChallenge.SetSelect(false);
			this.RefreshInfo(this.dungeonTagItems);
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.DailyActivitiesViewModule, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_ViewPopCheck_ReCheck, null);
		}

		private int ComparerTag(UIDailyActivitiesNodeBase x, UIDailyActivitiesNodeBase y)
		{
			return ((int)x.DailyType).CompareTo((int)y.DailyType);
		}

		private void OnRedPointChallengeTag(RedNodeListenData redData)
		{
			if (this.redNodeChallenge == null)
			{
				return;
			}
			this.redNodeChallenge.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointDungeonTag(RedNodeListenData redData)
		{
			if (this.redNodeDungeon == null)
			{
				return;
			}
			this.redNodeDungeon.gameObject.SetActive(redData.m_count > 0);
		}

		[SerializeField]
		private ScrollRect scroll;

		[SerializeField]
		private ModuleCurrencyCtrl currencyCtrl;

		[SerializeField]
		private CustomButton buttonClose;

		[SerializeField]
		private CustomChooseButton buttonChallenge;

		[SerializeField]
		private CustomChooseButton buttonDungeon;

		[SerializeField]
		private Transform nodeParent;

		[SerializeField]
		private RedNodeOneCtrl redNodeChallenge;

		[SerializeField]
		private RedNodeOneCtrl redNodeDungeon;

		private SequencePool m_seqPool = new SequencePool();

		private const float SPACE_Y = 20f;

		private List<UIDailyActivitiesNodeBase> nodeList = new List<UIDailyActivitiesNodeBase>();

		private List<UIDailyActivitiesNodeBase> challengeTagItems = new List<UIDailyActivitiesNodeBase>();

		private List<UIDailyActivitiesNodeBase> dungeonTagItems = new List<UIDailyActivitiesNodeBase>();

		private DailyActivitiesViewModule.OpenData openData;

		public class OpenData
		{
			public DailyActivitiesPageType openPageType = DailyActivitiesPageType.Challenge;

			public Action onCloseCallback;
		}
	}
}
