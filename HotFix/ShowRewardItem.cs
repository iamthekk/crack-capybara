using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using Proto.Common;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ShowRewardItem
	{
		public void SetShowItemList(string[] reward, bool isClear = true)
		{
			if (isClear)
			{
				this.deinitItems();
			}
			this.data.list = new List<ItemData>();
			if (reward == null)
			{
				reward = new string[0];
			}
			for (int i = 0; i < reward.Length; i++)
			{
				List<int> listInt = reward[i].GetListInt(',');
				this.data.list.Add(new ItemData(listInt[0], (long)listInt[1]));
			}
			this.m_scrollImage = this.m_scrollRect.GetComponent<Image>();
			this.m_item.SetActive(false);
			List<ItemData> combineList = this.data.GetCombineList();
			int count = combineList.Count;
			for (int j = 0; j < count; j++)
			{
				UIItem component = this.m_pool.DeQueue(this.PoolCacheName).GetComponent<UIItem>();
				component.SetCountShowType(UIItem.CountShowType.MissOne);
				component.onClick = new Action<UIItem, PropData, object>(DxxTools.UI.OnItemClick);
				component.Init();
				RectTransform rectTransform = component.rectTransform;
				rectTransform.SetParentNormal(this.m_scrollRect.content, false);
				if (this.m_item != null && rectTransform != null)
				{
					rectTransform.localScale = this.m_item.transform.localScale;
				}
				component.SetData(combineList[j].ToPropData());
				component.OnRefresh();
				this.m_list.Add(component);
			}
		}

		public void SetShowItemList(List<ItemData> reward)
		{
			this.deinitItems();
			this.data.list = reward;
			this.m_scrollImage = this.m_scrollRect.GetComponent<Image>();
			this.m_item.SetActive(false);
			List<ItemData> combineList = this.data.GetCombineList();
			int count = combineList.Count;
			for (int i = 0; i < count; i++)
			{
				UIItem component = this.m_pool.DeQueue(this.PoolCacheName).GetComponent<UIItem>();
				component.SetCountShowType(UIItem.CountShowType.MissOne);
				component.onClick = new Action<UIItem, PropData, object>(DxxTools.UI.OnItemClick);
				component.Init();
				RectTransform rectTransform = component.rectTransform;
				rectTransform.SetParentNormal(this.m_scrollRect.content, false);
				if (this.m_item != null && rectTransform != null)
				{
					rectTransform.localScale = this.m_item.transform.localScale;
				}
				component.SetData(combineList[i].ToPropData());
				component.OnRefresh();
				this.m_list.Add(component);
			}
		}

		public UIItem AddShowItem(ItemData itemdata, SequencePool seqPool = null)
		{
			if (this.m_scrollImage == null)
			{
				this.m_scrollImage = this.m_scrollRect.GetComponent<Image>();
			}
			UIItem component = this.m_pool.DeQueue(this.PoolCacheName).GetComponent<UIItem>();
			component.SetCountShowType(UIItem.CountShowType.MissOne);
			component.onClick = new Action<UIItem, PropData, object>(DxxTools.UI.OnItemClick);
			component.Init();
			RectTransform rectTransform = component.rectTransform;
			rectTransform.SetParentNormal(this.m_scrollRect.content, false);
			if (this.m_item != null && rectTransform != null)
			{
				rectTransform.localScale = this.m_item.transform.localScale;
			}
			component.SetData(itemdata.ToPropData());
			component.OnRefresh();
			this.m_list.Add(component);
			if (seqPool != null)
			{
				Sequence sequence = seqPool.Get();
				rectTransform.localScale = Vector3.zero;
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1.1f, 0.15f));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1f, 0.05f));
			}
			return component;
		}

		public void SetScrollEnable(bool value)
		{
			this.m_scrollRect.enabled = value;
			if (this.m_scrollImage != null)
			{
				this.m_scrollImage.raycastTarget = value;
			}
		}

		public void SetShowItemList(RepeatedField<RewardDto> Reward, Action<UIItem, PropData, object> onClickReward, SequencePool seqPool = null, float delayTime = 0.05f)
		{
			this.OnClickReward = onClickReward;
			this.deinitItems();
			this.data.list = new List<ItemData>();
			for (int i = 0; i < Reward.Count; i++)
			{
				RewardDto rewardDto = Reward[i];
				this.data.list.Add(new ItemData((int)rewardDto.ConfigId, (long)((int)rewardDto.Count)));
			}
			this.m_item.SetActive(false);
			List<ItemData> list = this.data.GetCombineList();
			int count = list.Count;
			if (seqPool == null)
			{
				for (int j = 0; j < count; j++)
				{
					this.initItemOne(list[j]);
				}
				return;
			}
			Sequence sequence = seqPool.Get();
			TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
			for (int k = 0; k < count; k++)
			{
				int index = k;
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					CanvasGroup orAddComponent = GameObjectExpand.GetOrAddComponent<CanvasGroup>(this.initItemOne(list[index]).gameObject);
					orAddComponent.alpha = 0f;
					TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.Append(seqPool.Get(), ShortcutExtensions46.DOFade(orAddComponent, 1f, 0.2f)), true);
					if (this.m_showSound)
					{
						GameApp.Sound.PlayClip(52, 1f);
					}
				});
				TweenSettingsExtensions.AppendInterval(sequence, delayTime);
			}
		}

		private UIItem initItemOne(ItemData itemData)
		{
			UIItem component = this.m_pool.DeQueue(this.PoolCacheName).GetComponent<UIItem>();
			component.SetCountShowType(UIItem.CountShowType.MissOne);
			component.Init();
			component.onClick = this.OnClickReward;
			RectTransform rectTransform = component.rectTransform;
			rectTransform.SetParentNormal(this.m_scrollRect.content, false);
			if (this.m_item != null && rectTransform != null)
			{
				rectTransform.localScale = this.m_item.transform.localScale;
			}
			component.SetData(itemData.ToPropData());
			component.OnRefresh();
			this.m_list.Add(component);
			return component;
		}

		public void deinitItems()
		{
			for (int i = 0; i < this.m_list.Count; i++)
			{
				this.m_list[i].DeInit();
			}
			this.m_list.Clear();
			this.m_pool.Collect(this.PoolCacheName);
		}

		public void Init()
		{
			if (!this.m_pool.IsHavePool(this.PoolCacheName))
			{
				this.m_pool.CreateCache(this.PoolCacheName, this.m_item);
			}
		}

		public GameObject m_item;

		public LocalUnityObjctPool m_pool;

		public GridLayoutGroup m_grid;

		public string PoolCacheName = "reward";

		public CustomScrollRect m_scrollRect;

		public Image m_scrollImage;

		public List<UIItem> m_list = new List<UIItem>();

		public RewardCommonData data = new RewardCommonData();

		public bool m_showSound = true;

		private Action<UIItem, PropData, object> OnClickReward;
	}
}
