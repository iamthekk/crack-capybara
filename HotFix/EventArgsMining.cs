using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsMining : BaseEventArgs
	{
		public UIMiningGridItem gridItem { get; private set; }

		public bool isGradeOre { get; private set; }

		public void SetData(UIMiningGridItem item, bool isGrade)
		{
			this.gridItem = item;
			this.isGradeOre = isGrade;
		}

		public override void Clear()
		{
		}
	}
}
