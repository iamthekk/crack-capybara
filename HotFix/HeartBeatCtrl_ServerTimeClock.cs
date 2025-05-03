using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
	public class HeartBeatCtrl_ServerTimeClock : MonoBehaviour
	{
		public static HeartBeatCtrl_ServerTimeClock Instance
		{
			get
			{
				if (HeartBeatCtrl_ServerTimeClock._instance == null)
				{
					GameObject gameObject = new GameObject("ServerTimeClock");
					Object.DontDestroyOnLoad(gameObject);
					HeartBeatCtrl_ServerTimeClock._instance = gameObject.AddComponent<HeartBeatCtrl_ServerTimeClock>();
				}
				return HeartBeatCtrl_ServerTimeClock._instance;
			}
		}

		public void Update()
		{
			this.mTimeCountDown -= (double)Time.unscaledDeltaTime;
			if (this.mTimeCountDown <= 0.0)
			{
				this.CheckInvoke();
				this.CalcTimeCountDown();
			}
		}

		private void CheckInvoke()
		{
			this.mCurServerTick = DxxTools.Time.ServerTimestamp;
			for (int i = 0; i < this.mClockList.Count; i++)
			{
				ServerTimeClockCall serverTimeClockCall = this.mClockList[i];
				if (!serverTimeClockCall.HasCall && this.mCurServerTick >= serverTimeClockCall.InvokeTick)
				{
					serverTimeClockCall.Invoke();
				}
			}
		}

		private void CalcTimeCountDown()
		{
			this.mCurServerTick = DxxTools.Time.ServerTimestamp;
			long num = long.MaxValue;
			for (int i = 0; i < this.mClockList.Count; i++)
			{
				ServerTimeClockCall serverTimeClockCall = this.mClockList[i];
				if (!serverTimeClockCall.HasCall && serverTimeClockCall.InvokeTick > this.mCurServerTick && num > serverTimeClockCall.InvokeTick)
				{
					num = serverTimeClockCall.InvokeTick;
				}
			}
			this.mTimeCountDown = (double)(num - this.mCurServerTick);
			if (this.mTimeCountDown < 0.0)
			{
				this.mTimeCountDown = 1.0;
			}
			if (this.mTimeCountDown > 600.0)
			{
				this.mTimeCountDown = 600.0;
			}
		}

		public void OnAddServerTimeClock(ServerTimeClockCall ClockCall)
		{
			bool flag = false;
			for (int i = 0; i < this.mClockList.Count; i++)
			{
				if (this.mClockList[i].IsSame(ClockCall))
				{
					this.mClockList[i] = ClockCall;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.mClockList.Add(ClockCall);
			}
			this.CalcTimeCountDown();
		}

		public void OnRemoveServerTimeClock(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return;
			}
			for (int i = 0; i < this.mClockList.Count; i++)
			{
				ServerTimeClockCall serverTimeClockCall = this.mClockList[i];
				if (key == serverTimeClockCall.UnionKey)
				{
					this.mClockList.RemoveAt(i);
					break;
				}
			}
			this.CalcTimeCountDown();
		}

		public void RemoveAll()
		{
			this.mClockList.Clear();
			this.CalcTimeCountDown();
		}

		private long mCurServerTick;

		private double mTimeCountDown;

		private List<ServerTimeClockCall> mClockList = new List<ServerTimeClockCall>();

		private static HeartBeatCtrl_ServerTimeClock _instance;
	}
}
