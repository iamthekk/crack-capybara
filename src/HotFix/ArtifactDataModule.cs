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
	public class ArtifactDataModule : IDataModule
	{
		public ArtifactInfo ArtifactInfo { get; private set; }

		public AddAttributeData AddAttributeData { get; private set; }

		public int GetName()
		{
			return 157;
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
			this.ArtifactInfo = null;
			this.AddAttributeData = null;
			this.ArtifactItemDtos.Clear();
			this.basicDataDic.Clear();
			this.advanceDataDic.Clear();
		}

		public void SetLoginData(ArtifactInfo artifactInfo, RepeatedField<ArtifactItemDto> artifactItemDtos)
		{
			if (artifactInfo == null)
			{
				return;
			}
			this.AddAttributeData = new AddAttributeData();
			this.basicDataDic.Clear();
			IList<Artifact_artifactStage> allElements = GameApp.Table.GetManager().GetArtifact_artifactStageModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				ArtifactBasicData artifactBasicData = new ArtifactBasicData(allElements[i]);
				this.basicDataDic.Add(artifactBasicData.Stage, artifactBasicData);
			}
			this.advanceDataDic.Clear();
			IList<Artifact_advanceArtifact> allElements2 = GameApp.Table.GetManager().GetArtifact_advanceArtifactModelInstance().GetAllElements();
			for (int j = 0; j < allElements2.Count; j++)
			{
				ArtifactAdvanceData artifactAdvanceData = new ArtifactAdvanceData(allElements2[j]);
				this.advanceDataDic.Add(artifactAdvanceData.ID, artifactAdvanceData);
			}
			this.ArtifactInfo = artifactInfo;
			this.ArtifactItemDtos = artifactItemDtos;
			for (int k = 0; k < artifactItemDtos.Count; k++)
			{
				ArtifactItemDto artifactItemDto = artifactItemDtos[k];
				ArtifactAdvanceData artifactAdvanceData2;
				if (this.advanceDataDic.TryGetValue(artifactItemDto.ConfigId, out artifactAdvanceData2))
				{
					artifactAdvanceData2.SetStar(artifactItemDto.Star);
					artifactAdvanceData2.SetUnLock();
				}
			}
			this.MathAddAttributeData();
		}

		public void UpdateArtifactInfo(ArtifactInfo artifactInfo)
		{
			if (artifactInfo == null)
			{
				return;
			}
			this.ArtifactInfo = artifactInfo;
			this.MathAddAttributeData();
			GameApp.Event.DispatchNow(null, 145, null);
		}

		public void UpdateArtifactItemDto(ArtifactItemDto artifactItemDto)
		{
			int i = 0;
			while (i < this.ArtifactItemDtos.Count)
			{
				if (this.ArtifactItemDtos[i].ConfigId == artifactItemDto.ConfigId)
				{
					this.ArtifactItemDtos[i] = artifactItemDto;
					ArtifactAdvanceData artifactAdvanceData;
					if (this.advanceDataDic.TryGetValue(artifactItemDto.ConfigId, out artifactAdvanceData))
					{
						artifactAdvanceData.SetStar(artifactItemDto.Star);
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

		public void UpdateArtifactItemDtos(RepeatedField<ArtifactItemDto> artifactItemDtos)
		{
			this.ArtifactItemDtos = artifactItemDtos;
			for (int i = 0; i < artifactItemDtos.Count; i++)
			{
				ArtifactItemDto artifactItemDto = artifactItemDtos[i];
				ArtifactAdvanceData artifactAdvanceData;
				if (this.advanceDataDic.TryGetValue(artifactItemDto.ConfigId, out artifactAdvanceData))
				{
					artifactAdvanceData.SetStar(artifactItemDto.Star);
					artifactAdvanceData.SetUnLock();
				}
			}
			this.MathAddAttributeData();
			GameApp.Event.DispatchNow(null, 145, null);
		}

		public List<ArtifactBasicData> GetBasicDataList()
		{
			List<ArtifactBasicData> list = this.basicDataDic.Values.ToList<ArtifactBasicData>();
			list.Sort((ArtifactBasicData a, ArtifactBasicData b) => a.StageConfig.stage.CompareTo(b.StageConfig.stage));
			return list;
		}

		public ArtifactBasicData GetCurrentBasicData()
		{
			ArtifactBasicData artifactBasicData;
			if (this.ArtifactInfo != null && this.basicDataDic.TryGetValue((int)this.ArtifactInfo.Stage, out artifactBasicData))
			{
				return artifactBasicData;
			}
			return null;
		}

		public ArtifactBasicData GetNextBasicData()
		{
			if (this.ArtifactInfo == null)
			{
				return null;
			}
			int basicArtifactMaxStage = this.GetBasicArtifactMaxStage();
			if ((ulong)this.ArtifactInfo.Stage == (ulong)((long)basicArtifactMaxStage))
			{
				return this.GetCurrentBasicData();
			}
			ArtifactBasicData artifactBasicData;
			if (this.basicDataDic.TryGetValue((int)(this.ArtifactInfo.Stage + 1U), out artifactBasicData))
			{
				return artifactBasicData;
			}
			return null;
		}

		public int GetBasicArtifactMaxStage()
		{
			return GameApp.Table.GetManager().GetMount_mountStageModelInstance().GetAllElements()
				.Last<Mount_mountStage>()
				.stage;
		}

		public Artifact_artifactLevel GetCurrentLevelInfo()
		{
			IList<Artifact_artifactLevel> allElements = GameApp.Table.GetManager().GetArtifact_artifactLevelModelInstance().GetAllElements();
			if (this.ArtifactInfo != null)
			{
				for (int i = 0; i < allElements.Count; i++)
				{
					Artifact_artifactLevel artifact_artifactLevel = allElements[i];
					if ((long)artifact_artifactLevel.stage == (long)((ulong)this.ArtifactInfo.Stage) && (long)artifact_artifactLevel.star == (long)((ulong)this.ArtifactInfo.Level))
					{
						return artifact_artifactLevel;
					}
				}
			}
			return allElements[0];
		}

		public Artifact_artifactLevel GetNextLevelInfo()
		{
			IList<Artifact_artifactLevel> allElements = GameApp.Table.GetManager().GetArtifact_artifactLevelModelInstance().GetAllElements();
			Artifact_artifactLevel currentLevelInfo = this.GetCurrentLevelInfo();
			int num = allElements.IndexOf(currentLevelInfo);
			if (num + 1 < allElements.Count)
			{
				return allElements[num + 1];
			}
			return allElements[allElements.Count - 1];
		}

		public int GetMaxLevel()
		{
			if (this.ArtifactInfo == null)
			{
				return 0;
			}
			List<Artifact_artifactLevel> list = new List<Artifact_artifactLevel>();
			IList<Artifact_artifactLevel> allElements = GameApp.Table.GetManager().GetArtifact_artifactLevelModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if ((long)allElements[i].stage == (long)((ulong)this.ArtifactInfo.Stage))
				{
					list.Add(allElements[i]);
				}
			}
			list.Sort((Artifact_artifactLevel a, Artifact_artifactLevel b) => a.star.CompareTo(b.star));
			return list[list.Count - 1].star;
		}

		public List<ArtifactAdvanceData> GetAdvanceDataList()
		{
			List<ArtifactAdvanceData> list = new List<ArtifactAdvanceData>();
			List<ArtifactAdvanceData> list2 = new List<ArtifactAdvanceData>();
			List<ArtifactAdvanceData> list3 = new List<ArtifactAdvanceData>();
			List<ArtifactAdvanceData> list4 = new List<ArtifactAdvanceData>();
			foreach (ArtifactAdvanceData artifactAdvanceData in this.advanceDataDic.Values)
			{
				if (artifactAdvanceData.IsUnlockEnabled())
				{
					list2.Add(artifactAdvanceData);
				}
				else if (artifactAdvanceData.IsUnlock)
				{
					list3.Add(artifactAdvanceData);
				}
				else
				{
					list4.Add(artifactAdvanceData);
				}
			}
			list2.Sort((ArtifactAdvanceData a, ArtifactAdvanceData b) => b.ItemConfig.quality.CompareTo(a.ItemConfig.quality));
			list3.Sort((ArtifactAdvanceData a, ArtifactAdvanceData b) => b.ItemConfig.quality.CompareTo(a.ItemConfig.quality));
			list4.Sort((ArtifactAdvanceData a, ArtifactAdvanceData b) => b.ItemConfig.quality.CompareTo(a.ItemConfig.quality));
			list.AddRange(list2);
			list.AddRange(list3);
			list.AddRange(list4);
			return list;
		}

		private void MathAddAttributeData()
		{
			this.AddAttributeData.Clear();
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			ArtifactBasicData currentBasicData = this.GetCurrentBasicData();
			list.Add(currentBasicData.StageAttributeData);
			Artifact_artifactLevel currentLevelInfo = this.GetCurrentLevelInfo();
			list.AddRange(currentLevelInfo.attribute.GetMergeAttributeData());
			int num = 0;
			foreach (ArtifactAdvanceData artifactAdvanceData in this.advanceDataDic.Values)
			{
				if (artifactAdvanceData.IsUnlock)
				{
					if ((long)artifactAdvanceData.ID == (long)((ulong)this.ArtifactInfo.SkillArtifactId))
					{
						GameSkill_skill skill = artifactAdvanceData.GetSkill();
						if (skill != null)
						{
							num = skill.id;
						}
					}
					MergeAttributeData currentAttribute = artifactAdvanceData.GetCurrentAttribute();
					list.Add(currentAttribute);
				}
			}
			if (num > 0)
			{
				this.AddAttributeData.m_skillIDs = new List<int> { num };
			}
			this.AddAttributeData.m_attributeDatas = list.Merge();
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			if (eventArgsFunctionOpen.FunctionID == 54)
			{
				RedPointController.Instance.ReCalc("Equip", true);
			}
		}

		private RepeatedField<ArtifactItemDto> ArtifactItemDtos = new RepeatedField<ArtifactItemDto>();

		private Dictionary<int, ArtifactBasicData> basicDataDic = new Dictionary<int, ArtifactBasicData>();

		private Dictionary<int, ArtifactAdvanceData> advanceDataDic = new Dictionary<int, ArtifactAdvanceData>();
	}
}
