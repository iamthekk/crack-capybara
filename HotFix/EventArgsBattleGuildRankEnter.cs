using System;
using Dxx.Guild;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsBattleGuildRankEnter : BaseEventArgs
	{
		public void SetData(PVPRecordDto record, GuildRaceUserVSRecord guildRaceUserVSRecord)
		{
			this.m_record = record;
			this.m_guildRaceUserVSRecord = guildRaceUserVSRecord;
		}

		public override void Clear()
		{
		}

		public PVPRecordDto m_record;

		public GuildRaceUserVSRecord m_guildRaceUserVSRecord;
	}
}
