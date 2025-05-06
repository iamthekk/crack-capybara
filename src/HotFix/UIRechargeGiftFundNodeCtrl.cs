using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIRechargeGiftFundNodeCtrl : UIRechargeGiftFundNodeBase
	{
		protected sealed override void OnInit()
		{
			this.buttonSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
			base.OnInit();
		}

		protected sealed override void OnDeInit()
		{
			this.buttonSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			base.OnDeInit();
		}

		protected override void OnNodeInit()
		{
		}

		protected override void OnNodeDeInit()
		{
		}

		protected override void OnRefresh()
		{
		}

		protected virtual void OnClickSelf()
		{
			GameApp.View.OpenView(ViewName.IAPFundViewModule, IAPFundViewModule.FundType.BattlePass, 1, null, null);
		}

		protected void OnRedPoint(RedNodeListenData redData)
		{
			if (this.redNode == null)
			{
				return;
			}
			this.redNode.gameObject.SetActive(redData.m_count > 0);
		}

		public GameObject activeObj;

		public GameObject inActiveObj;

		public CustomText textTips;

		public RectTransform timeRectTrans;

		public RectTransform infoRectTrans;

		public CustomText textInfo;

		public CustomText textContent;

		public CustomImage imageIcon;

		public CustomButton buttonSelf;

		public RedNodeOneCtrl redNode;
	}
}
