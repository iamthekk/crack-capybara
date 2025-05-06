using System;
using System.Collections.Generic;
using Dxx.Chat;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatCell_ShareItem : ChatProxy.ChatProxy_BaseBehaviour
	{
		public RectTransform RTFRoot
		{
			get
			{
				return base.rectTransform;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.UIItem.Init();
			for (int i = 0; i < this.TypeNodeList.Count; i++)
			{
				if (this.TypeNodeList[i] != null)
				{
					this.TypeNodeList[i].SetActive(false);
				}
			}
		}

		protected override void ChatUI_OnUnInit()
		{
			if (this.UIItem != null)
			{
				this.UIItem.DeInit();
			}
			this.TypeNodeList.Clear();
		}

		public void SetData(ChatData chatdata)
		{
			this.mChatData = chatdata;
		}

		public void RefreshUI()
		{
			ItemType itemType = (ItemType)this.mChatData.itemType;
			string text = "";
			if (!this.mTypeNodeNameDic.TryGetValue(itemType, out text))
			{
				text = "";
			}
			for (int i = 0; i < this.TypeNodeList.Count; i++)
			{
				GameObject gameObject = this.TypeNodeList[i];
				gameObject.SetActive(gameObject.name == text);
			}
			if (itemType == ItemType.eEquip)
			{
				this.RefreshAsEquip();
			}
		}

		public void RefreshAsEquip()
		{
		}

		public void RefreshAsItem()
		{
		}

		public void RefreshAsHero()
		{
		}

		public void RefreshAsPet()
		{
		}

		[Header("装备")]
		[Header("英雄")]
		[Header("宠物")]
		[Header("物品")]
		public UIItem UIItem;

		[Header("所有类型节点")]
		public List<GameObject> TypeNodeList = new List<GameObject>();

		private ChatData mChatData;

		private Dictionary<ItemType, string> mTypeNodeNameDic = new Dictionary<ItemType, string>
		{
			{
				ItemType.eEquip,
				"EquipNode"
			},
			{
				ItemType.eHero,
				"HeroNode"
			}
		};
	}
}
