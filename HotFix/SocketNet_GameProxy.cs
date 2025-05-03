using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dxx.Chat;
using Framework;
using Framework.EventSystem;
using Framework.SocketNet;
using Google.Protobuf;
using NetWork;
using Proto.Chat;
using Socket.Guild;

namespace HotFix
{
	public class SocketNet_GameProxy : ISocketGameProxy, ISocketGameProxyBase, ISocketGameLogicProxy
	{
		public event Action<SocketGroupType, string> OnJoinGroup
		{
			add
			{
				if (value != null)
				{
					foreach (KeyValuePair<SocketGroupType, string> keyValuePair in this.mSocketGroupSet)
					{
						value(keyValuePair.Key, keyValuePair.Value);
					}
				}
				this.mOnJoinGroup.Add(value);
			}
			remove
			{
				this.mOnJoinGroup.Remove(value);
			}
		}

		public event Action<SocketGroupType, string> OnQuitGroup
		{
			add
			{
				this.mOnQuitGroup.Add(value);
			}
			remove
			{
				this.mOnQuitGroup.Remove(value);
			}
		}

		public event Action<SocketPushMessage> OnPushMessage
		{
			add
			{
				this.mOnPushMessage.Add(value);
			}
			remove
			{
				this.mOnPushMessage.Remove(value);
			}
		}

		public ISocketNetGame SocketNet
		{
			get
			{
				return GameApp.SocketNet as ISocketNetGame;
			}
		}

