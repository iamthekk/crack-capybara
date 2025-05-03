using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class PowerRankDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PowerRankDto> Parser
		{
			get
			{
				return PowerRankDto._parser;
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
		public int Status
		{
			get
			{
				return this.status_;
			}
			set
			{
				this.status_ = value;
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
			if (this.Status != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Status);
			}
			if (this.Power != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.Power);
			}
			if (this.extra_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.Extra);
			}
			if (this.TitleId != 0)
			{
				output.WriteRawTag(72);
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
			if (this.Status != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Status);
			}
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			if (this.extra_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Extra);
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
							this.Level = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Avatar = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.AvatarFrame = input.ReadInt32();
						continue;
					}
					if (num == 48U)
					{
						this.Status = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.Power = input.ReadInt64();
						continue;
					}
					if (num == 66U)
					{
						if (this.extra_ == null)
						{
							this.extra_ = new LordDto();
						}
						input.ReadMessage(this.extra_);
						continue;
					}
					if (num == 72U)
					{
						this.TitleId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PowerRankDto> _parser = new MessageParser<PowerRankDto>(() => new PowerRankDto());

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

		public const int StatusFieldNumber = 6;

		private int status_;

		public const int PowerFieldNumber = 7;

		private long power_;

		public const int ExtraFieldNumber = 8;

		private LordDto extra_;

		public const int TitleIdFieldNumber = 9;

		private int titleId_;
	}
}
