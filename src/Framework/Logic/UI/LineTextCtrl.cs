using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class LineTextCtrl : MonoBehaviour
	{
		private void Awake()
		{
			this.OnRefresh();
		}

		private void OnRefresh()
		{
			this.m_rectTransform = base.transform as RectTransform;
			this.allWidth = this.m_rectTransform.sizeDelta.x;
			if (this.Image_LeftLine != null)
			{
				this.Image_LeftLine.rectTransform.anchoredPosition = new Vector2(this.LeftRightPadding, this.Image_LeftLine.rectTransform.anchoredPosition.y);
			}
			if (this.Image_RightLine != null)
			{
				this.Image_RightLine.rectTransform.anchoredPosition = new Vector2(this.LeftRightPadding, this.Image_RightLine.rectTransform.anchoredPosition.y);
			}
		}

		private void LateUpdate()
		{
			if (this.AutoRefresh && this.Text_Content != null && this.m_tempText != this.Text_Content.text)
			{
				this.m_tempText = this.Text_Content.text;
				this.onRefreshLayout();
			}
		}

		private void OnDestroy()
		{
		}

		private void onRefreshLayout()
		{
			if (this.Text_Content == null)
			{
				return;
			}
			float preferredWidth = this.Text_Content.preferredWidth;
			float num = (this.allWidth - this.LeftRightPadding * 2f - this.Interval * 2f - preferredWidth) / 2f;
			if (this.Image_LeftLine != null)
			{
				this.Image_LeftLine.rectTransform.sizeDelta = new Vector2(num, this.Image_LeftLine.rectTransform.sizeDelta.y);
			}
			if (this.Image_RightLine != null)
			{
				this.Image_RightLine.rectTransform.sizeDelta = new Vector2(num, this.Image_RightLine.rectTransform.sizeDelta.y);
				this.Image_RightLine.rectTransform.anchoredPosition = new Vector2(-this.LeftRightPadding - num, this.Image_RightLine.rectTransform.anchoredPosition.y);
			}
		}

		public void ForceLayout()
		{
			this.onRefreshLayout();
		}

		public void SetText(string text)
		{
			if (this.Text_Content != null)
			{
				this.Text_Content.text = text;
				this.ForceLayout();
				return;
			}
			HLog.LogError("LineTextCtrl.SetText() error , Text_Content is null!!!");
		}

		public float Interval = 10f;

		public float LeftRightPadding = 100f;

		public Image Image_LeftLine;

		public Image Image_RightLine;

		public Text Text_Content;

		[Tooltip("开启后自动在 LateUpdate 中检测文本变化并刷新布局，但是可能会有延时，如果需要立即刷新请调用ForceLayout()")]
		public bool AutoRefresh;

		private string m_tempText = "";

		private RectTransform m_rectTransform;

		private float allWidth;
	}
}
