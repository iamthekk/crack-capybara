using System;
using UnityEngine;

namespace HotFix
{
	internal class UIFunctionOpenShowLogic_ExtureLockObject : UIFunctionOpenShowLogicBase
	{
		public override void RefreshState(UIFunctionOpenShowState state)
		{
			if (this.FunctionUI == null)
			{
				return;
			}
			GameObject lockObject = this.FunctionUI.LockObject;
			if (lockObject == null)
			{
				return;
			}
			lockObject.SetActive(state == UIFunctionOpenShowState.Lock || state == UIFunctionOpenShowState.PrepareUnLock);
		}
	}
}
