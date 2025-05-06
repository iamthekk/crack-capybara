using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.LeaderBoard
{
	public sealed class RankUserDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RankUserDto> Parser
		{
			get
			{
				return RankUserDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public UserInfoDto UserInfo
		{
			get
			{
				return this.userInfo_;
			}
			set
			{
				this.userInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Rank
		{
			get
			{
				return this.rank_;
			}
			set
			{
				this.rank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.UserInfo);
			}
			if (this.Rank != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Rank);
			}
			if (this.Score != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.Score);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.Rank != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Rank);
			}
			if (this.Score != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Score);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Score = input.ReadInt64();
						}
					}
					else
					{
						this.Rank = input.ReadInt32();
					}
				}
				else
				{
					if (this.userInfo_ == null)
					{
						this.userInfo_ = new UserInfoDto();
					}
					input.ReadMessage(this.userInfo_);
				}
			}
		}

		private static readonly MessageParser<RankUserDto> _parser = new MessageParser<RankUserDto>(() => new RankUserDto());

		public const int UserInfoFieldNumber = 1;

		private UserInfoDto userInfo_;

		public const int RankFieldNumber = 2;

		private int rank_;

		public const int ScoreFieldNumber = 3;

		private long score_;
	}
}
