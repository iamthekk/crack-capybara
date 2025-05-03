using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIFunctionOpen : CustomBehaviour
	{
		protected void OnEnable()
		{
			if (Application.isPlaying)
			{
				this.OnInit();
				this.OnRefresh();
			}
		}

		protected void OnDisable()
		{
			if (Application.isPlaying)
			{
				this.OnDeInit();
			}
		}

		protected override void OnInit()
		{
			if (GameApp.Event == null)
			{
				return;
			}
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		protected override void OnDeInit()
		{
			if (GameApp.Event == null)
			{
				return;
			}
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void OnRefresh()
		{
			if (this.BindFunctionID != FunctionID.None && GameApp.Data != null)
			{
				this.CurrentState = (GameApp.Data.GetDataModule(DataName.FunctionDataModule).IsFunctionOpened(this.BindFunctionID) ? UIFunctionOpenShowState.UnLock : UIFunctionOpenShowState.Lock);
			}
			else if (this.BindFunctionID == FunctionID.None)
			{
				this.CurrentState = UIFunctionOpenShowState.UnLock;
			}
			this.RefreshFunctionOpenState();
		}

		public void RefreshShowLogic()
		{
			switch (this.ShowKind)
			{
			case UIFunctionOpenShowKind.Active:
				if (this.ShowLogic == null || !(this.ShowLogic is UIFunctionOpenShowLogic_Active))
				{
					this.ShowLogic = new UIFunctionOpenShowLogic_Active();
					this.ShowLogic.FunctionUI = this;
					this.ShowLogic.RefreshState(this.CurrentState);
				}
				break;
			case UIFunctionOpenShowKind.ExtureLockObject:
				if (this.ShowLogic == null || !(this.ShowLogic is UIFunctionOpenShowLogic_ExtureLockObject))
				{
					this.ShowLogic = new UIFunctionOpenShowLogic_ExtureLockObject();
					this.ShowLogic.FunctionUI = this;
					this.ShowLogic.RefreshState(this.CurrentState);
				}
				break;
			case UIFunctionOpenShowKind.ObjectSwitch:
				if (this.ShowLogic == null || !(this.ShowLogic is UIFunctionOpenShowLogic_ObjectSwitch))
				{
					this.ShowLogic = new UIFunctionOpenShowLogic_ObjectSwitch();
					this.ShowLogic.FunctionUI = this;
					this.ShowLogic.RefreshState(this.CurrentState);
				}
				break;
			}
			if (this.ShowLogic == null)
			{
				this.ShowLogic = new UIFunctionOpenShowLogic_None();
				this.ShowLogic.FunctionUI = this;
				this.ShowLogic.RefreshState(this.CurrentState);
			}
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			if (eventArgs == null)
			{
				return;
			}
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen != null && eventArgsFunctionOpen.FunctionID == (int)this.BindFunctionID)
			{
				this.CurrentState = UIFunctionOpenShowState.UnLock;
				this.RefreshFunctionOpenState();
			}
		}

		private void RefreshFunctionOpenState()
		{
			if (this.ShowLogic == null)
			{
				this.RefreshShowLogic();
			}
			if (this.ShowLogic != null)
			{
				this.ShowLogic.RefreshState(this.CurrentState);
			}
		}

		[Header("基础配置")]
		public FunctionID BindFunctionID;

		public UIFunctionOpenShowKind ShowKind = UIFunctionOpenShowKind.Active;

		[Header("绑定")]
		public GameObject LockObject;

		public GameObject UnLockObject;

		[Header("状态展示")]
		[Label]
		public UIFunctionOpenShowState CurrentState;

		public UIFunctionOpenShowLogicBase ShowLogic;
	}
}
