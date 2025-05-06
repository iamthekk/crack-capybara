using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildShopRefreshResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildShopRefreshResponse> Parser
		{
			get
			{
				return GuildShopRefreshResponse._parser;
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
		public RepeatedField<GuildShopDto> ShopDto
		{
			get
			{
				return this.shopDto_;
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
			if (this.Type != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Type);
			}
			this.shopDto_.WriteTo(output, GuildShopRefreshResponse._repeated_shopDto_codec);
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
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
			}
			return num + this.shopDto_.CalculateSize(GuildShopRefreshResponse._repeated_shopDto_codec);
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
					if (num == 24U)
					{
						this.Type = input.ReadUInt32();
						continue;
					}
					if (num == 34U)
					{
						this.shopDto_.AddEntriesFrom(input, GuildShopRefreshResponse._repeated_shopDto_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildShopRefreshResponse> _parser = new MessageParser<GuildShopRefreshResponse>(() => new GuildShopRefreshResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int TypeFieldNumber = 3;

		private uint type_;

		public const int ShopDtoFieldNumber = 4;

		private static readonly FieldCodec<GuildShopDto> _repeated_shopDto_codec = FieldCodec.ForMessage<GuildShopDto>(34U, GuildShopDto.Parser);

		private readonly RepeatedField<GuildShopDto> shopDto_ = new RepeatedField<GuildShopDto>();
	}
}
