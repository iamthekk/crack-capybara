using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Framework.Platfrom;
using Google.Protobuf;
using UnityEngine;

namespace Framework.SocketNet
{
	public class SocketNet : ISocketNet, ISocketNetGame
	{
		public string UserToken
		{
			get
			{
				return this.mToken;
			}
		}

		public bool Init()
		{
			this.mConfig = SocketNetConfig.GetConfig();
			if (this.mConfig == null)
			{
				HLog.LogError("未能初始化 SocketNet ,未找到配置文件！");
				return false;
			}
			this.SocketObject = new GameObject("SocketNet");
			Object.DontDestroyOnLoad(this.SocketObject);
			this.ConnectCtrl = this.SocketObject.AddComponent<SocketNetConnectCtrl>();
			this.ConnectCtrl.BindSocketNet = this;
			GameObject gameObject = this.CreateSubGameObject("MessageQueue");
			this.MessageQueue = gameObject.AddComponent<SocketNetMessageQueue>();
			this.MessageQueue.OnUpdate = new Action(this.OnMainThreadUpdate);
			return true;
		}

		public void DeInit()
		{
			this.StopSocket();
			if (this.SocketObject != null)
			{
				Object.Destroy(this.SocketObject);
				this.SocketObject = null;
			}
			this.DeleteGameProxy();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public GameObject CreateSubGameObject(string name)
		{
			if (this.SocketObject == null)
			{
				return null;
			}
			Transform transform = this.SocketObject.transform.Find(name);
			if (transform == null)
			{
				GameObject gameObject = new GameObject(name);
				gameObject.transform.SetParent(this.SocketObject.transform, false);
				return gameObject;
			}
			return transform.gameObject;
		}

		public void SetTokenAfterLogin(long userid, string token)
		{
			if (this.mUserID != userid || string.IsNullOrEmpty(this.mToken) || !this.Connected)
			{
				this.StopSocket();
				this.mUserID = userid;
				this.mToken = token;
				this.StartConnect(10);
				return;
			}
			if (this.mToken == token)
			{
				return;
			}
			this.mToken = token;
			if (this.LoginSuccess)
			{
				this.SendLogin();
			}
		}

		public void SetLoginSuccessFalse()
		{
			this.LoginSuccess = false;
		}

		private void OnMainThreadUpdate()
		{
			if (GameApp.State.GetCurrentStateName() != 103)
			{
				return;
			}
			this.MainThreadHandleMessage();
		}

		public ISocketGameProxyBase GameProxy
		{
			get
			{
				return this.mGameProxy;
			}
		}

		public bool LoginSuccess { get; private set; }

		public DateTime LastSendHeartTime
		{
			get
			{
				return this.mLastSendHeartTime;
			}
		}

		public DateTime LastRecvHeartTime
		{
			get
			{
				return this.mLastRecvHeartTime;
			}
			set
			{
				this.mLastRecvHeartTime = value;
			}
		}

		public void SetSocketMessageHandler(ISocketGameProxy proxy)
		{
			this.DeleteGameProxy();
			this.mGameProxy = proxy;
			if (this.mGameProxy != null)
			{
				this.mGameProxy.OnInit();
			}
		}

		public void SetSocketGameHandler(ISocketGameProxyBase proxy)
		{
			this.SetSocketMessageHandler((ISocketGameProxy)proxy);
		}

		private void DeleteGameProxy()
		{
			this.LoginSuccess = false;
			if (this.mGameProxy != null)
			{
				this.mGameProxy.OnUnInit();
			}
			this.mGameProxy = null;
		}

		public async Task SendLogin()
		{
			if (this.mGameProxy != null)
			{
				await this.mGameProxy.SendLogin();
			}
		}

		public void OnSetLoginSuccess()
		{
			this.LoginSuccess = true;
		}

		public void OnLoginRepeat()
		{
			this.LoginSuccess = true;
			this.StopSocket();
		}

		public void OnLoginReconnect()
		{
			this.StopSocket();
			this.LoginSuccess = false;
			this.CheckReconnect("服务器要求主动断开重新连接");
		}

		private void CheckSendHeart()
		{
			if (GameApp.State.GetCurrentStateName() != 103)
			{
				return;
			}
			if ((DateTime.Now - this.mLastSendHeartTime).TotalSeconds > (double)this.SendHeartInterval && this.Connected && this.LoginSuccess)
			{
				if (this.mGameProxy != null)
				{
					this.mGameProxy.SendHeart();
				}
				this.mLastSendHeartTime = DateTime.Now;
			}
		}

		public void SetSocketGroup(int kind, string groupName)
		{
			ISocketGameProxy socketGameProxy = this.mGameProxy;
			if (socketGameProxy == null)
			{
				return;
			}
			socketGameProxy.SetSocketGroup(kind, groupName);
		}

		private void AddJoinGroupHandler(Action<int, string> handler)
		{
		}

		private void RemoveJoinGroupHandler(Action<int, string> handler)
		{
		}

		private void AddQuitGroupHandler(Action<int, string> handler)
		{
		}

		private void RemoveQuitGroupHandler(Action<int, string> handler)
		{
		}

		public void SetSocketMessageHandler(ISocketMessageHandler handler)
		{
			this.mMessageHandler = handler;
		}

		public void SetMessageToQueue(IMessage msg)
		{
			this.mRecvMessageQueue.Enqueue(msg);
		}

		public void MainThreadHandleMessage()
		{
			while (this.mRecvMessageQueue.Count > 0)
			{
				IMessage message;
				if (this.mRecvMessageQueue.TryDequeue(out message))
				{
					this.HandleMessage(message);
				}
			}
		}

		private int TryGetMsgID(IMessage msg)
		{
			if (this.mGameProxy == null)
			{
				return 0;
			}
			return this.mGameProxy.GetSocketMsgID(msg);
		}

		private void HandleMessage(IMessage msg)
		{
			if (msg == null)
			{
				return;
			}
			int num = this.TryGetMsgID(msg);
			if (num <= 0)
			{
				HLog.LogError(string.Format("HandleMessage msgid must >=0 but it is {0} ( {1} )", num, msg.GetType().FullName));
				return;
			}
			SocketNet.RecvMsgHandler recvMsgHandler;
			if (this.mRecvMsgHandlerDic.TryGetValue(num, out recvMsgHandler))
			{
				recvMsgHandler.Invoke(msg);
			}
		}

		public void Send(IMessage msg)
		{
			if (msg == null)
			{
				return;
			}
			if (this.mMessageHandler == null)
			{
				return;
			}
			byte[] array = this.mMessageHandler.ConvertMessageToByteArray(msg);
			try
			{
				msg.GetType().Name != "SocketHeartBeatRequest";
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			this.OnSendData(array, 0U, array.Length);
		}

		public void AddMessageHandler(int msgid, Action<IMessage> handler)
		{
			SocketNet.RecvMsgHandler recvMsgHandler;
			if (!this.mRecvMsgHandlerDic.TryGetValue(msgid, out recvMsgHandler))
			{
				recvMsgHandler = null;
			}
			if (recvMsgHandler == null)
			{
				recvMsgHandler = new SocketNet.RecvMsgHandler();
				this.mRecvMsgHandlerDic[msgid] = recvMsgHandler;
			}
			recvMsgHandler.Add(handler);
		}

		public void RemoveMessageHandler(int msgid, Action<IMessage> handler)
		{
			SocketNet.RecvMsgHandler recvMsgHandler;
			if (this.mRecvMsgHandlerDic.TryGetValue(msgid, out recvMsgHandler))
			{
				recvMsgHandler.Remove(handler);
			}
		}

		public void ClearMessageHandler()
		{
			this.mRecvMsgHandlerDic.Clear();
		}

		public bool Connected
		{
			get
			{
				return this.mCurrentClient != null && this.mCurrentClient.Connected;
			}
		}

		private bool mIsConnecting
		{
			get
			{
				return this.mConnectThreadID != 0;
			}
		}

		public async Task StartConnect(int delaymilsec)
		{
			if (this.IsSocketEnable)
			{
				if (delaymilsec > 0)
				{
					await TaskExpand.Delay(delaymilsec);
				}
				if (!this.mIsConnecting)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.InternalThreadStartConnect));
				}
			}
		}

