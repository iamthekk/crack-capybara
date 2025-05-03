using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class TalentInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentInfo> Parser
		{
			get
			{
				return TalentInfo._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint RoleStep
		{
			get
			{
				return this.roleStep_;
			}
			set
			{
				this.roleStep_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TalentExp
		{
			get
			{
				return this.talentExp_;
			}
			set
			{
				this.talentExp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> AttributeMap
		{
			get
			{
				return this.attributeMap_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> TalentMap
		{
			get
			{
				return this.talentMap_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoleStep != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.RoleStep);
			}
			if (this.TalentExp != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.TalentExp);
			}
			this.attributeMap_.WriteTo(output, TalentInfo._map_attributeMap_codec);
			this.talentMap_.WriteTo(output, TalentInfo._map_talentMap_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoleStep != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RoleStep);
			}
			if (this.TalentExp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TalentExp);
			}
			num += this.attributeMap_.CalculateSize(TalentInfo._map_attributeMap_codec);
			return num + this.talentMap_.CalculateSize(TalentInfo._map_talentMap_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.RoleStep = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.TalentExp = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.attributeMap_.AddEntriesFrom(input, TalentInfo._map_attributeMap_codec);
						continue;
					}
					if (num == 34U)
					{
						this.talentMap_.AddEntriesFrom(input, TalentInfo._map_talentMap_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TalentInfo> _parser = new MessageParser<TalentInfo>(() => new TalentInfo());

		public const int RoleStepFieldNumber = 1;

		private uint roleStep_;

		public const int TalentExpFieldNumber = 2;

		private uint talentExp_;

		public const int AttributeMapFieldNumber = 3;

		private static readonly MapField<uint, uint>.Codec _map_attributeMap_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 26U);

		private readonly MapField<uint, uint> attributeMap_ = new MapField<uint, uint>();

		public const int TalentMapFieldNumber = 4;

		private static readonly MapField<uint, uint>.Codec _map_talentMap_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 34U);

		private readonly MapField<uint, uint> talentMap_ = new MapField<uint, uint>();
	}
}
