using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class TalentsInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentsInfo> Parser
		{
			get
			{
				return TalentsInfo._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Step
		{
			get
			{
				return this.step_;
			}
			set
			{
				this.step_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, uint> AttributesMap
		{
			get
			{
				return this.attributesMap_;
			}
		}

		[DebuggerNonUserCode]
		public uint ExpProcess
		{
			get
			{
				return this.expProcess_;
			}
			set
			{
				this.expProcess_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Step != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Step);
			}
			this.attributesMap_.WriteTo(output, TalentsInfo._map_attributesMap_codec);
			if (this.ExpProcess != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ExpProcess);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Step != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Step);
			}
			num += this.attributesMap_.CalculateSize(TalentsInfo._map_attributesMap_codec);
			if (this.ExpProcess != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ExpProcess);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.ExpProcess = input.ReadUInt32();
						}
					}
					else
					{
						this.attributesMap_.AddEntriesFrom(input, TalentsInfo._map_attributesMap_codec);
					}
				}
				else
				{
					this.Step = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<TalentsInfo> _parser = new MessageParser<TalentsInfo>(() => new TalentsInfo());

		public const int StepFieldNumber = 1;

		private uint step_;

		public const int AttributesMapFieldNumber = 2;

		private static readonly MapField<string, uint>.Codec _map_attributesMap_codec = new MapField<string, uint>.Codec(FieldCodec.ForString(10U), FieldCodec.ForUInt32(16U), 18U);

		private readonly MapField<string, uint> attributesMap_ = new MapField<string, uint>();

		public const int ExpProcessFieldNumber = 3;

		private uint expProcess_;
	}
}
