using System;

namespace HotFix
{
	public class UIDailyActivitiesWaitActiveNode : UIDailyActivitiesNodeBase
	{
		public override FunctionID FunctionOpenID
		{
			get
			{
				return FunctionID.None;
			}
		}

		public override UIDailyActivitiesType DailyType
		{
			get
			{
				return UIDailyActivitiesType.Wait;
			}
		}

		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		protected override void OnShow()
		{
		}

		protected override void OnHide()
		{
		}
	}
}
