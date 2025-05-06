using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class CityGoldmineHangRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityGoldmineHangRewardResponse> Parser
		{
			get
			{
				return CityGoldmineHangRewardResponse._parser;
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
		public ulong LastGoldmineRewardTime
		{
			get
			{
				return this.lastGoldmineRewardTime_;
			}
			set
			{
				this.lastGoldmineRewardTime_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.LastGoldmineRewardTime != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.LastGoldmineRewardTime);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.CommonData);
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
			if (this.LastGoldmineRewardTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.LastGoldmineRewardTime);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
						}
					}
					else
					{
						this.LastGoldmineRewardTime = input.ReadUInt64();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<CityGoldmineHangRewardResponse> _parser = new MessageParser<CityGoldmineHangRewardResponse>(() => new CityGoldmineHangRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int LastGoldmineRewardTimeFieldNumber = 2;

		private ulong lastGoldmineRewardTime_;

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
