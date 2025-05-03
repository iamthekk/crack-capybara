using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class ShopActDetailDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopActDetailDto> Parser
		{
			get
			{
				return ShopActDetailDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint ShopActivity
		{
			get
			{
				return this.shopActivity_;
			}
			set
			{
				this.shopActivity_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong StartTime
		{
			get
			{
				return this.startTime_;
			}
			set
			{
				this.startTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong EndTime
		{
			get
			{
				return this.endTime_;
			}
			set
			{
				this.endTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ShopActivityJson
		{
			get
			{
				return this.shopActivityJson_;
			}
			set
			{
				this.shopActivityJson_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string SummonJson
		{
			get
			{
				return this.summonJson_;
			}
			set
			{
				this.summonJson_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> SummonPoolItems
		{
			get
			{
				return this.summonPoolItems_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ShopActivity != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.ShopActivity);
			}
			if (this.StartTime != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.StartTime);
			}
			if (this.EndTime != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.EndTime);
			}
			if (this.ShopActivityJson.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.ShopActivityJson);
			}
			if (this.SummonJson.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.SummonJson);
			}
			this.summonPoolItems_.WriteTo(output, ShopActDetailDto._repeated_summonPoolItems_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ShopActivity != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ShopActivity);
			}
			if (this.StartTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.StartTime);
			}
			if (this.EndTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.EndTime);
			}
			if (this.ShopActivityJson.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ShopActivityJson);
			}
			if (this.SummonJson.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SummonJson);
			}
			return num + this.summonPoolItems_.CalculateSize(ShopActDetailDto._repeated_summonPoolItems_codec);
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
						this.ShopActivity = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.StartTime = input.ReadUInt64();
						continue;
					}
					if (num == 24U)
					{
						this.EndTime = input.ReadUInt64();
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 34U)
					{
						this.ShopActivityJson = input.ReadString();
						continue;
					}
					if (num == 42U)
					{
						this.SummonJson = input.ReadString();
						continue;
					}
				}
				else if (num == 48U || num == 50U)
				{
					this.summonPoolItems_.AddEntriesFrom(input, ShopActDetailDto._repeated_summonPoolItems_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopActDetailDto> _parser = new MessageParser<ShopActDetailDto>(() => new ShopActDetailDto());

		public const int ShopActivityFieldNumber = 1;

		private uint shopActivity_;

		public const int StartTimeFieldNumber = 2;

		private ulong startTime_;

		public const int EndTimeFieldNumber = 3;

		private ulong endTime_;

		public const int ShopActivityJsonFieldNumber = 4;

		private string shopActivityJson_ = "";

		public const int SummonJsonFieldNumber = 5;

		private string summonJson_ = "";

		public const int SummonPoolItemsFieldNumber = 6;

		private static readonly FieldCodec<int> _repeated_summonPoolItems_codec = FieldCodec.ForInt32(50U);

		private readonly RepeatedField<int> summonPoolItems_ = new RepeatedField<int>();
	}
}
