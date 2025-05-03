using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.UI;
using Framework.MailManager;
using Framework.SoundModule;
using Framework.State;
using Framework.TableModule;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;
using Server;
using UnityEngine;

namespace HotFix
{
	public static class FrameworkExpand
	{
		public static LocalModelManager GetManager(this TableManager tableManager)
		{
			return tableManager.GetITableManager() as LocalModelManager;
		}

		public static List<MergeAttributeData> GetMergeAttributeData(this Equip_equip equip_Equip)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			foreach (string text in equip_Equip.baseAttributes.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "")
				.Split('|', StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text, null, null);
					list.Add(mergeAttributeData);
				}
			}
			return list;
		}

		public static List<MergeAttributeData> GetMergeAttributeData(this Equip_equip equip_Equip, int level, int evolution)
		{
			List<MergeAttributeData> mergeAttributeData = equip_Equip.GetMergeAttributeData();
			int evolutionTableID = equip_Equip.GetEvolutionTableID(evolution);
			int evolutionTableID2 = equip_Equip.GetEvolutionTableID(evolution - 1);
			Equip_equipEvolution equip_equipEvolution = ((evolution - 1 > 0) ? GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionTableID2) : null);
			Equip_equipEvolution elementById = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionTableID);
			long[] upgradeAttributes = elementById.upgradeAttributes;
			long[] evolutionAttributes = elementById.evolutionAttributes;
			int num;
			if (equip_equipEvolution != null)
			{
				num = Utility.Math.Max(level - equip_equipEvolution.maxLevel, 0);
			}
			else
			{
				num = Utility.Math.Max(level - 1, 0);
			}
			int composeId = equip_Equip.composeId;
			int num2 = 0;
			for (int i = 1; i <= composeId; i++)
			{
				Equip_equipCompose elementById2 = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(i);
				if (elementById2 != null)
				{
					num2 += elementById2.qualityAttributes;
				}
			}
			float num3 = 1f + (float)num2 / 10000f;
			for (int j = 0; j < mergeAttributeData.Count; j++)
			{
				MergeAttributeData mergeAttributeData2 = mergeAttributeData[j];
				mergeAttributeData2.Value = evolutionAttributes[j];
				if (upgradeAttributes.Length > j)
				{
					long num4 = upgradeAttributes[j];
					if (num4 > 0L && num > 0)
					{
						mergeAttributeData2.Plus((long)num * num4);
					}
				}
				mergeAttributeData2.Multiply(num3);
			}
			return mergeAttributeData;
		}

		public static bool HasNextLevelUpID(this Equip_equip equip_Equip, int level, int evolution)
		{
			int evolutionTableID = equip_Equip.GetEvolutionTableID(evolution);
			Equip_equipEvolution elementById = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionTableID);
			return elementById != null && (elementById.nextID > 0 || level < elementById.maxLevel);
		}

		public static int GetUpdateLevelTableID(this Equip_equip equip_Equip, int level)
		{
			if (level <= 0)
			{
				level = 1;
			}
			return equip_Equip.Type * 10000 + level;
		}

		public static List<ItemData> GetLevelUpCosts(this Equip_equip equip_Equip, int level)
		{
			List<ItemData> list = new List<ItemData>();
			int updateLevelTableID = equip_Equip.GetUpdateLevelTableID(level);
			Equip_updateLevel elementById = GameApp.Table.GetManager().GetEquip_updateLevelModelInstance().GetElementById(updateLevelTableID);
			if (elementById == null)
			{
				return list;
			}
			for (int i = 0; i < elementById.levelupCost.Length; i++)
			{
				string text = elementById.levelupCost[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					int num;
					long num2;
					if (array.Length == 2 && int.TryParse(array[0], out num) && long.TryParse(array[1], out num2))
					{
						ItemData itemData = new ItemData(num, num2);
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public static List<ItemData> GetEvolutionCosts(this Equip_equip equip_Equip, int evolution)
		{
			List<ItemData> list = new List<ItemData>();
			int evolutionTableID = equip_Equip.GetEvolutionTableID(evolution);
			Equip_equipEvolution elementById = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionTableID);
			if (elementById == null)
			{
				return list;
			}
			for (int i = 0; i < elementById.evolutionCost.Length; i++)
			{
				string text = elementById.evolutionCost[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					int num;
					long num2;
					if (array.Length == 2 && int.TryParse(array[0], out num) && long.TryParse(array[1], out num2))
					{
						ItemData itemData = new ItemData(num, num2);
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public static List<PropData> ToPropDataList(this string[] items)
		{
			List<PropData> list = new List<PropData>();
			if (items == null || items.Length == 0)
			{
				return list;
			}
			foreach (string text in items)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					int num;
					long num2;
					if (array.Length >= 2 && int.TryParse(array[0], out num) && long.TryParse(array[1], out num2))
					{
						list.Add(new PropData
						{
							id = (uint)num,
							count = (ulong)((uint)num2)
						});
					}
				}
			}
			return list;
		}

		public static List<ItemData> ToItemDataList(this string[] items)
		{
			List<ItemData> list = new List<ItemData>();
			if (items == null || items.Length == 0)
			{
				return list;
			}
			foreach (string text in items)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					int num;
					long num2;
					if (array.Length >= 2 && int.TryParse(array[0], out num) && long.TryParse(array[1], out num2))
					{
						ItemData itemData = new ItemData(num, num2);
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public static List<ItemData> ToItemDataList(this string str)
		{
			return str.Split('|', StringSplitOptions.None).ToItemDataList();
		}

		public static void RegisterEvent(this EventSystemManager manager, LocalMessageName name, HandlerEvent handle)
		{
			manager.RegisterEvent((int)name, handle);
		}

		public static void UnRegisterEvent(this EventSystemManager manager, LocalMessageName name, HandlerEvent handle)
		{
			manager.UnRegisterEvent((int)name, handle);
		}

		public static void Dispatch(this EventSystemManager manager, object sender, LocalMessageName name, BaseEventArgs eventArgs)
		{
			manager.Dispatch(sender, (int)name, eventArgs);
		}

		public static void DispatchNow(this EventSystemManager manager, object sender, LocalMessageName name, BaseEventArgs eventArgs)
		{
			manager.DispatchNow(sender, (int)name, eventArgs);
		}

		public static T GetDataModule<T>(this DataModuleManager manager, DataName name) where T : IDataModule
		{
			return manager.GetDataModule<T>((int)name);
		}

		public static T GetViewModule<T>(this ViewModuleManager manager, ViewName name) where T : BaseViewModule
		{
			return manager.GetViewModule<T>((int)name);
		}

		public static async void OpenView(this ViewModuleManager manager, ViewName name, object data = null, UILayers layer = 0, Action<GameObject> loadedCallBack = null, Action<GameObject> openedCallBack = null)
		{
			await manager.OpenView((int)name, data, layer, loadedCallBack, openedCallBack);
			GameTGAExtend.OnViewOpen(name);
		}

		public static async Task OpenViewTask(this ViewModuleManager manager, ViewName name, object data = null, UILayers layer = 0, Action<GameObject> loadedCallBack = null, Action<GameObject> openedCallBack = null)
		{
			await manager.OpenView((int)name, data, layer, loadedCallBack, openedCallBack);
			GameTGAExtend.OnViewOpen(name);
		}

		public static void CloseView(this ViewModuleManager manager, ViewName name, Action onFinish = null)
		{
			if (manager.IsOpened(name))
			{
				manager.CloseView((int)name, delegate
				{
					Action onFinish3 = onFinish;
					if (onFinish3 != null)
					{
						onFinish3();
					}
					if (name != ViewName.LoginViewModule && FrameworkUIExpand.IsContainsQueue(name))
					{
						GameApp.View.ShowWindowToQueue();
					}
				});
				GameTGAExtend.OnViewClose(name);
				return;
			}
			Action onFinish2 = onFinish;
			if (onFinish2 == null)
			{
				return;
			}
			onFinish2();
		}

		public static bool IsOpened(this ViewModuleManager manager, ViewName name)
		{
			return manager.IsOpened((int)name);
		}

		public static bool IsLoading(this ViewModuleManager manager, ViewName name)
		{
			return manager.IsLoading((int)name);
		}

		public static bool IsOpenedOrLoading(this ViewModuleManager manager, ViewName name)
		{
			return manager.IsOpenedOrLoading((int)name);
		}

		public static void ShowStringTip(this ViewModuleManager manager, string info)
		{
			EventArgsString instance = Singleton<EventArgsString>.Instance;
			instance.SetData(info);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance);
		}

		public static void ShowItemNotEnoughTip(this ViewModuleManager manager, int itemId, bool isShowTip = true)
		{
			if (itemId <= 0)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById == null)
			{
				return;
			}
			bool flag = false;
			if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.ItemResources, false))
			{
				flag = FrameworkExpand.DispatchItemNotEnoughEvent(itemId, false);
			}
			if (isShowTip && !flag)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("tip_item_unenough", new object[] { infoByID }));
			}
		}

		public static bool DispatchItemNotEnoughEvent(int itemId, bool isShowFunctionTip = false)
		{
			if (GameApp.Table.GetManager().GetItemResources_itemget(itemId) != null && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.ItemResources, isShowFunctionTip))
			{
				GameApp.View.OpenView(ViewName.ItemResourcesViewModule, itemId, 1, null, null);
				return true;
			}
			return false;
		}

		public static void FlyItemDatas(this ViewModuleManager manager, FlyItemModel model, List<ItemData> itemDatas, OnFlyNodeItemDatasItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			EventArgsFlyItemFlyItemDatas eventArgsFlyItemFlyItemDatas = new EventArgsFlyItemFlyItemDatas();
			eventArgsFlyItemFlyItemDatas.SetData(model, itemDatas, onItemFinished, onFinished);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_FlyItemViewModule_FlyItemDatas, eventArgsFlyItemFlyItemDatas);
		}

		public static void FlyItemDatasWithStartPosition(this ViewModuleManager manager, FlyItemModel model, List<ItemData> itemDatas, Vector3 startPos, OnFlyNodeItemDatasItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			EventArgsFlyItemFlyItemDatas eventArgsFlyItemFlyItemDatas = new EventArgsFlyItemFlyItemDatas();
			eventArgsFlyItemFlyItemDatas.SetData(model, itemDatas, startPos, onItemFinished, onFinished);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_FlyItemViewModule_FlyItemDatas, eventArgsFlyItemFlyItemDatas);
		}

		public static void FlyItemDatas(this ViewModuleManager manager, FlyItemModel model, List<FlyItemData> itemDatas, OnFlyNodeFlyNodeOthersItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			EventArgsFlyItemFlyItemDatas eventArgsFlyItemFlyItemDatas = new EventArgsFlyItemFlyItemDatas();
			eventArgsFlyItemFlyItemDatas.SetData(model, itemDatas, onItemFinished, onFinished);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_FlyItemViewModule_FlyItemDatas, eventArgsFlyItemFlyItemDatas);
		}

		public static void ActiveState(this StateManager manager, StateName name)
		{
			manager.ActiveState((int)name);
		}

		public static T GetState<T>(this StateManager manager, StateName name) where T : State
		{
			return manager.GetState<T>((int)name);
		}

		public static Atlas_atlas GetAtlas(this TableManager tableManager, int id)
		{
			return tableManager.GetManager().GetAtlas_atlasModelInstance().GetElementById(id);
		}

		public static string GetAtlasPath(this TableManager tableManager, int id)
		{
			Atlas_atlas atlas = tableManager.GetAtlas(id);
			if (atlas == null)
			{
				return string.Empty;
			}
			return atlas.path;
		}

		public static void PlayClip(this AudioManager manager, int id, float volume = 1f)
		{
			if (id.Equals(0))
			{
				return;
			}
			Sound_sound elementById = GameApp.Table.GetManager().GetSound_soundModelInstance().GetElementById(id);
			if (elementById == null)
			{
				HLog.LogError(string.Format("FrameworkExpand.PlayClip id:[{0}] is null.", id));
				return;
			}
			if (elementById.volume == 0f)
			{
				HLog.LogError(string.Format("FrameworkExpand.PlayClip id:[{0}] volume == 0.", id));
				return;
			}
			manager.PlaySoundEffect(elementById.path, elementById.volume * volume);
		}

		public static void PlayBGM(this AudioManager manager, int id, float volume = 1f)
		{
			Sound_sound elementById = GameApp.Table.GetManager().GetSound_soundModelInstance().GetElementById(id);
			if (elementById == null)
			{
				HLog.LogError(string.Format("FrameworkExpand.PlayBGM id:[{0}] is null.", id));
				return;
			}
			if (elementById.volume == 0f)
			{
				HLog.LogError(string.Format("FrameworkExpand.PlayBGM id:[{0}] volume == 0.", id));
				return;
			}
			manager.PlayBackground(elementById.path, elementById.volume * volume);
		}

		public static List<MergeAttributeData> Merge(this List<MergeAttributeData> datas)
		{
			Dictionary<string, MergeAttributeData> dictionary = new Dictionary<string, MergeAttributeData>();
			for (int i = 0; i < datas.Count; i++)
			{
				MergeAttributeData mergeAttributeData = datas[i];
				if (mergeAttributeData != null)
				{
					MergeAttributeData mergeAttributeData2;
					dictionary.TryGetValue(mergeAttributeData.Header, out mergeAttributeData2);
					if (mergeAttributeData2 == null)
					{
						mergeAttributeData2 = mergeAttributeData.Clone();
					}
					else
					{
						mergeAttributeData2.Merge(mergeAttributeData);
					}
					dictionary[mergeAttributeData.Header] = mergeAttributeData2;
				}
			}
			return dictionary.Values.ToList<MergeAttributeData>();
		}

		public static HabbyMailManager GetManager(this MailManager manager)
		{
			return manager.Manager as HabbyMailManager;
		}

		public static List<ItemData> GetShowProductsData(this IAP_Purchase table, bool ignoreVipExp = false)
		{
			List<ItemData> list = new List<ItemData>();
			if (table.showProducts == null)
			{
				return list;
			}
			for (int i = 0; i < table.showProducts.Length; i++)
			{
				string text = table.showProducts[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array[0]), (long)int.Parse(array[1])));
					}
				}
			}
			return list;
		}

		public static List<ItemData> GetDayRewardItemData(this IAP_MonthCard table)
		{
			List<ItemData> list = new List<ItemData>();
			if (table.productsPerDay == null)
			{
				return list;
			}
			for (int i = 0; i < table.productsPerDay.Length; i++)
			{
				string text = table.productsPerDay[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array[0]), (long)int.Parse(array[1])));
					}
				}
			}
			return list;
		}

		public static List<ItemData> GetNowRewardItemData(this IAP_MonthCard table)
		{
			List<ItemData> list = new List<ItemData>();
			if (table.products == null)
			{
				return list;
			}
			for (int i = 0; i < table.products.Length; i++)
			{
				string text = table.products[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array[0]), (long)int.Parse(array[1])));
					}
				}
			}
			return list;
		}

		public static List<ItemData> GetRewardItemData(this IAP_GiftPacks table)
		{
			List<ItemData> list = new List<ItemData>();
			if (table.products == null)
			{
				return list;
			}
			for (int i = 0; i < table.products.Length; i++)
			{
				string text = table.products[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array[0]), long.Parse(array[1])));
					}
				}
			}
			return list;
		}

		public static List<ItemData> GetRewardItemData(this IAP_PushPacks table)
		{
			List<ItemData> list = new List<ItemData>();
			if (table.products == null)
			{
				return list;
			}
			for (int i = 0; i < table.products.Length; i++)
			{
				string text = table.products[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array[0]), (long)int.Parse(array[1])));
					}
				}
			}
			return list;
		}

		public static List<ItemData> GetRewardItemData(this IAP_PushPacks table, int day)
		{
			List<ItemData> list = new List<ItemData>();
			string[] array = null;
			switch (day)
			{
			case 1:
				array = table.products;
				break;
			case 2:
				array = table.products2;
				break;
			case 3:
				array = table.products3;
				break;
			}
			if (array == null)
			{
				return list;
			}
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string[] array2 = text.Split(',', StringSplitOptions.None);
					if (array2.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array2[0]), (long)int.Parse(array2[1])));
					}
				}
			}
			return list;
		}

		public static string GetMyTGAInfo(this PVPRecordDto record)
		{
			return "";
		}

		public static string GetEnemyTGAInfo(this PVPRecordDto record)
		{
			return "";
		}

		public static string GetUnitTGAInfo(this RepeatedField<CombatUnitDto> record)
		{
			return "";
		}

		public static string GetBattleUserTGAInfo(this BattleUserDto record)
		{
			return "";
		}

		public static void SetImage(this CustomImage customImage, string[] iconInfo)
		{
			if (customImage == null)
			{
				return;
			}
			if (iconInfo == null || iconInfo.Length < 2)
			{
				return;
			}
			int num = int.Parse(iconInfo[0]);
			customImage.SetImage(num, iconInfo[1]);
		}

		public static void SetImage(this CustomImage customImage, int atlasId, string spriteName)
		{
			if (customImage == null)
			{
				return;
			}
			Atlas_atlas elementById = GameApp.Table.GetManager().GetAtlas_atlasModelInstance().GetElementById(atlasId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("atlasId:{0} can't find in Atlas Config, please check!!", atlasId));
				return;
			}
			customImage.SetImage(elementById.path, spriteName);
		}

		public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
		{
			foreach (T t in items)
			{
				queue.Enqueue(t);
			}
		}
	}
}
