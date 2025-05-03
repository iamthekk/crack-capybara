using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class FortuneWheelViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.btnStartSpin.Init();
			this.wheelSymbolItem.gameObject.SetActive(false);
			this.pointerCollider.OnPointerColliderEnter = new Action(this.OnPointerCollide);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickBonus));
		}

		public override void OnOpen(object data)
		{
			GameApp.Sound.PlayClip(260018, 1f);
			this.EnableNailsCollider(false);
			this.openData = data as FortuneWheelViewModule.OpenData;
			if (this.openData == null)
			{
				this.InitWheel(1);
				this.xRandom = new XRandom(0);
			}
			else
			{
				this.xRandom = new XRandom(this.openData.randSeed);
				this.InitWheel(this.openData.turntableId);
			}
			this.btnStartSpin.SetTextUp(Singleton<LanguageManager>.Instance.GetInfoByID("fortune_wheel_rotate"));
			this.btnStartSpin.SetTextDown(Singleton<LanguageManager>.Instance.GetInfoByID("fortune_wheel_rotating"));
			this.btnStartSpin.SetLock(false);
			if (this.openData == null || !this.openData.isBonusGame)
			{
				this.ShowSpine();
				return;
			}
			if (this.openData.currentMinor >= this.openData.needMinor)
			{
				GameApp.SDK.Analyze.Track_StagetClickTest(null);
				GameTGATools.Ins.SetStageButtonContent(0, "小吉奖励");
				GameTGATools.Ins.SetStageButtonClickIndex(0);
				GameTGATools.Ins.OnStageClickButton();
				this.ShowSpine();
				return;
			}
			this.lockObj.SetActive(true);
			this.btnStartSpin.gameObject.SetActive(false);
			this.textLock.text = Singleton<LanguageManager>.Instance.GetInfoByID("minerbonus_lock_tip", new object[]
			{
				this.openData.needMinor,
				this.openData.currentMinor,
				this.openData.needMinor
			});
		}

		private void ShowSpine()
		{
			this.lockObj.SetActive(false);
			this.btnStartSpin.gameObject.SetActive(true);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.openData != null)
			{
				Action onCloseUI = this.openData.onCloseUI;
				if (onCloseUI == null)
				{
					return;
				}
				onCloseUI();
			}
		}

		public override void OnDelete()
		{
			this.pointerCollider.Clear();
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickBonus));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnBg.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
			this.btnStartSpin.button.onClick.AddListener(new UnityAction(this.OnBtnStartSpinClick));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnBg.onClick.RemoveListener(new UnityAction(this.OnBtnCloseClick));
			this.btnStartSpin.button.onClick.RemoveListener(new UnityAction(this.OnBtnStartSpinClick));
		}

		private void EnableNailsCollider(bool enable)
		{
			Collider2D[] componentsInChildren = this.nodeNails.GetComponentsInChildren<Collider2D>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = enable;
				}
			}
		}

		private void OnBtnCloseClick()
		{
			if (base.gameObject == null)
			{
				return;
			}
			if (this.remainSpinTimes > 0)
			{
				return;
			}
			if (this.isSpinning)
			{
				return;
			}
			this.CloseView();
		}

		private void CloseView()
		{
			EventCloseMiniGameUI eventCloseMiniGameUI = new EventCloseMiniGameUI();
			eventCloseMiniGameUI.SetData(MiniGameType.Turntable, this.historyList);
			GameApp.Event.DispatchNow(this, 375, eventCloseMiniGameUI);
			GameApp.View.CloseView(ViewName.FortuneWheelViewModule, null);
		}

		private void OnBtnStartSpinClick()
		{
			if (this.remainSpinTimes <= 0 && !this.isSpinning)
			{
				this.CloseView();
				return;
			}
			if (this.isSpinning)
			{
				return;
			}
			this.EnablePointer(true);
			this.btnStartSpin.SetLock(true);
			this.remainSpinTimes--;
			this.isSpinning = true;
			this.isWaitSendReward = true;
			this.CalcResult();
			this.EnableNailsCollider(true);
			this.wheelAnimator.Play("SpinStart", 0, 0f);
			float animationLength = DxxTools.Animator.GetAnimationLength(this.wheelAnimator, "SpinStart");
			base.StartCoroutine(this.ActiveWheelAutoSpin(animationLength));
		}

		private IEnumerator ActiveWheelAutoSpin(float duration)
		{
			yield return new WaitForSeconds(duration);
			float timer = 0f;
			this.wheelAnimator.Play("SpinLoop", 0, 0f);
			this.wheelRect.transform.localRotation = Quaternion.Euler(0f, 0f, this.randAngle);
			float loopDuration = DxxTools.Animator.GetAnimationLength(this.wheelAnimator, "SpinLoop");
			float randomTime = Random.Range(1f, 1.5f);
			yield return new WaitForSeconds(loopDuration);
			timer += loopDuration;
			while (timer < randomTime)
			{
				timer += Time.deltaTime;
				yield return 0;
			}
			this.endAnimName = FortuneWheelViewModule.EndAnimationsNames;
			this.wheelAnimator.Play(this.endAnimName, 0, 0f);
			float animationLength = DxxTools.Animator.GetAnimationLength(this.wheelAnimator, this.endAnimName);
			yield return new WaitForSeconds(animationLength);
			this.isSpinning = false;
			this.EnableNailsCollider(false);
			this.EnablePointer(false);
			if (this.isWaitSendReward)
			{
				GameApp.Sound.PlayClip(260020, 1f);
				this.btnStartSpin.SetLock(this.remainSpinTimes <= 0);
				this.SendReward();
			}
			yield break;
		}

		private void SendReward()
		{
			this.isWaitSendReward = false;
			try
			{
				this.EnablePointer(false);
				WheelSymbolData wheelSymbolData = this.rewardData;
				string[] param = wheelSymbolData.cfg.param;
				if (param[0] == "1")
				{
					NodeAttParam nodeAttParam = MiniGameUtils.GetNodeAttParam(param, ChapterDropSource.Event, this.openData.rewardRate);
					GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
					Singleton<GameEventController>.Instance.MergerAttribute(nodeAttParam);
					GameEventMiniGameData gameEventMiniGameData = new GameEventMiniGameData();
					gameEventMiniGameData.SetData(MiniGameType.Turntable, nodeAttParam, wheelSymbolData.cfg.id, MiniGameResult.None);
					this.historyList.Add(gameEventMiniGameData);
					RewardAttributeViewModule.OpenData openData = new RewardAttributeViewModule.OpenData();
					openData.nodeAttParam = nodeAttParam;
					openData.viewCloseCallback = new Action(this.OnBtnCloseClick);
					GameApp.View.OpenView(ViewName.RewardAttributeViewModule, openData, 1, null, null);
				}
				else if (param[0] == "2")
				{
					GameEventSkillBuildData skillBuildData = MiniGameUtils.GetSkillBuildData(param, this.openData.randSeed);
					GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { skillBuildData }, true);
					GameEventMiniGameData gameEventMiniGameData2 = new GameEventMiniGameData();
					gameEventMiniGameData2.SetData(MiniGameType.Turntable, skillBuildData, wheelSymbolData.cfg.id, MiniGameResult.None);
					this.historyList.Add(gameEventMiniGameData2);
					GameEventSlotSkillViewModule.OpenData openData2 = new GameEventSlotSkillViewModule.OpenData();
					openData2.skillBuild = skillBuildData;
					openData2.viewCloseCallback = new Action(this.OnBtnCloseClick);
					openData2.seed = this.openData.randSeed;
					GameApp.View.OpenView(ViewName.GameEventSlotSkillViewModule, openData2, 1, null, null);
				}
				else if (param[0] == "3")
				{
					NodeItemParam nodeItemParam = MiniGameUtils.GetNodeItemParam(param, NodeItemType.Item, ChapterDropSource.Event, this.openData.rewardRate);
					GameTGATools.Ins.AddStageClickTempItem(new List<NodeItemParam> { nodeItemParam }, true);
					Singleton<GameEventController>.Instance.AddDrop(nodeItemParam);
					GameEventMiniGameData gameEventMiniGameData3 = new GameEventMiniGameData();
					gameEventMiniGameData3.SetData(MiniGameType.Turntable, nodeItemParam, wheelSymbolData.cfg.id, MiniGameResult.None);
					this.historyList.Add(gameEventMiniGameData3);
					DxxTools.UI.OpenRewardCommon(nodeItemParam.ToItemData(), new Action(this.OnBtnCloseClick), false);
				}
				else
				{
					GameEventMiniGameData gameEventMiniGameData4 = new GameEventMiniGameData();
					gameEventMiniGameData4.SetData(MiniGameType.Turntable, null, wheelSymbolData.cfg.id, MiniGameResult.None);
					this.historyList.Add(gameEventMiniGameData4);
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("turntable_reward_empty"));
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		private void CalcResult()
		{
			this.rewardData = null;
			this.randResultIndex = RandUtils.GetWeightedRandomSelection(this.weightList, this.xRandom);
			this.rewardData = this.dataList[this.randResultIndex];
			float offsetAngle = this.rewardData.offsetAngle;
			float num = this.rewardData.offsetAngle + this.dataList[this.randResultIndex].areaAngle;
			this.randAngle = Random.Range(offsetAngle, num);
		}

		private void InitWheel(int turntableId)
		{
			this.EnablePointer(false);
			ChapterMiniGame_turntableBase elementById = GameApp.Table.GetManager().GetChapterMiniGame_turntableBaseModelInstance().GetElementById(turntableId);
			this.remainSpinTimes = elementById.count;
			this.defaultWheelOffsetAngle = elementById.offsetAngle;
			this.wheelRect.transform.localRotation = Quaternion.Euler(0f, 0f, this.defaultWheelOffsetAngle);
			List<int> list = new List<int>(elementById.rewards);
			this.dataList = new List<WheelSymbolData>();
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				ChapterMiniGame_turntableReward elementById2 = GameApp.Table.GetManager().GetChapterMiniGame_turntableRewardModelInstance().GetElementById(list[i]);
				num += (float)elementById2.weight;
				this.weightList.Add(elementById2.weight);
				num2 += (float)elementById2.showWeight;
				WheelSymbolData wheelSymbolData = new WheelSymbolData(elementById2, i);
				this.dataList.Add(wheelSymbolData);
			}
			float num3 = 0f;
			for (int j = 0; j < this.dataList.Count; j++)
			{
				WheelSymbolData wheelSymbolData2 = this.dataList[j];
				wheelSymbolData2.Calc(num, num2, num3);
				WheelSymbol wheelSymbol = Object.Instantiate<WheelSymbol>(this.wheelSymbolItem, this.wheelSymbolItem.transform.parent);
				wheelSymbol.gameObject.SetActive(true);
				Transform transform = wheelSymbol.transform;
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				transform.localRotation = Quaternion.Euler(0f, 0f, -num3);
				wheelSymbol.SetData(wheelSymbolData2);
				this.itemList.Add(wheelSymbol);
				num3 += wheelSymbolData2.areaAngle;
			}
		}

		private void EnablePointer(bool enable)
		{
			this.pointerCollider.SetColliderEnable(enable);
		}

		private void OnPointerCollide()
		{
			GameApp.Sound.PlayClip(260019, 1f);
		}

		private void OnClickBonus()
		{
			GameApp.View.CloseView(ViewName.FortuneWheelViewModule, null);
		}

		[GameTestMethod("事件小游戏", "幸运轮盘1", "", 1)]
		private static void OnOpenFortuneWheel1()
		{
			FortuneWheelViewModule.OnOpenFortuneWheel(1, 60001, 1);
		}

		[GameTestMethod("事件小游戏", "幸运轮盘2", "", 2)]
		private static void OnOpenFortuneWheel2()
		{
			FortuneWheelViewModule.OnOpenFortuneWheel(2, 0, 1);
		}

		[GameTestMethod("事件小游戏", "幸运轮盘3", "", 3)]
		private static void OnOpenFortuneWheel3()
		{
			FortuneWheelViewModule.OnOpenFortuneWheel(3, 0, 1);
		}

		private static void OnOpenFortuneWheel(int turntableId, int seed = 0, int rate = 1)
		{
			FortuneWheelViewModule.OpenData openData = new FortuneWheelViewModule.OpenData();
			openData.turntableId = turntableId;
			openData.randSeed = seed;
			openData.rewardRate = rate;
			GameApp.View.OpenView(ViewName.FortuneWheelViewModule, openData, 1, null, null);
		}

		public int resultAnimationIndex;

		public bool canStop;

		public WheelSymbol wheelSymbolItem;

		public Animator wheelAnimator;

		public RectTransform wheelRect;

		public Animator pointerAnimator;

		public UIOneButton4MiniGame btnStartSpin;

		public CustomButton btnBg;

		public Transform nodeNails;

		public PointerCollider pointerCollider;

		public GameObject lockObj;

		public CustomText textLock;

		public CustomButton buttonClose;

		private static readonly string EndAnimationsNames = "SpinEnd1";

		private float defaultWheelOffsetAngle;

		private int remainSpinTimes;

		private bool isSpinning;

		private bool isWaitSendReward;

		private string endAnimName;

		private XRandom xRandom;

		private FortuneWheelViewModule.OpenData openData;

		private List<GameEventMiniGameData> historyList = new List<GameEventMiniGameData>();

		private WheelSymbolData rewardData;

		private float randAngle;

		private int randResultIndex;

		private List<WheelSymbolData> dataList = new List<WheelSymbolData>();

		private List<WheelSymbol> itemList = new List<WheelSymbol>();

		private List<int> weightList = new List<int>();

		public class OpenData
		{
			public int turntableId;

			public int randSeed = 100;

			public int rewardRate = 1;

			public bool isBonusGame;

			public int needMinor;

			public int currentMinor;

			public Action onCloseUI;
		}
	}
}
