using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class CarnivalBoxItemCtrl : CarnivalItemBaseCtrl
	{
		protected override void OnInit()
		{
			base.OnInit();
			this.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Gray);
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
		}

		public override void SetOpen(CarnivalItemBaseCtrl.BoxState eState)
		{
			switch (eState)
			{
			case CarnivalItemBaseCtrl.BoxState.State_Gray:
				this.Image_Box.sprite = this.boxClose;
				return;
			case CarnivalItemBaseCtrl.BoxState.State_Normal:
				this.Image_Box.sprite = this.boxReward;
				return;
			case CarnivalItemBaseCtrl.BoxState.State_Opened:
				this.Image_Box.sprite = this.boxOpen;
				return;
			default:
				return;
			}
		}

		public override void SetActiveText(string textInfo)
		{
			this.ActiveText.text = textInfo;
		}

		[SerializeField]
		private CustomButton Button;

		[SerializeField]
		private CustomImage Image_Box;

		[SerializeField]
		private CustomText ActiveText;

		[SerializeField]
		private Sprite boxClose;

		[SerializeField]
		private Sprite boxOpen;

		[SerializeField]
		private Sprite boxReward;
	}
}
