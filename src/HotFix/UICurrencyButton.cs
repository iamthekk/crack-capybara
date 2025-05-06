using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UICurrencyButton : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.button.onClick.AddListener(new UnityAction(this.InternalClick));
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.InternalClick));
		}

		public void SetData(int itemId, int count, Action click, bool showHave, bool setRed = true)
		{
			this.onClick = click;
			Item_Item itemTable = GuildProxy.Table.GetItemTable(itemId);
			if (itemTable != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(itemTable.atlasID);
				this.costIcon.SetImage(atlasPath, itemTable.icon);
			}
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)itemId));
			if (showHave)
			{
				this.textCost.text = string.Format("{0}/{1}", itemDataCountByid, count);
			}
			else
			{
				this.textCost.text = count.ToString();
			}
			if (itemDataCountByid >= (long)count)
			{
				this.textCost.color = Color.white;
			}
			else
			{
				this.textCost.color = (setRed ? Color.red : Color.white);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.textCost.rectTransform.parent as RectTransform);
		}

		public void SetData(int atlasId, string icon, int price, int have, Action click, bool setRed = true)
		{
			this.onClick = click;
			string atlasPath = GameApp.Table.GetAtlasPath(atlasId);
			this.costIcon.SetImage(atlasPath, icon);
			this.textCost.text = string.Format("x{0}", price);
			if (have >= price)
			{
				this.textCost.color = Color.white;
				return;
			}
			this.textCost.color = (setRed ? Color.red : Color.white);
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

		public CustomImage costIcon;

		public CustomText textCost;

		public CustomButton button;

		private Action onClick;
	}
}
