using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class TowerRankIndexResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TowerRankIndexResponse> Parser
		{
			get
			{
				return TowerRankIndexResponse._parser;
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
		public uint Index
		{
			get
			{
				return this.index_;
			}
			set
			{
				this.index_ = value;
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
			if (this.Index != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Index);
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
			if (this.Index != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Index);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Index = input.ReadUInt32();
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

		private static readonly MessageParser<TowerRankIndexResponse> _parser = new MessageParser<TowerRankIndexResponse>(() => new TowerRankIndexResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int IndexFieldNumber = 3;

		private uint index_;
	}
}
