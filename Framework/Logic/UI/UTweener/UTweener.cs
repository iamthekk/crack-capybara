using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	public abstract class UTweener : MonoBehaviour
	{
		public float amountPerDelta
		{
			get
			{
				if (this.mDuration != this.duration)
				{
					this.mDuration = this.duration;
					this.mAmountPerDelta = Mathf.Abs((this.duration > 0f) ? (1f / this.duration) : 1000f) * Mathf.Sign(this.mAmountPerDelta);
				}
				return this.mAmountPerDelta;
			}
		}

		public float tweenFactor
		{
			get
			{
				return this.mFactor;
			}
			set
			{
				this.mFactor = Mathf.Clamp01(value);
			}
		}

		public UTweener.Direction direction
		{
			get
			{
				if (this.amountPerDelta >= 0f)
				{
					return UTweener.Direction.Forward;
				}
				return UTweener.Direction.Reverse;
			}
		}

		private void Reset()
		{
			if (!this.mStarted)
			{
				this.SetStartToCurrentValue();
				this.SetEndToCurrentValue();
			}
		}

		protected virtual void Start()
		{
			this.Update();
		}

		private void Update()
		{
			float num = (this.ignoreTimeScale ? URealTime.deltaTime : Time.deltaTime);
			float num2 = (this.ignoreTimeScale ? URealTime.time : Time.time);
			if (!this.mStarted)
			{
				this.mStarted = true;
				this.mStartTime = num2 + this.delay;
			}
			if (num2 < this.mStartTime)
			{
				return;
			}
			this.mFactor += this.amountPerDelta * num;
			if (this.style == UTweener.Style.Loop)
			{
				if (this.mFactor > 1f)
				{
					this.mFactor -= Mathf.Floor(this.mFactor);
				}
			}
			else if (this.style == UTweener.Style.PingPong)
			{
				if (this.mFactor > 1f)
				{
					this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
					this.mAmountPerDelta = -this.mAmountPerDelta;
				}
				else if (this.mFactor < 0f)
				{
					this.mFactor = -this.mFactor;
					this.mFactor -= Mathf.Floor(this.mFactor);
					this.mAmountPerDelta = -this.mAmountPerDelta;
				}
			}
			if (this.style == UTweener.Style.Once && (this.duration == 0f || this.mFactor > 1f || this.mFactor < 0f))
			{
				this.mFactor = Mathf.Clamp01(this.mFactor);
				this.Sample(this.mFactor, true);
				if (this.duration == 0f || (this.mFactor == 1f && this.mAmountPerDelta > 0f) || (this.mFactor == 0f && this.mAmountPerDelta < 0f))
				{
					base.enabled = false;
				}
				if (UTweener.current == null)
				{
					UTweener.current = this;
					if (this.onFinished != null)
					{
						this.mTemp = this.onFinished;
						this.onFinished = new List<UEventDelegate>();
						UEventDelegate.Execute(this.mTemp);
						for (int i = 0; i < this.mTemp.Count; i++)
						{
							UEventDelegate ueventDelegate = this.mTemp[i];
							if (ueventDelegate != null && !ueventDelegate.oneShot)
							{
								UEventDelegate.Add(this.onFinished, ueventDelegate, ueventDelegate.oneShot);
							}
						}
						this.mTemp = null;
					}
					if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
					{
						this.eventReceiver.SendMessage(this.callWhenFinished, this, 1);
					}
					UTweener.current = null;
					return;
				}
			}
			else
			{
				this.Sample(this.mFactor, false);
			}
		}

		public void SetOnFinished(UEventDelegate.Callback del)
		{
			UEventDelegate.Set(this.onFinished, del);
		}

		public void SetOnFinished(UEventDelegate del)
		{
			UEventDelegate.Set(this.onFinished, del);
		}

		public void AddOnFinished(UEventDelegate.Callback del)
		{
			UEventDelegate.Add(this.onFinished, del);
		}

		public void AddOnFinished(UEventDelegate del)
		{
			UEventDelegate.Add(this.onFinished, del);
		}

		public void RemoveOnFinished(UEventDelegate del)
		{
			if (this.onFinished != null)
			{
				this.onFinished.Remove(del);
			}
			if (this.mTemp != null)
			{
				this.mTemp.Remove(del);
			}
		}

		private void OnDisable()
		{
			this.mStarted = false;
		}

		public void Sample(float factor, bool isFinished)
		{
			float num = Mathf.Clamp01(factor);
			if (this.method == UTweener.Method.EaseIn)
			{
				num = 1f - Mathf.Sin(1.57079637f * (1f - num));
				if (this.steeperCurves)
				{
					num *= num;
				}
			}
			else if (this.method == UTweener.Method.EaseOut)
			{
				num = Mathf.Sin(1.57079637f * num);
				if (this.steeperCurves)
				{
					num = 1f - num;
					num = 1f - num * num;
				}
			}
			else if (this.method == UTweener.Method.EaseInOut)
			{
				num -= Mathf.Sin(num * 6.28318548f) / 6.28318548f;
				if (this.steeperCurves)
				{
					num = num * 2f - 1f;
					float num2 = Mathf.Sign(num);
					num = 1f - Mathf.Abs(num);
					num = 1f - num * num;
					num = num2 * num * 0.5f + 0.5f;
				}
			}
			else if (this.method == UTweener.Method.BounceIn)
			{
				num = this.BounceLogic(num);
			}
			else if (this.method == UTweener.Method.BounceOut)
			{
				num = 1f - this.BounceLogic(1f - num);
			}
			this.OnUpdate((this.animationCurve != null) ? this.animationCurve.Evaluate(num) : num, isFinished);
		}

		private float BounceLogic(float val)
		{
			if (val < 0.363636f)
			{
				val = 7.5685f * val * val;
			}
			else if (val < 0.727272f)
			{
				val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
			}
			else if (val < 0.90909f)
			{
				val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
			}
			else
			{
				val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
			}
			return val;
		}

		[Obsolete("Use PlayForward() instead")]
		public void Play()
		{
			this.Play(true);
		}

		public void PlayForward()
		{
			this.Play(true);
		}

		public void PlayReverse()
		{
			this.Play(false);
		}

		public void Play(bool forward)
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
			if (!forward)
			{
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			base.enabled = true;
			this.Update();
		}

		public void ResetToBeginning()
		{
			this.mStarted = false;
			this.mFactor = ((this.amountPerDelta < 0f) ? 1f : 0f);
			this.Sample(this.mFactor, false);
		}

		public void Toggle()
		{
			if (this.mFactor > 0f)
			{
				this.mAmountPerDelta = -this.amountPerDelta;
			}
			else
			{
				this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
			}
			base.enabled = true;
		}

		protected abstract void OnUpdate(float factor, bool isFinished);

		public virtual void SetStartToCurrentValue()
		{
		}

		public virtual void SetEndToCurrentValue()
		{
		}

		public static T Begin<T>(GameObject go, float duration) where T : UTweener
		{
			T t = go.GetComponent<T>();
			if (t != null && t.tweenGroup != 0)
			{
				t = default(T);
				T[] components = go.GetComponents<T>();
				int i = 0;
				int num = components.Length;
				while (i < num)
				{
					t = components[i];
					if (t != null && t.tweenGroup == 0)
					{
						break;
					}
					t = default(T);
					i++;
				}
			}
			if (t == null)
			{
				t = go.AddComponent<T>();
				if (t == null)
				{
					return default(T);
				}
			}
			t.mStarted = false;
			t.duration = duration;
			t.mFactor = 0f;
			t.mAmountPerDelta = Mathf.Abs(t.amountPerDelta);
			t.style = UTweener.Style.Once;
			t.animationCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f, 0f, 1f),
				new Keyframe(1f, 1f, 1f, 0f)
			});
			t.eventReceiver = null;
			t.callWhenFinished = null;
			t.enabled = true;
			return t;
		}

		public static UTweener current;

		[HideInInspector]
		public UTweener.Method method;

		public UTweener.Style style;

		public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});

		public bool ignoreTimeScale = true;

		public float delay;

		public float duration = 1f;

		[HideInInspector]
		public bool steeperCurves;

		public int tweenGroup;

		public List<UEventDelegate> onFinished = new List<UEventDelegate>();

		[HideInInspector]
		public GameObject eventReceiver;

		[HideInInspector]
		public string callWhenFinished;

		private bool mStarted;

		private float mStartTime;

		private float mDuration;

		private float mAmountPerDelta = 1000f;

		private float mFactor;

		private List<UEventDelegate> mTemp;

		public enum Method
		{
			Linear,
			EaseIn,
			EaseOut,
			EaseInOut,
			BounceIn,
			BounceOut
		}

		public enum Style
		{
			Once,
			Loop,
			PingPong
		}

		public enum Direction
		{
			Reverse = -1,
			Toggle,
			Forward
		}
	}
}
