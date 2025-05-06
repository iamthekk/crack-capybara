using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class FlyItemViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_pool.gameObject.SetActive(false);
			List<string> list = new List<string>();
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				FlyItemViewModule.Data data2 = this.m_datas[i];
				if (data2.m_item.gameObject != null)
				{
					data2.m_item.gameObject.SetActive(false);
					if (!list.Contains(data2.m_item.gameObject.name))
					{
						this.m_pool.CreateCache(data2.m_item.gameObject.name, data2.m_item.gameObject);
					}
				}
				for (int j = 0; j < data2.m_endNodes.Count; j++)
				{
					BaseFlyEndNode baseFlyEndNode = data2.m_endNodes[j];
					if (!(baseFlyEndNode == null) && !(baseFlyEndNode.gameObject == null))
					{
						baseFlyEndNode.gameObject.SetActive(false);
					}
				}
			}
		}

		public override void OnOpen(object data)
		{
			this.m_onUpdate.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.list.Clear();
			this.list.AddRange(this.m_onUpdate.Values);
			foreach (CustomBehaviour customBehaviour in this.list)
			{
				if (!(customBehaviour == null) && !(customBehaviour.gameObject == null))
				{
					customBehaviour.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public override void OnClose()
		{
			this.m_onUpdate.Clear();
		}

		public override void OnDelete()
		{
			this.m_pool.ClearAllCache();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_FlyItemViewModule_SetEnd, new HandlerEvent(this.OnEventSetEnd));
			manager.RegisterEvent(LocalMessageName.CC_FlyItemViewModule_AddUpdate, new HandlerEvent(this.OnEventAddUpdate));
			manager.RegisterEvent(LocalMessageName.CC_FlyItemViewModule_RemoveUpdate, new HandlerEvent(this.OnEventRemoveUpdate));
			manager.RegisterEvent(LocalMessageName.CC_FlyItemViewModule_FlyItemDatas, new HandlerEvent(this.OnEventFlyItemDatas));
			manager.RegisterEvent(LocalMessageName.CC_FlyItemViewModule_Show, new HandlerEvent(this.OnEventShow));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_FlyItemViewModule_SetEnd, new HandlerEvent(this.OnEventSetEnd));
			manager.UnRegisterEvent(LocalMessageName.CC_FlyItemViewModule_AddUpdate, new HandlerEvent(this.OnEventAddUpdate));
			manager.UnRegisterEvent(LocalMessageName.CC_FlyItemViewModule_RemoveUpdate, new HandlerEvent(this.OnEventRemoveUpdate));
			manager.UnRegisterEvent(LocalMessageName.CC_FlyItemViewModule_FlyItemDatas, new HandlerEvent(this.OnEventFlyItemDatas));
			manager.UnRegisterEvent(LocalMessageName.CC_FlyItemViewModule_Show, new HandlerEvent(this.OnEventShow));
		}

		private void OnEventSetEnd(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = eventargs as EventArgFlyItemViewModuleSetEnd;
			if (eventArgFlyItemViewModuleSetEnd == null)
			{
				return;
			}
			FlyItemViewModule.Data data = null;
			if (eventArgFlyItemViewModuleSetEnd.m_type == FlyItemType.Currency)
			{
				data = this.FindData(eventArgFlyItemViewModuleSetEnd.m_model, eventArgFlyItemViewModuleSetEnd.m_type, eventArgFlyItemViewModuleSetEnd.m_currencyType);
			}
			if (eventArgFlyItemViewModuleSetEnd.m_type == FlyItemType.Other)
			{
				data = this.FindData(eventArgFlyItemViewModuleSetEnd.m_model, eventArgFlyItemViewModuleSetEnd.m_type, eventArgFlyItemViewModuleSetEnd.m_otherType);
			}
			if (data == null)
			{
				return;
			}
			if (data.m_endNodes.Count == eventArgFlyItemViewModuleSetEnd.m_transforms.Count)
			{
				for (int i = 0; i < data.m_endNodes.Count; i++)
				{
					BaseFlyEndNode baseFlyEndNode = data.m_endNodes[i];
					if (!(baseFlyEndNode == null))
					{
						baseFlyEndNode.SetTransform(eventArgFlyItemViewModuleSetEnd.m_transforms[i]);
					}
				}
			}
			if (data.m_endNodes.Count == eventArgFlyItemViewModuleSetEnd.m_positions.Count)
			{
				for (int j = 0; j < data.m_endNodes.Count; j++)
				{
					BaseFlyEndNode baseFlyEndNode2 = data.m_endNodes[j];
					if (!(baseFlyEndNode2 == null))
					{
						baseFlyEndNode2.SetPos(eventArgFlyItemViewModuleSetEnd.m_positions[j]);
					}
				}
			}
		}

		private void OnEventAddUpdate(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsCustomBehaviour eventArgsCustomBehaviour = eventargs as EventArgsCustomBehaviour;
			if (eventArgsCustomBehaviour == null)
			{
				return;
			}
			if (eventArgsCustomBehaviour.m_target == null)
			{
				return;
			}
			this.AddUpdate(eventArgsCustomBehaviour.m_target);
		}

		private void OnEventRemoveUpdate(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsCustomBehaviour eventArgsCustomBehaviour = eventargs as EventArgsCustomBehaviour;
			if (eventArgsCustomBehaviour == null)
			{
				return;
			}
			if (eventArgsCustomBehaviour.m_target == null)
			{
				return;
			}
			this.RemoveUpdate(eventArgsCustomBehaviour.m_target);
		}

		private void OnEventFlyItemDatas(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsFlyItemFlyItemDatas eventArgsFlyItemFlyItemDatas = eventargs as EventArgsFlyItemFlyItemDatas;
			if (eventArgsFlyItemFlyItemDatas == null)
			{
				return;
			}
			if (eventArgsFlyItemFlyItemDatas.m_itemDatas != null)
			{
				this.Fly(eventArgsFlyItemFlyItemDatas.m_model, eventArgsFlyItemFlyItemDatas.m_itemDatas, eventArgsFlyItemFlyItemDatas.useCustomStartPos, eventArgsFlyItemFlyItemDatas.m_startPos, eventArgsFlyItemFlyItemDatas.m_onItemFinished, eventArgsFlyItemFlyItemDatas.m_onFinished);
				return;
			}
			if (eventArgsFlyItemFlyItemDatas.m_flyItemDatas != null)
			{
				this.Fly(eventArgsFlyItemFlyItemDatas.m_model, eventArgsFlyItemFlyItemDatas.m_flyItemDatas, eventArgsFlyItemFlyItemDatas.m_onFlyItemFinished, eventArgsFlyItemFlyItemDatas.m_onFinished);
			}
		}

		private void OnEventShow(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsBool eventArgsBool = eventArgs as EventArgsBool;
			if (eventArgsBool != null)
			{
				this.modelDefault.SetActiveSafe(eventArgsBool.Value);
			}
		}

		public FlyItemViewModule.Data FindData(FlyItemModel model, ItemData itemData)
		{
			if (itemData == null)
			{
				return null;
			}
			return this.FindData(model, FlyItemType.Currency, (CurrencyType)itemData.ID);
		}

		public FlyItemViewModule.Data FindData(FlyItemModel model, FlyItemType type, CurrencyType currencyType)
		{
			FlyItemViewModule.Data data = null;
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				FlyItemViewModule.Data data2 = this.m_datas[i];
				if (data2 != null && data2.m_model == model && data2.m_type == FlyItemType.Currency && currencyType == data2.m_currencyType)
				{
					data = data2;
					break;
				}
			}
			return data;
		}

		public FlyItemViewModule.Data FindData(FlyItemModel model, FlyItemOtherType otherType)
		{
			return this.FindData(model, FlyItemType.Other, otherType);
		}

		public FlyItemViewModule.Data FindData(FlyItemModel model, FlyItemType type, FlyItemOtherType otherType)
		{
			FlyItemViewModule.Data data = null;
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				FlyItemViewModule.Data data2 = this.m_datas[i];
				if (data2 != null && data2.m_model == model && data2.m_type == FlyItemType.Other && otherType == data2.m_otherType)
				{
					data = data2;
					break;
				}
			}
			return data;
		}

		private bool GetCountByItemData(ItemData data, out long from, out long to)
		{
			from = 0L;
			to = 0L;
			if (data == null)
			{
				return true;
			}
			CurrencyType id = (CurrencyType)data.ID;
			if (id <= CurrencyType.ChestScore)
			{
				switch (id)
				{
				case CurrencyType.Gold:
				{
					LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
					to = (long)((int)dataModule.userCurrency.Coins);
					from = (long)((int)dataModule.userCurrency.Coins) - data.TotalCount;
					return true;
				}
				case CurrencyType.Diamond:
				{
					LoginDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.LoginDataModule);
					to = (long)dataModule2.userCurrency.Diamonds;
					from = (long)dataModule2.userCurrency.Diamonds - data.TotalCount;
					return true;
				}
				case CurrencyType.Dust:
				case CurrencyType.TaskActive:
					break;
				case CurrencyType.DynamicGold:
				{
					LoginDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.LoginDataModule);
					to = (long)dataModule3.userCurrency.CardExp;
					from = (long)dataModule3.userCurrency.CardExp - data.TotalCount;
					return true;
				}
				case CurrencyType.TeamExp:
				{
					LoginDataModule dataModule4 = GameApp.Data.GetDataModule(DataName.LoginDataModule);
					to = (long)dataModule4.userLevel.Level;
					from = (long)dataModule4.userLevel.Level - data.TotalCount;
					return true;
				}
				case CurrencyType.VIPExp:
				{
					VIPDataModule dataModule5 = GameApp.Data.GetDataModule(DataName.VIPDataModule);
					to = (long)dataModule5.VipExp;
					from = (long)dataModule5.VipExp - data.TotalCount;
					return true;
				}
				default:
					if (id == CurrencyType.ChestScore)
					{
						MainCityDataModule dataModule6 = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
						to = (long)dataModule6.m_chestIntegral;
						from = (long)dataModule6.m_chestIntegral - data.TotalCount;
						return true;
					}
					break;
				}
			}
			else if (id - CurrencyType.BattleFood <= 1 || id == CurrencyType.BattleCoin)
			{
				return true;
			}
			return false;
		}

		private void Fly(FlyItemModel model, List<ItemData> itemDatas, OnFlyNodeItemDatasItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			this.Fly(model, itemDatas, false, Vector3.zero, onItemFinished, onFinished);
		}

		private void Fly(FlyItemModel model, List<ItemData> itemDatas, bool customStart, Vector3 startPos, OnFlyNodeItemDatasItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			FlyNodeItemDatas flyNodeItemDatas = new FlyNodeItemDatas();
			flyNodeItemDatas.m_model = model;
			flyNodeItemDatas.m_nodeParent = base.gameObject.transform;
			flyNodeItemDatas.m_onItemFinished = onItemFinished;
			flyNodeItemDatas.m_onFinished = onFinished;
			flyNodeItemDatas.m_customStartPos = startPos;
			flyNodeItemDatas.useCustomStartPos = customStart;
			for (int i = 0; i < itemDatas.Count; i++)
			{
				ItemData itemData = itemDatas[i];
				if (itemData != null)
				{
					FlyItemViewModule.Data data = this.FindData(model, itemData);
					if (data == null && model != FlyItemModel.Default)
					{
						data = this.FindData(FlyItemModel.Default, itemData);
					}
					long num;
					long num2;
					if (data != null && this.GetCountByItemData(itemData, out num, out num2))
					{
						flyNodeItemDatas.m_itemDatas.Add(itemData);
						flyNodeItemDatas.m_flyDatas.Add(data);
						FlyNodeItemDatas.Data data2 = new FlyNodeItemDatas.Data();
						data2.m_from = ((num < 0L) ? 0L : num);
						data2.m_to = ((num2 < 0L) ? 0L : num2);
						data2.m_count = itemData.TotalCount;
						flyNodeItemDatas.m_datas.Add(data2);
					}
				}
			}
			flyNodeItemDatas.m_pool = this.m_pool;
			flyNodeItemDatas.m_onFinished = new Action<BaseFlyNode>(this.OnNodeFinished);
			flyNodeItemDatas.Init();
			this.m_nodes.Add(flyNodeItemDatas);
			flyNodeItemDatas.Fly();
		}

		private void Fly(FlyItemModel model, List<FlyItemData> datas, OnFlyNodeFlyNodeOthersItemFinished onItemFinished, Action<BaseFlyNode> onFinished = null)
		{
			FlyNodeOthers flyNodeOthers = new FlyNodeOthers();
			flyNodeOthers.m_model = model;
			flyNodeOthers.m_nodeParent = base.gameObject.transform;
			flyNodeOthers.m_onItemFinished = onItemFinished;
			flyNodeOthers.m_onFinished = onFinished;
			for (int i = 0; i < datas.Count; i++)
			{
				FlyItemData flyItemData = datas[i];
				if (flyItemData != null)
				{
					FlyItemViewModule.Data data = this.FindData(model, flyItemData.m_type);
					if (data == null && model != FlyItemModel.Default)
					{
						data = this.FindData(FlyItemModel.Default, flyItemData.m_type);
					}
					if (data != null)
					{
						flyNodeOthers.m_itemDatas.Add(flyItemData);
						flyNodeOthers.m_flyDatas.Add(data);
						FlyNodeOthers.Data data2 = new FlyNodeOthers.Data();
						data2.m_from = ((flyItemData.m_from < 0L) ? 0L : flyItemData.m_from);
						data2.m_to = ((flyItemData.m_to < 0L) ? 0L : flyItemData.m_to);
						data2.m_count = flyItemData.m_count;
						data2.m_param = flyItemData.m_param;
						flyNodeOthers.m_datas.Add(data2);
					}
				}
			}
			flyNodeOthers.m_pool = this.m_pool;
			flyNodeOthers.m_onFinished = new Action<BaseFlyNode>(this.OnNodeFinished);
			flyNodeOthers.Init();
			this.m_nodes.Add(flyNodeOthers);
			flyNodeOthers.Fly();
		}

		private void OnNodeFinished(BaseFlyNode obj)
		{
			if (obj == null)
			{
				return;
			}
			obj.DeInit();
			this.m_nodes.Remove(obj);
		}

		public void AddUpdate(CustomBehaviour behaviour)
		{
			if (behaviour == null)
			{
				return;
			}
			if (behaviour.gameObject == null)
			{
				return;
			}
			int instanceID = behaviour.gameObject.GetInstanceID();
			if (this.m_onUpdate.ContainsKey(instanceID))
			{
				return;
			}
			this.m_onUpdate.Add(instanceID, behaviour);
		}

		public void RemoveUpdate(CustomBehaviour behaviour)
		{
			if (behaviour == null)
			{
				return;
			}
			if (behaviour.gameObject == null)
			{
				return;
			}
			int instanceID = behaviour.gameObject.GetInstanceID();
			this.m_onUpdate.Remove(instanceID);
		}

		[GameTestMethod("Fly", "OnFly", "", 0)]
		private static async void OnFly()
		{
			List<ItemData> list = new List<ItemData>();
			list.Add(new ItemData(1, 300L));
			list.Add(new ItemData(2, 400L));
			list.Add(new ItemData(4, 400L));
			list.Add(new ItemData(5, 400L));
			list.Add(new ItemData(7, 400L));
			list.Add(new ItemData(23, 400L));
			GameApp.View.GetViewModule(ViewName.FlyItemViewModule).Fly(FlyItemModel.Default, list, delegate(FlyNodeItemDatas data, int i, int r, int itemIndex, float p)
			{
			}, delegate(BaseFlyNode d)
			{
			});
		}

		public LocalUnityObjctPool m_pool;

		public GameObject modelDefault;

		public List<FlyItemViewModule.Data> m_datas = new List<FlyItemViewModule.Data>();

		private Dictionary<int, CustomBehaviour> m_onUpdate = new Dictionary<int, CustomBehaviour>();

		private List<CustomBehaviour> list = new List<CustomBehaviour>();

		public List<BaseFlyNode> m_nodes = new List<BaseFlyNode>();

		[Serializable]
		public class Data
		{
			public FlyItemModel m_model;

			public FlyItemType m_type;

			public CurrencyType m_currencyType = CurrencyType.Gold;

			public FlyItemOtherType m_otherType;

			public Transform m_startTrans;

			public int m_audioId;

			public string m_audioClipPath = "Assets/_Resources/Sound/UI/Button_Click_Big.wav";

			public BaseFlyItem m_item;

			public List<BaseFlyEndNode> m_endNodes = new List<BaseFlyEndNode>();
		}
	}
}
