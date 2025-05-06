using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class HellRankDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellRankDto> Parser
		{
			get
			{
				return HellRankDto._parser;
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
		public long Power
		{
			get
			{
				return this.power_;
			}
			set
			{
				this.power_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Tower
		{
			get
			{
				return this.tower_;
			}
			set
			{
				this.tower_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GuildName
		{
			get
			{
				return this.guildName_;
			}
			set
			{
				this.guildName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int TitleId
		{
			get
			{
				return this.titleId_;
			}
			set
			{
				this.titleId_ = value;
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
			if (this.Avatar != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.AvatarFrame);
			}
			if (this.Power != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.Power);
			}
			if (this.Tower != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Tower);
			}
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.GuildName);
			}
			if (this.TitleId != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.TitleId);
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
			if (this.Avatar != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AvatarFrame);
			}
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			if (this.Tower != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Tower);
			}
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
			}
			if (this.TitleId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TitleId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num <= 18U)
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
					}
					else
					{
						if (num == 24U)
						{
							this.Avatar = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.AvatarFrame = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.Power = input.ReadInt64();
						continue;
					}
					if (num == 48U)
					{
						this.Tower = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.GuildName = input.ReadString();
						continue;
					}
					if (num == 64U)
					{
						this.TitleId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<HellRankDto> _parser = new MessageParser<HellRankDto>(() => new HellRankDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int AvatarFieldNumber = 3;

		private int avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private int avatarFrame_;

		public const int PowerFieldNumber = 5;

		private long power_;

		public const int TowerFieldNumber = 6;

		private int tower_;

		public const int GuildNameFieldNumber = 7;

		private string guildName_ = "";

		public const int TitleIdFieldNumber = 8;

		private int titleId_;
	}
}
