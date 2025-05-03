using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserHabbyMailBindRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserHabbyMailBindRequest> Parser
		{
			get
			{
				return UserHabbyMailBindRequest._parser;
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
		public string EmailDress
		{
			get
			{
				return this.emailDress_;
			}
			set
			{
				this.emailDress_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, string> BindParams
		{
			get
			{
				return this.bindParams_;
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
			if (this.EmailDress.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.EmailDress);
			}
			this.bindParams_.WriteTo(output, UserHabbyMailBindRequest._map_bindParams_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.EmailDress.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.EmailDress);
			}
			return num + this.bindParams_.CalculateSize(UserHabbyMailBindRequest._map_bindParams_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.bindParams_.AddEntriesFrom(input, UserHabbyMailBindRequest._map_bindParams_codec);
						}
					}
					else
					{
						this.EmailDress = input.ReadString();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<UserHabbyMailBindRequest> _parser = new MessageParser<UserHabbyMailBindRequest>(() => new UserHabbyMailBindRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int EmailDressFieldNumber = 2;

		private string emailDress_ = "";

		public const int BindParamsFieldNumber = 3;

		private static readonly MapField<string, string>.Codec _map_bindParams_codec = new MapField<string, string>.Codec(FieldCodec.ForString(10U), FieldCodec.ForString(18U), 26U);

		private readonly MapField<string, string> bindParams_ = new MapField<string, string>();
	}
}
