using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using HotFix.Client;
using Proto.Common;
using Proto.Tower;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleTowerWinViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.BattleTowerWinViewModule;
		}

		public override void OnCreate(object data)
		{
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
		}

		public override void OnOpen(object data)
		{
			this.m_battleTowerDataModule = GameApp.Data.GetDataModule(DataName.BattleTowerDataModule);
			this.buttonOK.onClick.AddListener(new UnityAction(this.OnBtnCloseHandler));
			this.buttonNextFloor.onClick.AddListener(new UnityAction(this.OnNextFloor));
			this.buttonNextFloor.gameObject.SetActiveSafe(this.towerDataModule.FightLevelIndex < this.towerDataModule.MaxLevelCount - 1);
			this.m_rewardDatas.Clear();
			if (this.m_battleTowerDataModule.m_towerChallengeResponse != null)
			{
				this.m_rewardDatas = this.m_battleTowerDataModule.m_towerChallengeResponse.CommonData.Reward;
			}
			if (this.m_rewardPrefab != null)
			{
				this.m_rewardPrefab.SetActive(false);
			}
			this.CreateRewards();
			this.m_seqPool.Clear(false);
			this.PlayRewards();
			GameApp.Sound.PlayClip(52, 1f);
		}

		public override void OnClose()
		{
			this.buttonOK.onClick.RemoveListener(new UnityAction(this.OnBtnCloseHandler));
			this.buttonNextFloor.onClick.RemoveListener(new UnityAction(this.OnNextFloor));
			this.DestoryRewards();
			this.m_seqPool.Clear(false);
			this.m_rewardDatas.Clear();
			this.m_battleTowerDataModule = null;
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnBtnCloseHandler()
		{
			if (this.isLoading)
			{
				return;
			}
			this.isLoading = true;
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
					instance.SetData(GameOverType.Win);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
					GameApp.View.CloseView(ViewName.BattleTowerWinViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
					this.isLoading = false;
				});
			});
		}

		private void OnNextFloor()
		{
			UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.Tower);
			if (ticket == null || ticket.NewNum <= 0U)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_not_enough");
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			if (this.isLoading)
			{
				return;
			}
			this.isLoading = true;
			this.SetButtonEnabled(false);
			NetworkUtils.Tower.TowerChallengeRequest(this.towerDataModule.CurTowerLevelId, delegate(bool res, TowerChallengeResponse response)
			{
				if (!res)
				{
					this.SetButtonEnabled(true);
					this.isLoading = false;
					return;
				}
				EventArgsTowerChallengeEnd instance = Singleton<EventArgsTowerChallengeEnd>.Instance;
				instance.Result = (int)response.Result;
				instance.LevelId = (int)response.ConfigId;
				EventArgsAddItemTipData eventArgsAddItemTipData = new EventArgsAddItemTipData();
				eventArgsAddItemTipData.SetDataCount(19, -1, this.buttonNextFloor.transform.position);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, eventArgsAddItemTipData);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TowerDataMoudule_TowerChallengeEnd, instance);
				EventArgsBattleTowerEnter instance2 = Singleton<EventArgsBattleTowerEnter>.Instance;
				instance2.SetData(response);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleTower_BattleTowerEnter, instance2);
				this.DoBattle(instance, delegate
				{
					this.SetButtonEnabled(true);
				});
			});
		}

		private void DoBattle(EventArgsTowerChallengeEnd data, Action beginGame)
		{
			Action <>9__1;
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				LoadingViewModule viewModule = GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule);
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate
					{
						EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
						instance.SetData(DxxTools.UI.GetTowerOpenData());
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
						EventArgsSetTowerBattleReadyData instance2 = Singleton<EventArgsSetTowerBattleReadyData>.Instance;
						instance2.SetData(data.LevelId, false, data.Result);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_TowerDataMoudule_SetBattleReadyData, instance2);
						Action beginGame2 = beginGame;
						if (beginGame2 != null)
						{
							beginGame2();
						}
						GameApp.View.CloseView(this.GetName(), null);
						EventArgsGameDataEnter instance3 = Singleton<EventArgsGameDataEnter>.Instance;
						instance3.SetData(GameModel.Tower, null);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance3);
						GameApp.State.ActiveState(StateName.BattleTowerState);
						this.isLoading = false;
					});
				}
				viewModule.PlayShow(action);
			});
		}

		private void SetButtonEnabled(bool isEnabled)
		{
			this.buttonOK.enabled = isEnabled;
			this.buttonNextFloor.enabled = isEnabled;
		}

		private void CreateRewards()
		{
			if (this.m_rewardDatas == null || this.m_rewardDatas.Count == 0)
			{
				return;
			}
			if (this.m_rewardParent == null)
			{
				return;
			}
			for (int i = 0; i < this.m_rewardDatas.Count; i++)
			{
				RewardDto rewardDto = this.m_rewardDatas[i];
				if (this.m_rewardDatas != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_rewardPrefab);
					gameObject.SetParentNormal(this.m_rewardParent, false);
					gameObject.SetActive(true);
					UIItem component = gameObject.GetComponent<UIItem>();
					component.SetCountShowType(UIItem.CountShowType.ShowAll);
					component.Init();
					component.SetData(rewardDto.ToPropData());
					component.OnRefresh();
					this.m_rewards[gameObject.GetInstanceID()] = component;
				}
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_rewardParent);
		}

		public void DestoryRewards()
		{
			foreach (KeyValuePair<int, UIItem> keyValuePair in this.m_rewards)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_rewards.Clear();
		}

		private void PlayRewards()
		{
			foreach (KeyValuePair<int, UIItem> keyValuePair in this.m_rewards)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.transform.localScale = Vector3.zero;
				}
			}
			this.rewardBgObj.SetActiveSafe(false);
			this.flagObj.transform.localScale = Vector3.zero;
			this.buttonOK.transform.localScale = Vector3.zero;
			if (this.buttonNextFloor.gameObject.activeSelf)
			{
				this.buttonNextFloor.transform.localScale = Vector3.zero;
			}
			Sequence sequence = this.m_seqPool.Get();
			int num = 0;
			float num2 = 0.01f;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one, this.duration2));
			TweenSettingsExtensions.AppendInterval(sequence, this.duration1 + this.duration2);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.rewardBgObj.SetActiveSafe(true);
			});
			foreach (KeyValuePair<int, UIItem> keyValuePair2 in this.m_rewards)
			{
				if (!(keyValuePair2.Value == null))
				{
					TweenSettingsExtensions.AppendInterval(sequence, (float)num * num2);
					TweenSettingsExtensions.Join(sequence, this.PlayScaleAni(keyValuePair2.Value.transform));
					num++;
				}
			}
			if (this.m_rewards.Count > 0)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (this.m_rewardParent != null)
					{
						LayoutRebuilder.MarkLayoutForRebuild(this.m_rewardParent);
					}
				});
			}
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonOK.transform, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(this.buttonOK.transform, Vector3.one, this.duration2));
			if (this.buttonNextFloor.gameObject.activeSelf)
			{
				TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonNextFloor.transform, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(this.buttonNextFloor.transform, Vector3.one, this.duration2));
			}
		}

		private Sequence PlayScaleAni(Transform trans)
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(trans, Vector3.one * this.scale, this.duration1));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(trans, Vector3.one, this.duration2));
			return sequence;
		}

		[GameTestMethod("爬塔", "爬塔胜利", "", 401)]
		private static void OpenTowerWin()
		{
			GameApp.View.OpenView(ViewName.BattleTowerWinViewModule, null, 1, null, null);
		}

		[SerializeField]
		private CustomButton buttonOK;

		[SerializeField]
		private CustomButton buttonNextFloor;

		[SerializeField]
		private GameObject flagObj;

		[SerializeField]
		private GameObject rewardBgObj;

		public RectTransform m_rewardParent;

		public GameObject m_rewardPrefab;

		private BattleTowerDataModule m_battleTowerDataModule;

		private Dictionary<int, UIItem> m_rewards = new Dictionary<int, UIItem>();

		private SequencePool m_seqPool = new SequencePool();

		private RepeatedField<RewardDto> m_rewardDatas = new RepeatedField<RewardDto>();

		private TowerDataModule towerDataModule;

		private bool isLoading;

		private float scale = 1.1f;

		private float duration1 = 0.15f;

		private float duration2 = 0.05f;
	}
}
