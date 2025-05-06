using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UserCurrency : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserCurrency> Parser
		{
			get
			{
				return UserCurrency._parser;
			}
		}

		[DebuggerNonUserCode]
		public long Coins
		{
			get
			{
				return this.coins_;
			}
			set
			{
				this.coins_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Diamonds
		{
			get
			{
				return this.diamonds_;
			}
			set
			{
				this.diamonds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CardExp
		{
			get
			{
				return this.cardExp_;
			}
			set
			{
				this.cardExp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ChestScore
		{
			get
			{
				return this.chestScore_;
			}
			set
			{
				this.chestScore_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Coins != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.Coins);
			}
			if (this.Diamonds != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Diamonds);
			}
			if (this.CardExp != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.CardExp);
			}
			if (this.ChestScore != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.ChestScore);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Coins != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Coins);
			}
			if (this.Diamonds != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Diamonds);
			}
			if (this.CardExp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CardExp);
			}
			if (this.ChestScore != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChestScore);
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
						this.Coins = input.ReadInt64();
						continue;
					}
					if (num == 16U)
					{
						this.Diamonds = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.CardExp = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.ChestScore = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserCurrency> _parser = new MessageParser<UserCurrency>(() => new UserCurrency());

		public const int CoinsFieldNumber = 1;

		private long coins_;

		public const int DiamondsFieldNumber = 2;

		private int diamonds_;

		public const int CardExpFieldNumber = 3;

		private uint cardExp_;

		public const int ChestScoreFieldNumber = 4;

		private uint chestScore_;
	}
}
