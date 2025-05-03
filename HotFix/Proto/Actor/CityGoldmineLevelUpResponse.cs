using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class CityGoldmineLevelUpResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityGoldmineLevelUpResponse> Parser
		{
			get
			{
				return CityGoldmineLevelUpResponse._parser;
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
		public int Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.Level != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Level);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.CommonData);
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
			if (this.Level != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Level);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
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
					if (num != 16U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
						}
					}
					else
					{
						this.Level = input.ReadInt32();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<CityGoldmineLevelUpResponse> _parser = new MessageParser<CityGoldmineLevelUpResponse>(() => new CityGoldmineLevelUpResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int LevelFieldNumber = 2;

		private int level_;

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
