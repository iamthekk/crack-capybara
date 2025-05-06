using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Conquer
{
	public sealed class ConquerListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ConquerListResponse> Parser
		{
			get
			{
				return ConquerListResponse._parser;
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
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ConquerUserDto Owner
		{
			get
			{
				return this.owner_;
			}
			set
			{
				this.owner_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ConquerUserDto Lord
		{
			get
			{
				return this.lord_;
			}
			set
			{
				this.lord_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ConquerUserDto> Slaves
		{
			get
			{
				return this.slaves_;
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
			if (this.UserId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.UserId);
			}
			if (this.owner_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.Owner);
			}
			if (this.lord_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.Lord);
			}
			this.slaves_.WriteTo(output, ConquerListResponse._repeated_slaves_codec);
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
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.owner_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Owner);
			}
			if (this.lord_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Lord);
			}
			return num + this.slaves_.CalculateSize(ConquerListResponse._repeated_slaves_codec);
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
						this.UserId = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						if (this.owner_ == null)
						{
							this.owner_ = new ConquerUserDto();
						}
						input.ReadMessage(this.owner_);
						continue;
					}
					if (num == 42U)
					{
						if (this.lord_ == null)
						{
							this.lord_ = new ConquerUserDto();
						}
						input.ReadMessage(this.lord_);
						continue;
					}
					if (num == 50U)
					{
						this.slaves_.AddEntriesFrom(input, ConquerListResponse._repeated_slaves_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ConquerListResponse> _parser = new MessageParser<ConquerListResponse>(() => new ConquerListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int UserIdFieldNumber = 3;

		private long userId_;

		public const int OwnerFieldNumber = 4;

		private ConquerUserDto owner_;

		public const int LordFieldNumber = 5;

		private ConquerUserDto lord_;

		public const int SlavesFieldNumber = 6;

		private static readonly FieldCodec<ConquerUserDto> _repeated_slaves_codec = FieldCodec.ForMessage<ConquerUserDto>(50U, ConquerUserDto.Parser);

		private readonly RepeatedField<ConquerUserDto> slaves_ = new RepeatedField<ConquerUserDto>();
	}
}
