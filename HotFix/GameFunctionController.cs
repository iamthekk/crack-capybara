using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.GameTestTools;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class GameFunctionController : Singleton<GameFunctionController>
	{
		private FunctionDataModule DataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.FunctionDataModule);
			}
		}

		public bool IsFunctionOpened(int functionid, bool isShowTip = false)
		{
			bool flag = this.DataModule.IsFunctionOpened(functionid);
			if (!flag && isShowTip)
			{
				string lockTips = this.GetLockTips(functionid.GetHashCode());
				if (!string.IsNullOrEmpty(lockTips))
				{
					GameApp.View.ShowStringTip(lockTips);
				}
			}
			return flag;
		}

		public bool IsFunctionOpened(FunctionID functionid, bool isShowTip = false)
		{
			bool flag = this.DataModule.IsFunctionOpened(functionid);
			if (!flag && isShowTip)
			{
				string lockTips = this.GetLockTips(functionid.GetHashCode());
				if (!string.IsNullOrEmpty(lockTips))
				{
					GameApp.View.ShowStringTip(lockTips);
				}
			}
			return flag;
		}

		public bool CheckFunctionOpen(FunctionData functiondata)
		{
			if (!this.DataModule.CheckCloudDataOpen(functiondata.ID))
			{
				return false;
			}
			if (this.DataModule.IsServerClose(functiondata.ID))
			{
				return false;
			}
			if (functiondata.OpenTimeSecond > 0)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule.ServerOpenMidNightTimestamp > 0L)
				{
					long num = DxxTools.Time.ServerTimestamp - dataModule.ServerOpenMidNightTimestamp;
					if ((long)functiondata.OpenTimeSecond > num)
					{
						return false;
					}
				}
			}
			FunctionUnlockType unlockType = functiondata.UnlockType;
			switch (unlockType)
			{
			case FunctionUnlockType.ChapterMapMaxLevel:
			{
				if (functiondata.UnlockArgs.Equals("") || functiondata.UnlockArgs.Equals("0"))
				{
					return true;
				}
				string[] array = functiondata.UnlockArgs.Split(',', StringSplitOptions.None);
				if (array.Length < 2)
				{
					return true;
				}
				int argsToInt = this.GetArgsToInt(array[0]);
				int argsToInt2 = this.GetArgsToInt(array[1]);
				return GameApp.Data.GetDataModule(DataName.ChapterDataModule).IsPassChapterStage(argsToInt, argsToInt2);
			}
			case FunctionUnlockType.Tower:
			{
				TowerDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TowerDataModule);
				int num2 = dataModule2.CalculateShouldChallengeLevelID(dataModule2.CompleteTowerLevelId);
				return this.GetArgsToInt(functiondata.UnlockArgs) < num2;
			}
			case FunctionUnlockType.GetEquip:
				return GameApp.Data.GetDataModule(DataName.EquipDataModule).IsHaveIdleEquip();
			case FunctionUnlockType.PetDrawLevel:
			{
				PetDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PetDataModule);
				int argsToInt3 = this.GetArgsToInt(functiondata.UnlockArgs);
				return dataModule3.m_petDrawExpData.DrawLevel >= argsToInt3;
			}
			case FunctionUnlockType.EnterNewWorld:
			{
				NewWorldDataModule dataModule4 = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
				int argsToInt4 = this.GetArgsToInt(functiondata.UnlockArgs);
				return dataModule4.EnterNewWorldSign >= argsToInt4;
			}
			case FunctionUnlockType.TalentMaxLevel:
				return GameApp.Data.GetDataModule(DataName.TalentDataModule).IsCheckMaxLevel();
			default:
				return unlockType != FunctionUnlockType.ForceLock && false;
			}
		}

		private int GetArgsToInt(string args)
		{
			if (string.IsNullOrEmpty(args))
			{
				return 0;
			}
			int num;
			if (!int.TryParse(args, out num))
			{
				return 0;
			}
			return num;
		}

		public bool CheckNewFunctionOpen()
		{
			FunctionDataModule dataModule = this.DataModule;
			List<FunctionData> unlockList = dataModule.GetUnlockList();
			List<FunctionData> list = new List<FunctionData>();
			List<FunctionData> list2 = new List<FunctionData>();
			for (int i = 0; i < unlockList.Count; i++)
			{
				FunctionData functionData = unlockList[i];
				if (functionData.Status == FunctionOpenStatus.UnLocking)
				{
					if (functionData.IsShowOpenView)
					{
						list.Add(functionData);
					}
					else
					{
						list2.Add(functionData);
					}
				}
				else if (this.CheckFunctionOpen(functionData))
				{
					if (functionData.IsShowOpenView)
					{
						functionData.SetStatus(FunctionOpenStatus.UnLocking);
						list.Add(functionData);
					}
					else
					{
						list2.Add(functionData);
					}
				}
			}
			dataModule.SetUnlockedImmediately(list2);
			this.DataModule.SetUnlockingList(list);
			return list.Count > 0;
		}

		public bool CheckNewFunctionOpenSpecial(FunctionUnlockType unlockType, bool isOnlyCheckSilent)
		{
			FunctionDataModule dataModule = this.DataModule;
			List<FunctionData> unlockList = dataModule.GetUnlockList();
			List<FunctionData> list = new List<FunctionData>();
			List<FunctionData> list2 = new List<FunctionData>();
			for (int i = 0; i < unlockList.Count; i++)
			{
				FunctionData functionData = unlockList[i];
				if (functionData.Status == FunctionOpenStatus.UnLocking)
				{
					if (functionData.IsShowOpenView)
					{
						list.Add(functionData);
					}
					else
					{
						list2.Add(functionData);
					}
				}
				else if ((unlockType == FunctionUnlockType.None || unlockType == functionData.UnlockType) && (!isOnlyCheckSilent || !functionData.IsShowOpenView) && this.CheckFunctionOpen(functionData))
				{
					if (functionData.IsShowOpenView)
					{
						functionData.SetStatus(FunctionOpenStatus.UnLocking);
						list.Add(functionData);
					}
					else
					{
						list2.Add(functionData);
					}
				}
			}
			dataModule.SetUnlockedImmediately(list2);
			this.DataModule.SetUnlockingList(list);
			return list.Count > 0;
		}

		public bool SpecialCheckBattleNewFunctionOpen()
		{
			FunctionDataModule dataModule = this.DataModule;
			bool flag = false;
			if (!dataModule.IsFunctionOpened(FunctionID.BattleSpeedx2))
			{
				FunctionData functionData = dataModule.GetFunctionData(FunctionID.BattleSpeedx2);
				if (this.CheckFunctionOpen(functionData))
				{
					flag = true;
					dataModule.SetUnlockedImmediately(new List<FunctionData>((IEnumerable<FunctionData>)functionData));
				}
			}
			return flag;
		}

		public string GetLockTips(int functionid)
		{
			if (functionid == 1 && !this.DataModule.CheckCloudDataOpen(functionid))
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("4044");
			}
			FunctionData functionData = this.DataModule.GetFunctionData(functionid);
			if (functionData == null)
			{
				return "";
			}
			if (functionData.OpenTime > 0)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule.ServerOpenMidNightTimestamp > 0L)
				{
					long num = DxxTools.Time.ServerTimestamp - dataModule.ServerOpenMidNightTimestamp;
					if ((long)functionData.OpenTimeSecond > num)
					{
						return Singleton<LanguageManager>.Instance.GetInfoByID("function_open_tip_openserver", new object[] { functionData.OpenTime + 1 });
					}
				}
			}
			FunctionUnlockType unlockType = functionData.UnlockType;
			switch (unlockType)
			{
			case FunctionUnlockType.ChapterMapMaxLevel:
			{
				string[] array = functionData.UnlockArgs.Split(',', StringSplitOptions.None);
				int num2 = 0;
				int num3 = 0;
				if (array.Length >= 2)
				{
					num2 = this.GetArgsToInt(array[0]);
					num3 = this.GetArgsToInt(array[1]);
				}
				Chapter_chapter elementById = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(num2);
				int num4 = 0;
				if (elementById != null)
				{
					num4 = elementById.id;
					if (num3 == elementById.totalStage)
					{
						return Singleton<LanguageManager>.Instance.GetInfoByID("function_chapter_pass_locktip", new object[] { num4 });
					}
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID("function_chapter_stage_locktip", new object[] { num4, num3 });
			}
			case FunctionUnlockType.Tower:
			{
				int argsToInt = this.GetArgsToInt(functionData.UnlockArgs);
				TowerDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TowerDataModule);
				TowerChallenge_Tower towerConfigByLevelId = dataModule2.GetTowerConfigByLevelId(argsToInt);
				int towerConfigNum = dataModule2.GetTowerConfigNum(towerConfigByLevelId);
				int levelNumByLevelId = dataModule2.GetLevelNumByLevelId(argsToInt);
				return Singleton<LanguageManager>.Instance.GetInfoByID("function_tower_locktip", new object[] { towerConfigNum, levelNumByLevelId });
			}
			case FunctionUnlockType.GetEquip:
				return Singleton<LanguageManager>.Instance.GetInfoByID("function_equip_locktip");
			case FunctionUnlockType.PetDrawLevel:
			{
				int argsToInt2 = this.GetArgsToInt(functionData.UnlockArgs);
				return Singleton<LanguageManager>.Instance.GetInfoByID("function_petdraw_locktip", new object[] { argsToInt2 });
			}
			case FunctionUnlockType.EnterNewWorld:
				return Singleton<LanguageManager>.Instance.GetInfoByID("function_open_tip_newworld");
			case FunctionUnlockType.TalentMaxLevel:
				return Singleton<LanguageManager>.Instance.GetInfoByID("function_open_tip_talent");
			default:
				if (unlockType != FunctionUnlockType.ForceLock)
				{
					return Singleton<LanguageManager>.Instance.GetInfoByID("function_unlock_normal");
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID("function_lock_locktip");
			}
		}

		public bool HasWaitedNewFunction()
		{
			return this.DataModule.HasNewFunctionOpen();
		}

		public void SetFunctionTarget(string name, Transform tf)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			this.mFunctionOpenTargetDic[name] = tf;
		}

		public Transform TryGetFunctionTarget(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Transform transform;
			if (this.mFunctionOpenTargetDic.TryGetValue(name, out transform))
			{
				return transform;
			}
			return null;
		}

		public bool IsServerClose(int functionId)
		{
			return this.DataModule.IsServerClose(functionId);
		}

		[GameTestMethod("功能开启", "测试功能开启", "", 0)]
		private static void OnTestFunctionOpen()
		{
			FunctionID functionID = FunctionID.Friend;
			GameApp.Data.GetDataModule(DataName.FunctionDataModule).GetFunctionData(functionID).SetStatus(FunctionOpenStatus.Lock);
			Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen();
			if (Singleton<GameFunctionController>.Instance.HasWaitedNewFunction())
			{
				GameApp.View.OpenView(ViewName.FunctionOpenViewModule, null, 2, null, null);
			}
		}

		private Dictionary<string, Transform> mFunctionOpenTargetDic = new Dictionary<string, Transform>();
	}
}
