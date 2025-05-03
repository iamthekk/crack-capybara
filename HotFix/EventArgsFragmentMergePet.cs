using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventArgsFragmentMergePet : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public RepeatedField<PetDto> addPets;
	}
}
