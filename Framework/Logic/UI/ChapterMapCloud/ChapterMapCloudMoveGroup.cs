using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI.ChapterMapCloud
{
	public class ChapterMapCloudMoveGroup : MonoBehaviour
	{
		private void Awake()
		{
			this.RandomPosOffset(this.Clouds);
		}

		private void Update()
		{
			if (this.screenWidth <= 0)
			{
				this.screenWidth = Utility.UI.Width;
			}
			if (this.screenHeight <= 0)
			{
				this.screenHeight = Utility.UI.Height;
			}
			Vector2 vector = this.MoveSpeed * Time.deltaTime;
			for (int i = 0; i < this.Clouds.Count; i++)
			{
				RectTransform rectTransform = this.Clouds[i];
				if (!(rectTransform == null))
				{
					Utility.UI.MoveUIInScreen(rectTransform, vector, Vector2.zero);
				}
			}
		}

		public void ClearChildList()
		{
			this.Clouds.Clear();
		}

		public void SetChildList(List<RectTransform> childs)
		{
			this.Clouds.Clear();
			this.Clouds.AddRange(childs);
			this.RandomPosOffset(this.Clouds);
		}

		private void RandomPosOffset(List<RectTransform> cloudRectTransforms)
		{
			foreach (RectTransform rectTransform in cloudRectTransforms)
			{
				if (!(rectTransform == null))
				{
					Vector2 anchoredPosition = rectTransform.anchoredPosition;
					anchoredPosition.x += Random.Range(this.RandomPosOffsetMin.x, this.RandomPosOffsetMax.x);
					anchoredPosition.y += Random.Range(this.RandomPosOffsetMin.y, this.RandomPosOffsetMax.y);
					rectTransform.anchoredPosition = anchoredPosition;
				}
			}
		}

		public List<RectTransform> Clouds = new List<RectTransform>();

		public Vector2 MoveSpeed = new Vector2(30f, -5f);

		public Vector2 RandomPosOffsetMin = new Vector2(0f, 0f);

		public Vector2 RandomPosOffsetMax = new Vector2(0f, 0f);

		private int screenWidth;

		private int screenHeight;
	}
}
