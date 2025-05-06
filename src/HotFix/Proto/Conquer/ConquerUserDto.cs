using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Conquer
{
	public sealed class ConquerUserDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ConquerUserDto> Parser
		{
			get
			{
				return ConquerUserDto._parser;
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
		public ulong Power
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
		public uint Coin
		{
			get
			{
				return this.coin_;
			}
			set
			{
				this.coin_ = value;
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
			if (this.Avatar != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.AvatarFrame);
			}
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.NickName);
			}
			if (this.Power != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.Power);
			}
			if (this.Coin != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Coin);
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
			if (this.Avatar != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AvatarFrame);
			}
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.Power != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Power);
			}
			if (this.Coin != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Coin);
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
					if (num == 16U)
					{
						this.Avatar = input.ReadInt32();
						continue;
					}
					if (num == 24U)
					{
						this.AvatarFrame = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						this.NickName = input.ReadString();
						continue;
					}
					if (num == 40U)
					{
						this.Power = input.ReadUInt64();
						continue;
					}
					if (num == 48U)
					{
						this.Coin = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ConquerUserDto> _parser = new MessageParser<ConquerUserDto>(() => new ConquerUserDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int AvatarFieldNumber = 2;

		private int avatar_;

		public const int AvatarFrameFieldNumber = 3;

		private int avatarFrame_;

		public const int NickNameFieldNumber = 4;

		private string nickName_ = "";

		public const int PowerFieldNumber = 5;

		private ulong power_;

		public const int CoinFieldNumber = 6;

		private uint coin_;
	}
}
