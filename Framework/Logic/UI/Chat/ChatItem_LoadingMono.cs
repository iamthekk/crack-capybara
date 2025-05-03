using System;
using SuperScrollView;
using UnityEngine;

namespace Framework.Logic.UI.Chat
{
	public class ChatItem_LoadingMono : MonoBehaviour
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

		public void SetScroll(LoopListView2 scroll, LoopListViewItem2 item2)
		{
			this.Scroll = scroll;
			this.CurrentItem = item2;
			this.RTFView = this.Scroll.ScrollRect.viewport;
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
				if (this.RTFView != null)
				{
					float num2 = -this.Scroll.GetItemCornerPosInViewPort(this.CurrentItem, 0).y;
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					float num3 = num2;
					if (num3 > this.MaxScaleSize)
					{
						num3 = this.MaxScaleSize;
					}
					this.ScaleRTF.sizeDelta = new Vector2(num3, num3);
				}
			}
		}

		public RectTransform RotateRTF;

		public RectTransform ScaleRTF;

		public float RotateSpeed = 720f;

		public float MaxScaleSize = 100f;

		public GameObject ObjNoMoreData;

		public GameObject ObjLoading;

		[HideInInspector]
		public RectTransform RTFView;

		[HideInInspector]
		public LoopListViewItem2 CurrentItem;

		[HideInInspector]
		public LoopListView2 Scroll;
	}
}
