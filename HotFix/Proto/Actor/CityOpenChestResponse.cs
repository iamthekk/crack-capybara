using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class CityOpenChestResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityOpenChestResponse> Parser
		{
			get
			{
				return CityOpenChestResponse._parser;
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
		public RepeatedField<ulong> RowId
		{
			get
			{
				return this.rowId_;
			}
		}

		[DebuggerNonUserCode]
		public ulong RefreshTime
		{
			get
			{
				return this.refreshTime_;
			}
			set
			{
				this.refreshTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
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
			this.rowId_.WriteTo(output, CityOpenChestResponse._repeated_rowId_codec);
			if (this.RefreshTime != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.RefreshTime);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.Score);
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
			num += this.rowId_.CalculateSize(CityOpenChestResponse._repeated_rowId_codec);
			if (this.RefreshTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RefreshTime);
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
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
						goto IL_0060;
					}
				}
				else
				{
					if (num == 26U)
					{
						goto IL_0060;
					}
					if (num == 32U)
					{
						this.RefreshTime = input.ReadUInt64();
						continue;
					}
					if (num == 40U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_0060:
				this.rowId_.AddEntriesFrom(input, CityOpenChestResponse._repeated_rowId_codec);
			}
		}

		private static readonly MessageParser<CityOpenChestResponse> _parser = new MessageParser<CityOpenChestResponse>(() => new CityOpenChestResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RowIdFieldNumber = 3;

		private static readonly FieldCodec<ulong> _repeated_rowId_codec = FieldCodec.ForUInt64(26U);

		private readonly RepeatedField<ulong> rowId_ = new RepeatedField<ulong>();

		public const int RefreshTimeFieldNumber = 4;

		private ulong refreshTime_;

		public const int ScoreFieldNumber = 5;

		private int score_;
	}
}
