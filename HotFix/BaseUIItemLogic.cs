using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public abstract class BaseUIItemLogic
	{
		public Item_Item m_tableData
		{
			get
			{
				if (this.m_item == null)
				{
					return null;
				}
				return this.m_item.m_tableData;
			}
		}

		public ItemType m_itemType
		{
			get
			{
				if (this.m_item == null)
				{
					return ItemType.eCurrency;
				}
				return this.m_item.m_itemType;
			}
		}

		public PropType m_propType
		{
			get
			{
				if (this.m_item == null)
				{
					return PropType.Null;
				}
				return this.m_item.m_propType;
			}
		}

		public virtual void SetItem(UIItem item)
		{
			this.m_item = item;
		}

		public virtual void OnRefresh()
		{
			if (this.m_item == null || this.m_tableData == null)
			{
				this.m_item.RefreshAsNull();
				return;
			}
			this.OnBeforeRefresh();
			this.OnRefreshIcon();
			this.m_item.OnRefreshCount();
			this.OnRefreshBackground();
			this.OnRefreshGift();
			this.OnRefreshCustom();
			this.OnRefreshSpecialTypeView();
		}

		protected virtual void OnBeforeRefresh()
		{
			this.m_item.CloseOther();
		}

		protected virtual void OnRefreshIcon()
		{
			this.m_item.SetIcon(GameApp.Table.GetAtlasPath(this.m_tableData.atlasID), this.m_tableData.icon);
			this.m_item.SetFragmentFrame(this.m_tableData.quality);
		}

		protected virtual void OnRefreshGift()
		{
			if (this.m_item == null)
			{
				return;
			}
			if (this.m_tableData.GetItemGift() == null)
			{
				if (this.m_item.m_headerParent != null)
				{
					this.m_item.m_headerParent.gameObject.SetActive(false);
					return;
				}
			}
			else if (this.m_item.m_headerParent != null)
			{
				this.m_item.m_headerParent.gameObject.SetActive(true);
			}
		}

		protected virtual void OnRefreshBackground()
		{
			this.m_item.SetBg(this.getAtlasPath(105), this.getAtlasItemBG(this.m_tableData.quality));
		}

		public abstract void OnRefreshCustom();

		public virtual void OnRefreshSpecialTypeView()
		{
			this.m_item.SetQualityTxtImageActive(false);
			this.m_item.SetTypeBgImageActive(false);
			this.m_item.SetQualityRank(1);
		}

		protected string getAtlasPath(int id = 105)
		{
			return GameApp.Table.GetAtlasPath(id);
		}

		protected string getAtlasItemBG(int quality)
		{
			return string.Format("item_frame_{0}", quality);
		}

		public UIItem m_item;
	}
}
