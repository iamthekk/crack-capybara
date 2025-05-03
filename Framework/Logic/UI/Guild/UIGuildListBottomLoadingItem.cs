using System;
using SuperScrollView;
using UnityEngine;

namespace Framework.Logic.UI.Guild
{
	public class UIGuildListBottomLoadingItem : MonoBehaviour
	{
		public void SetActive(bool show)
		{
			if (base.gameObject != null && base.gameObject.activeSelf != show)
			{
				base.gameObject.SetActive(show);
			}
		}

		public void SetAsNoMoreData()
		{
			this.ObjNoMoreData.SetActive(true);
			this.ObjLoading.SetActive(false);
		}

		public void SetAsLoading()
		{
			this.ObjNoMoreData.SetActive(false);
			this.ObjLoading.SetActive(true);
		}

		public void SetScroll(LoopListView2 scroll)
		{
			this.mScroll = scroll;
			this.mCurrentItem = base.GetComponent<LoopListViewItem2>();
			this.mView = this.mScroll.ScrollRect.viewport;
		}

		private void LateUpdate()
		{
			if (this.ObjLoading.activeSelf)
			{
				float num = this.RotateRTF.eulerAngles.z - this.RotateSpeed * Time.deltaTime;
				if (num <= -360f)
				{
					num += 360f;
				}
				this.RotateRTF.eulerAngles = new Vector3(0f, 0f, num);
				if (this.mView != null)
				{
					float num2 = this.mScroll.GetItemCornerPosInViewPort(this.mCurrentItem, 1).y + this.mScroll.ViewPortSize;
					if (num2 > this.MaxScaleSize)
					{
						num2 = this.MaxScaleSize;
					}
					this.ScaleRTF.sizeDelta = new Vector2(num2, num2);
				}
			}
		}

		public RectTransform RotateRTF;

		public RectTransform ScaleRTF;

		public float RotateSpeed = 720f;

		public float MaxScaleSize = 100f;

		public GameObject ObjNoMoreData;

		public GameObject ObjLoading;

		private LoopListViewItem2 mCurrentItem;

		private LoopListView2 mScroll;

		private RectTransform mView;
	}
}
