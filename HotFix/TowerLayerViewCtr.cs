using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TowerLayerViewCtr : CustomBehaviour
	{
		public TowerChallenge_TowerLevel LevelConfig { get; private set; }

		protected override void OnInit()
		{
			for (int i = 0; i < this.monsterItemList.Count; i++)
			{
				this.monsterItemList[i].Init();
			}
			this.rewardItemPrefab.SetActive(false);
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.monsterItemList.Count; i++)
			{
				this.monsterItemList[i].DeInit();
			}
			this.showRewardList.ForEach(delegate(UIItem item)
			{
				item.DeInit();
			});
			this.showRewardList.Clear();
			for (int j = 0; j < this.hideRewardList.Count; j++)
			{
				this.hideRewardList.Dequeue().DeInit();
			}
		}

		public void RefreshData(int levelIndexVal, TowerChallenge_Tower towerConfigVal, bool isCurShow)
		{
			this.towerConfig = towerConfigVal;
			this.levelIndex = levelIndexVal;
			int num = towerConfigVal.level[this.levelIndex];
			this.LevelConfig = this.towerDataModule.GetLevelConfigByLevelId(num);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(towerConfigVal.name);
			this.textTowerName.text = infoByID;
			this.textTowerStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_stage", new object[] { this.levelIndex + 1 });
			if (!isCurShow)
			{
				this.contentRoot.SetActive(false);
				return;
			}
			this.contentRoot.SetActive(true);
			this.UpdateReward();
			this.UpdateLayer();
		}

		private void UpdateLayer()
		{
			List<TowerEnemyData> levelMemberData = this.towerDataModule.GetLevelMemberData(this.LevelConfig);
			for (int i = 0; i < this.monsterItemList.Count; i++)
			{
				if (i < levelMemberData.Count)
				{
					this.monsterItemList[i].gameObject.SetActiveSafe(true);
					this.monsterItemList[i].Refresh(levelMemberData[i]);
				}
				else
				{
					this.monsterItemList[i].gameObject.SetActiveSafe(false);
				}
			}
		}

		private void UpdateReward()
		{
			List<PropData> list = this.LevelConfig.reward.ToPropDataList();
			for (int i = 0; i < list.Count; i++)
			{
				if (i >= this.showRewardList.Count)
				{
					if (this.hideRewardList.Count > 0)
					{
						this.showRewardList.Add(this.hideRewardList.Dequeue());
					}
					else
					{
						UIItem component = Object.Instantiate<GameObject>(this.rewardItemPrefab, this.rewardPoint.transform).GetComponent<UIItem>();
						component.SetCountShowType(UIItem.CountShowType.ShowAll);
						component.Init();
						this.showRewardList.Add(component);
					}
				}
				UIItem uiitem = this.showRewardList[i];
				uiitem.SetData(list[i]);
				uiitem.SetActive(true);
				uiitem.OnRefresh();
			}
			for (int j = this.showRewardList.Count - 1; j > list.Count; j--)
			{
				UIItem uiitem2 = this.showRewardList[j];
				uiitem2.SetActive(false);
				this.showRewardList.RemoveAt(j);
				this.hideRewardList.Enqueue(uiitem2);
			}
			this.rewardObj.SetActive(this.showRewardList.Count > 0);
		}

		public void PlayShowAni(Action onFinish)
		{
			this.textTowerStage.transform.localScale = Vector3.zero;
			this.rewardTipTrans.transform.localScale = Vector3.zero;
			this.rewardLayerTrans.transform.localScale = new Vector3(0f, 1f, 1f);
			for (int i = 0; i < this.showRewardList.Count; i++)
			{
				this.showRewardList[i].transform.localScale = Vector3.zero;
			}
			for (int j = 0; j < this.monsterItemList.Count; j++)
			{
				TowerMonsterItem towerMonsterItem = this.monsterItemList[j];
				if (towerMonsterItem.gameObject.activeSelf)
				{
					towerMonsterItem.spineTrans.gameObject.SetActiveSafe(false);
					towerMonsterItem.spineTrans.anchoredPosition = new Vector2(towerMonsterItem.spineTrans.anchoredPosition.x, 200f);
					towerMonsterItem.levelTrans.localScale = Vector3.zero;
					towerMonsterItem.powerTrans.localScale = Vector3.zero;
				}
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, this.PlayRewardAni());
			float num = 0.01f;
			for (int k = 0; k < this.monsterItemList.Count; k++)
			{
				TowerMonsterItem towerMonsterItem2 = this.monsterItemList[k];
				if (towerMonsterItem2.gameObject.activeSelf)
				{
					TweenSettingsExtensions.Join(sequence, this.PlayMonsterAni(towerMonsterItem2));
				}
			}
			for (int l = 0; l < this.monsterItemList.Count; l++)
			{
				TowerMonsterItem towerMonsterItem3 = this.monsterItemList[l];
				if (towerMonsterItem3.gameObject.activeSelf)
				{
					TweenSettingsExtensions.AppendInterval(sequence, (float)l * num);
					TweenSettingsExtensions.Join(sequence, this.PlayScaleAni(towerMonsterItem3.powerTrans));
				}
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		private Sequence PlayRewardAni()
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textTowerStage.transform, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(this.textTowerStage.transform, Vector3.one, this.duration2));
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardTipTrans.transform, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(this.rewardTipTrans.transform, Vector3.one * this.scale, this.duration2));
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScaleX(this.rewardLayerTrans.transform, this.scale, this.duration1)), ShortcutExtensions.DOScaleX(this.rewardLayerTrans.transform, 1f, this.duration2));
			for (int i = 0; i < this.showRewardList.Count; i++)
			{
				TweenSettingsExtensions.Join(sequence, this.PlayScaleAni(this.showRewardList[i].transform));
			}
			return sequence;
		}

		private Sequence PlayMonsterAni(TowerMonsterItem item)
		{
			Sequence sequence = DOTween.Sequence();
			item.spineTrans.gameObject.SetActiveSafe(true);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(item.spineTrans, 0f, 0.1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				item.PlayEffect();
			});
			return sequence;
		}

		private Sequence PlayScaleAni(Transform trans)
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(trans, Vector3.one * this.scale, this.duration1)), ShortcutExtensions.DOScale(trans, Vector3.one, this.duration2));
			return sequence;
		}

		[SerializeField]
		private GameObject contentRoot;

		[SerializeField]
		private GameObject rewardItemPrefab;

		[SerializeField]
		private GameObject rewardObj;

		[SerializeField]
		private GameObject rewardPoint;

		[SerializeField]
		private CustomText textTowerName;

		[SerializeField]
		private CustomText textTowerStage;

		[SerializeField]
		private List<TowerMonsterItem> monsterItemList;

		[SerializeField]
		private RectTransform rewardTipTrans;

		[SerializeField]
		private RectTransform rewardLayerTrans;

		private int levelIndex;

		private TowerChallenge_Tower towerConfig;

		private TowerDataModule towerDataModule;

		private readonly List<UIItem> showRewardList = new List<UIItem>();

		private readonly Queue<UIItem> hideRewardList = new Queue<UIItem>();

		private float scale = 1.1f;

		private float duration1 = 0.15f;

		private float duration2 = 0.05f;
	}
}
