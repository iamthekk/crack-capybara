using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.CrossArena
{
	public sealed class CrossArenaChallengeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaChallengeResponse> Parser
		{
			get
			{
				return CrossArenaChallengeResponse._parser;
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
		public PVPRecordDto Record
		{
			get
			{
				return this.record_;
			}
			set
			{
				this.record_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TargetRank
		{
			get
			{
				return this.targetRank_;
			}
			set
			{
				this.targetRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TargetAfterRank
		{
			get
			{
				return this.targetAfterRank_;
			}
			set
			{
				this.targetAfterRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TargetAfterScore
		{
			get
			{
				return this.targetAfterScore_;
			}
			set
			{
				this.targetAfterScore_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint OwnerAfterScore
		{
			get
			{
				return this.ownerAfterScore_;
			}
			set
			{
				this.ownerAfterScore_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint OwnerAfterRank
		{
			get
			{
				return this.ownerAfterRank_;
			}
			set
			{
				this.ownerAfterRank_ = value;
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
			if (this.record_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.Record);
			}
			if (this.TargetRank != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.TargetRank);
			}
			if (this.TargetAfterRank != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.TargetAfterRank);
			}
			if (this.TargetAfterScore != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.TargetAfterScore);
			}
			if (this.OwnerAfterScore != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.OwnerAfterScore);
			}
			if (this.OwnerAfterRank != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.OwnerAfterRank);
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
			if (this.record_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Record);
			}
			if (this.TargetRank != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TargetRank);
			}
			if (this.TargetAfterRank != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TargetAfterRank);
			}
			if (this.TargetAfterScore != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TargetAfterScore);
			}
			if (this.OwnerAfterScore != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.OwnerAfterScore);
			}
			if (this.OwnerAfterRank != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.OwnerAfterRank);
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
				if (num <= 48U)
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
						if (num == 34U)
						{
							if (this.record_ == null)
							{
								this.record_ = new PVPRecordDto();
							}
							input.ReadMessage(this.record_);
							continue;
						}
						if (num == 40U)
						{
							this.TargetRank = input.ReadUInt32();
							continue;
						}
						if (num == 48U)
						{
							this.TargetAfterRank = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 56U)
					{
						this.TargetAfterScore = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.OwnerAfterScore = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.OwnerAfterRank = input.ReadUInt32();
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

		private static readonly MessageParser<CrossArenaChallengeResponse> _parser = new MessageParser<CrossArenaChallengeResponse>(() => new CrossArenaChallengeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RecordFieldNumber = 4;

		private PVPRecordDto record_;

		public const int TargetRankFieldNumber = 5;

		private uint targetRank_;

		public const int TargetAfterRankFieldNumber = 6;

		private uint targetAfterRank_;

		public const int TargetAfterScoreFieldNumber = 7;

		private uint targetAfterScore_;

		public const int OwnerAfterScoreFieldNumber = 8;

		private uint ownerAfterScore_;

		public const int OwnerAfterRankFieldNumber = 9;

		private uint ownerAfterRank_;

		public const int BattleServerLogIdFieldNumber = 10;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 11;

		private string battleServerLogData_ = "";
	}
}
