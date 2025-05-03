using System;
using System.Collections.Generic;
using Framework;

namespace HotFix
{
	public class ChapterActivityShowRewardHelper : Singleton<ChapterActivityShowRewardHelper>
	{
		public void ShowData(List<ItemData> rewards, Action onAction)
		{
			ChapterActivityShowRewardHelper.ShowRewardData showRewardData = new ChapterActivityShowRewardHelper.ShowRewardData();
			showRewardData.rewards = rewards;
			showRewardData.onAction = onAction;
			this.list.Add(showRewardData);
			if (!GameApp.View.IsOpened(ViewName.RewardCommonViewModule))
			{
				DxxTools.UI.OpenRewardCommon(showRewardData.rewards, new Action(this.OnCloseShow), false);
			}
		}

		private void OnCloseShow()
		{
			if (this.list.Count > 0)
			{
				Action onAction = this.list[0].onAction;
				if (onAction != null)
				{
					onAction();
				}
				this.list.RemoveAt(0);
			}
			if (this.list.Count > 0)
			{
				DxxTools.UI.OpenRewardCommon(this.list[0].rewards, new Action(this.OnCloseShow), false);
			}
		}

		public List<ChapterActivityShowRewardHelper.ShowRewardData> list = new List<ChapterActivityShowRewardHelper.ShowRewardData>();

		public class ShowRewardData
		{
			public List<ItemData> rewards;

			public Action onAction;
		}
	}
}
