using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace HotFix
{
	public class PushGiftBaseViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.currencyCtrl.Init();
			this.closeBtn.onClick.AddListener(new UnityAction(this.CloseSelf));
		}

		public override void OnOpen(object data)
		{
			this._posType = (PushGiftPosType)data;
			this.currencyCtrl.SetStyle(EModuleId.PushGift, new List<int> { 9, 2, 1 });
			this.OnRefreshData(null, 0, null);
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.PushGiftBundleViewModule, null);
		}

		private async void SetItems()
		{
			foreach (PushGiftData data in this._datas)
			{
				string text = this.prefabPath + data.PrefabType + ".prefab";
				AsyncOperationHandle<GameObject> prefabHandle = GameApp.Resources.LoadAssetAsync<GameObject>(text);
				await prefabHandle.Task;
				PushGiftItemBase component = Object.Instantiate<GameObject>(prefabHandle.Result, this.content.transform).GetComponent<PushGiftItemBase>();
				component.Init();
				component.SetData(data, null);
				this._items.Add(component);
				prefabHandle = default(AsyncOperationHandle<GameObject>);
				data = null;
			}
			List<PushGiftData>.Enumerator enumerator = default(List<PushGiftData>.Enumerator);
		}

		private void OnRefreshData(object obj, int type, BaseEventArgs args)
		{
			long time = DxxTools.Time.ServerTimestamp;
			if (GameApp.Data.GetDataModule(DataName.PushGiftDataModule).PushGiftDataDicByPosType.ContainsKey(this._posType))
			{
				List<PushGiftData> list = GameApp.Data.GetDataModule(DataName.PushGiftDataModule).PushGiftDataDicByPosType[this._posType];
				this._datas = (from x in list
					where x.EndTime > time
					orderby x.EndTime
					select x).ToList<PushGiftData>();
				foreach (PushGiftItemBase pushGiftItemBase in this._items)
				{
					pushGiftItemBase.DeInit();
					Object.DestroyImmediate(pushGiftItemBase.gameObject);
				}
				this._items.Clear();
				this.SetItems();
				return;
			}
			this._datas = new List<PushGiftData>();
			foreach (PushGiftItemBase pushGiftItemBase2 in this._items)
			{
				pushGiftItemBase2.DeInit();
				Object.DestroyImmediate(pushGiftItemBase2.gameObject);
			}
			this._items.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			foreach (PushGiftItemBase pushGiftItemBase in this._items)
			{
				pushGiftItemBase.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.currencyCtrl.DeInit();
			this.closeBtn.onClick.RemoveListener(new UnityAction(this.CloseSelf));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.SC_IAPData_PushGiftDataUpdate, new HandlerEvent(this.OnRefreshData));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.SC_IAPData_PushGiftDataUpdate, new HandlerEvent(this.OnRefreshData));
		}

		[SerializeField]
		private ScrollView loopListView;

		[SerializeField]
		private CustomButton closeBtn;

		[SerializeField]
		private ModuleCurrencyCtrl currencyCtrl;

		[SerializeField]
		private Transform content;

		private List<PushGiftData> _datas;

		private PushGiftPosType _posType;

		private string prefabPath = "Assets/_Resources/Prefab/UI/PushGift/";

		private List<PushGiftItemBase> _items = new List<PushGiftItemBase>();
	}
}
