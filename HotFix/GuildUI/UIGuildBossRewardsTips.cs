using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildBossRewardsTips : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.PrefabItem.SetActive(false);
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.GuildUI_OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void GuildUI_OnUnInit()
		{
			this.m_seqPool.Clear(false);
		}

		public void SetData(List<PropData> datalist)
		{
			this.mItemDataList.Clear();
			if (datalist != null)
			{
				this.mItemDataList.AddRange(datalist);
			}
		}

		public void RefreshUI()
		{
			this.RTFItemRoot.DestroyChildren();
			this.mUIItemList.Clear();
			for (int i = 0; i < this.mItemDataList.Count; i++)
			{
				PropData propData = this.mItemDataList[i];
				GameObject gameObject = Object.Instantiate<GameObject>(this.PrefabItem, this.RTFItemRoot);
				UIItem component = gameObject.GetComponent<UIItem>();
				component.SetData(propData);
				component.onClick = new Action<UIItem, PropData, object>(this.OnClickItem);
				component.OnRefresh();
				gameObject.SetActive(true);
				this.mUIItemList.Add(component);
			}
		}

		private void OnClickItem(UIItem item, PropData data, object arg3)
		{
			DxxTools.UI.OnItemClick(item, data, arg3);
		}

		public void AttachTo(Transform tf)
		{
			this.AttachNode.position = tf.position;
		}

		public void PlayHide(bool ani = false)
		{
			this.mIsShow = false;
			if (ani)
			{
				TweenSettingsExtensions.Append(this.m_seqPool.Get(), ShortcutExtensions.DOScale(this.AttachNode, 0f, 0.15f));
				return;
			}
			this.AttachNode.localScale = Vector3.zero;
		}

		public void PlayShow(bool ani = false)
		{
			this.mIsShow = true;
			if (ani)
			{
				TweenSettingsExtensions.Append(this.m_seqPool.Get(), ShortcutExtensions.DOScale(this.AttachNode, 1f, 0.15f));
				return;
			}
			this.AttachNode.localScale = Vector3.one;
		}

		public RectTransform AttachNode;

		public RectTransform RTFItemRoot;

		public GameObject PrefabItem;

		private List<UIItem> mUIItemList = new List<UIItem>();

		private List<PropData> mItemDataList = new List<PropData>();

		private SequencePool m_seqPool = new SequencePool();

		private bool mIsShow;
	}
}
