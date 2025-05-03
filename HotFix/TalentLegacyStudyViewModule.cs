using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Talents;
using Server;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyStudyViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.Button_Close.m_onClick = new Action(this.OnClickClose);
			this.Button_Mask.m_onClick = new Action(this.OnClickClose);
			this.Button_Study.m_onClick = new Action(this.OnClickStudy);
			this.Button_AutoSpeed.m_onClick = new Action(this.OnClickAutoSpeed);
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.m_propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.Ctrl_NodeItem.Init();
			this.Ctrl_ButtonAd.Init();
		}

		public override void OnOpen(object data)
		{
			this.m_openData = (TalentLegacyStudyViewModule.OpenData)data;
			if (this.m_openData == null)
			{
				return;
			}
			this.Ctrl_CostItem1.Init();
			this.Ctrl_CostItem2.Init();
			GuideController.Instance.DelTarget("Button_Study");
			GuideController.Instance.AddTarget("Button_Study", this.Button_Study.transform);
			GuideController.Instance.OpenViewTrigger(ViewName.TalentLegacyStudyViewModule);
			this.OnRefreshView();
			this.Ctrl_NodeItem.OnShow();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.Ctrl_CostItem1.DeInit();
			this.Ctrl_CostItem2.DeInit();
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnRefreshCountDown));
			this.Ctrl_NodeItem.OnClose();
		}

		public override void OnDelete()
		{
			this.Button_Close.m_onClick = null;
			this.Button_Mask.m_onClick = null;
			this.Button_Study.m_onClick = null;
			this.Button_AutoSpeed.m_onClick = null;
			this.Ctrl_NodeItem.DeInit();
			this.Ctrl_ButtonAd.DeInit();
		}

		private void OnClickAutoSpeed()
		{
			if (this.m_skillInfoData == null)
			{
				return;
			}
			long num = this.m_skillInfoData.LevelUpTime - DxxTools.Time.ServerTimestamp;
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (dataModule.GetItemDataCountByid(47UL) <= 0L)
			{
				Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(1);
				int num2 = Utility.Math.CeilToInt((float)num / (float)Singleton<GameConfig>.Instance.TalentLegacyGoldSpeedTime);
				long haveGoldCount = dataModule.GetItemDataCountByid(1UL);
				long needGold = (long)(num2 * Singleton<GameConfig>.Instance.TalentLegacyGoldSpeedNum);
				string text = "#F4883f";
				if (haveGoldCount < needGold)
				{
					text = "#FF0000";
				}
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(item_Item.nameID);
				string text2 = HLog.StringBuilderFormat(Singleton<LanguageManager>.Instance.GetInfoByID("legacy_speed_use_gold_tip"), HLog.StringBuilder("<color=", text, ">", DxxTools.FormatNumber(needGold), infoByID, "</color>"));
				RememberTipCommonViewModule.TryRunRememberTip(new RememberTipCommonViewModule.OpenData
				{
					titleStr = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_speed_title"),
					rememberTipType = RememberTipType.None,
					contentStr = text2,
					onConfirmCallback = delegate
					{
						if (haveGoldCount < needGold)
						{
							GameApp.View.ShowItemNotEnoughTip(1, true);
							return;
						}
						GameApp.Sound.PlayClip(689, 1f);
						NetworkUtils.TalentLegacy.DoTalentLegacyLevelUpCoolDownRequest(this.m_openData.CareerId, this.m_openData.TalentLagcyNodeId, 3, (int)needGold, delegate(bool result, TalentLegacyLevelUpCoolDownResponse resp)
						{
							if (result)
							{
								GameApp.View.CloseView(ViewName.CommonUseTipViewModule, null);
								this.OnRefreshView();
								this.Ctrl_NodeItem.PlayStudySpeedEffect();
							}
						});
					}
				});
				return;
			}
			CommonUseTipViewModule.OpenData openData = new CommonUseTipViewModule.OpenData();
			openData.ItemId = 47;
			openData.BuyButtonTitle = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_Skill_Speed");
			openData.CallBack = new Action<int>(this.OnUseCallBack);
			int num3 = Utility.Math.CeilToInt((float)num / (float)Singleton<GameConfig>.Instance.TalentLegacySpeedItemTime);
			if (num3 <= 0)
			{
				num3 = 1;
			}
			openData.MaxCount = num3;
			openData.TitleStr = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_speed");
			DxxTools.UI.OpenPopCommonUse(openData);
		}

		private void OnUseCallBack(int num)
		{
			if (this.m_skillInfoData == null)
			{
				return;
			}
			GameApp.Sound.PlayClip(689, 1f);
			NetworkUtils.TalentLegacy.DoTalentLegacyLevelUpCoolDownRequest(this.m_openData.CareerId, this.m_openData.TalentLagcyNodeId, 1, num, delegate(bool result, TalentLegacyLevelUpCoolDownResponse resp)
			{
				if (result)
				{
					GameApp.View.CloseView(ViewName.CommonUseTipViewModule, null);
					this.OnRefreshView();
					this.Ctrl_NodeItem.PlayStudySpeedEffect();
				}
			});
		}

		private void OnClickAdSpeed()
		{
			AdBridge.PlayRewardVideo(14, delegate(bool isSuccess)
			{
				if (isSuccess)
				{
					GameApp.Sound.PlayClip(689, 1f);
					NetworkUtils.TalentLegacy.DoTalentLegacyLevelUpCoolDownRequest(this.m_openData.CareerId, this.m_openData.TalentLagcyNodeId, 0, 0, delegate(bool result, TalentLegacyLevelUpCoolDownResponse resp)
					{
						if (result)
						{
							GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(14), "REWARD ", "", resp.CommonData.Reward, null);
							this.OnRefreshView();
							this.Ctrl_NodeItem.PlayStudySpeedEffect();
						}
					});
				}
			});
		}

		private void OnClickStudy()
		{
			if (this.m_openData == null)
			{
				return;
			}
			if (this.m_nextEffectCfg == null)
			{
				return;
			}
			List<ItemData> list = this.m_nextEffectCfg.levelupCost.ToItemDataList();
			bool flag = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)list[i].ID)) < list[i].TotalCount)
				{
					flag = false;
					GameApp.View.ShowItemNotEnoughTip(list[i].ID, true);
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			GameApp.Sound.PlayClip(687, 1f);
			NetworkUtils.TalentLegacy.DoTalentLegacyLevelUpRequest(this.m_openData.CareerId, this.m_openData.TalentLagcyNodeId, delegate(bool result, TalentLegacyLevelUpResponse resp)
			{
				if (result)
				{
					this.OnRefreshView();
				}
			});
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.TalentLegacyStudyViewModule, null);
		}

		private void OnRefreshView()
		{
			this.m_talentLegacyInfo = this.m_talentLegacyModule.OnGetTalentLegacyInfo();
			if (this.m_talentLegacyInfo == null)
			{
				return;
			}
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(this.m_openData.TalentLagcyNodeId);
			if (talentLegacy_talentLegacyNode == null)
			{
				return;
			}
			this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_talentLegacyNode.name);
			this.Ctrl_NodeItem.SetData(this.m_openData.CareerId, this.m_openData.TalentLagcyNodeId, false, false, true);
			this.m_skillInfoData = this.m_talentLegacyModule.OnGetTalentLegacySkillInfo(this.m_openData.CareerId, this.m_openData.TalentLagcyNodeId);
			this.m_effectCfg = null;
			this.m_nextEffectCfg = null;
			this.m_itemDataList.Clear();
			if (!this.m_talentLegacyModule.IsUnlockTalentLegacyNode(talentLegacy_talentLegacyNode.id))
			{
				this.OnShowFirstNodeEffect();
				this.Text_Max.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_node_unlock_tip");
				return;
			}
			bool flag = false;
			if (this.m_skillInfoData != null)
			{
				this.m_effectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_skillInfoData.TalentLegacyNodeId, this.m_skillInfoData.Level);
				this.m_nextEffectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_skillInfoData.TalentLegacyNodeId, this.m_skillInfoData.Level + 1);
				if (this.m_nextEffectCfg == null)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.OnRefreshCurAttrItem();
				this.Text_Max.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_max_level");
			}
			else
			{
				ValueTuple<int, int> valueTuple = this.m_talentLegacyModule.IsHaveStudyingNode();
				if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1 && valueTuple.Item2 != this.m_openData.TalentLagcyNodeId)
				{
					this.OnShowFirstNodeEffect();
					this.Text_Max.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_node_studying_tip");
					return;
				}
			}
			if (this.m_skillInfoData == null)
			{
				this.ImageArrow.gameObject.SetActiveSafe(false);
				this.Obj_Time.SetActiveSafe(true);
				this.Button_Study.gameObject.SetActiveSafe(true);
				this.Obj_CostNode.gameObject.SetActiveSafe(true);
				this.Button_AdSpeed.gameObject.SetActiveSafe(false);
				this.Button_AutoSpeed.gameObject.SetActiveSafe(false);
				this.Text_Max.gameObject.SetActiveSafe(false);
				this.m_effectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_openData.TalentLagcyNodeId, 0);
				this.m_nextEffectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_openData.TalentLagcyNodeId, 0);
				this.Text_Time.text = DxxTools.FormatFullTimeWithDay((long)this.m_nextEffectCfg.levelupTime);
				this.m_itemDataList.AddRange(this.m_nextEffectCfg.levelupCost.ToItemDataList());
				this.OnRefreshStudyCost();
				this.OnRefreshCurAttrItem();
			}
			else
			{
				this.m_effectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_skillInfoData.TalentLegacyNodeId, this.m_skillInfoData.Level);
				this.m_nextEffectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_skillInfoData.TalentLegacyNodeId, this.m_skillInfoData.Level + 1);
				if (this.m_nextEffectCfg != null)
				{
					if (this.m_effectCfg == null)
					{
						return;
					}
					this.Obj_CurAttr.SetActiveSafe(false);
					this.ImageArrow.gameObject.SetActiveSafe(true);
					this.Obj_LastAttr.SetActiveSafe(true);
					this.Obj_NextAttr.SetActiveSafe(true);
					if (this.m_skillInfoData.Level >= 1)
					{
						this.SetAttr(this.Text_LastAttr, this.m_effectCfg, false);
						this.SetAttr(this.Text_NextAttr, this.m_nextEffectCfg, false);
					}
					else
					{
						this.OnRefreshCurAttrItem();
					}
					if (this.m_skillInfoData.LevelUpTime > 0L)
					{
						this.Obj_Time.SetActiveSafe(true);
						this.Button_Study.gameObject.SetActiveSafe(false);
						this.Obj_CostNode.SetActiveSafe(false);
						this.Button_AdSpeed.gameObject.SetActiveSafe(true);
						this.Button_AutoSpeed.gameObject.SetActiveSafe(true);
						this.Text_Max.gameObject.SetActiveSafe(false);
						GameConfig_Config gameConfig_Config = GameApp.Table.GetManager().GetGameConfig_Config(8502);
						int maxTimes = this.m_adDataModule.GetMaxTimes(14);
						int watchTimes = this.m_adDataModule.GetWatchTimes(14);
						if (watchTimes < maxTimes)
						{
							this.Button_AdSpeed.GetComponent<UIGrays>().Recovery();
							this.Button_AdSpeed.enabled = true;
						}
						else
						{
							this.Button_AdSpeed.GetComponent<UIGrays>().SetUIGray();
							this.Button_AdSpeed.enabled = false;
						}
						this.Ctrl_ButtonAd.SetData(-1, -1, watchTimes, maxTimes, new Action(this.OnClickAdSpeed));
						this.Text_AdDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_ad_speed", new object[] { gameConfig_Config.Value });
						long serverTimestamp = DxxTools.Time.ServerTimestamp;
						long num = this.m_skillInfoData.LevelUpTime - serverTimestamp;
						this.Text_Time.text = DxxTools.FormatFullTimeWithDay(num);
						DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnRefreshCountDown));
						DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.OnRefreshCountDown));
					}
					else
					{
						DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnRefreshCountDown));
						this.Obj_Time.SetActiveSafe(true);
						this.Button_Study.gameObject.SetActiveSafe(true);
						this.Obj_CostNode.SetActiveSafe(true);
						this.Button_AdSpeed.gameObject.SetActiveSafe(false);
						this.Button_AutoSpeed.gameObject.SetActiveSafe(false);
						this.Text_Max.gameObject.SetActiveSafe(false);
						this.Text_Time.text = DxxTools.FormatFullTimeWithDay((long)this.m_nextEffectCfg.levelupTime);
						this.m_itemDataList.AddRange(this.m_nextEffectCfg.levelupCost.ToItemDataList());
						this.OnRefreshStudyCost();
					}
				}
				else
				{
					this.ImageArrow.gameObject.SetActiveSafe(false);
					this.Obj_Time.SetActiveSafe(false);
					this.Button_Study.gameObject.SetActiveSafe(false);
					this.Obj_CostNode.SetActiveSafe(false);
					this.Button_AdSpeed.gameObject.SetActiveSafe(false);
					this.Button_AutoSpeed.gameObject.SetActiveSafe(false);
					this.Text_Max.gameObject.SetActiveSafe(true);
					this.Text_Max.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_max_level");
					this.OnRefreshCurAttrItem();
				}
			}
			int maxTimes2 = this.m_adDataModule.GetMaxTimes(14);
			int watchTimes2 = this.m_adDataModule.GetWatchTimes(14);
			this.Ctrl_Red_AdSpeed.gameObject.SetActiveSafe(watchTimes2 < maxTimes2);
			long itemDataCountByid = this.m_propDataModule.GetItemDataCountByid(47UL);
			this.Ctrl_Red_AutoSpeed.gameObject.SetActiveSafe(itemDataCountByid > 0L);
		}

		private void OnShowFirstNodeEffect()
		{
			this.ImageArrow.gameObject.SetActiveSafe(false);
			this.Button_Study.gameObject.SetActiveSafe(false);
			this.Obj_CostNode.gameObject.SetActiveSafe(false);
			this.Button_AdSpeed.gameObject.SetActiveSafe(false);
			this.Button_AutoSpeed.gameObject.SetActiveSafe(false);
			this.Text_Max.gameObject.SetActiveSafe(true);
			this.Obj_Time.SetActiveSafe(true);
			this.m_effectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_openData.TalentLagcyNodeId, 0);
			this.m_nextEffectCfg = this.m_talentLegacyModule.GetTalentLegacySkillCfgByLevel(this.m_openData.TalentLagcyNodeId, 0);
			this.m_itemDataList.AddRange(this.m_nextEffectCfg.levelupCost.ToItemDataList());
			this.Text_Time.text = DxxTools.FormatFullTimeWithDay((long)this.m_nextEffectCfg.levelupTime);
			this.OnRefreshStudyCost();
			this.OnRefreshCurAttrItem();
		}

		private void OnRefreshStudyCost()
		{
			this.Ctrl_CostItem1.gameObject.SetActiveSafe(false);
			this.Ctrl_CostItem2.gameObject.SetActiveSafe(false);
			if (this.m_itemDataList.Count == 1)
			{
				this.Ctrl_CostItem1.gameObject.SetActiveSafe(true);
				this.Ctrl_CostItem2.gameObject.SetActiveSafe(false);
				long itemDataCountByid = this.m_propDataModule.GetItemDataCountByid((ulong)((long)this.m_itemDataList[0].ID));
				long count = this.m_itemDataList[0].Count;
				this.Ctrl_CostItem1.SetData(this.m_itemDataList[0], itemDataCountByid, count);
			}
			else if (this.m_itemDataList.Count >= 2)
			{
				long itemDataCountByid2 = this.m_propDataModule.GetItemDataCountByid((ulong)((long)this.m_itemDataList[0].ID));
				long count2 = this.m_itemDataList[0].Count;
				long itemDataCountByid3 = this.m_propDataModule.GetItemDataCountByid((ulong)((long)this.m_itemDataList[1].ID));
				long count3 = this.m_itemDataList[1].Count;
				this.Ctrl_CostItem1.gameObject.SetActiveSafe(true);
				this.Ctrl_CostItem2.gameObject.SetActiveSafe(true);
				this.Ctrl_CostItem1.SetData(this.m_itemDataList[0], itemDataCountByid2, count2);
				this.Ctrl_CostItem2.SetData(this.m_itemDataList[1], itemDataCountByid3, count3);
			}
			bool flag = true;
			for (int i = 0; i < this.m_itemDataList.Count; i++)
			{
				if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.m_itemDataList[i].ID)) < this.m_itemDataList[i].TotalCount)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.Button_Study.GetComponent<UIGrays>().Recovery();
			}
			else
			{
				this.Button_Study.GetComponent<UIGrays>().SetUIGray();
			}
			this.Ctrl_Red_Study.gameObject.SetActiveSafe(flag);
		}

		private void OnRefreshCurAttrItem()
		{
			this.ImageArrow.gameObject.SetActiveSafe(false);
			this.Obj_CurAttr.SetActiveSafe(true);
			this.Obj_LastAttr.SetActiveSafe(false);
			this.Obj_NextAttr.SetActiveSafe(false);
			this.SetAttr(this.Text_CurAttr, this.m_effectCfg, true);
		}

		private void SetAttr(CustomText text, TalentLegacy_talentLegacyEffect effectCfg, bool isCur = false)
		{
			if (effectCfg == null)
			{
				text.text = "";
				return;
			}
			if (string.IsNullOrEmpty(effectCfg.desc))
			{
				string text2 = "";
				List<MergeAttributeData> mergeAttributeData = effectCfg.attributes.GetMergeAttributeData();
				for (int i = 0; i < mergeAttributeData.Count; i++)
				{
					Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData[i].Header);
					if (elementById != null)
					{
						string text3 = "";
						if (mergeAttributeData[i].Header.Contains("%"))
						{
							text3 = "%";
						}
						string text4 = DxxTools.FormatNumber((long)mergeAttributeData[i].Value);
						if (isCur)
						{
							text2 = HLog.StringBuilder(text2, Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId), "<color=#298744>", " +", text4, text3, "</color>", (i == mergeAttributeData.Count - 1) ? "" : ",");
						}
						else
						{
							text2 = HLog.StringBuilder(text2, Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId), "<color=#298744>", " +", text4, text3, "</color>", (i == mergeAttributeData.Count - 1) ? "" : ",");
						}
					}
				}
				text.text = text2;
				return;
			}
			text.text = Singleton<LanguageManager>.Instance.GetInfoByID(effectCfg.desc);
		}

		private void OnRefreshCountDown()
		{
			if (this.m_skillInfoData != null)
			{
				long num = this.m_skillInfoData.LevelUpTime - DxxTools.Time.ServerTimestamp;
				if (num <= 0L)
				{
					this.Obj_Time.SetActiveSafe(false);
					this.Text_Time.text = "";
					return;
				}
				this.Obj_Time.SetActiveSafe(true);
				this.Text_Time.text = DxxTools.FormatFullTimeWithDay(num);
			}
		}

		private void Event_Update(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshStudyCost();
		}

		private void OnTalentLegacyNodeTimeEnd(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshView();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
		}

		public GameObject Obj_Time;

		public CustomText Text_Time;

		public CustomImage ImageArrow;

		public TalentLegacyNodeItem Ctrl_NodeItem;

		public CustomText Text_Title;

		[Header("属性")]
		public GameObject Obj_LastAttr;

		public CustomImage Image_LastBg;

		public CustomText Text_LastAttr;

		public GameObject Obj_NextAttr;

		public CustomImage Image_NextBg;

		public CustomText Text_NextAttr;

		public GameObject Obj_CurAttr;

		public CustomImage Image_CurBg;

		public CustomText Text_CurAttr;

		public CustomText Text_Max;

		[Header("按钮")]
		public CustomButton Button_AdSpeed;

		public CustomText Text_AdDesc;

		public CustomText Text_AdNum;

		public UIAdExchangeButton Ctrl_ButtonAd;

		public CustomButton Button_AutoSpeed;

		public CustomButton Button_Study;

		public GameObject Obj_CostNode;

		public CommonCostItem Ctrl_CostItem1;

		public CommonCostItem Ctrl_CostItem2;

		public CustomButton Button_Mask;

		public CustomButton Button_Close;

		[Header("红点")]
		public RedNodeOneCtrl Ctrl_Red_Study;

		public RedNodeOneCtrl Ctrl_Red_AdSpeed;

		public RedNodeOneCtrl Ctrl_Red_AutoSpeed;

		private TalentLegacyDataModule m_talentLegacyModule;

		private TalentLegacyDataModule.TalentLegacyInfo m_talentLegacyInfo;

		private TalentLegacyStudyViewModule.OpenData m_openData;

		private TalentLegacy_talentLegacyEffect m_effectCfg;

		private TalentLegacy_talentLegacyEffect m_nextEffectCfg;

		private List<ItemData> m_itemDataList = new List<ItemData>();

		private TalentLegacyDataModule.TalentLegacySkillInfo m_skillInfoData;

		private PropDataModule m_propDataModule;

		private AdDataModule m_adDataModule;

		public class OpenData
		{
			public int CareerId;

			public int TalentLagcyNodeId;
		}
	}
}
