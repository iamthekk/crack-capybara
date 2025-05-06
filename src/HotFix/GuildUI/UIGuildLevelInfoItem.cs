using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildLevelInfoItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void Refresh(GuildShareDataEx.LevelInfoData data)
		{
			if (data == null)
			{
				return;
			}
			this.textInfo.text = data.info;
			if (data.curProgress >= data.needProgress)
			{
				this.textProgress.text = GuildProxy.Language.GetInfoByID("400216");
			}
			else if (data.curProgress < data.needProgress)
			{
				this.textProgress.text = string.Format("{0}/{1}", data.curProgress, data.needProgress);
			}
			float num = (float)data.curProgress / (float)data.needProgress;
			num = ((num > 1f) ? num : num);
			this.sliderProgress.value = num;
		}

		public CustomText textInfo;

		public CustomText textProgress;

		public Slider sliderProgress;
	}
}
