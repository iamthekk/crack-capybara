using System;
using UnityEngine;

namespace HotFix
{
	public class UIItemStar : UIItemBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetStarCount(int starCount)
		{
			this.m_imageBG.SetActive(starCount > 0);
			if (starCount < 0)
			{
				return;
			}
			if (starCount > this.m_stars.Length)
			{
				return;
			}
			for (int i = 0; i < this.m_stars.Length; i++)
			{
				if (!(this.m_stars[i] == null))
				{
					this.m_stars[i].SetActive(i < starCount);
				}
			}
		}

		public GameObject[] m_stars = new GameObject[5];

		public GameObject m_imageBG;
	}
}
