using System;
using System.Runtime.CompilerServices;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPRechargeViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
		}

		public override void OnOpen(object data)
		{
			IAPRechargeViewModule.<OnOpen>d__5 <OnOpen>d__;
			<OnOpen>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<OnOpen>d__.<>4__this = this;
			<OnOpen>d__.<>1__state = -1;
			<OnOpen>d__.<>t__builder.Start<IAPRechargeViewModule.<OnOpen>d__5>(ref <OnOpen>d__);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.rechargeNodeCtrl)
			{
				this.rechargeNodeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			if (this.rechargeNodeCtrl)
			{
				this.rechargeNodeCtrl.DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.GiftRechargeViewModule, null);
		}

		[SerializeField]
		private CustomButton buttonMask;

		[SerializeField]
		private GameObject parent;

		private IAPRechargeNodeCtrl rechargeNodeCtrl;

		private const string NodePath = "Assets/_Resources/Prefab/UI/IAPGift/Node/{0}.prefab";
	}
}
