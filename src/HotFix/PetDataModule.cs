using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Pet;
using Proto.User;
using Server;

namespace HotFix
{
	public class PetDataModule : IDataModule
	{
		public int AdvertisementResultCount { get; private set; } = 15;

		public long FreeDrawResetTimestamp { get; private set; }

		public long PetFreeDrawResetTimestamp { get; private set; }

		public MapField<uint, uint> DayDrawTimesCounter { get; private set; } = new MapField<uint, uint>();

		public List<ulong> PetShowRowIds { get; private set; } = new List<ulong>();

		public List<ulong> FormationRowIds { get; private set; } = new List<ulong> { 0UL, 0UL, 0UL };

		public List<PetCollectionData> PetCollectionDataList { get; private set; } = new List<PetCollectionData>();

		public int PetTrainingLv { get; private set; } = 1;

		public int PetTrainingExp { get; private set; }

		public int UnlockTrainingPosCount { get; private set; }

		public int GetName()
		{
			return 120;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_DayChange_Pet_DataPull, new HandlerEvent(this.OnDayChangePetDataPull));
			manager.RegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetData, new HandlerEvent(this.OnEventUpdatePetData));
			manager.RegisterEvent(LocalMessageName.CC_PetDataModule_FragmentMergePet, new HandlerEvent(this.OnEventFragmentMergePet));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_Pet_DataPull, new HandlerEvent(this.OnDayChangePetDataPull));
			manager.UnRegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetData, new HandlerEvent(this.OnEventUpdatePetData));
			manager.UnRegisterEvent(LocalMessageName.CC_PetDataModule_FragmentMergePet, new HandlerEvent(this.OnEventFragmentMergePet));
		}

		public void Reset()
		{
			this.FreeDrawResetTimestamp = 0L;
			this.DayDrawTimesCounter.Clear();
			this.PetShowRowIds.Clear();
			this.FormationRowIds = new List<ulong> { 0UL, 0UL, 0UL };
			this.m_addAttributeData.Clear();
		}

		public float GetLatestRefreshTime()
		{
			return 0f;
		}

		public Dictionary<int, int> GetPetStarData()
		{
			return new Dictionary<int, int>();
		}

		public int GetDrawTimesCounter(EDrawType drawType)
		{
			uint num;
			this.DayDrawTimesCounter.TryGetValue((uint)drawType, ref num);
			return (int)num;
		}

		public List<CardData> GetFightPetCardData()
		{
			List<CardData> list = new List<CardData>();
			List<ulong> fightPetRowIds = this.GetFightPetRowIds();
			int num = 0;
			for (int i = 0; i < fightPetRowIds.Count; i++)
			{
				ulong num2 = fightPetRowIds[i];
				PetData petData = this.GetPetData(num2);
				if (petData != null)
				{
					num++;
					AddAttributeData addAttributeData = petData.ToPetDto().MathPetAttributeData(petData.formationType, GameApp.Table.GetManager());
					MemberAttributeData memberAttributeData = new MemberAttributeData();
					memberAttributeData.MergeAttributes(addAttributeData.m_attributeDatas, false);
					CardData cardData = new CardData();
					cardData.m_rowID = (int)petData.petRowId;
					cardData.m_memberID = petData.memberId;
					cardData.m_instanceID = 150 + num;
					cardData.m_posIndex = (MemberPos)num;
					cardData.m_camp = MemberCamp.Friendly;
					cardData.SetMemberRace(MemberRace.Pet);
					cardData.m_memberAttributeData.CopyFrom(memberAttributeData);
					cardData.AddSkill(addAttributeData.m_skillIDs);
					cardData.m_petData = new SPetData();
					cardData.m_petData.petQuality = (EPetQuality)petData.quality;
					list.Add(cardData);
				}
			}
			return list;
		}

		public List<ulong> GetFightPetRowIds()
		{
			List<ulong> list = new List<ulong> { 0UL, 0UL, 0UL };
			foreach (PetData petData in this.m_petDataDict.Values)
			{
				if (petData.formationType == EPetFormationType.Fight1)
				{
					list[0] = petData.petRowId;
				}
				else if (petData.formationType == EPetFormationType.Fight2)
				{
					list[1] = petData.petRowId;
				}
				else if (petData.formationType == EPetFormationType.Fight3)
				{
					list[2] = petData.petRowId;
				}
			}
			return list;
		}

		public bool IsDeploy(ulong rowId)
		{
			return this.FormationRowIds.Contains(rowId);
		}

		public bool IsDeploy(EPetFormationType formationType)
		{
			int num = formationType - EPetFormationType.Fight1;
			return 0 <= num && num <= this.FormationRowIds.Count - 1 && this.FormationRowIds[num] > 0UL;
		}

		public bool IsHavePetInDeployList()
		{
			foreach (PetData petData in this.m_petDataDict.Values)
			{
				if (petData.formationType == EPetFormationType.Fight1 || petData.formationType == EPetFormationType.Fight2 || petData.formationType == EPetFormationType.Fight3)
				{
					return true;
				}
			}
			return false;
		}

		public void UpdatePetFormationData()
		{
			ulong num = this.FormationRowIds[0];
			ulong num2 = this.FormationRowIds[1];
			ulong num3 = this.FormationRowIds[2];
			for (int i = 0; i < this.FormationRowIds.Count; i++)
			{
				this.FormationRowIds[i] = 0UL;
			}
			foreach (PetData petData in this.m_petDataDict.Values)
			{
				if (petData != null)
				{
					if (petData.formationType == EPetFormationType.Fight1)
					{
						this.FormationRowIds[0] = petData.petRowId;
					}
					else if (petData.formationType == EPetFormationType.Fight2)
					{
						this.FormationRowIds[1] = petData.petRowId;
					}
					else if (petData.formationType == EPetFormationType.Fight3)
					{
						this.FormationRowIds[2] = petData.petRowId;
					}
				}
			}
			if (this.FormationRowIds[0] != num || this.FormationRowIds[1] != num2 || this.FormationRowIds[2] != num3)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Pet_FormationIdsChange, null);
			}
		}

		public PetCollectionData GetCollectionData(int groupId)
		{
			for (int i = 0; i < this.PetCollectionDataList.Count; i++)
			{
				PetCollectionData petCollectionData = this.PetCollectionDataList[i];
				if (petCollectionData.groupId.Equals(groupId))
				{
					return petCollectionData;
				}
			}
			return null;
		}

		public void UpdateCollectionData(List<uint> ids)
		{
			for (int i = 0; i < ids.Count; i++)
			{
				int num = (int)ids[i];
				this.UpdateCollectionData(num);
			}
		}

		public void UpdateCollectionData(int id)
		{
			int group = GameApp.Table.GetManager().GetPet_petCollectionModelInstance().GetElementById(id)
				.group;
			PetCollectionData collectionData = this.GetCollectionData(group);
			if (collectionData != null)
			{
				collectionData.UpdateCurrentId(id);
				collectionData.CheckCondition();
			}
		}

		private void UpdateCollectionCheckConditionResult()
		{
			for (int i = 0; i < this.PetCollectionDataList.Count; i++)
			{
				this.PetCollectionDataList[i].CheckCondition();
			}
		}

		private void InitPetCollections()
		{
			this.PetCollectionDataList.Clear();
			IList<Pet_petCollection> allElements = GameApp.Table.GetManager().GetPet_petCollectionModelInstance().GetAllElements();
			PetCollectionData petCollectionData = new PetCollectionData();
			for (int i = 0; i < allElements.Count; i++)
			{
				Pet_petCollection pet_petCollection = allElements[i];
				if (petCollectionData.groupId != pet_petCollection.group)
				{
					petCollectionData = new PetCollectionData();
					petCollectionData.groupId = pet_petCollection.group;
					petCollectionData.conditionStar = pet_petCollection.condition;
					petCollectionData.requirePetIds = new List<int>(pet_petCollection.petIDGroup);
					this.PetCollectionDataList.Add(petCollectionData);
				}
				petCollectionData.Add2Ids(pet_petCollection.id);
			}
		}

		public List<ulong> GetShowPetRowIds()
		{
			List<ulong> list = new List<ulong>();
			for (int i = 0; i < this.PetShowRowIds.Count; i++)
			{
				list.Add(this.PetShowRowIds[i]);
			}
			return list;
		}

		public bool IsDeployPosUnlock(EPetFormationType ePetFormationType)
		{
			bool flag = false;
			if (ePetFormationType == EPetFormationType.Fight1)
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.PetDeploy1, false);
			}
			else if (ePetFormationType == EPetFormationType.Fight2)
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.PetDeploy2, false);
			}
			else if (ePetFormationType == EPetFormationType.Fight3)
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.PetDeploy3, false);
			}
			return flag;
		}

		public PetData GetPetData(ulong rowId)
		{
			if (rowId <= 0UL)
			{
				return null;
			}
			foreach (PetData petData in this.m_petDataDict.Values)
			{
				if (petData.petRowId.Equals(rowId) || petData.fragmentRowId.Equals(rowId))
				{
					return petData;
				}
			}
			return null;
		}

		public List<PetData> GetPetList(EPetFilterType petFilterType)
		{
			List<PetData> list = new List<PetData>();
			foreach (PetData petData in this.m_petDataDict.Values)
			{
				if (petFilterType == EPetFilterType.All)
				{
					list.Add(petData);
				}
				if (petFilterType == EPetFilterType.AllPets && petData.PetItemType == EPetItemType.Pet)
				{
					list.Add(petData);
				}
				else if (petFilterType == EPetFilterType.Idle && petData.PetItemType == EPetItemType.Pet && petData.formationType == EPetFormationType.Idle)
				{
					list.Add(petData);
				}
				if (petFilterType == EPetFilterType.Assist && petData.PetItemType == EPetItemType.Pet && (petData.formationType == EPetFormationType.Fight1 || petData.formationType == EPetFormationType.Fight2 || petData.formationType == EPetFormationType.Fight3))
				{
					list.Add(petData);
				}
			}
			return list;
		}

		public PetData GetPetDataByConfigId(int configId)
		{
			PetData petData;
			if (this.m_petDataDict.TryGetValue(configId, out petData))
			{
				return petData;
			}
			return null;
		}

		public void UpdatePetDrawData(PetDrawResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.FreeDrawResetTimestamp = (long)resp.FreeRemindTime;
			this.DayDrawTimesCounter = resp.DayDrawTimes;
			this.UpdatePetsData(resp.AddPet, true, true);
			this.m_petDrawExpData.SetData((int)resp.Level, (int)resp.Exp);
			AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			if (dataModule != null)
			{
				dataModule.UpdateAdData(resp.AdDataDto);
			}
			this.UpdatePetInfo(resp.PetInfo);
		}

		public void InitServerData(PetInfo petInfo)
		{
			for (int i = 0; i < this.FormationRowIds.Count; i++)
			{
				this.FormationRowIds[i] = 0UL;
			}
			this.m_petDataDict.Clear();
			this.DayDrawTimesCounter.Clear();
			this.PetShowRowIds.Clear();
			this.PetCollectionDataList.Clear();
			this.UpdatePetInfo(petInfo);
			this.UpdatePetFormationData();
			this.MathAddAttributeData();
		}

		public void UpdateTrainingData(int lv, int exp)
		{
			this.PetTrainingLv = ((lv > 0) ? lv : 1);
			this.PetTrainingExp = exp;
			this.TryCheckSetTrainingPosCount();
		}

		private void TryCheckSetTrainingPosCount()
		{
			IList<Pet_PetTraining> allElements = GameApp.Table.GetManager().GetPet_PetTrainingModelInstance().GetAllElements();
			for (int i = allElements.Count - 1; i >= 0; i--)
			{
				Pet_PetTraining pet_PetTraining = allElements[i];
				if (this.PetTrainingLv >= pet_PetTraining.level)
				{
					this.UnlockTrainingPosCount = pet_PetTraining.id;
					return;
				}
			}
		}

		public void UpdatePetInfo(PetInfo petInfo)
		{
			if (petInfo == null)
			{
				return;
			}
			this.UpdateTrainingData((int)petInfo.TrainingLevel, (int)petInfo.TrainingExp);
			this.FreeDrawResetTimestamp = (long)petInfo.FreeTimeRemind;
			this.PetFreeDrawResetTimestamp = (long)petInfo.DayResetTimeStamp;
			this.InitPetCollections();
			this.UpdateCollectionData(petInfo.Fetters.ToList<uint>());
			this.m_petDrawExpData.SetData((int)petInfo.Level, (int)petInfo.Exp);
			this.DayDrawTimesCounter = petInfo.DayDrawTimes;
			int num = (int)(petInfo.AdvertDrawTimes + (uint)Singleton<GameConfig>.Instance.PeAdDrawResultCountMin);
			this.AdvertisementResultCount = Utility.Math.Min(num, Singleton<GameConfig>.Instance.PeAdDrawResultCountMax);
			this.UpdatePetsData(petInfo.Pets, false, false);
		}

		public void OnDayChangePetDataPull(object sender, int type, BaseEventArgs eventArgs)
		{
			NetworkUtils.Pet.PetInfoRefreshRequest(delegate(bool isOk, UserRefDataResponse resp)
			{
			});
		}

		private void OnEventUpdatePetData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsUpdatePetData eventArgsUpdatePetData = eventArgs as EventArgsUpdatePetData;
			if (eventArgsUpdatePetData == null)
			{
				return;
			}
			this.UpdatePetsData(eventArgsUpdatePetData.m_pets, true, false);
		}

		private void OnEventFragmentMergePet(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFragmentMergePet eventArgsFragmentMergePet = eventArgs as EventArgsFragmentMergePet;
			if (eventArgsFragmentMergePet == null)
			{
				return;
			}
			this.UpdatePetsData(eventArgsFragmentMergePet.addPets, true, false);
		}

		private void UpdatePetShowIds()
		{
			this.PetShowRowIds.Clear();
			foreach (PetData petData in this.m_petDataDict.Values)
			{
				if (petData.isShow > 0)
				{
					this.PetShowRowIds.Add(petData.petRowId);
				}
			}
		}

		private void UpdatePetsData(RepeatedField<PetDto> repeatedPets, bool isAutoShow, bool isAddData)
		{
			if (repeatedPets != null && repeatedPets.Count > 0)
			{
				List<ulong> list = new List<ulong>();
				List<int> list2 = new List<int>();
				for (int i = 0; i < repeatedPets.Count; i++)
				{
					PetDto petDto = repeatedPets[i];
					bool flag;
					PetData petData = this.UpdatePet(petDto, out flag, isAddData);
					if (petData != null)
					{
						if (petDto.PetType == 1U && flag)
						{
							list.Add(petDto.RowId);
						}
						if (!list2.Contains(petData.petId))
						{
							list2.Add(petData.petId);
						}
					}
				}
				foreach (PetData petData2 in this.m_petDataDict.Values)
				{
					if (list2.Contains(petData2.petId))
					{
						petData2.UpdateLocalData();
					}
				}
				this.UpdatePetShowIds();
				this.UpdateCollectionCheckConditionResult();
				if (isAutoShow)
				{
					this.NewPetCheckAndShow(list);
				}
			}
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Pet);
		}

		private PetData UpdatePet(PetDto petDto, out bool isNew, bool isAdd)
		{
			int petConfigId = petDto.GetPetConfigId(GameApp.Table.GetManager());
			if (petConfigId <= 0)
			{
				HLog.LogError("petdto configId is invalid!! petDto.ConfigId: " + petDto.ConfigId.ToString());
				isNew = false;
				return null;
			}
			PetData petData;
			if (this.m_petDataDict.TryGetValue(petConfigId, out petData))
			{
				isNew = petData.petRowId <= 0UL && petDto.PetType == 1U;
				if (isAdd)
				{
					petData.AddData(petDto);
				}
				else
				{
					petData.UpdateData(petDto);
				}
				return petData;
			}
			isNew = petDto.PetType == 1U;
			PetData petData2 = new PetData();
			petData2.UpdateData(petDto);
			this.m_petDataDict[petConfigId] = petData2;
			return petData2;
		}

		private void NewPetCheckAndShow(List<ulong> rowIds)
		{
			if (this.PetShowRowIds.Count >= Singleton<GameConfig>.Instance.PetShowMaxCount)
			{
				return;
			}
			if (rowIds == null || rowIds.Count <= 0)
			{
				return;
			}
			List<ulong> list = new List<ulong>();
			for (int i = 0; i < this.PetShowRowIds.Count; i++)
			{
				list.Add(this.PetShowRowIds[i]);
			}
			for (int j = 0; j < rowIds.Count; j++)
			{
				list.Add(rowIds[j]);
				if (list.Count >= Singleton<GameConfig>.Instance.PetShowMaxCount)
				{
					break;
				}
			}
			NetworkUtils.Pet.PetShowRequest(list, delegate(bool isOk, PetShowResponse resp)
			{
			}, false);
		}

		public List<PetData> SortPetList(List<PetData> list, EPetSortType sortType)
		{
			IOrderedEnumerable<PetData> orderedEnumerable = null;
			if (sortType != EPetSortType.Quality)
			{
				if (sortType == EPetSortType.Combat)
				{
					orderedEnumerable = from item in list
						orderby item.combat descending, item.quality descending, item.petId descending
						select item;
				}
			}
			else
			{
				orderedEnumerable = from item in list
					orderby item.quality descending, item.combat descending, item.petId descending
					select item;
			}
			return orderedEnumerable.ToList<PetData>();
		}

		public double MathCombatData(PetData petData)
		{
			CombatData combatData = new CombatData();
			AddAttributeData addAttributeData = petData.ToPetDto().MathPetAttributeData(petData.formationType, GameApp.Table.GetManager());
			List<int> skillIDs = addAttributeData.m_skillIDs;
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(addAttributeData.m_attributeDatas, false);
			combatData.MathCombat(GameApp.Table.GetManager(), memberAttributeData, skillIDs);
			return combatData.CurComba;
		}

		public void MathAddAttributeData()
		{
			this.m_addAttributeData.Clear();
			AddAttributePet addAttributePet = new AddAttributePet(GameApp.Table.GetManager());
			List<uint> list = new List<uint>();
			List<PetDto> list2 = new List<PetDto>();
			foreach (PetData petData in this.m_petDataDict.Values)
			{
				if (petData.PetItemType == EPetItemType.Pet)
				{
					list2.Add(petData.ToPetDto());
				}
			}
			addAttributePet.SetData(list2, list);
			this.m_addAttributeData.Merge(addAttributePet.MathAll());
		}

		public PetDrawExpData m_petDrawExpData = new PetDrawExpData();

		public Dictionary<int, PetData> m_petDataDict = new Dictionary<int, PetData>();

		public int MaxPetFormation = 3;

		public AddAttributeData m_addAttributeData = new AddAttributeData();

		public bool IsSkipAni;
	}
}
