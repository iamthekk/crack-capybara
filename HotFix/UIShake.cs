using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIShake : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.shakeTarget = base.transform;
		}

		protected override void OnDeInit()
		{
			this.shakeTarget = null;
		}

		public void SetData(Transform target)
		{
			this.shakeTarget = target;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isPlaying && !this.isShake)
			{
				this.elapsedTime += unscaledDeltaTime;
				if (this.elapsedTime >= this.delay)
				{
					this.elapsedTime = 0f;
					this.isShake = true;
				}
			}
			if (this.isPlaying && this.isShake)
			{
				float num = Mathf.Sin(this.elapsedTime % this.intervalTime / this.intervalTime * 2f * 3.14159274f) * this.power * (1f - this.elapsedTime / this.duration);
				switch (this.shakeType)
				{
				case UIShake.ShakeType.Random:
					this.shakeTarget.localPosition = new Vector3(this.orgPos.x + (float)this.x * num, this.orgPos.y + (float)this.y * num, this.orgPos.z + (float)this.z * num);
					break;
				case UIShake.ShakeType.Horizontal:
					this.shakeTarget.localPosition = new Vector3(num, this.orgPos.y, 0f);
					break;
				case UIShake.ShakeType.Vertical:
					this.shakeTarget.localPosition = new Vector3(this.orgPos.x, this.orgPos.y + num, 0f);
					break;
				}
				this.elapsedTime += unscaledDeltaTime;
				if (this.elapsedTime >= this.duration)
				{
					this.Stop();
				}
			}
		}

		public void Stop()
		{
			this.isShake = false;
			this.isPlaying = false;
			this.shakeTarget.localPosition = this.orgPos;
			this.elapsedTime = 0f;
		}

		public void Shake()
		{
			if (this.shakeTarget == null)
			{
				return;
			}
			this.Shake(UIShake.ShakeType.Random, 0f, 0.3f, 0.2f, 1);
		}

		public void Shake(UIShake.ShakeType shakeType, float delay, float duration, float power, int count)
		{
			if (this.shakeTarget == null)
			{
				return;
			}
			if (this.isPlaying)
			{
				this.Stop();
			}
			this.shakeType = shakeType;
			this.delay = delay;
			this.duration = duration;
			this.power = power;
			this.intervalTime = duration / (float)count;
			this.x = (((double)Random.Range(0f, 1f) > 0.5) ? 1 : (-1));
			this.y = (((double)Random.Range(0f, 1f) > 0.5) ? 1 : (-1));
			this.z = (((double)Random.Range(0f, 1f) > 0.5) ? 1 : (-1));
			this.orgPos = this.shakeTarget.localPosition;
			this.elapsedTime = 0f;
			this.isPlaying = true;
		}

		private Transform shakeTarget;

		private UIShake.ShakeType shakeType;

		private float delay;

		private float duration;

		private float power;

		private bool isPlaying;

		private bool isShake;

		private Vector3 orgPos;

		private float elapsedTime;

		private float intervalTime;

		private int x;

		private int y;

		private int z;

		public enum ShakeType
		{
			Random,
			Horizontal,
			Vertical
		}
	}
}
