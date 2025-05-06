using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public abstract class BaseUIMainButton : CustomBehaviour
	{
		public abstract bool IsShow();

		public abstract void GetPriority(out int priority, out int subPriority);

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.lastRefreshTime += unscaledDeltaTime;
			if (this.lastRefreshTime >= 1f)
			{
				this.lastRefreshTime -= (float)((int)this.lastRefreshTime);
				this.OnUpdatePerSecond();
			}
		}

		protected virtual void OnUpdatePerSecond()
		{
		}

		public abstract void OnRefresh();

		public abstract void OnLanguageChange();

		public abstract void OnRefreshAnimation();

		public void SetShow(bool isShow)
		{
			base.gameObject.SetActiveSafe(isShow);
		}

		public GameObject root;

		private float lastRefreshTime;
	}
}
