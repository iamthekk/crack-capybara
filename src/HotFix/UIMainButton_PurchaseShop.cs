using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainButton_PurchaseShop : BaseUIMainButton
	{
		public override bool IsShow()
		{
			return GameApp.Purchase.IsEnable && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.IAPShop, false);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 2;
			subPriority = 0;
		}

		protected override void OnInit()
		{
			this.m_button.onClick.AddListener(new UnityAction(this.OnClickButton));
			this.OnRefreshNameText();
			this.m_animator.SetTrigger(UIMainButton_PurchaseShop.Idle);
		}

		protected override void OnDeInit()
		{
			this.m_button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
		}

		public override void OnRefresh()
		{
		}

		public override void OnLanguageChange()
		{
			this.OnRefreshNameText();
		}

		public override void OnRefreshAnimation()
		{
			if (this.m_redNode != null && this.m_animator != null)
			{
				int num = ((this.m_redNode.count > 0) ? UIMainButton_PurchaseShop.Shake : UIMainButton_PurchaseShop.Idle);
				this.m_animator.ResetTrigger(UIMainButton_PurchaseShop.Shake);
				this.m_animator.ResetTrigger(UIMainButton_PurchaseShop.Idle);
				this.m_animator.SetTrigger(num);
			}
		}

		private void OnClickButton()
		{
			IAPShopViewModule.OpenData openData = new IAPShopViewModule.OpenData(IAPShopJumpTabData.CreateMain(IAPMainSubType.DiamondsPack));
			GameApp.View.OpenView(ViewName.IAPShopViewModule, openData, 1, null, null);
		}

		private void OnRefreshNameText()
		{
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(3214);
			}
		}

		[SerializeField]
		private CustomButton m_button;

		[SerializeField]
		private CustomText m_nameTxt;

		[SerializeField]
		private Animator m_animator;

		[SerializeField]
		private RedNodeOneCtrl m_redNode;

		private static readonly int Shake = Animator.StringToHash("Shake");

		private static readonly int Idle = Animator.StringToHash("Idle");
	}
}
