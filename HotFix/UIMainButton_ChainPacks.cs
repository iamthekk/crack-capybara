using System;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIMainButton_ChainPacks : BaseUIMainButton
	{
		public ChainPacksDataModule.ChainPacksTypeBind ChainPacksTypeBind { get; private set; }

		public FunctionID Function_ID
		{
			get
			{
				return this.ChainPacksTypeBind.Function_ID;
			}
		}

		public string Function_TargetName
		{
			get
			{
				return this.ChainPacksTypeBind.Function_TargetName;
			}
		}

		public string REDPOINT_NAME
		{
			get
			{
				return this.ChainPacksTypeBind.REDPOINT_NAME;
			}
		}

		public int UIMainButton_Priority
		{
			get
			{
				return this.ChainPacksTypeBind.UIMainButton_Priority;
			}
		}

		public override bool IsShow()
		{
			return this.dataModule.CanShow(this.m_ChainPacksType);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = this.UIMainButton_Priority;
			subPriority = 0;
		}

		public void PreInit()
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChainPacksDataModule);
			this.ChainPacksTypeBind = this.dataModule.GetChainPacksTypeBind(this.m_ChainPacksType);
			if (this.ChainPacksTypeBind == null)
			{
				HLog.LogError("ChainPacksType setting error : " + base.gameObject.name);
			}
		}

		protected override void OnInit()
		{
			this.m_button.m_onClick = new Action(this.OnClickButton);
			this.m_redNode.SetType(240);
			this.m_redNode.Value = 0;
			this.OnRefreshNameText();
			this.m_animator.SetTrigger("Idle");
			RedPointController.Instance.RegRecordChange(this.REDPOINT_NAME, new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			RedPointController.Instance.UnRegRecordChange(this.REDPOINT_NAME, new Action<RedNodeListenData>(this.OnRedPointChange));
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
			GameApp.View.OpenView(ViewName.ChainPacksViewModule, null, 1, null, null);
		}

		private void OnRedPointChange(RedNodeListenData data)
		{
			base.SetActive(this.IsShow());
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
				Function_Function function_Function = GameApp.Table.GetManager().GetFunction_Function((int)this.Function_ID);
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(function_Function.nameID);
			}
		}

		protected override void OnUpdatePerSecond()
		{
			if (!this.IsShow())
			{
				base.SetActive(false);
			}
		}

		public ChainPacksDataModule.ChainPacksType m_ChainPacksType;

		public CustomButton m_button;

		public CustomText m_nameTxt;

		public RedNodeOneCtrl m_redNode;

		public Animator m_animator;

		private ChainPacksDataModule dataModule;
	}
}
