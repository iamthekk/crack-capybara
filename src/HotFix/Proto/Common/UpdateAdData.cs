using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UpdateAdData : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UpdateAdData> Parser
		{
			get
			{
				return UpdateAdData._parser;
			}
		}

		[DebuggerNonUserCode]
		public bool IsChange
		{
			get
			{
				return this.isChange_;
			}
			set
			{
				this.isChange_ = value;
			}
		}

		[DebuggerNonUserCode]
		public AdDataDto AdData
		{
			get
			{
				return this.adData_;
			}
			set
			{
				this.adData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.IsChange)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.IsChange);
			}
			if (this.adData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.AdData);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.IsChange)
			{
				num += 2;
			}
			if (this.adData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.AdData);
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
						input.SkipLastField();
					}
					else
					{
						if (this.adData_ == null)
						{
							this.adData_ = new AdDataDto();
						}
						input.ReadMessage(this.adData_);
					}
				}
				else
				{
					this.IsChange = input.ReadBool();
				}
			}
		}

		private static readonly MessageParser<UpdateAdData> _parser = new MessageParser<UpdateAdData>(() => new UpdateAdData());

		public const int IsChangeFieldNumber = 1;

		private bool isChange_;

		public const int AdDataFieldNumber = 2;

		private AdDataDto adData_;
	}
}
