using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Collection
{
	public sealed class ChestRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChestRewardResponse> Parser
		{
			get
			{
				return ChestRewardResponse._parser;
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
		public ChestInfo ChestInfo
		{
			get
			{
				return this.chestInfo_;
			}
			set
			{
				this.chestInfo_ = value;
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
			if (this.chestInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.ChestInfo);
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
			if (this.chestInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ChestInfo);
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
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.chestInfo_ == null)
							{
								this.chestInfo_ = new ChestInfo();
							}
							input.ReadMessage(this.chestInfo_);
						}
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
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ChestRewardResponse> _parser = new MessageParser<ChestRewardResponse>(() => new ChestRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChestInfoFieldNumber = 3;

		private ChestInfo chestInfo_;
	}
}
