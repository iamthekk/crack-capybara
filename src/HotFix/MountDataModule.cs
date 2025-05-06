using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Server;

namespace HotFix
{
	public class MountDataModule : IDataModule
	{
		public MountInfo MountInfo { get; private set; }

		public AddAttributeData AddAttributeData { get; private set; }

		public int GetName()
		{
			return 156;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void Reset()
		{
		}

		public void SetLoginData(MountInfo mountInfo, RepeatedField<MountItemDto> mountItemDtos)
		{
			if (mountInfo == null)
			{
				return;
			}
			this.AddAttributeData = new AddAttributeData();
			this.basicDataDic.Clear();
			IList<Mount_mountStage> allElements = GameApp.Table.GetManager().GetMount_mountStageModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				MountBasicData mountBasicData = new MountBasicData(allElements[i]);
				this.basicDataDic.Add(mountBasicData.Stage, mountBasicData);
			}
			this.advanceDataDic.Clear();
			IList<Mount_advanceMount> allElements2 = GameApp.Table.GetManager().GetMount_advanceMountModelInstance().GetAllElements();
			for (int j = 0; j < allElements2.Count; j++)
			{
				MountAdvanceData mountAdvanceData = new MountAdvanceData(allElements2[j]);
				this.advanceDataDic.Add(mountAdvanceData.ID, mountAdvanceData);
			}
			this.MountInfo = mountInfo;
			this.MountItemDtos = mountItemDtos;
			for (int k = 0; k < mountItemDtos.Count; k++)
			{
				MountItemDto mountItemDto = mountItemDtos[k];
				MountAdvanceData mountAdvanceData2;
				if (this.advanceDataDic.TryGetValue(mountItemDto.ConfigId, out mountAdvanceData2))
				{
					mountAdvanceData2.SetStar(mountItemDto.Star);
					mountAdvanceData2.SetUnLock();
				}
			}
			this.MathAddAttributeData();
		}

		public void UpdateMountInfo(MountInfo mountInfo)
		{
			if (mountInfo == null)
			{
				return;
			}
			this.MountInfo = mountInfo;
			this.MathAddAttributeData();
			GameApp.Event.DispatchNow(null, 145, null);
		}

