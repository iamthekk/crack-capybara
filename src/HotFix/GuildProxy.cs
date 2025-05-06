using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using Dxx.Chat;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using Framework.SDKManager;
using Framework.SocketNet;
using Framework.ViewModule;
using Google.Protobuf;
using Google.Protobuf.Collections;
using HotFix;
using LocalModels;
using LocalModels.Bean;
using Newtonsoft.Json;
using Proto.Common;
using Proto.Guild;
using Proto.User;
using SuperScrollView;
using UnityEngine;

public class GuildProxy
{
	public const string UILoadingPath = "Assets/_Resources/Prefab/UI/Guild/Prefabs/UI/UIGuildLoading.prefab";

	public const string UIJoinedPath = "Assets/_Resources/Prefab/UI/Guild/UI_GuildHall.prefab";

	public const string UINotJoinedPath = "Assets/_Resources/Prefab/UI/Guild/UI_MainGuildInfo.prefab";

	public abstract class GuildProxy_BaseBehaviour : CustomBehaviour
	{
		protected GuildSDKManager SDK
		{
			get
			{
				return this.mGuildSDK;
			}
		}

		protected sealed override void OnInit()
		{
			this.mCachedGameObject = base.gameObject;
			this.mGuildSDK = GuildSDKManager.Instance;
			this.GuildUI_OnInit();
		}

		protected sealed override void OnDeInit()
		{
			this.GuildUI_OnUnInit();
			this.mCachedGameObject = null;
		}

		protected abstract void GuildUI_OnInit();

		protected abstract void GuildUI_OnUnInit();

		public void Show()
		{
			this.GuildUI_OnShow();
			base.SetActive(true);
		}

		public void Close()
		{
			this.GuildUI_OnClose();
			base.SetActive(false);
		}

		protected virtual void GuildUI_OnShow()
		{
		}

		protected virtual void GuildUI_OnClose()
		{
		}

		public sealed override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.GuildUI_OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected virtual void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public bool IsActive()
		{
			return this.mCachedGameObject != null && this.mCachedGameObject.activeInHierarchy;
		}

		private GuildSDKManager mGuildSDK;

		protected GameObject mCachedGameObject;
	}

	public abstract class GuildProxy_BaseView : BaseViewModule
	{
		protected GuildSDKManager SDK
		{
			get
			{
				return this.mGuildSDK;
			}
		}

		public sealed override void OnCreate(object data)
		{
			this.mGuildSDK = GuildSDKManager.Instance;
			this.OnViewCreate();
		}

		public sealed override void OnOpen(object data)
		{
			this.IsViewClose = false;
			this.OnViewOpen(data);
		}

		public sealed override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.OnViewUpdate(deltaTime, unscaledDeltaTime);
		}

		public sealed override void OnClose()
		{
			this.OnViewClose();
			this.IsViewClose = true;
		}

		public sealed override void OnDelete()
		{
			this.OnViewDelete();
		}

		public sealed override void RegisterEvents(EventSystemManager manager)
		{
			this.OnViewRegisterEvents(manager);
		}

		public sealed override void UnRegisterEvents(EventSystemManager manager)
		{
			this.OnViewUnRegisterEvents(manager);
		}

		protected virtual void OnViewCreate()
		{
		}

		protected virtual void OnViewOpen(object data)
		{
		}

		protected virtual void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected virtual void OnViewClose()
		{
		}

		protected virtual void OnViewDelete()
		{
		}

		protected virtual void OnViewRegisterEvents(EventSystemManager manager)
		{
		}

		protected virtual void OnViewUnRegisterEvents(EventSystemManager manager)
		{
		}

		public bool CheckIsViewOpen()
		{
			return !this.IsViewClose && base.gameObject != null && base.gameObject.activeSelf;
		}

		private GuildSDKManager mGuildSDK;

