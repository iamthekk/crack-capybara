using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildDonationDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildDonationDto> Parser
		{
			get
			{
				return GuildDonationDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public ulong RequestItemTimestamp
		{
			get
			{
				return this.requestItemTimestamp_;
			}
			set
			{
				this.requestItemTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint DonationItemCount
		{
			get
			{
				return this.donationItemCount_;
			}
			set
			{
				this.donationItemCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint DonationMaxItemCount
		{
			get
			{
				return this.donationMaxItemCount_;
			}
			set
			{
				this.donationMaxItemCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RequestItemTimestamp != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.RequestItemTimestamp);
			}
			if (this.DonationItemCount != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.DonationItemCount);
			}
			if (this.DonationMaxItemCount != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.DonationMaxItemCount);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RequestItemTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RequestItemTimestamp);
			}
			if (this.DonationItemCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DonationItemCount);
			}
			if (this.DonationMaxItemCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DonationMaxItemCount);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.DonationMaxItemCount = input.ReadUInt32();
						}
					}
					else
					{
						this.DonationItemCount = input.ReadUInt32();
					}
				}
				else
				{
					this.RequestItemTimestamp = input.ReadUInt64();
				}
			}
		}

		private static readonly MessageParser<GuildDonationDto> _parser = new MessageParser<GuildDonationDto>(() => new GuildDonationDto());

		public const int RequestItemTimestampFieldNumber = 1;

		private ulong requestItemTimestamp_;

		public const int DonationItemCountFieldNumber = 2;

		private uint donationItemCount_;

		public const int DonationMaxItemCountFieldNumber = 3;

		private uint donationMaxItemCount_;
	}
}
