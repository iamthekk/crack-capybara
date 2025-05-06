using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.User
{
	public sealed class UserUpdateSystemMaskResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserUpdateSystemMaskResponse> Parser
		{
			get
			{
				return UserUpdateSystemMaskResponse._parser;
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
		public ulong SystemMask
		{
			get
			{
				return this.systemMask_;
			}
			set
			{
				this.systemMask_ = value;
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
			if (this.SystemMask != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.SystemMask);
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
			if (this.SystemMask != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.SystemMask);
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
						this.SystemMask = input.ReadUInt64();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserUpdateSystemMaskResponse> _parser = new MessageParser<UserUpdateSystemMaskResponse>(() => new UserUpdateSystemMaskResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int SystemMaskFieldNumber = 2;

		private ulong systemMask_;
	}
}
