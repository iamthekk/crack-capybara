using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ItemTipNode : CustomBehaviour
	{
		public Action<ItemTipNode> OnFinished { get; set; }

		protected override void OnInit()
		{
			if (this.animListen != null)
			{
				this.animListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		protected override void OnDeInit()
		{
			if (this.animListen != null)
			{
				this.animListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		public void SetInfo(int itemId, string tip)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById != null)
			{
				this.iconImage.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			}
			this.tipText.text = tip;
		}

		private void OnAnimatorListen(GameObject gameObj, string eventParameter)
		{
			if (string.Equals(eventParameter, "End"))
			{
				Action<ItemTipNode> onFinished = this.OnFinished;
				if (onFinished == null)
				{
					return;
				}
				onFinished(this);
			}
		}

		public AnimatorListen animListen;

		public CustomImage iconImage;

		public CustomText tipText;
	}
}
