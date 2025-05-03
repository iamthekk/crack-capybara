using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class OpenBombResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<OpenBombResponse> Parser
		{
			get
			{
				return OpenBombResponse._parser;
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
		public int Pos
		{
			get
			{
				return this.pos_;
			}
			set
			{
				this.pos_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MiningInfoDto MiningInfoDto
		{
			get
			{
				return this.miningInfoDto_;
			}
			set
			{
				this.miningInfoDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GridDto> Grids
		{
			get
			{
				return this.grids_;
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
			if (this.Pos != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Pos);
			}
			if (this.miningInfoDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.MiningInfoDto);
			}
			this.grids_.WriteTo(output, OpenBombResponse._repeated_grids_codec);
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
			if (this.Pos != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Pos);
			}
			if (this.miningInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MiningInfoDto);
			}
			return num + this.grids_.CalculateSize(OpenBombResponse._repeated_grids_codec);
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
						this.Pos = input.ReadInt32();
						continue;
					}
					if (num == 34U)
					{
						if (this.miningInfoDto_ == null)
						{
							this.miningInfoDto_ = new MiningInfoDto();
						}
						input.ReadMessage(this.miningInfoDto_);
						continue;
					}
					if (num == 42U)
					{
						this.grids_.AddEntriesFrom(input, OpenBombResponse._repeated_grids_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<OpenBombResponse> _parser = new MessageParser<OpenBombResponse>(() => new OpenBombResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int PosFieldNumber = 3;

		private int pos_;

		public const int MiningInfoDtoFieldNumber = 4;

		private MiningInfoDto miningInfoDto_;

		public const int GridsFieldNumber = 5;

		private static readonly FieldCodec<GridDto> _repeated_grids_codec = FieldCodec.ForMessage<GridDto>(42U, GridDto.Parser);

		private readonly RepeatedField<GridDto> grids_ = new RepeatedField<GridDto>();
	}
}
