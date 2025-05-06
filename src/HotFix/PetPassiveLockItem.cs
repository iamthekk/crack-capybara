using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class PetPassiveLockItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
			this.onItemClickCallback = null;
		}

		public void InitData(Action<PetPassiveLockItem> callback)
		{
			this.onItemClickCallback = callback;
		}

		public void SetData(bool isLock)
		{
			this.imgIconLock.gameObject.SetActive(isLock);
			this.imgIconUnlock.gameObject.SetActive(!isLock);
		}

		public void SetActive(bool isActive)
		{
			base.gameObject.SetActive(isActive);
		}

		private void OnBtnItemClick()
		{
			Action<PetPassiveLockItem> action = this.onItemClickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public CustomButton btnItem;

		public CustomImage imgIconLock;

		public CustomImage imgIconUnlock;

		private Action<PetPassiveLockItem> onItemClickCallback;
	}
}
