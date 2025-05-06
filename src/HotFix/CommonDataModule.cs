using System;
using System.Collections.Generic;
using Framework.DataModule;
using Framework.EventSystem;

namespace HotFix
{
	public class CommonDataModule : IDataModule
	{
		public int GetName()
		{
			return 2;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public void SetRememberTipState(RememberTipType rememberTipType, bool value)
		{
			CommonTipData commonTipData;
			commonTipData.isSkip = value;
			commonTipData.lastState = true;
			this.rememberTipStateDic[rememberTipType] = commonTipData;
		}

		public void SetRememberTipState(RememberTipType rememberTipType, bool value, bool lastState)
		{
			CommonTipData commonTipData;
			commonTipData.isSkip = value;
			commonTipData.lastState = lastState;
			this.rememberTipStateDic[rememberTipType] = commonTipData;
		}

		public bool GetRememberTipState(RememberTipType rememberTipType)
		{
			return this.rememberTipStateDic.ContainsKey(rememberTipType) && this.rememberTipStateDic[rememberTipType].isSkip;
		}

		public CommonTipData GetRememberTipData(RememberTipType rememberTipType)
		{
			if (this.rememberTipStateDic.ContainsKey(rememberTipType))
			{
				return this.rememberTipStateDic[rememberTipType];
			}
			CommonTipData commonTipData;
			commonTipData.isSkip = false;
			commonTipData.lastState = true;
			return commonTipData;
		}

		public Dictionary<RememberTipType, CommonTipData> rememberTipStateDic = new Dictionary<RememberTipType, CommonTipData>();
	}
}
