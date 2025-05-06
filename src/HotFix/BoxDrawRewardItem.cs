using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coffee.UIExtensions;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class BoxDrawRewardItem : CustomBehaviour
	{
		public ItemData ItemData { get; private set; }

		protected override void OnInit()
		{
			this.loadGameObjects = new LoadPool<GameObject>();
			this.uiItem.SetActive(false);
			this.uiItem.SetCountShowType(UIItem.CountShowType.MissOne);
			this.uiItem.Init();
			this.uiItem.onClick = new Action<UIItem, PropData, object>(this.OnBtnItemClick);
		}

		private void OnBtnItemClick(UIItem item, PropData data, object openData)
		{
			float num = base.rectTransform.rect.height * base.rectTransform.localScale.y * 0.5f + 10f;
			DxxTools.UI.OnItemClick(item, data, openData, item.transform.position, num);
		}

		protected override void OnDeInit()
		{
			this.loadGameObjects.UnLoadAll();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = this.curShowEffectList.Count - 1; i >= 0; i--)
			{
				ParticleSystem particleSystem = this.curShowEffectList[i];
				if (particleSystem == null)
				{
					this.curShowEffectList.RemoveAt(i);
				}
				else
				{
					this.EffectFollow(particleSystem);
				}
			}
		}

		public async Task SetData(ItemData itemDataVal, Transform bgEffectPointVal, Transform fgEffectPointVal)
		{
			this.ItemData = itemDataVal;
			this.itemTable = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.ItemData.ID);
			if (this.itemTable.itemType == 19 || this.itemTable.itemType == 20)
			{
				this.petQualityTable = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(this.itemTable.quality);
				this.drawCardBgEffectPath = this.petQualityTable.drawCardBgEffect;
				this.drawCardFireEffectPath = this.petQualityTable.drawCardFireEffect;
				this.drawCardFireBurstEffect2Path = this.petQualityTable.drawCardFireBurstEffect2;
				this.drawCardBurstLightEffectPath = this.petQualityTable.drawCardBurstLightEffect;
				this.drawCardFireEffect2Path = this.petQualityTable.drawCardFireEffect2;
			}
			else if (this.itemTable.itemType == 1)
			{
				this.equipQualityTable = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(this.itemTable.quality);
				this.drawCardBgEffectPath = this.equipQualityTable.drawCardBgEffect;
				this.drawCardFireEffectPath = this.equipQualityTable.drawCardFireEffect;
				this.drawCardFireBurstEffect2Path = this.equipQualityTable.drawCardFireBurstEffect2;
				this.drawCardBurstLightEffectPath = this.equipQualityTable.drawCardBurstLightEffect;
				this.drawCardFireEffect2Path = this.equipQualityTable.drawCardFireEffect2;
			}
			else
			{
				this.itemQualityTable = GameApp.Table.GetManager().GetQuality_itemQualityModelInstance().GetElementById(this.itemTable.quality);
				this.drawCardBgEffectPath = this.itemQualityTable.drawCardBgEffect;
				this.drawCardFireEffectPath = this.itemQualityTable.drawCardFireEffect;
				this.drawCardFireBurstEffect2Path = this.itemQualityTable.drawCardFireBurstEffect2;
				this.drawCardBurstLightEffectPath = this.itemQualityTable.drawCardBurstLightEffect;
				this.drawCardFireEffect2Path = this.itemQualityTable.drawCardFireEffect2;
			}
			this.bgEffectPoint = bgEffectPointVal;
			this.fgEffectPoint = fgEffectPointVal;
			await this.LoadEffect(null);
		}

		public void ShowQualityEffect()
		{
			this.SetDrawCardFireEffect(true);
		}

		public void ShowItem(Action showEnd)
		{
			this.showItemSequencePool.Clear(false);
			this.uiItem.SetActive(true);
			this.uiItem.SetData(this.ItemData.ToPropData());
			this.uiItem.OnRefresh();
			this.uiItem.transform.localScale = Vector3.zero;
			this.SetDrawCardFireEffect(false);
			Sequence sequence = this.showItemSequencePool.Get();
			if (this.SetDrawCardFireBurstEffect2(true))
			{
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.SetDrawCardFireBurstEffect2(false);
				});
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.SetDrawCardBurstLightEffect(true);
			});
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.uiItem.transform, Vector3.one, 0.4f), 27));
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.SetDrawCardBgEffect(true);
				Action showEnd2 = showEnd;
				if (showEnd2 != null)
				{
					showEnd2();
				}
				this.SetDrawCardBurstLightEffect(false);
			});
		}

		private bool SetDrawCardBgEffect(bool isShow)
		{
			this.SetEffect(this.drawCardBgEffectStar, isShow);
			return this.SetEffect(this.drawCardBgEffect, isShow);
		}

		private bool SetDrawCardFireEffect(bool isShow)
		{
			return this.SetEffect(this.drawCardFireEffect, isShow);
		}

		private bool SetDrawCardFireBurstEffect2(bool isShow)
		{
			return this.SetEffect(this.drawCardFireBurstEffect2, isShow);
		}

		private bool SetDrawCardBurstLightEffect(bool isShow)
		{
			return this.SetEffect(this.drawCardBurstLightEffect, isShow);
		}

		private bool SetEffect(ParticleSystem effect, bool isShow)
		{
			if (!effect)
			{
				return false;
			}
			effect.gameObject.SetActive(isShow);
			if (isShow)
			{
				this.EffectFollow(effect);
				effect.Stop();
				effect.Play();
				this.curShowEffectList.Add(effect);
			}
			else
			{
				effect.Stop();
				effect.Clear();
				this.curShowEffectList.Remove(effect);
			}
			return true;
		}

		private void EffectFollow(ParticleSystem effect)
		{
			Transform transform = effect.transform;
			Transform transform2 = base.transform;
			transform.position = transform2.position;
			transform.localScale = transform2.localScale;
		}

		private void TryLoadEffect(List<Task> allTask, string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			allTask.Add(this.loadGameObjects.Load(path));
		}

		private async Task LoadEffect(Action onCompletion)
		{
			List<Task> list = new List<Task>();
			this.TryLoadEffect(list, this.drawCardBgEffectPath);
			this.TryLoadEffect(list, this.drawCardFireEffectPath);
			this.TryLoadEffect(list, this.drawCardFireBurstEffect2Path);
			this.TryLoadEffect(list, this.drawCardBurstLightEffectPath);
			await Task.WhenAll(list);
			if (this.drawCardBgEffect != null)
			{
				Object.Destroy(this.drawCardBgEffect.gameObject);
			}
			if (this.drawCardBgEffectStar != null)
			{
				Object.Destroy(this.drawCardBgEffectStar.gameObject);
			}
			GameObject gameObject = this.loadGameObjects.Get(this.drawCardBgEffectPath);
			if (gameObject)
			{
				this.drawCardBgEffect = Object.Instantiate<ParticleSystem>(gameObject.GetComponent<ParticleSystem>(), this.bgEffectPoint);
				this.drawCardBgEffect.Stop();
				this.drawCardBgEffect.Clear();
				this.drawCardBgEffect.gameObject.SetActive(false);
				Transform transform = this.drawCardBgEffect.transform.Find("Star");
				ParticleSystem particleSystem;
				if (transform != null && transform.TryGetComponent<ParticleSystem>(ref particleSystem))
				{
					this.drawCardBgEffectStar = particleSystem;
					this.drawCardBgEffectStar.transform.SetParent(this.fgEffectPoint);
					this.drawCardBgEffectStar.gameObject.SetActive(false);
				}
			}
			if (this.drawCardFireEffect != null)
			{
				Object.Destroy(this.drawCardFireEffect.gameObject);
			}
			GameObject gameObject2 = this.loadGameObjects.Get(this.drawCardFireEffectPath);
			if (gameObject2)
			{
				this.drawCardFireEffect = Object.Instantiate<ParticleSystem>(gameObject2.GetComponent<ParticleSystem>(), this.bgEffectPoint);
				this.drawCardFireEffect.Stop();
				this.drawCardFireEffect.Clear();
				this.drawCardFireEffect.gameObject.SetActive(false);
			}
			if (this.drawCardFireEffect2 != null)
			{
				Object.Destroy(this.drawCardFireEffect2.gameObject);
			}
			GameObject gameObject3 = this.loadGameObjects.Get(this.drawCardFireEffect2Path);
			if (gameObject3)
			{
				this.drawCardFireEffect2 = Object.Instantiate<ParticleSystem>(gameObject3.GetComponent<ParticleSystem>(), this.bgEffectPoint);
				this.drawCardFireEffect2.Stop();
				this.drawCardFireEffect2.Clear();
				this.drawCardFireEffect2.gameObject.SetActive(false);
			}
			if (this.drawCardFireBurstEffect2 != null)
			{
				Object.Destroy(this.drawCardFireBurstEffect2.gameObject);
			}
			GameObject gameObject4 = this.loadGameObjects.Get(this.drawCardFireBurstEffect2Path);
			if (gameObject4)
			{
				this.drawCardFireBurstEffect2 = Object.Instantiate<ParticleSystem>(gameObject4.GetComponent<ParticleSystem>(), this.bgEffectPoint);
				this.drawCardFireBurstEffect2.Stop();
				this.drawCardFireBurstEffect2.Clear();
				this.drawCardFireBurstEffect2.gameObject.SetActive(false);
			}
			if (this.drawCardBurstLightEffect != null)
			{
				Object.Destroy(this.drawCardBurstLightEffect.gameObject);
			}
			GameObject gameObject5 = this.loadGameObjects.Get(this.drawCardBurstLightEffectPath);
			if (gameObject5)
			{
				this.drawCardBurstLightEffect = Object.Instantiate<ParticleSystem>(gameObject5.GetComponent<ParticleSystem>(), this.fgEffectPoint);
				this.drawCardBurstLightEffect.Stop();
				this.drawCardBurstLightEffect.Clear();
				this.drawCardBurstLightEffect.gameObject.SetActive(false);
			}
			UIParticle uiparticle;
			if (this.fgEffectPoint.TryGetComponent<UIParticle>(ref uiparticle))
			{
				uiparticle.RefreshParticles();
			}
			UIParticle uiparticle2;
			if (this.bgEffectPoint.TryGetComponent<UIParticle>(ref uiparticle2))
			{
				uiparticle2.RefreshParticles();
			}
			if (onCompletion != null)
			{
				onCompletion();
			}
		}

		[SerializeField]
		private UIItem uiItem;

		private readonly SequencePool showItemSequencePool = new SequencePool();

		private Item_Item itemTable;

		private Quality_petQuality petQualityTable;

		private Quality_equipQuality equipQualityTable;

		private Quality_itemQuality itemQualityTable;

		private LoadPool<GameObject> loadGameObjects;

		private Transform bgEffectPoint;

		private Transform fgEffectPoint;

		private ParticleSystem drawCardBgEffect;

		private ParticleSystem drawCardBgEffectStar;

		private ParticleSystem drawCardFireEffect;

		private ParticleSystem drawCardFireEffect2;

		private ParticleSystem drawCardFireBurstEffect2;

		private ParticleSystem drawCardBurstLightEffect;

		private string drawCardBgEffectPath;

		private string drawCardFireEffectPath;

		private string drawCardFireBurstEffect2Path;

		private string drawCardBurstLightEffectPath;

		private string drawCardFireEffect2Path;

		private readonly List<ParticleSystem> curShowEffectList = new List<ParticleSystem>();
	}
}
