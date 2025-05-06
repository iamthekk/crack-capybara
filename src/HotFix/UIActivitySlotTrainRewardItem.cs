using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIActivitySlotTrainRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
		}

		public ActivityTurntable_TurntableReward cfg;

		public UIItem uiItem;

		public CustomText textNeed;

		public GameObject maskObj;

		public CustomImage canPick;

		public RedNodeOneCtrl redNode;

		public GameObject canPickEffect;
	}
}
