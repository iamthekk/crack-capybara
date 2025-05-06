using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class IAPShopTimeCtrl : CustomBehaviour
	{
		public event Func<IAPShopTimeCtrl, string> OnRefreshText;

		public event Func<IAPShopTimeCtrl, IAPShopTimeCtrl.State> OnChangeState;

		public bool IsPlaying { get; set; }

		protected override void OnInit()
		{
			this.time = 0f;
			if (this.timeTxt != null)
			{
				this.timeTxt.text = string.Empty;
			}
			this.Stop();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.IsPlaying)
			{
				return;
			}
			if (this.state != IAPShopTimeCtrl.State.Show)
			{
				return;
			}
			this.time += deltaTime;
			if (this.time >= this.duration)
			{
				this.OnRefresh();
				this.time = 0f;
			}
		}

		protected override void OnDeInit()
		{
			this.OnRefreshText = null;
			this.OnChangeState = null;
		}

		public void Play()
		{
			this.IsPlaying = true;
			this.OnRefresh();
		}

		public void Stop()
		{
			this.IsPlaying = false;
			this.time = 0f;
		}

		public void SetState(IAPShopTimeCtrl.State stateVal)
		{
			this.state = stateVal;
		}

		public void OnRefresh()
		{
			if (this.OnRefreshText != null && this.timeTxt != null)
			{
				this.timeTxt.text = this.OnRefreshText(this);
			}
			if (this.OnChangeState != null)
			{
				this.state = this.OnChangeState(this);
			}
		}

		[SerializeField]
		private CustomText timeTxt;

		[SerializeField]
		private float duration = 1f;

		[SerializeField]
		private IAPShopTimeCtrl.State state;

		private float time;

		public enum State
		{
			Show,
			Load
		}
	}
}
