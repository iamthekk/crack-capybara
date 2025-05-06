using System;
using System.Collections.Generic;

namespace Framework.NetWork
{
	public class ProtocolRateLimiter
	{
		public ProtocolRateLimiter(TimeSpan interval, int maxCount, Type messageType)
		{
			this._interval = interval;
			this._maxCount = maxCount;
			this._messageType = messageType;
			this.timestamps = new Queue<DateTime>();
		}

		public bool Record()
		{
			Queue<DateTime> queue = this.timestamps;
			bool flag2;
			lock (queue)
			{
				while (this.timestamps.Count > 0 && DateTime.UtcNow - this.timestamps.Peek() > this._interval)
				{
					this.timestamps.Dequeue();
				}
				if (this.timestamps.Count < this._maxCount)
				{
					this.timestamps.Enqueue(DateTime.UtcNow);
					flag2 = true;
				}
				else
				{
					this.TriggerAlarm();
					flag2 = false;
				}
			}
			return flag2;
		}

		private void TriggerAlarm()
		{
			this._dicTemp.Clear();
			this._dicTemp.Add("MessageType", this._messageType.ToString());
		}

		private readonly TimeSpan _interval;

		private readonly int _maxCount;

		private readonly Queue<DateTime> timestamps;

		private Type _messageType;

		private Dictionary<string, object> _dicTemp = new Dictionary<string, object>();
	}
}
