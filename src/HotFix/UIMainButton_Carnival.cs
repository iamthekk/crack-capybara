using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIMainButton_Carnival : BaseUIMainButton
	{
		public override bool IsShow()
		{
			SevenDayCarnivalDataModule dataModule = GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule);
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Carnival, false) && !dataModule.IfTaskTimeOut;
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 3;
			subPriority = 0;
		}

		protected override void OnInit()
		{
			this.m_button.m_onClick = new Action(this.OnClickButton);
			this.m_redNode.SetType(240);
			this.m_redNode.Value = 0;
			this.OnRefreshNameText();
			if (!this.m_animator.gameObject.activeSelf)
			{
				return;
			}
			this.m_animator.SetTrigger("Idle");
			RedPointController.Instance.RegRecordChange("Main.Carnival", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			RedPointController.Instance.UnRegRecordChange("Main.Carnival", new Action<RedNodeListenData>(this.OnRedPointChange));
			this.m_button.m_onClick = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
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
				base.gameObject.SetActive(false);
				return;
			}
			GameApp.View.OpenView(ViewName.SevenDayCarnivalViewModule, null, 1, null, null);
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
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9510106");
			}
		}

		protected override void OnUpdatePerSecond()
		{
			base.OnUpdatePerSecond();
			base.gameObject.SetActive(this.IsShow());
		}

		public CustomButton m_button;

		public CustomText m_nameTxt;

		public RedNodeOneCtrl m_redNode;

		public Animator m_animator;

		public const string REDPOINT_NAME = "Main.Carnival";
	}
}
