using System;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class FishingFocusCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			ShortcutExtensions.DOKill(this.indicator, false);
		}

		public void RandomRotate()
		{
			float randomAngleRange = this.GetRandomAngleRange();
			this.randomAngle = Mathf.RoundToInt(Random.Range(-randomAngleRange, randomAngleRange));
			this.angleRange.transform.rotation = Quaternion.Euler(new Vector3(0f, this._indicatorYRotation, (float)this.randomAngle));
			this.focusTrans.gameObject.SetActive(true);
			TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(this.indicator, new Vector3(0f, this._indicatorYRotation, this.swingAngle), this.swingDuration / 2f, 0), 1), -1, 1);
		}

		private float GetRandomAngleRange()
		{
			return this.swingAngle - this.normalTargetAngle;
		}

		public FishingEval GetEval()
		{
			ShortcutExtensions.DOKill(this.indicator, false);
			FishingEval fishingEval = FishingEval.Normal;
			float num = this.indicator.localEulerAngles.z;
			if (num > 180f)
			{
				num -= 360f;
			}
			if ((float)this.randomAngle - this.highTargetAngle <= num && num <= (float)this.randomAngle + this.highTargetAngle)
			{
				fishingEval = FishingEval.Perfect;
			}
			else if ((float)this.randomAngle - this.normalTargetAngle <= num && num <= (float)this.randomAngle + this.normalTargetAngle)
			{
				fishingEval = FishingEval.Nice;
			}
			return fishingEval;
		}

		public void ResetIndicator()
		{
			ShortcutExtensions.DOKill(this.indicator, false);
			this.indicator.rotation = Quaternion.Euler(new Vector3(0f, this._indicatorYRotation, -this.swingAngle));
		}

		public Transform focusTrans;

		public Transform indicator;

		public Transform angleRange;

		[Header("鱼竿度数配置")]
		public float swingDuration = 1f;

		public float swingAngle = 30f;

		public float normalTargetAngle = 15f;

		public float highTargetAngle = 5f;

		private readonly float _indicatorYRotation = 180f;

		private int randomAngle;
	}
}
