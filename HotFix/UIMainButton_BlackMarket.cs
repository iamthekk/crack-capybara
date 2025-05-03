using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIMainButton_BlackMarket : BaseUIMainButton
	{
		public override bool IsShow()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BlackMarket, false);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 13;
			subPriority = 0;
		}

		protected override void OnInit()
		{
			this.m_button.m_onClick = new Action(this.OnClickButton);
			this.m_redNode.SetType(240);
			this.m_redNode.Value = 0;
			this.OnRefreshNameText();
			RedPointController.Instance.RegRecordChange("Main.BlackMarket", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			RedPointController.Instance.UnRegRecordChange("Main.BlackMarket", new Action<RedNodeListenData>(this.OnRedPointChange));
			this.m_button.m_onClick = null;
		}

		public override void OnRefresh()
		{
			this.OnRefreshAnimation();
		}

		public override void OnLanguageChange()
		{
			this.OnRefreshNameText();
		}

		private void OnClickButton()
		{
			if (!this.IsShow())
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(18));
				return;
			}
			GameApp.View.CloseView(ViewName.MoreExtensionViewModule, null);
			GuildProxy.UI.OpenBlackMarket();
		}

		private void OnRedPointChange(RedNodeListenData data)
		{
			if (this.m_redNode != null)
			{
				this.m_redNode.Value = data.m_count;
			}
			this.OnRefreshAnimation();
		}

		public override void OnRefreshAnimation()
		{
			if (this.m_redNode != null && this.m_animator != null)
			{
				if (!this.m_animator.gameObject.activeSelf)
				{
					return;
				}
				string text = ((this.m_redNode.count > 0) ? "Shake" : "Idle");
				this.m_animator.ResetTrigger("Shake");
				this.m_animator.ResetTrigger("Idle");
				this.m_animator.SetTrigger(text);
			}
		}

		private void OnRefreshNameText()
		{
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_more_blackmarket");
			}
		}

		public RectTransform GetButtonTransform()
		{
			if (this.m_button != null)
			{
				return this.m_button.transform as RectTransform;
			}
			return null;
		}

		public CustomButton m_button;

		public CustomText m_nameTxt;

		public RedNodeOneCtrl m_redNode;

		public Animator m_animator;

		public const string REDPOINT_NAME = "Main.BlackMarket";
	}
}
