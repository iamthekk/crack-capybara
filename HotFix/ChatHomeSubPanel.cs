using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class ChatHomeSubPanel : CustomBehaviour
	{
		public abstract ChatHomeSubPanelType PanelType { get; }

		private protected bool IsSelect { protected get; private set; }

		protected sealed override void OnInit()
		{
			this.OnPreInit();
			this.SetUnSelect(true);
		}

		protected sealed override void OnDeInit()
		{
			this.OnPreDeInit();
		}

		public void SetSelect(bool isForce = false)
		{
			if (!isForce && this.IsSelect)
			{
				return;
			}
			this.IsSelect = true;
			this.OnSelect();
		}

		public void SetUnSelect(bool isForce = false)
		{
			if (!isForce && !this.IsSelect)
			{
				return;
			}
			this.IsSelect = false;
			this.OnUnSelect();
		}

		public void ResetListView()
		{
			this.OnResetListView();
		}

		protected abstract void OnPreInit();

		protected abstract void OnPreDeInit();

		public abstract void OnLanguageChange();

		public virtual void OnResetListView()
		{
		}

		protected virtual void OnSelect()
		{
			base.gameObject.SetActive(true);
		}

		protected virtual void OnUnSelect()
		{
			base.gameObject.SetActive(false);
		}
	}
}
