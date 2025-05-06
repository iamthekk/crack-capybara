using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class Consume : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<Consume> Parser
		{
			get
			{
				return Consume._parser;
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
			if (this.timeBase_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.TimeBase);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Score);
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
				if (num != 10U)
				{
					if (num != 24U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Score = input.ReadInt32();
					}
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

		private static readonly MessageParser<Consume> _parser = new MessageParser<Consume>(() => new Consume());

		public const int TimeBaseFieldNumber = 1;

		private TimeBase timeBase_;

		public const int ScoreFieldNumber = 3;

		private int score_;
	}
}
