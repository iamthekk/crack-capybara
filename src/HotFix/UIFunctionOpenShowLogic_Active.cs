using System;

namespace HotFix
{
	public class UIFunctionOpenShowLogic_Active : UIFunctionOpenShowLogicBase
	{
		public override void RefreshState(UIFunctionOpenShowState state)
		{
			if (this.FunctionUI == null)
			{
				return;
			}
			this.FunctionUI.SetActive(state == UIFunctionOpenShowState.UnLock);
		}
	}
}
