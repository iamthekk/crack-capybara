using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Chapter
{
	public sealed class ChapterActRankDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterActRankDto> Parser
		{
			get
			{
				return ChapterActRankDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string NickName
		{
			get
			{
				return this.nickName_;
			}
			set
			{
				this.nickName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Avatar
		{
			get
			{
				return this.avatar_;
			}
			set
			{
				this.avatar_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int AvatarFrame
		{
			get
			{
				return this.avatarFrame_;
			}
			set
			{
				this.avatarFrame_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Score
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.UserId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.NickName);
			}
			if (this.Level != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Level);
			}
			if (this.Avatar != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.AvatarFrame);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Score);
			}
			if (this.Rank != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.Rank);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.Level != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Level);
			}
			if (this.Avatar != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AvatarFrame);
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
			}
			if (this.Rank != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Rank);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.UserId = input.ReadInt64();
						continue;
					}
					if (num == 18U)
					{
						this.NickName = input.ReadString();
						continue;
					}
					if (num == 24U)
					{
						this.Level = input.ReadInt32();
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.Avatar = input.ReadInt32();
						continue;
					}
					if (num == 40U)
					{
						this.AvatarFrame = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
					if (num == 56U)
					{
						this.Rank = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterActRankDto> _parser = new MessageParser<ChapterActRankDto>(() => new ChapterActRankDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int LevelFieldNumber = 3;

		private int level_;

		public const int AvatarFieldNumber = 4;

		private int avatar_;

		public const int AvatarFrameFieldNumber = 5;

		private int avatarFrame_;

		public const int ScoreFieldNumber = 6;

		private int score_;

		public const int RankFieldNumber = 7;

		private int rank_;
	}
}
