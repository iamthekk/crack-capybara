using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UserUnlockAvatarDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserUnlockAvatarDto> Parser
		{
			get
			{
				return UserUnlockAvatarDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong ExpireTime
		{
			get
			{
				return this.expireTime_;
			}
			set
			{
				this.expireTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.ConfigId);
			}
			if (this.ExpireTime != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.ExpireTime);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.ExpireTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ExpireTime);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.ExpireTime = input.ReadUInt64();
					}
				}
				else
				{
					this.ConfigId = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserUnlockAvatarDto> _parser = new MessageParser<UserUnlockAvatarDto>(() => new UserUnlockAvatarDto());

		public const int ConfigIdFieldNumber = 1;

		private int configId_;

		public const int ExpireTimeFieldNumber = 2;

		private ulong expireTime_;
	}
}
