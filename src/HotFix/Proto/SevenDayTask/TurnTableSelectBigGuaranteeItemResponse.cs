using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class TurnTableSelectBigGuaranteeItemResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnTableSelectBigGuaranteeItemResponse> Parser
		{
			get
			{
				return TurnTableSelectBigGuaranteeItemResponse._parser;
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
		public int ItemId
		{
			get
			{
				return this.itemId_;
			}
			set
			{
				this.itemId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ItemNum
		{
			get
			{
				return this.itemNum_;
			}
			set
			{
				this.itemNum_ = value;
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
			if (this.ItemId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ItemId);
			}
			if (this.ItemNum != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.ItemNum);
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
			if (this.ItemId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ItemId);
			}
			if (this.ItemNum != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ItemNum);
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
						this.ItemId = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.ItemNum = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TurnTableSelectBigGuaranteeItemResponse> _parser = new MessageParser<TurnTableSelectBigGuaranteeItemResponse>(() => new TurnTableSelectBigGuaranteeItemResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ItemIdFieldNumber = 3;

		private int itemId_;

		public const int ItemNumFieldNumber = 4;

		private uint itemNum_;
	}
}
