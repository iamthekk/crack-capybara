using System;
using Framework.Logic.UI.Chat;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatItem_Loading : ChatItemBase
	{
		internal void SetAsLoading()
		{
			ChatItem_LoadingMono loadingMono = this.LoadingMono;
			if (loadingMono == null)
			{
				return;
			}
			loadingMono.SetAsLoading();
		}

		internal void SetAsNoMoreData()
		{
			ChatItem_LoadingMono loadingMono = this.LoadingMono;
			if (loadingMono == null)
			{
				return;
			}
			loadingMono.SetAsNoMoreData();
		}

		public override void SetSizeRate(float show)
		{
			base.RTFRoot.sizeDelta = new Vector2(base.RTFRoot.sizeDelta.x, (float)this.RootSizeHeight * Mathf.Clamp01(show));
		}

		public static float LoadingTipItemHeight = 100f;

		public ChatItem_LoadingMono LoadingMono;
	}
}
