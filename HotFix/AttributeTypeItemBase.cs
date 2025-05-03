using System;
using UnityEngine;

namespace HotFix
{
	public abstract class AttributeTypeItemBase
	{
		public void Init(GameObject o)
		{
			this.m_gameObject = o;
			this.m_rectTransform = this.m_gameObject.transform as RectTransform;
			this.OnInit();
		}

		protected virtual void OnInit()
		{
		}

		public void DeInit()
		{
			this.OnDeInit();
		}

		protected virtual void OnDeInit()
		{
		}

		public void SetData(AttributeTypeDataBase typeData, string commaString)
		{
			this.OnSetData(typeData, commaString);
		}

		protected abstract void OnSetData(AttributeTypeDataBase typeData, string commaString);

		public void SetString(string str)
		{
			this.OnSetString(str);
		}

		protected virtual void OnSetString(string str)
		{
		}

		public float GetWidth()
		{
			return this.m_width;
		}

		public virtual GameObject GetFlyItem()
		{
			return null;
		}

		public GameObject m_gameObject;

		public RectTransform m_rectTransform;

		protected float m_width;
	}
}
