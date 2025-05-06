using System;
using System.Collections.Generic;
using Framework.Logic.AttributeExpansion;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class ColorRender : MonoBehaviour
	{
		public void OnInit(List<Renderer> renderers)
		{
			this.m_renderers.Clear();
			if (renderers != null && renderers.Count > 0)
			{
				this.m_renderers.AddRange(renderers);
			}
			this.m_curTime = 0f;
			this.isPlayAlgha = false;
			this.Stop();
			this.InitBlock();
			this.SetFillAlgha(1f);
			this.SetFillPhase(0f);
		}

		public void OnDeInit()
		{
			this.m_renderers.Clear();
		}

		public void AddRenderers(List<Renderer> renderers)
		{
			if (renderers != null && renderers.Count > 0)
			{
				for (int i = 0; i < renderers.Count; i++)
				{
					Renderer renderer = renderers[i];
					if (!this.m_renderers.Contains(renderer))
					{
						this.m_renderers.Add(renderer);
					}
				}
			}
		}

		private void InitBlock()
		{
			if (this.m_block == null)
			{
				this.m_block = new MaterialPropertyBlock();
			}
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.Update_FillPhase(deltaTime);
			this.Update_Algha(deltaTime);
			this.Update_V(deltaTime);
		}

		private void Update_FillPhase(float deltaTime)
		{
			if (this.m_isPlaying)
			{
				this.m_curProgress = this.m_curTime / this.m_durtion;
				float num = this.m_curve.Evaluate(this.m_curProgress) * this.m_maxIntensity;
				if (this.m_renderers != null)
				{
					this.SetFillPhase(num);
				}
				this.m_curTime += deltaTime;
				if (this.m_curTime >= this.m_durtion)
				{
					this.m_curTime = this.m_durtion;
					this.SetFillPhase(0f);
					this.Stop();
				}
			}
		}

		private void Update_Algha(float deltaTime)
		{
			if (!this.isPlayAlgha)
			{
				return;
			}
			this.curAlphaProgress = this.curAlphaTime / this.alghaDurtion;
			float num = this.alghaCurve.Evaluate(this.curAlphaProgress) * this.m_maxIntensity;
			if (this.m_renderers != null)
			{
				this.SetFillAlgha(num);
			}
			this.curAlphaTime += deltaTime;
			if (this.curAlphaTime >= this.alghaDurtion)
			{
				Action playAlghaComplete = this.PlayAlghaComplete;
				if (playAlghaComplete != null)
				{
					playAlghaComplete();
				}
				this.isPlayAlgha = false;
				this.curAlphaTime = this.alghaDurtion;
			}
		}

		private void Update_V(float deltaTime)
		{
			if (!this.isPlayV)
			{
				return;
			}
			float num = this.curVTime / this.vDuration;
			float num2 = Mathf.Lerp(this.vFrom, this.vTo, num);
			if (this.m_renderers != null)
			{
				this.SetFillVColor(num2);
			}
			this.curVTime += deltaTime;
			if (this.curVTime >= this.vDuration)
			{
				this.SetFillVColor(this.vTo);
				Action playVComplete = this.PlayVComplete;
				if (playVComplete != null)
				{
					playVComplete();
				}
				this.isPlayV = false;
				this.curVTime = this.vDuration;
			}
		}

		public void Play(float progress = 0f)
		{
			this.m_curTime = progress * this.m_durtion;
			this.m_isPlaying = true;
			this.SetFillPhase(0f);
		}

		public void Stop()
		{
			this.m_isPlaying = false;
		}

		public void SetFillPhase(float value)
		{
			if (this.m_renderers == null)
			{
				return;
			}
			this.InitBlock();
			this.m_block.SetFloat("_FillPhase", value);
			for (int i = 0; i < this.m_renderers.Count; i++)
			{
				this.m_renderers[i].SetPropertyBlock(this.m_block);
			}
		}

		public void SetFillColor(Color c)
		{
			if (this.m_renderers == null)
			{
				return;
			}
			this.InitBlock();
			this.m_block.SetColor("_FillColor", c);
			for (int i = 0; i < this.m_renderers.Count; i++)
			{
				this.m_renderers[i].SetPropertyBlock(this.m_block);
			}
		}

		public void PlayAlgha(Action complete = null)
		{
			this.PlayAlghaComplete = complete;
			this.curAlphaTime = 0f;
			this.alghaCurve = this.alghaHideCurve;
			this.isPlayAlgha = true;
			this.SetFillAlgha(1f);
		}

		public void PlayAlphaShow(Action complete = null)
		{
			this.PlayAlghaComplete = complete;
			this.curAlphaTime = 0f;
			this.alghaCurve = this.alghaShowCurve;
			this.isPlayAlgha = true;
			this.SetFillAlgha(0f);
		}

		public void SetFillAlgha(float value)
		{
			if (this.m_renderers == null)
			{
				return;
			}
			this.InitBlock();
			this.m_block.SetFloat("_Alpha", value);
			for (int i = 0; i < this.m_renderers.Count; i++)
			{
				if (this.m_renderers[i] != null)
				{
					this.m_renderers[i].SetPropertyBlock(this.m_block);
				}
			}
		}

		public void PlayVColor(float to, float duration, Action complete = null)
		{
			this.vDuration = duration;
			this.PlayVComplete = complete;
			this.InitBlock();
			this.vFrom = this.m_block.GetFloat("_V");
			this.vTo = to;
			this.curVTime = 0f;
			this.isPlayV = true;
		}

		public void SetFillVColor(float value)
		{
			if (this.m_renderers == null)
			{
				return;
			}
			this.InitBlock();
			this.m_block.SetFloat("_V", value);
			for (int i = 0; i < this.m_renderers.Count; i++)
			{
				if (this.m_renderers[i] != null)
				{
					this.m_renderers[i].SetPropertyBlock(this.m_block);
				}
			}
		}

		public const string m_fillColorName = "_FillColor";

		public const string m_fillPhaseName = "_FillPhase";

		public const string m_fillAlphaName = "_Alpha";

		public const string m_fillVname = "_V";

		[Header("白光受击效果")]
		public float m_durtion = 0.3f;

		public AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public float m_maxIntensity = 1f;

		[Space(10f)]
		[Label]
		public bool m_isPlaying;

		[Label]
		public float m_curTime;

		[Label]
		public float m_curProgress;

		[Header("透明度效果")]
		[Space(10f)]
		public float alghaDurtion = 0.5f;

		public AnimationCurve alghaShowCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public AnimationCurve alghaHideCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		public AnimationCurve alghaCurve;

		private bool isPlayAlgha;

		private bool isPlayV;

		private float curAlphaProgress;

		private float curAlphaTime;

		private float curVTime;

		private float vDuration;

		private float vFrom;

		private float vTo;

		private List<Renderer> m_renderers = new List<Renderer>();

		private MaterialPropertyBlock m_block;

		private Action PlayAlghaComplete;

		private Action PlayVComplete;
	}
}
