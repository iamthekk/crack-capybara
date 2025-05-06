using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class SevenDayTaskActiveRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SevenDayTaskActiveRewardResponse> Parser
		{
			get
			{
				return SevenDayTaskActiveRewardResponse._parser;
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
		public ulong ActiveLog
		{
			get
			{
				return this.activeLog_;
			}
			set
			{
				this.activeLog_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<SevenDayTaskDto> UpdateTaskDto
		{
			get
			{
				return this.updateTaskDto_;
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
			if (this.ActiveLog != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.ActiveLog);
			}
			this.updateTaskDto_.WriteTo(output, SevenDayTaskActiveRewardResponse._repeated_updateTaskDto_codec);
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
			if (this.ActiveLog != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ActiveLog);
			}
			return num + this.updateTaskDto_.CalculateSize(SevenDayTaskActiveRewardResponse._repeated_updateTaskDto_codec);
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
					if (num == 24U)
					{
						this.ActiveLog = input.ReadUInt64();
						continue;
					}
					if (num == 34U)
					{
						this.updateTaskDto_.AddEntriesFrom(input, SevenDayTaskActiveRewardResponse._repeated_updateTaskDto_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<SevenDayTaskActiveRewardResponse> _parser = new MessageParser<SevenDayTaskActiveRewardResponse>(() => new SevenDayTaskActiveRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ActiveLogFieldNumber = 3;

		private ulong activeLog_;

		public const int UpdateTaskDtoFieldNumber = 4;

		private static readonly FieldCodec<SevenDayTaskDto> _repeated_updateTaskDto_codec = FieldCodec.ForMessage<SevenDayTaskDto>(34U, SevenDayTaskDto.Parser);

		private readonly RepeatedField<SevenDayTaskDto> updateTaskDto_ = new RepeatedField<SevenDayTaskDto>();
	}
}
