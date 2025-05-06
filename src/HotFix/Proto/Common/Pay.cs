using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class Pay : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<Pay> Parser
		{
			get
			{
				return Pay._parser;
			}
		}

		[DebuggerNonUserCode]
		public TimeBase TimeBase
		{
			get
			{
				return this.timeBase_;
			}
			set
			{
				this.timeBase_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.timeBase_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.TimeBase);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.timeBase_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.TimeBase);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					input.SkipLastField();
				}
				else
				{
					if (this.timeBase_ == null)
					{
						this.timeBase_ = new TimeBase();
					}
					input.ReadMessage(this.timeBase_);
				}
			}
		}

		private static readonly MessageParser<Pay> _parser = new MessageParser<Pay>(() => new Pay());

		public const int TimeBaseFieldNumber = 1;

		private TimeBase timeBase_;
	}
}
