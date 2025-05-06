using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class TowerChallengeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TowerChallengeResponse> Parser
		{
			get
			{
				return TowerChallengeResponse._parser;
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
		public uint ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Result
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
		public int Seed
		{
			get
			{
				return this.seed_;
			}
			set
			{
				this.seed_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CombatUnitDto> StartUnits
		{
			get
			{
				return this.startUnits_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CombatUnitDto> EndUnits
		{
			get
			{
				return this.endUnits_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, ulong> ChapterGiftTime
		{
			get
			{
				return this.chapterGiftTime_;
			}
		}

		[DebuggerNonUserCode]
		public BattleUserDto UserInfo
		{
			get
			{
				return this.userInfo_;
			}
			set
			{
				this.userInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string BattleServerLogId
		{
			get
			{
				return this.battleServerLogId_;
			}
			set
			{
				this.battleServerLogId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string BattleServerLogData
		{
			get
			{
				return this.battleServerLogData_;
			}
			set
			{
				this.battleServerLogData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.Result != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Result);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.Seed);
			}
			this.startUnits_.WriteTo(output, TowerChallengeResponse._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, TowerChallengeResponse._repeated_endUnits_codec);
			this.chapterGiftTime_.WriteTo(output, TowerChallengeResponse._map_chapterGiftTime_codec);
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.UserInfo);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.BattleServerLogData);
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
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.Result != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Result);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(TowerChallengeResponse._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(TowerChallengeResponse._repeated_endUnits_codec);
			num += this.chapterGiftTime_.CalculateSize(TowerChallengeResponse._map_chapterGiftTime_codec);
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogData);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 40U)
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
							this.ConfigId = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Result = input.ReadUInt32();
							continue;
						}
						if (num == 40U)
						{
							this.Seed = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 66U)
				{
					if (num == 50U)
					{
						this.startUnits_.AddEntriesFrom(input, TowerChallengeResponse._repeated_startUnits_codec);
						continue;
					}
					if (num == 58U)
					{
						this.endUnits_.AddEntriesFrom(input, TowerChallengeResponse._repeated_endUnits_codec);
						continue;
					}
					if (num == 66U)
					{
						this.chapterGiftTime_.AddEntriesFrom(input, TowerChallengeResponse._map_chapterGiftTime_codec);
						continue;
					}
				}
				else
				{
					if (num == 74U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new BattleUserDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
					if (num == 82U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
					if (num == 90U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TowerChallengeResponse> _parser = new MessageParser<TowerChallengeResponse>(() => new TowerChallengeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ConfigIdFieldNumber = 3;

		private uint configId_;

		public const int ResultFieldNumber = 4;

		private uint result_;

		public const int SeedFieldNumber = 5;

		private int seed_;

		public const int StartUnitsFieldNumber = 6;

		private static readonly FieldCodec<CombatUnitDto> _repeated_startUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(50U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> startUnits_ = new RepeatedField<CombatUnitDto>();

		public const int EndUnitsFieldNumber = 7;

		private static readonly FieldCodec<CombatUnitDto> _repeated_endUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(58U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> endUnits_ = new RepeatedField<CombatUnitDto>();

		public const int ChapterGiftTimeFieldNumber = 8;

		private static readonly MapField<uint, ulong>.Codec _map_chapterGiftTime_codec = new MapField<uint, ulong>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt64(16U), 66U);

		private readonly MapField<uint, ulong> chapterGiftTime_ = new MapField<uint, ulong>();

		public const int UserInfoFieldNumber = 9;

		private BattleUserDto userInfo_;

		public const int BattleServerLogIdFieldNumber = 10;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 11;

		private string battleServerLogData_ = "";
	}
}
