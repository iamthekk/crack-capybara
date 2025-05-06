using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Battle
{
	public sealed class RPlayerDetailAttributeResp : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RPlayerDetailAttributeResp> Parser
		{
			get
			{
				return RPlayerDetailAttributeResp._parser;
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
		public long Power
		{
			get
			{
				return this.power_;
			}
			set
			{
				this.power_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, double> Attributes
		{
			get
			{
				return this.attributes_;
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
			if (this.Power != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.Power);
			}
			this.attributes_.WriteTo(output, RPlayerDetailAttributeResp._map_attributes_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			return num + this.attributes_.CalculateSize(RPlayerDetailAttributeResp._map_attributes_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.attributes_.AddEntriesFrom(input, RPlayerDetailAttributeResp._map_attributes_codec);
						}
					}
					else
					{
						this.Power = input.ReadInt64();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<RPlayerDetailAttributeResp> _parser = new MessageParser<RPlayerDetailAttributeResp>(() => new RPlayerDetailAttributeResp());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PowerFieldNumber = 2;

		private long power_;

		public const int AttributesFieldNumber = 3;

		private static readonly MapField<int, double>.Codec _map_attributes_codec = new MapField<int, double>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForDouble(17U), 26U);

		private readonly MapField<int, double> attributes_ = new MapField<int, double>();
	}
}
