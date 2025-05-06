using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public class AddAttributeBase : BaseAddAttribute
	{
		public AddAttributeBase(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(int memberID)
		{
			this.m_memberID = memberID;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			GameMember_member elementById = this.m_tableManager.GetGameMember_memberModelInstance().GetElementById(this.m_memberID);
			if (elementById == null)
			{
				return addAttributeData;
			}
			List<MergeAttributeData> mergeAttributeData = elementById.baseAttributes.GetMergeAttributeData();
			addAttributeData.m_attributeDatas = mergeAttributeData.Merge();
			addAttributeData.m_skillIDs.AddRange(elementById.skillIDs.GetListInt('|'));
			return addAttributeData;
		}

		public int m_memberID;
	}
}
