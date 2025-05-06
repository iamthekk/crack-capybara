using System;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class SPetData
	{
		public SPetData Init(PetDto petDto, LocalModelManager manager)
		{
			int petConfigId = petDto.GetPetConfigId(manager);
			Pet_pet elementById = manager.GetPet_petModelInstance().GetElementById(petConfigId);
			this.petQuality = (EPetQuality)elementById.quality;
			return this;
		}

		public EPetQuality petQuality;
	}
}
