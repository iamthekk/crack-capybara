using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Proto.Common;
using Server;

namespace HotFix.RedPoint.Calculators
{
	public class RPEquip
	{
		public class Artifact : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Artifact_UpgradeTag : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Artifact))
				{
					return 0;
				}
				ArtifactDataModule dataModule = GameApp.Data.GetDataModule(DataName.ArtifactDataModule);
				Artifact_artifactLevel currentLevelInfo = dataModule.GetCurrentLevelInfo();
				Artifact_artifactLevel nextLevelInfo = dataModule.GetNextLevelInfo();
				if ((currentLevelInfo.stage != nextLevelInfo.stage || currentLevelInfo.star != nextLevelInfo.star) && GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)currentLevelInfo.itemCost) >= (long)currentLevelInfo.levelCost)
				{
					return 1;
				}
				return 0;
			}
		}

		public class Artifact_AdvanceTag : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Artifact))
				{
					return 0;
				}
				List<ArtifactAdvanceData> advanceDataList = GameApp.Data.GetDataModule(DataName.ArtifactDataModule).GetAdvanceDataList();
				for (int i = 0; i < advanceDataList.Count; i++)
				{
					if (advanceDataList[i].IsRedPoint())
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class Collection : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class CollectionTabMain : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Collection, false))
				{
					return 0;
				}
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Collection))
				{
					return 0;
				}
				foreach (CollectionData collectionData in GameApp.Data.GetDataModule(DataName.CollectionDataModule).collectionDict.Values)
				{
					if (collectionData.IsCanMerge)
					{
						return 1;
					}
					if (collectionData.IsMatchStarUpgradeCondition())
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class CollectionTabSuit : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class CollectionTabStarUpgrade : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Collection, false))
				{
					return 0;
				}
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Collection))
				{
					return 0;
				}
				using (Dictionary<int, CollectionData>.ValueCollection.Enumerator enumerator = GameApp.Data.GetDataModule(DataName.CollectionDataModule).collectionDict.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsMatchStarUpgradeCondition())
						{
							return 1;
						}
					}
				}
				return 0;
			}
		}

		public class Equip : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				int num = 0;
				int num2 = 0;
				if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Equip, false))
				{
					EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
					bool flag = false;
					foreach (EquipData equipData in dataModule.m_equipDatas.Values)
					{
						if (!dataModule.IsPutOn(equipData.rowID))
						{
							int equipTypeMaxCount = EquipTypeHelper.GetEquipTypeMaxCount(equipData.equipType);
							for (int i = 1; i <= equipTypeMaxCount; i++)
							{
								if (dataModule.NeedEquipRedTip(equipData.equipType, equipData.composeId))
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						num2 |= 2;
						num++;
					}
					if (dataModule.IsHaveCanLevelUp())
					{
						num2 |= 8;
						num++;
					}
					if (dataModule.IsHaveCanEvolution())
					{
						num2 |= 8;
						num++;
					}
				}
				record.UpdateSelfRedPriorityValue(num2);
				return num;
			}

			public class EquipCompose : IRedPointRecordCalculator
			{
				public int CalcRedPoint(RedPointDataRecord record)
				{
					int num = 0;
					int num2 = 0;
					if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Equip, false))
					{
						EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
						foreach (EquipData equipData in dataModule.m_equipDatas.Values)
						{
							if (dataModule.IsCanMerge(equipData.rowID))
							{
								num++;
								num2 |= 16;
								break;
							}
						}
					}
					record.UpdateSelfRedPriorityValue(num2);
					return num;
				}
			}
		}

		public class Fashion : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule.IsAvatarOrFrameRedNode())
				{
					return 1;
				}
				if (dataModule.IsClothesRedNode())
				{
					return 1;
				}
				if (dataModule.IsSceneSkinRedNode())
				{
					return 1;
				}
				return 0;
			}
		}

		public class Mount : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Mount_RideTag : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Mount))
				{
					return 0;
				}
				MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
				MountInfo mountInfo = dataModule.MountInfo;
				if (dataModule.GetBasicDataList().Count <= 0)
				{
					return 0;
				}
				string mountRideRed = PlayerPrefsKeys.GetMountRideRed();
				if (mountInfo != null && mountInfo.ConfigId <= 0U && string.IsNullOrEmpty(mountRideRed))
				{
					return 1;
				}
				return 0;
			}
		}

		public class Mount_UpgradeTag : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Mount))
				{
					return 0;
				}
				MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
				Mount_mountLevel currentLevelInfo = dataModule.GetCurrentLevelInfo();
				Mount_mountLevel nextLevelInfo = dataModule.GetNextLevelInfo();
				if ((currentLevelInfo.stage != nextLevelInfo.stage || currentLevelInfo.star != nextLevelInfo.star) && GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)currentLevelInfo.itemCost) >= (long)currentLevelInfo.levelCost)
				{
					return 1;
				}
				return 0;
			}
		}

		public class Mount_AdvanceTag : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Mount))
				{
					return 0;
				}
				List<MountAdvanceData> advanceDataList = GameApp.Data.GetDataModule(DataName.MountDataModule).GetAdvanceDataList();
				for (int i = 0; i < advanceDataList.Count; i++)
				{
					if (advanceDataList[i].IsRedPoint())
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class Pet : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class PetRanch : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Pet, false))
				{
					return 0;
				}
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Pet))
				{
					return 0;
				}
				AdData adData = GameApp.Data.GetDataModule(DataName.AdDataModule).GetAdData(8);
				if (adData == null)
				{
					return 0;
				}
				if (adData.watchCount < adData.watchCountMax)
				{
					return 1;
				}
				int pet15DrawTicketCost = Singleton<GameConfig>.Instance.Pet15DrawTicketCost;
				if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid(11UL) >= (long)pet15DrawTicketCost)
				{
					return 1;
				}
				return 0;
			}
		}

		public class PetCollection : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class PetList : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Pet, false))
				{
					return 0;
				}
				if (!GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(FunctionID.Pet))
				{
					return 0;
				}
				foreach (PetData petData in GameApp.Data.GetDataModule(DataName.PetDataModule).m_petDataDict.Values)
				{
					if (petData.PetItemType == EPetItemType.Pet && petData.formationType > EPetFormationType.Idle && petData.IsCanLevelUp(petData.level) && petData.HasEnoughLevelUpCost())
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public class Pictorial : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}

		public class Relic : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				return 0;
			}
		}
	}
}
