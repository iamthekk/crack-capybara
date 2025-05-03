using System;
using Framework.Logic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class MapWaveItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.material = Object.Instantiate<Material>(this.copyMat);
			this.sprite.material = this.material;
			this.spriteColor = this.sprite.color;
			this.spriteAni.SetPause(true);
			this.root.transform.localScale = Vector3.zero;
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

		protected override void OnDeInit()
		{
		}

		public void ShowWave(int delay)
		{
			this.delayTime = delay;
			DelayCall.Instance.CallOnce(delay, delegate
			{
				if (this.spriteAni != null)
				{
					this.RandomPosition();
					this.spriteAni.SetFinishAction(new Action(this.OnAnimationFinish));
					this.spriteAni.Reset();
					this.spriteAni.SetPause(false);
					float num = Utility.Math.Random(0.6f, 1f);
					this.root.transform.localScale = Vector3.one * num;
					Color color = this.spriteColor;
					color.a = Utility.Math.Random(0.6f, 0.9f);
					this.spriteColor = color;
				}
			});
		}

		private void OnAnimationFinish()
		{
			this.spriteAni.SetPause(true);
			this.root.transform.localScale = Vector3.zero;
			if (this.waitDestroy)
			{
				this.DestroySelf();
				return;
			}
			this.ShowWave(this.delayTime);
		}

		private void RandomPosition()
		{
			base.gameObject.transform.localPosition = new Vector3(Utility.Math.Random(-10.8f, 10.8f), Utility.Math.Random(-4.6f, 4.6f), 0f);
		}

		public void SetWaitDestroy()
		{
			this.waitDestroy = true;
		}

		private void DestroySelf()
		{
			Object.Destroy(base.gameObject);
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

		public Transform root;

		public SpriteAnimation spriteAni;

		public SpriteRenderer sprite;

		private int delayTime;

		private Color spriteColor;

		private bool waitDestroy;

		private Material material;

		private bool isAni;

		private float curTime;

		private float mDuration;

		private float mFrom;

		private float mTo;

		private const string V_NAME = "_V";
	}
}
