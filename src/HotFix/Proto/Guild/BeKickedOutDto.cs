using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class BeKickedOutDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BeKickedOutDto> Parser
		{
			get
			{
				return BeKickedOutDto._parser;
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
		public long FromUserId
		{
			get
			{
				return this.fromUserId_;
			}
			set
			{
				this.fromUserId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string FromUserNickName
		{
			get
			{
				return this.fromUserNickName_;
			}
			set
			{
				this.fromUserNickName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint FromUserPosition
		{
			get
			{
				return this.fromUserPosition_;
			}
			set
			{
				this.fromUserPosition_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.GuildId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.GuildName);
			}
			if (this.FromUserId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.FromUserId);
			}
			if (this.FromUserNickName.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.FromUserNickName);
			}
			if (this.FromUserPosition != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.FromUserPosition);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.GuildId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
			}
			if (this.FromUserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.FromUserId);
			}
			if (this.FromUserNickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.FromUserNickName);
			}
			if (this.FromUserPosition != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.FromUserPosition);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.GuildId = input.ReadUInt64();
						continue;
					}
					if (num == 18U)
					{
						this.GuildName = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.FromUserId = input.ReadInt64();
						continue;
					}
					if (num == 34U)
					{
						this.FromUserNickName = input.ReadString();
						continue;
					}
					if (num == 40U)
					{
						this.FromUserPosition = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<BeKickedOutDto> _parser = new MessageParser<BeKickedOutDto>(() => new BeKickedOutDto());

		public const int GuildIdFieldNumber = 1;

		private ulong guildId_;

		public const int GuildNameFieldNumber = 2;

		private string guildName_ = "";

		public const int FromUserIdFieldNumber = 3;

		private long fromUserId_;

		public const int FromUserNickNameFieldNumber = 4;

		private string fromUserNickName_ = "";

		public const int FromUserPositionFieldNumber = 5;

		private uint fromUserPosition_;
	}
}
