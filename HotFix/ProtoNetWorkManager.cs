using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Framework;
using Framework.Logic.Modules;
using Framework.NetWork;
using Google.Protobuf;
using NetWork;
using Proto.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace HotFix
{
	public class ProtoNetWorkManager : INetWorkManager
	{
		public void SetData(string url, int version, NetWorkUsingType usingType)
		{
			this.m_url = url;
			this.m_version = version;
			this.m_netWorkUsingType = usingType;
		}

		public bool IsNetConnect()
		{
			return Application.internetReachability > 0;
		}

		public IEnumerator SendWebRequest(NetWorkSendData sendData)
		{
			if (sendData == null)
			{
				yield break;
			}
			if (sendData.m_message == null)
			{
				yield break;
			}
			byte[] body = this.ToByteArray(sendData.m_message);
			if (sendData.m_isShowMask)
			{
				this.ShowMask();
			}
			int sendID = (int)PackageFactory.GetMessageId(sendData.m_message);
			UnityWebRequest request = UnityWebRequest.Put(this.m_url, body);
			this.InitRequestHeader(request, sendData.m_message, body, sendData.m_key);
			yield return request.SendWebRequest();
			if (request.result != 1)
			{
				request = UnityWebRequest.Put(this.m_url, body);
				this.InitRequestHeader(request, sendData.m_message, body, sendData.m_key);
				yield return request.SendWebRequest();
			}
			if (request.result != 1)
			{
				request = UnityWebRequest.Put(this.m_url, body);
				this.InitRequestHeader(request, sendData.m_message, body, sendData.m_key);
				yield return request.SendWebRequest();
			}
			if (request.result != 1)
			{
				if (request.result == 3)
				{
					long responseCode = request.responseCode;
					if (responseCode > 0L && responseCode != 200L)
					{
						int num = (int)responseCode;
						if (sendData.m_isShowMask)
						{
							this.CloseMask();
						}
						if (sendData.m_callBack != null)
						{
							IMessage message = this.SetError(sendData.m_message, num);
							sendData.m_callBack(message);
						}
						this.ShowPostError(sendData, sendID, num, "171");
						yield break;
					}
				}
				if (sendData.m_isShowMask)
				{
					this.CloseMask();
				}
				if (sendData.m_callBack != null)
				{
					IMessage message2 = this.SetError(sendData.m_message, -101);
					sendData.m_callBack(message2);
				}
				if (sendData.m_isShowError)
				{
					this.ShowPostError(sendData, sendID, -101, "160");
				}
				yield break;
			}
			if (request.downloadedBytes == 0UL || request.downloadHandler == null || request.downloadHandler.data == null)
			{
				if (sendData.m_isShowMask)
				{
					this.CloseMask();
				}
				if (sendData.m_callBack != null)
				{
					IMessage message3 = this.SetError(sendData.m_message, -102);
					sendData.m_callBack(message3);
				}
				if (sendData.m_isShowError)
				{
					this.ShowGetError(sendData, sendID, -102);
				}
				yield break;
			}
			byte[] data = request.downloadHandler.data;
			ushort num2 = BitConverter.ToUInt16(data, 0);
			int num3 = BitConverter.ToInt32(data, 2);
			if (data.Length != num3 + 6)
			{
				HLog.LogError(string.Format("HttpNetManager.message length error ,id:{0} size:{1} length:{2}", num2, num3, data.Length));
				if (sendData.m_isShowMask)
				{
					this.CloseMask();
				}
				if (sendData.m_callBack != null)
				{
					IMessage message4 = this.SetError(sendData.m_message, -103);
					sendData.m_callBack(message4);
				}
				if (sendData.m_isShowError)
				{
					this.ShowGetError(sendData, (int)num2, -103);
				}
				yield break;
			}
			IMessage message5 = PackageFactory.CreateMessage(num2);
			this.m_tgaMessage = message5;
			if (message5 == null)
			{
				HLog.LogError("messageid 不存在 id:", num2.ToString());
				if (sendData.m_isShowMask)
				{
					this.CloseMask();
				}
				if (sendData.m_callBack != null)
				{
					IMessage message6 = this.SetError(sendData.m_message, -104);
					sendData.m_callBack(message6);
				}
				if (sendData.m_isShowError)
				{
					this.ShowGetError(sendData, (int)num2, -104);
				}
				yield break;
			}
			CodedInputStream codedInputStream = new CodedInputStream(data, 6, num3);
			message5.MergeFrom(codedInputStream);
			int code = this.GetCode(message5);
			if (code != 0)
			{
				if (message5 is ErrorMsg)
				{
					string text = "messageid 服务器错误 id:{0} ErrorCode:{1} request:{2}";
					object obj = num2;
					object obj2 = code;
					object obj3;
					if (sendData == null)
					{
						obj3 = null;
					}
					else
					{
						IMessage message7 = sendData.m_message;
						obj3 = ((message7 != null) ? message7.GetType().Name : null);
					}
					HLog.LogError(string.Format(text, obj, obj2, obj3));
					message5 = this.SetError(sendData.m_message, code);
				}
				if (sendData.m_isShowMask)
				{
					this.CloseMask();
				}
				if (sendData.m_callBack != null)
				{
					sendData.m_callBack(message5);
				}
				if (sendData.m_isShowError)
				{
					this.ShowCodeError(sendData, (int)num2, code);
				}
				codedInputStream.Dispose();
				yield break;
			}
			if (sendData.m_isShowMask)
			{
				this.CloseMask();
			}
			if (sendData.m_callBack != null)
			{
				sendData.m_callBack(message5);
			}
			codedInputStream.Dispose();
			this.m_tgaMessage = null;
			yield break;
		}

		private byte[] ToByteArray(IMessage message)
		{
			int num = message.CalculateSize();
			byte[] array = new byte[num + 4];
			Buffer.BlockCopy(BitConverter.GetBytes(PackageFactory.GetMessageId(message)), 0, array, 0, 2);
			Buffer.BlockCopy(BitConverter.GetBytes((ushort)num), 0, array, 2, 2);
			CodedOutputStream codedOutputStream = new CodedOutputStream(array, 4, num);
			message.WriteTo(codedOutputStream);
			return array;
		}

		private void InitRequestHeader(UnityWebRequest request, IMessage message, byte[] body, string guid)
		{
			request.method = "PUT";
			request.certificateHandler = new MyCertificate(this.m_netWorkUsingType, ProtoNetWorkManager.PUB_KEYS, ProtoNetWorkManager.CERT_HASHS);
			request.timeout = 8;
			request.SetRequestHeader("DxxVersion", this.m_version.ToString());
			long num = 0L;
			request.SetRequestHeader("DxxTime", num.ToString());
			if (!string.IsNullOrEmpty(guid))
			{
				request.SetRequestHeader("messageGuid", guid);
			}
			request.SetRequestHeader("DxxCheck", this.GetSHA256(num, body));
			request.SetRequestHeader("DxxType", PackageFactory.GetMessageId(message).ToString());
		}

		private string GetSHA256(long time, byte[] body)
		{
			this.sha_list.Clear();
			this.sha_list.AddRange(Encoding.Default.GetBytes("6F80DA08742462C12D7C9598B464E802"));
			this.sha_list.AddRange(Encoding.Default.GetBytes(time.ToString()));
			this.sha_list.AddRange(body);
			byte[] array = SHA256.Create().ComputeHash(this.sha_list.ToArray());
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		private IMessage SetError(IMessage message, int codeID)
		{
			IMessage message2 = PackageFactory.CreateMessage(PackageFactory.GetMessageId(message) + 1);
			PropertyInfo property = message2.GetType().GetProperty("Code");
			if (property != null)
			{
				property.SetValue(message2, codeID, null);
			}
			return message2;
		}

		private int GetCode(IMessage message)
		{
			PropertyInfo property = message.GetType().GetProperty("Code");
			int num = 0;
			if (property != null)
			{
				object value = property.GetValue(message, null);
				if (value is uint)
				{
					num = (int)((uint)value);
				}
				else if (value is int)
				{
					num = (int)value;
				}
				else
				{
					num = -105;
					HLog.LogError("code is a invalid type:{0}", value.GetType().ToString());
				}
			}
			return num;
		}

		public void HandleCommonData(IMessage msg)
		{
			if (msg == null)
			{
				return;
			}
			if (msg is CommonData)
			{
				CommonData commonData = msg as CommonData;
				if (commonData == null)
				{
					return;
				}
				NetworkUtils.HandleResponse_CommonDataInternal(commonData);
				GameTGAExtend.OnMessageCommonData(this.m_tgaMessage, commonData);
			}
		}

		private void ShowPostError(NetWorkSendData sendData, int sendID, int code, string languageId)
		{
			string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID(languageId), sendID, code);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("96");
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("163");
			DxxTools.UI.OpenPopCommon(text, delegate(int id)
			{
				if (id == 1)
				{
					GameApp.NetWork.Send(sendData.m_message, sendData.m_callBack, sendData.m_isShowMask, sendData.m_isCache, sendData.m_key, sendData.m_isShowError);
					return;
				}
				if (id == -1)
				{
					if (GameApp.View.IsOpened(ViewName.LoadingViewModule))
					{
						GameApp.View.CloseView(ViewName.LoadingViewModule, null);
					}
					GameApp.View.OpenView(ViewName.LoadingViewModule, null, 2, null, delegate(GameObject x)
					{
						GameApp.View.GetViewModule(ViewName.LoadingViewModule).PlayShow(delegate
						{
							GameApp.View.CloseAllView(new int[] { 101, 102, 106 });
							GameApp.State.ActiveState(StateName.LoginState);
						});
					});
				}
			}, string.Empty, infoByID, infoByID2, false, 2);
		}

		private void ShowGetError(NetWorkSendData sendData, int sendID, int code)
		{
			string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("161"), sendID, code);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("96");
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("163");
			DxxTools.UI.OpenPopCommon(text, delegate(int id)
			{
				if (id == 1)
				{
					GameApp.NetWork.Send(sendData.m_message, sendData.m_callBack, sendData.m_isShowMask, sendData.m_isCache, sendData.m_key, sendData.m_isShowError);
					return;
				}
				if (id == -1)
				{
					if (GameApp.View.IsOpened(ViewName.LoadingViewModule))
					{
						GameApp.View.CloseView(ViewName.LoadingViewModule, null);
					}
					GameApp.View.OpenView(ViewName.LoadingViewModule, null, 2, null, delegate(GameObject x)
					{
						GameApp.View.GetViewModule(ViewName.LoadingViewModule).PlayShow(delegate
						{
							GameApp.View.CloseAllView(new int[] { 101, 102, 106 });
							GameApp.State.ActiveState(StateName.LoginState);
						});
					});
				}
			}, string.Empty, infoByID, infoByID2, false, 2);
		}

		private void ShowCodeError(NetWorkSendData sendData, int receiveID, int code)
		{
			string text = string.Format("server_err_{0}", code);
			string text2 = Singleton<LanguageManager>.Instance.GetInfoByID(text);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = text;
			}
			if (code == 104 || code == 105)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("163");
				DxxTools.UI.OpenPopCommon(text2, delegate(int id)
				{
					if (id == 1)
					{
						if (GameApp.View.IsOpened(ViewName.LoadingViewModule))
						{
							GameApp.View.CloseView(ViewName.LoadingViewModule, null);
						}
						GameApp.View.OpenView(ViewName.LoadingViewModule, null, 2, null, delegate(GameObject x)
						{
							GameApp.View.GetViewModule(ViewName.LoadingViewModule).PlayShow(delegate
							{
								GameApp.View.CloseAllView(new int[] { 101, 102, 106 });
								GameApp.State.ActiveState(StateName.LoginState);
							});
						});
					}
				}, string.Empty, infoByID, string.Empty, false, 2);
				return;
			}
			if (code == 112 || code == 113)
			{
				string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("17");
				DxxTools.UI.OpenPopCommon(text2, delegate(int id)
				{
					if (id == 1)
					{
						GameApp.Quit();
					}
				}, string.Empty, infoByID2, string.Empty, false, 2);
				return;
			}
			if (code < 0)
			{
				string infoByID3 = Singleton<LanguageManager>.Instance.GetInfoByID("server_err_system", new object[] { code });
				string infoByID4 = Singleton<LanguageManager>.Instance.GetInfoByID("163");
				DxxTools.UI.OpenPopCommon(infoByID3, delegate(int id)
				{
					if (id == 1)
					{
						if (GameApp.View.IsOpened(ViewName.LoadingViewModule))
						{
							GameApp.View.CloseView(ViewName.LoadingViewModule, null);
						}
						GameApp.View.OpenView(ViewName.LoadingViewModule, null, 2, null, delegate(GameObject x)
						{
							GameApp.View.GetViewModule(ViewName.LoadingViewModule).PlayShow(delegate
							{
								GameApp.View.CloseAllView(new int[] { 101, 102, 106 });
								GameApp.State.ActiveState(StateName.LoginState);
							});
						});
					}
				}, string.Empty, infoByID4, string.Empty, false, 2);
				return;
			}
			GameApp.View.ShowStringTip(text2);
			if (code == 108)
			{
				GameApp.View.ShowItemNotEnoughTip(2, false);
			}
			if (code == 107)
			{
				GameApp.View.ShowItemNotEnoughTip(1, false);
			}
			if (code == 132)
			{
				GameApp.View.ShowItemNotEnoughTip(9, false);
			}
		}

		private void ShowMask()
		{
			if (!this.bMaskShow)
			{
				this.bMaskShow = true;
				GameApp.View.ShowNetLoading(true);
			}
		}

		private void CloseMask()
		{
			if (this.bMaskShow)
			{
				this.bMaskShow = false;
				GameApp.View.ShowNetLoading(false);
			}
		}

		private string m_url;

		private List<byte> sha_list = new List<byte>();

		private const string SHA_KEY = "6F80DA08742462C12D7C9598B464E802";

		private static string[] PUB_KEYS = new string[] { "3082010A0282010100BB867F6F970276FECC4D411C7E7EDA04B9472F81B3BA290803D40E1E610A7F701FF0179E6168F247E0B9BD63CA26C7D79699E56A2ED9B5CA1FA3C195F56056A1A03E77B97E84F998A3ED0621B9D0EBC723B5B91C9F253CDBDA89D06BE65CB29D2F31D4D516C109297E4759A8BE0EAD1A80145601C4645BC41023D51E06942D5E0053CEBFA319CC8D3FFBDFDEBCE2D386FFB44FF87F42C81C092E405C5F29672C2DC5EAB4FA78AAAFA0E04751D3E6E17B3B6EE53142D03671BCA273FD370D9DAA3FA9F43E3AE6D2B02B5B59B51ECF2121D807339C8BDA60CED8EE8543E7B7AF0C59C2AF99EEB3F48CBFEDD872EE01C1C13F7011BDFC08282D4EB781FC9E0C51010203010001" };

		private static string[] CERT_HASHS = new string[] { "FC43E02B63500F9355E1A0914A205F93F8BD3454" };

		private NetWorkUsingType m_netWorkUsingType;

		private int m_version;

		private bool bMaskShow;

		private IMessage m_tgaMessage;
	}
}
