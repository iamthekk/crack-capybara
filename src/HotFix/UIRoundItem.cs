using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIRoundItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int round)
		{
			this.mRound = round;
			this.textRound.text = round.ToString();
		}

		public void Refresh(int currentRound)
		{
			if (this.mRound < currentRound)
			{
				this.imageRound.sprite = this.spriteReg.GetSprite("select");
				this.textRound.color = this.colorSelect;
				return;
			}
			if (this.mRound > currentRound)
			{
				this.imageRound.sprite = this.spriteReg.GetSprite("unselect");
				this.textRound.color = this.colorUnSelect;
				return;
			}
			this.imageRound.sprite = this.spriteReg.GetSprite("current");
			this.textRound.color = this.colorCurrent;
		}

		public CustomImage imageRound;

		public CustomText textRound;

		public SpriteRegister spriteReg;

		public Color colorSelect;

		public Color colorCurrent;

		public Color colorUnSelect;

		private int mRound;
	}
}
