using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Shop.Arena
{
	public sealed class GmVideoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GmVideoDto> Parser
		{
			get
			{
				return GmVideoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string VideoId
		{
			get
			{
				return this.videoId_;
			}
			set
			{
				this.videoId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string VideoUrl
		{
			get
			{
				return this.videoUrl_;
			}
			set
			{
				this.videoUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GmVideImgUrlDto> ImgUrl
		{
			get
			{
				return this.imgUrl_;
			}
		}

		[DebuggerNonUserCode]
		public bool IsReward
		{
			get
			{
				return this.isReward_;
			}
			set
			{
				this.isReward_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.VideoId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.VideoId);
			}
			if (this.VideoUrl.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.VideoUrl);
			}
			this.imgUrl_.WriteTo(output, GmVideoDto._repeated_imgUrl_codec);
			if (this.IsReward)
			{
				output.WriteRawTag(32);
				output.WriteBool(this.IsReward);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.VideoId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.VideoId);
			}
			if (this.VideoUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.VideoUrl);
			}
			num += this.imgUrl_.CalculateSize(GmVideoDto._repeated_imgUrl_codec);
			if (this.IsReward)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 10U)
					{
						this.VideoId = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.VideoUrl = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.imgUrl_.AddEntriesFrom(input, GmVideoDto._repeated_imgUrl_codec);
						continue;
					}
					if (num == 32U)
					{
						this.IsReward = input.ReadBool();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GmVideoDto> _parser = new MessageParser<GmVideoDto>(() => new GmVideoDto());

		public const int VideoIdFieldNumber = 1;

		private string videoId_ = "";

		public const int VideoUrlFieldNumber = 2;

		private string videoUrl_ = "";

		public const int ImgUrlFieldNumber = 3;

		private static readonly FieldCodec<GmVideImgUrlDto> _repeated_imgUrl_codec = FieldCodec.ForMessage<GmVideImgUrlDto>(26U, GmVideImgUrlDto.Parser);

		private readonly RepeatedField<GmVideImgUrlDto> imgUrl_ = new RepeatedField<GmVideImgUrlDto>();

		public const int IsRewardFieldNumber = 4;

		private bool isReward_;
	}
}
