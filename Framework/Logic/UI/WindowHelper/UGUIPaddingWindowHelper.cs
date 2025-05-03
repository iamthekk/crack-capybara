using System;
using Framework.Logic.Platform;
using UnityEngine;

namespace Framework.Logic.UI.WindowHelper
{
	public class UGUIPaddingWindowHelper : MonoBehaviour
	{
		private void Awake()
		{
			this.OnRefresh();
		}

		public void OnRefresh()
		{
			if (this.m_target == null)
			{
				return;
			}
			if (this.m_target == null)
			{
				return;
			}
			this.m_offsetMin = this.m_target.offsetMin;
			this.m_offectMax = this.m_target.offsetMax;
			float num = (this.m_ignoreTop ? 0f : Singleton<PlatformHelper>.Instance.GetTopHeight());
			float num2 = (this.m_ignoreBottom ? 0f : Singleton<PlatformHelper>.Instance.GetBottomHeight());
			this.m_target.offsetMin = this.m_offsetMin + new Vector2((float)this.m_addPadding.left, num2 + (float)this.m_addPadding.bottom);
			this.m_target.offsetMax = this.m_offectMax + new Vector2((float)this.m_addPadding.right, num + (float)this.m_addPadding.top);
		}

		public RectTransform m_target;

		public RectOffset m_addPadding = new RectOffset();

		private Vector2 m_offsetMin;

		private Vector2 m_offectMax;

		[Header("忽略顶部")]
		public bool m_ignoreTop;

		[Header("忽略底部")]
		public bool m_ignoreBottom;
	}
}
