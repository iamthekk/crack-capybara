using System;
using System.Collections.Generic;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class EventArgsFlyItemFlyItemDatas : BaseEventArgs
	{
		public void SetData(FlyItemModel model, List<ItemData> itemDatas, OnFlyNodeItemDatasItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			this.m_model = model;
			this.m_itemDatas = itemDatas;
			this.m_onItemFinished = onItemFinished;
			this.m_onFinished = onFinished;
			this.m_flyItemDatas = null;
			this.m_onFlyItemFinished = null;
			this.useCustomStartPos = false;
		}

		public void SetData(FlyItemModel model, List<ItemData> itemDatas, Vector3 startPos, OnFlyNodeItemDatasItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			this.m_model = model;
			this.m_itemDatas = itemDatas;
			this.m_onItemFinished = onItemFinished;
			this.m_onFinished = onFinished;
			this.m_flyItemDatas = null;
			this.m_onFlyItemFinished = null;
			this.m_startPos = startPos;
			this.useCustomStartPos = true;
		}

		public void SetData(FlyItemModel model, List<FlyItemData> itemDatas, OnFlyNodeFlyNodeOthersItemFinished onItemFinished = null, Action<BaseFlyNode> onFinished = null)
		{
			this.m_model = model;
			this.m_flyItemDatas = itemDatas;
			this.m_onFlyItemFinished = onItemFinished;
			this.m_onFinished = onFinished;
			this.m_itemDatas = null;
			this.m_onItemFinished = null;
			this.useCustomStartPos = false;
		}

		public override void Clear()
		{
		}

		public FlyItemModel m_model;

		public List<ItemData> m_itemDatas;

		public List<FlyItemData> m_flyItemDatas;

		public Vector3 m_startPos;

		public bool useCustomStartPos;

		public OnFlyNodeItemDatasItemFinished m_onItemFinished;

		public OnFlyNodeFlyNodeOthersItemFinished m_onFlyItemFinished;

		public Action<BaseFlyNode> m_onFinished;
	}
}
