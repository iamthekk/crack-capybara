using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class ItemCurrencyUICtrl : CustomBehaviour
	{
		public Item_Item ItemCfg { get; private set; }

		protected override void OnInit()
		{
			this.Button.m_onClick = new Action(this.OnClickCurrency);
		}

		protected override void OnDeInit()
		{
		}

		private void OnClickCurrency()
		{
			Action<ItemCurrencyUICtrl> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public void SetData(int itemId, string formatNum = "{0}")
		{
			this.ItemId = itemId;
			this.FormatNum = formatNum;
		}

		public void SetFresh(int itemId, string formatNum = "{0}")
		{
			this.SetData(itemId, formatNum);
			this.FreshAll();
		}

		public void FreshAll()
		{
			if (this.ItemId > 0)
			{
				base.gameObject.SetActiveSafe(true);
				this.ItemCfg = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.ItemId);
				this.CustomSetIcon(GameApp.Table.GetAtlasPath(this.ItemCfg.atlasID), this.ItemCfg.icon);
				this.FreshNum();
				return;
			}
			base.gameObject.SetActiveSafe(false);
		}

		public void FreshNum()
		{
			if (this.ItemId > 0)
			{
				long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.ItemId));
				this.CustomSetText(string.Format(this.FormatNum, DxxTools.FormatNumber(itemDataCountByid)));
				return;
			}
			this.CustomSetText("");
		}

		public void CustomSetIcon(string atlasPath, string spriteName)
		{
			if (this.Icon != null)
			{
				this.Icon.SetImage(atlasPath, spriteName);
			}
		}

		public void CustomSetText(string text)
		{
			if (this.Text == null)
			{
				return;
			}
			this.Text.text = text;
		}

		public CustomImage Icon;

		public CustomButton Button;

		public CustomText Text;

		public int ItemId;

		public string FormatNum = "{0}";

		public Action<ItemCurrencyUICtrl> OnClick;
	}
}
