using System;

namespace HotFix
{
	public enum GuideSkipKind
	{
		None,
		GuideComplete,
		TargetOutOfScreen,
		TargetLost,
		UserSkip = 90,
		AdditionalUIDestroy = 99
	}
}
