using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class PetStarUpTo : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int oldStar, int newStar)
		{
		}

		public CustomImage imgStarOld;

		public CustomImage imgStarNew;

		public CustomText txtStarOld;

		public CustomText txtStarNew;

		public GameObject goArrow;
	}
}
