using System;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class TowerProgressNode : CustomBehaviour
	{
		public RectTransform TipItemPoint { get; private set; }

		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int index, int currentProgress)
		{
			this.image.gameObject.SetActiveSafe(index < currentProgress);
			if (index == 0)
			{
				this.image.sprite = this.fg1;
				return;
			}
			this.image.sprite = this.fg2;
		}

		public Image image;

		public Sprite fg1;

		public Sprite fg2;
	}
}
