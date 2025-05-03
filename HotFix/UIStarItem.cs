using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIStarItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(bool isLightStar)
		{
			if (isLightStar)
			{
				this.imageStar.sprite = this.spriteRegister.GetSprite("star1");
				return;
			}
			this.imageStar.sprite = this.spriteRegister.GetSprite("star0");
		}

		public CustomImage imageStar;

		public SpriteRegister spriteRegister;
	}
}
