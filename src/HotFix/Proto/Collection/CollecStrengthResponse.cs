using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Collection
{
	public sealed class CollecStrengthResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CollecStrengthResponse> Parser
		{
			get
			{
				return CollecStrengthResponse._parser;
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
		public CollectionDto CollectionDto
		{
			get
			{
				return this.collectionDto_;
			}
			set
			{
				this.collectionDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
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
			if (this.collectionDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CollectionDto);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.CommonData);
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
			if (this.collectionDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CollectionDto);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
						}
					}
					else
					{
						if (this.collectionDto_ == null)
						{
							this.collectionDto_ = new CollectionDto();
						}
						input.ReadMessage(this.collectionDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<CollecStrengthResponse> _parser = new MessageParser<CollecStrengthResponse>(() => new CollecStrengthResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CollectionDtoFieldNumber = 2;

		private CollectionDto collectionDto_;

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
