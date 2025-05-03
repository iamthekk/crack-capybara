using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Proto.User;
using UnityEngine;

namespace HotFix
{
	public class GameAFTools
	{
		public static GameAFTools Ins
		{
			get
			{
				if (GameAFTools.mToolIns == null)
				{
					GameAFTools.mToolIns = new GameAFTools();
				}
				return GameAFTools.mToolIns;
			}
		}

		public static void CreateAF()
		{
			GameAFTools.mToolIns = new GameAFTools();
		}

		public static void DestroyAF()
		{
			GameAFTools.mToolIns = null;
		}

		public void OnLogin(UserLoginResponse loginResp)
		{
			this.ResetAccountExtraADTimes();
			this.RefreshChapterProgress();
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (dataModule != null)
			{
				string accountId = dataModule.accountId;
				long userId = dataModule.userId;
				int accountTotalLoginDays = dataModule.AccountTotalLoginDays;
				DateTime dateTime = DxxTools.Time.UnixTimestampToDateTime(loginResp.Timestamp);
				DateTime dateTime2 = DxxTools.Time.UnixTimestampToDateTime((double)loginResp.TgaInfoDto.CreateTime);
				if (Mathf.Abs((float)(dateTime - dateTime2).TotalSeconds) < 10f)
				{
					GameAFExtend.Track_account_login_finish();
				}
				DateTime dateTime3 = DxxTools.Time.UnixTimestampToDateTime(loginResp.RegisterTimestamp);
				if (Mathf.Abs((float)(dateTime - dateTime3).TotalSeconds) < 10f)
				{
					GameAFExtend.Track_user_login_finish();
				}
				for (int i = 2; i <= 7; i++)
				{
					if (dataModule.AccountTotalLoginDays == i)
					{
						string accountKey = this.GetAccountKey(string.Format("s_day{0}", i));
						if (GameAFTools.GetInt(accountKey, 0) == 0)
						{
							GameAFExtend.Track_s_day(i);
							GameAFTools.SetInt(accountKey, 1);
						}
					}
				}
				DateTime dateTime4 = dateTime2.AddDays(1.0);
				if (dateTime.Date == dateTime4.Date)
				{
					string accountKey2 = this.GetAccountKey("retention_day1");
					if (GameAFTools.GetInt(accountKey2, 0) == 0)
					{
						GameAFExtend.Track_retention_day1();
						GameAFTools.SetInt(accountKey2, 1);
					}
				}
				this.PDay(2);
				this.PDay(7);
			}
		}

		public void OnADSuccess()
		{
			this.AddAccountExtraADTimes(1);
			AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			if (dataModule != null)
			{
				int num = dataModule.AccountTotalADTimes + this.GetAccountExtraADTimes();
				GameAFExtend.Track_rv(1);
				if (num % 5 == 0)
				{
					GameAFExtend.Track_rv(5);
				}
				if (num % 10 == 0)
				{
					GameAFExtend.Track_rv(10);
				}
				if (num % 20 == 0)
				{
					GameAFExtend.Track_rv(20);
				}
			}
		}

