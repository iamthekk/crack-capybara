using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;
using Proto.SignIn;

namespace Proto.Activity
{
	public sealed class ActivityGetListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActivityGetListResponse> Parser
		{
			get
			{
				return ActivityGetListResponse._parser;
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
		public SignInData SignInData
		{
			get
			{
				return this.signInData_;
			}
			set
			{
				this.signInData_ = value;
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
		public RepeatedField<Chapter> Chapter
		{
			get
			{
				return this.chapter_;
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
			if (this.signInData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.SignInData);
			}
			this.consumeData_.WriteTo(output, ActivityGetListResponse._repeated_consumeData_codec);
			this.shopData_.WriteTo(output, ActivityGetListResponse._repeated_shopData_codec);
			this.dropData_.WriteTo(output, ActivityGetListResponse._repeated_dropData_codec);
			this.payData_.WriteTo(output, ActivityGetListResponse._repeated_payData_codec);
			this.chapter_.WriteTo(output, ActivityGetListResponse._repeated_chapter_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.signInData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SignInData);
			}
			num += this.consumeData_.CalculateSize(ActivityGetListResponse._repeated_consumeData_codec);
			num += this.shopData_.CalculateSize(ActivityGetListResponse._repeated_shopData_codec);
			num += this.dropData_.CalculateSize(ActivityGetListResponse._repeated_dropData_codec);
			num += this.payData_.CalculateSize(ActivityGetListResponse._repeated_payData_codec);
			return num + this.chapter_.CalculateSize(ActivityGetListResponse._repeated_chapter_codec);
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
						if (this.signInData_ == null)
						{
							this.signInData_ = new SignInData();
						}
						input.ReadMessage(this.signInData_);
						continue;
					}
					if (num == 26U)
					{
						this.consumeData_.AddEntriesFrom(input, ActivityGetListResponse._repeated_consumeData_codec);
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 34U)
					{
						this.shopData_.AddEntriesFrom(input, ActivityGetListResponse._repeated_shopData_codec);
						continue;
					}
					if (num == 42U)
					{
						this.dropData_.AddEntriesFrom(input, ActivityGetListResponse._repeated_dropData_codec);
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						this.payData_.AddEntriesFrom(input, ActivityGetListResponse._repeated_payData_codec);
						continue;
					}
					if (num == 58U)
					{
						this.chapter_.AddEntriesFrom(input, ActivityGetListResponse._repeated_chapter_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ActivityGetListResponse> _parser = new MessageParser<ActivityGetListResponse>(() => new ActivityGetListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int SignInDataFieldNumber = 2;

		private SignInData signInData_;

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

		public const int ChapterFieldNumber = 7;

		private static readonly FieldCodec<Chapter> _repeated_chapter_codec = FieldCodec.ForMessage<Chapter>(58U, Proto.Common.Chapter.Parser);

		private readonly RepeatedField<Chapter> chapter_ = new RepeatedField<Chapter>();
	}
}
