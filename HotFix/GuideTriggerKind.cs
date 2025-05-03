using System;

namespace HotFix
{
	public enum GuideTriggerKind
	{
		NULL,
		ViewOpen,
		ViewClose,
		ChangeMainViewTab,
		GuideOver,
		GuideGroupOver,
		MainDownButton = 51,
		OnlyMainUI,
		CustomizeString = 99
	}
}
