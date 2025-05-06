using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	[ExecuteInEditMode]
	public class UIMainShopTab : IAPTabBase<MainShopType>
	{
		protected override void OnSelect()
		{
			base.OnSelect();
			this.UpdateView(true);
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
			this.UpdateView(false);
		}

		private void UpdateView(bool isSelect)
		{
			this.imgSelectBg.gameObject.SetActive(isSelect);
			this.imgUnSelectBg.gameObject.SetActive(!isSelect);
			this.txtSelect.gameObject.SetActive(isSelect);
			this.txtUnSelect.gameObject.SetActive(!isSelect);
		}

		[SerializeField]
		private CustomImage imgSelectBg;

		[SerializeField]
		private CustomImage imgUnSelectBg;

		[SerializeField]
		private CustomLanguageText txtSelect;

		[SerializeField]
		private CustomLanguageText txtUnSelect;
	}
}