		public void OnInit()
		{
			this.mMessageHandler = new SocketMessageHandler_GameProxy();
			this.SocketNet.SetSocketMessageHandler(this.mMessageHandler);
			this.RegCommonSocketMessageHandler();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnUserLogin));
		}

		public void OnUnInit()
		{
			this.UnRegCommonSocketMessageHandler();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnUserLogin));
		}

		public async Task SendLogin()
		{
			if (this.SocketNet != null && !string.IsNullOrEmpty(this.SocketNet.UserToken))
			{
				DateTime dateTime = DateTime.Now;
				DateTime dateLastSendTime = DateTime.Now.AddHours(-1.0);
				int trycount = 0;
				Dictionary<SocketGroupType, string> logingroup = new Dictionary<SocketGroupType, string>();
				while (this.SocketNet != null && this.SocketNet.Connected && !this.SocketNet.LoginSuccess)
				{
					if ((DateTime.Now - dateTime).TotalSeconds > 60.0)
					{
						HLog.LogError("登录超时！！！");
						break;
					}
					if (DateTime.Now < dateLastSendTime)
					{
						await Task.Delay(10);
					}
					else
					{
						trycount++;
						SocketLoginRequest socketLoginRequest = new SocketLoginRequest();
						socketLoginRequest.AccessToken = this.SocketNet.UserToken;
						string text = "login join group ";
						logingroup.Clear();
						foreach (KeyValuePair<SocketGroupType, string> keyValuePair in this.mSocketGroupSet)
						{
							text += string.Format("[{0}={1}]", keyValuePair.Key, keyValuePair.Value);
							socketLoginRequest.GroupInfo[(uint)keyValuePair.Key] = keyValuePair.Value;
							logingroup[keyValuePair.Key] = keyValuePair.Value;
						}
						dateLastSendTime = DateTime.Now.AddMilliseconds(5000.0);
						this.Send(socketLoginRequest);
					}
				}
				this.CheckJoinGroup(logingroup);
			}
		}

		public void SetSocketGroup(int kind, string groupName)
		{
			this.SetSocketGroup((SocketGroupType)kind, groupName);
		}

		public void SendHeart()
		{
			SocketHeartBeatRequest socketHeartBeatRequest = new SocketHeartBeatRequest();
			this.Send(socketHeartBeatRequest);
		}

		public void AddJoinGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnJoinGroup += handler;
		}

		public void RemoveJoinGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnJoinGroup -= handler;
		}

		public void AddQuitGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnQuitGroup += handler;
		}

		public void RemoveQuitGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnQuitGroup -= handler;
		}

		public void AddPushMessageHandler(Action<SocketPushMessage> handler)
		{
			this.OnPushMessage += handler;
		}

		public void RemovePushMessageHandler(Action<SocketPushMessage> handler)
		{
			this.OnPushMessage -= handler;
		}

		public void SendChat(ChatCommonRequest req, Action<bool, ChatCommonResponse> callback)
		{
			ChatProxy.Net.TrySendMessage(req, delegate(IMessage response)
			{
				ChatCommonResponse chatCommonResponse = response as ChatCommonResponse;
				if (chatCommonResponse != null && chatCommonResponse.Code == 0)
				{
					Action<bool, ChatCommonResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, chatCommonResponse);
					return;
				}
				else
				{
					Action<bool, ChatCommonResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, chatCommonResponse);
					return;
				}
			}, false, false);
		}

		public void SendChat(ChatGuildRequest req, Action<bool, ChatGuildResponse> callback)
		{
			ChatProxy.Net.TrySendMessage(req, delegate(IMessage response)
			{
				ChatGuildResponse chatGuildResponse = response as ChatGuildResponse;
				if (chatGuildResponse != null && chatGuildResponse.Code == 0)
				{
					Action<bool, ChatGuildResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, chatGuildResponse);
					return;
				}
				else
				{
					Action<bool, ChatGuildResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, chatGuildResponse);
					return;
				}
			}, false, false);
		}

		public void GetMessageHistory(long msgSeq, SocketGroupType groupType, string groupId, Action<bool, ChatGetMessageRecordsResponse> callback)
		{
			NetworkUtils.Chat.DoRequest_GetMessageRecords(msgSeq, groupType, groupId, callback);
		}

		public void GetUnreadCount(long msgSeq, SocketGroupType groupType, string groupId, Action<bool, long, int> callback)
		{
			if (callback != null)
			{
				callback(false, 0L, 0);
			}
		}

		public void TranslateChat(long msgSeq, SocketGroupType groupType, string groupId, int fromLanguage, int toLanguage, string content, Action<bool, ChatTranslateResponse> callback)
		{
			ChatProxy.Net.TrySendMessage(new ChatTranslateRequest
			{
				CommonParams = ChatProxy.Net.GetCommonParams(),
				Context = content,
				SourceLanguageId = (uint)fromLanguage,
				TargetLanguageId = (uint)toLanguage
			}, delegate(IMessage response)
			{
				ChatTranslateResponse chatTranslateResponse = response as ChatTranslateResponse;
				if (chatTranslateResponse != null && chatTranslateResponse.Code == 0)
				{
					Action<bool, ChatTranslateResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, chatTranslateResponse);
					return;
				}
				else
				{
					Action<bool, ChatTranslateResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, chatTranslateResponse);
					return;
				}
			}, false, false);
		}

		public void SetSocketGroup(SocketGroupType type, string key)
		{
			this.InternalSetGroup(type, key);
		}

		private void InternalSetGroup(SocketGroupType type, string key)
		{
			string text;
			if (this.mSocketGroupSet.TryGetValue(type, out text))
			{
				if (text == key)
				{
					return;
				}
			}
			else if (string.IsNullOrEmpty(key))
			{
				return;
			}
			if (string.IsNullOrEmpty(key))
			{
				this.mSocketGroupSet.Remove(type);
				this.InternalOnQuitGroup(type, text);
			}
			else
			{
				this.mSocketGroupSet[type] = key;
				this.InternalOnQuitGroup(type, text);
				this.InternalOnJoinGroup(type, key);
			}
			if (this.SocketNet.LoginSuccess)
			{
				this.JoinOrQuitSocketGroup(type, text, key);
			}
		}

		private void InternalOnJoinGroup(SocketGroupType type, string key)
		{
			for (int i = 0; i < this.mOnJoinGroup.Count; i++)
			{
				Action<SocketGroupType, string> action = this.mOnJoinGroup[i];
				if (action != null)
				{
					action(type, key);
				}
			}
		}

		private void InternalOnQuitGroup(SocketGroupType type, string key)
		{
			for (int i = 0; i < this.mOnQuitGroup.Count; i++)
			{
				Action<SocketGroupType, string> action = this.mOnQuitGroup[i];
				if (action != null)
				{
					action(type, key);
				}
			}
		}

		private void InternalOnPushMessage(SocketPushMessage msg)
		{
			for (int i = 0; i < this.mOnPushMessage.Count; i++)
			{
				Action<SocketPushMessage> action = this.mOnPushMessage[i];
				if (action != null)
				{
					action(msg);
				}
			}
		}

		private void CheckJoinGroup(Dictionary<SocketGroupType, string> dic)
		{
			foreach (KeyValuePair<SocketGroupType, string> keyValuePair in this.mSocketGroupSet)
			{
				string text;
				if (!dic.TryGetValue(keyValuePair.Key, out text) || !(keyValuePair.Value == text))
				{
					this.JoinOrQuitSocketGroup(keyValuePair.Key, text, keyValuePair.Value);
				}
			}
		}

		private void JoinOrQuitSocketGroup(SocketGroupType type, string oldkey, string newkey)
		{
			if (string.IsNullOrEmpty(newkey))
			{
				this.Send(new SocketQuitGroupRequest
				{
					GroupId = oldkey,
					GroupType = (uint)type
				});
				return;
			}
			if (!string.IsNullOrEmpty(oldkey))
			{
				this.Send(new SocketQuitGroupRequest
				{
					GroupId = oldkey,
					GroupType = (uint)type
				});
			}
			SocketJoinGroupRequest socketJoinGroupRequest = new SocketJoinGroupRequest();
			socketJoinGroupRequest.GroupInfo[(uint)type] = newkey;
			this.Send(socketJoinGroupRequest);
		}

		private void Send(IMessage msg)
		{
			this.SocketNet.Send(msg);
		}

		private void OnUserLogin(object sender, int type, BaseEventArgs eventArgs)
		{
			EventLogin eventLogin = eventArgs as EventLogin;
			if (eventLogin != null)
			{
				this.SocketNet.SetTokenAfterLogin(eventLogin.userLoginResponse.UserId, eventLogin.userLoginResponse.AccessToken);
			}
		}

		private void RegCommonSocketMessageHandler()
		{
			ISocketNetGame socketNet = this.SocketNet;
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketLoginResponse)), new Action<IMessage>(this.S2C_SocketLoginResponse));
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketLoginRepeatMessage)), new Action<IMessage>(this.S2C_SocketLoginRepeatMessage));
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketPushMessage)), new Action<IMessage>(this.S2C_SocketPushMessage));
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketErrorMessage)), new Action<IMessage>(this.S2C_SocketErrorMessage));
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketReconnectMessage)), new Action<IMessage>(this.S2C_SocketReconnectMessage));
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketHeartBeatResponse)), new Action<IMessage>(this.S2C_SocketHeartBeatResponse));
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketJoinGroupResponse)), new Action<IMessage>(this.S2C_SocketJoinGroupResponse));
			socketNet.AddMessageHandler(this._MsgID(typeof(SocketQuitGroupResponse)), new Action<IMessage>(this.S2C_SocketQuitGroupResponse));
		}

		private void UnRegCommonSocketMessageHandler()
		{
			ISocketNetGame socketNet = this.SocketNet;
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketLoginResponse)), new Action<IMessage>(this.S2C_SocketLoginResponse));
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketLoginRepeatMessage)), new Action<IMessage>(this.S2C_SocketLoginRepeatMessage));
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketPushMessage)), new Action<IMessage>(this.S2C_SocketPushMessage));
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketErrorMessage)), new Action<IMessage>(this.S2C_SocketErrorMessage));
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketReconnectMessage)), new Action<IMessage>(this.S2C_SocketReconnectMessage));
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketHeartBeatResponse)), new Action<IMessage>(this.S2C_SocketHeartBeatResponse));
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketJoinGroupResponse)), new Action<IMessage>(this.S2C_SocketJoinGroupResponse));
			socketNet.RemoveMessageHandler(this._MsgID(typeof(SocketQuitGroupResponse)), new Action<IMessage>(this.S2C_SocketQuitGroupResponse));
		}

		private int _MsgID(Type msgtype)
		{
			return (int)PackageFactory.GetMessageId(Activator.CreateInstance(msgtype) as IMessage);
		}

		public int GetSocketMsgID(IMessage msg)
		{
			return this._MsgID(msg.GetType());
		}

		private void S2C_SocketLoginResponse(IMessage msg)
		{
			SocketLoginResponse socketLoginResponse = msg as SocketLoginResponse;
			if (socketLoginResponse != null && socketLoginResponse.Code == 0)
			{
				this.SocketNet.OnSetLoginSuccess();
			}
		}

		private void S2C_SocketLoginRepeatMessage(IMessage msg)
		{
			if (msg is SocketLoginRepeatMessage)
			{
				this.SocketNet.OnLoginRepeat();
			}
		}

		private void S2C_SocketPushMessage(IMessage msg)
		{
			SocketPushMessage socketPushMessage = msg as SocketPushMessage;
			if (socketPushMessage != null)
			{
				"服务器推送：type:" + socketPushMessage.MessageType.ToString() + "msg:" + socketPushMessage.MessageContent;
				this.InternalOnPushMessage(socketPushMessage);
			}
		}

		private void S2C_SocketErrorMessage(IMessage msg)
		{
			SocketErrorMessage socketErrorMessage = msg as SocketErrorMessage;
			if (socketErrorMessage != null)
			{
				HLog.LogError(string.Format("服务器通用错误：Code:{0} msg:{1}", socketErrorMessage.Code, socketErrorMessage.Msg));
			}
		}

		private void S2C_SocketReconnectMessage(IMessage msg)
		{
			if (msg is SocketReconnectMessage)
			{
				HLog.LogError("服务器要求主动断开重新连接 ");
				this.SocketNet.OnLoginReconnect();
			}
		}

		private void S2C_SocketHeartBeatResponse(IMessage msg)
		{
			if (msg is SocketHeartBeatResponse)
			{
				this.SocketNet.LastRecvHeartTime = DateTime.Now;
			}
		}

		private void S2C_SocketJoinGroupResponse(IMessage msg)
		{
			SocketJoinGroupResponse socketJoinGroupResponse = msg as SocketJoinGroupResponse;
		}

		private void S2C_SocketQuitGroupResponse(IMessage msg)
		{
			SocketQuitGroupResponse socketQuitGroupResponse = msg as SocketQuitGroupResponse;
		}

		private Dictionary<SocketGroupType, string> mSocketGroupSet = new Dictionary<SocketGroupType, string>();

		private List<Action<SocketGroupType, string>> mOnJoinGroup = new List<Action<SocketGroupType, string>>();

		private List<Action<SocketGroupType, string>> mOnQuitGroup = new List<Action<SocketGroupType, string>>();

		private List<Action<SocketPushMessage>> mOnPushMessage = new List<Action<SocketPushMessage>>();

		private SocketMessageHandler_GameProxy mMessageHandler;
	}
}
