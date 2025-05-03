using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellSaveSkillRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellSaveSkillRequest> Parser
		{
			get
			{
				return HellSaveSkillRequest._parser;
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
		public RepeatedField<int> SkillList
		{
			get
			{
				return this.skillList_;
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
		public int OptType
		{
			get
			{
				return this.optType_;
			}
			set
			{
				this.optType_ = value;
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
			this.skillList_.WriteTo(output, HellSaveSkillRequest._repeated_skillList_codec);
			this.attrMap_.WriteTo(output, HellSaveSkillRequest._map_attrMap_codec);
			if (this.Hp != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.Hp);
			}
			if (this.OptType != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.OptType);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			num += this.skillList_.CalculateSize(HellSaveSkillRequest._repeated_skillList_codec);
			num += this.attrMap_.CalculateSize(HellSaveSkillRequest._map_attrMap_codec);
			if (this.Hp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Hp);
			}
			if (this.OptType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OptType);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
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
					if (num == 16U || num == 18U)
					{
						this.skillList_.AddEntriesFrom(input, HellSaveSkillRequest._repeated_skillList_codec);
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.attrMap_.AddEntriesFrom(input, HellSaveSkillRequest._map_attrMap_codec);
						continue;
					}
					if (num == 32U)
					{
						this.Hp = input.ReadInt64();
						continue;
					}
					if (num == 40U)
					{
						this.OptType = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<HellSaveSkillRequest> _parser = new MessageParser<HellSaveSkillRequest>(() => new HellSaveSkillRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int SkillListFieldNumber = 2;

		private static readonly FieldCodec<int> _repeated_skillList_codec = FieldCodec.ForInt32(18U);

		private readonly RepeatedField<int> skillList_ = new RepeatedField<int>();

		public const int AttrMapFieldNumber = 3;

		private static readonly MapField<string, int>.Codec _map_attrMap_codec = new MapField<string, int>.Codec(FieldCodec.ForString(10U), FieldCodec.ForInt32(16U), 26U);

		private readonly MapField<string, int> attrMap_ = new MapField<string, int>();

		public const int HpFieldNumber = 4;

		private long hp_;

		public const int OptTypeFieldNumber = 5;

		private int optType_;
	}
}
