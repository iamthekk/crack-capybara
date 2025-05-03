using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIPageCtrl : CustomBehaviour
	{
		public int TotalPage { get; private set; }

		public int CurrentPage { get; private set; }

		protected override void OnInit()
		{
			if (this.btnPre != null)
			{
				this.btnPre.m_onClick = new Action(this.OnClickPre);
			}
			if (this.btnNext != null)
			{
				this.btnNext.m_onClick = new Action(this.OnClickNext);
			}
			for (int i = this.transPagePointParent.childCount - 1; i >= 0; i--)
			{
				GameObject gameObject = this.transPagePointParent.GetChild(i).gameObject;
				gameObject.SetActive(false);
				if (gameObject != this.objPagePointTemplate)
				{
					this.pagePointPool.Add(gameObject);
				}
			}
			if (this.redNodePre != null)
			{
				this.redNodePre.Value = 0;
			}
			if (this.redNodeNext != null)
			{
				this.redNodeNext.Value = 0;
			}
		}

		protected override void OnDeInit()
		{
			if (this.btnPre != null)
			{
				this.btnPre.m_onClick = null;
			}
			if (this.btnNext != null)
			{
				this.btnNext.m_onClick = null;
			}
			this.onPageChangePreCheck = null;
			this.onPageChangedPre = null;
			this.onPageChanged = null;
			this.onPageChangedNext = null;
		}

		public void SetPage(int totalPage, int currentPage)
		{
			this.TotalPage = Mathf.Max(0, totalPage);
			if (this.TotalPage < 1 || (this.TotalPage == 1 && !this.onePageShow))
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			this.CurrentPage = Mathf.Clamp(currentPage, 0, this.TotalPage);
			if (!this.showPageBtn)
			{
				if (this.btnPre != null)
				{
					this.btnPre.gameObject.SetActive(false);
				}
				if (this.btnNext != null)
				{
					this.btnNext.gameObject.SetActive(false);
				}
			}
			for (int i = 0; i < this.pagePointList.Count; i++)
			{
				this.pagePointList[i].SetActive(false);
				this.pagePointPool.Add(this.pagePointList[i]);
			}
			this.pagePointList.Clear();
			if (this.showPagePoint)
			{
				this.transPagePointParent.gameObject.SetActive(true);
				this.transPagePointCurrent.gameObject.SetActive(true);
				for (int j = 0; j < this.TotalPage; j++)
				{
					GameObject gameObject;
					if (this.pagePointPool.Count > 0)
					{
						gameObject = this.pagePointPool[0];
						this.pagePointPool.RemoveAt(0);
					}
					else
					{
						gameObject = Object.Instantiate<GameObject>(this.objPagePointTemplate, this.transPagePointParent);
					}
					gameObject.SetActive(true);
					gameObject.transform.SetAsLastSibling();
					this.pagePointList.Add(gameObject);
				}
			}
			else
			{
				this.transPagePointParent.gameObject.SetActive(false);
				this.transPagePointCurrent.gameObject.SetActive(false);
			}
			this.FreshUI();
		}

		private void FreshUI()
		{
			if (this.showPageBtn)
			{
				if (this.btnPre != null)
				{
					this.btnPre.gameObject.SetActive(this.CurrentPage > 0 || this.alwaysShowBtnPre);
				}
				if (this.btnNext != null)
				{
					this.btnNext.gameObject.SetActive(this.CurrentPage < this.TotalPage - 1 || this.alwaysShowBtnNext);
				}
			}
			if (this.showPagePoint)
			{
				this.transPagePointCurrent.SetParent(this.pagePointList[this.CurrentPage].transform);
				this.transPagePointCurrent.localPosition = Vector3.zero;
			}
		}

		public void SetRedNodePre(bool show)
		{
			if (this.redNodePre != null)
			{
				this.redNodePre.Value = (show ? 1 : 0);
			}
		}

		public void SetRedNodeNext(bool show)
		{
			if (this.redNodeNext != null)
			{
				this.redNodeNext.Value = (show ? 1 : 0);
			}
		}

		private void OnClickPre()
		{
			if (this.CurrentPage < 1)
			{
				return;
			}
			Predicate<int> predicate = this.onPageChangePreCheck;
			if (predicate != null && !predicate(this.CurrentPage - 1))
			{
				return;
			}
			int currentPage = this.CurrentPage;
			this.CurrentPage = currentPage - 1;
			Action<UIPageCtrl> action = this.onPageChangedPre;
			if (action != null)
			{
				action(this);
			}
			Action<UIPageCtrl> action2 = this.onPageChanged;
			if (action2 != null)
			{
				action2(this);
			}
			this.FreshUI();
		}

		private void OnClickNext()
		{
			if (this.CurrentPage >= this.TotalPage - 1)
			{
				return;
			}
			Predicate<int> predicate = this.onPageChangePreCheck;
			if (predicate != null && !predicate(this.CurrentPage + 1))
			{
				return;
			}
			int currentPage = this.CurrentPage;
			this.CurrentPage = currentPage + 1;
			Action<UIPageCtrl> action = this.onPageChangedNext;
			if (action != null)
			{
				action(this);
			}
			Action<UIPageCtrl> action2 = this.onPageChanged;
			if (action2 != null)
			{
				action2(this);
			}
			this.FreshUI();
		}

		[SerializeField]
		private CustomButton btnPre;

		[SerializeField]
		private CustomButton btnNext;

		[SerializeField]
		private GameObject objPagePointTemplate;

		[SerializeField]
		private Transform transPagePointParent;

		[SerializeField]
		private Transform transPagePointCurrent;

		[SerializeField]
		private RedNodeOneCtrl redNodePre;

		[SerializeField]
		private RedNodeOneCtrl redNodeNext;

		public bool showPagePoint = true;

		public bool showPageBtn = true;

		public bool onePageShow;

		public bool alwaysShowBtnPre;

		public bool alwaysShowBtnNext;

		public Predicate<int> onPageChangePreCheck;

		public Action<UIPageCtrl> onPageChangedPre;

		public Action<UIPageCtrl> onPageChanged;

		public Action<UIPageCtrl> onPageChangedNext;

		private List<GameObject> pagePointPool = new List<GameObject>();

		private List<GameObject> pagePointList = new List<GameObject>();
	}
}
