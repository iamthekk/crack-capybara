using System;
using HotFix.ChatUI.ChatItemUI;
using SuperScrollView;
using UnityEngine;

namespace HotFix.ChatUI
{
	public class ChatOutputItemSizeCtrl
	{
		public bool IsPlaying
		{
			get
			{
				return this.mPlayTime > 0f;
			}
		}

		public void PlayItem(ChatOutputItemSizeCtrl.PlayType type, float time = 1f)
		{
			this.PType = type;
			this.mMaxPlayTime = time;
			if ((double)this.mMaxPlayTime < 0.01)
			{
				this.mMaxPlayTime = 0.01f;
			}
			this.mPlayTime = this.mMaxPlayTime;
		}

		public void SetBottomItem(ChatItemBase ui)
		{
			if (this.BottomItem == ui)
			{
				return;
			}
			if (this.BottomItem != null && this.BottomItem != ui)
			{
				this.BottomItem.SetSizeRate(1f);
				this.BottomItem = null;
			}
			this.BottomItem = ui;
		}

		public void Update(float deltatime)
		{
			if (this.mPlayTime > 0f)
			{
				this.mPlayTime -= deltatime;
				if (this.mPlayTime < 0f)
				{
					this.mPlayTime = 0f;
				}
				if (this.PType == ChatOutputItemSizeCtrl.PlayType.NewBottomItem)
				{
					this.SetItemSize((this.mMaxPlayTime - this.mPlayTime) / this.mMaxPlayTime);
				}
			}
		}

		public void CompleteNow()
		{
			if (this.PType == ChatOutputItemSizeCtrl.PlayType.NewBottomItem)
			{
				this.SetItemSize(1f);
			}
		}

		private void SetItemSize(float rate)
		{
			rate = Mathf.Clamp01(rate);
			if (this.PType == ChatOutputItemSizeCtrl.PlayType.NewBottomItem && this.BottomItem != null)
			{
				this.BottomItem.SetSizeRate(rate);
				if (this.KeepToBottom)
				{
					this.LoopListUI.ScrollRect.verticalNormalizedPosition = 0f;
				}
			}
		}

		public LoopListView2 LoopListUI;

		public ChatOutputItemSizeCtrl.PlayType PType;

		private float mMaxPlayTime = 0.2f;

		private float mPlayTime;

		public ChatItemBase BottomItem;

		public bool KeepToBottom;

		public enum PlayType
		{
			NewBottomItem
		}
	}
}
