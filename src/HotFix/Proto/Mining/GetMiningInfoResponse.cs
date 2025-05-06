using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class GetMiningInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GetMiningInfoResponse> Parser
		{
			get
			{
				return GetMiningInfoResponse._parser;
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
		public MiningDrawDto MiningDrawDto
		{
			get
			{
				return this.miningDrawDto_;
			}
			set
			{
				this.miningDrawDto_ = value;
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
			if (this.miningInfoDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.MiningInfoDto);
			}
			if (this.miningDrawDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.MiningDrawDto);
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
			if (this.miningInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MiningInfoDto);
			}
			if (this.miningDrawDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MiningDrawDto);
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
					if (num == 26U)
					{
						if (this.miningInfoDto_ == null)
						{
							this.miningInfoDto_ = new MiningInfoDto();
						}
						input.ReadMessage(this.miningInfoDto_);
						continue;
					}
					if (num == 34U)
					{
						if (this.miningDrawDto_ == null)
						{
							this.miningDrawDto_ = new MiningDrawDto();
						}
						input.ReadMessage(this.miningDrawDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GetMiningInfoResponse> _parser = new MessageParser<GetMiningInfoResponse>(() => new GetMiningInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int MiningInfoDtoFieldNumber = 3;

		private MiningInfoDto miningInfoDto_;

		public const int MiningDrawDtoFieldNumber = 4;

		private MiningDrawDto miningDrawDto_;
	}
}
