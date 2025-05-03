using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildDonationGetOperationRecordsResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildDonationGetOperationRecordsResponse> Parser
		{
			get
			{
				return GuildDonationGetOperationRecordsResponse._parser;
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
		public RepeatedField<string> Records
		{
			get
			{
				return this.records_;
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
			this.records_.WriteTo(output, GuildDonationGetOperationRecordsResponse._repeated_records_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.records_.CalculateSize(GuildDonationGetOperationRecordsResponse._repeated_records_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.records_.AddEntriesFrom(input, GuildDonationGetOperationRecordsResponse._repeated_records_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildDonationGetOperationRecordsResponse> _parser = new MessageParser<GuildDonationGetOperationRecordsResponse>(() => new GuildDonationGetOperationRecordsResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int RecordsFieldNumber = 2;

		private static readonly FieldCodec<string> _repeated_records_codec = FieldCodec.ForString(18U);

		private readonly RepeatedField<string> records_ = new RepeatedField<string>();
	}
}
