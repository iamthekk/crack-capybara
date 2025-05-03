using System;
using System.Collections.Generic;
using Proto.Common;

namespace HotFix
{
	public class CrossArenaChallengeRecord
	{
		public string GetNick()
		{
			if (this.UserInfo == null)
			{
				return "???";
			}
			if (string.IsNullOrEmpty(this.UserInfo.NickName))
			{
				return DxxTools.GetDefaultNick(this.UserInfo.UserId);
			}
			return this.UserInfo.NickName;
		}

		public void SetData(CrossArenaRecordDto data)
		{
			this.UserInfo = data.UserInfo;
			this.ScoreChg = data.Score;
			this.Time = data.Time;
			this.IsAttack = data.IsAtt;
			this.ReportID = data.ReportId;
		}

		public static List<CrossArenaChallengeRecord> ToList(IList<CrossArenaRecordDto> datalist)
		{
			List<CrossArenaChallengeRecord> list = new List<CrossArenaChallengeRecord>();
			for (int i = 0; i < datalist.Count; i++)
			{
				CrossArenaRecordDto crossArenaRecordDto = datalist[i];
				CrossArenaChallengeRecord crossArenaChallengeRecord = new CrossArenaChallengeRecord();
				crossArenaChallengeRecord.SetData(crossArenaRecordDto);
				list.Add(crossArenaChallengeRecord);
			}
			return list;
		}

		public static int SortByTime(CrossArenaChallengeRecord x, CrossArenaChallengeRecord y)
		{
			return y.Time.CompareTo(x.Time);
		}

		public UserInfoDto UserInfo;

		public long Time;

		public int ScoreChg;

		public bool IsAttack;

		public long ReportID;
	}
}
