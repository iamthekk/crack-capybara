using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class DoMiningRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DoMiningRequest> Parser
		{
			get
			{
				return DoMiningRequest._parser;
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
		public ulong OptType
		{
			get
			{
				return this.optType_;
			}
			set
			{
				this.optType_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.OptType != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.OptType);
			}
			this.pos_.WriteTo(output, DoMiningRequest._repeated_pos_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.OptType != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.OptType);
			}
			return num + this.pos_.CalculateSize(DoMiningRequest._repeated_pos_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.OptType = input.ReadUInt64();
						continue;
					}
				}
				else if (num == 24U || num == 26U)
				{
					this.pos_.AddEntriesFrom(input, DoMiningRequest._repeated_pos_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<DoMiningRequest> _parser = new MessageParser<DoMiningRequest>(() => new DoMiningRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int OptTypeFieldNumber = 2;

		private ulong optType_;

		public const int PosFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_pos_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> pos_ = new RepeatedField<int>();
	}
}
