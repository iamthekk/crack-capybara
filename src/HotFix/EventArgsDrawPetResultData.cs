using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsDrawPetResultData : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public PetOpenEggViewModule.OpenData openData;
	}
}
