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
	public class CustomButtonIconAndText : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_button.onClick.AddListener(new UnityAction(this.InternalOnClick));
		}

		protected override void OnDeInit()
		{
			if (this.m_button != null)
			{
				this.m_button.onClick.RemoveListener(new UnityAction(this.InternalOnClick));
			}
			this.m_button = null;
			this.itemParent = null;
			this.m_text = null;
		}

		public void SetText(string text)
		{
			if (this.m_text == null)
			{
				return;
			}
			this.m_text.text = text;
			RectTransform rectTransform = this.m_text.rectTransform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.x = this.m_text.preferredWidth;
			rectTransform.sizeDelta = sizeDelta;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.itemParent);
		}

		public void SetItem(ItemData itemdata)
		{
			if (itemdata == null)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemdata.ID);
			if (elementById != null)
			{
				this.m_image.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			}
			this.SetText(string.Format("X{0}", itemdata.TotalCount));
		}

		private void InternalOnClick()
		{
			Action onClick = this.m_onClick;
			if (onClick == null)
			{
				return;
			}
			onClick();
		}

		public RectTransform itemParent;

		public CustomImage m_image;

		public CustomText m_text;

		public CustomButton m_button;

		public Action m_onClick;
	}
}
