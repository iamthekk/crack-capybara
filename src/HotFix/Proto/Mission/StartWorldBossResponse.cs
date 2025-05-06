using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class StartWorldBossResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<StartWorldBossResponse> Parser
		{
			get
			{
				return StartWorldBossResponse._parser;
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
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Seed
		{
			get
			{
				return this.seed_;
			}
			set
			{
				this.seed_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int FreeCount
		{
			get
			{
				return this.freeCount_;
			}
			set
			{
				this.freeCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ChallengeCnt
		{
			get
			{
				return this.challengeCnt_;
			}
			set
			{
				this.challengeCnt_ = value;
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
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonParams);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ConfigId);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Seed);
			}
			if (this.FreeCount != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.FreeCount);
			}
			if (this.ChallengeCnt != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.ChallengeCnt);
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
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			if (this.FreeCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FreeCount);
			}
			if (this.ChallengeCnt != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChallengeCnt);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 24U)
					{
						this.ConfigId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.Seed = input.ReadInt32();
						continue;
					}
					if (num == 40U)
					{
						this.FreeCount = input.ReadInt32();
						continue;
					}
					if (num == 48U)
					{
						this.ChallengeCnt = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<StartWorldBossResponse> _parser = new MessageParser<StartWorldBossResponse>(() => new StartWorldBossResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonParamsFieldNumber = 2;

		private CommonParams commonParams_;

		public const int ConfigIdFieldNumber = 3;

		private int configId_;

		public const int SeedFieldNumber = 4;

		private int seed_;

		public const int FreeCountFieldNumber = 5;

		private int freeCount_;

		public const int ChallengeCntFieldNumber = 6;

		private int challengeCnt_;
	}
}
