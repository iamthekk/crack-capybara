using System;
using Framework.Logic.AttributeExpansion;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class RimPower : MonoBehaviour
	{
		public void OnInit(Renderer[] renderers)
		{
			this.m_currentTime = 0f;
			this.m_renderers = renderers;
			this.initBlock();
		}

		private void initBlock()
		{
			if (this.m_block == null)
			{
				this.m_block = new MaterialPropertyBlock();
			}
		}

		public void OnUpdate(float deltaTime)
		{
			if (this.m_isPlaying)
			{
				this.m_currentTime += deltaTime;
				if (this.m_currentTime >= this.m_durtion)
				{
					this.m_currentTime = this.m_durtion;
					this.m_isPlaying = false;
				}
				this.m_currentProgress = this.m_currentTime / this.m_durtion;
				float num = this.m_curve.Evaluate(this.m_currentProgress) * this.m_maxIntensity;
				if (this.m_renderers != null)
				{
					this.SetRimPower(num);
				}
			}
		}

		public void OnDeInit()
		{
		}

		public void Play(float progress = 0f)
		{
			this.m_currentTime = progress * this.m_durtion;
			this.m_isPlaying = true;
			this.SetRimPower(0f);
		}

		public void Stop()
		{
			this.m_isPlaying = false;
		}

		public void SetRimPower(float value)
		{
			this.initBlock();
			if (this.m_renderers == null)
			{
				return;
			}
			this.m_block.SetFloat("_RimPower", value);
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				this.m_renderers[i].SetPropertyBlock(this.m_block);
			}
		}

		public void SetRimScale(float value)
		{
			this.initBlock();
			if (this.m_renderers == null)
			{
				return;
			}
			this.m_block.SetFloat("_RimScale", value);
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				this.m_renderers[i].SetPropertyBlock(this.m_block);
			}
		}

		public void SetRimColor(Color color)
		{
			this.initBlock();
			if (this.m_renderers == null)
			{
				return;
			}
			this.m_block.SetColor("_MainColor", color);
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				this.m_renderers[i].SetPropertyBlock(this.m_block);
			}
		}

		public float m_durtion = 0.2f;

		public AnimationCurve m_curve;

		public float m_maxIntensity = 1f;

		public const string m_rimPowerName = "_RimPower";

		public const string m_rimScaleName = "_RimScale";

		public const string m_rimMainColorName = "_MainColor";

		private Renderer[] m_renderers;

		private MaterialPropertyBlock m_block;

		[Label]
		public bool m_isPlaying;

		[Label]
		public float m_currentTime;

		[Label]
		public float m_currentProgress;
	}
}
