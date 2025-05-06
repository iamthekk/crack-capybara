using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Tools
{
	public class EffectColor : MonoBehaviour
	{
		private void Start()
		{
			this.Init();
		}

		private void Update()
		{
			if (!this.isPlaying)
			{
				return;
			}
			if (this.isOut)
			{
				this.m_curProgress = this.m_curTime / this.m_durtion;
				float num = this.curCurve.Evaluate(this.m_curProgress);
				this.SetMaterialAlpha(num);
				this.m_curTime -= Time.deltaTime;
				if (this.m_curTime <= 0f)
				{
					this.m_curTime = 0f;
					this.isPlaying = false;
					this.SetMaterialAlpha(0f);
					return;
				}
			}
			else
			{
				this.m_curProgress = this.m_curTime / this.m_durtion;
				float num2 = this.curCurve.Evaluate(this.m_curProgress);
				this.SetMaterialAlpha(num2);
				this.m_curTime += Time.deltaTime;
				if (this.m_curTime >= this.m_durtion)
				{
					this.m_curTime = this.m_durtion;
					this.isPlaying = false;
					this.SetMaterialAlpha(1f);
				}
			}
		}

		private void Init()
		{
			this.reders = base.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < this.reders.Length; i++)
			{
				this.mates.Add(this.reders[i].material);
			}
			this.SetMaterialAlpha(0f);
		}

		public void OnShowBefore()
		{
			this.SetMaterialAlpha(0f);
			this.m_curTime = 0f;
			this.isOut = false;
			this.isPlaying = true;
		}

		public void OnShowAfter()
		{
			this.SetMaterialAlpha(1f);
			this.m_curTime = this.m_durtion;
			this.isOut = true;
			this.isPlaying = true;
		}

		private void SetMaterialAlpha(float alpha)
		{
			for (int i = 0; i < this.mates.Count; i++)
			{
				this.mates[i].SetColor("_TintColor", new Color(2f, 2f, 2f, alpha));
			}
		}

		private bool isPlaying;

		private bool isOut;

		private float m_durtion = 0.3f;

		private float m_curTime;

		private float m_curProgress;

		private Renderer[] reders;

		private List<Material> mates = new List<Material>();

		private AnimationCurve curCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	}
}
