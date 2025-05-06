using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class BookMarkCtrl : CustomBehaviour
	{
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		protected override void OnInit()
		{
			this._carnivalDataModule = GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule);
		}

		protected override void OnDeInit()
		{
			this.OnSwitchBookMark = null;
			this.ButtonCtrl.onClick.RemoveAllListeners();
			this.ButtonCtrl.m_onClick = null;
			this.ButtonCtrl.onDown = null;
			this.ButtonCtrl.onUp = null;
		}

		public void Init(SevenDayCarnivalDataModule.ViewType viewType, int index, Action<BookMarkCtrl> onswitchbookmark)
		{
			base.Init();
			this._index = index;
			this.SetViewType(viewType);
			this.OnSwitchBookMark = onswitchbookmark;
			this.ButtonCtrl.m_onClick = delegate
			{
				Action<BookMarkCtrl> onSwitchBookMark = this.OnSwitchBookMark;
				if (onSwitchBookMark == null)
				{
					return;
				}
				onSwitchBookMark(this);
			};
			this.DayLockText.text = Singleton<LanguageManager>.Instance.GetInfoByID("191", new object[] { this._index });
			this.DayUnlockText.text = Singleton<LanguageManager>.Instance.GetInfoByID("191", new object[] { this._index });
			this.RefreshUI();
		}

		public void SetViewType(SevenDayCarnivalDataModule.ViewType viewType)
		{
			this._viewType = viewType;
		}

		private void SetRedShow(bool value)
		{
			this.RedNode.gameObject.SetActive(value);
		}

		private bool IfShowRed()
		{
			return SevenDayCarnivalDataModule.IsTaskOneShowRed(this._carnivalDataModule, this._viewType, this._index);
		}

		public bool IsUnlock()
		{
			return this._index <= this._carnivalDataModule.UnLockDay;
		}

		public void RefreshUI()
		{
			this.ObjLock.SetActive(!this.IsUnlock());
			this.SetRedShow(this.IfShowRed());
		}

		public void SetAsSelect(bool sel)
		{
			this.ObjSel.SetActive(sel);
			this.ObjUnSel.SetActive(!sel);
			if (!this.IsUnlock())
			{
				this.ObjLock.SetActive(!this.IsUnlock());
				this.DayUnlockText.gameObject.SetActive(false);
				return;
			}
			this.ObjLock.SetActive(!this.IsUnlock());
			this.DayUnlockText.gameObject.SetActive(true);
		}

		public void SetParent(Transform tfparent)
		{
		}

		public CustomText DayUnlockText;

		public CustomText DayLockText;

		public CustomButton ButtonCtrl;

		public GameObject ObjLock;

		public GameObject ObjSel;

		public GameObject ObjUnSel;

		public RedNodeOneCtrl RedNode;

		private int _index;

		private SevenDayCarnivalDataModule.ViewType _viewType;

		private Action<BookMarkCtrl> OnSwitchBookMark;

		[HideInInspector]
		public Transform BigMarkRoot;

		[HideInInspector]
		public Transform NormalMarkRoot;

		private SevenDayCarnivalDataModule _carnivalDataModule;
	}
}
