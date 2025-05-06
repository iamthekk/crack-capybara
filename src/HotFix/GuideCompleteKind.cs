using System;

namespace HotFix
{
	public enum GuideCompleteKind
	{
		ButtonClick = 1,
		ButtonDown,
		ButtonUp,
		ViewClose = 11,
		ViewOpen,
		TargetPositionChange = 21,
		MainDownButton = 51,
		SpecialString = 999
	}
}
