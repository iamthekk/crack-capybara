using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class DoMiningResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DoMiningResponse> Parser
		{
			get
			{
				return DoMiningResponse._parser;
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
		public RepeatedField<int> Pos
		{
			get
			{
				return this.pos_;
			}
		}

		[DebuggerNonUserCode]
		public MiningInfoDto MiningInfoDto
		{
			get
			{
				return this.miningInfoDto_;
			}
			set
			{
				this.miningInfoDto_ = value;
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
			this.pos_.WriteTo(output, DoMiningResponse._repeated_pos_codec);
			if (this.miningInfoDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.MiningInfoDto);
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
			num += this.pos_.CalculateSize(DoMiningResponse._repeated_pos_codec);
			if (this.miningInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MiningInfoDto);
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
					if (num == 24U || num == 26U)
					{
						this.pos_.AddEntriesFrom(input, DoMiningResponse._repeated_pos_codec);
						continue;
					}
					if (num == 34U)
					{
						if (this.miningInfoDto_ == null)
						{
							this.miningInfoDto_ = new MiningInfoDto();
						}
						input.ReadMessage(this.miningInfoDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<DoMiningResponse> _parser = new MessageParser<DoMiningResponse>(() => new DoMiningResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int PosFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_pos_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> pos_ = new RepeatedField<int>();

		public const int MiningInfoDtoFieldNumber = 4;

		private MiningInfoDto miningInfoDto_;
	}
}
