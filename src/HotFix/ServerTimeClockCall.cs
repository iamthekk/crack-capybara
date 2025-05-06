using System;

namespace HotFix
{
	public class ServerTimeClockCall
	{
		public ServerTimeClockCall(string unionkey, Action callback)
		{
			this.UnionKey = unionkey;
			this.Callback = callback;
			this.HasCall = false;
		}

		public void SetTick(long tick)
		{
			this.InvokeTick = tick;
			this.HasCall = false;
		}

		public void SetClockTime(int hour, int minute, int second)
		{
			DateTime dateTime = DxxTools.TickToTime(DxxTools.Time.ServerTimestamp);
			DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second);
			if (dateTime2 < dateTime)
			{
				dateTime2 = dateTime2.AddDays(1.0);
			}
			this.InvokeTick = DxxTools.GetTotalSecend(dateTime2);
			this.HasCall = false;
		}

		public void Invoke()
		{
			this.HasCall = true;
			Action callback = this.Callback;
			if (callback != null)
			{
				callback();
			}
			if (this.RepeatDaily)
			{
				this.HasCall = false;
				this.InvokeTick += (long)this.RepeatInterval;
			}
		}

		public bool IsSame(ServerTimeClockCall other)
		{
			return !string.IsNullOrEmpty(other.UnionKey) && other.UnionKey == this.UnionKey;
		}

		public static ServerTimeClockCall ZERO(string unionkey, Action callback)
		{
			ServerTimeClockCall serverTimeClockCall = new ServerTimeClockCall(unionkey, callback);
			serverTimeClockCall.SetClockTime(0, 0, 0);
			return serverTimeClockCall;
		}

		public string UnionKey;

		public long InvokeTick;

		public Action Callback;

		public bool HasCall;

		public bool RepeatDaily = true;

		public int RepeatInterval = 86400;
	}
}
