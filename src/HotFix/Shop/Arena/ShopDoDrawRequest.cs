using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopDoDrawRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopDoDrawRequest> Parser
		{
			get
			{
				return ShopDoDrawRequest._parser;
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
		public uint SummonId
		{
			get
			{
				return this.summonId_;
			}
			set
			{
				this.summonId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CostType
		{
			get
			{
				return this.costType_;
			}
			set
			{
				this.costType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint DrawType
		{
			get
			{
				return this.drawType_;
			}
			set
			{
				this.drawType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.SummonId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.SummonId);
			}
			if (this.CostType != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.CostType);
			}
			if (this.DrawType != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.DrawType);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.SummonId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SummonId);
			}
			if (this.CostType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CostType);
			}
			if (this.DrawType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DrawType);
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
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.SummonId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.CostType = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.DrawType = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopDoDrawRequest> _parser = new MessageParser<ShopDoDrawRequest>(() => new ShopDoDrawRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int SummonIdFieldNumber = 2;

		private uint summonId_;

		public const int CostTypeFieldNumber = 3;

		private uint costType_;

		public const int DrawTypeFieldNumber = 4;

		private uint drawType_;
	}
}
