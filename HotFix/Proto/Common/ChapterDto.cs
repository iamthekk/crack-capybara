using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class ChapterDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterDto> Parser
		{
			get
			{
				return ChapterDto._parser;
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
		public RepeatedField<int> CanRewardList
		{
			get
			{
				return this.canRewardList_;
			}
		}

		[DebuggerNonUserCode]
		public string BattleKey
		{
			get
			{
				return this.battleKey_;
			}
			set
			{
				this.battleKey_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int BattleTimes
		{
			get
			{
				return this.battleTimes_;
			}
			set
			{
				this.battleTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.WaveIndex);
			}
			this.canRewardList_.WriteTo(output, ChapterDto._repeated_canRewardList_codec);
			if (this.BattleKey.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.BattleKey);
			}
			if (this.BattleTimes != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.BattleTimes);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.WaveIndex);
			}
			num += this.canRewardList_.CalculateSize(ChapterDto._repeated_canRewardList_codec);
			if (this.BattleKey.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleKey);
			}
			if (this.BattleTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BattleTimes);
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
						this.ChapterId = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.WaveIndex = input.ReadInt32();
						continue;
					}
					if (num == 24U)
					{
						goto IL_004A;
					}
				}
				else
				{
					if (num == 26U)
					{
						goto IL_004A;
					}
					if (num == 34U)
					{
						this.BattleKey = input.ReadString();
						continue;
					}
					if (num == 40U)
					{
						this.BattleTimes = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_004A:
				this.canRewardList_.AddEntriesFrom(input, ChapterDto._repeated_canRewardList_codec);
			}
		}

		private static readonly MessageParser<ChapterDto> _parser = new MessageParser<ChapterDto>(() => new ChapterDto());

		public const int ChapterIdFieldNumber = 1;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 2;

		private int waveIndex_;

		public const int CanRewardListFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_canRewardList_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> canRewardList_ = new RepeatedField<int>();

		public const int BattleKeyFieldNumber = 4;

		private string battleKey_ = "";

		public const int BattleTimesFieldNumber = 5;

		private int battleTimes_;
	}
}
