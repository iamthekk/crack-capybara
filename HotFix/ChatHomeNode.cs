using System;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class ChatHomeNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.chatPanel.Init();
			this.chatPanel.OnBack = new Action(this.OnBackPanelView);
			this.miniUI.Init();
			this.miniUI.OnClickViewArea = new Action<ChatHomeSubPanelType>(this.OnClickMiniView);
			this.SetChatPanelView(ChatHomeSubPanelType.Null);
		}

		protected override void OnDeInit()
		{
			this.chatPanelSeqPool.Clear(false);
			this.miniUI.DeInit();
			this.chatPanel.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.miniUI.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void SetView(bool isShow)
		{
			this.miniUI.SetActive(isShow);
			if (isShow)
			{
				this.miniUI.OnShow();
			}
			else
			{
				this.miniUI.OnHide();
				this.SetChatPanelView(ChatHomeSubPanelType.Null);
			}
			if (!isShow)
			{
				base.gameObject.SetActiveSafe(isShow);
				return;
			}
			if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Chat_Tab_Show, false))
			{
				base.gameObject.SetActiveSafe(isShow);
				return;
			}
			base.gameObject.SetActiveSafe(false);
		}

		public void OnRefreshShowByFunction(bool isCanShow)
		{
			if (isCanShow)
			{
				if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Chat_Tab_Show, false))
				{
					base.gameObject.SetActiveSafe(true);
					return;
				}
				base.gameObject.SetActiveSafe(false);
			}
		}

		private void OnClickMiniView(ChatHomeSubPanelType panelType)
		{
			this.SetChatPanelView(panelType);
		}

		private void OnBackPanelView()
		{
			this.SetChatPanelView(ChatHomeSubPanelType.Null);
		}

		private void SetChatPanelView(ChatHomeSubPanelType panelType)
		{
			this.chatPanelSeqPool.Clear(false);
			this.chatPanel.SetView(panelType);
			if (this.chatPanel.IsShow())
			{
				Sequence sequence = this.chatPanelSeqPool.Get();
				this.chatPanel.transform.localScale = new Vector3(1f, 0f, 1f);
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleY(this.chatPanel.transform, 1f, 0.25f), 27));
			}
		}

		[SerializeField]
		private ChatMiniUI miniUI;

		[SerializeField]
		private ChatHomePanel chatPanel;

		private readonly SequencePool chatPanelSeqPool = new SequencePool();
	}
}
