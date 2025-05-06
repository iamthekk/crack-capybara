using System;

namespace HotFix
{
	public enum FishingRodState : uint
	{
		Idle,
		Focus,
		Throwing,
		Wait,
		Bite,
		Relax,
		Reel,
		Success,
		Failed,
		End
	}
}
