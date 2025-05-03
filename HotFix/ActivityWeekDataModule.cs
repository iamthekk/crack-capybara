using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using LocalModels.Model;
using Proto.Common;

namespace HotFix
{
	public class ActivityWeekDataModule : IDataModule
	{
		public RepeatedField<Consume> ConsumeData { get; private set; }

		public RepeatedField<Shop> ShopData { get; private set; }

		public RepeatedField<Drop> DropData { get; private set; }

		public RepeatedField<Pay> PayData { get; private set; }

		public RepeatedField<Chapter> ChapterData { get; private set; }

		public int GetName()
		{
			return 137;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_CommonActivity, new HandlerEvent(this.OnEventSetCommonActivityData));
			manager.RegisterEvent(LocalMessageName.CC_ActTimeActivity, new HandlerEvent(this.OnEventSetActTimeActivityData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_CommonActivity, new HandlerEvent(this.OnEventSetCommonActivityData));
			manager.UnRegisterEvent(LocalMessageName.CC_ActTimeActivity, new HandlerEvent(this.OnEventSetActTimeActivityData));
		}

		public void Reset()
		{
		}

		private void OnEventSetCommonActivityData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsActivityCommonData eventArgsActivityCommonData = eventargs as EventArgsActivityCommonData;
			if (eventArgsActivityCommonData == null)
			{
				return;
			}
			this.ConsumeData = eventArgsActivityCommonData.Response.ConsumeData;
			this.ShopData = eventArgsActivityCommonData.Response.ShopData;
			this.DropData = eventArgsActivityCommonData.Response.DropData;
			this.PayData = eventArgsActivityCommonData.Response.PayData;
			this.ChapterData = eventArgsActivityCommonData.Response.Chapter;
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_ActivityWeek_DataPull, null);
			RedPointController.Instance.ReCalc("Main.Activity_Week", true);
		}

		private void OnEventSetActTimeActivityData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsActTimeInfoData eventArgsActTimeInfoData = eventargs as EventArgsActTimeInfoData;
			if (eventArgsActTimeInfoData == null)
			{
				return;
			}
			this.ConsumeData = eventArgsActTimeInfoData.Response.ConsumeData;
			this.ShopData = eventArgsActTimeInfoData.Response.ShopData;
			this.DropData = eventArgsActTimeInfoData.Response.DropData;
			this.PayData = eventArgsActTimeInfoData.Response.PayData;
			this.ChapterData = eventArgsActTimeInfoData.Response.ChapterData;
			RedPointController.Instance.ReCalc("Main.Activity_Week", true);
		}

		public void CommonUpdateConsumeData(RepeatedField<Consume> consumeData)
		{
			if (consumeData == null)
			{
				return;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (Consume consume in this.ConsumeData)
			{
				dictionary[consume.TimeBase.ActId] = consume.Score;
			}
			this.ConsumeData = consumeData;
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (Consume consume2 in this.ConsumeData)
			{
				dictionary2[consume2.TimeBase.ActId] = consume2.Score;
			}
			foreach (KeyValuePair<int, int> keyValuePair in dictionary2)
			{
				int num;
				if (dictionary.ContainsKey(keyValuePair.Key))
				{
					num = keyValuePair.Value - dictionary[keyValuePair.Key];
				}
				else
				{
					num = keyValuePair.Value;
				}
				if (num > 0)
				{
					int key = keyValuePair.Key;
					CommonActivity_CommonActivity elementById = GameApp.Table.GetManager().GetCommonActivity_CommonActivityModelInstance().GetElementById(key);
					if (elementById != null)
					{
						string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById.Name);
						int value = keyValuePair.Value;
						GameApp.SDK.Analyze.Track_ActivityPoint(infoByID, num, value);
					}
				}
			}
			RedPointController.Instance.ReCalc("Main.Activity_Week", true);
		}

		public bool CanShow()
		{
			return this.IsAynActOpen();
		}

		private bool IsOpen()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Week, false);
		}

		private bool IsAynActOpen()
		{
			if (!this.IsOpen())
			{
				return false;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			if (this.DropData != null)
			{
				foreach (Drop drop in this.DropData)
				{
					if (drop.TimeBase.ETime - serverTimestamp > 0L && drop.TimeBase.STime < serverTimestamp)
					{
						return true;
					}
				}
			}
			if (this.ChapterData != null)
			{
				foreach (Chapter chapter in this.ChapterData)
				{
					if (chapter.TimeBase.ETime - serverTimestamp > 0L && chapter.TimeBase.STime < serverTimestamp)
					{
						return true;
					}
				}
			}
			if (this.ConsumeData != null)
			{
				foreach (Consume consume in this.ConsumeData)
				{
					if (consume.TimeBase.ETime - serverTimestamp > 0L && consume.TimeBase.STime < serverTimestamp)
					{
						return true;
					}
				}
			}
			if (this.ShopData != null)
			{
				foreach (Shop shop in this.ShopData)
				{
					if (shop.TimeBase.ETime - serverTimestamp > 0L && shop.TimeBase.STime < serverTimestamp)
					{
						return true;
					}
				}
			}
			if (this.PayData != null)
			{
				foreach (Pay pay in this.PayData)
				{
					if (pay.TimeBase.ETime - serverTimestamp > 0L && pay.TimeBase.STime < serverTimestamp)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool ShowAnyRed()
		{
			if (!this.IsOpen())
			{
				return false;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			if (this.DropData != null)
			{
				foreach (Drop drop in this.DropData)
				{
					if (drop.TimeBase.ETime - serverTimestamp > 0L && drop.TimeBase.STime <= serverTimestamp)
					{
						int actId = drop.TimeBase.ActId;
						if (this.ShowRed(actId, null))
						{
							return true;
						}
					}
				}
			}
			if (this.ChapterData != null)
			{
				foreach (Chapter chapter in this.ChapterData)
				{
					if (chapter.TimeBase.ETime - serverTimestamp > 0L && chapter.TimeBase.STime <= serverTimestamp)
					{
						int actId2 = chapter.TimeBase.ActId;
						if (this.ShowRed(actId2, null))
						{
							return true;
						}
					}
				}
			}
			if (this.ConsumeData != null)
			{
				foreach (Consume consume in this.ConsumeData)
				{
					if (consume.TimeBase.ETime - serverTimestamp > 0L && consume.TimeBase.STime <= serverTimestamp)
					{
						int actId3 = consume.TimeBase.ActId;
						if (this.ShowRed(actId3, null))
						{
							return true;
						}
					}
				}
			}
			if (this.ShopData != null)
			{
				foreach (Shop shop in this.ShopData)
				{
					if (shop.TimeBase.ETime - serverTimestamp > 0L && shop.TimeBase.STime <= serverTimestamp)
					{
						int actId4 = shop.TimeBase.ActId;
						if (this.ShowRed(actId4, null))
						{
							return true;
						}
					}
				}
			}
			if (this.PayData != null)
			{
				foreach (Pay pay in this.PayData)
				{
					if (pay.TimeBase.ETime - serverTimestamp > 0L && pay.TimeBase.STime <= serverTimestamp)
					{
						int actId5 = pay.TimeBase.ActId;
						if (this.ShowRed(actId5, null))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool ShowRed(int actId, CommonActivity_CommonActivityModel actCommonTab = null)
		{
			if (!this.IsOpen())
			{
				return false;
			}
			if (actCommonTab == null)
			{
				actCommonTab = GameApp.Table.GetManager().GetCommonActivity_CommonActivityModelInstance();
			}
			CommonActivity_CommonActivity elementById = actCommonTab.GetElementById(actId);
			if (elementById == null)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < ActivityWeekDataModule.redViewTypes.Length; i++)
			{
				switch (ActivityWeekDataModule.redViewTypes[i])
				{
				case ActivityWeekViewModule.ActViewType.ConsumeType:
					if (elementById.QuestID > 0)
					{
						Consume consume = null;
						if (this.ConsumeData != null)
						{
							foreach (Consume consume2 in this.ConsumeData)
							{
								if (consume2.TimeBase.ActId == actId)
								{
									consume = consume2;
									break;
								}
							}
						}
						flag = this.ConsumeShowRed(actId, consume);
					}
					break;
				case ActivityWeekViewModule.ActViewType.ShopType:
					if (elementById.Type == 2 && elementById.ShopID > 0)
					{
						Shop shop = null;
						if (this.ShopData != null)
						{
							foreach (Shop shop2 in this.ShopData)
							{
								if (shop2.TimeBase.ActId == actId)
								{
									shop = shop2;
									break;
								}
							}
						}
						flag = this.ShopShowRed(actId, shop);
					}
					break;
				case ActivityWeekViewModule.ActViewType.PayType:
					if (elementById.PayID > 0)
					{
						Pay pay = null;
						if (this.PayData != null)
						{
							foreach (Pay pay2 in this.PayData)
							{
								if (pay2.TimeBase.ActId == actId)
								{
									pay = pay2;
									break;
								}
							}
						}
						flag = this.PayShowRed(actId, pay);
					}
					break;
				}
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		public bool ConsumeShowRed(int actId, Consume data)
		{
			if (!this.IsOpen())
			{
				return false;
			}
			if (data != null)
			{
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				if (data.TimeBase.ETime <= serverTimestamp || data.TimeBase.STime > serverTimestamp)
				{
					return false;
				}
				if (data.TimeBase.RoundRewardTimes > 0L)
				{
					return true;
				}
			}
			CommonActivity_CommonActivity commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actId);
			if (commonActivityData == null)
			{
				return false;
			}
			if (data != null && data.TimeBase.Round >= commonActivityData.Round && data.TimeBase.RoundRewardTimes < 1L)
			{
				return false;
			}
			if (commonActivityData.QuestID < 1)
			{
				return false;
			}
			List<CommonActivity_ConsumeObj> list = (from x in GameApp.Table.GetManager().GetCommonActivity_ConsumeObjModelInstance().GetAllElements()
				where x.ObjGroup == commonActivityData.QuestID
				select x).ToList<CommonActivity_ConsumeObj>();
			if (list.Count < 1)
			{
				return false;
			}
			int objNum = list.Last<CommonActivity_ConsumeObj>().ObjNum;
			List<int> list2 = ((data != null) ? data.TimeBase.EntryId.ToList<int>() : null);
			for (int i = 0; i < list.Count; i++)
			{
				CommonActivity_ConsumeObj group = list[i];
				if (((list2 != null) ? list2.FindAll((int x) => x == group.Id).Count<int>() : 0) < 1 && data.Score - data.TimeBase.Round * objNum >= group.ObjNum)
				{
					return true;
				}
			}
			return false;
		}

		public bool ShopShowRed(int actId, Shop data)
		{
			if (!this.IsOpen())
			{
				return false;
			}
			CommonActivity_CommonActivity commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actId);
			if (commonActivityData == null || commonActivityData.ShopID < 1)
			{
				return false;
			}
			if (data != null)
			{
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				if (data.TimeBase.ETime <= serverTimestamp || data.TimeBase.STime > serverTimestamp)
				{
					return false;
				}
			}
			List<CommonActivity_ShopObj> list = (from x in GameApp.Table.GetManager().GetCommonActivity_ShopObjModelInstance().GetAllElements()
				where x.ObjGroup == commonActivityData.ShopID
				select x).ToList<CommonActivity_ShopObj>();
			List<int> list2 = ((data != null) ? data.TimeBase.EntryId.ToList<int>() : null);
			for (int i = 0; i < list.Count; i++)
			{
				CommonActivity_ShopObj group = list[i];
				int num = ((list2 != null) ? list2.FindAll((int x) => x == group.id).Count<int>() : 0);
				if ((group.objToplimit == 0 || num < group.objToplimit) && group.ObjPrice2 == 0)
				{
					return true;
				}
			}
			return false;
		}

		public bool PayShowRed(int actId, Pay data)
		{
			if (!this.IsOpen())
			{
				return false;
			}
			CommonActivity_CommonActivity commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actId);
			if (commonActivityData == null || commonActivityData.PayID < 1)
			{
				return false;
			}
			if (data != null)
			{
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				if (data.TimeBase.ETime <= serverTimestamp || data.TimeBase.STime > serverTimestamp)
				{
					return false;
				}
			}
			List<CommonActivity_PayObj> list = (from x in GameApp.Table.GetManager().GetCommonActivity_PayObjModelInstance().GetAllElements()
				where x.ObjGroup == commonActivityData.PayID
				select x).ToList<CommonActivity_PayObj>();
			List<int> list2 = ((data != null) ? data.TimeBase.EntryId.ToList<int>() : null);
			for (int i = 0; i < list.Count; i++)
			{
				CommonActivity_PayObj group = list[i];
				int num = ((list2 != null) ? list2.FindAll((int x) => x == group.id).Count<int>() : 0);
				float num2 = 0f;
				if (group.PurchaseId > 0)
				{
					num2 = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(group.PurchaseId)
						.price1;
				}
				if ((group.objToplimit == 0 || num < group.objToplimit) && num2 == 0f)
				{
					return true;
				}
			}
			return false;
		}

		public const int AdditionalShow_SECONDS = 0;

		private static readonly ActivityWeekViewModule.ActViewType[] redViewTypes = new ActivityWeekViewModule.ActViewType[]
		{
			ActivityWeekViewModule.ActViewType.ConsumeType,
			ActivityWeekViewModule.ActViewType.ShopType,
			ActivityWeekViewModule.ActViewType.PayType
		};
	}
}
