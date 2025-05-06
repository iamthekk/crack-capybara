using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.UI;

namespace HotFix
{
	public class UICostNode : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(ItemData itemData)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			}
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)itemData.ID));
			if (this.m_valueTxt != null)
			{
				this.m_valueTxt.text = ((itemDataCountByid >= itemData.TotalCount) ? (DxxTools.FormatNumber(itemData.TotalCount) ?? "") : ("<color=red>" + DxxTools.FormatNumber(itemData.TotalCount) + "</color>"));
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		public void SetData(ItemData itemData, long currentCount)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			}
			string text = ((currentCount >= itemData.TotalCount) ? DxxTools.FormatNumber(currentCount) : ("<color=red>" + DxxTools.FormatNumber(currentCount) + "</color>"));
			if (this.m_valueTxt != null)
			{
				this.m_valueTxt.text = text + "/" + DxxTools.FormatNumber(itemData.TotalCount);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		public CustomImage m_icon;

		public CustomText m_valueTxt;
	}
}
