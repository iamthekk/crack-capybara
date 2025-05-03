using System;
using System.Reflection;
using Framework;
using Framework.SocketNet;
using Google.Protobuf;
using NetWork;

namespace HotFix
{
	public class SocketMessageHandler_GameProxy : ISocketMessageHandler
	{
		protected ISocketNetGame SocketNet
		{
			get
			{
				return GameApp.SocketNet as ISocketNetGame;
			}
		}

		public byte[] ConvertMessageToByteArray(IMessage message)
		{
			ushort messageId = PackageFactory.GetMessageId(message);
			ushort num = (ushort)message.CalculateSize();
			byte[] array = new byte[(int)(num + 2 + 2)];
			int num2 = 0;
			Buffer.BlockCopy(BitConverter.GetBytes(messageId), 0, array, num2, 2);
			num2 += 2;
			Buffer.BlockCopy(BitConverter.GetBytes(num), 0, array, num2, 2);
			num2 += 2;
			using (CodedOutputStream codedOutputStream = new CodedOutputStream(array, num2, (int)num))
			{
				message.WriteTo(codedOutputStream);
			}
			return array;
		}

		private IMessage ConvertByteArrayToMessage(byte[] body, int startindex)
		{
			ushort num = BitConverter.ToUInt16(body, startindex);
			int num2 = startindex + 2;
			ushort num3 = BitConverter.ToUInt16(body, num2);
			num2 += 2;
			IMessage message = PackageFactory.CreateMessage(num);
			if (body.Length <= num2 + (int)num3)
			{
				HLog.LogError(string.Format("ConvertByteArrayToMessage error , data.Length:{0} readindex:{1} size:{2}", body.Length, num2, num3));
			}
			else
			{
				if (message != null)
				{
					try
					{
						CodedInputStream codedInputStream = new CodedInputStream(body, num2, (int)num3);
						message.MergeFrom(codedInputStream);
						goto IL_0089;
					}
					catch (Exception ex)
					{
						HLog.LogException(ex);
						return null;
					}
				}
				HLog.LogError(string.Format("messageid 不存在 id:{0}", num));
			}
			IL_0089:
			PropertyInfo property = message.GetType().GetProperty("CommonParams");
			if (property != null)
			{
				property.SetValue(message, NetworkUtils.GetCommonParams(), null);
			}
			return message;
		}

		public int GetMsgID(IMessage msg)
		{
			return (int)PackageFactory.GetMessageId(msg);
		}

		public bool HandleByteMessage(byte[] bytes, int startindex)
		{
			IMessage message = this.ConvertByteArrayToMessage(bytes, startindex);
			this.SocketNet.SetMessageToQueue(message);
			return message != null;
		}
	}
}
