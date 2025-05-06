using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pet
{
	public sealed class PetTrainRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetTrainRequest> Parser
		{
			get
			{
				return PetTrainRequest._parser;
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
		public ulong RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> LockIndex
		{
			get
			{
				return this.lockIndex_;
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
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.RowId);
			}
			this.lockIndex_.WriteTo(output, PetTrainRequest._repeated_lockIndex_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			return num + this.lockIndex_.CalculateSize(PetTrainRequest._repeated_lockIndex_codec);
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
						this.RowId = input.ReadUInt64();
						continue;
					}
				}
				else if (num == 24U || num == 26U)
				{
					this.lockIndex_.AddEntriesFrom(input, PetTrainRequest._repeated_lockIndex_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PetTrainRequest> _parser = new MessageParser<PetTrainRequest>(() => new PetTrainRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RowIdFieldNumber = 2;

		private ulong rowId_;

		public const int LockIndexFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_lockIndex_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> lockIndex_ = new RepeatedField<int>();
	}
}
