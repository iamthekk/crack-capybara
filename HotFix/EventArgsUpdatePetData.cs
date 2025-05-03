using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventArgsUpdatePetData : BaseEventArgs
	{
		public void SetData(PetDto petDto)
		{
			this.m_pets = new RepeatedField<PetDto>();
			this.m_pets.Add(petDto);
		}

		public void SetData(RepeatedField<PetDto> pets)
		{
			this.m_pets = pets;
		}

		public override void Clear()
		{
		}

		public RepeatedField<PetDto> m_pets;
	}
}
