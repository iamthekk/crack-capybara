using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributeMount : BaseAddAttribute
	{
		public AddAttributeMount(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(MountInfo mountInfo, RepeatedField<MountItemDto> mountItemDtos)
		{
			this.mMountInfo = mountInfo;
			this.mMountItemDtos = mountItemDtos;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (this.mMountInfo == null)
			{
				return addAttributeData;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			Mount_mountStage mount_mountStage = null;
			IList<Mount_mountStage> allElements = this.m_tableManager.GetMount_mountStageModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if ((long)allElements[i].stage == (long)((ulong)this.mMountInfo.Stage))
				{
					mount_mountStage = allElements[i];
					break;
				}
			}
			if (mount_mountStage != null)
			{
				list.AddRange(mount_mountStage.attribute.GetMergeAttributeData());
				IList<Mount_mountLevel> allElements2 = this.m_tableManager.GetMount_mountLevelModelInstance().GetAllElements();
				Mount_mountLevel mount_mountLevel = null;
				for (int j = 0; j < allElements2.Count; j++)
				{
					Mount_mountLevel mount_mountLevel2 = allElements2[j];
					if ((long)mount_mountLevel2.stage == (long)((ulong)this.mMountInfo.Stage) && (long)mount_mountLevel2.star == (long)((ulong)this.mMountInfo.Level))
					{
						mount_mountLevel = mount_mountLevel2;
						break;
					}
				}
				if (mount_mountLevel != null)
				{
					list.AddRange(mount_mountLevel.attribute.GetMergeAttributeData());
				}
			}
			int num = 0;
			if (this.mMountItemDtos != null)
			{
				for (int k = 0; k < this.mMountItemDtos.Count; k++)
				{
					MountItemDto mountItemDto = this.mMountItemDtos[k];
					Mount_advanceMount elementById = this.m_tableManager.GetMount_advanceMountModelInstance().GetElementById(mountItemDto.ConfigId);
					if (elementById != null)
					{
						if ((long)mountItemDto.ConfigId == (long)((ulong)this.mMountInfo.SkillMountId))
						{
							if (mountItemDto.Star == elementById.maxStar)
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
								if (l < mountItemDto.Star)
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

		private MountInfo mMountInfo;

		private RepeatedField<MountItemDto> mMountItemDtos;
	}
}
