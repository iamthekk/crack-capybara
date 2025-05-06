using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Collection
{
	public sealed class CollecStarResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CollecStarResponse> Parser
		{
			get
			{
				return CollecStarResponse._parser;
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
		public RepeatedField<CollectionDto> CollectionDto
		{
			get
			{
				return this.collectionDto_;
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
			this.collectionDto_.WriteTo(output, CollecStarResponse._repeated_collectionDto_codec);
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
			num += this.collectionDto_.CalculateSize(CollecStarResponse._repeated_collectionDto_codec);
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
						this.collectionDto_.AddEntriesFrom(input, CollecStarResponse._repeated_collectionDto_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<CollecStarResponse> _parser = new MessageParser<CollecStarResponse>(() => new CollecStarResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CollectionDtoFieldNumber = 2;

		private static readonly FieldCodec<CollectionDto> _repeated_collectionDto_codec = FieldCodec.ForMessage<CollectionDto>(18U, Proto.Common.CollectionDto.Parser);

		private readonly RepeatedField<CollectionDto> collectionDto_ = new RepeatedField<CollectionDto>();

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
