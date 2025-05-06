using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.User
{
	public sealed class UserUpdateGuideMaskResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserUpdateGuideMaskResponse> Parser
		{
			get
			{
				return UserUpdateGuideMaskResponse._parser;
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
		public ulong GuideMask
		{
			get
			{
				return this.guideMask_;
			}
			set
			{
				this.guideMask_ = value;
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
			if (this.GuideMask != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.GuideMask);
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
			if (this.GuideMask != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuideMask);
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
						this.GuideMask = input.ReadUInt64();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserUpdateGuideMaskResponse> _parser = new MessageParser<UserUpdateGuideMaskResponse>(() => new UserUpdateGuideMaskResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int GuideMaskFieldNumber = 2;

		private ulong guideMask_;
	}
}
