using System;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public abstract class UIRechargeGiftFundNodeBase : CustomBehaviour
	{
		public void SetIndex(int index)
		{
			this._index = index;
		}

		protected override void OnInit()
		{
			this._isOpen = true;
			this.OnNodeInit();
			this.child.anchoredPosition = new Vector2(-1000f, 0f);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(this._seqPool.Get(), (float)this._index * 0.06f), ShortcutExtensions46.DOAnchorPosX(this.child, 0f, 0.15f, false));
		}

		protected override void OnDeInit()
		{
			this._isOpen = false;
			this._seqPool.Clear(false);
			this.OnNodeDeInit();
		}

		public void Refresh()
		{
			if (!this._isOpen)
			{
				return;
			}
			this.OnRefresh();
		}

		protected abstract void OnRefresh();

		protected abstract void OnNodeInit();

		protected abstract void OnNodeDeInit();

		[Header("根节点的唯一子节点，做动画使用")]
		public RectTransform child;

		protected bool _isOpen;

		private int _index;

		protected SequencePool _seqPool = new SequencePool();
	}
}
