using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class IAPMainPanel : IAPShopPanelBase
	{
		public override IAPShopType PanelType
		{
			get
			{
				return IAPShopType.Main;
			}
		}

		protected override void OnPreInit()
		{
			for (int i = 0; i < this.packGroups.Count; i++)
			{
				this.packGroups[i].Init();
				this.packGroups[i].UpdateContent();
			}
			this.packGroups.Sort(delegate(MainShopPackGroupBase a, MainShopPackGroupBase b)
			{
				int num;
				int num2;
				a.GetPriority(out num, out num2);
				int num3;
				int num4;
				b.GetPriority(out num3, out num4);
				if (num == num3)
				{
					return num2 - num4;
				}
				return num - num3;
			});
			for (int j = 0; j < this.packGroups.Count; j++)
			{
				this.packGroups[j].transform.SetSiblingIndex(j);
			}
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventShopInfoData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoData));
		}

		protected override void OnPreDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventShopInfoData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoData));
			for (int i = 0; i < this.packGroups.Count; i++)
			{
				this.packGroups[i].DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.packGroups.Count; i++)
			{
				this.packGroups[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnSelect(IAPShopJumpTabData jumpTabData)
		{
			base.OnSelect(jumpTabData);
			if (jumpTabData != null)
			{
				switch (jumpTabData.MainSubType)
				{
				}
			}
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
		}

		public void RefreshAllContent()
		{
			for (int i = 0; i < this.packGroups.Count; i++)
			{
				this.packGroups[i].UpdateContent();
			}
			base.StartCoroutine(this.CheckGuide());
		}

		private void OnEventShopInfoData(object sender, int type, BaseEventArgs args)
		{
			for (int i = 0; i < this.packGroups.Count; i++)
			{
				this.packGroups[i].UpdateContent();
			}
		}

		private void OnEventIAPInfoData(object sender, int type, BaseEventArgs eventargs)
		{
			for (int i = 0; i < this.packGroups.Count; i++)
			{
				this.packGroups[i].UpdateContent();
			}
		}

		private IEnumerator CheckGuide()
		{
			yield return 1;
			if (GuideController.Instance.CurrentGuide != null)
			{
				for (int i = 0; i < this.packGroups.Count; i++)
				{
					if (this.contentRT != null && this.packGroups[i] is MainShopEquipPackGroup)
					{
						Vector2 anchoredPosition = this.packGroups[i].gameObject.GetComponent<RectTransform>().anchoredPosition;
						anchoredPosition.y = -anchoredPosition.y;
						this.contentRT.anchoredPosition = new Vector2(this.contentRT.anchoredPosition.x, anchoredPosition.y);
						break;
					}
				}
			}
			yield break;
		}

		public RectTransform contentRT;

		public List<MainShopPackGroupBase> packGroups = new List<MainShopPackGroupBase>();
	}
}
