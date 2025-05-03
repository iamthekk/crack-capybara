using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class MiningDrawDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MiningDrawDto> Parser
		{
			get
			{
				return MiningDrawDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int FreeTimes
		{
			get
			{
				return this.freeTimes_;
			}
			set
			{
				this.freeTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Progress
		{
			get
			{
				return this.progress_;
			}
			set
			{
				this.progress_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Rate
		{
			get
			{
				return this.rate_;
			}
			set
			{
				this.rate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ContinueFreeTimes
		{
			get
			{
				return this.continueFreeTimes_;
			}
			set
			{
				this.continueFreeTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.FreeTimes != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.FreeTimes);
			}
			if (this.Progress != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Progress);
			}
			if (this.Rate != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Rate);
			}
			if (this.ContinueFreeTimes != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.ContinueFreeTimes);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.FreeTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FreeTimes);
			}
			if (this.Progress != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Progress);
			}
			if (this.Rate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Rate);
			}
			if (this.ContinueFreeTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ContinueFreeTimes);
			}
			return num;
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
						this.FreeTimes = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Progress = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Rate = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.ContinueFreeTimes = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<MiningDrawDto> _parser = new MessageParser<MiningDrawDto>(() => new MiningDrawDto());

		public const int FreeTimesFieldNumber = 1;

		private int freeTimes_;

		public const int ProgressFieldNumber = 2;

		private int progress_;

		public const int RateFieldNumber = 3;

		private int rate_;

		public const int ContinueFreeTimesFieldNumber = 4;

		private int continueFreeTimes_;
	}
}
