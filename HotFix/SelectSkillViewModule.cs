using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using Habby.Ads;
using Shop.Arena;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class SelectSkillViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.selectSkillItemObj.SetActiveSafe(false);
			this.adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.buttonRefreshAd.gameObject.SetActiveSafe(false);
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.selectSkillItemObj);
				gameObject.transform.SetParentNormal(this.skillLayout.transform, false);
				UIGameEventSelectSkillItem component = gameObject.GetComponent<UIGameEventSelectSkillItem>();
				component.Init();
				this.itemList.Add(component);
			}
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				this.CloseSelf();
				return;
			}
			this.openData = data as SelectSkillViewModule.OpenData;
			if (this.openData == null)
			{
				this.CloseSelf();
				return;
			}
			if (this.openData.randomNum == 0)
			{
				HLog.LogError("随机技能数量为0，请检查");
				this.CloseSelf();
				return;
			}
			this.buttonRefresh.onClick.AddListener(new UnityAction(this.OnClickRefresh));
			this.Button_LearnedSkills.onClick.AddListener(new UnityAction(this.OnClickLearnedSkills));
			this.fxA.SetActiveSafe(true);
			this.refreshNum = 0;
			if (this.openData.type == SelectSkillViewModule.SelectSkillType.LevelUp)
			{
				this.refreshNum = Singleton<GameEventController>.Instance.GetCanRefreshSkillCount();
			}
			this.buttonRefresh.gameObject.SetActiveSafe(this.refreshNum > 0);
			this.RefreshAd();
			this.ShowSkills(this.refreshNum, this.openData.randomSeed);
			this.modelItem.Init();
			this.modelItem.OnShow();
			this.modelItem.ShowSelfPlayerModel("SelectSkillViewModule", false);
		}

		private void RefreshAd()
		{
			if (this.openData != null)
			{
				bool flag = this.openData.type == SelectSkillViewModule.SelectSkillType.LevelUp;
			}
			this.adDataModule.GetMaxTimes(GameConfig.GameEvent_RefreshSkill_AdId);
			this.adDataModule.GetWatchTimes(GameConfig.GameEvent_RefreshSkill_AdId);
			Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.AdRefreshSkill, false);
		}

		public override void OnClose()
		{
			this.modelItem.OnHide(false);
			this.modelItem.DeInit();
			this.skills.Clear();
			this.buttonRefresh.onClick.RemoveListener(new UnityAction(this.OnClickRefresh));
			this.Button_LearnedSkills.onClick.RemoveListener(new UnityAction(this.OnClickLearnedSkills));
			this.sequencePool.Clear(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnDelete()
		{
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void ShowSkills(int num, int seed)
		{
			this.popAni.Clear();
			this.textRefreshCount.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiselectskill_refresh", new object[] { num });
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(this.openData.sourceType, this.openData.randomNum, seed);
			int num2 = 0;
			for (int i = 0; i < randomSkillList.Count; i++)
			{
				int quality = (int)randomSkillList[i].quality;
				if (num2 < quality)
				{
					num2 = quality;
				}
			}
			GameTGATools.Ins.AddStageClickTempSkillShow(randomSkillList);
			if (randomSkillList == null || randomSkillList.Count == 0)
			{
				this.CloseSelf();
				return;
			}
			this.curSelectNum = 0;
			for (int j = 0; j < this.itemList.Count; j++)
			{
				this.itemList[j].gameObject.SetActiveSafe(false);
			}
			for (int k = 0; k < randomSkillList.Count; k++)
			{
				UIGameEventSelectSkillItem uigameEventSelectSkillItem;
				if (k < this.itemList.Count)
				{
					uigameEventSelectSkillItem = this.itemList[k];
					this.popAni.AddData(new PopAnimationSequence.Data
					{
						transform = uigameEventSelectSkillItem.transform,
						itemFinish = new Action<GameObject>(this.OnItemAniFinish),
						playTime = 0.1f
					});
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.selectSkillItemObj);
					gameObject.transform.SetParentNormal(this.skillLayout.transform, false);
					uigameEventSelectSkillItem = gameObject.GetComponent<UIGameEventSelectSkillItem>();
					uigameEventSelectSkillItem.Init();
					this.itemList.Add(uigameEventSelectSkillItem);
					this.popAni.AddData(new PopAnimationSequence.Data
					{
						transform = gameObject.transform,
						itemFinish = new Action<GameObject>(this.OnItemAniFinish),
						playTime = 0.1f
					});
				}
				uigameEventSelectSkillItem.gameObject.transform.localScale = Vector3.zero;
				uigameEventSelectSkillItem.gameObject.SetActiveSafe(true);
				uigameEventSelectSkillItem.Refresh(randomSkillList[k], new Action<GameEventSkillBuildData>(this.SelectSkill));
			}
			if (this.buttonRefresh.gameObject.activeSelf)
			{
				this.popAni.AddData(new PopAnimationSequence.Data
				{
					transform = this.buttonRefresh.transform
				});
			}
			this.fxA2B.SetActiveSafe(false);
			this.fxA2C.SetActiveSafe(false);
			this.fxA.SetActiveSafe(true);
			this.fxB.SetActiveSafe(false);
			this.fxC.SetActiveSafe(false);
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
			float num3 = 0f;
			if (num2 == 0)
			{
				GameApp.Sound.PlayClip(58, 1f);
			}
			if (num2 >= 1)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.fxA2B.SetActiveSafe(true);
					GameApp.Sound.PlayClip(130, 1f);
				});
				TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.fxA.SetActiveSafe(false);
					this.fxB.SetActiveSafe(true);
				});
				num3 = 0.1f;
			}
			if (num2 >= 2)
			{
				TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.fxA2B.SetActiveSafe(false);
					this.fxA2C.SetActiveSafe(true);
					GameApp.Sound.PlayClip(131, 1f);
				});
				TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.fxB.SetActiveSafe(false);
					this.fxC.SetActiveSafe(true);
				});
				num3 = 0.1f;
			}
			if (num3 > 0f)
			{
				TweenSettingsExtensions.AppendInterval(sequence, num3);
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.PlayPopAni();
			});
		}

		public void SelectSkill(GameEventSkillBuildData data)
		{
			this.skills.Add(data);
			GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { data }, true);
			Singleton<GameEventController>.Instance.SelectSkill(data, this.openData.type != SelectSkillViewModule.SelectSkillType.GetSkill);
			this.CloseSelf();
			Action callBack = this.openData.callBack;
			if (callBack == null)
			{
				return;
			}
			callBack();
		}

		private void CloseSelf()
		{
			EventArgSelectSkillEnd instance = Singleton<EventArgSelectSkillEnd>.Instance;
			instance.SetData(this.skills);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSelectSkill, instance);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, null);
			GameApp.View.CloseView(ViewName.SelectSkillViewModule, null);
		}

		private void OnClickRefresh()
		{
			AdsRequestHelper.Instance.RefreshAds();
			Singleton<GameEventController>.Instance.UpdateRefreshSkillCount();
			this.refreshNum = Singleton<GameEventController>.Instance.GetCanRefreshSkillCount();
			int refreshSkillSeed = Singleton<GameEventController>.Instance.GetRefreshSkillSeed(false);
			this.ShowSkills(this.refreshNum, refreshSkillSeed);
			if (this.refreshNum <= 0)
			{
				this.buttonRefresh.gameObject.SetActiveSafe(false);
				this.RefreshAd();
			}
		}

		private void OnClickRefreshAd()
		{
			int maxTimes = this.adDataModule.GetMaxTimes(GameConfig.GameEvent_RefreshSkill_AdId);
			int watch = this.adDataModule.GetWatchTimes(GameConfig.GameEvent_RefreshSkill_AdId);
			int seed = Singleton<GameEventController>.Instance.GetRefreshSkillSeed(true);
			if (watch < maxTimes)
			{
				Action<bool, FinishAdvertResponse> <>9__1;
				AdBridge.PlayRewardVideo(GameConfig.GameEvent_RefreshSkill_AdId, delegate(bool isSuccess)
				{
					if (isSuccess)
					{
						int gameEvent_RefreshSkill_AdId = GameConfig.GameEvent_RefreshSkill_AdId;
						Action<bool, FinishAdvertResponse> action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate(bool result, FinishAdvertResponse resp)
							{
								if (result)
								{
									this.ShowSkills(watch, seed);
									this.RefreshAd();
									GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(GameConfig.GameEvent_RefreshSkill_AdId), "REWARD ", "", null, null);
								}
							});
						}
						NetworkUtils.Shop.FinishAdvertRequest(gameEvent_RefreshSkill_AdId, action);
					}
				});
			}
		}

		private void OnClickLearnSkill()
		{
		}

		private void OnClickLearnedSkills()
		{
			GameApp.View.OpenView(ViewName.LearnedSkillsViewModule, null, 1, null, null);
		}

		private void OnItemAniFinish(GameObject obj)
		{
			UIGameEventSelectSkillItem component = obj.GetComponent<UIGameEventSelectSkillItem>();
			if (component)
			{
				component.PlayEffect();
			}
		}

		private void PlayPopAni()
		{
			Sequence sequence = this.sequencePool.Get();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				Transform trans = this.itemList[i].transform;
				Sequence sequence2 = this.sequencePool.Get();
				TweenSettingsExtensions.AppendCallback(sequence2, delegate
				{
					trans.localScale = Vector3.one * 0.5f;
				});
				TweenSettingsExtensions.Append(sequence2, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(trans, Vector3.one, 0.15f), 27));
				TweenSettingsExtensions.AppendCallback(sequence2, delegate
				{
					this.OnItemAniFinish(trans.gameObject);
				});
				if (i == 0)
				{
					TweenSettingsExtensions.Join(sequence, sequence2);
				}
				else
				{
					TweenSettingsExtensions.AppendInterval(sequence, 0.01f);
					TweenSettingsExtensions.Join(sequence, sequence2);
				}
			}
		}

		public GameObject skillLayout;

		public GameObject buttonsObj;

		public GameObject selectSkillItemObj;

		public CustomButton buttonRefresh;

		public PopAnimationSequence popAni;

		public CustomButton Button_LearnedSkills;

		public UIModelItem modelItem;

		public CustomText textRefreshCount;

		public CustomButton buttonRefreshAd;

		public GameObject fxA;

		public GameObject fxB;

		public GameObject fxC;

		public GameObject fxA2B;

		public GameObject fxA2C;

		private int curSelectNum;

		private List<UIGameEventSelectSkillItem> itemList = new List<UIGameEventSelectSkillItem>();

		private List<GameEventSkillBuildData> skills = new List<GameEventSkillBuildData>();

		private SelectSkillViewModule.OpenData openData;

		private int refreshNum;

		private AdDataModule adDataModule;

		private SequencePool sequencePool = new SequencePool();

		public enum SelectSkillType
		{
			None,
			LevelUp,
			GetSkill,
			BeginSkill
		}

		public class OpenData
		{
			public SelectSkillViewModule.SelectSkillType type;

			public int randomNum;

			public int selectNum;

			public Action callBack;

			public SkillBuildSourceType sourceType = SkillBuildSourceType.Normal;

			public int randomSeed;
		}
	}
}
