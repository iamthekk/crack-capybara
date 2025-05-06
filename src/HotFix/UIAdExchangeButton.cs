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
	public class UIAdExchangeButton : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.button.onClick.AddListener(new UnityAction(this.InternalClick));
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.InternalClick));
		}

		public void SetData(int itemId, int exchangeNum, int adWatchTime, int adWatchLimit, Action click)
		{
			this.onClick = click;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById != null)
			{
				this.textItem.text = string.Format("{0}x{1}", Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID), exchangeNum);
			}
			else
			{
				this.textItem.text = "";
			}
			this.textNum.text = string.Format("({0}/{1})", adWatchLimit - adWatchTime, adWatchLimit);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutTrans);
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

		public CustomText textItem;

		public CustomText textNum;

		public CustomButton button;

		public RectTransform layoutTrans;

		private Action onClick;
	}
}
