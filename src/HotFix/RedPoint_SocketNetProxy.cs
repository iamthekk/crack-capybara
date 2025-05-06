using System;
using Framework;
using Google.Protobuf;
using NetWork;
using Proto.Actor;
using Socket.Guild;

namespace HotFix
{
	public class RedPoint_SocketNetProxy
	{
		public void StartProxy()
		{
		}

		public void StopProxy()
		{
		}

		private int _MsgID(Type msgtype)
		{
			return (int)PackageFactory.GetMessageId(Activator.CreateInstance(msgtype) as IMessage);
		}

		private void OnServerSocketPushMessage(IMessage msg)
		{
			SocketPushMessage socketPushMessage = msg as SocketPushMessage;
			if (socketPushMessage != null)
			{
				uint messageType = socketPushMessage.MessageType;
				if (messageType == 301U)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_AddInteractiveSocketCount, null);
					RedPointController.Instance.ReCalc("Main.Sociality", true);
					return;
				}
				if (messageType != 302U)
				{
					return;
				}
				NetworkUtils.MainCity.DoCityGetInfoRequest(delegate(bool isOk, CityGetInfoResponse rep)
				{
					EventArgsRefreshLordAddSlaveData instance = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
					instance.Clear();
					instance.SetData(rep.Extra, (int)rep.SlaveCount);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance);
				});
			}
		}
	}
}