		public void UpdateMountItemDto(MountItemDto mountItemDto)
		{
			int i = 0;
			while (i < this.MountItemDtos.Count)
			{
				if (this.MountItemDtos[i].ConfigId == mountItemDto.ConfigId)
				{
					this.MountItemDtos[i] = mountItemDto;
					MountAdvanceData mountAdvanceData;
					if (this.advanceDataDic.TryGetValue(mountItemDto.ConfigId, out mountAdvanceData))
					{
						mountAdvanceData.SetStar(mountItemDto.Star);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			this.MathAddAttributeData();
			GameApp.Event.DispatchNow(null, 145, null);
		}

		public void UpdateMountItemDtos(RepeatedField<MountItemDto> mountItemDtos)
		{
			this.MountItemDtos = mountItemDtos;
			for (int i = 0; i < mountItemDtos.Count; i++)
			{
				MountItemDto mountItemDto = mountItemDtos[i];
				MountAdvanceData mountAdvanceData;
				if (this.advanceDataDic.TryGetValue(mountItemDto.ConfigId, out mountAdvanceData))
				{
					mountAdvanceData.SetStar(mountItemDto.Star);
					mountAdvanceData.SetUnLock();
				}
			}
			this.MathAddAttributeData();
			GameApp.Event.DispatchNow(null, 145, null);
		}

		public List<MountBasicData> GetBasicDataList()
		{
			List<MountBasicData> list = this.basicDataDic.Values.ToList<MountBasicData>();
			list.Sort((MountBasicData a, MountBasicData b) => a.StageConfig.stage.CompareTo(b.StageConfig.stage));
			return list;
		}

		public MountBasicData GetCurrentBasicData()
		{
			MountBasicData mountBasicData;
			if (this.MountInfo != null && this.basicDataDic.TryGetValue((int)this.MountInfo.Stage, out mountBasicData))
			{
				return mountBasicData;
			}
			return null;
		}

		public MountBasicData GetNextBasicData()
		{
			if (this.MountInfo == null)
			{
				return null;
			}
			int basicMountMaxStage = this.GetBasicMountMaxStage();
			if ((ulong)this.MountInfo.Stage == (ulong)((long)basicMountMaxStage))
			{
				return this.GetCurrentBasicData();
			}
			MountBasicData mountBasicData;
			if (this.basicDataDic.TryGetValue((int)(this.MountInfo.Stage + 1U), out mountBasicData))
			{
				return mountBasicData;
			}
			return null;
		}

		public int GetBasicMountMaxStage()
		{
			return GameApp.Table.GetManager().GetMount_mountStageModelInstance().GetAllElements()
				.Last<Mount_mountStage>()
				.stage;
		}

		public Mount_mountLevel GetCurrentLevelInfo()
		{
			IList<Mount_mountLevel> allElements = GameApp.Table.GetManager().GetMount_mountLevelModelInstance().GetAllElements();
			if (this.MountInfo != null)
			{
				for (int i = 0; i < allElements.Count; i++)
				{
					Mount_mountLevel mount_mountLevel = allElements[i];
					if ((long)mount_mountLevel.stage == (long)((ulong)this.MountInfo.Stage) && (long)mount_mountLevel.star == (long)((ulong)this.MountInfo.Level))
					{
						return mount_mountLevel;
					}
				}
			}
			return allElements[0];
		}

		public Mount_mountLevel GetNextLevelInfo()
		{
			IList<Mount_mountLevel> allElements = GameApp.Table.GetManager().GetMount_mountLevelModelInstance().GetAllElements();
			Mount_mountLevel currentLevelInfo = this.GetCurrentLevelInfo();
			int num = allElements.IndexOf(currentLevelInfo);
			if (num + 1 < allElements.Count)
			{
				return allElements[num + 1];
			}
			return allElements[allElements.Count - 1];
		}

		public int GetMaxLevel()
		{
			if (this.MountInfo == null)
			{
				return 0;
			}
			List<Mount_mountLevel> list = new List<Mount_mountLevel>();
			IList<Mount_mountLevel> allElements = GameApp.Table.GetManager().GetMount_mountLevelModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if ((long)allElements[i].stage == (long)((ulong)this.MountInfo.Stage))
				{
					list.Add(allElements[i]);
				}
			}
			list.Sort((Mount_mountLevel a, Mount_mountLevel b) => a.star.CompareTo(b.star));
			return list[list.Count - 1].star;
		}

		public List<MountAdvanceData> GetAdvanceDataList()
		{
			List<MountAdvanceData> list = new List<MountAdvanceData>();
			List<MountAdvanceData> list2 = new List<MountAdvanceData>();
			List<MountAdvanceData> list3 = new List<MountAdvanceData>();
			List<MountAdvanceData> list4 = new List<MountAdvanceData>();
			foreach (MountAdvanceData mountAdvanceData in this.advanceDataDic.Values)
			{
				if (mountAdvanceData.IsUnlockEnabled())
				{
					list2.Add(mountAdvanceData);
				}
				else if (mountAdvanceData.IsUnlock)
				{
					list3.Add(mountAdvanceData);
				}
				else
				{
					list4.Add(mountAdvanceData);
				}
			}
			list2.Sort((MountAdvanceData a, MountAdvanceData b) => b.Config.quality.CompareTo(a.Config.quality));
			list3.Sort((MountAdvanceData a, MountAdvanceData b) => b.Config.quality.CompareTo(a.Config.quality));
			list4.Sort((MountAdvanceData a, MountAdvanceData b) => b.Config.quality.CompareTo(a.Config.quality));
			list.AddRange(list2);
			list.AddRange(list3);
			list.AddRange(list4);
			return list;
		}

		private void MathAddAttributeData()
		{
			this.AddAttributeData.Clear();
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			MountBasicData currentBasicData = this.GetCurrentBasicData();
			if (currentBasicData != null)
			{
				list.Add(currentBasicData.StageAttributeData);
			}
			Mount_mountLevel currentLevelInfo = this.GetCurrentLevelInfo();
			list.AddRange(currentLevelInfo.attribute.GetMergeAttributeData());
			int num = 0;
			foreach (MountAdvanceData mountAdvanceData in this.advanceDataDic.Values)
			{
				if (mountAdvanceData.IsUnlock)
				{
					if ((long)mountAdvanceData.ID == (long)((ulong)this.MountInfo.SkillMountId))
					{
						GameSkill_skill skill = mountAdvanceData.GetSkill();
						if (skill != null)
						{
							num = skill.id;
						}
					}
					MergeAttributeData currentAttribute = mountAdvanceData.GetCurrentAttribute();
					list.Add(currentAttribute);
				}
			}
			if (num > 0)
			{
				this.AddAttributeData.m_skillIDs = new List<int> { num };
			}
			this.AddAttributeData.m_attributeDatas = list.Merge();
		}

		public int GetMountMemberId(MountInfo mountInfo = null)
		{
			MountInfo mountInfo2 = mountInfo ?? this.MountInfo;
			int num = 0;
			if (mountInfo2 != null)
			{
				num = MountDataModule.CheckMountMemberId((int)mountInfo2.ConfigType, (int)mountInfo2.ConfigId);
			}
			return num;
		}

		public int GetMountModelId()
		{
			int mountMemberId = this.GetMountMemberId(null);
			if (mountMemberId > 0)
			{
				GameMember_member gameMember_member = GameApp.Table.GetManager().GetGameMember_member(mountMemberId);
				if (gameMember_member != null)
				{
					return gameMember_member.modelID;
				}
			}
			return 0;
		}

		public static int CheckMountMemberId(int enemyMountType, int enemyMountId)
		{
			int num = 0;
			if (enemyMountId > 0)
			{
				if (GameConfig.NotExist_Test)
				{
					num = 0;
				}
				else if (enemyMountType == 1)
				{
					Mount_mountStage mount_mountStage = GameApp.Table.GetManager().GetMount_mountStage(enemyMountId);
					if (mount_mountStage != null)
					{
						num = mount_mountStage.memberId;
					}
				}
				else if (enemyMountType == 2)
				{
					Mount_advanceMount mount_advanceMount = GameApp.Table.GetManager().GetMount_advanceMount(enemyMountId);
					if (mount_advanceMount != null)
					{
						num = mount_advanceMount.memberId;
					}
				}
			}
			return num;
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			if (eventArgsFunctionOpen.FunctionID == 53)
			{
				RedPointController.Instance.ReCalc("Equip", true);
				RedPointController.Instance.ReCalc("Equip.Mount.RideTag", true);
			}
		}

		private RepeatedField<MountItemDto> MountItemDtos;

		private Dictionary<int, MountBasicData> basicDataDic = new Dictionary<int, MountBasicData>();

		private Dictionary<int, MountAdvanceData> advanceDataDic = new Dictionary<int, MountAdvanceData>();
	}
}
