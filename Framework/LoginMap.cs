using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginMap : MonoBehaviour
{
	private void Start()
	{
		if (Application.systemLanguage == 22)
		{
			this.imageLogo.sprite = this.japan;
		}
		else if (Application.systemLanguage == 23)
		{
			this.imageLogo.sprite = this.korea;
		}
		else if (Application.systemLanguage == 41)
		{
			this.imageLogo.sprite = this.taiwan;
		}
		else
		{
			this.imageLogo.sprite = this.normal;
		}
		this.imageLogo.SetNativeSize();
	}

	public Image imageLogo;

	public Sprite normal;

	public Sprite japan;

	public Sprite korea;

	public Sprite taiwan;

	public Sprite chinese;
}
