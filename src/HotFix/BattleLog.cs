using System;
using System.Text;

namespace HotFix
{
	public class BattleLog : Singleton<BattleLog>
	{
		public StringBuilder CLog
		{
			get
			{
				return this.m_clog;
			}
		}

		public string BattleServerLogId { get; private set; }

		public void AddCLog(StringBuilder log)
		{
			this.m_clog.Append(log);
		}

		public void AddCLog(string log)
		{
			this.m_clog.Append(log);
		}

		public void ClearCLog()
		{
			this.m_clog.Clear();
		}

		public StringBuilder SLog
		{
			get
			{
				return this.m_slog;
			}
		}

		public void AddSLog(StringBuilder log, string serverLogId)
		{
			this.m_slog.Append(log);
		}

		public void AddSLog(string log, string serverLogId)
		{
			this.BattleServerLogId = serverLogId;
			this.m_slog.Append(log);
		}

		public void ClearSLog()
		{
			this.m_slog.Clear();
		}

		private StringBuilder m_clog = new StringBuilder();

		private StringBuilder m_slog = new StringBuilder();
	}
}
