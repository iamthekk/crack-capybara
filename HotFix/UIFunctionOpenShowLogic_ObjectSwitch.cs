using System;
using UnityEngine;

namespace HotFix
{
	public class UIFunctionOpenShowLogic_ObjectSwitch : UIFunctionOpenShowLogicBase
	{
		public override void RefreshState(UIFunctionOpenShowState state)
		{
			if (this.FunctionUI == null)
			{
				return;
			}
			GameObject lockObject = this.FunctionUI.LockObject;
			if (lockObject != null)
			{
				lockObject.SetActive(state == UIFunctionOpenShowState.Lock || state == UIFunctionOpenShowState.PrepareUnLock);
			}
			GameObject unLockObject = this.FunctionUI.UnLockObject;
			if (unLockObject == null)
			{
				return;
			}
			unLockObject.SetActive(state == UIFunctionOpenShowState.UnLock);
		}
	}
}
