using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class InteractDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<InteractDto> Parser
		{
			get
			{
				return InteractDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long RowId
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
		public long Id
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
		public long Time
		{
			get
			{
				return this.time_;
			}
			set
			{
				this.time_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool Status
		{
			get
			{
				return this.status_;
			}
			set
			{
				this.status_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<string> Params
		{
			get
			{
				return this.params_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.RowId);
			}
			if (this.Id != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.Id);
			}
			if (this.Time != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.Time);
			}
			if (this.Status)
			{
				output.WriteRawTag(32);
				output.WriteBool(this.Status);
			}
			this.params_.WriteTo(output, InteractDto._repeated_params_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RowId);
			}
			if (this.Id != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Id);
			}
			if (this.Time != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Time);
			}
			if (this.Status)
			{
				num += 2;
			}
			return num + this.params_.CalculateSize(InteractDto._repeated_params_codec);
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
						this.RowId = input.ReadInt64();
						continue;
					}
					if (num == 16U)
					{
						this.Id = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Time = input.ReadInt64();
						continue;
					}
					if (num == 32U)
					{
						this.Status = input.ReadBool();
						continue;
					}
					if (num == 42U)
					{
						this.params_.AddEntriesFrom(input, InteractDto._repeated_params_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<InteractDto> _parser = new MessageParser<InteractDto>(() => new InteractDto());

		public const int RowIdFieldNumber = 1;

		private long rowId_;

		public const int IdFieldNumber = 2;

		private long id_;

		public const int TimeFieldNumber = 3;

		private long time_;

		public const int StatusFieldNumber = 4;

		private bool status_;

		public const int ParamsFieldNumber = 5;

		private static readonly FieldCodec<string> _repeated_params_codec = FieldCodec.ForString(42U);

		private readonly RepeatedField<string> params_ = new RepeatedField<string>();
	}
}
