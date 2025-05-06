using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildSearchRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildSearchRequest> Parser
		{
			get
			{
				return GuildSearchRequest._parser;
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
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Value
		{
			get
			{
				return this.value_;
			}
			set
			{
				this.value_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool IsOnlyJoinable
		{
			get
			{
				return this.isOnlyJoinable_;
			}
			set
			{
				this.isOnlyJoinable_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ulong> ExcludeGuildIds
		{
			get
			{
				return this.excludeGuildIds_;
			}
		}

		[DebuggerNonUserCode]
		public uint PageIndex
		{
			get
			{
				return this.pageIndex_;
			}
			set
			{
				this.pageIndex_ = value;
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
			if (this.Type != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Type);
			}
			if (this.Value.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Value);
			}
			if (this.IsOnlyJoinable)
			{
				output.WriteRawTag(32);
				output.WriteBool(this.IsOnlyJoinable);
			}
			this.excludeGuildIds_.WriteTo(output, GuildSearchRequest._repeated_excludeGuildIds_codec);
			if (this.PageIndex != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.PageIndex);
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
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
			}
			if (this.Value.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Value);
			}
			if (this.IsOnlyJoinable)
			{
				num += 2;
			}
			num += this.excludeGuildIds_.CalculateSize(GuildSearchRequest._repeated_excludeGuildIds_codec);
			if (this.PageIndex != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PageIndex);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 26U)
				{
					if (num <= 40U)
					{
						if (num == 32U)
						{
							this.IsOnlyJoinable = input.ReadBool();
							continue;
						}
						if (num != 40U)
						{
							goto IL_0036;
						}
					}
					else if (num != 42U)
					{
						if (num != 48U)
						{
							goto IL_0036;
						}
						this.PageIndex = input.ReadUInt32();
						continue;
					}
					this.excludeGuildIds_.AddEntriesFrom(input, GuildSearchRequest._repeated_excludeGuildIds_codec);
					continue;
				}
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
					this.Type = input.ReadUInt32();
					continue;
				}
				if (num == 26U)
				{
					this.Value = input.ReadString();
					continue;
				}
				IL_0036:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildSearchRequest> _parser = new MessageParser<GuildSearchRequest>(() => new GuildSearchRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int TypeFieldNumber = 2;

		private uint type_;

		public const int ValueFieldNumber = 3;

		private string value_ = "";

		public const int IsOnlyJoinableFieldNumber = 4;

		private bool isOnlyJoinable_;

		public const int ExcludeGuildIdsFieldNumber = 5;

		private static readonly FieldCodec<ulong> _repeated_excludeGuildIds_codec = FieldCodec.ForUInt64(42U);

		private readonly RepeatedField<ulong> excludeGuildIds_ = new RepeatedField<ulong>();

		public const int PageIndexFieldNumber = 6;

		private uint pageIndex_;
	}
}
