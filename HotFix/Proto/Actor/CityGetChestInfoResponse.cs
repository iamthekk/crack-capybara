using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class CityGetChestInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityGetChestInfoResponse> Parser
		{
			get
			{
				return CityGetChestInfoResponse._parser;
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
		public RepeatedField<CityChestDto> CityChest
		{
			get
			{
				return this.cityChest_;
			}
		}

		[DebuggerNonUserCode]
		public ulong StartTime
		{
			get
			{
				return this.startTime_;
			}
			set
			{
				this.startTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong RefreshTime
		{
			get
			{
				return this.refreshTime_;
			}
			set
			{
				this.refreshTime_ = value;
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
			this.cityChest_.WriteTo(output, CityGetChestInfoResponse._repeated_cityChest_codec);
			if (this.StartTime != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.StartTime);
			}
			if (this.RefreshTime != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.RefreshTime);
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
			num += this.cityChest_.CalculateSize(CityGetChestInfoResponse._repeated_cityChest_codec);
			if (this.StartTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.StartTime);
			}
			if (this.RefreshTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RefreshTime);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
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
				}
				else
				{
					if (num == 26U)
					{
						this.cityChest_.AddEntriesFrom(input, CityGetChestInfoResponse._repeated_cityChest_codec);
						continue;
					}
					if (num == 32U)
					{
						this.StartTime = input.ReadUInt64();
						continue;
					}
					if (num == 40U)
					{
						this.RefreshTime = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CityGetChestInfoResponse> _parser = new MessageParser<CityGetChestInfoResponse>(() => new CityGetChestInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int CityChestFieldNumber = 3;

		private static readonly FieldCodec<CityChestDto> _repeated_cityChest_codec = FieldCodec.ForMessage<CityChestDto>(26U, CityChestDto.Parser);

		private readonly RepeatedField<CityChestDto> cityChest_ = new RepeatedField<CityChestDto>();

		public const int StartTimeFieldNumber = 4;

		private ulong startTime_;

		public const int RefreshTimeFieldNumber = 5;

		private ulong refreshTime_;
	}
}
