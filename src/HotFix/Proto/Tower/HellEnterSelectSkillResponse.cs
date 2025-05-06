using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellEnterSelectSkillResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellEnterSelectSkillResponse> Parser
		{
			get
			{
				return HellEnterSelectSkillResponse._parser;
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
			this.roundSkillList_.WriteTo(output, HellEnterSelectSkillResponse._repeated_roundSkillList_codec);
			this.stageSkillMap_.WriteTo(output, HellEnterSelectSkillResponse._map_stageSkillMap_codec);
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
			num += this.roundSkillList_.CalculateSize(HellEnterSelectSkillResponse._repeated_roundSkillList_codec);
			return num + this.stageSkillMap_.CalculateSize(HellEnterSelectSkillResponse._map_stageSkillMap_codec);
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
					if (num == 24U || num == 26U)
					{
						this.roundSkillList_.AddEntriesFrom(input, HellEnterSelectSkillResponse._repeated_roundSkillList_codec);
						continue;
					}
					if (num == 34U)
					{
						this.stageSkillMap_.AddEntriesFrom(input, HellEnterSelectSkillResponse._map_stageSkillMap_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<HellEnterSelectSkillResponse> _parser = new MessageParser<HellEnterSelectSkillResponse>(() => new HellEnterSelectSkillResponse());

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
	}
}
