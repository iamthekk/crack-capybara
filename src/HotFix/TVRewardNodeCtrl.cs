using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class TVRewardNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.item.Init();
		}

		protected override void OnDeInit()
		{
			this.item.DeInit();
		}

		public string uid;

		public CustomButton btn;

		public RawImage banner;

		public GameObject rewardObj;

		public UIItem item;

		public RedNodeOneCtrl redNodeOneCtrl;
	}
}
