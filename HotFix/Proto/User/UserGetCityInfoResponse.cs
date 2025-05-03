using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserGetCityInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetCityInfoResponse> Parser
		{
			get
			{
				return UserGetCityInfoResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
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
		public uint Avatar
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
		public uint AvatarFrame
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
		public ulong GuildId
		{
			get
			{
				return this.guildId_;
			}
			set
			{
				this.guildId_ = value;
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
		public int SlaveCount
		{
			get
			{
				return this.slaveCount_;
			}
			set
			{
				this.slaveCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public LordDto Extra
		{
			get
			{
				return this.extra_;
			}
			set
			{
				this.extra_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.NickName);
			}
			if (this.UserId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.UserId);
			}
			if (this.Avatar != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.AvatarFrame);
			}
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.GuildName);
			}
			if (this.GuildId != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.GuildId);
			}
			if (this.Power != 0L)
			{
				output.WriteRawTag(64);
				output.WriteInt64(this.Power);
			}
			if (this.SlaveCount != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.SlaveCount);
			}
			if (this.extra_ != null)
			{
				output.WriteRawTag(82);
				output.WriteMessage(this.Extra);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.Avatar != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarFrame);
			}
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
			}
			if (this.GuildId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildId);
			}
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			if (this.SlaveCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.SlaveCount);
			}
			if (this.extra_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Extra);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 40U)
				{
					if (num <= 18U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
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
							this.UserId = input.ReadInt64();
							continue;
						}
						if (num == 32U)
						{
							this.Avatar = input.ReadUInt32();
							continue;
						}
						if (num == 40U)
						{
							this.AvatarFrame = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 56U)
				{
					if (num == 50U)
					{
						this.GuildName = input.ReadString();
						continue;
					}
					if (num == 56U)
					{
						this.GuildId = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 64U)
					{
						this.Power = input.ReadInt64();
						continue;
					}
					if (num == 72U)
					{
						this.SlaveCount = input.ReadInt32();
						continue;
					}
					if (num == 82U)
					{
						if (this.extra_ == null)
						{
							this.extra_ = new LordDto();
						}
						input.ReadMessage(this.extra_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserGetCityInfoResponse> _parser = new MessageParser<UserGetCityInfoResponse>(() => new UserGetCityInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int UserIdFieldNumber = 3;

		private long userId_;

		public const int AvatarFieldNumber = 4;

		private uint avatar_;

		public const int AvatarFrameFieldNumber = 5;

		private uint avatarFrame_;

		public const int GuildNameFieldNumber = 6;

		private string guildName_ = "";

		public const int GuildIdFieldNumber = 7;

		private ulong guildId_;

		public const int PowerFieldNumber = 8;

		private long power_;

		public const int SlaveCountFieldNumber = 9;

		private int slaveCount_;

		public const int ExtraFieldNumber = 10;

		private LordDto extra_;
	}
}
