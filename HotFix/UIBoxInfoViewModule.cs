using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIBoxInfoViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this._itemPool = LocalUnityObjctPool.Create(base.gameObject);
			this._itemPool.CreateCache<UIItem>(this.CopyItem.gameObject);
			this.CopyItem.gameObject.SetParentNormal(base.gameObject, false);
			this.CopyItem.gameObject.SetActive(false);
			foreach (UIBoxInfoNode uiboxInfoNode in this.InfoNodePool)
			{
				uiboxInfoNode.SetView(false);
			}
		}

		public override void OnOpen(object data)
		{
			this.CloseButton.onClick.AddListener(new UnityAction(this.OnCloseButtonClick));
			this._transferData = (UIBoxInfoViewModule.Transfer)data;
			this.curInfoNode = this.InfoNodePool.Find((UIBoxInfoNode item) => item.NodeType == this._transferData.nodeType);
			if (this.curInfoNode == null)
			{
				this.OnCloseButtonClick();
				return;
			}
			this.curInfoNode.Init();
			this.curInfoNode.SetView(true);
			if (this._transferData.nodeType == UIBoxInfoViewModule.UIBoxInfoNodeType.Title)
			{
				UIRewardTitleInfoNode uirewardTitleInfoNode = this.curInfoNode as UIRewardTitleInfoNode;
				if (uirewardTitleInfoNode != null)
				{
					uirewardTitleInfoNode.SetTitleInfo(this._transferData.title, this._transferData.info);
					this.commonScale = 0.8f;
					goto IL_00C2;
				}
			}
			this.commonScale = 0.6f;
			IL_00C2:
			this.InitItems();
			base.StartCoroutine(this.EndFrameInit());
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.CloseButton.onClick.RemoveListener(new UnityAction(this.OnCloseButtonClick));
			this._itemPool.CollectAll();
			if (this.curInfoNode != null)
			{
				this.curInfoNode.DeInit();
			}
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnCloseButtonClick()
		{
			GameApp.View.CloseView(ViewName.RewardBoxInfoViewModule, null);
		}

		private IEnumerator EndFrameInit()
		{
			this.BGRoot.gameObject.SetActiveSafe(false);
			yield return new WaitForEndOfFrame();
			this.BGRoot.gameObject.SetActiveSafe(true);
			this.BGRoot.position = this._transferData.position;
			Vector3 vector = this.BGRoot.anchoredPosition3D + this._transferData.anchoredPositionOffset;
			vector.z = 0f;
			this.BGRoot.anchoredPosition3D = vector;
			this.curInfoNode.SetArrowPos(this._transferData.position, this._transferData.IsSetArrowPos);
			yield break;
		}

		private void InitItems()
		{
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < this._transferData.rewards.Count; i++)
			{
				list.Add(this._transferData.rewards[i]);
			}
			this.InitItems(list);
		}

		private void InitItems(List<ItemData> list)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				UIItem uiitem = this._itemPool.DeQueue<UIItem>();
				uiitem.Init();
				RectTransform rectTransform = uiitem.rectTransform;
				rectTransform.SetParentNormal(this.curInfoNode.ItemParent, false);
				rectTransform.localScale = Vector3.one * this.commonScale;
				uiitem.SetData(list[i].ToPropData());
				uiitem.OnRefresh();
			}
		}

		[SerializeField]
		private RectTransform BGRoot;

		[SerializeField]
		private CustomButton CloseButton;

		[SerializeField]
		private UIItem CopyItem;

		[SerializeField]
		private List<UIBoxInfoNode> InfoNodePool;

		private UIBoxInfoViewModule.Transfer _transferData;

		private LocalUnityObjctPool _itemPool;

		private UIBoxInfoNode curInfoNode;

		private float commonScale = 1f;

		public enum UIBoxInfoNodeType
		{
			Up,
			Right,
			Title
		}

		public class Transfer
		{
			public UIBoxInfoViewModule.UIBoxInfoNodeType nodeType;

			public List<ItemData> rewards;

			public Vector3 position;

			public Vector3 anchoredPositionOffset;

			public bool secondLayer;

			public string title;

			public string info;

			public bool IsSetArrowPos;
		}
	}
}
