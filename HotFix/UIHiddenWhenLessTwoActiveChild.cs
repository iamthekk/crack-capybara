using System;
using UnityEngine;

namespace HotFix
{
	public class UIHiddenWhenLessTwoActiveChild : MonoBehaviour
	{
		private void Start()
		{
			if (this.checkOnStart)
			{
				this.CheckHidden();
			}
		}

		public void CheckHidden()
		{
			int childCount = base.transform.childCount;
			if (childCount < 2)
			{
				base.gameObject.SetActive(false);
				return;
			}
			int num = 0;
			for (int i = 0; i < childCount; i++)
			{
				if (base.transform.GetChild(i).gameObject.activeSelf)
				{
					num++;
					if (num >= 2)
					{
						break;
					}
				}
			}
			base.gameObject.SetActive(num >= 2);
		}

		[SerializeField]
		private bool checkOnStart;
	}
}
