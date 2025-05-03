using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIMainButton_Sign : BaseUIMainButton
	{
		public override bool IsShow()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Sign, false);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 5;
			subPriority = 0;
		}

		protected override void OnInit()
		{
			this.signDataModule = GameApp.Data.GetDataModule(DataName.SignDataModule);
			this.m_button.m_onClick = new Action(this.OnClickButton);
			this.m_redNode.SetType(240);
			this.m_redNode.Value = 0;
			this.OnRefreshStyle();
			if (this.m_animator != null)
			{
				this.m_animator.SetTrigger("Idle");
			}
			RedPointController.Instance.RegRecordChange("Main.Sign", new Action<RedNodeListenData>(this.OnRedPointChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_SignIn_DataUpdate, new HandlerEvent(this.OnSignInDataUpdate));
		}

		protected override void OnDeInit()
		{
			RedPointController.Instance.UnRegRecordChange("Main.Sign", new Action<RedNodeListenData>(this.OnRedPointChange));
			this.m_button.m_onClick = null;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_SignIn_DataUpdate, new HandlerEvent(this.OnSignInDataUpdate));
		}

		public override void OnRefresh()
		{
			this.OnRefreshAnimation();
		}

		public override void OnLanguageChange()
		{
			this.OnRefreshNameText();
		}

		private void OnSignInDataUpdate(object sender, int eventID, BaseEventArgs eventArgs)
		{
			this.OnRefreshStyle();
		}

		private void OnClickButton()
		{
			if (!this.IsShow())
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(101));
				return;
			}
			GameApp.View.OpenView(ViewName.SignViewModule, null, 1, null, null);
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
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.nameTextId);
			}
		}

		private void OnRefreshStyle()
		{
			this.OnRefreshNameText();
		}

		public const string REDPOINT_NAME = "Main.Sign";

		public CustomButton m_button;

		public CustomText m_nameTxt;

		public RedNodeOneCtrl m_redNode;

		public Animator m_animator;

		private SignDataModule signDataModule;

		private string nameTextId = "2309";
	}
}
