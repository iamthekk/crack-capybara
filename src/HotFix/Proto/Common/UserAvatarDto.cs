using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UserAvatarDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserAvatarDto> Parser
		{
			get
			{
				return UserAvatarDto._parser;
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
		public int Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.ConfigId);
			}
			if (this.Type != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Type);
			}
			if (this.Status != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Status);
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
			if (this.Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Type);
			}
			if (this.Status != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Status);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Status = input.ReadInt32();
						}
					}
					else
					{
						this.Type = input.ReadInt32();
					}
				}
				else
				{
					this.ConfigId = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserAvatarDto> _parser = new MessageParser<UserAvatarDto>(() => new UserAvatarDto());

		public const int ConfigIdFieldNumber = 1;

		private int configId_;

		public const int TypeFieldNumber = 2;

		private int type_;

		public const int StatusFieldNumber = 3;

		private int status_;
	}
}
