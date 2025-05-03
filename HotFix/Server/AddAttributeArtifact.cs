using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributeArtifact : BaseAddAttribute
	{
		public AddAttributeArtifact(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(ArtifactInfo artifactInfo, RepeatedField<ArtifactItemDto> artifactItemDtos)
		{
			this.mArtifactInfo = artifactInfo;
			this.mArtifactItemDtos = artifactItemDtos;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (this.mArtifactInfo == null)
			{
				return addAttributeData;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			Artifact_artifactStage artifact_artifactStage = null;
			IList<Artifact_artifactStage> allElements = this.m_tableManager.GetArtifact_artifactStageModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if ((long)allElements[i].stage == (long)((ulong)this.mArtifactInfo.Stage))
				{
					artifact_artifactStage = allElements[i];
					break;
				}
			}
			if (artifact_artifactStage != null)
			{
				list.AddRange(artifact_artifactStage.attribute.GetMergeAttributeData());
				IList<Artifact_artifactLevel> allElements2 = this.m_tableManager.GetArtifact_artifactLevelModelInstance().GetAllElements();
				Artifact_artifactLevel artifact_artifactLevel = null;
				for (int j = 0; j < allElements2.Count; j++)
				{
					Artifact_artifactLevel artifact_artifactLevel2 = allElements2[j];
					if ((long)artifact_artifactLevel2.stage == (long)((ulong)this.mArtifactInfo.Stage) && (long)artifact_artifactLevel2.star == (long)((ulong)this.mArtifactInfo.Level))
					{
						artifact_artifactLevel = artifact_artifactLevel2;
						break;
					}
				}
				if (artifact_artifactLevel != null)
				{
					list.AddRange(artifact_artifactLevel.attribute.GetMergeAttributeData());
				}
			}
			int num = 0;
			if (this.mArtifactItemDtos != null)
			{
				for (int k = 0; k < this.mArtifactItemDtos.Count; k++)
				{
					ArtifactItemDto artifactItemDto = this.mArtifactItemDtos[k];
					Artifact_advanceArtifact elementById = this.m_tableManager.GetArtifact_advanceArtifactModelInstance().GetElementById(artifactItemDto.ConfigId);
					if (elementById != null)
					{
						if ((long)artifactItemDto.ConfigId == (long)((ulong)this.mArtifactInfo.SkillArtifactId))
						{
							if (artifactItemDto.Star == elementById.maxStar)
							{
								num = elementById.maxStarSkill;
							}
							else
							{
								num = elementById.initSkill;
							}
						}
						MergeAttributeData mergeAttributeData = null;
						List<MergeAttributeData> mergeAttributeData2 = elementById.attribute.GetMergeAttributeData();
						if (mergeAttributeData2.Count > 0)
						{
							mergeAttributeData = mergeAttributeData2[0];
						}
						if (mergeAttributeData != null)
						{
							long num2 = mergeAttributeData.Value.GetValue();
							for (int l = 0; l < elementById.levelAttribute.Length; l++)
							{
								if (l < artifactItemDto.Star)
								{
									num2 += (long)elementById.levelAttribute[l];
								}
							}
							MergeAttributeData mergeAttributeData3 = new MergeAttributeData(mergeAttributeData.Header + "=" + num2.ToString(), null, null);
							list.Add(mergeAttributeData3);
						}
					}
				}
			}
			if (num > 0)
			{
				addAttributeData.m_skillIDs = new List<int> { num };
			}
			addAttributeData.m_attributeDatas = list.Merge();
			return addAttributeData;
		}

		private ArtifactInfo mArtifactInfo;

		private RepeatedField<ArtifactItemDto> mArtifactItemDtos;
	}
}
