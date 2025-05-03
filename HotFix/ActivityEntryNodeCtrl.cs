using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class ActivityEntryNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public CommonActivity_CommonActivity actCfg;

		public CustomButton btn_Entry;

		public CustomImage icon_Entry;

		public CustomText txt_Name;

		public CustomText txt_Duration;

		public CustomText txt_Left;

		public RedNodeOneCtrl redNodeOneCtrl;

		public int actId;

		public bool isEnd;

		public long endTime;
	}
}
