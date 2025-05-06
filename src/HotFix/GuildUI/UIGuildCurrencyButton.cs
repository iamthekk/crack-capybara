using System;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildCurrencyButton : GuildProxy.GuildProxy_BaseBehaviour
	{
		public void SetData(int itemid, int count)
		{
			Item_Item itemTable = GuildProxy.Table.GetItemTable(itemid);
			if (itemTable != null)
			{
				GuildProxy.Resources.SetDxxImage(this.Image_Icon, itemTable.atlasID, itemTable.icon);
			}
			this.Text_Count.text = count.ToString();
			RectTransform rectTransform = this.Text_Count.rectTransform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.y = this.Text_Count.preferredWidth + 20f;
			rectTransform.sizeDelta = sizeDelta;
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform.parent as RectTransform);
		}

		private void InternalClick()
		{
			Action action = this.onClick;
			if (action == null)
			{
				return;
			}
			action();
		}

		protected override void GuildUI_OnInit()
		{
			if (this.Button_Currency != null)
			{
				this.Button_Currency.m_onClick = new Action(this.InternalClick);
			}
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public CustomImage Image_Icon;

		public CustomText Text_Count;

		public CustomButton Button_Currency;

		public Action onClick;
	}
}
