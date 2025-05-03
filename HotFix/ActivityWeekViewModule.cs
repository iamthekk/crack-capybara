using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ActivityWeekViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.ActivityWeekViewModule;
		}

		public override void OnCreate(object data)
		{
			this.Obj_NetLoading.SetActive(false);
			this._catchList = new List<ActivityWeekNode>();
			for (int i = 0; i < this.roundItems.Count; i++)
			{
				this.roundItems[i].gameObject.SetActiveSafe(false);
				this.roundItems[i].Init();
			}
			this.Button_Close.m_onClick = new Action(this.OnClickClose);
			this.buttonMask.m_onClick = new Action(this.OnClickClose);
			this.m_RoundCollectBtn.m_onClick = new Action(this.OnClickRoundCollectBtn);
			this.m_btnInfo.m_onClick = new Action(this.OnBtnInfoClick);
			this.m_gotoBt.m_onClick = new Action(this.OnClickGotoBt);
			this.rankRewardSelf.Init();
			this.currencyCtrl.Init();
		}

		public override void OnDelete()
		{
			for (int i = 0; i < this._catchList.Count; i++)
			{
				Object.Destroy(this._catchList[i]);
			}
			this._catchList.Clear();
			for (int j = 0; j < this.roundItems.Count; j++)
			{
				this.roundItems[j].gameObject.SetActiveSafe(false);
				this.roundItems[j].DeInit();
			}
			this.Button_Close.m_onClick = null;
			this.buttonMask.m_onClick = null;
			this.m_RoundCollectBtn.m_onClick = null;
			this.m_btnInfo.m_onClick = null;
			this.m_gotoBt.m_onClick = null;
			this.rankRewardSelf.DeInit();
			this.currencyCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_ActTimeActivity, new HandlerEvent(this.OnViewOpen));
			manager.RegisterEvent(LocalMessageName.CC_ActTimeRank, new HandlerEvent(this.OnRankViewOpen));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_ActTimeActivity, new HandlerEvent(this.OnViewOpen));
			manager.UnRegisterEvent(LocalMessageName.CC_ActTimeRank, new HandlerEvent(this.OnRankViewOpen));
		}

		public override void OnOpen(object data)
		{
			this.actDataModule = GameApp.Data.GetDataModule(DataName.ActivityWeekDataModule);
			ActivityWeekEntryViewModule viewModule = GameApp.View.GetViewModule(ViewName.ActivityWeekEntryModule);
			if (viewModule != null)
			{
				viewModule.SetBgVisible(false);
			}
			this.m_ActId = (int)data;
			this.commonCfg = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(this.m_ActId);
			this.rankCtrl.Init();
			this.m_titleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.commonCfg.Name);
			int num = 0;
			if (this.commonCfg.Currency.Length != 0)
			{
				num = this.commonCfg.Currency[0];
			}
			this.currencyCtrl.SetItemId(num);
			this.m_ShowTabs.Clear();
			for (int i = 0; i < this.m_actBtList.Count; i++)
			{
				ActivityWeekViewModule.ActViewData actViewData = this.m_actBtList[i];
				bool flag = false;
				switch (actViewData.type)
				{
				case ActivityWeekViewModule.ActViewType.ConsumeType:
					flag = this.commonCfg.QuestID > 0;
					break;
				case ActivityWeekViewModule.ActViewType.ShopType:
					if (this.commonCfg.Type == 2)
					{
						flag = this.commonCfg.ShopID > 0;
					}
					break;
				case ActivityWeekViewModule.ActViewType.PayType:
					flag = this.commonCfg.PayID > 0;
					break;
				case ActivityWeekViewModule.ActViewType.RankRewardType:
					flag = this.commonCfg.RankID > 0;
					break;
				case ActivityWeekViewModule.ActViewType.RankOrderType:
					flag = this.commonCfg.RankID > 0;
					break;
				}
				if (flag)
				{
					int index = this.m_ShowTabs.Count;
					this.m_ShowTabs.Add(actViewData);
					actViewData.obj.SetActive(true);
					actViewData.btn.onClick.RemoveAllListeners();
					actViewData.btn.onClick.AddListener(delegate
					{
						this.OnClickActBt(index);
					});
					actViewData.redNodeCtrl.gameObject.SetActive(false);
				}
				else
				{
					actViewData.obj.SetActive(false);
				}
			}
			this.actBtnHiddenChecker.CheckHidden();
			this.m_selectIndex = -1;
			this.FreshAllView();
		}

		private void FreshAllView()
		{
			if (this.m_selectIndex < 0)
			{
				this.OnSelectIndex(0);
				return;
			}
			this.OnSelectIndex(this.m_selectIndex);
		}

		private void OnViewOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			if (!(eventArgs is EventArgsActTimeInfoData))
			{
				return;
			}
			this.Obj_NetLoading.SetActive(false);
			this.FreshAllView();
		}

		private void OnRankViewOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			if (!(eventArgs is EventArgsActTimeRankData))
			{
				return;
			}
			this.Obj_NetLoading.SetActive(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.UpdateLeftTime();
		}

		public override void OnClose()
		{
			this.rankCtrl.DeInit();
			this.m_pool.Clear(false);
			DxxTools.UI.RemoveServerTimeClockCallback("ActivityWeekViewModule");
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(this.GetName(), null);
			ActivityWeekEntryViewModule viewModule = GameApp.View.GetViewModule(ViewName.ActivityWeekEntryModule);
			if (viewModule == null)
			{
				return;
			}
			viewModule.SetBgVisible(true);
		}

		private TimeBase GetTimeBase()
		{
			if (this.commonCfg.Type == 1)
			{
				if (this.actDataModule.ConsumeData == null)
				{
					goto IL_0235;
				}
				using (IEnumerator<Consume> enumerator = this.actDataModule.ConsumeData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Consume consume = enumerator.Current;
						if (consume.TimeBase.ActId == this.m_ActId)
						{
							return consume.TimeBase;
						}
					}
					goto IL_0235;
				}
			}
			if (this.commonCfg.Type == 2)
			{
				if (this.actDataModule.ShopData == null)
				{
					goto IL_0235;
				}
				using (IEnumerator<Shop> enumerator2 = this.actDataModule.ShopData.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Shop shop = enumerator2.Current;
						if (shop.TimeBase.ActId == this.m_ActId)
						{
							return shop.TimeBase;
						}
					}
					goto IL_0235;
				}
			}
			if (this.commonCfg.Type == 3)
			{
				if (this.actDataModule.DropData == null)
				{
					goto IL_0235;
				}
				using (IEnumerator<Drop> enumerator3 = this.actDataModule.DropData.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Drop drop = enumerator3.Current;
						if (drop.TimeBase.ActId == this.m_ActId)
						{
							return drop.TimeBase;
						}
					}
					goto IL_0235;
				}
			}
			if (this.commonCfg.Type == 4)
			{
				if (this.actDataModule.PayData == null)
				{
					goto IL_0235;
				}
				using (IEnumerator<Pay> enumerator4 = this.actDataModule.PayData.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Pay pay = enumerator4.Current;
						if (pay.TimeBase.ActId == this.m_ActId)
						{
							return pay.TimeBase;
						}
					}
					goto IL_0235;
				}
			}
			if (this.commonCfg.Type == 5 && this.actDataModule.ChapterData != null)
			{
				foreach (Chapter chapter in this.actDataModule.ChapterData)
				{
					if (chapter.TimeBase.ActId == this.m_ActId)
					{
						return chapter.TimeBase;
					}
				}
			}
			IL_0235:
			return null;
		}

		private UIActivityWeekRankItem.RankType GetRankType()
		{
			if (this.commonCfg.Type == 1)
			{
				return UIActivityWeekRankItem.RankType.Score;
			}
			if (this.commonCfg.Type == 5)
			{
				return UIActivityWeekRankItem.RankType.Chapter;
			}
			return UIActivityWeekRankItem.RankType.Score;
		}

		private void FreshRedNode()
		{
			for (int i = 0; i < this.m_ShowTabs.Count; i++)
			{
				ActivityWeekViewModule.ActViewData actViewData = this.m_ShowTabs[i];
				bool flag = false;
				switch (actViewData.type)
				{
				case ActivityWeekViewModule.ActViewType.ConsumeType:
				{
					Consume consume = null;
					if (this.actDataModule.ConsumeData != null)
					{
						foreach (Consume consume2 in this.actDataModule.ConsumeData)
						{
							if (consume2.TimeBase.ActId == this.m_ActId)
							{
								consume = consume2;
								break;
							}
						}
					}
					flag = this.actDataModule.ConsumeShowRed(this.m_ActId, consume);
					break;
				}
				case ActivityWeekViewModule.ActViewType.ShopType:
				{
					Shop shop = null;
					if (this.actDataModule.ShopData != null)
					{
						foreach (Shop shop2 in this.actDataModule.ShopData)
						{
							if (shop2.TimeBase.ActId == this.m_ActId)
							{
								shop = shop2;
								break;
							}
						}
					}
					flag = this.actDataModule.ShopShowRed(this.m_ActId, shop);
					break;
				}
				case ActivityWeekViewModule.ActViewType.PayType:
				{
					Pay pay = null;
					if (this.actDataModule.PayData != null)
					{
						foreach (Pay pay2 in this.actDataModule.PayData)
						{
							if (pay2.TimeBase.ActId == this.m_ActId)
							{
								pay = pay2;
								break;
							}
						}
					}
					flag = this.actDataModule.PayShowRed(this.m_ActId, pay);
					break;
				}
				}
				actViewData.redNodeCtrl.gameObject.SetActive(flag);
			}
		}

		private void OnSelectIndex(int index)
		{
			if (index < 0 || index >= this.m_ShowTabs.Count)
			{
				return;
			}
			bool showAnim = this.m_selectIndex != index;
			ActivityWeekViewModule.ActViewData data = this.m_ShowTabs[index];
			this.rankCtrl.SetContentVisible(false);
			if (data.type == ActivityWeekViewModule.ActViewType.ConsumeType)
			{
				this.SetToViewTab(index);
				this.FreshRedNode();
				this.SetViewRoot(data);
				Consume consume = null;
				if (this.actDataModule.ConsumeData != null)
				{
					foreach (Consume consume2 in this.actDataModule.ConsumeData)
					{
						if (consume2.TimeBase.ActId == this.m_ActId)
						{
							consume = consume2;
							break;
						}
					}
				}
				bool flag = false;
				bool flag2 = false;
				if (consume != null)
				{
					flag2 = consume.TimeBase.Round >= this.commonCfg.Round;
					flag = flag2 && consume.TimeBase.RoundRewardTimes < 1L;
				}
				this.ShowConsume(this.m_ActId, consume, flag2, showAnim);
				this.ShowActBanner(data, flag);
				return;
			}
			UIActivityWeekRankItem.RankType rankType = this.GetRankType();
			if (this.commonCfg.RankID > 0)
			{
				this.rankCtrl.Open(data.type, rankType, this.m_ActId, delegate(bool success)
				{
					if (!success)
					{
						return;
					}
					this.SetToViewTab(index);
					this.ShowRankView(data, rankType, showAnim);
				});
				return;
			}
			this.SetToViewTab(index);
			this.ShowRankView(data, rankType, showAnim);
		}

		private void SetToViewTab(int index)
		{
			if (this.m_selectIndex != index)
			{
				this.m_selectIndex = index;
				this.content.localPosition = Vector2.zero;
			}
			ActivityWeekViewModule.ActViewData actViewData = this.m_ShowTabs[index];
			this.SetViewTab(actViewData);
		}

		private void SetViewTab(ActivityWeekViewModule.ActViewData data)
		{
			for (int i = 0; i < this.m_ShowTabs.Count; i++)
			{
				ActivityWeekViewModule.ActViewData actViewData = this.m_ShowTabs[i];
				if (actViewData != null)
				{
					actViewData.btn.SetSelect(actViewData.btn == data.btn);
				}
			}
		}

		private void SetViewRoot(ActivityWeekViewModule.ActViewData data)
		{
			this.taskRoot.SetActiveSafe(data.type != ActivityWeekViewModule.ActViewType.RankOrderType);
			this.rankTop.SetActiveSafe(data.type != ActivityWeekViewModule.ActViewType.ConsumeType && this.commonCfg.RankID > 0);
			this.taskTop.SetActiveSafe(data.type == ActivityWeekViewModule.ActViewType.ConsumeType || this.commonCfg.RankID < 1);
			this.elementLayout.spacing = ((data.type == ActivityWeekViewModule.ActViewType.RankRewardType) ? this.m_layoutSpace_Rank : this.m_layoutSpace_Normal);
			Vector2 offsetMax = this.taskScrollNode.offsetMax;
			if (this.commonCfg.RankID < 1 && data.type != ActivityWeekViewModule.ActViewType.RankRewardType && data.type != ActivityWeekViewModule.ActViewType.ConsumeType)
			{
				offsetMax.y = -this.m_scrollTopHeight_Rank_UseTask;
			}
			else
			{
				offsetMax.y = -this.m_scrollTopHeight_Normal;
			}
			this.taskScrollNode.offsetMax = offsetMax;
			Vector2 offsetMin = this.taskScrollNode.offsetMin;
			offsetMin.y = ((data.type == ActivityWeekViewModule.ActViewType.RankRewardType) ? this.m_scrollBottomHeight_Rank : this.m_scrollBottomHeight_Normal);
			this.taskScrollNode.offsetMin = offsetMin;
			this.rankRewardSelf.gameObject.SetActiveSafe(data.type == ActivityWeekViewModule.ActViewType.RankRewardType);
		}

		private void ShowRankView(ActivityWeekViewModule.ActViewData data, UIActivityWeekRankItem.RankType rankType, bool showAnim)
		{
			this.FreshRedNode();
			this.SetViewRoot(data);
			if (this.commonCfg.RankID < 1)
			{
				this.ShowActBanner(data, false);
			}
			if (data.type == ActivityWeekViewModule.ActViewType.ShopType)
			{
				Shop shop = null;
				if (this.actDataModule.ShopData != null)
				{
					foreach (Shop shop2 in this.actDataModule.ShopData)
					{
						if (shop2.TimeBase.ActId == this.m_ActId)
						{
							shop = shop2;
							break;
						}
					}
				}
				this.rankCtrl.FreshTopRank();
				this.ShowShop(this.m_ActId, shop, showAnim);
				return;
			}
			if (data.type == ActivityWeekViewModule.ActViewType.PayType)
			{
				Pay pay = null;
				if (this.actDataModule.PayData != null)
				{
					foreach (Pay pay2 in this.actDataModule.PayData)
					{
						if (pay2.TimeBase.ActId == this.m_ActId)
						{
							pay = pay2;
							break;
						}
					}
				}
				this.rankCtrl.FreshTopRank();
				this.ShowPay(this.m_ActId, pay, showAnim);
				return;
			}
			if (data.type == ActivityWeekViewModule.ActViewType.RankRewardType)
			{
				this.rankCtrl.FreshTopRank();
				this.rankCtrl.RefreshSelfRank(this.rankRewardSelf, rankType);
				this.ShowRankReward(this.m_ActId, this.GetTimeBase(), true);
				return;
			}
			ActivityWeekViewModule.ActViewType type = data.type;
		}

		private void ShowActBanner(ActivityWeekViewModule.ActViewData data, bool finishAll)
		{
			this.img_Banner.SetImage(GameApp.Table.GetAtlasPath(this.commonCfg.atlasID), this.commonCfg.banner2);
			if (string.IsNullOrEmpty(this.commonCfg.JumpInterfaceName))
			{
				this.m_gotoText.gameObject.SetActive(false);
			}
			else
			{
				this.m_gotoText.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.commonCfg.JumpInterfaceName);
				this.m_gotoText.gameObject.SetActive(true);
			}
			this.UpdateLeftTime();
			if (data.type == ActivityWeekViewModule.ActViewType.ConsumeType)
			{
				TimeBase timeBase = this.GetTimeBase();
				this.m_roundText.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_currentRound") + Mathf.Min(timeBase.Round + 1, this.commonCfg.Round).ToString() + "/" + this.commonCfg.Round.ToString();
				this.roundItemRoot.SetActive(true);
				ulong roundRewardTimes = (ulong)timeBase.RoundRewardTimes;
				string[] finalReward = this.commonCfg.FinalReward;
				for (int i = 0; i < this.commonCfg.FinalReward.Length; i++)
				{
					this.roundItems[i].gameObject.SetActiveSafe(true);
					int num = int.Parse(finalReward[i].Split(',', StringSplitOptions.None)[0]);
					long num2 = long.Parse(finalReward[i].Split(',', StringSplitOptions.None)[1]);
					ItemData itemData = new ItemData(num, num2);
					this.roundItems[i].SetData(itemData.ToPropData());
					this.roundItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
					this.roundItems[i].OnRefresh();
				}
				this.FreshRoundCollectState(roundRewardTimes, finishAll);
				return;
			}
			this.m_roundText.text = "";
			this.roundItemRoot.SetActive(false);
		}

		private void UpdateLeftTime()
		{
			long num = this.GetTimeBase().ETime - DxxTools.Time.ServerTimestamp;
			if (num > 0L)
			{
				this.m_leftTimeText.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_lefttime", new object[] { DxxTools.FormatFullTimeWithDay(num) });
			}
			else
			{
				this.m_leftTimeText.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_end");
			}
			if (num <= 0L)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_end"));
				this.OnClickClose();
			}
		}

		private void onClickItem(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, item.transform.position, 70f);
		}

		public void OnClickActBt(int index)
		{
			if (this.m_selectIndex == index)
			{
				return;
			}
			this.OnSelectIndex(index);
		}

		private async void OnClickGotoBt()
		{
			if (this.commonCfg.JumpInterface > 0)
			{
				ViewJumpType viewJumpType = (ViewJumpType)this.commonCfg.JumpInterface;
				if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(viewJumpType, null, true))
				{
					if (viewJumpType == ViewJumpType.Tower)
					{
						if (!GameApp.Data.GetDataModule(DataName.TowerDataModule).IsAllFinish)
						{
							await Singleton<ViewJumpCtrl>.Instance.JumpTo(viewJumpType, null);
						}
						else
						{
							viewJumpType = ViewJumpType.RogueDungeon;
							await Singleton<ViewJumpCtrl>.Instance.JumpTo(viewJumpType, null);
						}
					}
					else
					{
						await Singleton<ViewJumpCtrl>.Instance.JumpTo(viewJumpType, null);
					}
					GameApp.View.CloseView(ViewName.ActivityWeekViewModule, null);
					ActivityWeekEntryViewModule viewModule = GameApp.View.GetViewModule(ViewName.ActivityWeekEntryModule);
					if (viewModule != null)
					{
						viewModule.SetBgVisible(true);
					}
				}
			}
		}

		private void FreshRoundCollectState(ulong rewardTimes, bool finishAll)
		{
			if (finishAll)
			{
				this.m_RoundUnFinishText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_finished");
				this.m_RoundCollectBtn.gameObject.SetActiveSafe(false);
				return;
			}
			if (rewardTimes > 0UL)
			{
				this.m_RoundUnFinishText.text = "";
				this.m_RoundCollectBtn.gameObject.SetActiveSafe(true);
				this.m_RoundCollectText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_collect");
				return;
			}
			this.m_RoundUnFinishText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_unfinish");
			this.m_RoundCollectBtn.gameObject.SetActiveSafe(false);
		}

		private void OnClickRoundCollectBtn()
		{
			this.Obj_NetLoading.SetActive(true);
			NetworkUtils.ActivityWeek.RequestActTimeReward(this.m_ActId, 2, this.m_ActId, null);
		}

		private void OnBtnInfoClick()
		{
			Module_moduleInfo elementById = GameApp.Table.GetManager().GetModule_moduleInfoModelInstance().GetElementById(this.systemId);
			if (elementById != null)
			{
				InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
				{
					m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId),
					m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID(this.commonCfg.ActvDes)
				};
				GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
			}
		}

		private void ShowConsume(int actId, Consume data, bool finishAllRound, bool playAnim = false)
		{
			ActivityWeekViewModule.<>c__DisplayClass72_0 CS$<>8__locals1 = new ActivityWeekViewModule.<>c__DisplayClass72_0();
			CS$<>8__locals1.commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actId);
			IList<CommonActivity_ConsumeObj> allElements = GameApp.Table.GetManager().GetCommonActivity_ConsumeObjModelInstance().GetAllElements();
			CS$<>8__locals1.groups = (from x in allElements
				where x.ObjGroup == CS$<>8__locals1.commonActivityData.QuestID
				orderby x.ObjNum
				select x).ToList<CommonActivity_ConsumeObj>();
			this.setNodeByCount(CS$<>8__locals1.groups.Count);
			if (playAnim)
			{
				this.PlayShowAnim();
			}
			CS$<>8__locals1.leftGroups = new List<CommonActivity_ConsumeObj>();
			int num = 0;
			int num2;
			if (CS$<>8__locals1.groups.Count > 0)
			{
				num2 = CS$<>8__locals1.groups.Last<CommonActivity_ConsumeObj>().ObjNum;
			}
			else
			{
				num2 = 0;
			}
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.groups.Count; i = i2 + 1)
			{
				int num3;
				int num4;
				if (finishAllRound)
				{
					num3 = 1;
					num4 = CS$<>8__locals1.groups[i].ObjNum;
				}
				else
				{
					num3 = ((data != null) ? data.TimeBase.EntryId.ToList<int>().FindAll((int x) => x == CS$<>8__locals1.groups[i].Id).Count<int>() : 0);
					num4 = ((data != null) ? (data.Score - data.TimeBase.Round * num2) : 0);
				}
				if (num3 >= 1)
				{
					CS$<>8__locals1.leftGroups.Add(CS$<>8__locals1.groups[i]);
				}
				else
				{
					int k = i;
					int num5 = num;
					this._catchList[num5].ShowConsume(actId, CS$<>8__locals1.groups[k], num3, num4, num2);
					num++;
				}
				i2 = i;
			}
			for (int j = 0; j < CS$<>8__locals1.leftGroups.Count; j++)
			{
				int i1 = j;
				int num6 = num;
				int num7;
				int num8;
				if (finishAllRound)
				{
					num7 = 1;
					num8 = CS$<>8__locals1.groups[j].ObjNum;
				}
				else
				{
					num7 = ((data != null) ? data.TimeBase.EntryId.ToList<int>().FindAll((int x) => x == CS$<>8__locals1.leftGroups[i1].Id).Count<int>() : 0);
					num8 = ((data != null) ? (data.Score - data.TimeBase.Round * num2) : 0);
				}
				this._catchList[num6].ShowConsume(actId, CS$<>8__locals1.leftGroups[i1], num7, num8, num2);
				num++;
			}
		}

		private void ShowShop(int actId, Shop data, bool playAnim = false)
		{
			ActivityWeekViewModule.<>c__DisplayClass73_0 CS$<>8__locals1 = new ActivityWeekViewModule.<>c__DisplayClass73_0();
			CS$<>8__locals1.commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actId);
			IList<CommonActivity_ShopObj> allElements = GameApp.Table.GetManager().GetCommonActivity_ShopObjModelInstance().GetAllElements();
			CS$<>8__locals1.groups = allElements.Where((CommonActivity_ShopObj x) => x.ObjGroup == CS$<>8__locals1.commonActivityData.ShopID).ToList<CommonActivity_ShopObj>();
			this.setNodeByCount(CS$<>8__locals1.groups.Count);
			if (playAnim)
			{
				this.PlayShowAnim();
			}
			CS$<>8__locals1.leftGroups = new List<CommonActivity_ShopObj>();
			int num = 0;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.groups.Count; i = i2 + 1)
			{
				int num2 = ((data != null) ? data.TimeBase.EntryId.ToList<int>().FindAll((int x) => x == CS$<>8__locals1.groups[i].id).Count<int>() : 0);
				if (CS$<>8__locals1.groups[i].objToplimit > 0 && num2 >= CS$<>8__locals1.groups[i].objToplimit)
				{
					CS$<>8__locals1.leftGroups.Add(CS$<>8__locals1.groups[i]);
				}
				else
				{
					int k = i;
					int num3 = num;
					this._catchList[num3].ShowShop(actId, CS$<>8__locals1.groups[k], num2);
					num++;
				}
				i2 = i;
			}
			for (int j = 0; j < CS$<>8__locals1.leftGroups.Count; j++)
			{
				int i1 = j;
				int num4 = num;
				int num5 = ((data != null) ? data.TimeBase.EntryId.ToList<int>().FindAll((int x) => x == CS$<>8__locals1.leftGroups[i1].id).Count<int>() : 0);
				this._catchList[num4].ShowShop(actId, CS$<>8__locals1.leftGroups[i1], num5);
				num++;
			}
		}

		private void ShowPay(int actId, Pay data, bool playAnim = false)
		{
			ActivityWeekViewModule.<>c__DisplayClass74_0 CS$<>8__locals1 = new ActivityWeekViewModule.<>c__DisplayClass74_0();
			CS$<>8__locals1.commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actId);
			IList<CommonActivity_PayObj> allElements = GameApp.Table.GetManager().GetCommonActivity_PayObjModelInstance().GetAllElements();
			CS$<>8__locals1.groups = allElements.Where((CommonActivity_PayObj x) => x.ObjGroup == CS$<>8__locals1.commonActivityData.PayID).ToList<CommonActivity_PayObj>();
			this.setNodeByCount(CS$<>8__locals1.groups.Count);
			if (playAnim)
			{
				this.PlayShowAnim();
			}
			CS$<>8__locals1.leftGroups = new List<CommonActivity_PayObj>();
			int num = 0;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.groups.Count; i = i2 + 1)
			{
				int num2 = ((data != null) ? data.TimeBase.EntryId.ToList<int>().FindAll((int x) => x == CS$<>8__locals1.groups[i].id).Count<int>() : 0);
				if (CS$<>8__locals1.groups[i].objToplimit > 0 && num2 >= CS$<>8__locals1.groups[i].objToplimit)
				{
					CS$<>8__locals1.leftGroups.Add(CS$<>8__locals1.groups[i]);
				}
				else
				{
					int k = i;
					int num3 = num;
					this._catchList[num3].ShowPay(actId, CS$<>8__locals1.groups[k], num2);
					num++;
				}
				i2 = i;
			}
			for (int j = 0; j < CS$<>8__locals1.leftGroups.Count; j++)
			{
				int i1 = j;
				int num4 = num;
				int num5 = ((data != null) ? data.TimeBase.EntryId.ToList<int>().FindAll((int x) => x == CS$<>8__locals1.leftGroups[i1].id).Count<int>() : 0);
				this._catchList[num4].ShowPay(actId, CS$<>8__locals1.leftGroups[i1], num5);
				num++;
			}
		}

		private void ShowRankReward(int actId, TimeBase timeBase, bool playAnim = false)
		{
			CommonActivity_CommonActivity commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actId);
			List<CommonActivity_RankObj> list = (from x in GameApp.Table.GetManager().GetCommonActivity_RankObjModelInstance().GetAllElements()
				where x.randID == commonActivityData.RankID
				select x).ToList<CommonActivity_RankObj>();
			this.setNodeByCount(list.Count);
			if (playAnim)
			{
				this.PlayShowAnim();
			}
			for (int i = 0; i < list.Count; i++)
			{
				this._catchList[i].ShowRankReward(actId, list[i]);
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
					ActivityWeekNode activityWeekNode = Object.Instantiate<ActivityWeekNode>(this.m_activityNode, this.content.transform);
					this._catchList.Add(activityWeekNode);
				}
			}
		}

		private const string ServerClockKey = "ActivityWeekViewModule";

		public int systemId = 110;

		public CustomButton Button_Close;

		public CustomButton buttonMask;

		public CustomButton m_btnInfo;

		public UIActivityCurrency currencyCtrl;

		public GameObject taskRoot;

		public UIActivityWeekRankCtrl rankCtrl;

		public GameObject taskTop;

		public GameObject rankTop;

		public UIActivityWeekRankItem rankRewardSelf;

		public RectTransform taskScrollNode;

		public float m_scrollTopHeight_Normal;

		public float m_scrollTopHeight_Rank_UseTask = -158f;

		public float m_scrollBottomHeight_Normal = 60f;

		public float m_scrollBottomHeight_Rank = 242f;

		public VerticalLayoutGroup elementLayout;

		public float m_layoutSpace_Normal = 16f;

		public float m_layoutSpace_Rank = 4f;

		public CustomImage img_Banner;

		public CustomText m_titleText;

		public CustomText m_leftTimeText;

		public CustomText m_roundText;

		public CustomText m_gotoText;

		public CustomButton m_gotoBt;

		public CustomButton m_RoundCollectBtn;

		public CustomText m_RoundCollectText;

		public CustomText m_RoundUnFinishText;

		public GameObject roundItemRoot;

		public List<UIItem> roundItems;

		public GameObject Obj_NetLoading;

		private SequencePool m_pool = new SequencePool();

		public UIHiddenWhenLessTwoActiveChild actBtnHiddenChecker;

		public List<ActivityWeekViewModule.ActViewData> m_actBtList = new List<ActivityWeekViewModule.ActViewData>();

		private List<ActivityWeekViewModule.ActViewData> m_ShowTabs = new List<ActivityWeekViewModule.ActViewData>();

		private int m_ActId;

		private CommonActivity_CommonActivity commonCfg;

		private ActivityWeekDataModule actDataModule;

		private int m_selectIndex = -1;

		public ActivityWeekNode m_activityNode;

		private List<ActivityWeekNode> _catchList;

		public RectTransform content;

		public enum ActViewType
		{
			ConsumeType = 1,
			ShopType,
			PayType,
			RankRewardType,
			RankOrderType
		}

		[Serializable]
		public class ActViewData
		{
			public ActivityWeekViewModule.ActViewType type;

			public GameObject obj;

			public CustomChooseButton btn;

			public RedNodeOneCtrl redNodeCtrl;
		}
	}
}
