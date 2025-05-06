using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.HabbyTimerManager
{
	public class HabbyTimer : MonoBehaviour
	{
		public static HabbyTimer Instance
		{
			get
			{
				if (HabbyTimer._instance == null)
				{
					HabbyTimer._instance = Object.FindObjectOfType(typeof(HabbyTimer)) as HabbyTimer;
					if (HabbyTimer._instance == null)
					{
						HabbyTimer._instance = new GameObject("HabbyTimer").AddComponent<HabbyTimer>();
						Object.DontDestroyOnLoad(HabbyTimer._instance.gameObject);
					}
				}
				return HabbyTimer._instance;
			}
		}

		private void Update()
		{
			HabbyTimer.Loop(Time.deltaTime);
			MainThreadTaskQueue.ExecuteTasks();
		}

		private static void Loop(float aDeltaTime)
		{
			foreach (KeyValuePair<int, HabbyTimer> keyValuePair in HabbyTimer.mTempDict)
			{
				if (keyValuePair.Value.Active && keyValuePair.Key == keyValuePair.Value.mId)
				{
					HabbyTimer.mTimersDict.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			HabbyTimer.mTempDict.Clear();
			foreach (KeyValuePair<int, HabbyTimer> keyValuePair2 in HabbyTimer.mTimersDict)
			{
				if (keyValuePair2.Value.Active && keyValuePair2.Key == keyValuePair2.Value.mId)
				{
					keyValuePair2.Value.Step(aDeltaTime);
				}
				else
				{
					HabbyTimer.mTempList.Add(keyValuePair2.Key);
				}
			}
			foreach (int num in HabbyTimer.mTempList)
			{
				HabbyTimer.mTimersDict.Remove(num);
			}
			HabbyTimer.mTempList.Clear();
			if ((float)HabbyTimer.mReusable.Count > (float)HabbyTimer.MAXIMUM_REUSABLE_COUNT * 1.5f)
			{
				HabbyTimer.Truncate();
			}
		}

		internal bool Active { get; private set; }

		private static HabbyTimer Get(int aTimerId)
		{
			HabbyTimer habbyTimer;
			if (HabbyTimer.mReusable.Count > 0)
			{
				habbyTimer = HabbyTimer.mReusable.Pop();
			}
			habbyTimer = new HabbyTimer();
			HabbyTimer.mTempDict.Add(aTimerId, habbyTimer);
			return habbyTimer;
		}

		public int Execute(Action<int> TimerAction, float aInterval, int aCount = 1, bool aExecuteNow = false, float aTimeScale = 1f)
		{
			if (TimerAction == null)
			{
				return -1;
			}
			HabbyTimer.mTimerId++;
			HabbyTimer habbyTimer = HabbyTimer.Get(HabbyTimer.mTimerId);
			habbyTimer.Active = true;
			habbyTimer.OnTrigger = TimerAction;
			habbyTimer.mId = HabbyTimer.mTimerId;
			habbyTimer.mInterval = aInterval;
			habbyTimer.mTotlaCount = aCount;
			habbyTimer.mCount = 0;
			habbyTimer.mTime = 0f;
			habbyTimer.mTimeScale = aTimeScale;
			if (aExecuteNow)
			{
				habbyTimer.Trigger(habbyTimer.mCount);
				habbyTimer.mCount++;
			}
			return HabbyTimer.mTimerId;
		}

		public static void Cancel(int aTimerId)
		{
			HabbyTimer habbyTimer = null;
			if (!HabbyTimer.mTimersDict.TryGetValue(aTimerId, out habbyTimer))
			{
				HabbyTimer.mTempDict.TryGetValue(aTimerId, out habbyTimer);
			}
			if (null != habbyTimer && habbyTimer.mId == aTimerId)
			{
				habbyTimer.Cancel();
			}
		}

		public static void SetTimeScale(int aTimerId, float aTimeScale)
		{
			HabbyTimer habbyTimer = null;
			if (!HabbyTimer.mTimersDict.TryGetValue(aTimerId, out habbyTimer))
			{
				HabbyTimer.mTempDict.TryGetValue(aTimerId, out habbyTimer);
			}
			if (null != habbyTimer && habbyTimer.mId == aTimerId)
			{
				habbyTimer.SetTimeScale(aTimeScale);
			}
		}

		private bool IntervalStep(float aDeltaTime)
		{
			if (this.mInterval > this.mTime)
			{
				return false;
			}
			this.mTime = 0f;
			int num = this.mTotlaCount;
			int num2 = this.mCount + 1;
			this.mCount = num2;
			return num >= num2;
		}

		internal void Step(float aDeltaTime)
		{
			if (this.IntervalStep(aDeltaTime))
			{
				this.Trigger(this.mCount);
			}
			this.mTime += aDeltaTime * this.mTimeScale;
		}

		internal void Trigger(int count)
		{
			try
			{
				this.OnTrigger(count);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			finally
			{
				if (this.mCount >= this.mTotlaCount)
				{
					this.Cancel();
				}
			}
		}

		internal void Cancel()
		{
			if (this.Active)
			{
				this.Active = false;
				this.OnTrigger = null;
				if (!HabbyTimer.mReusable.Contains(this))
				{
					HabbyTimer.mReusable.Push(this);
				}
			}
		}

		internal void SetTimeScale(float aTimeScale)
		{
			if (this.Active)
			{
				this.mTimeScale = aTimeScale;
			}
		}

		internal static void Truncate()
		{
			int num = Math.Max(HabbyTimer.MAXIMUM_REUSABLE_COUNT, HabbyTimer.mReusable.Count / 10);
			while (HabbyTimer.mReusable.Count > num)
			{
				HabbyTimer.mReusable.Pop();
			}
		}

		internal static void Clear()
		{
			foreach (KeyValuePair<int, HabbyTimer> keyValuePair in HabbyTimer.mTimersDict)
			{
				if (keyValuePair.Key == keyValuePair.Value.mId)
				{
					keyValuePair.Value.Cancel();
				}
			}
			foreach (KeyValuePair<int, HabbyTimer> keyValuePair2 in HabbyTimer.mTempDict)
			{
				if (keyValuePair2.Key == keyValuePair2.Value.mId)
				{
					keyValuePair2.Value.Cancel();
				}
			}
			HabbyTimer.mTimersDict.Clear();
			HabbyTimer.mTempDict.Clear();
			HabbyTimer.Truncate();
		}

		private static int mTimerId = 0;

		private static Dictionary<int, HabbyTimer> mTimersDict = new Dictionary<int, HabbyTimer>(32);

		private static Dictionary<int, HabbyTimer> mTempDict = new Dictionary<int, HabbyTimer>(32);

		private static List<int> mTempList = new List<int>();

		private static Stack<HabbyTimer> mReusable = new Stack<HabbyTimer>(32);

		public static int MAXIMUM_REUSABLE_COUNT = 8;

		protected static HabbyTimer _instance;

		private Action<int> OnTrigger;

		private int mId;

		private float mInterval;

		private int mTotlaCount;

		private int mCount;

		private float mTime;

		private float mTimeScale = 1f;
	}
}
