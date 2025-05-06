using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainHero : UIBaseMainPageNode
	{
		protected override void OnInit()
		{
			this.m_tabDatas.Clear();
			this.m_tabDatas.Add(new UIMainHero.TabData
			{
				m_index = 0,
				m_bt = this.m_heroBt,
				m_btGrays = this.m_heroBtGrays,
				m_panel = this.m_heroPanerl
			});
			this.m_heroPanerl.Init();
			this.m_canvasWidth = base.gameObject.GetComponent<RectTransform>().rect.width;
		}

		protected override void OnShow(UIBaseMainPageNode.OpenData openData)
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(false));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCorner, Singleton<EventArgsBool>.Instance.SetData(false));
			this.OnSelectIndex(0, false);
			this.m_heroBt.onClick.AddListener(new UnityAction(this.OnClickHeroBt));
			this.m_talentSystemBt.onClick.AddListener(new UnityAction(this.OnClickTalentSystemBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			Input.GetMouseButtonDown(0);
			if (this.m_heroPanerl != null)
			{
				this.m_heroPanerl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnHide()
		{
			this.m_heroBt.onClick.RemoveAllListeners();
			this.m_talentSystemBt.onClick.RemoveAllListeners();
			this.OnSelectIndex(-1, false);
		}

		protected override void OnDeInit()
		{
			this.m_tabDatas.Clear();
			this.m_heroPanerl.DeInit();
		}

		public override void OnLanguageChange()
		{
		}

		private void OnClickHeroBt()
		{
			this.OnSelectIndex(0, true);
		}

		private void OnClickTalentSystemBt()
		{
			this.OnSelectIndex(1, true);
		}

		private void OnSelectIndex(int index, bool isLerp = false)
		{
			if (this.m_selectIndex == index)
			{
				return;
			}
			this.m_seqPool.Clear(false);
			if (!isLerp)
			{
				if (this.m_selectIndex != -1)
				{
					UIMainHero.TabData tabData = this.m_tabDatas[this.m_selectIndex];
					tabData.m_bt.SetSelect(false);
					tabData.m_btGrays.SetUIGray();
					tabData.m_panel.OnHide();
					tabData.m_panel.SetActive(false);
				}
				if (index >= 0)
				{
					for (int i = 0; i < this.m_tabDatas.Count; i++)
					{
						UIMainHero.TabData tabData2 = this.m_tabDatas[i];
						if (tabData2 != null)
						{
							if (index != i)
							{
								tabData2.m_panel.SetActive(false);
								tabData2.m_btGrays.SetUIGray();
							}
							else
							{
								tabData2.m_bt.SetSelect(true);
								tabData2.m_btGrays.Recovery();
								tabData2.m_panel.rectTransform.anchoredPosition = new Vector2(0f, tabData2.m_panel.rectTransform.anchoredPosition.y);
								tabData2.m_panel.SetActive(true);
								tabData2.m_panel.OnShow();
								tabData2.m_panel.PlayAnimation();
								this.OnMoveFinished(index);
							}
						}
					}
				}
			}
			else
			{
				UIMainHero.TabData one = null;
				UIMainHero.TabData two = null;
				if (this.m_selectIndex != -1)
				{
					one = this.m_tabDatas[this.m_selectIndex];
					one.m_bt.SetSelect(false);
					one.m_btGrays.SetUIGray();
				}
				if (index >= 0)
				{
					for (int j = 0; j < this.m_tabDatas.Count; j++)
					{
						UIMainHero.TabData tabData3 = this.m_tabDatas[j];
						if (tabData3 != null)
						{
							if (index != j)
							{
								tabData3.m_btGrays.SetUIGray();
								tabData3.m_panel.SetActive(j == this.m_selectIndex);
							}
							else
							{
								two = tabData3;
								two.m_bt.SetSelect(true);
								two.m_btGrays.Recovery();
								two.m_panel.SetActive(true);
								two.m_panel.OnShow();
							}
						}
					}
				}
				if (one != null && two != null)
				{
					bool flag = one.m_index < two.m_index;
					one.m_panel.rectTransform.anchoredPosition = new Vector2(0f, one.m_panel.rectTransform.anchoredPosition.y);
					two.m_panel.rectTransform.anchoredPosition = new Vector2(flag ? this.m_canvasWidth : (-this.m_canvasWidth), two.m_panel.rectTransform.anchoredPosition.y);
					float num = (flag ? (-this.m_canvasWidth) : this.m_canvasWidth);
					float num2 = 0f;
					Sequence sequence = this.m_seqPool.Get();
					TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOAnchorPosX(one.m_panel.rectTransform, num, this.m_moveDuration, false), this.m_ease));
					TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOAnchorPosX(two.m_panel.rectTransform, num2, this.m_moveDuration, false), this.m_ease));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						one.m_panel.SetActive(false);
						one.m_panel.OnHide();
						two.m_panel.rectTransform.anchoredPosition = new Vector2(0f, two.m_panel.rectTransform.anchoredPosition.y);
						this.OnMoveFinished(index);
					});
				}
			}
			this.m_selectIndex = index;
		}

		private void OnMoveFinished(int index)
		{
		}

		[SerializeField]
		private CustomChooseButton m_heroBt;

		[SerializeField]
		private CustomChooseButton m_talentSystemBt;

		[SerializeField]
		private UIGrays m_heroBtGrays;

		[SerializeField]
		private UIGrays m_talentSystemBtGrays;

		[SerializeField]
		private UIMainHero_Hero m_heroPanerl;

		private int m_selectIndex = -1;

		private List<UIMainHero.TabData> m_tabDatas = new List<UIMainHero.TabData>();

		private float m_canvasWidth;

		private SequencePool m_seqPool = new SequencePool();

		private float m_moveDuration = 0.2f;

		private Ease m_ease = 1;

		public class TabData
		{
			public int m_index;

			public CustomChooseButton m_bt;

			public UIGrays m_btGrays;

			public BaseMainHeroPanel m_panel;
		}
	}
}
