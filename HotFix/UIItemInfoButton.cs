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
	public class UIItemInfoButton : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.OnDeInit();
			this.SetOnClick(null);
			this.SetCountText(string.Empty, false);
			this.SetInfoText(string.Empty);
			if (this.itemImage != null)
			{
				this.itemImage.sprite = null;
			}
			this.SetGrayState(false);
			this.customButton.onClick.AddListener(new UnityAction(this.OnCustomButtonClick));
		}

		protected override void OnDeInit()
		{
			this.SetOnClick(null);
			this.customButton.onClick.RemoveListener(new UnityAction(this.OnCustomButtonClick));
		}

		private void OnCustomButtonClick()
		{
			Action action = this.onClick;
			if (action == null)
			{
				return;
			}
			action();
		}

		public void SetOnClick(Action onClickVal)
		{
			this.onClick = onClickVal;
		}

		public void SetCountText(string countTxt, bool isSimple)
		{
			this.itemRoot.SetActive(!isSimple);
			this.simpleCountText.gameObject.SetActive(isSimple);
			(isSimple ? this.simpleCountText : this.countText).text = countTxt;
			this.ForceRebuildLayoutImmediate(isSimple);
		}

		public void SetCountTextByLanguageId(int languageId, bool isSimple)
		{
			this.itemRoot.SetActive(!isSimple);
			this.simpleCountText.gameObject.SetActive(isSimple);
			(isSimple ? this.simpleCountText : this.countText).SetText(languageId);
			this.ForceRebuildLayoutImmediate(isSimple);
		}

		public void SetInfoText(string countTxt)
		{
			this.infoText.text = countTxt;
		}

		public void SetInfoTextByLanguageId(int languageId)
		{
			this.infoText.SetText(languageId);
		}

		public void SetItemIcon(int itemId)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById == null)
			{
				this.itemImage.sprite = null;
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(elementById.atlasID);
			this.itemImage.SetImage(atlasPath, elementById.icon);
		}

		public void SetGrayState(bool isGray)
		{
			if (isGray)
			{
				this.uiGrays.SetUIGray();
			}
			else
			{
				this.uiGrays.Recovery();
			}
			this.SetButtonEnable(!isGray);
		}

		public void SetButtonEnable(bool isEnable)
		{
			this.customButton.enabled = isEnable;
		}

		private void ForceRebuildLayoutImmediate(bool isSimple)
		{
			if (!isSimple && this.countText != null)
			{
				RectTransform rectTransform = this.countText.transform.parent as RectTransform;
				if (rectTransform != null)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
				}
			}
		}

		[SerializeField]
		private CustomButton customButton;

		[SerializeField]
		private GameObject itemRoot;

		[SerializeField]
		private CustomImage itemImage;

		[SerializeField]
		private CustomText countText;

		[SerializeField]
		private CustomText simpleCountText;

		[SerializeField]
		private CustomText infoText;

		[SerializeField]
		private UIGrays uiGrays;

		[Tooltip("红点，非必须，按需设置")]
		[SerializeField]
		public RedNodeOneCtrl redNode;

		private Action onClick;
	}
}
