using System;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class PetQuickStarUpgradeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
			this.SetImageWhiteAlpha(0f);
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
		}

		public void SetData(ItemData itemData, int oldStar, int newStar)
		{
			this.uiItem.SetData(itemData.ToPropData());
			this.uiItem.OnRefresh();
			this.ShowEffect(itemData);
			this.petStarUpTo.SetData(oldStar, newStar);
		}

		private async void ShowEffect(ItemData itemData)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("itemId:{0} not found in item config", itemData.ID));
			}
			else
			{
				Quality_itemQuality elementById2 = GameApp.Table.GetManager().GetQuality_itemQualityModelInstance().GetElementById(elementById.quality);
				if (elementById2 == null)
				{
					HLog.LogError(string.Format("quality:{0} not found in item quality config", elementById.quality));
				}
				else if (elementById.showEffect > 0 && !string.IsNullOrEmpty(elementById2.drawCardBgEffect))
				{
					AsyncOperationHandle<GameObject> handler = GameApp.Resources.LoadAssetAsync<GameObject>(elementById2.drawCardBgEffect);
					await handler.Task;
					GameObject gameObject = Object.Instantiate<GameObject>(handler.Result, this.backEffect.transform, false);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					this.effectList.Add(gameObject);
					Transform transform = gameObject.transform.Find("Star");
					ParticleSystem particleSystem;
					if (transform != null && transform.TryGetComponent<ParticleSystem>(ref particleSystem))
					{
						ParticleSystem particleSystem2 = particleSystem;
						particleSystem2.transform.SetParent(this.frontEffect.transform, false);
						this.effectList.Add(particleSystem2.gameObject);
					}
					this.backEffect.RefreshParticles();
					this.frontEffect.RefreshParticles();
					handler = default(AsyncOperationHandle<GameObject>);
				}
				else
				{
					for (int i = 0; i < this.effectList.Count; i++)
					{
						if (!(this.effectList[i] == null))
						{
							Object.Destroy(this.effectList[i]);
						}
					}
					this.backEffect.RefreshParticles();
					this.frontEffect.RefreshParticles();
				}
			}
		}

		public void SetImageWhiteAlpha(float alpha)
		{
			this.imgWhite.gameObject.SetActive(alpha > 0f);
			Color color = this.imgWhite.color;
			color.a = alpha;
			this.imgWhite.color = color;
		}

		public UIItem uiItem;

		public UIParticle backEffect;

		public UIParticle frontEffect;

		public CustomImage imgWhite;

		public PetStarUpTo petStarUpTo;

		private List<GameObject> effectList = new List<GameObject>();
	}
}
