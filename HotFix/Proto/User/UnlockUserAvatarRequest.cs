using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UnlockUserAvatarRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UnlockUserAvatarRequest> Parser
		{
			get
			{
				return UnlockUserAvatarRequest._parser;
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
		public uint ItemType
		{
			get
			{
				return this.itemType_;
			}
			set
			{
				this.itemType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ItemId
		{
			get
			{
				return this.itemId_;
			}
			set
			{
				this.itemId_ = value;
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
			if (this.ItemType != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ItemType);
			}
			if (this.ItemId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ItemId);
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
			if (this.ItemType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ItemType);
			}
			if (this.ItemId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ItemId);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.ItemId = input.ReadUInt32();
						}
					}
					else
					{
						this.ItemType = input.ReadUInt32();
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

		private static readonly MessageParser<UnlockUserAvatarRequest> _parser = new MessageParser<UnlockUserAvatarRequest>(() => new UnlockUserAvatarRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ItemTypeFieldNumber = 2;

		private uint itemType_;

		public const int ItemIdFieldNumber = 3;

		private uint itemId_;
	}
}
