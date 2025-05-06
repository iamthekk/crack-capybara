using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class ChainActvDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChainActvDto> Parser
		{
			get
			{
				return ChainActvDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return this.id_;
			}
			set
			{
				this.id_ = value;
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
		public ulong EndTime
		{
			get
			{
				return this.endTime_;
			}
			set
			{
				this.endTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> GetRewardIds
		{
			get
			{
				return this.getRewardIds_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Id);
			}
			if (this.StartTime != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.StartTime);
			}
			if (this.EndTime != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.EndTime);
			}
			this.getRewardIds_.WriteTo(output, ChainActvDto._repeated_getRewardIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Id);
			}
			if (this.StartTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.StartTime);
			}
			if (this.EndTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.EndTime);
			}
			return num + this.getRewardIds_.CalculateSize(ChainActvDto._repeated_getRewardIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.Id = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.StartTime = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.EndTime = input.ReadUInt64();
						continue;
					}
					if (num == 32U || num == 34U)
					{
						this.getRewardIds_.AddEntriesFrom(input, ChainActvDto._repeated_getRewardIds_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChainActvDto> _parser = new MessageParser<ChainActvDto>(() => new ChainActvDto());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int StartTimeFieldNumber = 2;

		private ulong startTime_;

		public const int EndTimeFieldNumber = 3;

		private ulong endTime_;

		public const int GetRewardIdsFieldNumber = 4;

		private static readonly FieldCodec<int> _repeated_getRewardIds_codec = FieldCodec.ForInt32(34U);

		private readonly RepeatedField<int> getRewardIds_ = new RepeatedField<int>();
	}
}
