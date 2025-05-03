using System;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	[RequireComponent(typeof(SpriteRegister))]
	public class UILanguageHelper : CustomBehaviour
	{
		protected override void OnInit()
		{
			SpriteRegister component = base.GetComponent<SpriteRegister>();
			if (component != null && this.image != null)
			{
				if (Singleton<LanguageManager>.Instance.GameLanguage == 2)
				{
					this.image.sprite = component.GetSprite("cn");
				}
				else if (Singleton<LanguageManager>.Instance.GameLanguage == 3)
				{
					this.image.sprite = component.GetSprite("cnf");
				}
				else if (Singleton<LanguageManager>.Instance.GameLanguage == 11)
				{
					this.image.sprite = component.GetSprite("kr");
				}
				else if (Singleton<LanguageManager>.Instance.GameLanguage == 4)
				{
					this.image.sprite = component.GetSprite("jp");
				}
				else
				{
					this.image.sprite = component.GetSprite("en");
				}
				this.image.SetNativeSize();
			}
		}

		protected override void OnDeInit()
		{
		}

		public Image image;
	}
}
