using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("Layout/Content Size Fitter Ex", 141)]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class ContentSizeFitterEx : UIBehaviour, ILayoutSelfController, ILayoutController
	{
		public ContentSizeFitterEx.FitMode horizontalFit
		{
			get
			{
				return this.m_HorizontalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitterEx.FitMode>(ref this.m_HorizontalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		public ContentSizeFitterEx.FitMode verticalFit
		{
			get
			{
				return this.m_VerticalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitterEx.FitMode>(ref this.m_VerticalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		protected ContentSizeFitterEx()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		private void HandleSelfFittingAlongAxis(int axis)
		{
			ContentSizeFitterEx.FitMode fitMode = ((axis == 0) ? this.horizontalFit : this.verticalFit);
			if (fitMode == ContentSizeFitterEx.FitMode.Unconstrained)
			{
				this.m_Tracker.Add(this, this.rectTransform, 0);
				return;
			}
			this.m_Tracker.Add(this, this.rectTransform, (axis == 0) ? 4096 : 8192);
			if (fitMode == ContentSizeFitterEx.FitMode.MinSize)
			{
				this.rectTransform.SetSizeWithCurrentAnchors(axis, LayoutUtility.GetMinSize(this.m_Rect, axis));
				return;
			}
			if (fitMode == ContentSizeFitterEx.FitMode.MaxSize)
			{
				this.rectTransform.SetSizeWithCurrentAnchors(axis, this.GetMaxSize(this.m_Rect, axis));
				return;
			}
			this.rectTransform.SetSizeWithCurrentAnchors(axis, LayoutUtility.GetPreferredSize(this.m_Rect, axis));
		}

		private float GetMaxSize(RectTransform rect, int axis)
		{
			float num = ((axis == 0) ? LayoutUtility.GetPreferredWidth(rect) : LayoutUtility.GetPreferredHeight(rect));
			if (axis == 0)
			{
				if (num > (float)this.m_MaxHorizontal)
				{
					return (float)this.m_MaxHorizontal;
				}
			}
			else if (num > (float)this.m_MaxVertical)
			{
				return (float)this.m_MaxVertical;
			}
			return num;
		}

		public virtual void SetLayoutHorizontal()
		{
			this.m_Tracker.Clear();
			this.HandleSelfFittingAlongAxis(0);
		}

		public virtual void SetLayoutVertical()
		{
			this.HandleSelfFittingAlongAxis(1);
		}

		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		[SerializeField]
		protected ContentSizeFitterEx.FitMode m_HorizontalFit;

		[SerializeField]
		protected int m_MaxHorizontal = 900;

		[SerializeField]
		protected ContentSizeFitterEx.FitMode m_VerticalFit;

		[SerializeField]
		protected int m_MaxVertical = 100;

		[NonSerialized]
		private RectTransform m_Rect;

		private DrivenRectTransformTracker m_Tracker;

		public enum FitMode
		{
			Unconstrained,
			MinSize,
			MaxSize,
			PreferredSize
		}
	}
}
