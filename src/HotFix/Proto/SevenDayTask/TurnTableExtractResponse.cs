using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class TurnTableExtractResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnTableExtractResponse> Parser
		{
			get
			{
				return TurnTableExtractResponse._parser;
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
		public RepeatedField<DropItemDto> DropItemDtos
		{
			get
			{
				return this.dropItemDtos_;
			}
		}

		[DebuggerNonUserCode]
		public uint Count
		{
			get
			{
				return this.count_;
			}
			set
			{
				this.count_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, int> SmallGuaranteeCount
		{
			get
			{
				return this.smallGuaranteeCount_;
			}
		}

		[DebuggerNonUserCode]
		public int BigRewardCount
		{
			get
			{
				return this.bigRewardCount_;
			}
			set
			{
				this.bigRewardCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int BigGuaranteeCount
		{
			get
			{
				return this.bigGuaranteeCount_;
			}
			set
			{
				this.bigGuaranteeCount_ = value;
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
			this.dropItemDtos_.WriteTo(output, TurnTableExtractResponse._repeated_dropItemDtos_codec);
			if (this.Count != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Count);
			}
			this.smallGuaranteeCount_.WriteTo(output, TurnTableExtractResponse._map_smallGuaranteeCount_codec);
			if (this.BigRewardCount != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.BigRewardCount);
			}
			if (this.BigGuaranteeCount != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.BigGuaranteeCount);
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
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			num += this.dropItemDtos_.CalculateSize(TurnTableExtractResponse._repeated_dropItemDtos_codec);
			if (this.Count != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Count);
			}
			num += this.smallGuaranteeCount_.CalculateSize(TurnTableExtractResponse._map_smallGuaranteeCount_codec);
			if (this.BigRewardCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BigRewardCount);
			}
			if (this.BigGuaranteeCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BigGuaranteeCount);
			}
			return num;
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
						this.dropItemDtos_.AddEntriesFrom(input, TurnTableExtractResponse._repeated_dropItemDtos_codec);
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 32U)
					{
						this.Count = input.ReadUInt32();
						continue;
					}
					if (num == 42U)
					{
						this.smallGuaranteeCount_.AddEntriesFrom(input, TurnTableExtractResponse._map_smallGuaranteeCount_codec);
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.BigRewardCount = input.ReadInt32();
						continue;
					}
					if (num == 56U)
					{
						this.BigGuaranteeCount = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TurnTableExtractResponse> _parser = new MessageParser<TurnTableExtractResponse>(() => new TurnTableExtractResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int DropItemDtosFieldNumber = 3;

		private static readonly FieldCodec<DropItemDto> _repeated_dropItemDtos_codec = FieldCodec.ForMessage<DropItemDto>(26U, DropItemDto.Parser);

		private readonly RepeatedField<DropItemDto> dropItemDtos_ = new RepeatedField<DropItemDto>();

		public const int CountFieldNumber = 4;

		private uint count_;

		public const int SmallGuaranteeCountFieldNumber = 5;

		private static readonly MapField<int, int>.Codec _map_smallGuaranteeCount_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForInt32(16U), 42U);

		private readonly MapField<int, int> smallGuaranteeCount_ = new MapField<int, int>();

		public const int BigRewardCountFieldNumber = 6;

		private int bigRewardCount_;

		public const int BigGuaranteeCountFieldNumber = 7;

		private int bigGuaranteeCount_;
	}
}
