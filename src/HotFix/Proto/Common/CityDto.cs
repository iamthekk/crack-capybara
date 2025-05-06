using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class CityDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityDto> Parser
		{
			get
			{
				return CityDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int GoldmineLevel
		{
			get
			{
				return this.goldmineLevel_;
			}
			set
			{
				this.goldmineLevel_ = value;
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
		public RepeatedField<CityChestDto> CityChest
		{
			get
			{
				return this.cityChest_;
			}
		}

		[DebuggerNonUserCode]
		public ulong StartChestHangTime
		{
			get
			{
				return this.startChestHangTime_;
			}
			set
			{
				this.startChestHangTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong RefreshChestHangTime
		{
			get
			{
				return this.refreshChestHangTime_;
			}
			set
			{
				this.refreshChestHangTime_ = value;
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
		public ulong InitGoldTime
		{
			get
			{
				return this.initGoldTime_;
			}
			set
			{
				this.initGoldTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.GoldmineLevel != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.GoldmineLevel);
			}
			if (this.LastGoldmineRewardTime != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.LastGoldmineRewardTime);
			}
			this.cityChest_.WriteTo(output, CityDto._repeated_cityChest_codec);
			if (this.StartChestHangTime != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.StartChestHangTime);
			}
			if (this.RefreshChestHangTime != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.RefreshChestHangTime);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Score);
			}
			if (this.InitGoldTime != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.InitGoldTime);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.GoldmineLevel != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.GoldmineLevel);
			}
			if (this.LastGoldmineRewardTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.LastGoldmineRewardTime);
			}
			num += this.cityChest_.CalculateSize(CityDto._repeated_cityChest_codec);
			if (this.StartChestHangTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.StartChestHangTime);
			}
			if (this.RefreshChestHangTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RefreshChestHangTime);
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
			}
			if (this.InitGoldTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.InitGoldTime);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 8U)
					{
						this.GoldmineLevel = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.LastGoldmineRewardTime = input.ReadUInt64();
						continue;
					}
					if (num == 26U)
					{
						this.cityChest_.AddEntriesFrom(input, CityDto._repeated_cityChest_codec);
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.StartChestHangTime = input.ReadUInt64();
						continue;
					}
					if (num == 40U)
					{
						this.RefreshChestHangTime = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
					if (num == 56U)
					{
						this.InitGoldTime = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CityDto> _parser = new MessageParser<CityDto>(() => new CityDto());

		public const int GoldmineLevelFieldNumber = 1;

		private int goldmineLevel_;

		public const int LastGoldmineRewardTimeFieldNumber = 2;

		private ulong lastGoldmineRewardTime_;

		public const int CityChestFieldNumber = 3;

		private static readonly FieldCodec<CityChestDto> _repeated_cityChest_codec = FieldCodec.ForMessage<CityChestDto>(26U, CityChestDto.Parser);

		private readonly RepeatedField<CityChestDto> cityChest_ = new RepeatedField<CityChestDto>();

		public const int StartChestHangTimeFieldNumber = 4;

		private ulong startChestHangTime_;

		public const int RefreshChestHangTimeFieldNumber = 5;

		private ulong refreshChestHangTime_;

		public const int ScoreFieldNumber = 6;

		private int score_;

		public const int InitGoldTimeFieldNumber = 7;

		private ulong initGoldTime_;
	}
}
