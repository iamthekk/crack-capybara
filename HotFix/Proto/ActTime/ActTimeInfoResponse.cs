using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.ActTime
{
	public sealed class ActTimeInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActTimeInfoResponse> Parser
		{
			get
			{
				return ActTimeInfoResponse._parser;
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
		public RepeatedField<Consume> ConsumeData
		{
			get
			{
				return this.consumeData_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Shop> ShopData
		{
			get
			{
				return this.shopData_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Drop> DropData
		{
			get
			{
				return this.dropData_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Pay> PayData
		{
			get
			{
				return this.payData_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Chapter> ChapterData
		{
			get
			{
				return this.chapterData_;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			this.consumeData_.WriteTo(output, ActTimeInfoResponse._repeated_consumeData_codec);
			this.shopData_.WriteTo(output, ActTimeInfoResponse._repeated_shopData_codec);
			this.dropData_.WriteTo(output, ActTimeInfoResponse._repeated_dropData_codec);
			this.payData_.WriteTo(output, ActTimeInfoResponse._repeated_payData_codec);
			this.chapterData_.WriteTo(output, ActTimeInfoResponse._repeated_chapterData_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			num += this.consumeData_.CalculateSize(ActTimeInfoResponse._repeated_consumeData_codec);
			num += this.shopData_.CalculateSize(ActTimeInfoResponse._repeated_shopData_codec);
			num += this.dropData_.CalculateSize(ActTimeInfoResponse._repeated_dropData_codec);
			num += this.payData_.CalculateSize(ActTimeInfoResponse._repeated_payData_codec);
			return num + this.chapterData_.CalculateSize(ActTimeInfoResponse._repeated_chapterData_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 26U)
					{
						this.consumeData_.AddEntriesFrom(input, ActTimeInfoResponse._repeated_consumeData_codec);
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 34U)
					{
						this.shopData_.AddEntriesFrom(input, ActTimeInfoResponse._repeated_shopData_codec);
						continue;
					}
					if (num == 42U)
					{
						this.dropData_.AddEntriesFrom(input, ActTimeInfoResponse._repeated_dropData_codec);
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						this.payData_.AddEntriesFrom(input, ActTimeInfoResponse._repeated_payData_codec);
						continue;
					}
					if (num == 58U)
					{
						this.chapterData_.AddEntriesFrom(input, ActTimeInfoResponse._repeated_chapterData_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ActTimeInfoResponse> _parser = new MessageParser<ActTimeInfoResponse>(() => new ActTimeInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ConsumeDataFieldNumber = 3;

		private static readonly FieldCodec<Consume> _repeated_consumeData_codec = FieldCodec.ForMessage<Consume>(26U, Consume.Parser);

		private readonly RepeatedField<Consume> consumeData_ = new RepeatedField<Consume>();

		public const int ShopDataFieldNumber = 4;

		private static readonly FieldCodec<Shop> _repeated_shopData_codec = FieldCodec.ForMessage<Shop>(34U, Shop.Parser);

		private readonly RepeatedField<Shop> shopData_ = new RepeatedField<Shop>();

		public const int DropDataFieldNumber = 5;

		private static readonly FieldCodec<Drop> _repeated_dropData_codec = FieldCodec.ForMessage<Drop>(42U, Drop.Parser);

		private readonly RepeatedField<Drop> dropData_ = new RepeatedField<Drop>();

		public const int PayDataFieldNumber = 6;

		private static readonly FieldCodec<Pay> _repeated_payData_codec = FieldCodec.ForMessage<Pay>(50U, Pay.Parser);

		private readonly RepeatedField<Pay> payData_ = new RepeatedField<Pay>();

		public const int ChapterDataFieldNumber = 7;

		private static readonly FieldCodec<Chapter> _repeated_chapterData_codec = FieldCodec.ForMessage<Chapter>(58U, Chapter.Parser);

		private readonly RepeatedField<Chapter> chapterData_ = new RepeatedField<Chapter>();
	}
}
