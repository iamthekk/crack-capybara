using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIEventProgressNodeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iconTrans = this.imageIcon.GetComponent<RectTransform>();
			this.textTrans = this.textStage.GetComponent<RectTransform>();
			this.arrowObj.SetActiveSafe(false);
			this.sliderArrow.SetActiveSafe(false);
			this.initPosX = base.transform.position.x;
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.totalStage = dataModule.CurrentChapter.TotalStage;
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(GameEventProgressData data, int index)
		{
			this.progressData = data;
			this.mIndex = index;
			this.slider.gameObject.SetActiveSafe(index < 2);
			this.slider.value = 0f;
			if (data.stage == 1)
			{
				this.imageIcon.sprite = this.register.GetSprite(this.nameDic[GameEventType.None]);
			}
			else
			{
				this.imageIcon.sprite = this.register.GetSprite(this.nameDic[data.type]);
			}
			this.textStage.text = data.stage.ToString();
		}

		public void SetDay(int currentStage, int nextStage, bool isAni)
		{
			if (this.progressData == null)
			{
				return;
			}
			if (currentStage == this.progressData.stage)
			{
				if (this.mIndex == 3 && currentStage != this.totalStage)
				{
					this.arrowObj.SetActiveSafe(false);
					this.sliderArrow.SetActiveSafe(false);
					return;
				}
				this.arrowObj.SetActiveSafe(true);
				this.sliderArrow.SetActiveSafe(false);
				if (isAni)
				{
					this.PlayBig();
					return;
				}
				this.iconTrans.localScale = Vector3.one * 1f;
				this.textTrans.anchoredPosition = new Vector2(this.textTrans.anchoredPosition.x, -36f);
				return;
			}
			else if (currentStage > this.progressData.stage && currentStage < nextStage)
			{
				this.arrowObj.SetActiveSafe(false);
				this.sliderArrow.SetActiveSafe(true);
				float num = (float)(nextStage - this.progressData.stage);
				float num2 = (float)(currentStage - this.progressData.stage) / num;
				if (isAni)
				{
					this.PlaySliderAnimation(this.slider.value, num2);
					this.PlayNormal();
					return;
				}
				this.slider.value = num2;
				this.iconTrans.localScale = Vector3.one * 0.7f;
				this.textTrans.anchoredPosition = new Vector2(this.textTrans.anchoredPosition.x, -26f);
				return;
			}
			else
			{
				this.arrowObj.SetActiveSafe(false);
				this.sliderArrow.SetActiveSafe(false);
				if (currentStage >= nextStage)
				{
					if (isAni)
					{
						this.PlaySliderAnimation(this.slider.value, 1f);
					}
					else
					{
						this.slider.value = 1f;
					}
				}
				if (isAni)
				{
					this.PlayNormal();
					return;
				}
				this.iconTrans.localScale = Vector3.one * 0.7f;
				this.textTrans.anchoredPosition = new Vector2(this.textTrans.anchoredPosition.x, -26f);
				return;
			}
		}

		public GameEventProgressData GetData()
		{
			return this.progressData;
		}

		public void ResetInitPos()
		{
			base.transform.position = new Vector3(this.initPosX, base.transform.position.y, base.transform.position.z);
		}

		public void HideArrow()
		{
			this.arrowObj.SetActiveSafe(false);
			this.sliderArrow.SetActiveSafe(false);
		}

		public void ShowSlider()
		{
			if (!this.slider.gameObject.activeSelf)
			{
				this.slider.gameObject.SetActiveSafe(true);
				this.slider.value = 0f;
			}
		}

		private void PlayBig()
		{
			Sequence sequence = DOTween.Sequence();
			float num = 0.2f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.iconTrans, Vector3.one * 1f, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.textTrans, -36f, num, false));
		}

		private void PlayNormal()
		{
			Sequence sequence = DOTween.Sequence();
			float num = 0.2f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.iconTrans, Vector3.one * 0.7f, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.textTrans, -26f, num, false));
		}

		private void PlaySliderAnimation(float current, float to)
		{
			float num = 0.2f;
			float p = current;
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.OnUpdate<Sequence>(TweenSettingsExtensions.Append(DOTween.Sequence(), DOTween.To(() => p, delegate(float x)
			{
				p = x;
			}, to, num)), delegate
			{
				this.slider.value = p;
			}), delegate
			{
				this.slider.value = to;
			});
		}

		public CustomImage imageIcon;

		public CustomText textStage;

		public Slider slider;

		public SpriteRegister register;

		public GameObject arrowObj;

		public GameObject sliderArrow;

		public CanvasGroup canvasGroup;

		private Dictionary<GameEventType, string> nameDic = new Dictionary<GameEventType, string>
		{
			{
				GameEventType.Select,
				"icon_event"
			},
			{
				GameEventType.BattleNormal,
				"icon_battle"
			},
			{
				GameEventType.BattleElite,
				"icon_elite"
			},
			{
				GameEventType.BattleBoss,
				"icon_boss"
			},
			{
				GameEventType.Rest,
				"icon_rest"
			},
			{
				GameEventType.Fishing,
				"icon_fish"
			},
			{
				GameEventType.None,
				"icon_start"
			}
		};

		private GameEventProgressData progressData;

		private int mIndex;

		private RectTransform iconTrans;

		private RectTransform textTrans;

		private float initPosX;

		private int totalStage;

		private const float Normal_Scale = 0.7f;

		private const float Big_Scale = 1f;

		private const float Normal_Y = -26f;

		private const float Big_Y = -36f;
	}
}
