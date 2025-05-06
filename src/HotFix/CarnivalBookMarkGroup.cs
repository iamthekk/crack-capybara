using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class CarnivalBookMarkGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void InitBookMarkBar(SevenDayCarnivalDataModule.ViewType viewType, int initIndex)
		{
			this.SetViewType(viewType);
			if (this.BookMarks == null)
			{
				this.BookMarks = new List<BookMarkCtrl>();
			}
			this.PrefabBookMark.SetActive(false);
			for (int i = this.BookMarks.Count; i < this.MaxBookMarkCount; i++)
			{
				BookMarkCtrl bookMarkCtrl = Object.Instantiate<BookMarkCtrl>(this.PrefabBookMark, this.BookMarkContents);
				bookMarkCtrl.SetActive(true);
				this.BookMarks.Add(bookMarkCtrl);
			}
			for (int j = 0; j < this.BookMarks.Count; j++)
			{
				if (j + 1 == initIndex)
				{
					BookMarkCtrl bookMarkCtrl2 = this.BookMarks[j];
				}
				BookMarkCtrl bookMarkCtrl3 = this.BookMarks[j];
				bookMarkCtrl3.Init(this._viewType, j + 1, new Action<BookMarkCtrl>(this.OnMarkClick));
				bookMarkCtrl3.NormalMarkRoot = this.BookMarkContents;
				bookMarkCtrl3.SetAsSelect(false);
			}
		}

		public void DelInit()
		{
			if (this.BookMarks != null)
			{
				for (int i = 0; i < this.BookMarks.Count; i++)
				{
					this.BookMarks[i].DeInit();
				}
			}
		}

		public void ClearSelect()
		{
			this.mCurentSelIndex = -1;
		}

		public void SetViewType(SevenDayCarnivalDataModule.ViewType viewType)
		{
			this._viewType = viewType;
			for (int i = 0; i < this.BookMarks.Count; i++)
			{
				this.BookMarks[i].SetViewType(viewType);
			}
		}

		public void ResfreshBookMarks(SevenDayCarnivalDataModule.ViewType viewType, int selectIndex)
		{
			this._viewType = viewType;
			BookMarkCtrl bookMarkCtrl = null;
			for (int i = 0; i < this.BookMarks.Count; i++)
			{
				if (i + 1 == selectIndex)
				{
					bookMarkCtrl = this.BookMarks[i];
				}
				BookMarkCtrl bookMarkCtrl2 = this.BookMarks[i];
				bookMarkCtrl2.Init(this._viewType, i + 1, new Action<BookMarkCtrl>(this.OnMarkClick));
				bookMarkCtrl2.NormalMarkRoot = this.BookMarkContents;
				bookMarkCtrl2.SetAsSelect(false);
			}
			this.OnMarkClick(bookMarkCtrl);
		}

		public void RefreshBookMarksRedNode(int nIndex = 0)
		{
			bool flag = nIndex > 0;
			for (int i = 0; i < this.BookMarks.Count; i++)
			{
				if (!flag || i + 1 == nIndex)
				{
					this.BookMarks[i].RefreshUI();
					if (flag)
					{
						break;
					}
				}
			}
		}

		private void OnMarkClick(BookMarkCtrl bookmark)
		{
			if (bookmark == null)
			{
				return;
			}
			if (!bookmark.IsUnlock())
			{
				EventArgsString instance = Singleton<EventArgsString>.Instance;
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("190");
				instance.SetData(infoByID);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance);
				return;
			}
			if (this.OnBookMarkSwitch == null)
			{
				HLog.LogError("[CarnivalBookMarkGroup]OnBookMarkSwitch(typeof Func<int, bool>) mast be setted!");
				return;
			}
			int index = bookmark.Index;
			if (this.mCurentSelIndex == index)
			{
				return;
			}
			this.mCurentSelIndex = index;
			if (!this.OnBookMarkSwitch(index))
			{
				return;
			}
			for (int i = 0; i < this.BookMarks.Count; i++)
			{
				this.BookMarks[i].SetAsSelect(this.BookMarks[i] == bookmark);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.BookMarkContents);
		}

		[SerializeField]
		private BookMarkCtrl PrefabBookMark;

		[SerializeField]
		public int MaxBookMarkCount = 7;

		[SerializeField]
		private List<BookMarkCtrl> BookMarks;

		[SerializeField]
		private RectTransform BookMarkContents;

		[SerializeField]
		public Func<int, bool> OnBookMarkSwitch;

		private int mCurentSelIndex = -1;

		private SevenDayCarnivalDataModule.ViewType _viewType;
	}
}