		public void OnPaySuccess(int configID)
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			if (dataModule != null)
			{
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(configID);
				IAP_platformID elementById2 = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(elementById.platformID);
				if (dataModule.AccountTotalPayTimes > 1)
				{
					GameAFExtend.Track_iaptimes_2();
				}
				if (dataModule.AccountTotalPayTimes == 1)
				{
					this.PStageByPay(10);
					this.PStageByPay(50);
					this.PStageByPay(100);
				}
				if (dataModule.AccountTotalPayTimes == 1)
				{
					this.PDay(2);
					this.PDay(7);
				}
				LoginDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule2 != null)
				{
					DateTime dateTime = DxxTools.Time.UnixTimestampToDateTime((double)DxxTools.Time.lastServerTimestamp);
					DateTime dateTime2 = DxxTools.Time.UnixTimestampToDateTime((double)dataModule2.AccountCreateTime);
					if ((dateTime - dateTime2).TotalHours < 24.0)
					{
						GameAFExtend.Track_iap_24h();
					}
				}
				if (elementById != null && elementById.productType == 8)
				{
					IAP_ChapterPacks elementById3 = GameApp.Table.GetManager().GetIAP_ChapterPacksModelInstance().GetElementById(elementById.id);
					if (elementById3 != null)
					{
						int chapterId = elementById3.chapterId;
						if (chapterId <= 5)
						{
							string text = string.Format("purchase_package_stage{0}", chapterId);
							if (GameAFTools.GetInt(this.GetAccountKey(text), 0) == 0)
							{
								GameAFExtend.Track_purchase_package_stage(chapterId);
								GameAFTools.SetInt(this.GetAccountKey(text), 1);
							}
						}
					}
				}
				if (dataModule.AccountTotalPayTimes >= 1 && dataModule.AccountTotalPayTimes <= 6)
				{
					GameAFExtend.Track_iap_times(dataModule.AccountTotalPayTimes);
				}
				if (elementById != null && (elementById.productType == 4 || elementById.productType == 5))
				{
					GameAFExtend.Track_iap_battlepass();
				}
				if (elementById2 != null && elementById2.price > 0.99f)
				{
					GameAFExtend.Track_purchase_r();
				}
			}
		}

		public void OnStartChapter(int chapter)
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if (dataModule != null && chapter == 1 && dataModule.ChapterID == 1 && dataModule.MaxStage == 0)
			{
				string accountKey = this.GetAccountKey("start_game");
				if (GameAFTools.GetInt(accountKey, 0) == 0)
				{
					GameAFExtend.Track_start_game();
					GameAFTools.SetInt(accountKey, 1);
				}
			}
		}

		public void OnEndChapter(bool finish, int chapter)
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			int chapterID = dataModule.ChapterID;
			int maxStage = dataModule.MaxStage;
			if (finish && (chapter == 10 || chapter == 50 || chapter == 100))
			{
				IAPDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				if (dataModule2 != null && dataModule2.AccountTotalPayTimes > 0)
				{
					string accountKey = this.GetAccountKey(string.Format("p-stage_{0}", chapter));
					if (GameAFTools.GetInt(accountKey, 0) == 0)
					{
						GameAFExtend.Track_p_stage(chapter);
						GameAFTools.SetInt(accountKey, 1);
					}
				}
			}
			if (chapter == this._cacheChapterID && this.stage_list.ContainsKey(chapter))
			{
				foreach (int num in this.stage_list[chapter])
				{
					if (num >= this._cacheMaxDay)
					{
						bool flag = true;
						if (finish)
						{
							flag = true;
						}
						else if (num > maxStage)
						{
							flag = false;
						}
						if (flag)
						{
							string accountKey2 = this.GetAccountKey(string.Format("stage_{0}_{1}", chapter, num));
							if (GameAFTools.GetInt(accountKey2, 0) == 0)
							{
								GameAFExtend.Track_stage(chapter, num);
								GameAFTools.SetInt(accountKey2, 1);
							}
						}
					}
				}
			}
			this.RefreshChapterProgress();
		}

		private void PStageByPay(int chapter)
		{
			if (chapter != 10 && chapter != 50 && chapter != 100)
			{
				return;
			}
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if (dataModule != null)
			{
				if (dataModule.ChapterID <= chapter)
				{
					return;
				}
				string accountKey = this.GetAccountKey(string.Format("p-stage_{0}", chapter));
				if (GameAFTools.GetInt(accountKey, 0) == 0)
				{
					GameAFExtend.Track_p_stage(chapter);
					GameAFTools.SetInt(accountKey, 1);
				}
			}
		}

		private void PDay(int day)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			IAPDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			if (dataModule != null && dataModule2 != null)
			{
				if (dataModule.AccountTotalLoginDays < day)
				{
					return;
				}
				if (dataModule2.AccountTotalPayTimes == 0)
				{
					return;
				}
				string accountKey = this.GetAccountKey(string.Format("p-day{0}", day));
				if (GameAFTools.GetInt(accountKey, 0) == 0)
				{
					GameAFExtend.Track_p_day(day);
					GameAFTools.SetInt(accountKey, 1);
				}
			}
		}

		private void RefreshChapterProgress()
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if (dataModule != null)
			{
				this._cacheChapterID = dataModule.ChapterID;
				this._cacheMaxDay = dataModule.MaxStage;
			}
		}

		public void ResetAccountExtraADTimes()
		{
			GameAFTools.SetInt(this.GetAccountKey("AccountExtraADCount"), 0);
		}

		public void AddAccountExtraADTimes(int count = 1)
		{
			int num = GameAFTools.GetInt(this.GetAccountKey("AccountExtraADCount"), 0);
			num += count;
			GameAFTools.SetInt(this.GetAccountKey("AccountExtraADCount"), num);
		}

		public int GetAccountExtraADTimes()
		{
			return GameAFTools.GetInt(this.GetAccountKey("AccountExtraADCount"), 0);
		}

		private string GetAccountKey(string key)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			return "AF_" + dataModule.accountId + "_" + key;
		}

		private static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		private static int GetInt(string key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		private static GameAFTools mToolIns;

		private Dictionary<int, List<int>> stage_list = new Dictionary<int, List<int>>
		{
			{
				1,
				new List<int> { 25, 40, 60 }
			},
			{
				2,
				new List<int> { 25, 40, 60 }
			},
			{
				3,
				new List<int> { 10, 20, 30 }
			},
			{
				4,
				new List<int> { 10, 20, 30 }
			},
			{
				5,
				new List<int> { 3, 5 }
			},
			{
				6,
				new List<int> { 60 }
			},
			{
				7,
				new List<int> { 60 }
			},
			{
				8,
				new List<int> { 30 }
			},
			{
				9,
				new List<int> { 30 }
			},
			{
				10,
				new List<int> { 5 }
			},
			{
				11,
				new List<int> { 60 }
			},
			{
				12,
				new List<int> { 60 }
			},
			{
				13,
				new List<int> { 30 }
			},
			{
				14,
				new List<int> { 30 }
			},
			{
				15,
				new List<int> { 5 }
			}
		};

		private int _cacheChapterID;

		private int _cacheMaxDay;

		private const string Key_AccountExtraADCount = "AccountExtraADCount";
	}
}
