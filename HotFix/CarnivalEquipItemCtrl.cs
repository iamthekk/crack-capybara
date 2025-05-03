using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class CarnivalEquipItemCtrl : CarnivalItemBaseCtrl
	{
		protected override void OnInit()
		{
			base.OnInit();
			this.SetOpen(false);
			this.EquipItem.Init();
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
		}

		public override void SetOpen(bool open)
		{
			this.EquipHaveGet.gameObject.SetActive(open);
		}

		public override void SetEquipInfo(PropData data)
		{
			this.EquipItem.SetData(data);
			this.EquipItem.OnRefresh();
		}

		public override void SetEquipClickCallBack(Action<UIItem, PropData, object> clickCallBack)
		{
			this.EquipItem.onClick = clickCallBack;
		}

		public override void SetActiveText(string textInfo)
		{
			this.ActiveText.text = textInfo;
		}

		[SerializeField]
		private UIItem EquipItem;

		[SerializeField]
		private RectTransform EquipHaveGet;

		[SerializeField]
		private CustomText ActiveText;
	}
}
