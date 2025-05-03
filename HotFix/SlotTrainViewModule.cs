using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class SlotTrainViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 947;
		}

		public override void OnCreate(object data)
		{
			for (int i = 0; i < this.nodesParent.transform.childCount; i++)
			{
				GameObject gameObject = this.nodesParent.transform.GetChild(i).gameObject;
				this.nodes.Add(gameObject);
			}
			this.buttonTrade.Init();
			this.buttonTrade.SetData(new Action(this.OnTrade));
			this.buttonTrade.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_Spin"));
			this.buttonTrade.SetLock(false);
			this.buttonMiddleClose.Init();
			this.buttonMiddleClose.SetData(new Action(this.OnCloseSelf));
			this.buttonMiddleClose.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_Back"));
			this.buttonMiddleClose.SetLock(false);
			this.buttonMiddleClose.gameObject.SetActiveSafe(false);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnCloseSelf));
			this.buttonRefresh.onClick.AddListener(new UnityAction(this.OnRefreshReward));
			this.buttonCloseTrans = this.buttonClose.GetComponent<RectTransform>();
			this.slotTrainItemObj.SetActiveSafe(false);
			this.HpAnima = new AttributeHpAnim();
			this.HpAnima.Init(this.tranHpInfo, this.textHp);
			this.HpPercentAnima = new AttributeAnim();
			this.HpPercentAnima.Init(this.tranHpInfo, this.textHpRate, true);
			this.CreateSlotTrainItems();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Close_SlotTrain_Reward, new HandlerEvent(this.OnCloseSlotRewardUI));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Close_SlotTrain_Reward, new HandlerEvent(this.OnCloseSlotRewardUI));
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as SlotTrainViewModule.OpenData;
			if (this.openData == null)
			{
				this.OnCloseSelf();
				return;
			}
			GameApp.Sound.PlayClip(79, 1f);
			this.cacheSkills.Clear();
			this.cacheAttrDic.Clear();
			this.RefreshHP(false);
			if (this.InitRewards())
			{
				this.ResetSlot();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.selectNodeCtrl != null)
			{
				if (this.isAddSpeed)
				{
					this.selectNodeCtrl.SpeedRatio += deltaTime * 3.5f * 0.6f;
					this.selectNodeCtrl.SpeedRatio = Utility.Math.Clamp(this.selectNodeCtrl.SpeedRatio, 0f, 3.5f);
				}
				this.selectNodeCtrl.OnUpdate(deltaTime);
			}
			AttributeHpAnim hpAnima = this.HpAnima;
			if (hpAnima != null)
			{
				hpAnima.OnUpdate(deltaTime);
			}
			AttributeAnim hpPercentAnima = this.HpPercentAnima;
			if (hpPercentAnima == null)
			{
				return;
			}
			hpPercentAnima.OnUpdate(deltaTime);
		}

		public override void OnClose()
		{
			this.sequencePool.Clear(false);
		}

		public override void OnDelete()
		{
			this.buttonTrade.DeInit();
			this.buttonMiddleClose.DeInit();
			this.buttonRefresh.onClick.RemoveListener(new UnityAction(this.OnRefreshReward));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.nodes.Clear();
			for (int i = 0; i < this.slotTrainItems.Count; i++)
			{
				this.slotTrainItems[i].DeInit();
			}
			this.slotTrainItems.Clear();
		}

		private bool InitRewards()
		{
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(this.openData.sourceType, GameEventSlotTrainFactory.SkillCount, this.openData.seed);
			this.slotTrainBuilds = Singleton<GameEventController>.Instance.CreateSlotTrainBuilds(randomSkillList, this.openData.seed);
			if (this.slotTrainItems.Count != this.slotTrainBuilds.Count)
			{
				HLog.LogError("大转盘奖励数量与UI节点数量不一致，请立即检查!");
				return false;
			}
			this.lastIndex = 0;
			this.isAddSpeed = false;
			this.playCount = 0;
			return true;
		}

		private void ResetSlot()
		{
			this.RefreshInfo();
			this.ShowSlots();
			this.CheckTradeButton();
			this.PlayOpenAni();
		}

		private void CreateSlotTrainItems()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.slotTrainItemObj);
				gameObject.SetParentNormal(this.nodes[i], false);
				gameObject.SetActiveSafe(true);
				UISlotTrainItem component = gameObject.GetComponent<UISlotTrainItem>();
				component.Init();
				this.slotTrainItems.Add(component);
			}
		}

		public void RefreshInfo()
		{
			int slotTrainPrice = GameConfig.GetSlotTrainPrice(this.playCount);
			this.hpObj.SetActiveSafe(slotTrainPrice > 0);
			if (slotTrainPrice < 0)
			{
				this.textTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_TipEnd");
				this.priceObj.SetActive(false);
				return;
			}
			if (slotTrainPrice > 0)
			{
				this.textTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_TipPrice", new object[] { slotTrainPrice });
				this.textBtn.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_Spin");
				this.buttonTrade.SetText("");
				long num = Singleton<GameEventController>.Instance.PlayerData.CurrentHp.AsLong();
				long num2 = ((double)(Singleton<GameEventController>.Instance.PlayerData.HpMax.AsLong() * (long)slotTrainPrice) * 0.01).GetValue();
				if (num2 >= num)
				{
					num2 = num - 1L;
				}
				this.textPrice.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_Cost", new object[] { DxxTools.FormatNumber(num2) });
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.priceTrans);
				this.priceObj.SetActive(true);
				return;
			}
			this.textTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_TipFree");
			this.priceObj.SetActive(false);
		}

		private void CheckTradeButton()
		{
			int slotTrainPrice = GameConfig.GetSlotTrainPrice(this.playCount);
			this.buttonTrade.SetLock(slotTrainPrice < 0);
			this.buttonTrade.ShowLoading(false);
			this.buttonTrade.gameObject.SetActiveSafe(slotTrainPrice >= 0);
			this.buttonMiddleClose.gameObject.SetActiveSafe(slotTrainPrice < 0);
		}

		private void ShowSlots()
		{
			for (int i = 0; i < this.slotTrainBuilds.Count; i++)
			{
				this.slotTrainItems[i].Refresh(this.slotTrainBuilds[i], this.showNode);
				int num = i % 6 + 1;
				this.slotTrainItems[i].SetSoundId(num);
			}
		}

		private void RefreshHP(bool useAnimation)
		{
			long num = Singleton<GameEventController>.Instance.PlayerData.CurrentHp.AsLong();
			long num2 = Singleton<GameEventController>.Instance.PlayerData.HpMax.AsLong();
			int num3 = Mathf.Clamp((int)((double)num / (double)num2 * 100.0), 1, 100);
			this.HpAnima.SetHp(num, num2, useAnimation);
			this.HpPercentAnima.SetValue((long)num3);
			this.sliderHP.value = Utility.Math.Clamp01((float)((double)num / (double)num2));
		}

		private void OnRefreshReward()
		{
			if (this.isPlayOpenAni)
			{
				return;
			}
			if (this.isStarAni)
			{
				return;
			}
			if (this.isOpenRewardUI)
			{
				return;
			}
			if (!Singleton<GameEventController>.Instance.BuyRefreshSlotTrain())
			{
				return;
			}
			if (this.InitRewards())
			{
				this.ResetSlot();
			}
		}

		private void OnTrade()
		{
			if (this.isPlayOpenAni)
			{
				return;
			}
			if (this.isStarAni)
			{
				return;
			}
			if (this.isOpenRewardUI)
			{
				return;
			}
			if (this.isTradeBtn)
			{
				return;
			}
			this.isTradeBtn = true;
			DelayCall.Instance.CallOnce(500, delegate
			{
				this.isTradeBtn = false;
			});
			if (this.playCount == 12)
			{
				EventArgsString instance = Singleton<EventArgsString>.Instance;
				instance.SetData(Singleton<LanguageManager>.Instance.GetInfoByID("UISlotTrain_145"));
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance);
				return;
			}
			if (!Singleton<GameEventController>.Instance.BuySlotTrain(this.playCount))
			{
				return;
			}
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonCloseTrans, -800f, 0.1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.buttonCloseTrans.localScale = Vector3.zero;
				this.StartSlotTrain();
				this.RefreshHP(true);
			});
		}

		private void OnCloseSelf()
		{
			if (this.isPlayOpenAni)
			{
				return;
			}
			if (this.isStarAni)
			{
				return;
			}
			if (this.isOpenRewardUI)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.SlotTrainViewModule, null);
			EventArgSelectSurprise eventArgSelectSurprise = new EventArgSelectSurprise();
			eventArgSelectSurprise.SetData(GameEventPushType.CloseSlotTrain, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, eventArgSelectSurprise);
			EventArgSaveSkillAndAttr eventArgSaveSkillAndAttr = new EventArgSaveSkillAndAttr();
			eventArgSaveSkillAndAttr.SetData(this.cacheSkills, this.cacheAttrDic);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, eventArgSaveSkillAndAttr);
		}

		public void PlayOpenAni()
		{
			if (this.isPlayOpenAni)
			{
				return;
			}
			this.isPlayOpenAni = true;
			this.buttonTrade.transform.localScale = Vector3.zero;
			this.buttonRefresh.transform.localScale = Vector3.zero;
			this.buttonClose.transform.localScale = Vector3.zero;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			for (int i = 0; i < 6; i++)
			{
				int index = i;
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.slotTrainItems[index].Show();
					this.slotTrainItems[index + 6].Show();
				});
				TweenSettingsExtensions.AppendInterval(sequence, 0.05f);
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.buttonCloseTrans.localScale = Vector3.one;
				this.buttonCloseTrans.anchoredPosition = new Vector2(-800f, this.buttonCloseTrans.anchoredPosition.y);
			});
			float num = 0.2f;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonTrade.transform, Vector3.one * 1.1f, num)), ShortcutExtensions.DOScale(this.buttonTrade.transform, Vector3.one, num / 2f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonCloseTrans, -414f, num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isPlayOpenAni = false;
			});
		}

		public void StartSlotTrain()
		{
			this.priceObj.SetActive(false);
			this.playCount++;
			this.isStarAni = true;
			GameEventSlotTrainFactory.SlotTrainBuild slotTrainBuild = Singleton<GameEventController>.Instance.RandomSlotTrain();
			this.targetIndex = 0;
			for (int i = 0; i < this.slotTrainBuilds.Count; i++)
			{
				if (slotTrainBuild.id.Equals(this.slotTrainBuilds[i].id))
				{
					this.targetIndex = i;
					break;
				}
			}
			this.buttonTrade.SetLock(true);
			this.buttonTrade.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_Spinning"));
			if (this.playCount == 12)
			{
				for (int j = 0; j < this.slotTrainItems.Count; j++)
				{
					if (j == this.targetIndex)
					{
						this.slotTrainItems[this.targetIndex].SelectNodeEnter(false);
						this.slotTrainItems[this.targetIndex].SelectNodeSelect(false);
					}
					else
					{
						this.slotTrainItems[j].SelectNodeExit(false);
					}
				}
				this.EndSlotTrain(this.targetIndex);
				return;
			}
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.selectNodeCtrl = new SlotTrainSelectNodeCtrl(this.slotTrainItems, new Action<int>(this.EndSlotTrain));
				this.selectNodeCtrl.StartAtIndex(this.lastIndex, 1);
				this.isAddSpeed = true;
			});
			TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.selectNodeCtrl.StopAtIndex(this.targetIndex);
			});
		}

		private void EndSlotTrain(int index)
		{
			this.isStarAni = false;
			this.lastIndex = index;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.buttonTrade.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventSlotTrain_Spin"));
				this.ShowResult();
				this.RefreshInfo();
				this.ResetMask();
				this.CheckTradeButton();
			});
		}

		public void ResetMask()
		{
			for (int i = 0; i < this.slotTrainItems.Count; i++)
			{
				this.slotTrainItems[i].ResetMask();
			}
		}

		public void SkipAnimation()
		{
			for (int i = 0; i < this.slotTrainItems.Count - 1; i++)
			{
				this.slotTrainItems[i].Stop();
			}
			this.slotTrainItems[this.targetIndex].SelectNodeSelect(true);
		}

		private void ShowResult()
		{
			SlotTrainRewardViewModule.OpenData openData = new SlotTrainRewardViewModule.OpenData();
			if (this.slotTrainBuilds[this.targetIndex].IsSkill)
			{
				openData.skillBuild = this.slotTrainBuilds[this.targetIndex].skillBuild;
			}
			else
			{
				openData.atlasId = this.slotTrainBuilds[this.targetIndex].Config.atlas;
				openData.icon = this.slotTrainBuilds[this.targetIndex].Config.icon;
			}
			GameEventSlotTrainFactory.SlotTrainBuild slotTrainBuild = this.slotTrainBuilds[this.targetIndex];
			openData.showInfo = SlotTrainViewModule.GetAttributeInfo(slotTrainBuild.slotTrainType, slotTrainBuild.param);
			switch (slotTrainBuild.slotTrainType)
			{
			case SlotTrainType.RecoverHp:
			{
				NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.RecoverHpRate, (double)slotTrainBuild.param, ChapterDropSource.Event, 1);
				GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
				Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
				break;
			}
			case SlotTrainType.Attack:
			{
				NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.AttackPercent, (double)slotTrainBuild.param, ChapterDropSource.Event, 1);
				GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
				Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
				this.cacheAttrDic.Add("Attack%", slotTrainBuild.param);
				break;
			}
			case SlotTrainType.Defense:
			{
				NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.DefencePercent, (double)slotTrainBuild.param, ChapterDropSource.Event, 1);
				GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
				Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
				this.cacheAttrDic.Add("Defence%", slotTrainBuild.param);
				break;
			}
			case SlotTrainType.MaxHp:
			{
				NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.HPMaxPercent, (double)slotTrainBuild.param, ChapterDropSource.Event, 1);
				GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
				Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
				this.cacheAttrDic.Add("HPMax%", slotTrainBuild.param);
				break;
			}
			case SlotTrainType.Skill:
				Singleton<GameEventController>.Instance.SelectSkill(slotTrainBuild.skillBuild, false);
				GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { slotTrainBuild.skillBuild }, true);
				this.cacheSkills.Add(slotTrainBuild.skillBuild);
				break;
			}
			this.RefreshHP(true);
			this.isOpenRewardUI = true;
			GameApp.View.OpenView(ViewName.SlotTrainRewardViewModule, openData, 1, null, null);
		}

		private void OnCloseSlotRewardUI(object sender, int type, BaseEventArgs eventargs)
		{
			Sequence sequence = this.sequencePool.Get();
			this.buttonCloseTrans.localScale = Vector3.one;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonCloseTrans, -414f, 0.1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isOpenRewardUI = false;
			});
		}

		public static string GetAttributeName(SlotTrainType slotTrainType)
		{
			string text = "";
			switch (slotTrainType)
			{
			case SlotTrainType.RecoverHp:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_RestoreHP");
				break;
			case SlotTrainType.Attack:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_16");
				break;
			case SlotTrainType.Defense:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_37");
				break;
			case SlotTrainType.MaxHp:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_18");
				break;
			}
			return text;
		}

		public static string GetAttributeInfo(SlotTrainType slotTrainType, int attParam)
		{
			string attributeName = SlotTrainViewModule.GetAttributeName(slotTrainType);
			string text = string.Format("{0}+{1}%", attributeName, attParam);
			if (slotTrainType == SlotTrainType.RecoverHp)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_149", new object[] { attParam.ToString() + "%" });
			}
			return text;
		}

		public static int GetItemIndex(int index, int itemsCount)
		{
			if (itemsCount <= 0)
			{
				return 0;
			}
			while (index < 0)
			{
				index += itemsCount;
			}
			return index % itemsCount;
		}

		[GameTestMethod("事件小游戏", "水果机", "", 301)]
		private static void OpenSlotTrain()
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			SlotTrainViewModule.OpenData openData = new SlotTrainViewModule.OpenData();
			openData.seed = random.Next();
			GameApp.View.OpenView(ViewName.SlotTrainViewModule, openData, 1, null, null);
		}

		public GameObject slotTrainItemObj;

		public UIOneButtonCtrl buttonTrade;

		public UIOneButtonCtrl buttonMiddleClose;

		public CustomButton buttonClose;

		public GameObject nodesParent;

		public GameObject showNode;

		public CustomButton buttonRefresh;

		public CustomText textTip;

		public CustomText textHp;

		public CustomText textHpRate;

		public Slider sliderHP;

		public Transform tranHpInfo;

		public GameObject hpObj;

		public GameObject priceObj;

		public CustomText textBtn;

		public CustomText textPrice;

		public RectTransform priceTrans;

		private List<GameObject> nodes = new List<GameObject>();

		private List<UISlotTrainItem> slotTrainItems = new List<UISlotTrainItem>();

		private List<GameEventSlotTrainFactory.SlotTrainBuild> slotTrainBuilds;

		private SlotTrainSelectNodeCtrl selectNodeCtrl;

		private SequencePool sequencePool = new SequencePool();

		private List<GameEventSkillBuildData> cacheSkills = new List<GameEventSkillBuildData>();

		private Dictionary<string, int> cacheAttrDic = new Dictionary<string, int>();

		public const float MAX_SPIN_SPEED_RATIO = 3.5f;

		public const float SPIN_ACCELERATE_RATIO = 0.6f;

		public const float STOP_DURATION = 0.2f;

		private bool isAddSpeed;

		private int targetIndex;

		private int lastIndex;

		private bool isStarAni;

		private bool isTradeBtn;

		private int playCount;

		private bool isOpenRewardUI;

		private bool isPlayOpenAni;

		private RectTransform buttonCloseTrans;

		private SlotTrainViewModule.OpenData openData;

		private AttributeHpAnim HpAnima;

		private AttributeAnim HpPercentAnima;

		public class OpenData
		{
			public int seed;

			public SkillBuildSourceType sourceType = SkillBuildSourceType.SlotTrain;
		}
	}
}
