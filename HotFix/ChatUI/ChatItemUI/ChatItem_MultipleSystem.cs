using System;
using System.Collections.Generic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatItem_MultipleSystem : ChatItemBase
	{
		protected override void ChatUI_OnInit()
		{
			base.ChatUI_OnInit();
			this.ObjCopy.SetActive(false);
		}

		protected override void ChatUI_OnUnInit()
		{
			base.ChatUI_OnUnInit();
		}

		public override void RefreshUI()
		{
			this.RTFCopyRoot.DestroyChildren();
			this.mContentTextList.Clear();
			Vector2 sizeDelta = base.RTFRoot.sizeDelta;
			float num = 0f;
			if (this.Data.ContentList != null)
			{
				string[] contentList = this.Data.ContentList;
				for (int i = 0; i < this.Data.ContentList.Length; i++)
				{
					string text = contentList[i];
					if (!string.IsNullOrEmpty(text))
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.ObjCopy, this.RTFCopyRoot);
						ChatItem_MultipleSystem.SubContent subContent = new ChatItem_MultipleSystem.SubContent();
						gameObject.SetActive(true);
						subContent.SetGameObject(gameObject);
						subContent.SetText(text);
						subContent.Resize();
						subContent.SetPos(num);
						num += subContent.SizeHeight() + 10f;
						this.mContentTextList.Add(subContent);
					}
				}
			}
			sizeDelta.y = num;
			base.RTFRoot.sizeDelta = sizeDelta;
		}

		public override void RefreshSize()
		{
			this.RootSizeHeight = (int)base.RTFRoot.sizeDelta.y;
		}

		public RectTransform RTFCopyRoot;

		public GameObject ObjCopy;

		private List<ChatItem_MultipleSystem.SubContent> mContentTextList = new List<ChatItem_MultipleSystem.SubContent>();

		public class SubContent
		{
			public void SetGameObject(GameObject obj)
			{
				this.gameObject = obj;
				this.RTFRoot = this.gameObject.transform as RectTransform;
				this.TextContent = this.RTFRoot.Find("Text").GetComponent<CustomText>();
			}

			public void SetText(string text)
			{
				this.TextContent.text = text;
			}

			public void Resize()
			{
				Vector2 sizeDelta = this.RTFRoot.sizeDelta;
				sizeDelta.x = this.TextContent.preferredWidth + 100f;
				this.RTFRoot.sizeDelta = sizeDelta;
			}

			public float SizeHeight()
			{
				return this.RTFRoot.sizeDelta.y;
			}

			public void SetPos(float ypos)
			{
				this.RTFRoot.anchoredPosition = new Vector2(0f, -ypos);
			}

			public GameObject gameObject;

			public RectTransform RTFRoot;

			public CustomText TextContent;
		}
	}
}
