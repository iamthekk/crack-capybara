using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class SlotMachineViewModule : BaseViewModule
	{
		public bool spinAnimationEnded { get; private set; } = true;

		public override void OnCreate(object data)
		{
			this.betFlyEndPos = this.betNode.position;
			this.betNode.gameObject.SetActive(false);
			this.oneButtonCtrl.Init();
			this.ClearWinText();
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("slot_spin");
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("slot_spin");
			this.oneButtonCtrl.SetTextUp(infoByID);
			this.oneButtonCtrl.SetTextDown(infoByID2);
			this.bet3Effect.SetActive(false);
			this.imageLoading.fillAmount = 0f;
			this.imageLoadingBigWin.fillAmount = 0f;
		}

		public override void OnOpen(object data)
		{
			this.openData = data as SlotMachineViewModule.OpenData;
			if (this.openData == null)
			{
				this.OnForceClose();
				return;
			}
			this.inAutoStart = this.openData.isInSweep;
			SlotMachineViewModule.xRandom = new XRandom(this.openData.randSeed);
			this.symbolIds.Clear();
			ChapterMiniGame_slotBase elementById = GameApp.Table.GetManager().GetChapterMiniGame_slotBaseModelInstance().GetElementById(this.openData.slotMachineId);
			this.spinTimes = elementById.spinTimes;
			for (int i = 0; i < elementById.slotIds.Length; i++)
			{
				this.symbolIds.Add(elementById.slotIds[i]);
			}
			this.weightKeyList.Add(0);
			this.weightValueList.Add(elementById.weight0);
			for (int j = 0; j < this.symbolIds.Count; j++)
			{
				int num = this.symbolIds[j];
				ChapterMiniGame_slotReward elementById2 = GameApp.Table.GetManager().GetChapterMiniGame_slotRewardModelInstance().GetElementById(num);
				this.weightKeyList.Add(num * 100 + 2);
				this.weightValueList.Add(elementById2.weight2);
				this.weightKeyList.Add(num * 100 + 3);
				this.weightValueList.Add(elementById2.weight3);
			}
			this.CreateSlotSymbolsObject();
			this.SetRandomSlotState();
			base.StartCoroutine(this.AutoStartTrigger());
		}

		private IEnumerator AutoStartTrigger()
		{
			if (this.openData.isInSweep)
			{
				ShortcutExtensions46.DOColor(this.imageLoading, this.normalColor, 0f);
				ShortcutExtensions46.DOFillAmount(this.imageLoading, 1f, this.openData.autoStartDelay);
				yield return new WaitForSeconds(this.openData.autoStartDelay);
				this.imageLoading.fillAmount = 0f;
				if (!this.inAutoStart || this.spinning || this.spinTimes <= 0)
				{
					yield break;
				}
				this.OnBtnSpinClickImpl(true);
			}
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.oneButtonCtrl.DeInit();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.oneButtonCtrl.SetData(new Action(this.OnBtnSpinClick));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.oneButtonCtrl.SetData(null);
		}

		private void OnForceClose()
		{
			GameApp.View.CloseView(ViewName.SlotMachineViewModule, null);
			EventCloseMiniGameUI eventCloseMiniGameUI = new EventCloseMiniGameUI();
			eventCloseMiniGameUI.SetData(MiniGameType.MiniSlot, this.historyList);
			GameApp.Event.DispatchNow(this, 375, eventCloseMiniGameUI);
		}

		private void CalcResults()
		{
			this.resultIndexList.Clear();
			this.resultIdList.Clear();
			int weightedRandomSelection = RandUtils.GetWeightedRandomSelection(this.weightValueList, SlotMachineViewModule.xRandom);
			int num = this.weightKeyList[weightedRandomSelection] / 100;
			int num2 = this.weightKeyList[weightedRandomSelection] % 100;
			if (num2 == 0)
			{
				int symbolViewRandomIndex = this.GetSymbolViewRandomIndex();
				int num3 = this.GetSymbolViewRandomIndex();
				int num4 = 1000;
				while (num3 == symbolViewRandomIndex && num4 > 0)
				{
					num3 = this.GetSymbolViewRandomIndex();
					num4--;
				}
				int symbolViewRandomIndex2 = this.GetSymbolViewRandomIndex();
				this.resultIndexList.Add(symbolViewRandomIndex);
				this.resultIndexList.Add(num3);
				this.resultIndexList.Add(symbolViewRandomIndex2);
			}
			else if (num2 == 2)
			{
				this.resultIndexList.Add(this.symbolIds.IndexOf(num));
				this.resultIndexList.Add(this.symbolIds.IndexOf(num));
				int num5 = this.GetSymbolViewRandomIndex();
				int num6 = 1000;
				while (num5 == this.resultIndexList[0] && num6 > 0)
				{
					num5 = this.GetSymbolViewRandomIndex();
					num6--;
				}
				this.resultIndexList.Add(num5);
			}
			else if (num2 == 3)
			{
				this.resultIndexList.Add(this.symbolIds.IndexOf(num));
				this.resultIndexList.Add(this.symbolIds.IndexOf(num));
				this.resultIndexList.Add(this.symbolIds.IndexOf(num));
			}
			for (int i = 0; i < this.resultIndexList.Count; i++)
			{
				this.resultIdList.Add(this.GetSlotSymbolId(this.resultIndexList[i]));
			}
		}

		public void StartSpin()
		{
			this.uiSlotMachineAnimator.Play("Run");
			this.ClearWinText();
			this.slotBarList[2].speedBet = 1f;
			this.spinTimes--;
			this.CalcResults();
			this.imgHighLight.gameObject.SetActive(false);
			this.spinAnimationEnded = false;
			this.spinning = true;
			this.spinCompleted = false;
			for (int i = 0; i < this.slotBarList.Count; i++)
			{
				this.slotBarList[i].SetResultSymbol(this.resultIndexList[i]);
			}
			base.StartCoroutine(this.PlaySlotStart());
		}

		private int GetSlotSymbolId(int resultIndex)
		{
			return this.symbolIds[resultIndex];
		}

		private IEnumerator PlaySlotStart()
		{
			int num = this.resultIdList[0];
			GameApp.Table.GetManager().GetChapterMiniGame_slotRewardModelInstance().GetElementById(num);
			this.slotBarList[0].ActivateSpin(0f);
			this.slotBarList[1].ActivateSpin(0.25f);
			this.slotBarList[2].ActivateSpin(0.5f);
			float num2 = Random.Range(0.5f, 2f);
			this.slotBarList[0].EndSpin("StopBar_01", num2);
			yield break;
		}

		private void OnBtnSpinClick()
		{
			this.OnBtnSpinClickImpl(false);
		}

		private void OnBtnSpinClickImpl(bool isAutoClick = false)
		{
			if (this.spinning || !this.spinCompleted || !this.spinAnimationEnded)
			{
				return;
			}
			if (this.spinTimes <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("slot_machine_spin_unenough"));
				this.OnForceClose();
				return;
			}
			this.oneButtonCtrl.SetLock(true);
			if (!isAutoClick)
			{
				this.inAutoStart = false;
			}
			ShortcutExtensions.DOKill(this.imageLoading, false);
			this.imageLoading.fillAmount = 0f;
			this.StartSpin();
		}

		public void SlotBarAnimComplete(int barId)
		{
			if (barId == 1)
			{
				this.SlotBar1Complete();
				return;
			}
			if (barId == 2)
			{
				this.SlotBar2Complete();
				return;
			}
			if (barId == 3)
			{
				this.SlotBar3Complete();
			}
		}

		private void SlotBar1Complete()
		{
			GameApp.Sound.PlayClip(62, 1f);
			if (GameApp.Table.GetManager().GetChapterMiniGame_slotRewardModelInstance().GetElementById(this.resultIdList[0])
				.symbolType == 1)
			{
				string text = ((Random.Range(0f, 1f) > 0.5f) ? "StopBar_02" : "StopBar_03");
				this.slotBarList[1].EndSpin(text, 0.5f);
				return;
			}
			string text2 = "StopBar_01";
			this.slotBarList[1].EndSpin(text2, 0f);
		}

		private void SlotBar2Complete()
		{
			GameApp.Sound.PlayClip(62, 1f);
			ChapterMiniGame_slotReward elementById = GameApp.Table.GetManager().GetChapterMiniGame_slotRewardModelInstance().GetElementById(this.resultIdList[1]);
			if (this.resultIndexList[0] != this.resultIndexList[1])
			{
				string text = ((Random.Range(0f, 1f) > 0.5f) ? "StopBar_01" : "StopBar_02");
				this.slotBarList[2].EndSpin(text, 0.5f);
				return;
			}
			string[] reward = elementById.reward2;
			string reward2txt = elementById.reward2txt;
			if (reward[0] == "1")
			{
				NodeAttParam nodeAttParam = MiniGameUtils.GetNodeAttParam(reward, ChapterDropSource.TinySlot, this.openData.rewardRate);
				this.winIcon.gameObject.SetActive(true);
				this.winIcon.SetImage(elementById.atlasRegular, elementById.iconRegular);
				this.winText.text = NodeAttParam.GetAttParam(nodeAttParam).m_value;
			}
			else if (reward[0] == "2")
			{
				this.winIcon.gameObject.SetActive(true);
				this.winIcon.SetImage(elementById.atlasRegular, elementById.iconRegular);
				this.winText.text = Singleton<LanguageManager>.Instance.GetInfoByID(reward2txt);
			}
			else if (reward[0] == "3")
			{
				NodeItemParam nodeItemParam = MiniGameUtils.GetNodeItemParam(reward, NodeItemType.Item, ChapterDropSource.TinySlot, this.openData.rewardRate);
				this.winIcon.gameObject.SetActive(true);
				this.winIcon.SetImage(elementById.atlasRegular, elementById.iconRegular);
				this.winText.text = NodeItemParam.GetNodeItemRewardText(nodeItemParam);
			}
			ShortcutExtensions.DOPunchScale(this.winTextScaleNode.transform, 1.2f * Vector3.one, 0.4f, 10, 1f);
			this.winTextEffectAnimator.Play("Show");
			string text2 = ((Random.Range(0f, 1f) > 0.5f) ? "StopBar_04" : "StopBar_05");
			if (elementById.symbolType == 1)
			{
				GameApp.Sound.PlayClip(63, 1f);
				this.imgHighLight.gameObject.SetActive(true);
				this.slotBarList[2].speedBet = 1.5f;
				this.slotBarList[2].EndSpin(text2, 3f);
				return;
			}
			this.slotBarList[2].EndSpin(text2, 1.5f);
		}

		private void SlotBar3Complete()
		{
			base.StartCoroutine(this.PlayRewardBetAnimation());
		}

		private IEnumerator PlayRewardBetAnimation()
		{
			GameApp.Sound.PlayClip(62, 1f);
			if (this.resultIndexList[0] == this.resultIndexList[1] && this.resultIndexList[0] == this.resultIndexList[2])
			{
				this.betNode.transform.localScale = Vector3.one * 3f;
				this.betNode.gameObject.SetActive(true);
				TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.betNode.transform, Vector3.one * 1.5f, 0.3f), 5));
				yield return new WaitForSeconds(0.3f);
				GameApp.Sound.PlayClip(129, 1f);
				this.bet3Effect.SetActive(true);
				base.StartCoroutine(this.WaitToActivateWinSequence());
				yield return new WaitForSeconds(0.2f);
				this.spinning = false;
			}
			else
			{
				base.StartCoroutine(this.WaitToActivateWinSequence());
				yield return new WaitForSeconds(0.2f);
				this.spinning = false;
			}
			yield break;
		}

		private IEnumerator WaitToActivateWinSequence()
		{
			yield return null;
			this.ActivateWinSequence();
			this.spinAnimationEnded = true;
			yield break;
		}

		private void ActivateWinSequence()
		{
			try
			{
				int slotSymbolId = this.GetSlotSymbolId(this.resultIndexList[0]);
				ChapterMiniGame_slotReward elementById = GameApp.Table.GetManager().GetChapterMiniGame_slotRewardModelInstance().GetElementById(slotSymbolId);
				string[] array = ((this.resultIndexList[0] == this.resultIndexList[2]) ? elementById.reward3 : elementById.reward2);
				string text = ((this.resultIndexList[0] == this.resultIndexList[2]) ? elementById.reward3txt : elementById.reward2txt);
				int num4 = int.Parse(array[2]);
				ShortcutExtensions.DOPunchScale(this.winTextScaleNode.transform, 1.2f * Vector3.one, 0.4f, 10, 1f);
				MiniGameResult miniGameResult;
				if (this.resultIndexList[0] == this.resultIndexList[1] && this.resultIndexList[0] == this.resultIndexList[2])
				{
					miniGameResult = MiniGameResult.GearThree;
				}
				else if (this.resultIndexList[0] == this.resultIndexList[1])
				{
					miniGameResult = MiniGameResult.GearTwo;
				}
				else
				{
					miniGameResult = MiniGameResult.GearOne;
				}
				if (this.resultIndexList[0] == this.resultIndexList[1])
				{
					this.winIcon.gameObject.SetActive(true);
					this.winIcon.SetImage(elementById.atlasRegular, elementById.iconRegular);
					if (array[0] == "1")
					{
						NodeAttParam nodeAttParam = MiniGameUtils.GetNodeAttParam(array, ChapterDropSource.TinySlot, this.openData.rewardRate);
						GameEventMiniGameData gameEventMiniGameData = new GameEventMiniGameData();
						gameEventMiniGameData.SetData(MiniGameType.MiniSlot, nodeAttParam, elementById.id, miniGameResult);
						this.historyList.Add(gameEventMiniGameData);
						if (this.resultIndexList[0] == this.resultIndexList[2])
						{
							nodeAttParam.SetNum((double)num4);
							GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
							Singleton<GameEventController>.Instance.MergerAttribute(nodeAttParam);
							NodeAttParam cacheNodeAttParam2 = nodeAttParam.Clone();
							this.winTextEffectAnimator.Play("Show");
							int value2 = 0;
							int num2 = this.spinTimes;
							TweenSettingsExtensions.OnComplete<Tweener>(DOTween.To(() => value2, delegate(int num)
							{
								cacheNodeAttParam2.SetNum((double)num);
								this.winText.text = NodeAttParam.GetAttParam(cacheNodeAttParam2).m_value;
							}, num4, 0.5f), delegate
							{
								RewardAttributeViewModule.OpenData openData3 = new RewardAttributeViewModule.OpenData();
								openData3.nodeAttParam = cacheNodeAttParam2;
								openData3.viewCloseCallback = new Action(this.OnForceClose);
								openData3.isInSweep = this.openData.isInSweep;
								openData3.autoCloseTime = 1f;
								GameApp.View.OpenView(ViewName.RewardAttributeViewModule, openData3, 1, null, null);
							});
						}
						else
						{
							GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
							Singleton<GameEventController>.Instance.MergerAttribute(nodeAttParam);
							NodeAttParam nodeAttParam2 = nodeAttParam.Clone();
							this.winText.text = NodeAttParam.GetAttParam(nodeAttParam2).m_value;
							RewardAttributeViewModule.OpenData openData = new RewardAttributeViewModule.OpenData();
							openData.nodeAttParam = nodeAttParam2;
							openData.viewCloseCallback = new Action(this.OnForceClose);
							openData.isInSweep = this.openData.isInSweep;
							openData.autoCloseTime = 1f;
							GameApp.View.OpenView(ViewName.RewardAttributeViewModule, openData, 1, null, null);
						}
					}
					else if (array[0] == "2")
					{
						if (this.resultIndexList[0] == this.resultIndexList[2])
						{
							this.winTextEffectAnimator.Play("Show");
							this.winText.text = Singleton<LanguageManager>.Instance.GetInfoByID(text);
						}
						GameEventSkillBuildData skillBuildData = MiniGameUtils.GetSkillBuildData(array, this.openData.randSeed);
						GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { skillBuildData }, true);
						GameEventMiniGameData gameEventMiniGameData2 = new GameEventMiniGameData();
						gameEventMiniGameData2.SetData(MiniGameType.MiniSlot, skillBuildData, elementById.id, miniGameResult);
						this.historyList.Add(gameEventMiniGameData2);
						GameEventSlotSkillViewModule.OpenData openData2 = new GameEventSlotSkillViewModule.OpenData();
						openData2.skillBuild = skillBuildData;
						openData2.viewCloseCallback = new Action(this.OnForceClose);
						openData2.seed = this.openData.randSeed;
						GameApp.View.OpenView(ViewName.GameEventSlotSkillViewModule, openData2, 1, null, null);
					}
					else if (array[0] == "3")
					{
						NodeItemParam nodeItemParam = MiniGameUtils.GetNodeItemParam(array, NodeItemType.Item, ChapterDropSource.TinySlot, this.openData.rewardRate);
						GameEventMiniGameData gameEventMiniGameData3 = new GameEventMiniGameData();
						gameEventMiniGameData3.SetData(MiniGameType.MiniSlot, nodeItemParam, elementById.id, miniGameResult);
						this.historyList.Add(gameEventMiniGameData3);
						if (this.resultIndexList[0] == this.resultIndexList[2])
						{
							nodeItemParam.SetNum(num4);
							GameTGATools.Ins.AddStageClickTempItem(new List<NodeItemParam> { nodeItemParam }, true);
							Singleton<GameEventController>.Instance.AddDrop(nodeItemParam);
							NodeItemParam cacheNodeAttParam = nodeItemParam.Clone();
							this.winTextEffectAnimator.Play("Show");
							int value = 0;
							int num3 = this.spinTimes;
							TweenSettingsExtensions.OnComplete<Tweener>(DOTween.To(() => value, delegate(int num)
							{
								cacheNodeAttParam.SetNum(num);
								this.winText.text = NodeItemParam.GetNodeItemRewardText(cacheNodeAttParam);
							}, num4, 0.5f), delegate
							{
								RewardCommonData rewardCommonData2 = new RewardCommonData();
								rewardCommonData2.list = new List<ItemData> { cacheNodeAttParam.ToItemData() };
								rewardCommonData2.OnClose = new Action(this.OnForceClose);
								rewardCommonData2.m_isFly = false;
								rewardCommonData2.m_isInSweep = this.openData.isInSweep;
								rewardCommonData2.m_autoCloseTime = 1f;
								GameApp.View.OpenView(ViewName.RewardCommonViewModule, rewardCommonData2, 2, null, null);
							});
						}
						else
						{
							GameTGATools.Ins.AddStageClickTempItem(new List<NodeItemParam> { nodeItemParam }, true);
							Singleton<GameEventController>.Instance.AddDrop(nodeItemParam);
							NodeItemParam nodeItemParam2 = nodeItemParam.Clone();
							this.winText.text = NodeItemParam.GetNodeItemRewardText(nodeItemParam2);
							RewardCommonData rewardCommonData = new RewardCommonData();
							rewardCommonData.list = new List<ItemData> { nodeItemParam2.ToItemData() };
							rewardCommonData.OnClose = new Action(this.OnForceClose);
							rewardCommonData.m_isFly = false;
							rewardCommonData.m_isInSweep = this.openData.isInSweep;
							rewardCommonData.m_autoCloseTime = 1f;
							GameApp.View.OpenView(ViewName.RewardCommonViewModule, rewardCommonData, 2, null, null);
						}
					}
					else if (this.spinTimes > 0)
					{
						this.oneButtonCtrl.SetLock(false);
					}
				}
				else
				{
					GameEventMiniGameData gameEventMiniGameData4 = new GameEventMiniGameData();
					gameEventMiniGameData4.SetData(MiniGameType.MiniSlot, null, 0, miniGameResult);
					this.historyList.Add(gameEventMiniGameData4);
					this.ClearWinText();
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("slot_result_noreward"));
					if (this.spinTimes > 0)
					{
						this.oneButtonCtrl.SetLock(false);
					}
					else
					{
						base.Invoke("OnForceClose", 1f);
					}
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			finally
			{
				this.spinCompleted = true;
			}
		}

		private void ClearWinText()
		{
			this.betNode.gameObject.SetActive(false);
			this.winTextScaleNode.alpha = 1f;
			this.winIcon.gameObject.SetActive(false);
			this.winText.text = Singleton<LanguageManager>.Instance.GetInfoByID("slot_lucky_tip");
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.winIcon.transform.parent as RectTransform);
		}

		private void CreateSlotSymbolsObject()
		{
			for (int i = 0; i < this.slotBarList.Count; i++)
			{
				this.slotBarList[i].Init();
				this.slotBarList[i].SetViewModule(this, i + 1);
				this.slotBarList[i].SetUpSymbols(this.symbolIds);
			}
			this.imgHighLight.gameObject.SetActive(false);
		}

		private void SetRandomSlotState()
		{
			for (int i = 0; i < this.slotBarList.Count; i++)
			{
				int symbolViewRandomIndex = this.GetSymbolViewRandomIndex();
				this.slotBarList[i].SetSymbolByIndex(symbolViewRandomIndex);
			}
		}

		private int GetSymbolViewRandomIndex()
		{
			return Random.Range(0, this.symbolIds.Count);
		}

		[GameTestMethod("事件小游戏", "老虎机", "", 201)]
		private static void OpenSlotMachine()
		{
			SlotMachineViewModule.OpenData openData = new SlotMachineViewModule.OpenData();
			openData.slotMachineId = 1;
			openData.randSeed = Random.Range(1, 10000);
			openData.rewardRate = 1;
			openData.isInSweep = true;
			openData.autoStartDelay = 3f;
			GameApp.View.OpenView(ViewName.SlotMachineViewModule, openData, 1, null, null);
		}

		public Animator uiSlotMachineAnimator;

		public UIOneButton4MiniGame oneButtonCtrl;

		public List<SlotBarManager> slotBarList = new List<SlotBarManager>();

		public CanvasGroup winTextScaleNode;

		public CustomImage winIcon;

		public CustomText winText;

		public Animator winTextEffectAnimator;

		public Transform betNode;

		public GameObject bet3Effect;

		public CustomImage imgHighLight;

		public Image imageLoading;

		public Image imageLoadingBigWin;

		public Color normalColor;

		public Color minorWinColor;

		public Color jackpotColor;

		private bool spinCompleted = true;

		private bool spinning;

		private List<int> symbolIds = new List<int>();

		private List<int> weightKeyList = new List<int>();

		private List<int> weightValueList = new List<int>();

		private List<int> resultIndexList = new List<int>();

		private List<int> resultIdList = new List<int>();

		private SlotMachineViewModule.OpenData openData;

		private List<GameEventMiniGameData> historyList = new List<GameEventMiniGameData>();

		private Vector3 betFlyEndPos;

		private int spinTimes;

		private static XRandom xRandom;

		private bool inAutoStart;

		public class OpenData
		{
			public int slotMachineId;

			public int randSeed = 100;

			public int rewardRate = 1;

			public bool isInSweep;

			public float autoStartDelay = 1f;
		}
	}
}
