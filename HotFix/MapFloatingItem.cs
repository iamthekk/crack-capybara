using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class MapFloatingItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.material = Object.Instantiate<Material>(this.copyMat);
			this.sprite.material = this.material;
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.isAni)
			{
				return;
			}
			float num = this.curTime / this.mDuration;
			float num2 = Mathf.Lerp(this.mFrom, this.mTo, num);
			if (this.material != null)
			{
				this.material.SetFloat("_V", num2);
			}
			this.curTime += deltaTime;
			if (this.curTime >= this.mDuration)
			{
				if (this.material != null)
				{
					this.material.SetFloat("_V", this.mTo);
				}
				this.isAni = false;
				this.curTime = this.mDuration;
			}
		}

		public void SetTime(float to, float duration)
		{
			if (this.isAni)
			{
				return;
			}
			this.mTo = to;
			this.mDuration = duration;
			if (this.material)
			{
				this.mFrom = this.material.GetFloat("_V");
			}
			this.curTime = 0f;
			this.isAni = true;
		}

		public Material copyMat;

		public SpriteRenderer sprite;

		private Material material;

		private bool isAni;

		private float curTime;

		private float mDuration;

		private float mFrom;

		private float mTo;

		private const string V_NAME = "_V";
	}
}
