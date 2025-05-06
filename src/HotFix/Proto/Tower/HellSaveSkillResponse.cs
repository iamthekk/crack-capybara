using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellSaveSkillResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellSaveSkillResponse> Parser
		{
			get
			{
				return HellSaveSkillResponse._parser;
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
		public RepeatedField<int> RoundSkillList
		{
			get
			{
				return this.roundSkillList_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, SkillListDto> StageSkillMap
		{
			get
			{
				return this.stageSkillMap_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, int> AttrMap
		{
			get
			{
				return this.attrMap_;
			}
		}

		[DebuggerNonUserCode]
		public long Hp
		{
			get
			{
				return this.hp_;
			}
			set
			{
				this.hp_ = value;
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
			this.roundSkillList_.WriteTo(output, HellSaveSkillResponse._repeated_roundSkillList_codec);
			this.stageSkillMap_.WriteTo(output, HellSaveSkillResponse._map_stageSkillMap_codec);
			this.attrMap_.WriteTo(output, HellSaveSkillResponse._map_attrMap_codec);
			if (this.Hp != 0L)
			{
				output.WriteRawTag(48);
				output.WriteInt64(this.Hp);
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
			num += this.roundSkillList_.CalculateSize(HellSaveSkillResponse._repeated_roundSkillList_codec);
			num += this.stageSkillMap_.CalculateSize(HellSaveSkillResponse._map_stageSkillMap_codec);
			num += this.attrMap_.CalculateSize(HellSaveSkillResponse._map_attrMap_codec);
			if (this.Hp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Hp);
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
						goto IL_006C;
					}
				}
				else if (num <= 34U)
				{
					if (num == 26U)
					{
						goto IL_006C;
					}
					if (num == 34U)
					{
						this.stageSkillMap_.AddEntriesFrom(input, HellSaveSkillResponse._map_stageSkillMap_codec);
						continue;
					}
				}
				else
				{
					if (num == 42U)
					{
						this.attrMap_.AddEntriesFrom(input, HellSaveSkillResponse._map_attrMap_codec);
						continue;
					}
					if (num == 48U)
					{
						this.Hp = input.ReadInt64();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_006C:
				this.roundSkillList_.AddEntriesFrom(input, HellSaveSkillResponse._repeated_roundSkillList_codec);
			}
		}

		private static readonly MessageParser<HellSaveSkillResponse> _parser = new MessageParser<HellSaveSkillResponse>(() => new HellSaveSkillResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RoundSkillListFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_roundSkillList_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> roundSkillList_ = new RepeatedField<int>();

		public const int StageSkillMapFieldNumber = 4;

		private static readonly MapField<int, SkillListDto>.Codec _map_stageSkillMap_codec = new MapField<int, SkillListDto>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForMessage<SkillListDto>(18U, SkillListDto.Parser), 34U);

		private readonly MapField<int, SkillListDto> stageSkillMap_ = new MapField<int, SkillListDto>();

		public const int AttrMapFieldNumber = 5;

		private static readonly MapField<string, int>.Codec _map_attrMap_codec = new MapField<string, int>.Codec(FieldCodec.ForString(10U), FieldCodec.ForInt32(16U), 42U);

		private readonly MapField<string, int> attrMap_ = new MapField<string, int>();

		public const int HpFieldNumber = 6;

		private long hp_;
	}
}
