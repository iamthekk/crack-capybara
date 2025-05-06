using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsGameChangeAttribute : BaseEventArgs
	{
		public BattleChangeAttributeData changeData { get; private set; }

		public void SetData(BattleChangeAttributeData data)
		{
			this.changeData = data;
		}

		public override void Clear()
		{
		}
	}
}
