using System;
using UnityEngine;

namespace HotFix.Client
{
	public class CameraShake
	{
		public void SetData(Camera camera)
		{
			this.m_camera = camera;
		}

		public void OnInit()
		{
		}

		public void OnDeInit()
		{
			this.m_camera = null;
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
				case 0:
					this.m_camera.transform.localPosition = new Vector3(this.orgPos.x + (float)this.x * num, this.orgPos.y + (float)this.y * num, this.orgPos.z + (float)this.z * num);
					break;
				case 1:
					this.m_camera.transform.localPosition = new Vector3(num, this.orgPos.y, 0f);
					break;
				case 2:
					this.m_camera.transform.localPosition = new Vector3(this.orgPos.x, this.orgPos.y + num, 0f);
					break;
				case 3:
					this.m_camera.orthographicSize = this.initOrthographicSize + num;
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
			this.m_camera.transform.localPosition = this.orgPos;
			this.m_camera.orthographicSize = this.initOrthographicSize;
			this.elapsedTime = 0f;
		}

		public void Shake()
		{
			if (this.m_camera == null)
			{
				return;
			}
			this.Shake(2, 0f, 0.3f, 0.2f, 1);
		}

		public void Shake(int shakeType, float delay, float duration, float power, int count)
		{
			if (this.m_camera == null)
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
			this.orgPos = this.m_camera.transform.localPosition;
			this.initOrthographicSize = this.m_camera.orthographicSize;
			this.elapsedTime = 0f;
			this.isPlaying = true;
		}

		public void OnStopByKillShot()
		{
			if (this.shakeType == 3)
			{
				this.Stop();
			}
		}

		public Camera m_camera;

		private int shakeType;

		private float delay;

		private float duration;

		private float power;

		private bool isPlaying;

		private bool isShake;

		private Vector3 orgPos;

		private float initOrthographicSize;

		private float elapsedTime;

		private float intervalTime;

		private int x;

		private int y;

		private int z;
	}
}
