using System;
using System.Collections;
using UnityEngine;

namespace HotFix
{
	public class EquipShopPanel : MainShopPanelBase
	{
		public override MainShopType ShopPanelType
		{
			get
			{
				return MainShopType.EquipShop;
			}
		}

		protected override void OnPreInit()
		{
			this.originPos = this.scrollRectTransform.anchoredPosition;
			this.shopChapterPackGroup.Init();
			this.shopEquipSUpPoolActivityGroup.Init();
			this.shopEquipActivityPackGroup.Init();
			this.shopEquipPackGroup.Init();
		}

		protected override void OnPreDeInit()
		{
			this.shopChapterPackGroup.DeInit();
			this.shopEquipSUpPoolActivityGroup.DeInit();
			this.shopEquipActivityPackGroup.DeInit();
			this.shopEquipPackGroup.DeInit();
		}

		protected override void OnSelect(MainShopJumpTabData jumpTabData)
		{
			base.OnSelect(jumpTabData);
			Vector2 anchoredPosition = this.scrollContentRect.anchoredPosition;
			anchoredPosition.y = 0f;
			this.scrollContentRect.anchoredPosition = anchoredPosition;
			this.scrollRectTransform.anchoredPosition = new Vector2(anchoredPosition.x - 1080f, anchoredPosition.y);
			this.UpdateContent();
			if (this != null && base.isActiveAndEnabled)
			{
				if (this.IsTriggerGuide())
				{
					this.scrollRectTransform.anchoredPosition = anchoredPosition;
					base.StartCoroutine(this.CheckGuide());
					return;
				}
				base.StartCoroutine(this.PlayAnimation());
			}
		}

		private IEnumerator PlayAnimation()
		{
			yield return 0;
			this.scrollRectTransform.anchoredPosition = this.originPos;
			float time = Time.time;
			int num = this.shopChapterPackGroup.PlayAnimation(time, 0);
			num = this.shopEquipSUpPoolActivityGroup.PlayAnimation(time, num);
			num = this.shopEquipActivityPackGroup.PlayAnimation(time, num);
			this.shopEquipPackGroup.PlayAnimation(time, num);
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopChapterPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopEquipSUpPoolActivityGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopEquipActivityPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopEquipPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void UpdateContent()
		{
			this.shopChapterPackGroup.UpdateContent();
			this.shopEquipSUpPoolActivityGroup.UpdateContent();
			this.shopEquipActivityPackGroup.UpdateContent();
			this.shopEquipPackGroup.UpdateContent();
			if (!(this == null) && !(base.gameObject == null))
			{
				bool isActiveAndEnabled = base.isActiveAndEnabled;
			}
		}

		private bool IsTriggerGuide()
		{
			return GuideController.Instance.CurrentGuide != null && GuideController.Instance.CurrentGuide.Group == 4;
		}

		private IEnumerator CheckGuide()
		{
			yield return 1;
			if (GuideController.Instance.CurrentGuide != null)
			{
				RectTransform component = this.shopEquipPackGroup.transform.parent.GetComponent<RectTransform>();
				RectTransform component2 = component.parent.GetComponent<RectTransform>();
				if (component.rect.height > component2.rect.height)
				{
					float num = component.rect.height - component2.rect.height;
					component.anchoredPosition = new Vector2(component.anchoredPosition.x, num);
				}
			}
			yield break;
		}

		public RectTransform scrollContentRect;

		public RectTransform scrollRectTransform;

		public MainShopChapterPackGroup shopChapterPackGroup;

		public MainShopEquipSUpPoolActivityGroup shopEquipSUpPoolActivityGroup;

		public MainShopEquipActivityPackGroup shopEquipActivityPackGroup;

		public MainShopEquipPackGroup shopEquipPackGroup;

		private Vector2 originPos;
	}
}
