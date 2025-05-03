using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class BattleResultWorldBossViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.itemDefault.SetActiveSafe(false);
			this.btn_Close.m_onClick = new Action(this.OnClickClose);
			this.btn_Mask.m_onClick = new Action(this.OnClickClose);
		}

		public override void OnDelete()
		{
			this.btn_Close.m_onClick = null;
			this.btn_Mask.m_onClick = null;
			for (int i = 0; i < this.itemCacheList.Count; i++)
			{
				this.itemCacheList[i].DeInit();
			}
			this.itemCacheList.Clear();
		}

		public override void OnOpen(object data)
		{
			BattleResultWorldBossViewModule.OpenData openData = data as BattleResultWorldBossViewModule.OpenData;
			if (openData != null)
			{
				this.m_OpenData = openData;
				this.Refresh();
				this.ResetAni();
				base.StartCoroutine(this.PlayResultAni());
				if (this.m_OpenData.IsSuccess)
				{
					GameApp.Sound.PlayClip(88, 1f);
					return;
				}
				GameApp.Sound.PlayClip(89, 1f);
			}
		}

		private IEnumerator PlayResultAni()
		{
			if (this.m_OpenData.IsSuccess)
			{
				float duration = this.spineWin.SkeletonData.FindAnimation("ShengLi_1").Duration;
				this.spineWin.AnimationState.SetAnimation(0, "ShengLi_1", false);
				yield return new WaitForSeconds(duration);
				this.winEffect.SetActiveSafe(true);
				this.spineWin.AnimationState.SetAnimation(0, "ShengLi_2", true);
				this.PlayWinAni();
			}
			else
			{
				float duration2 = this.spineLose.SkeletonData.FindAnimation("ShiBai_1").Duration;
				this.spineLose.AnimationState.SetAnimation(0, "ShiBai_1", false);
				yield return new WaitForSeconds(duration2);
				this.spineLose.AnimationState.SetAnimation(0, "ShiBai_2", true);
				this.PlayFailAni();
			}
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			foreach (UIItem uiitem in this.itemCacheList)
			{
				uiitem.gameObject.SetActive(false);
			}
			this.itemList.Clear();
			this.m_seqPool.Clear(false);
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					GameApp.View.CloseView(ViewName.GameEventViewModule, null);
					GameApp.View.CloseView(ViewName.GameEventFinishViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void Refresh()
		{
			if (this.m_OpenData == null)
			{
				return;
			}
			this.victoryObj.SetActiveSafe(this.m_OpenData.IsSuccess);
			this.failObj.SetActiveSafe(!this.m_OpenData.IsSuccess);
			this.textDamage.text = DxxTools.FormatNumber(this.m_OpenData.CurrentDamage);
			this.textBestDamage.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Damage_Best", new object[] { DxxTools.FormatNumber(this.m_OpenData.BestDamage) });
			this.textAllDamage.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Damage_Total", new object[] { DxxTools.FormatNumber(this.m_OpenData.TotalDamage) });
			if (this.victoryObj.activeSelf)
			{
				this.stageOutline.effectColor = this.outlineWin;
			}
			else
			{
				this.stageOutline.effectColor = this.outlineFail;
			}
			this.RefreshRewards();
		}

		private void RefreshRewards()
		{
			if (this.m_OpenData.Rewards != null)
			{
				this.itemList.Clear();
				for (int i = 0; i < this.m_OpenData.Rewards.Length; i++)
				{
					UIItem uiitem;
					if (i < this.itemCacheList.Count)
					{
						uiitem = this.itemCacheList[i];
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.itemDefault);
						gameObject.transform.SetParentNormal(this.gridObj.transform, false);
						uiitem = gameObject.GetComponent<UIItem>();
						this.itemCacheList.Add(uiitem);
					}
					if (!uiitem.gameObject.activeSelf)
					{
						uiitem.gameObject.SetActiveSafe(true);
					}
					uiitem.Init();
					this.itemList.Add(uiitem);
					uiitem.SetData(this.m_OpenData.Rewards[i]);
					uiitem.OnRefresh();
				}
				this.textNoReward.text = ((this.m_OpenData.Rewards.Length != 0) ? "" : Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_NoReward"));
				return;
			}
			this.textNoReward.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEventFinish_NoReward");
		}

		private void ResetAni()
		{
			this.btn_Close.transform.localScale = Vector3.zero;
			this.winEffect.SetActiveSafe(false);
			this.winLightObj.SetActiveSafe(false);
			this.winTextObj.SetActiveSafe(false);
			this.winSurviveTextObj.SetActiveSafe(false);
			this.winRewardBgTrans.sizeDelta = new Vector2(this.winRewardBgTrans.sizeDelta.x, 0f);
			this.failLightObj.SetActiveSafe(false);
			this.failTextObj.SetActiveSafe(false);
			this.failSurviveTextObj.SetActiveSafe(false);
			this.failRewardBgTrans.sizeDelta = new Vector2(this.failRewardBgTrans.sizeDelta.x, 0f);
			this.textBestDamage.gameObject.SetActiveSafe(false);
			this.textAllDamage.gameObject.SetActiveSafe(false);
			this.textDamage.gameObject.SetActiveSafe(false);
			this.newBest.SetActiveSafe(false);
			this.textNoReward.gameObject.SetActiveSafe(false);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].transform.localScale = Vector3.zero;
			}
		}

		private void PlayWinAni()
		{
			Sequence sequence = this.m_seqPool.Get();
			float num = 0.15f;
			this.winTextObj.SetActiveSafe(true);
			this.winTextObj.transform.localScale = Vector3.one * 3f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.winTextObj.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textAllDamage.gameObject.SetActiveSafe(true);
				this.textAllDamage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textAllDamage.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.winSurviveTextObj.SetActiveSafe(true);
				this.winSurviveTextObj.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.winSurviveTextObj.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textDamage.gameObject.SetActiveSafe(true);
				this.textDamage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textDamage.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textBestDamage.gameObject.SetActiveSafe(true);
				this.textBestDamage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textBestDamage.transform, Vector3.one, num));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOSizeDelta(this.winRewardBgTrans, new Vector2(this.winRewardBgTrans.sizeDelta.x, 710f), num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.winLightObj.SetActiveSafe(true);
				this.ShowRewardAni();
			});
		}

		private void PlayFailAni()
		{
			Sequence sequence = this.m_seqPool.Get();
			float num = 0.15f;
			this.failTextObj.SetActiveSafe(true);
			this.failTextObj.transform.localScale = Vector3.one * 3f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.failTextObj.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textAllDamage.gameObject.SetActiveSafe(true);
				this.textAllDamage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textAllDamage.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.failSurviveTextObj.SetActiveSafe(true);
				this.failSurviveTextObj.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.failSurviveTextObj.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textDamage.gameObject.SetActiveSafe(true);
				this.textDamage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textDamage.transform, Vector3.one, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.textBestDamage.gameObject.SetActiveSafe(true);
				this.textBestDamage.transform.localScale = Vector3.one * 2f;
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.textBestDamage.transform, Vector3.one, num));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOSizeDelta(this.failRewardBgTrans, new Vector2(this.failRewardBgTrans.sizeDelta.x, 710f), num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.failLightObj.SetActiveSafe(true);
				this.ShowRewardAni();
			});
		}

		private void ShowRewardAni()
		{
			Sequence sequence = this.m_seqPool.Get();
			if (this.itemList.Count > 0)
			{
				for (int i = 0; i < this.itemList.Count; i++)
				{
					Transform transform = this.itemList[i].transform;
					transform.localScale = Vector3.zero;
					TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(transform, Vector3.one * 1.1f, 0.15f)), ShortcutExtensions.DOScale(transform, Vector3.one, 0.05f));
				}
			}
			else
			{
				this.textNoReward.gameObject.SetActiveSafe(true);
				RectTransform component = this.textNoReward.GetComponent<RectTransform>();
				Color color = this.textNoReward.color;
				color.a = 0f;
				this.textNoReward.color = color;
				component.anchoredPosition = new Vector2(component.anchoredPosition.x, -300f);
				float num = 0.15f;
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textNoReward, 1f, num));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(component, -235f, num, false));
			}
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.btn_Close.transform, Vector3.one * 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.btn_Close.transform, Vector3.one, 0.05f));
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.BattleResultViewWorldBossModule, null);
		}

		[Header("Base")]
		public CustomButton btn_Close;

		public CustomButton btn_Mask;

		[SerializeField]
		protected CustomText textBestDamage;

		[SerializeField]
		protected CustomText textAllDamage;

		[SerializeField]
		protected CustomText textDamage;

		[SerializeField]
		protected GameObject newBest;

		public GameObject winEffect;

		public SkeletonGraphic spineWin;

		public SkeletonGraphic spineLose;

		public GameObject gridObj;

		public GameObject victoryObj;

		public GameObject failObj;

		public CustomOutLine stageOutline;

		public GameObject itemDefault;

		public CustomText textNoReward;

		public GameObject winLightObj;

		public GameObject winTextObj;

		public GameObject winSurviveTextObj;

		public RectTransform winRewardBgTrans;

		public GameObject failLightObj;

		public GameObject failTextObj;

		public GameObject failSurviveTextObj;

		public RectTransform failRewardBgTrans;

		public Color outlineWin;

		public Color outlineFail;

		private BattleResultWorldBossViewModule.OpenData m_OpenData;

		private List<UIItem> itemList = new List<UIItem>();

		private List<UIItem> itemCacheList = new List<UIItem>();

		private SequencePool m_seqPool = new SequencePool();

		public class OpenData
		{
			public bool IsSuccess;

			public long BestDamage;

			public long CurrentDamage;

			public long TotalDamage;

			public PropData[] Rewards;
		}
	}
}
