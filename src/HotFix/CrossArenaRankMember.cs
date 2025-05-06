using System;
using System.Collections.Generic;
using Proto.Common;

namespace HotFix
{
	public class CrossArenaRankMember
	{
		public string GetNick()
		{
			if (string.IsNullOrEmpty(this.Nick))
			{
				return DxxTools.GetDefaultNick(this.UserID);
			}
			return this.Nick;
		}

		public void CloneFrom(CrossArenaRankMember m)
		{
			this.UserID = m.UserID;
			this.Nick = m.Nick;
			this.Avatar = m.Avatar;
			this.AvatarFrame = m.AvatarFrame;
			this.Score = m.Score;
			this.Power = m.Power;
		}

		public void SetData(CrossArenaRankDto data)
		{
			this.UserID = data.UserId;
			this.Nick = data.NickName;
			this.Avatar = data.Avatar;
			this.AvatarFrame = data.AvatarFrame;
			this.Score = data.Score;
			this.Power = data.Power;
			this.Rank = data.RankIndex + 1;
		}

		public static List<CrossArenaRankMember> ToList(IList<CrossArenaRankDto> datalist)
		{
			List<CrossArenaRankMember> list = new List<CrossArenaRankMember>();
			for (int i = 0; i < datalist.Count; i++)
			{
				CrossArenaRankDto crossArenaRankDto = datalist[i];
				CrossArenaRankMember crossArenaRankMember = new CrossArenaRankMember();
				crossArenaRankMember.SetData(crossArenaRankDto);
				list.Add(crossArenaRankMember);
			}
			return list;
		}

		public void SetDataWithRank(CrossArenaRankDto data)
		{
			this.UserID = data.UserId;
			this.Nick = data.NickName;
			this.Avatar = data.Avatar;
			this.AvatarFrame = data.AvatarFrame;
			this.Score = data.Score;
			this.Power = data.Power;
			this.Rank = data.RankIndex;
		}

		public static List<CrossArenaRankMember> ToListWithRank(IList<CrossArenaRankDto> datalist)
		{
			List<CrossArenaRankMember> list = new List<CrossArenaRankMember>();
			for (int i = 0; i < datalist.Count; i++)
			{
				CrossArenaRankDto crossArenaRankDto = datalist[i];
				CrossArenaRankMember crossArenaRankMember = new CrossArenaRankMember();
				crossArenaRankMember.SetDataWithRank(crossArenaRankDto);
				list.Add(crossArenaRankMember);
			}
			return list;
		}

		public long UserID;

		public string Nick;

		public int Avatar;

		public int AvatarFrame;

		public int Score;

		public long Power;

		public int Rank;
	}
}
