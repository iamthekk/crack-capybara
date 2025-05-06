using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public abstract class IAPShopMainEquipActivityBase : CustomBehaviour
	{
		public RectTransform fg;

		public CustomButton btnDrawOne;

		public CustomButton btnDrawTen;

		public CommonCostItem costItemOne;

		public CommonCostItem costItemTen;

		public CustomText txtTitle;

		public CustomTextScrollView txtDesc;

		public CustomText txtTip1;

		public CustomText txtTip2;

		public CustomImage imgIcon;

		public CustomImage imgSPlus;

		public RedNodeOneCtrl redNode1;

		public RedNodeOneCtrl redNode10;

		public CustomButton btnProbability;
	}
}
