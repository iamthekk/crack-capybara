using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributeCollection : BaseAddAttribute
	{
		public AddAttributeCollection(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(RepeatedField<CollectionDto> dtos, UserStatisticInfo userStatisticInfo)
		{
			this.dtos = dtos;
			this.userStatisticInfo = userStatisticInfo;
		}

		private long GetConditionDataCount(uint conditionId)
		{
			if (this.userStatisticInfo == null || this.userStatisticInfo.DataMap == null || this.userStatisticInfo.DataMap.Count <= 0)
			{
				return 0L;
			}
			ulong num;
			if (this.userStatisticInfo.DataMap.TryGetValue(conditionId, ref num))
			{
				return (long)num;
			}
			return 0L;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (this.dtos == null)
			{
				return addAttributeData;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			for (int i = 0; i < this.dtos.Count; i++)
			{
				CollectionDto collectionDto = this.dtos[i];
				if (collectionDto != null)
				{
					int configId = (int)collectionDto.ConfigId;
					int num = this.m_tableManager.GetCollection_collectionModelInstance().GetElementById(configId).tagId * 100 + (int)collectionDto.CollecStar;
					Collection_collectionStar elementById = this.m_tableManager.GetCollection_collectionStarModelInstance().GetElementById(num);
					list.AddRange(elementById.basicAttribute.GetMergeAttributeData());
					CollectionConditionChecker collectionConditionChecker = new CollectionConditionChecker(configId, num, this.m_tableManager);
					List<MergeAttributeData> passiveAttributeList = collectionConditionChecker.GetPassiveAttributeList(num, 1U, this.GetConditionDataCount((uint)collectionConditionChecker.conditionId));
					if (passiveAttributeList != null && passiveAttributeList.Count > 0)
					{
						list.AddRange(passiveAttributeList);
					}
				}
			}
			IList<Collection_collectionSuit> allElements = this.m_tableManager.GetCollection_collectionSuitModelInstance().GetAllElements();
			for (int j = 0; j < allElements.Count; j++)
			{
				Collection_collectionSuit collection_collectionSuit = allElements[j];
				int conditionType = collection_collectionSuit.conditionType;
				int conditionParam = collection_collectionSuit.conditionParam;
				bool flag = true;
				for (int k = 0; k < collection_collectionSuit.collectionId.Length; k++)
				{
					int num2 = collection_collectionSuit.collectionId[k];
					CollectionDto collectionDto2 = this.GetCollectionDto(num2);
					if (collectionDto2 == null || collectionDto2.CollecType != 1U)
					{
						flag = false;
						break;
					}
					if (conditionType == 1)
					{
						if ((ulong)collectionDto2.CollecStar < (ulong)((long)conditionParam))
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				if (flag)
				{
					list.AddRange(collection_collectionSuit.attributes.GetMergeAttributeData());
				}
			}
			addAttributeData.m_attributeDatas = list.Merge();
			return addAttributeData;
		}

		private CollectionDto GetCollectionDto(int collectionId)
		{
			if (this.dtos == null)
			{
				return null;
			}
			for (int i = 0; i < this.dtos.Count; i++)
			{
				CollectionDto collectionDto = this.dtos[i];
				if (collectionDto != null && (ulong)collectionDto.ConfigId == (ulong)((long)collectionId))
				{
					return collectionDto;
				}
			}
			return null;
		}

		private RepeatedField<CollectionDto> dtos;

		private UserStatisticInfo userStatisticInfo;
	}
}