		private async void InternalThreadStartConnect(object obj)
		{
			this.mConnectThreadID = Thread.CurrentThread.ManagedThreadId;
			await this.InternalConnect();
			this.mConnectThreadID = 0;
		}

		private async Task InternalConnect()
		{
			this.mRecvTaskVersion += 1L;
			long taskversion = this.mRecvTaskVersion;
			try
			{
				this.mCurrentClient = new TcpClient();
				IPAddress[] addressList = (await Dns.GetHostEntryAsync(this.mConfig.GetHost())).AddressList;
				if (addressList.Length != 0)
				{
					IPAddress ipaddress = addressList[0];
					await this.mCurrentClient.ConnectAsync(ipaddress, (int)this.mConfig.GetPort());
					await this.WaitConnect();
				}
				else
				{
					HLog.LogError("无法解析域名 " + this.mConfig.GetHost());
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			try
			{
				if (this.Connected && taskversion == this.mRecvTaskVersion)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.RecvDataAsync));
				}
			}
			catch (Exception ex2)
			{
				HLog.LogException(ex2);
			}
			try
			{
				if (this.Connected && taskversion == this.mRecvTaskVersion)
				{
					await this.SendLogin();
				}
			}
			catch (Exception ex3)
			{
				HLog.LogException(ex3);
			}
		}

		public void StopSocket()
		{
			this.mRecvTaskVersion += 1L;
			try
			{
				if (this.mCurrentClient != null)
				{
					this.mCurrentClient.Close();
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			finally
			{
				this.mCurrentClient = null;
			}
		}

		public void PrepareReConnect()
		{
			this.LoginSuccess = false;
			this.mRecvBuf.Clear();
			this.StopSocket();
		}

		private async Task InternalCheckReconnect(string whyneed)
		{
			if (this.IsSocketEnable)
			{
				if (!this.Connected)
				{
					this.PrepareReConnect();
					await this.StartConnect(0);
				}
			}
		}

		public void CheckReconnect(string whyneed)
		{
			this.InternalCheckReconnect(whyneed);
		}

		private async Task WaitConnect()
		{
			float wait = 10000f;
			long taskversion = this.mRecvTaskVersion;
			while (wait > 0f && taskversion == this.mRecvTaskVersion && !this.Connected)
			{
				await TaskExpand.Delay(100);
				wait -= 100f;
			}
		}

		private async void RecvDataAsync(object obj)
		{
			this.mRecvThreadID = Thread.CurrentThread.ManagedThreadId;
			NetworkStream netstream = null;
			long taskversion = this.mRecvTaskVersion;
			try
			{
				if (!this.Connected)
				{
					this.mRecvThreadID = 0;
					return;
				}
				while (taskversion == this.mRecvTaskVersion && this.Connected)
				{
					if (netstream == null)
					{
						netstream = this.mCurrentClient.GetStream();
					}
					this.CheckSendHeart();
					if (!netstream.DataAvailable)
					{
						await TaskExpand.Delay(10);
					}
					else
					{
						if (this.mRecvBuf.DataLength > 0)
						{
							if (DateTime.Now - this.mRecvBuf.LastRecvData > TimeSpan.FromSeconds(4.0))
							{
								this.mRecvBuf.DataLength = 0;
							}
							HLog.LogError("因为接收数据超时，放弃部分脏数据！！！");
						}
						netstream.ReadTimeout = 100;
						int num = await netstream.ReadAsync(this.mRecvBuf.Bytes, this.mRecvBuf.DataLength, this.mRecvBuf.Length);
						if (num <= 0)
						{
							HLog.LogError("Connection closed !!!!");
							break;
						}
						this.mRecvBuf.LastRecvData = DateTime.Now;
						this.OnRecvData(this.mRecvBuf, num);
					}
				}
				if (netstream != null)
				{
					netstream.Dispose();
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			if (taskversion == this.mRecvTaskVersion && this.Connected)
			{
				this.mRecvThreadID = 0;
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.RecvDataAsync));
			}
			else
			{
				this.mRecvThreadID = 0;
			}
		}

		private void OnRecvData(SocketNet.NetRecvBuffer buffer, int readlength)
		{
			int num = buffer.DataLength + readlength;
			byte[] bytes = buffer.Bytes;
			if (buffer.Length > num + 2)
			{
				bytes[num] = 0;
			}
			int num2 = 0;
			while (num2 + 2 + 2 < bytes.Length && num2 < num && BitConverter.ToUInt16(bytes, num2) != 0)
			{
				int num3 = num2;
				num2 += 2;
				ushort num4 = BitConverter.ToUInt16(bytes, num2);
				num2 += 2;
				if ((int)num4 + num2 > buffer.DataLength + readlength)
				{
					buffer.DataLength += readlength;
					buffer.CheckSize((int)num4 + num3 + 2 + 2);
					num2 = num3;
					break;
				}
				if (this.mMessageHandler != null)
				{
					this.mMessageHandler.HandleByteMessage(bytes, num3);
				}
				num2 += (int)num4;
			}
			if (num2 >= bytes.Length || num2 >= num)
			{
				buffer.DataLength = 0;
				return;
			}
			int num5 = num - num2;
			for (int i = num2; i < num; i++)
			{
				bytes[i - num2] = bytes[i];
			}
			buffer.DataLength = num5;
		}

		private async void OnSendData(byte[] data, uint startindex, int len)
		{
			if (data == null || (long)data.Length < (long)((ulong)startindex + (ulong)((long)len)))
			{
				HLog.LogError("要发送的数据有问题，请检查！");
			}
			else
			{
				if (!this.Connected)
				{
					HLog.LogError("网络未连接，无法发送数据！");
				}
				try
				{
					await this.mCurrentClient.GetStream().WriteAsync(data, (int)startindex, len);
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
			}
		}

		public GameObject SocketObject;

		public SocketNetMessageQueue MessageQueue;

		public SocketNetConnectCtrl ConnectCtrl;

		private SocketNetConfig mConfig;

		private long mUserID;

		private string mToken;

		public bool IsSocketEnable = true;

		private ISocketGameProxy mGameProxy;

		private DateTime mLastSendHeartTime;

		private DateTime mLastRecvHeartTime;

		public float SendHeartInterval = 10f;

		private ISocketMessageHandler mMessageHandler;

		private Dictionary<int, SocketNet.RecvMsgHandler> mRecvMsgHandlerDic = new Dictionary<int, SocketNet.RecvMsgHandler>();

		private ConcurrentQueue<IMessage> mRecvMessageQueue = new ConcurrentQueue<IMessage>();

		private TcpClient mCurrentClient;

		private long mRecvTaskVersion;

		private SocketNet.NetRecvBuffer mRecvBuf = new SocketNet.NetRecvBuffer(65536);

		private int mConnectThreadID;

		private int mRecvThreadID;

		private class RecvMsgHandler
		{
			public void Add(Action<IMessage> handler)
			{
				List<Action<IMessage>> list = this.mActions;
				lock (list)
				{
					this.mActions.Add(handler);
				}
			}

			public void Remove(Action<IMessage> handler)
			{
				List<Action<IMessage>> list = this.mActions;
				lock (list)
				{
					for (int i = 0; i < this.mActions.Count; i++)
					{
						if (this.mActions[i] == handler)
						{
							this.mActions.RemoveAt(i);
							i--;
						}
					}
				}
			}

			public void Invoke(IMessage msg)
			{
				if (msg == null)
				{
					return;
				}
				List<Action<IMessage>> list = this.mActions;
				lock (list)
				{
					this.mCallList.Clear();
					this.mCallList.AddRange(this.mActions);
				}
				for (int i = 0; i < this.mCallList.Count; i++)
				{
					if (this.mCallList[i] != null)
					{
						Action<IMessage> action = this.mCallList[i];
						if (action != null)
						{
							action(msg);
						}
					}
				}
			}

			private List<Action<IMessage>> mActions = new List<Action<IMessage>>();

			private List<Action<IMessage>> mCallList = new List<Action<IMessage>>();
		}

		private class NetRecvBuffer
		{
			public NetRecvBuffer(int size)
			{
				this.Bytes = new byte[size];
			}

			public int Length
			{
				get
				{
					return this.Bytes.Length;
				}
			}

			public void CheckSize(int size)
			{
				if (this.Length >= size)
				{
					return;
				}
				Array.Resize<byte>(ref this.Bytes, size);
			}

			public void Clear()
			{
				this.DataLength = 0;
				this.LastRecvData = DateTime.Now;
			}

			public byte[] Bytes;

			public int DataLength;

			public DateTime LastRecvData;
		}
	}
}
