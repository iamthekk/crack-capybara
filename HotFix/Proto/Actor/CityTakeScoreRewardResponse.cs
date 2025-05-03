using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class CityTakeScoreRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityTakeScoreRewardResponse> Parser
		{
			get
			{
				return CityTakeScoreRewardResponse._parser;
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
		public ulong RefreshTime
		{
			get
			{
				return this.refreshTime_;
			}
			set
			{
				this.refreshTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
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
			if (this.RefreshTime != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.RefreshTime);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Score);
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
			if (this.RefreshTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RefreshTime);
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
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
					if (num == 24U)
					{
						this.RefreshTime = input.ReadUInt64();
						continue;
					}
					if (num == 32U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CityTakeScoreRewardResponse> _parser = new MessageParser<CityTakeScoreRewardResponse>(() => new CityTakeScoreRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RefreshTimeFieldNumber = 3;

		private ulong refreshTime_;

		public const int ScoreFieldNumber = 4;

		private int score_;
	}
}