		protected bool IsViewClose;
	}

	public class Chat
	{
		public static ChatManager ChatMgr
		{
			get
			{
				return Singleton<ChatManager>.Instance;
			}
		}

		private static ChatGroupData GetGuildChatGroupData()
		{
			string guildChatGroupId = ChatProxy.GuildChat.GetGuildChatGroupId();
			if (string.IsNullOrEmpty(guildChatGroupId))
			{
				return null;
			}
			ChatGroupData group = Singleton<ChatManager>.Instance.GetGroup(SocketGroupType.GUILD, guildChatGroupId);
			if (group == null)
			{
				return null;
			}
			return group;
		}

		internal static void OnRecvGuildChatRecords(ChatGroupData group, GuildGetMessageRecordsResponse resp)
		{
			if (group != null)
			{
				List<GuildPushMessageDto> list = new List<GuildPushMessageDto>();
				if (resp.MessageRecords.Count > 0)
				{
					list.AddRange(resp.MessageRecords);
				}
				List<ChatData> list2 = new List<ChatData>();
				for (int i = 0; i < list.Count; i++)
				{
					GuildPushMessageDto guildPushMessageDto = list[i];
					ChatData chatData = GuildProxy.Chat.TryGetChatData((int)guildPushMessageDto.MessageType, guildPushMessageDto.MessageContent);
					if (chatData != null)
					{
						list2.Add(chatData);
					}
					else if (guildPushMessageDto.MessageType == 201U)
					{
						ChatData chatData2 = ChatManager.JsonStringToChatData(guildPushMessageDto.MessageContent);
						if (chatData2 != null)
						{
							list2.Add(chatData2);
						}
					}
				}
				if (list2.Count > 0)
				{
					group.AddPage();
					group.AddNewChatList(list2);
				}
			}
		}

		internal static ChatData TryGetChatData(int type, string json)
		{
			if (type >= 101 && type <= 199)
			{
				GuildSocketPushJsonData.GuildBaseJsonData guildBaseJsonData = GuildSocketPushJsonData.TryGetData(type, json);
				if (guildBaseJsonData != null && guildBaseJsonData.timestamp > 0L && guildBaseJsonData.msgId > 0)
				{
					string text = guildBaseJsonData.MakeLogString();
					if (!string.IsNullOrEmpty(text))
					{
						ChatData chatData = new ChatData();
						chatData.msgId = (long)guildBaseJsonData.msgId;
						chatData.ShowKind = ChatShowKind.SystemTips;
						chatData.timestamp = guildBaseJsonData.timestamp;
						chatData.chatContent = text;
						if (type == 102)
						{
							chatData.ContentList = text.Split(';', StringSplitOptions.None);
							chatData.ShowKind = ChatShowKind.MultipleSystemTips;
						}
						return chatData;
					}
				}
			}
			return null;
		}

		public static ChatGroupData GetGuildChatGroup(string groupId)
		{
			return Singleton<ChatManager>.Instance.GetGroup(SocketGroupType.GUILD, groupId);
		}

		public static void CheckGetChatHistoryWhenEmpty()
		{
			ChatGroupData guildChatGroupData = GuildProxy.Chat.GetGuildChatGroupData();
			if (guildChatGroupData == null)
			{
				return;
			}
			if (guildChatGroupData.ChatMessageList.Count > 0)
			{
				return;
			}
			ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
			if (socketNetProxy == null)
			{
				return;
			}
			socketNetProxy.GetMessageHistory(0L, SocketGroupType.GUILD, GuildSDKManager.Instance.GuildInfo.IMGroupId, null);
		}
	}

	public class GameEvent
	{
		public static void AddReceiverInt(int eventID, HandlerEvent callback)
		{
			GameApp.Event.RegisterEvent(eventID, callback);
		}

		public static void RemoveReceiverInt(int eventID, HandlerEvent callback)
		{
			GameApp.Event.UnRegisterEvent(eventID, callback);
		}

		public static void AddReceiver(LocalMessageName eventID, HandlerEvent callback)
		{
			GameApp.Event.RegisterEvent(eventID, callback);
		}

		public static void RemoveReceiver(LocalMessageName eventID, HandlerEvent callback)
		{
			GameApp.Event.UnRegisterEvent(eventID, callback);
		}

		public static void PushEvent(LocalMessageName eventID)
		{
			GameApp.Event.DispatchNow(null, eventID, null);
		}

		public static void PushEventArg(LocalMessageName eventID, BaseEventArgs arg0)
		{
			GameApp.Event.DispatchNow(null, eventID, arg0);
		}
	}

	public class JSON
	{
		public static T ToObject<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}

	public class Language
	{
		public static LanguageManager Lan
		{
			get
			{
				return Singleton<LanguageManager>.Instance;
			}
		}

		public static int GetCurrentLanguage()
		{
			return Singleton<LanguageManager>.Instance.GameLanguage;
		}

		public static string GetInfoByID_LogError(int id)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(id);
		}

		public static string GetInfoByID1_LogError(int id, object obj)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(id, new object[] { obj });
		}

		public static string GetInfoByID2_LogError(int id, object obj1, object obj2)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(id, new object[] { obj1, obj2 });
		}

		public static string GetInfoByID3_LogError(int id, object obj1, object obj2, object obj3)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(id, new object[] { obj1, obj2, obj3 });
		}

		public static string GetInfoByID4_LogError(int id, object obj1, object obj2, object obj3, object obj4)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(id, new object[] { obj1, obj2, obj3, obj4 });
		}

		public static string GetInfoByID(string id)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(id);
		}

		public static string GetInfoByID1(string id, object obj1)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(id, new object[] { obj1 });
		}

		public static string GetInfoByID2(string id, object obj1, object obj2)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(id, new object[] { obj1, obj2 });
		}

		public static string GetInfoByID3(string id, object obj1, object obj2, object obj3)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(id, new object[] { obj1, obj2, obj3 });
		}

		public static string GetInfoByID4(string id, object obj1, object obj2, object obj3, object obj4)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(id, new object[] { obj1, obj2, obj3, obj4 });
		}

		public static string GetLanguageNameString(LanguageType type)
		{
			return FrameworkExpand.GetDataModule<LanguageDataModule>(GameApp.Data, 1).GetLanguageName(type);
		}

		public static string GetCurrentLanguageNameString()
		{
			return GuildProxy.Language.GetInfoByID("44");
		}

		public static int CheckTextLength(string text)
		{
			return DxxTools.Char.GetLength(text);
		}

		public static int CheckTextNameLength(string text)
		{
			return DxxTools.Char.GetNameLength(text);
		}

		public static char CheckValidateInput(string text, int charIndex, char addedChar, int maxLen)
		{
			if (!DxxTools.Char.CheckLength(text, addedChar, maxLen))
			{
				return '\0';
			}
			if (DxxTools.Char.CheckEmoji(addedChar))
			{
				return '\0';
			}
			return addedChar;
		}

		public static string FormatNumber(long num)
		{
			return DxxTools.FormatNumber(num);
		}

		public static string GetLongNumberTime(long time)
		{
			if (time < 0L)
			{
				HLog.LogError(string.Format("GetLongNumberTime(long time) time[{0}] is error", time));
				return "00:00:00";
			}
			if ((double)time > TimeSpan.MaxValue.TotalSeconds)
			{
				HLog.LogError(string.Format("GetLongNumberTime(long time) time[{0}] is too bigger than {1}", time, TimeSpan.MaxValue.TotalSeconds));
				return "00:00:00";
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			int seconds = timeSpan.Seconds;
			int minutes = timeSpan.Minutes;
			int num = timeSpan.Hours;
			if (timeSpan.Days > 0)
			{
				num += timeSpan.Hours * 24;
			}
			string text = ((num > 10) ? num.ToString() : num.ToString("D2"));
			return string.Concat(new string[]
			{
				text,
				":",
				minutes.ToString("D2"),
				":",
				seconds.ToString("D2")
			});
		}

		public static string GetTime(long time)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			if (timeSpan.Days > 0)
			{
				return GuildProxy.Language.GetInfoByID2_LogError(21, timeSpan.Days, timeSpan.Hours);
			}
			if (timeSpan.Hours > 0)
			{
				return GuildProxy.Language.GetInfoByID2_LogError(22, timeSpan.Hours, timeSpan.Minutes);
			}
			if (timeSpan.Minutes > 0)
			{
				return GuildProxy.Language.GetInfoByID2_LogError(23, timeSpan.Minutes, timeSpan.Seconds);
			}
			return GuildProxy.Language.GetInfoByID1("24", timeSpan.Seconds);
		}

		public static string GetGoTime(long time, bool isShowSecond = false)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			if (timeSpan.Days > 0)
			{
				return GuildProxy.Language.GetInfoByID1("25", timeSpan.Days);
			}
			if (timeSpan.Hours > 0)
			{
				return GuildProxy.Language.GetInfoByID1("26", timeSpan.Hours);
			}
			if (timeSpan.Minutes > 0)
			{
				return GuildProxy.Language.GetInfoByID1("27", timeSpan.Minutes);
			}
			if (isShowSecond)
			{
				return GuildProxy.Language.GetInfoByID1("28", timeSpan.Seconds);
			}
			return GuildProxy.Language.GetInfoByID("400133");
		}

		public static string GetRaceEndTime(long time)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			if (timeSpan.Days > 0)
			{
				return GuildProxy.Language.GetInfoByID3_LogError(400442, timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
			}
			if (timeSpan.Hours > 0)
			{
				return GuildProxy.Language.GetInfoByID2_LogError(400443, timeSpan.Hours, timeSpan.Minutes);
			}
			if (timeSpan.Minutes > 0)
			{
				return GuildProxy.Language.GetInfoByID1_LogError(400444, timeSpan.Minutes);
			}
			if (timeSpan.Seconds > 0)
			{
				return GuildProxy.Language.GetInfoByID1_LogError(400444, 1);
			}
			return GuildProxy.Language.GetInfoByID_LogError(400445);
		}

		public static string GetRaceDanName(int racedan)
		{
			return GuildProxy.Language.GetInfoByID_LogError(racedan + 400460);
		}

		public static string GetJoinTypeShow(GuildJoinKind kind)
		{
			if (kind == GuildJoinKind.Conditional)
			{
				return GuildProxy.Language.GetInfoByID("400027");
			}
			return GuildProxy.Language.GetInfoByID("400028");
		}
	}

	public enum AnimationType
	{
		IDLE,
		SHOW
	}

	public class Model
	{
		public static GameObject CreateModel(int modelId, GameObject customObj, GuildProxy.AnimationType ani)
		{
			return null;
		}
	}

	public class Net
	{
		public static ISocketNet SocketNet
		{
			get
			{
				return GameApp.SocketNet;
			}
		}

		public static ISocketGameLogicProxy SocketNetProxy
		{
			get
			{
				return GameApp.SocketNet.GameProxy as ISocketGameLogicProxy;
			}
		}

		public static bool IsGuildError(int code)
		{
			return (code >= 300000 && code <= 309999) || (code >= 310000 && code <= 319999) || (code >= 320000 && code <= 329999);
		}

		private static int TryGetMessageCode(IMessage msg)
		{
			int num = 0;
			if (msg == null)
			{
				return num;
			}
			PropertyInfo property = msg.GetType().GetProperty("Code");
			if (property != null)
			{
				object value = property.GetValue(msg, null);
				if (value is uint)
				{
					num = (int)((uint)value);
				}
				else if (value is int)
				{
					num = (int)value;
				}
			}
			return num;
		}

		public static void TrySendMessage(IMessage req, Action<IMessage> callBack, bool isShowMask, bool isCache, bool isShowError = true)
		{
			GameApp.NetWork.Send(req, delegate(IMessage response)
			{
				GuildProxy.Net.IsGuildError(GuildProxy.Net.TryGetMessageCode(response));
				Action<IMessage> callBack2 = callBack;
				if (callBack2 == null)
				{
					return;
				}
				callBack2(response);
			}, isShowMask, isCache, string.Empty, isShowError);
		}

		public static CommonParams GetCommonParams()
		{
			return NetworkUtils.GetCommonParams();
		}

		public static long ServerTime()
		{
			return GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerUTC + GuildProxy.Net.mDebugServerTime;
		}

		public const int Guild_ErrorCodeStart = 300000;

		public const int Guild_ErrorCodeEnd = 309999;

		public const int SOCKET_ErrorCodeStart = 310000;

		public const int SOCKET_ErrorCodeEnd = 319999;

		public const int CHAT_ErrorCodeStart = 320000;

		public const int CHAT_ErrorCodeEnd = 329999;

		public static long mDebugServerTime;
	}

	public class RedPoint
	{
		public static void CalcRedPoint(string redpath, bool calcsub)
		{
			RedPointController.Instance.ReCalc(redpath, calcsub);
		}

		public static void ClickRedPoint(string redpath)
		{
			RedPointController.Instance.ClickRecord(redpath);
		}
	}

	public class Resources
	{
		public static void SetDxxImage(CustomImage image, int atlasid, string icon)
		{
			if (image != null)
			{
				image.SetImage(GameApp.Table.GetAtlasPath(atlasid), icon);
			}
		}

		public static void SetDxxImageSingle(CustomImage image, string imagepath)
		{
			if (image != null)
			{
				image.SetImageSingle(imagepath);
			}
		}

		public static void SetDxxSprite(CustomImage image, Sprite sprite)
		{
			if (image != null)
			{
				image.SetSprite(sprite);
			}
		}

		public static int GetGuildAtlasID()
		{
			return 50;
		}
	}

	public class GuildPreLoad
	{
		private string MakePath(string path)
		{
			return "Assets/_Resources/Prefab/UI/Guild/" + path + ".prefab";
		}

		public Task LoadGameObject(string path)
		{
			GuildProxy.GuildPreLoad.<LoadGameObject>d__2 <LoadGameObject>d__;
			<LoadGameObject>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadGameObject>d__.<>4__this = this;
			<LoadGameObject>d__.path = path;
			<LoadGameObject>d__.<>1__state = -1;
			<LoadGameObject>d__.<>t__builder.Start<GuildProxy.GuildPreLoad.<LoadGameObject>d__2>(ref <LoadGameObject>d__);
			return <LoadGameObject>d__.<>t__builder.Task;
		}

		public GameObject CreateGameObject(string path, Transform tf)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			path = this.MakePath(path);
			GameObject gameObject;
			if (!this.PrefabDic.TryGetValue(path, out gameObject))
			{
				return null;
			}
			if (gameObject == null)
			{
				return null;
			}
			GameObject gameObject2;
			if (tf != null)
			{
				gameObject2 = Object.Instantiate<GameObject>(gameObject, tf);
			}
			else
			{
				gameObject2 = Object.Instantiate<GameObject>(gameObject);
			}
			return gameObject2;
		}

		public void ReleaseAll()
		{
			foreach (GameObject gameObject in this.PrefabDic.Values)
			{
				if (!(gameObject == null))
				{
					GameApp.Resources.Release<GameObject>(gameObject);
				}
			}
		}

		public Dictionary<string, GameObject> PrefabDic = new Dictionary<string, GameObject>();
	}

	public class Table
	{
		public static int NAME_LENGTH_MIN
		{
			get
			{
				return GuildProxy.Table.GetGuildConstTable(105).TypeInt;
			}
		}

		public static int NAME_LENGTH_MAX
		{
			get
			{
				return GuildProxy.Table.GetGuildConstTable(106).TypeInt;
			}
		}

		public static int SLOGAN_LENGTH
		{
			get
			{
				return GuildProxy.Table.GetGuildConstTable(107).TypeInt;
			}
		}

		public static int NOTIC_LENGTH
		{
			get
			{
				return GuildProxy.Table.GetGuildConstTable(107).TypeInt;
			}
		}

		public static LocalModelManager TableMgr
		{
			get
			{
				return GameApp.Table.GetManager();
			}
		}

		public static Guild_guildStyle GetGuildStyleTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuild_guildStyleModelInstance().GetElementById(id);
		}

		public static List<Guild_guildStyle> GetGuildStyleTableAll()
		{
			IList<Guild_guildStyle> allElements = GuildProxy.Table.TableMgr.GetGuild_guildStyleModelInstance().GetAllElements();
			List<Guild_guildStyle> list = new List<Guild_guildStyle>(allElements.Count);
			list.AddRange(allElements);
			return list;
		}

		public static Guild_guildLevel GetGuildLevelTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuild_guildLevelModelInstance().GetElementById(id);
		}

		public static List<Guild_guildLevel> GetGuildLevelTableAll()
		{
			IList<Guild_guildLevel> allElements = GuildProxy.Table.TableMgr.GetGuild_guildLevelModelInstance().GetAllElements();
			List<Guild_guildLevel> list = new List<Guild_guildLevel>(allElements.Count);
			list.AddRange(allElements);
			return list;
		}

		public static int GetMaxGuildLevel()
		{
			IList<Guild_guildLevel> allElements = GuildProxy.Table.TableMgr.GetGuild_guildLevelModelInstance().GetAllElements();
			int num = 0;
			for (int i = 0; i < allElements.Count; i++)
			{
				if (num < allElements[i].ID)
				{
					num = allElements[i].ID;
				}
			}
			return num;
		}

		public static Guild_guildConst GetGuildConstTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuild_guildConstModelInstance().GetElementById(id);
		}

		public static Guild_guildTask GetGuildTaskTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuild_guildTaskModelInstance().GetElementById(id);
		}

		public static Guild_guildEvent GetGuildEventTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuild_guildEventModelInstance().GetElementById(id);
		}

		public static List<Guild_guildEvent> GetGuildEventTableAll()
		{
			IList<Guild_guildEvent> allElements = GuildProxy.Table.TableMgr.GetGuild_guildEventModelInstance().GetAllElements();
			List<Guild_guildEvent> list = new List<Guild_guildEvent>(allElements.Count);
			list.AddRange(allElements);
			return list;
		}

		public static Item_Item GetItemTable(int id)
		{
			return GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(id);
		}

		public static string GetItemShowName(int id)
		{
			Item_Item itemTable = GuildProxy.Table.GetItemTable(id);
			if (itemTable == null)
			{
				return "";
			}
			return GuildProxy.Language.GetInfoByID(itemTable.nameID);
		}

		public static GuildItemData StringToGuildItemData(string itemstr)
		{
			if (string.IsNullOrEmpty(itemstr))
			{
				return null;
			}
			string[] array = itemstr.Split(',', StringSplitOptions.None);
			if (array.Length < 2)
			{
				return null;
			}
			GuildItemData guildItemData = new GuildItemData();
			if (!int.TryParse(array[0], out guildItemData.id))
			{
				guildItemData.id = 0;
			}
			if (!int.TryParse(array[1], out guildItemData.count))
			{
				guildItemData.count = 0;
			}
			return guildItemData;
		}

		public static Guild_guildShop GetGuildShopTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuild_guildShopModelInstance().GetElementById(id);
		}

		public static GuildBOSS_guildBossTask GetGuildBossTaskTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuildBOSS_guildBossTask(id);
		}

		public static GuildBOSS_guildBossBox GetGuildBossBoxTable(int id)
		{
			return null;
		}

		public static List<GuildBOSS_guildBossBox> GetAllGuildBossDamageTables(int bossid)
		{
			IList<GuildBOSS_guildBossBox> allElements = GuildProxy.Table.TableMgr.GetGuildBOSS_guildBossBoxModelInstance().GetAllElements();
			List<GuildBOSS_guildBossBox> list = new List<GuildBOSS_guildBossBox>();
			for (int i = 0; i < allElements.Count; i++)
			{
				GuildBOSS_guildBossBox guildBOSS_guildBossBox = allElements[i];
				if (guildBOSS_guildBossBox.BossId == bossid)
				{
					list.Add(guildBOSS_guildBossBox);
				}
			}
			list.Sort((GuildBOSS_guildBossBox x, GuildBOSS_guildBossBox y) => x.Damage.CompareTo(y.Damage));
			return list;
		}

		public static GuildBOSS_guildBoss GetGuildBossTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuildBOSS_guildBossModelInstance().GetElementById(id);
		}

		public static GuildBOSS_guildSubection GetGuildBossDanTable(int danId)
		{
			return GuildProxy.Table.TableMgr.GetGuildBOSS_guildSubection(danId);
		}

		public static GuildBOSS_guildBossStep GetGuildBossStepTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuildBOSS_guildBossStep(id);
		}

		public static Quality_guildBossQuality GetGuildBossStepQualityTable(int id)
		{
			GuildBOSS_guildBossStep guildBossStepTable = GuildProxy.Table.GetGuildBossStepTable(id);
			if (guildBossStepTable != null)
			{
				return GuildProxy.Table.TableMgr.GetQuality_guildBossQuality(guildBossStepTable.BossGradeQuality);
			}
			return null;
		}

		public static string GetGuildBossNameID(int id)
		{
			return GuildProxy.Table.GetGuildBossTable(id).NameID;
		}

		public static int GetGuildBossModelID(int id)
		{
			return 1001;
		}

		public static List<Guild_guildPower> GetGuildPowerTableAll()
		{
			IList<Guild_guildPower> allElements = GuildProxy.Table.TableMgr.GetGuild_guildPowerModelInstance().GetAllElements();
			List<Guild_guildPower> list = new List<Guild_guildPower>(allElements.Count);
			list.AddRange(allElements);
			return list;
		}

		public static GuildRace_baseRace GetRaceBaseTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuildRace_baseRaceModelInstance().GetElementById(id);
		}

		public static GuildRace_baseRace GetRaceBaseTable(GuildRaceBattlePosition pos)
		{
			return GuildProxy.Table.TableMgr.GetGuildRace_baseRaceModelInstance().GetElementById((int)pos);
		}

		public static GuildRace_opentime GetRaceOpenTimeTable(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuildRace_opentimeModelInstance().GetElementById(id);
		}

		public static int GetRaceSeasonTotalTimeMinute(int id)
		{
			GuildRace_opentime raceOpenTimeTable = GuildProxy.Table.GetRaceOpenTimeTable(id);
			return raceOpenTimeTable.stage1 + raceOpenTimeTable.stage2 + raceOpenTimeTable.stage3 + raceOpenTimeTable.stage4 + raceOpenTimeTable.stage5 + raceOpenTimeTable.stage6 + raceOpenTimeTable.stage7 + raceOpenTimeTable.stage8 + raceOpenTimeTable.stage9 + raceOpenTimeTable.stage10 + raceOpenTimeTable.stage11 + raceOpenTimeTable.stage12 + raceOpenTimeTable.stage13 + raceOpenTimeTable.stage14;
		}

		public static GuildRace_level GetRaceLevelTab(int id)
		{
			return GuildProxy.Table.TableMgr.GetGuildRace_levelModelInstance().GetElementById(id);
		}

		public static List<GuildRace_level> GetRaceLevelAllTab()
		{
			IList<GuildRace_level> allElements = GuildProxy.Table.TableMgr.GetGuildRace_levelModelInstance().GetAllElements();
			List<GuildRace_level> list = new List<GuildRace_level>(allElements.Count);
			list.AddRange(allElements);
			return list;
		}

		public static GuildRaceBattlePosition GuildRaceUserIndexToPosition(int index)
		{
			GuildRace_level raceLevelTab = GuildProxy.Table.GetRaceLevelTab(GuildProxy.Table.GuildRaceDan);
			if (raceLevelTab == null)
			{
				return GuildRaceBattlePosition.None;
			}
			if (index == 0)
			{
				return GuildRaceBattlePosition.None;
			}
			int num = index - raceLevelTab.generalNum;
			if (num <= 0)
			{
				return GuildRaceBattlePosition.General;
			}
			num -= raceLevelTab.eliteNum;
			if (num <= 0)
			{
				return GuildRaceBattlePosition.Elite;
			}
			num -= raceLevelTab.warriorNum;
			if (num <= 0)
			{
				return GuildRaceBattlePosition.Warrior;
			}
			return GuildRaceBattlePosition.None;
		}

		public static int GuildRaceMaxInPositionCount()
		{
			GuildRace_level raceLevelTab = GuildProxy.Table.GetRaceLevelTab(GuildProxy.Table.GuildRaceDan);
			if (raceLevelTab == null)
			{
				return 0;
			}
			return raceLevelTab.warriorNum + raceLevelTab.eliteNum + raceLevelTab.generalNum;
		}

		public static List<GuildBOSS_guildBossSeasonReward> OnGetSeasonRewardByGuildDan(int curDan)
		{
			List<GuildBOSS_guildBossSeasonReward> list = new List<GuildBOSS_guildBossSeasonReward>();
			IList<GuildBOSS_guildBossSeasonReward> guildBOSS_guildBossSeasonRewardElements = GameApp.Table.GetManager().GetGuildBOSS_guildBossSeasonRewardElements();
			for (int i = 0; i < guildBOSS_guildBossSeasonRewardElements.Count; i++)
			{
				if (guildBOSS_guildBossSeasonRewardElements[i].levelID == curDan)
				{
					list.Add(guildBOSS_guildBossSeasonRewardElements[i]);
				}
			}
			return list;
		}

		public static List<GuildBOSS_rankRewards> OnGetGuildBossRankReward(int bossId)
		{
			List<GuildBOSS_rankRewards> list = new List<GuildBOSS_rankRewards>();
			IList<GuildBOSS_rankRewards> guildBOSS_rankRewardsElements = GameApp.Table.GetManager().GetGuildBOSS_rankRewardsElements();
			for (int i = 0; i < guildBOSS_rankRewardsElements.Count; i++)
			{
				if (guildBOSS_rankRewardsElements[i].BossId == bossId)
				{
					list.Add(guildBOSS_rankRewardsElements[i]);
				}
			}
			return list;
		}

		public static GuildBOSS_rankRewards OnGetSelfGuildBossRankReward(int rank)
		{
			IList<GuildBOSS_rankRewards> guildBOSS_rankRewardsElements = GameApp.Table.GetManager().GetGuildBOSS_rankRewardsElements();
			for (int i = 0; i < guildBOSS_rankRewardsElements.Count; i++)
			{
				if (rank >= guildBOSS_rankRewardsElements[i].RankRange[0] && rank <= guildBOSS_rankRewardsElements[i].RankRange[1])
				{
					return guildBOSS_rankRewardsElements[i];
				}
			}
			return null;
		}

		public static int GuildRaceDan = 1;
	}

	public class TGA
	{
		private static SDKManager.SDKTGA _TGA
		{
			get
			{
				return GameApp.SDK.Analyze;
			}
		}

		private static GuildSDKManager SDK
		{
			get
			{
				return GuildSDKManager.Instance;
			}
		}

		private static string GuildID
		{
			get
			{
				GuildSDKManager sdk = GuildProxy.TGA.SDK;
				if (sdk != null && sdk.GuildInfo.HasGuild)
				{
					return sdk.GuildInfo.GuildData.GuildID;
				}
				return "";
			}
		}

		private static int GuildLevel
		{
			get
			{
				GuildSDKManager sdk = GuildProxy.TGA.SDK;
				if (sdk != null && sdk.GuildInfo.HasGuild)
				{
					return sdk.GuildInfo.GuildData.GuildLevel;
				}
				return 0;
			}
		}

		public static void OnGuildCreate()
		{
		}

		public static void OnGuildJoin(int joinkind, long userid, long applyuserid)
		{
		}

		public static void OnGuildQuit(string guildid, int guildlevel, int permission)
		{
		}

		public static void OnGuildKicked(int userid, int userpermission, int mypermission)
		{
		}

		public static void OnGuildJobChange(int userid, int oldpermission, int newpermission, int mypermission)
		{
		}

		public static void OnGuildSign(int signcount, int costid, int costcount)
		{
		}

		public static void OnGuildTaskGetRewards(int taskid, string rewards)
		{
		}

		public static void OnGuildTaskRefresh(int taskid)
		{
		}

		public static void OnGuildRaceGuildApply(int type)
		{
		}

		public static void OnGuildRaceUserApply()
		{
		}
	}

	public class UI
	{
		public static void SetNetShow(bool b)
		{
			GameApp.View.ShowNetLoading(b);
		}

		public static void OpenGuildLog()
		{
			GameApp.View.OpenView(ViewName.GuildLog, null, 1, null, null);
		}

		public static void CloseGuildLog()
		{
			GameApp.View.CloseView(ViewName.GuildLog, null);
		}

		public static void OpenMainGuild()
		{
			GameApp.View.OpenView(ViewName.MainGuildViewModule, null, 1, null, null);
		}

		public static void CloseMainGuild()
		{
			GameApp.View.CloseView(ViewName.MainGuildViewModule, null);
		}

		public static void OpenMainGuildInfo()
		{
			GameApp.View.OpenView(ViewName.MainGuildInfo, null, 1, null, null);
		}

		public static void CloseMainGuildInfo()
		{
			GameApp.View.CloseView(ViewName.MainGuildInfo, null);
		}

		public static void OpenUIGuildContribute()
		{
			GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse resp)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.GuildContributeViewModule, null, 1, null, null);
				}
			});
		}

		public static void OpenUILanguage(LanguageType language, Action<LanguageType> callback)
		{
			LanguageChooseOpenData languageChooseOpenData = new LanguageChooseOpenData();
			languageChooseOpenData.DefaultLanguage = language;
			languageChooseOpenData.Callback = callback;
			GameApp.View.OpenView(ViewName.LanguageChooseViewModule, languageChooseOpenData, 1, null, null);
		}

		public static void CloseUILanguage()
		{
			GameApp.View.CloseView(ViewName.LanguageChooseViewModule, null);
		}

		public static void OpenUIGuildIconSet(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildIconSetViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildIconSet(object data = null)
		{
			GameApp.View.CloseView(ViewName.GuildIconSetViewModule, null);
		}

		public static void OpenUIGuildDetailInfo(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildDetailInfoViewModule, data, 1, null, null);
		}

		public static bool IsCanShowLevelUp()
		{
			GuildUIDataModule module = GuildSDKManager.Instance.GetModule<GuildUIDataModule>();
			return module == null || module.LevelUP.CanShowGuildLevelUp;
		}

		public static void OpenUIGuildLevelUp(object data = null)
		{
			int guildLevel = GuildSDKManager.Instance.GuildInfo.GuildData.GuildLevel;
			GuildUIDataModule module = GuildSDKManager.Instance.GetModule<GuildUIDataModule>();
			if (module == null || module.LevelUP.HasShowLevelUp(guildLevel))
			{
				return;
			}
			GameApp.View.OpenView(ViewName.GuildLevelUpViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildLevelUp()
		{
			GameApp.View.CloseView(ViewName.GuildLevelUpViewModule, null);
		}

		public static void OpenUIGuildCheckPop(string guildId, ulong guildId_long = 0UL)
		{
			if (string.IsNullOrEmpty(guildId))
			{
				guildId = guildId_long.ToString();
			}
			GuildNetUtil.Guild.DoRequest_GetGuildDetailInfo(guildId, delegate(bool result, GuildGetDetailResponse response)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.GuildCheckPopViewModule, response.GuildDetailInfoDto.ToGuildDetailData(), 1, null, null);
				}
			}, true);
		}

		public static void OpenUIGuildList(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildListViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildList()
		{
			GameApp.View.CloseView(ViewName.GuildListViewModule, null);
		}

		public static void OpenUIGuildManage(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildManageViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildManage()
		{
			GameApp.View.CloseView(ViewName.GuildManageViewModule, null);
		}

		public static void OpenUIGuildManageMember(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildManageMemberViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildManageMember()
		{
			if (GameApp.View.IsOpened(ViewName.GuildManageMemberViewModule))
			{
				GameApp.View.CloseView(ViewName.GuildManageMemberViewModule, null);
			}
		}

		public static void OpenUIGuildBoss(object data = null, Action<GameObject> loadcallback = null, Action<GameObject> opencallback = null)
		{
			GuildNetUtil.Guild.DoRequest_GetGuildBossInfo(delegate(bool result, GuildBossGetInfoResponse resp)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.GuildBossViewModule, data, 1, loadcallback, opencallback);
				}
			}, true);
		}

		public static void CloseUIGuildBoss()
		{
			GameApp.View.CloseView(ViewName.GuildBossViewModule, null);
		}

		public static void OpenUIGuildRace(object data = null, Action<GameObject> loadcallback = null, Action<GameObject> opencallback = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceMainViewModule, data, 1, loadcallback, opencallback);
		}

		public static void CloseUIGuildRace()
		{
			GameApp.View.CloseView(ViewName.GuildRaceMainViewModule, null);
		}

		public static void OpenUIGuildInfoPop(object data = null)
		{
			GuildNetUtil.Guild.DoRequest_GetGuildDetailInfo(GuildSDKManager.Instance.GuildInfo.GuildID, delegate(bool result, GuildGetDetailResponse response)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.GuildInfoPopViewModule, data, 1, null, null);
				}
			}, true);
		}

		public static void CloseUIGuildInfoPop()
		{
			GameApp.View.CloseView(ViewName.GuildInfoPopViewModule, null);
		}

		public static void OpenUIGuildAnnouncementModify(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildAnnouncementModifyViewModule, data, 1, null, null);
		}

		public static void OpenUIGuildLevelInfo(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildLevelInfoViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildLevelInfo()
		{
			GameApp.View.CloseView(ViewName.GuildLevelInfoViewModule, null);
		}

		public static void OpenUIGuildApplyJoin(object data = null)
		{
			GuildNetUtil.Guild.DoRequest_GetApplyJoinGuildUserList(delegate(bool result, GuildGetApplyListResponse resp)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.GuildApplyJoinViewModule, data, 1, null, null);
				}
			}, true);
		}

		public static void OpenUIGuildBossBuyCount(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildBossBuyCountViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildBossBuyCount()
		{
			GameApp.View.CloseView(ViewName.GuildBossBuyCountViewModule, null);
		}

		public static void OpenUIGuildBossChallengeRecord(object data = null)
		{
			GuildNetUtil.Guild.DoRequest_GetGuildBossChallengeList(delegate(bool result, GuildBossGetChallengeListResponse response)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.GuildBossChallengeViewModule, data, 1, null, null);
				}
			});
		}

		public static void CloseUIGuildBossChallengeRecord()
		{
			GameApp.View.CloseView(ViewName.GuildBossChallengeViewModule, null);
		}

		public static void OpenUIGuildBossGuildRank(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildBossGuildRankViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildBossGuildRank()
		{
			GameApp.View.CloseView(ViewName.GuildBossGuildRankViewModule, null);
		}

		public static void OpenUIGuildBossTask(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildBossTaskViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildBossTask()
		{
			GameApp.View.CloseView(ViewName.GuildBossTaskViewModule, null);
		}

		public static void OpenUIGuildBossUpDan(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildBossUpDanViewModule, data, 1, null, null);
		}

		public static void CloseUIGuildBossUpDan()
		{
			GameApp.View.CloseView(ViewName.GuildBossUpDanViewModule, null);
		}

		public static void OpenGuildHall(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildHallViewModule, data, 1, null, null);
		}

		public static void OpenGuildShop()
		{
			GameApp.View.OpenView(ViewName.GuildShopViewModule, new MainShopJumpTabData(MainShopType.GuildShop), 1, null, null);
		}

		public static void OpenBlackMarket()
		{
			GameApp.View.OpenView(ViewName.GuildShopViewModule, new MainShopJumpTabData(MainShopType.BlackMarket), 1, null, null);
		}

		public static void OpenGuildActivity(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildActivityViewModule, data, 1, null, null);
		}

		public static void CloseGuildActivity(object data = null)
		{
			if (GameApp.View.IsOpened(ViewName.GuildActivityViewModule))
			{
				GameApp.View.CloseView(ViewName.GuildActivityViewModule, null);
			}
		}

		public static void OpenUIGuildChat(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildChatViewModule, data, 1, null, null);
		}

		public static void OpenGuildRaceRecord(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceRecordMainViewModule, data, 1, null, null);
		}

		public static void CloseGuildRaceRecord()
		{
			GameApp.View.CloseView(ViewName.GuildRaceRecordMainViewModule, null);
		}

		public static void OpenGuildRaceSeasonSelect(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceSeasonSelViewModule, data, 1, null, null);
		}

		public static void CloseGuildRaceSeasonSelect()
		{
			GameApp.View.CloseView(ViewName.GuildRaceSeasonSelViewModule, null);
		}

		public static void OpenGuildRacePositionSet(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRacePositionSetViewModule, data, 1, null, null);
		}

		public static void CloseGuildRacePositionSet()
		{
			GameApp.View.CloseView(ViewName.GuildRacePositionSetViewModule, null);
		}

		public static void OpenGuildRaceRank(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceRankViewModule, data, 1, null, null);
		}

		public static void CloseGuildRaceRank()
		{
			GameApp.View.CloseView(ViewName.GuildRaceRankViewModule, null);
		}

		public static void OpenGuildRaceRewardsShow(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceRewardsShowViewModule, data, 1, null, null);
		}

		public static void CloseGuildRaceRewardsShow()
		{
			GameApp.View.CloseView(ViewName.GuildRaceRewardsShowViewModule, null);
		}

		public static void OpenGuildRaceRecordBattle(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceRecordBattleViewModule, data, 1, null, null);
		}

		public static void CloseGuildRaceRecordBattle()
		{
			GameApp.View.CloseView(ViewName.GuildRaceRecordBattleViewModule, null);
		}

		public static void OpenGuildRaceDanChange(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceDanChangeViewModule, data, 1, null, null);
		}

		public static void CloseGuildRaceDanChange()
		{
			GameApp.View.CloseView(ViewName.GuildRaceDanChangeViewModule, null);
		}

		public static void OpenGuildRaceBattleMatchViewModule(object data = null)
		{
			GameApp.View.OpenView(ViewName.GuildRaceBattleMatchViewModule, data, 1, null, null);
		}

		public static void CloseGuildRaceBattleMatchViewModule()
		{
			GameApp.View.CloseView(ViewName.GuildRaceBattleMatchViewModule, null);
		}

		public static void CloseGuildRaceAllView()
		{
			List<ViewName> list = new List<ViewName>
			{
				ViewName.GuildRaceMainViewModule,
				ViewName.GuildRaceRecordMainViewModule,
				ViewName.GuildRaceRecordBattleViewModule,
				ViewName.GuildRaceRankViewModule
			};
			for (int i = 0; i < list.Count; i++)
			{
				if (GameApp.View.IsOpened(list[i]))
				{
					GameApp.View.CloseView(list[i], null);
				}
			}
		}

		public static void OpenOtherPlayer(object data = null)
		{
			if (data is long)
			{
				long num = (long)data;
				PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(num);
				if (GameApp.View.IsOpened(ViewName.GuildPlayerInformationViewModule))
				{
					GameApp.View.CloseView(ViewName.GuildPlayerInformationViewModule, null);
				}
				GameApp.View.OpenView(ViewName.GuildPlayerInformationViewModule, openData, 1, null, null);
			}
		}

		public static void OpenOtherPlayerRace(object data = null)
		{
			if (data is long)
			{
				long num = (long)data;
				PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(num);
				if (GameApp.View.IsOpened(ViewName.GuildPlayerInformationViewModule))
				{
					GameApp.View.CloseView(ViewName.GuildPlayerInformationViewModule, null);
				}
				GameApp.View.OpenView(ViewName.GuildPlayerInformationViewModule, openData, 1, null, null);
			}
		}

		public static void ShowTips(string tips)
		{
			GameApp.View.ShowStringTip(tips);
		}

		public static void ShowTipsID(int lanid)
		{
			GameApp.View.ShowStringTip(GuildProxy.Language.GetInfoByID_LogError(lanid));
		}

		public static void ShowItemInfo(UIItem ctrl, PropData data, object obj)
		{
			DxxTools.UI.OnItemClick(ctrl, data, obj);
		}

		private static string GetPopSureButtonString()
		{
			return GuildProxy.Language.GetInfoByID("17");
		}

		private static string GetPopCancleButtonString()
		{
			return GuildProxy.Language.GetInfoByID("18");
		}

		public static void OpenUIPopCommon(string title, string content, bool showCloseButton, bool ifShowCancel, Action onSure, Action onCancel, Action onPanelClose = null)
		{
			DxxTools.UI.OpenPopCommon(content, delegate(int ret)
			{
				switch (ret)
				{
				case -1:
				case 2:
				{
					Action onCancel2 = onCancel;
					if (onCancel2 == null)
					{
						return;
					}
					onCancel2();
					return;
				}
				case 0:
				{
					Action onPanelClose2 = onPanelClose;
					if (onPanelClose2 == null)
					{
						return;
					}
					onPanelClose2();
					return;
				}
				case 1:
				{
					Action onSure2 = onSure;
					if (onSure2 == null)
					{
						return;
					}
					onSure2();
					return;
				}
				default:
					return;
				}
			}, title, GuildProxy.UI.GetPopSureButtonString(), GuildProxy.UI.GetPopCancleButtonString(), showCloseButton, 2);
		}

		public static void OpenUIPopCommon(string title, string content, string surebtn, string canclebtn, Action onSure, Action onCancel)
		{
			if (string.IsNullOrEmpty(surebtn))
			{
				surebtn = GuildProxy.UI.GetPopSureButtonString();
			}
			if (string.IsNullOrEmpty(canclebtn))
			{
				canclebtn = GuildProxy.UI.GetPopCancleButtonString();
			}
			DxxTools.UI.OpenPopCommon(content, delegate(int ret)
			{
				switch (ret)
				{
				case 0:
				{
					Action onCancel2 = onCancel;
					if (onCancel2 == null)
					{
						return;
					}
					onCancel2();
					return;
				}
				case 1:
				{
					Action onSure2 = onSure;
					if (onSure2 == null)
					{
						return;
					}
					onSure2();
					return;
				}
				case 2:
				{
					Action onCancel3 = onCancel;
					if (onCancel3 == null)
					{
						return;
					}
					onCancel3();
					return;
				}
				default:
					return;
				}
			}, title, surebtn, canclebtn, false, 2);
		}

		public static void OpenUIPopCommonDanger(string title, string content, string surebtn, string canclebtn, Action onSure, Action onCancel)
		{
			if (string.IsNullOrEmpty(surebtn))
			{
				surebtn = GuildProxy.UI.GetPopSureButtonString();
			}
			if (string.IsNullOrEmpty(canclebtn))
			{
				canclebtn = GuildProxy.UI.GetPopCancleButtonString();
			}
			DxxTools.UI.OpenPopCommonDanger(content, delegate(int ret)
			{
				switch (ret)
				{
				case 0:
				{
					Action onCancel2 = onCancel;
					if (onCancel2 == null)
					{
						return;
					}
					onCancel2();
					return;
				}
				case 1:
				{
					Action onSure2 = onSure;
					if (onSure2 == null)
					{
						return;
					}
					onSure2();
					return;
				}
				case 2:
				{
					Action onCancel3 = onCancel;
					if (onCancel3 == null)
					{
						return;
					}
					onCancel3();
					return;
				}
				default:
					return;
				}
			}, title, surebtn, canclebtn, false);
		}

		public static void OpenUIPopCommonNoClose(string title, string content, Action onSure, Action onCancel)
		{
			DxxTools.UI.OpenPopCommon(content, delegate(int ret)
			{
				switch (ret)
				{
				case 0:
				{
					Action onCancel2 = onCancel;
					if (onCancel2 == null)
					{
						return;
					}
					onCancel2();
					return;
				}
				case 1:
				{
					Action onSure2 = onSure;
					if (onSure2 == null)
					{
						return;
					}
					onSure2();
					return;
				}
				case 2:
				{
					Action onCancel3 = onCancel;
					if (onCancel3 == null)
					{
						return;
					}
					onCancel3();
					return;
				}
				default:
					return;
				}
			}, title, GuildProxy.UI.GetPopSureButtonString(), GuildProxy.UI.GetPopCancleButtonString(), false, 2);
		}

		public static void OpenUIPopCommonSimple(string title, string content)
		{
			DxxTools.UI.OpenPopCommon(content, null, title, GuildProxy.UI.GetPopSureButtonString(), "", false, 2);
		}

		public static void OpenUIPopCommonOnlySure(string title, string content, Action onSure)
		{
			DxxTools.UI.OpenPopCommon(content, delegate(int ret)
			{
				Action onSure2 = onSure;
				if (onSure2 == null)
				{
					return;
				}
				onSure2();
			}, title, GuildProxy.UI.GetPopSureButtonString(), "", true, 2);
		}

		public static void OpenUICommonReward(RepeatedField<RewardDto> rewards, Action onclose = null)
		{
			DxxTools.UI.OpenRewardCommon(rewards, onclose, true);
		}

		public static void OpenUIBuyPop(GuildItemData showItem, GuildItemData cost, Action onSuer)
		{
			if (showItem == null)
			{
				HLog.LogError("ShowBuyPop, showItem is null");
				return;
			}
			ShopBuyConfirmData shopBuyConfirmData = new ShopBuyConfirmData();
			shopBuyConfirmData.SetRewards(new List<ItemData>
			{
				new ItemData(showItem.id, (long)showItem.count)
			});
			shopBuyConfirmData.SetCost(cost.id, cost.count);
			GameApp.View.OpenView(ViewName.ShopBuyConfirmViewModule, shopBuyConfirmData, 2, null, null);
		}

		public static void ShowBuyPop(string title, string content, GuildItemData cost, Action onSuer)
		{
			DxxTools.UI.OpenPopCommon(content, delegate(int ret)
			{
				if (ret == 1)
				{
					Action onSuer2 = onSuer;
					if (onSuer2 == null)
					{
						return;
					}
					onSuer2();
				}
			}, title, GuildProxy.UI.GetPopSureButtonString(), GuildProxy.UI.GetPopCancleButtonString(), true, 2);
		}

		public static void OpenUserDetailUI(long userid)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(userid);
			if (GameApp.View.IsOpened(ViewName.GuildPlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.GuildPlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.GuildPlayerInformationViewModule, openData, 1, null, null);
		}

		public static void OpenUIItemPop(GameObject obj, GuildItemData itemData)
		{
			GuildProxy.UI.ShowTips("###点击查看物品详情！");
		}

		public static void GetBattleReport(ulong battlerecordid, Action<bool, UserGetBattleReportResponse> callback)
		{
			NetworkUtils.User.DoUserGetBattleReportRequest(battlerecordid, callback);
		}

		public static void JumpToBattle(UserGetBattleReportResponse reportresponse, GuildRaceUserVSRecord vsRecord)
		{
			EventArgsBattleGuildRankEnter instance = Singleton<EventArgsBattleGuildRankEnter>.Instance;
			instance.SetData(reportresponse.Record, vsRecord);
			GuildProxy.GameEvent.PushEventArg(LocalMessageName.CC_BattleGuildRank_BattleGuildRankEnter, instance);
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					EventArgsRefreshMainOpenData instance2 = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance2.SetData(DxxTools.UI.GetGuildRaceOpenData());
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance2);
					EventArgsGameDataEnter instance3 = Singleton<EventArgsGameDataEnter>.Instance;
					instance3.SetData(GameModel.GuildRank, null);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameData_GameEnter, instance3);
					GameApp.State.ActiveState(StateName.BattleGuildRankState);
				});
			});
		}

		public static void PlayScaleLoopListItemScale(SequencePool m_seqPool, LoopListView2 Scroll)
		{
			m_seqPool.Clear(false);
			for (int i = 0; i < Scroll.ShownItemCount; i++)
			{
				LoopListViewItem2 shownItemByIndex = Scroll.GetShownItemByIndex(i);
				if (!(shownItemByIndex == null))
				{
					RectTransform cachedRectTransform = shownItemByIndex.CachedRectTransform;
					cachedRectTransform.localScale = Vector3.zero;
					TweenSettingsExtensions.Append(m_seqPool.Get(), TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(cachedRectTransform, 1f, 0.2f), (float)i * 0.05f));
				}
			}
		}

		public static int OfflineSec()
		{
			return 600;
		}
	}

	public class GameUser
	{
		public static string MyNick()
		{
			return GameApp.Data.GetDataModule(DataName.LoginDataModule).NickName;
		}

		public static long UserID()
		{
			return GameApp.Data.GetDataModule(DataName.LoginDataModule).userId;
		}

		public static int MyLevel()
		{
			return (int)GameApp.Data.GetDataModule(DataName.LoginDataModule).userLevel.Level;
		}

		public static int MyDiamond()
		{
			return GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.Diamonds;
		}

		public static int MyGold()
		{
			return (int)GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.Coins;
		}

		public static string GetPlayerDefaultNick(long userid)
		{
			return DxxTools.GetDefaultNick(userid);
		}
	}
}
