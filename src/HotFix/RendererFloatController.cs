using System;
using Framework.Logic.Easing;
using UnityEngine;

namespace HotFix
{
	public class RendererFloatController
	{
		public bool IsPlaying { get; private set; }

		public bool IsDelay { get; private set; }

		public bool IsFinished { get; private set; }

		public bool IsPause { get; private set; }

		public void SetData(RendererFloatController.Data data)
		{
			this.m_data = data;
		}

		public void Play()
		{
			if (this.m_data == null)
			{
				return;
			}
			this.IsPlaying = true;
			this.IsDelay = true;
			this.IsFinished = false;
			this.m_time = 0f;
		}

		public void Stop()
		{
			this.IsPlaying = false;
		}

		public void Pause(bool pause)
		{
			this.IsPause = pause;
		}

		public void OnInit()
		{
			this.IsPlaying = false;
			this.IsFinished = false;
			this.IsDelay = false;
			this.IsPause = false;
			this.m_time = 0f;
		}

		public void OnDeInit()
		{
			this.ResetAlpha();
			this.IsPlaying = false;
			this.IsFinished = false;
			this.IsPause = false;
			this.IsDelay = false;
			this.m_time = 0f;
			this.m_data = null;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.IsPause)
			{
				return;
			}
			if (!this.IsPlaying)
			{
				return;
			}
			if (this.IsFinished)
			{
				return;
			}
			if (this.m_data == null)
			{
				return;
			}
			if (this.m_data.m_renderers == null)
			{
				return;
			}
			if (this.IsDelay)
			{
				this.m_time += deltaTime;
				if (this.m_time >= this.m_data.m_delay)
				{
					this.m_time = this.m_data.m_delay;
					this.IsDelay = false;
					this.m_time = 0f;
				}
				return;
			}
			this.m_time += deltaTime;
			if (this.m_time >= this.m_data.m_duration)
			{
				this.m_time = this.m_data.m_duration;
				this.IsFinished = true;
			}
			float num = Easing.EasingFloat(this.m_time, this.m_data.m_from, this.m_data.m_to, this.m_data.m_duration, this.m_data.m_easeType);
			for (int i = 0; i < this.m_data.m_renderers.Length; i++)
			{
				Renderer renderer = this.m_data.m_renderers[i];
				if (!(renderer == null) && !(renderer.sharedMaterial == null) && renderer.sharedMaterial.HasProperty(this.m_data.m_fieldName))
				{
					Color materialColor = MaterialManager.GetMaterialColor(renderer, this.m_data.m_fieldName);
					materialColor.a = num;
					MaterialManager.SetMaterialColor(renderer, this.m_data.m_fieldName, materialColor);
				}
			}
			if (this.IsFinished && this.m_data.m_onFinished != null)
			{
				this.m_data.m_onFinished();
			}
		}

		public void ResetAlpha()
		{
			if (this.m_data == null || this.m_data.m_renderers == null)
			{
				return;
			}
			for (int i = 0; i < this.m_data.m_renderers.Length; i++)
			{
				Renderer renderer = this.m_data.m_renderers[i];
				if (!(renderer == null) && !(renderer.sharedMaterial == null) && renderer.sharedMaterial.HasProperty(this.m_data.m_fieldName))
				{
					Color materialColor = MaterialManager.GetMaterialColor(renderer, this.m_data.m_fieldName);
					materialColor.a = 1f;
					MaterialManager.SetMaterialColor(renderer, this.m_data.m_fieldName, materialColor);
				}
			}
		}

		public RendererFloatController.Data m_data;

		public float m_time;

		public class Data
		{
			public Renderer[] m_renderers;

			public string m_fieldName = "_Alpha";

			public EaseType m_easeType;

			public float m_delay;

			public float m_duration = 2f;

			public float m_from = 1f;

			public float m_to;

			public Action m_onFinished;
		}
	}
}
