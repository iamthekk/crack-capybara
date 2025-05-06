using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossBuyCntResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossBuyCntResponse> Parser
		{
			get
			{
				return GuildBossBuyCntResponse._parser;
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
		public uint ChallengeCnt
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
		public uint BuyCntByDiamonds
		{
			get
			{
				return this.buyCntByDiamonds_;
			}
			set
			{
				this.buyCntByDiamonds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BuyCntCostByDiamonds
		{
			get
			{
				return this.buyCntCostByDiamonds_;
			}
			set
			{
				this.buyCntCostByDiamonds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BuyCntByCoins
		{
			get
			{
				return this.buyCntByCoins_;
			}
			set
			{
				this.buyCntByCoins_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BuyCntCostByCoins
		{
			get
			{
				return this.buyCntCostByCoins_;
			}
			set
			{
				this.buyCntCostByCoins_ = value;
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
			if (this.ChallengeCnt != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ChallengeCnt);
			}
			if (this.BuyCntByDiamonds != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.BuyCntByDiamonds);
			}
			if (this.BuyCntCostByDiamonds != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.BuyCntCostByDiamonds);
			}
			if (this.BuyCntByCoins != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.BuyCntByCoins);
			}
			if (this.BuyCntCostByCoins != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.BuyCntCostByCoins);
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
			if (this.ChallengeCnt != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChallengeCnt);
			}
			if (this.BuyCntByDiamonds != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntByDiamonds);
			}
			if (this.BuyCntCostByDiamonds != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntCostByDiamonds);
			}
			if (this.BuyCntByCoins != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntByCoins);
			}
			if (this.BuyCntCostByCoins != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntCostByCoins);
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
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 24U)
					{
						this.ChallengeCnt = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.BuyCntByDiamonds = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.BuyCntCostByDiamonds = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.BuyCntByCoins = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.BuyCntCostByCoins = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildBossBuyCntResponse> _parser = new MessageParser<GuildBossBuyCntResponse>(() => new GuildBossBuyCntResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChallengeCntFieldNumber = 3;

		private uint challengeCnt_;

		public const int BuyCntByDiamondsFieldNumber = 4;

		private uint buyCntByDiamonds_;

		public const int BuyCntCostByDiamondsFieldNumber = 5;

		private uint buyCntCostByDiamonds_;

		public const int BuyCntByCoinsFieldNumber = 6;

		private uint buyCntByCoins_;

		public const int BuyCntCostByCoinsFieldNumber = 7;

		private uint buyCntCostByCoins_;
	}
}
