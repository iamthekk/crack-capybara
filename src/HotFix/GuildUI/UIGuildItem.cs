using System;
using Dxx.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		public GuildItemData GuildItemData
		{
			get
			{
				return this.mItemData;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.NItemCtrl = this.PrefabItem.GetComponent<UIItem>();
			this.NItemCtrl.Init();
			this.NItemCtrl.onClick = new Action<UIItem, PropData, object>(this.OnClickItem);
		}

		protected override void GuildUI_OnUnInit()
		{
			UIItem nitemCtrl = this.NItemCtrl;
			if (nitemCtrl != null)
			{
				nitemCtrl.DeInit();
			}
			this.NItemCtrl = null;
		}

		private void OnClickItem(UIItem ctrl, PropData data, object obj)
		{
			if (base.gameObject == null)
			{
				return;
			}
			Action<UIGuildItem> onClick = this.OnClick;
			if (onClick != null)
			{
				onClick(this);
			}
			if (!this.IgnoreDefaultItemClick && this.OnClick == null)
			{
				GuildProxy.UI.ShowItemInfo(ctrl, data, obj);
			}
		}

		public void SetItem(GuildItemData item)
		{
			this.mItemData = item;
			this.mNItemData = UIGuildItem.GetNativeData(this.mItemData);
		}

		public virtual void RefreshUI()
		{
			if (this.NItemCtrl != null)
			{
				this.NItemCtrl.SetData(this.mNItemData);
				this.NItemCtrl.OnRefresh();
			}
		}

		public static PropData GetNativeData(GuildItemData data)
		{
			if (data == null)
			{
				return null;
			}
			return new PropData
			{
				rowId = (ulong)data.rowId,
				id = (uint)data.id,
				count = (ulong)data.count
			};
		}

		public bool IgnoreDefaultItemClick;

		public Action<UIGuildItem> OnClick;

		private GuildItemData mItemData;

		public GameObject PrefabItem;

		public UIItem NItemCtrl;

		private PropData mNItemData;
	}
}
