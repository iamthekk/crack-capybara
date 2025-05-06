using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Tower;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleRogueDungeonViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			this.buttonMonsterSkill.onClick.AddListener(new UnityAction(this.OnClickMonsterSkill));
			this.buttonEscape.onClick.AddListener(new UnityAction(this.OnClickEscape));
			this.attrCtrl.Init();
			this.skillCtrl.Init();
			this.copyPointItem.SetActiveSafe(false);
			this.copyPointBgItem.SetActiveSafe(false);
			this.buttonSpeedRT = this.speedButtonCtrl.gameObject.GetComponent<RectTransform>();
			this.nextFloorAni.Init();
			this.roundObj.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			this.befourBattleAtk = 0L;
			this.befourBattleDef = 0L;
			this.nextFloorAni.gameObject.SetActiveSafe(false);
			this.speedButtonCtrl.SetData(UISpeedButtonCtrl.SpeedType.Main);
			this.RefreshInfo();
			this.RefreshRound(0, 15);
			this.RefreshAttr(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.attrCtrl)
			{
				this.attrCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.attrCtrl.DeInit();
			this.skillCtrl.DeInit();
			this.buttonMonsterSkill.onClick.RemoveListener(new UnityAction(this.OnClickMonsterSkill));
			this.buttonEscape.onClick.RemoveListener(new UnityAction(this.OnClickEscape));
			this.sequencePool.Clear(false);
			for (int i = 0; i < this.pointItems.Count; i++)
			{
				UIProgressPointItem uiprogressPointItem = this.pointItems[i];
				if (uiprogressPointItem)
				{
					uiprogressPointItem.DeInit();
				}
			}
			this.pointItems.Clear();
			for (int j = 0; j < this.pointBgItems.Count; j++)
			{
				UIProgressPointBgItem uiprogressPointBgItem = this.pointBgItems[j];
				if (uiprogressPointBgItem)
				{
					uiprogressPointBgItem.DeInit();
				}
			}
			this.pointBgItems.Clear();
			this.nextFloorAni.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnEventRefreshRound));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattleStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBattleEnd));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_RogueDungeon_NextFloorStart, new HandlerEvent(this.OnEventNextFloorStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_RogueDungeon_NextFloorEnd, new HandlerEvent(this.OnEventNextFloorEnd));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshAttribute, new HandlerEvent(this.OnRefreshAttribute));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIBattleRogueDungeon_Refresh, new HandlerEvent(this.OnRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIBattleRogueDungeon_ShowHideRound, new HandlerEvent(this.OnShowHideRound));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnEventRefreshRound));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattleStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBattleEnd));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_RogueDungeon_NextFloorStart, new HandlerEvent(this.OnEventNextFloorStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_RogueDungeon_NextFloorEnd, new HandlerEvent(this.OnEventNextFloorEnd));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshAttribute, new HandlerEvent(this.OnRefreshAttribute));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIBattleRogueDungeon_Refresh, new HandlerEvent(this.OnRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIBattleRogueDungeon_ShowHideRound, new HandlerEvent(this.OnShowHideRound));
		}

		private void RefreshInfo()
		{
			RogueDungeon_rogueDungeon rogueDungeon_rogueDungeon = GameApp.Table.GetManager().GetRogueDungeon_rogueDungeon((int)this.mDataModule.CurrentFloorID);
			if (rogueDungeon_rogueDungeon == null)
			{
				return;
			}
			this.textFloor.text = Singleton<LanguageManager>.Instance.GetInfoByID(rogueDungeon_rogueDungeon.name);
			for (int i = 0; i < this.mDataModule.MonsterCfgList.Count; i++)
			{
				int num = this.mDataModule.MonsterCfgList[i];
				UIProgressPointItem uiprogressPointItem;
				if (i < this.pointItems.Count)
				{
					uiprogressPointItem = this.pointItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyPointItem);
					gameObject.SetParentNormal(this.progressParent, false);
					uiprogressPointItem = gameObject.GetComponent<UIProgressPointItem>();
					uiprogressPointItem.Init();
					this.pointItems.Add(uiprogressPointItem);
				}
				uiprogressPointItem.gameObject.SetActiveSafe(true);
				uiprogressPointItem.SetData(i, (int)this.mDataModule.CurrentWaveIndex, num);
				UIProgressPointBgItem uiprogressPointBgItem;
				if (i < this.pointBgItems.Count)
				{
					uiprogressPointBgItem = this.pointBgItems[i];
				}
				else
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.copyPointBgItem);
					gameObject2.SetParentNormal(this.progressBgParent, false);
					uiprogressPointBgItem = gameObject2.GetComponent<UIProgressPointBgItem>();
					uiprogressPointBgItem.Init();
					this.pointBgItems.Add(uiprogressPointBgItem);
				}
				uiprogressPointBgItem.gameObject.SetActiveSafe(true);
				uiprogressPointBgItem.SetData(i);
			}
			this.sliderProgress.minValue = 0f;
			this.sliderProgress.maxValue = (float)(this.mDataModule.MonsterCfgList.Count - 1);
			this.sliderProgress.wholeNumbers = true;
			float num2 = this.mDataModule.CurrentWaveIndex;
			this.sliderProgress.value = num2;
			this.skillCtrl.SetData(Singleton<GameEventController>.Instance.PlayerData.GetPlayerSkillBuildList());
			RectTransform component = this.progressParent.GetComponent<RectTransform>();
			RectTransform component2 = this.sliderProgress.GetComponent<RectTransform>();
			component2.sizeDelta = new Vector2(component.sizeDelta.x - 80f, component2.sizeDelta.y);
			this.sliderBgRT.sizeDelta = new Vector2(component2.sizeDelta.x + 120f, this.sliderBgRT.sizeDelta.y);
		}

		private void RefreshRound(int current, int max)
		{
			this.textRound.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitowerbattle_round", new object[] { current, max });
		}

		private void RefreshAttr(bool useAni = true)
		{
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			this.attrCtrl.SetHP(playerData.CurrentHp.GetValue(), playerData.HpMax.GetValue(), useAni);
			this.attrCtrl.SetAttack(playerData.Attack.GetValue());
			this.attrCtrl.SetDefence(playerData.Defence.GetValue());
			int num = (int)(playerData.CurrentHp.AsDouble() / playerData.HpMax.AsDouble() * 100.0);
			if (playerData.CurrentHp.GetValue() <= 0L)
			{
				num = 0;
			}
			else
			{
				num = Mathf.Clamp(num, 1, 100);
			}
			this.attrCtrl.SetHpPercent((long)num);
			if (!this.isShowColor)
			{
				this.attrCtrl.SetAttackColor(UIAttributeController.AttributeColorType.Normal);
				this.attrCtrl.SetDefenceColor(UIAttributeController.AttributeColorType.Normal);
				return;
			}
			if (playerData.Attack.GetValue() > this.befourBattleAtk)
			{
				this.attrCtrl.SetAttackColor(UIAttributeController.AttributeColorType.Green);
			}
			else if (playerData.Attack.GetValue() < this.befourBattleAtk)
			{
				this.attrCtrl.SetAttackColor(UIAttributeController.AttributeColorType.Red);
			}
			else
			{
				this.attrCtrl.SetAttackColor(UIAttributeController.AttributeColorType.Normal);
			}
			if (playerData.Defence.GetValue() > this.befourBattleDef)
			{
				this.attrCtrl.SetDefenceColor(UIAttributeController.AttributeColorType.Green);
				return;
			}
			if (playerData.Defence.GetValue() < this.befourBattleDef)
			{
				this.attrCtrl.SetDefenceColor(UIAttributeController.AttributeColorType.Red);
				return;
			}
			this.attrCtrl.SetDefenceColor(UIAttributeController.AttributeColorType.Normal);
		}

		private void ShowBattleAni(bool isBattle)
		{
			this.speedButtonCtrl.SetClickDisabled(true);
			Sequence sequence = this.sequencePool.Get();
			float num = 462f;
			float num2 = 630f;
			if (isBattle)
			{
				this.speedButtonCtrl.gameObject.SetActiveSafe(true);
				this.buttonSpeedRT.anchoredPosition = new Vector2(num2, this.buttonSpeedRT.anchoredPosition.y);
				TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonSpeedRT, num - 20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosX(this.buttonSpeedRT, num, 0.1f, false));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.speedButtonCtrl.SetClickDisabled(false);
				});
				return;
			}
			this.buttonSpeedRT.anchoredPosition = new Vector2(num, this.buttonSpeedRT.anchoredPosition.y);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonSpeedRT, num - 20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosX(this.buttonSpeedRT, num2, 0.1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.speedButtonCtrl.gameObject.SetActiveSafe(false);
				this.speedButtonCtrl.SetClickDisabled(false);
			});
		}

		private void OnEventRefreshRound(object sender, int type, BaseEventArgs args)
		{
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart == null)
			{
				return;
			}
			this.RefreshRound(eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound);
		}

		private void OnEventBattleStart(object sender, int type, BaseEventArgs args)
		{
			this.ShowBattleAni(true);
			this.befourBattleAtk = Singleton<GameEventController>.Instance.PlayerData.Attack.GetValue();
			this.befourBattleDef = Singleton<GameEventController>.Instance.PlayerData.Defence.GetValue();
			this.isShowColor = true;
		}

		private void OnEventBattleEnd(object sender, int type, BaseEventArgs args)
		{
			this.ShowBattleAni(false);
			this.isShowColor = false;
		}

		private void OnEventNextFloorStart(object sender, int type, BaseEventArgs args)
		{
			this.nextFloorAni.gameObject.SetActiveSafe(true);
			this.nextFloorAni.PlayAnimation("Start", false);
			this.nextFloorAni.AddAnimation("Loop", true, 0f);
		}

		private void OnEventNextFloorEnd(object sender, int type, BaseEventArgs args)
		{
			float animationDuration = this.nextFloorAni.GetAnimationDuration("End");
			this.nextFloorAni.PlayAnimation("End", false);
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, animationDuration);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.RefreshInfo();
				this.nextFloorAni.gameObject.SetActiveSafe(false);
			});
		}

		private void OnRefreshAttribute(object sender, int type, BaseEventArgs args)
		{
			this.RefreshAttr(true);
		}

		private void OnRefreshUI(object sender, int type, BaseEventArgs args)
		{
			this.RefreshInfo();
		}

		private void OnShowHideRound(object sender, int type, BaseEventArgs args)
		{
			EventArgsBool eventArgsBool = args as EventArgsBool;
			if (eventArgsBool != null)
			{
				this.roundObj.SetActiveSafe(eventArgsBool.Value);
			}
		}

		private void OnClickMonsterSkill()
		{
			SpecialChallengesViewModule.OpenData openData = new SpecialChallengesViewModule.OpenData();
			openData.monsterEntryIds = this.mDataModule.MonsterSkills.ToList<int>();
			openData.source = SpecialChallengesViewModule.Source.RogueDungeon;
			GameApp.View.OpenView(ViewName.SpecialChallengesViewModule, openData, 1, null, null);
		}

		private void OnClickEscape()
		{
			string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("uiroguedungeon_escape_pop"), Array.Empty<object>());
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uiroguedungeon_escape");
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Cancel");
			DxxTools.UI.OpenPopCommon(text, delegate(int id)
			{
				if (id == -1)
				{
					NetworkUtils.RogueDungeon.DoHellExitBattleRequest(delegate(bool result, HellExitBattleResponse response)
					{
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIBattleRogueDungeon_Escape, null);
						GameApp.View.OpenView(ViewName.BattleRogueDungeonResultViewModule, null, 1, null, null);
						if (result && response != null)
						{
							BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
							if (this.mDataModule.BattleResult == 1 && playerData != null)
							{
								GameApp.SDK.Analyze.Track_EnterDungeon(2, playerData.AttributeData.Attack.GetValue(), playerData.AttributeData.Defence.GetValue(), playerData.AttributeData.HPMax.GetValue(), playerData.GetPlayerSkillBuildList(), response.CommonData.Reward);
							}
						}
					});
				}
			}, string.Empty, infoByID2, infoByID, false, 2);
		}

		[Header("顶部")]
		public CustomText textFloor;

		public CustomText textRound;

		public CustomButton buttonMonsterSkill;

		public GameObject roundObj;

		[Header("中间")]
		public UISpeedButtonCtrl speedButtonCtrl;

		public GameObject progressParent;

		public GameObject copyPointItem;

		public GameObject progressBgParent;

		public GameObject copyPointBgItem;

		public Slider sliderProgress;

		public UIPlayerAttributeCtrl attrCtrl;

		public UIRogueDungeonSkillCtrl skillCtrl;

		public UISpineModelItem nextFloorAni;

		[Header("底部")]
		public CustomButton buttonEscape;

		public RectTransform sliderBgRT;

		private RectTransform buttonSpeedRT;

		private SequencePool sequencePool = new SequencePool();

		private List<UIProgressPointItem> pointItems = new List<UIProgressPointItem>();

		private List<UIProgressPointBgItem> pointBgItems = new List<UIProgressPointBgItem>();

		private bool isShowColor;

		private long befourBattleAtk;

		private long befourBattleDef;

		private RogueDungeonDataModule mDataModule;
	}
}
