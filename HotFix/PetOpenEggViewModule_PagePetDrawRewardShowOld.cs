using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.Platfrom;
using UnityEngine;

namespace HotFix
{
	public class PetOpenEggViewModule_PagePetDrawRewardShowOld : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.rewardObjectPool = LocalUnityObjctPool.Create(base.gameObject);
			this.rewardObjectPool.CreateCache("UIItemPool", this.rewardItemPrefab.gameObject);
			this.rewardItemPrefab.SetActive(false);
			this.btnTapClose.m_onClick = new Action(this.OnBtnCloseClick);
			this.btnDrawAgain.m_onClick = new Action(this.OnBtnAgainClick);
		}

		protected override void OnDeInit()
		{
			this.btnTapClose.m_onClick = null;
			this.btnDrawAgain.m_onClick = null;
			this.showItemSequencePool.Clear(false);
			this.RemoveAllItem();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void PlayRewardAnimation(int petBoxType, List<ItemData> rewards)
		{
			this.btnTapClose.gameObject.SetActive(false);
			this.btnDrawAgain.gameObject.SetActive(false);
			this.petBoxType = petBoxType;
			this.rewards = rewards;
			if (rewards == null)
			{
				this.OnBtnCloseClick();
				return;
			}
			this.SetInitView();
			this.PlayAnim();
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.PetOpenEggViewModule, null);
		}

		private void OnBtnAgainClick()
		{
			this.btnDrawAgain.gameObject.SetActive(false);
			NetworkUtils.Pet.PetDrawRequest(this.petBoxType, null);
		}

		private void SetInitView()
		{
			this.bgGameObject.SetActive(false);
			this.multiLightBg.alpha = 0f;
			this.multiFlashWhiteEffect.gameObject.SetActive(false);
		}

		public void PlayAnim()
		{
			EPetBoxType epetBoxType = (EPetBoxType)this.petBoxType;
			if (epetBoxType - EPetBoxType.AdDraw > 1)
			{
				if (epetBoxType != EPetBoxType.Draw35)
				{
				}
				this.ShowMulti();
				return;
			}
			this.ShowSingle();
		}

		private async void ShowSingle()
		{
			this.showItemSequencePool.Clear(false);
			this.bgGameObject.SetActive(true);
			await this.InstantiateItem(this.singleParent);
			BoxDrawRewardItem uiItem = this.curRewardItem[0];
			uiItem.transform.localScale = Vector3.one * 1.2f;
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.SetActive(true);
				boxDrawRewardItem.ShowQualityEffect();
			}
			TweenCallback <>9__2;
			Action <>9__1;
			TweenSettingsExtensions.AppendCallback(this.showItemSequencePool.Get(), delegate
			{
				BoxDrawRewardItem boxDrawRewardItem2 = this.curRewardItem[0];
				ItemData itemData = this.rewards[0];
				if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID)
					.quality >= 5)
				{
					PetOpenEggViewModule_PagePetDrawRewardShowOld <>4__this = this;
					List<BoxDrawRewardItem> list = this.curRewardItem;
					int num = 0;
					Action action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate
						{
							Sequence sequence = this.showItemSequencePool.Get();
							TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
							TweenCallback tweenCallback;
							if ((tweenCallback = <>9__2) == null)
							{
								tweenCallback = (<>9__2 = delegate
								{
									this.ShowButtons();
								});
							}
							TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
							TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(uiItem.transform, Vector3.zero, 0.6f, false), 12));
							TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(uiItem.transform, Vector3.one, 0.6f), 12));
							TweenSettingsExtensions.AppendCallback(sequence, null);
						});
					}
					<>4__this.ShowItem(list, num, action, true);
					return;
				}
				boxDrawRewardItem2.ShowItem(new Action(this.ShowItemOneByOne));
			});
		}

		private async void ShowMulti()
		{
			this.showItemSequencePool.Clear(false);
			this.bgGameObject.SetActive(true);
			this.multiLightBg.alpha = 1f;
			await this.InstantiateItem(this.multiItemParent);
			Sequence sequence = this.showItemSequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 0f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.multiLightBg.alpha = 0f;
				this.multiFlashWhiteEffect.gameObject.SetActive(true);
				this.multiFlashWhiteEffect.Stop();
				this.multiFlashWhiteEffect.Play();
			});
			await TaskExpand.Delay(150);
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.SetActive(true);
				boxDrawRewardItem.ShowQualityEffect();
			}
			bool flag = false;
			for (int i = 0; i < this.rewards.Count; i++)
			{
				ItemData itemData = this.rewards[i];
				if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID)
					.quality < 5)
				{
					flag = true;
				}
			}
			if (flag)
			{
				await TaskExpand.Delay(800);
				for (int j = 0; j < this.curRewardItem.Count; j++)
				{
					BoxDrawRewardItem boxDrawRewardItem2 = this.curRewardItem[j];
					ItemData itemData2 = this.rewards[j];
					if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData2.ID)
						.quality < 5)
					{
						boxDrawRewardItem2.ShowItem(new Action(this.ShowItemOneByOne));
					}
				}
			}
			else
			{
				this.ShowItemOneByOne();
			}
		}

		private void ShowItemOneByOne()
		{
			if (this.isShowItemOneByOne)
			{
				return;
			}
			this.isShowItemOneByOne = true;
			this.ShowItem(this.curRewardItem, 0, new Action(this.ShowButtons), true);
		}

		private void ShowItem(List<BoxDrawRewardItem> itemList, int itemIndex, Action onComplete, bool skipLowerQuality)
		{
			if (itemIndex >= itemList.Count)
			{
				Action onComplete2 = onComplete;
				if (onComplete2 == null)
				{
					return;
				}
				onComplete2();
				return;
			}
			else
			{
				BoxDrawRewardItem boxDrawRewardItem = itemList[itemIndex];
				int itemIndex2 = itemIndex;
				itemIndex = itemIndex2 + 1;
				if (!skipLowerQuality)
				{
					boxDrawRewardItem.ShowItem(delegate
					{
						this.ShowItem(itemList, itemIndex, onComplete, skipLowerQuality);
					});
					return;
				}
				ItemData itemData = boxDrawRewardItem.ItemData;
				if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID)
					.quality >= 5)
				{
					boxDrawRewardItem.ShowItem(delegate
					{
						this.ShowItem(itemList, itemIndex, onComplete, skipLowerQuality);
					});
					return;
				}
				this.ShowItem(itemList, itemIndex, onComplete, skipLowerQuality);
				return;
			}
		}

		private void ShowButtons()
		{
			this.btnTapClose.gameObject.SetActive(true);
			this.RefreshDrawAgainCost();
		}

		private void RefreshDrawAgainCost()
		{
			GameApp.Data.GetDataModule(DataName.PetDataModule);
			GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (this.petBoxType == 11)
			{
				this.btnDrawAgainText.text = Singleton<LanguageManager>.Instance.GetInfoByID("common_free");
				AdData adData = GameApp.Data.GetDataModule(DataName.AdDataModule).GetAdData(8);
				int remainTimes = adData.GetRemainTimes();
				this.txtAdCost.text = remainTimes.ToString() + "/" + adData.watchCountMax.ToString();
				this.drawAgainCost.gameObject.SetActive(false);
				this.txtAdCost.gameObject.SetActive(true);
				this.btnDrawAgain.gameObject.SetActive(remainTimes > 0);
				return;
			}
			this.drawAgainCost.gameObject.SetActive(true);
			this.txtAdCost.gameObject.SetActive(false);
			this.btnDrawAgain.gameObject.SetActive(true);
		}

		private async Task InstantiateItem(Transform parent)
		{
			this.RemoveAllItem();
			List<Task> list = new List<Task>();
			foreach (ItemData itemData in this.rewards)
			{
				BoxDrawRewardItem component = this.rewardObjectPool.DeQueue("UIItemPool").GetComponent<BoxDrawRewardItem>();
				this.curRewardItem.Add(component);
				Transform transform = component.transform;
				transform.SetParent(parent);
				transform.localScale = this.rewardItemPrefab.transform.localScale;
				transform.localPosition = Vector3.zero;
				transform.SetAsLastSibling();
				component.Init();
				list.Add(component.SetData(itemData, this.bgEffectPoint, this.fgEffectPoint));
			}
			await Task.WhenAll(list);
		}

		private void RemoveAllItem()
		{
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.DeInit();
				this.rewardObjectPool.EnQueue("UIItemPool", boxDrawRewardItem.gameObject);
			}
			this.curRewardItem.Clear();
		}

		private const string UIItemPoolKey = "UIItemPool";

		public CustomButton btnTapClose;

		public CustomButton btnDrawAgain;

		public CustomText btnDrawAgainText;

		public CommonCostItem drawAgainCost;

		public CustomText txtAdCost;

		[SerializeField]
		private GameObject bgGameObject;

		[SerializeField]
		private GameObject titleGameObject;

		[SerializeField]
		private BoxDrawRewardItem rewardItemPrefab;

		[SerializeField]
		private CustomGridLayoutGroup gridLayout;

		[SerializeField]
		private Transform bgEffectPoint;

		[SerializeField]
		private Transform fgEffectPoint;

		[SerializeField]
		private Transform singleParent;

		[SerializeField]
		private Transform multiItemParent;

		[SerializeField]
		private CanvasGroup multiLightBg;

		[SerializeField]
		private ParticleSystem multiFlashWhiteEffect;

		private LocalUnityObjctPool rewardObjectPool;

		private readonly List<BoxDrawRewardItem> curRewardItem = new List<BoxDrawRewardItem>();

		private readonly SequencePool showItemSequencePool = new SequencePool();

		private static readonly int AnimKeyPlay = Animator.StringToHash("Play");

		private int petBoxType;

		private List<ItemData> rewards;

		private bool isShowItemOneByOne;
	}
}
