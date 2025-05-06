using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserUpdateGuideMaskRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserUpdateGuideMaskRequest> Parser
		{
			get
			{
				return UserUpdateGuideMaskRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
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
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
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
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
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
				if (num != 10U)
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
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<UserUpdateGuideMaskRequest> _parser = new MessageParser<UserUpdateGuideMaskRequest>(() => new UserUpdateGuideMaskRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int GuideMaskFieldNumber = 2;

		private ulong guideMask_;
	}
}
