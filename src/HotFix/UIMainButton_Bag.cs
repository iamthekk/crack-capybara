using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIMainButton_Bag : BaseUIMainButton
	{
		public override bool IsShow()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Bag, false);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 11;
			subPriority = 0;
		}

		protected override void OnInit()
		{
			this.m_button.m_onClick = new Action(this.OnClickButton);
			this.m_redNode.SetType(240);
			this.m_redNode.Value = 0;
			this.OnRefreshNameText();
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd.SetData(FlyItemModel.Default, CurrencyType.DynamicGold, new List<Transform> { base.gameObject.transform });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd);
		}

		protected override void OnDeInit()
		{
			this.m_button.m_onClick = null;
		}

		public override void OnRefresh()
		{
		}

		public override void OnLanguageChange()
		{
			this.OnRefreshNameText();
		}

		private void OnClickButton()
		{
			if (!this.IsShow())
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(12));
				return;
			}
			GameApp.View.CloseView(ViewName.MoreExtensionViewModule, null);
			BagViewModule.OpenData openData = new BagViewModule.OpenData();
			openData.srcViewName = ViewName.MoreExtensionViewModule;
			GameApp.View.OpenView(ViewName.BagViewModule, openData, 1, null, null);
		}

		private void OnRedPointChange(RedPointDataRecord record)
		{
		}

		public override void OnRefreshAnimation()
		{
			if (this.m_redNode != null && this.m_animator != null)
			{
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
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_more_bag");
			}
		}

		public CustomButton m_button;

		public CustomText m_nameTxt;

		public RedNodeOneCtrl m_redNode;

		public Animator m_animator;

		public const string REDPOINT_NAME = "Main.Bag";
	}
}
