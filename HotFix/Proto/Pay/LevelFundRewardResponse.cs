using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class LevelFundRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<LevelFundRewardResponse> Parser
		{
			get
			{
				return LevelFundRewardResponse._parser;
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
		public MapField<uint, IntegerArray> LevelFundReward
		{
			get
			{
				return this.levelFundReward_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, IntegerArray> FreeLevelFundReward
		{
			get
			{
				return this.freeLevelFundReward_;
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
			this.levelFundReward_.WriteTo(output, LevelFundRewardResponse._map_levelFundReward_codec);
			this.freeLevelFundReward_.WriteTo(output, LevelFundRewardResponse._map_freeLevelFundReward_codec);
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
			num += this.levelFundReward_.CalculateSize(LevelFundRewardResponse._map_levelFundReward_codec);
			return num + this.freeLevelFundReward_.CalculateSize(LevelFundRewardResponse._map_freeLevelFundReward_codec);
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
						this.levelFundReward_.AddEntriesFrom(input, LevelFundRewardResponse._map_levelFundReward_codec);
						continue;
					}
					if (num == 34U)
					{
						this.freeLevelFundReward_.AddEntriesFrom(input, LevelFundRewardResponse._map_freeLevelFundReward_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<LevelFundRewardResponse> _parser = new MessageParser<LevelFundRewardResponse>(() => new LevelFundRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int LevelFundRewardFieldNumber = 3;

		private static readonly MapField<uint, IntegerArray>.Codec _map_levelFundReward_codec = new MapField<uint, IntegerArray>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<IntegerArray>(18U, IntegerArray.Parser), 26U);

		private readonly MapField<uint, IntegerArray> levelFundReward_ = new MapField<uint, IntegerArray>();

		public const int FreeLevelFundRewardFieldNumber = 4;

		private static readonly MapField<uint, IntegerArray>.Codec _map_freeLevelFundReward_codec = new MapField<uint, IntegerArray>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<IntegerArray>(18U, IntegerArray.Parser), 34U);

		private readonly MapField<uint, IntegerArray> freeLevelFundReward_ = new MapField<uint, IntegerArray>();
	}
}
