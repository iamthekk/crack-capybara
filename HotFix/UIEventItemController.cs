using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIEventItemController : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.itemObj.SetActiveSafe(false);
			this.SetNextFlyNode();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshEventTaskItems, new HandlerEvent(this.OnEventRefreshItems));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshEventTaskItems, new HandlerEvent(this.OnEventRefreshItems));
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
		}

		private void OnEventRefreshItems(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshItems();
		}

		private void RefreshItems()
		{
			this.flyNode.transform.SetParentNormal(this.child.transform, false);
			List<GameEventItemData> showItems = Singleton<GameEventController>.Instance.GetShowItems();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < showItems.Count; j++)
			{
				UIGameEventItemItem uigameEventItemItem;
				if (j < this.itemList.Count)
				{
					uigameEventItemItem = this.itemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.itemObj);
					gameObject.transform.SetParentNormal(this.itemParent.transform, false);
					uigameEventItemItem = gameObject.GetComponent<UIGameEventItemItem>();
					uigameEventItemItem.Init();
					this.itemList.Add(uigameEventItemItem);
				}
				uigameEventItemItem.gameObject.SetActiveSafe(true);
				uigameEventItemItem.Refresh(showItems[j]);
			}
			this.SetNextFlyNode();
		}

		public void SetShow(bool isShow)
		{
			this.child.SetActiveSafe(isShow);
		}

		private void SetNextFlyNode()
		{
			this.flyNode.transform.SetParentNormal(this.itemParent.transform, false);
			this.flyNode.transform.SetSiblingIndex(this.itemList.Count);
		}

		public GameObject child;

		public GameObject itemParent;

		public GameObject itemObj;

		public GameObject flyNode;

		private List<UIGameEventItemItem> itemList = new List<UIGameEventItemItem>();

		private int count;
	}
}
