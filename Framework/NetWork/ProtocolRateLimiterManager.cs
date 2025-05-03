using System;
using System.Collections.Generic;

namespace Framework.NetWork
{
	public class ProtocolRateLimiterManager : Singleton<ProtocolRateLimiterManager>
	{
		public bool Record(Type messageType)
		{
			if (this._rateLimiters.ContainsKey(messageType))
			{
				return this._rateLimiters[messageType].Record();
			}
			ProtocolRateLimiter protocolRateLimiter = new ProtocolRateLimiter(this._interval, this._maxCount, messageType);
			this._rateLimiters.Add(messageType, protocolRateLimiter);
			return protocolRateLimiter.Record();
		}

		private readonly TimeSpan _interval = TimeSpan.FromSeconds(1.0);

		private readonly int _maxCount = 10;

		private Dictionary<Type, ProtocolRateLimiter> _rateLimiters = new Dictionary<Type, ProtocolRateLimiter>();
	}
}
