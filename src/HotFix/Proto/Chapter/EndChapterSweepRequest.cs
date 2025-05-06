using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class EndChapterSweepRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EndChapterSweepRequest> Parser
		{
			get
			{
				return EndChapterSweepRequest._parser;
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
		public int ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int WaveIndex
		{
			get
			{
				return this.waveIndex_;
			}
			set
			{
				this.waveIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Result
		{
			get
			{
				return this.result_;
			}
			set
			{
				this.result_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string FightData
		{
			get
			{
				return this.fightData_;
			}
			set
			{
				this.fightData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int AddCoin
		{
			get
			{
				return this.addCoin_;
			}
			set
			{
				this.addCoin_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> ServerRewards
		{
			get
			{
				return this.serverRewards_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> MonsterRewards
		{
			get
			{
				return this.monsterRewards_;
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
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.WaveIndex);
			}
			if (this.Result != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Result);
			}
			if (this.FightData.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.FightData);
			}
			if (this.AddCoin != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.AddCoin);
			}
			this.serverRewards_.WriteTo(output, EndChapterSweepRequest._repeated_serverRewards_codec);
			this.monsterRewards_.WriteTo(output, EndChapterSweepRequest._repeated_monsterRewards_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.WaveIndex);
			}
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			if (this.FightData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.FightData);
			}
			if (this.AddCoin != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AddCoin);
			}
			num += this.serverRewards_.CalculateSize(EndChapterSweepRequest._repeated_serverRewards_codec);
			return num + this.monsterRewards_.CalculateSize(EndChapterSweepRequest._repeated_monsterRewards_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
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
							this.ChapterId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.WaveIndex = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Result = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 42U)
					{
						this.FightData = input.ReadString();
						continue;
					}
					if (num == 48U)
					{
						this.AddCoin = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.serverRewards_.AddEntriesFrom(input, EndChapterSweepRequest._repeated_serverRewards_codec);
						continue;
					}
					if (num == 66U)
					{
						this.monsterRewards_.AddEntriesFrom(input, EndChapterSweepRequest._repeated_monsterRewards_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EndChapterSweepRequest> _parser = new MessageParser<EndChapterSweepRequest>(() => new EndChapterSweepRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ChapterIdFieldNumber = 2;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 3;

		private int waveIndex_;

		public const int ResultFieldNumber = 4;

		private int result_;

		public const int FightDataFieldNumber = 5;

		private string fightData_ = "";

		public const int AddCoinFieldNumber = 6;

		private int addCoin_;

		public const int ServerRewardsFieldNumber = 7;

		private static readonly FieldCodec<RewardDto> _repeated_serverRewards_codec = FieldCodec.ForMessage<RewardDto>(58U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> serverRewards_ = new RepeatedField<RewardDto>();

		public const int MonsterRewardsFieldNumber = 8;

		private static readonly FieldCodec<RewardDto> _repeated_monsterRewards_codec = FieldCodec.ForMessage<RewardDto>(66U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> monsterRewards_ = new RepeatedField<RewardDto>();
	}
}
