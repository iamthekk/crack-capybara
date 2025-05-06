using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace HotFix
{
	public class IAPDiamondsPanel : IAPShopPanelBase
	{
		public override IAPShopType PanelType
		{
			get
			{
				return IAPShopType.Diamonds;
			}
		}

		protected override void OnPreInit()
		{
			this.curDiamondsType = IAPDiamondsType.Null;
			this.tabParentPos = this.tabParent.anchoredPosition;
			foreach (IAPDiamondsSubTab iapdiamondsSubTab in this.tabNodeList)
			{
				iapdiamondsSubTab.Init();
				iapdiamondsSubTab.SetData(false, delegate(IAPDiamondsType shopType)
				{
					this.SwitchPanel(shopType, null);
				});
			}
			foreach (IAPDiamondsSubPanelBase iapdiamondsSubPanelBase in this.shopPanelList)
			{
				iapdiamondsSubPanelBase.Init();
			}
		}

		protected override void OnPreDeInit()
		{
			foreach (IAPDiamondsSubPanelBase iapdiamondsSubPanelBase in this.shopPanelList)
			{
				iapdiamondsSubPanelBase.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			foreach (IAPDiamondsSubPanelBase iapdiamondsSubPanelBase in this.shopPanelList)
			{
				iapdiamondsSubPanelBase.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnSelect(IAPShopJumpTabData jumpTabData)
		{
			base.OnSelect(jumpTabData);
			if (jumpTabData != null)
			{
				this.SwitchPanel(jumpTabData.DiamondsType, jumpTabData);
			}
			else if (this.lastUnSelectDiamondsType != IAPDiamondsType.Null)
			{
				this.SwitchPanel(this.lastUnSelectDiamondsType, null);
			}
			if (this.curDiamondsType == IAPDiamondsType.Null)
			{
				this.SwitchPanel(IAPDiamondsType.DiamondsPack, null);
			}
			this.PlaySelectAnim();
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
			this.lastUnSelectDiamondsType = this.curDiamondsType;
			this.SwitchPanel(IAPDiamondsType.Null, null);
		}

		private void SwitchPanel(IAPDiamondsType shopType, IAPShopJumpTabData jumpTabData = null)
		{
			if (shopType == this.curDiamondsType)
			{
				return;
			}
			IAPDiamondsType lastDiamondsType = this.curDiamondsType;
			this.curDiamondsType = shopType;
			foreach (IAPDiamondsSubTab iapdiamondsSubTab in this.tabNodeList)
			{
				iapdiamondsSubTab.SetSelect(iapdiamondsSubTab.TabType == this.curDiamondsType);
			}
			IAPDiamondsSubTab iapdiamondsSubTab2 = this.tabNodeList.Find((IAPDiamondsSubTab item) => item.TabType == this.curDiamondsType);
			IAPDiamondsSubTab iapdiamondsSubTab3 = this.tabNodeList.Find((IAPDiamondsSubTab item) => item.TabType == lastDiamondsType);
			float num = 0f;
			float num2 = 0f;
			if (iapdiamondsSubTab2 != null && iapdiamondsSubTab3 != null)
			{
				num = this.shopPanelParent.rect.width;
				if (iapdiamondsSubTab2.rectTransform.anchoredPosition.x < iapdiamondsSubTab3.rectTransform.anchoredPosition.x)
				{
					num = -num;
				}
				num2 = 0.4f;
			}
			this.sequencePool.Clear(true);
			Sequence sequence = this.sequencePool.Get();
			IAPDiamondsSubPanelBase lastShopPanel = this.shopPanelList.Find((IAPDiamondsSubPanelBase item) => item.PanelType == lastDiamondsType);
			IAPDiamondsSubPanelBase iapdiamondsSubPanelBase;
			if (this.GetShopPanel(this.curDiamondsType, out iapdiamondsSubPanelBase))
			{
				iapdiamondsSubPanelBase.SetSelect(jumpTabData);
				Vector2 vector;
				vector..ctor(0f, 0f);
				if (num2 > 0f)
				{
					iapdiamondsSubPanelBase.rectTransform.anchoredPosition = new Vector2(num, 0f);
					Tweener tweener = ShortcutExtensions46.DOAnchorPos(iapdiamondsSubPanelBase.rectTransform, vector, num2, false);
					TweenSettingsExtensions.Insert(sequence, 0f, tweener);
				}
				else
				{
					iapdiamondsSubPanelBase.rectTransform.anchoredPosition = vector;
				}
			}
			if (lastShopPanel != null)
			{
				if (num2 > 0f)
				{
					Tweener tweener2 = TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions46.DOAnchorPos(lastShopPanel.rectTransform, new Vector2(-num, 0f), num2, false), delegate
					{
						lastShopPanel.SetUnSelect();
					});
					TweenSettingsExtensions.Insert(sequence, 0f, tweener2);
					return;
				}
				lastShopPanel.SetUnSelect();
			}
		}

		private bool GetShopPanel(IAPDiamondsType shopType, out IAPDiamondsSubPanelBase shopPanel)
		{
			int num = this.shopPanelList.FindIndex((IAPDiamondsSubPanelBase item) => item.PanelType == shopType);
			if (num >= 0)
			{
				shopPanel = this.shopPanelList[num];
				return true;
			}
			IAPDiamondsSubPanelBase iapdiamondsSubPanelBase;
			if (base.ShopLoader.GetIAPDiamondsSubPanel(shopType, out iapdiamondsSubPanelBase))
			{
				shopPanel = Object.Instantiate<IAPDiamondsSubPanelBase>(iapdiamondsSubPanelBase, this.shopPanelParent);
				shopPanel.transform.localPosition = Vector3.zero;
				this.shopPanelList.Add(shopPanel);
				shopPanel.Init();
				return true;
			}
			shopPanel = null;
			return false;
		}

		private void PlaySelectAnim()
		{
			ShortcutExtensions.DOKill(this.tabParent, false);
			Vector2 vector = this.tabParentPos;
			vector.y -= 200f;
			this.tabParent.anchoredPosition = vector;
			ShortcutExtensions46.DOAnchorPosY(this.tabParent, this.tabParentPos.y, 0.25f, false);
		}

		[SerializeField]
		private RectTransform tabParent;

		[SerializeField]
		private List<IAPDiamondsSubTab> tabNodeList;

		[SerializeField]
		private RectTransform shopPanelParent;

		private readonly List<IAPDiamondsSubPanelBase> shopPanelList = new List<IAPDiamondsSubPanelBase>();

		private IAPDiamondsType curDiamondsType;

		private IAPDiamondsType lastUnSelectDiamondsType;

		private readonly SequencePool sequencePool = new SequencePool();

		private Vector2 tabParentPos;
	}
}
