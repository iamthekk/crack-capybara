using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public abstract class UIBaseMainPageNode : CustomBehaviour
	{
		public void SetPageName(int pageName)
		{
			this.m_pageName = pageName;
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			this.OnShow(null);
		}

		public void Show(UIBaseMainPageNode.OpenData openData)
		{
			base.gameObject.SetActive(true);
			this.OnShow(openData);
		}

		public void Show(object param)
		{
			base.gameObject.SetActive(true);
			this.OnShow(param);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
			this.OnHide();
		}

		public void SetLoadObjects(LoadPool<GameObject> loadpool)
		{
			this.m_loadPages = loadpool;
		}

		protected abstract void OnShow(UIBaseMainPageNode.OpenData openData);

		protected virtual void OnShow(object param)
		{
		}

		protected abstract void OnHide();

		public abstract void OnLanguageChange();

		public int m_pageName;

		protected LoadPool<GameObject> m_loadPages;

		public enum EOriginType
		{
			Talent,
			Equip
		}

		public class OpenData
		{
			public UIBaseMainPageNode.EOriginType OriginType;
		}
	}
}
