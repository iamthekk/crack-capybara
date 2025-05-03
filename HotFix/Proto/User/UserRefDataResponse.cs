using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserRefDataResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserRefDataResponse> Parser
		{
			get
			{
				return UserRefDataResponse._parser;
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
		public PetInfo PetInfo
		{
			get
			{
				return this.petInfo_;
			}
			set
			{
				this.petInfo_ = value;
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
			if (this.petInfo_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.PetInfo);
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
			if (this.petInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.PetInfo);
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
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						if (this.petInfo_ == null)
						{
							this.petInfo_ = new PetInfo();
						}
						input.ReadMessage(this.petInfo_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserRefDataResponse> _parser = new MessageParser<UserRefDataResponse>(() => new UserRefDataResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PetInfoFieldNumber = 2;

		private PetInfo petInfo_;
	}
}
