using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.ServerList
{
	public sealed class RoleDetailDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RoleDetailDto> Parser
		{
			get
			{
				return RoleDetailDto._parser;
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
		public uint GroupId
		{
			get
			{
				return this.groupId_;
			}
			set
			{
				this.groupId_ = value;
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
		public uint ServerId
		{
			get
			{
				return this.serverId_;
			}
			set
			{
				this.serverId_ = value;
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
		public ulong LastLoginPass
		{
			get
			{
				return this.lastLoginPass_;
			}
			set
			{
				this.lastLoginPass_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.NickName);
			}
			if (this.GroupId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.GroupId);
			}
			if (this.Avatar != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.AvatarFrame);
			}
			if (this.ServerId != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.ServerId);
			}
			if (this.Power != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.Power);
			}
			if (this.LastLoginPass != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.LastLoginPass);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.GroupId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GroupId);
			}
			if (this.Avatar != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarFrame);
			}
			if (this.ServerId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ServerId);
			}
			if (this.Power != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Power);
			}
			if (this.LastLoginPass != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.LastLoginPass);
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
					if (num == 10U)
					{
						this.NickName = input.ReadString();
						continue;
					}
					if (num == 16U)
					{
						this.GroupId = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.Avatar = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.AvatarFrame = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.ServerId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.Power = input.ReadUInt64();
						continue;
					}
					if (num == 56U)
					{
						this.LastLoginPass = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RoleDetailDto> _parser = new MessageParser<RoleDetailDto>(() => new RoleDetailDto());

		public const int NickNameFieldNumber = 1;

		private string nickName_ = "";

		public const int GroupIdFieldNumber = 2;

		private uint groupId_;

		public const int AvatarFieldNumber = 3;

		private uint avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private uint avatarFrame_;

		public const int ServerIdFieldNumber = 5;

		private uint serverId_;

		public const int PowerFieldNumber = 6;

		private ulong power_;

		public const int LastLoginPassFieldNumber = 7;

		private ulong lastLoginPass_;
	}
}
