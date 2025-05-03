using System;
using System.Linq;
using LocalModels;
using Proto.Common;

namespace Server
{
	public class AddAttributeController
	{
		public LocalModelManager m_tableManager { get; }

		public AddAttributeController(LocalModelManager tableManager)
		{
			this.m_tableManager = tableManager;
		}

		public AddAttributeData MathMainMember(int memberID, BattleUserDto battleUserDto)
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			AddAttributeBase addAttributeBase = new AddAttributeBase(this.m_tableManager);
			addAttributeBase.SetData(memberID);
			AddAttributeData addAttributeData2 = addAttributeBase.MathAll();
			addAttributeData.Merge(addAttributeData2);
			AddAttributePet addAttributePet = new AddAttributePet(this.m_tableManager);
			addAttributePet.SetData(battleUserDto.Pets.ToList<PetDto>(), battleUserDto.Fetters.ToList<uint>());
			AddAttributeData addAttributeData3 = addAttributePet.MathAll();
			addAttributeData.Merge(addAttributeData3);
			AddAttributeEquip addAttributeEquip = new AddAttributeEquip(this.m_tableManager);
			addAttributeEquip.SetData(battleUserDto.Equips);
			AddAttributeData addAttributeData4 = addAttributeEquip.MathAll();
			addAttributeData.Merge(addAttributeData4);
			AddAttributeMount addAttributeMount = new AddAttributeMount(this.m_tableManager);
			addAttributeMount.SetData(battleUserDto.MountInfo, battleUserDto.MountItemDtos);
			AddAttributeData addAttributeData5 = addAttributeMount.MathAll();
			addAttributeData.Merge(addAttributeData5);
			AddAttributeArtifact addAttributeArtifact = new AddAttributeArtifact(this.m_tableManager);
			addAttributeArtifact.SetData(battleUserDto.ArtifactInfo, battleUserDto.ArtifactItemDtos);
			AddAttributeData addAttributeData6 = addAttributeArtifact.MathAll();
			addAttributeData.Merge(addAttributeData6);
			AddAttributeTalent addAttributeTalent = new AddAttributeTalent(this.m_tableManager);
			addAttributeTalent.SetData(battleUserDto.TalentsInfo);
			AddAttributeData addAttributeData7 = addAttributeTalent.MathAll();
			addAttributeData.Merge(addAttributeData7);
			AddAttributeTalentLegacy addAttributeTalentLegacy = new AddAttributeTalentLegacy(this.m_tableManager);
			addAttributeTalentLegacy.SetData(battleUserDto.TalentLegacyInfo);
			AddAttributeData addAttributeData8 = addAttributeTalentLegacy.MathAll();
			addAttributeData.Merge(addAttributeData8);
			if (battleUserDto.CollectionInfo != null && battleUserDto.CollectionInfo.CollectionList != null)
			{
				AddAttributeCollection addAttributeCollection = new AddAttributeCollection(this.m_tableManager);
				addAttributeCollection.SetData(battleUserDto.CollectionInfo.CollectionList, battleUserDto.UserStatisticInfo);
				AddAttributeData addAttributeData9 = addAttributeCollection.MathAll();
				addAttributeData.Merge(addAttributeData9);
			}
			addAttributeData.m_attributeDatas.Merge();
			return addAttributeData;
		}
	}
}
