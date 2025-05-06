using System;

namespace HotFix
{
	public class NoticeInfoData
	{
		public bool isValid()
		{
			return this.expireTimestamp > DxxTools.Time.ServerTimestamp && this.effectiveTimestamp < DxxTools.Time.ServerTimestamp;
		}

		public string announcementId = "";

		public string title;

		public string content;

		public long effectiveTimestamp;

		public long expireTimestamp;

		public bool isNew;

		public bool readed;

		public NoticeBannerInfoData BannerInfoData;
	}
}
