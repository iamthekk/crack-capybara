using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class TalentProgressNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.prefabTalentRewardNode1.gameObject.SetActive(false);
			this.prefabTalentRewardNode2.gameObject.SetActive(false);
			this.prefabTalentStageReward2.gameObject.SetActive(false);
			this.prefabTalentStageReward3.gameObject.SetActive(false);
			this.imgFillR.gameObject.SetActive(false);
			this.ScreenAdaptation();
		}

		private void ScreenAdaptation()
		{
			Vector2 size = base.gameObject.GetComponent<RectTransform>().rect.size;
			Vector2 sizeDelta = this.slider.GetComponent<RectTransform>().sizeDelta;
			float num = (size.x - sizeDelta.x) / 2f;
			this.SetLeftAndRight(this.sliderBackground, -num, -num);
			this.imgFilllLeft.sizeDelta = new Vector2(num, this.imgFilllLeft.sizeDelta.y);
			this.imgFilllRight.sizeDelta = new Vector2(num, this.imgFilllRight.sizeDelta.y);
		}

		private void SetLeftAndRight(RectTransform rtf, float left, float right)
		{
			float num = rtf.rect.width - left - right;
			rtf.anchorMin = new Vector2(0f, rtf.anchorMin.y);
			rtf.anchorMax = new Vector2(1f, rtf.anchorMax.y);
			rtf.sizeDelta = new Vector2(num, rtf.sizeDelta.y);
			rtf.offsetMin = new Vector2(left, rtf.offsetMin.y);
			rtf.offsetMax = new Vector2(-right, rtf.offsetMax.y);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.rewardNodeList.Count; i++)
			{
				this.rewardNodeList[i].DeInit();
			}
			this.rewardNodeList.Clear();
			this.rewardNodeList = null;
		}

		public void InitProgress(TalentProgressData progressData)
		{
			this.data = progressData;
			this.curExp = progressData.curLevel;
			this.slider.value = progressData.GetProgress();
			RectTransform rectTransform = this.expNode as RectTransform;
			float x = this.sliderRectTransform.sizeDelta.x;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			float num = 0f;
			float num2 = (float)this.data.startLevel;
			for (int i = 0; i < this.data.rewardList.Count; i++)
			{
				TalentProgressRewardData talentProgressRewardData = this.data.rewardList[i];
				TalentRewardNode talentRewardNode = null;
				if (talentProgressRewardData.rewardShowType == 101)
				{
					talentRewardNode = Object.Instantiate<TalentRewardNode>(this.prefabTalentRewardNode1, this.rewardRoot, false);
				}
				else if (talentProgressRewardData.rewardShowType == 102 || talentProgressRewardData.rewardShowType == 103)
				{
					talentRewardNode = Object.Instantiate<TalentRewardNode>(this.prefabTalentRewardNode2, this.rewardRoot, false);
				}
				else if (talentProgressRewardData.rewardShowType == 104)
				{
					talentRewardNode = Object.Instantiate<TalentRewardNode>(this.prefabTalentRewardNode2, this.rewardRoot, false);
				}
				else if (talentProgressRewardData.rewardShowType == 2)
				{
					talentRewardNode = Object.Instantiate<TalentRewardNode>(this.prefabTalentStageReward2, this.rewardRoot, false);
				}
				else if (talentProgressRewardData.rewardShowType == 3)
				{
					talentRewardNode = Object.Instantiate<TalentRewardNode>(this.prefabTalentStageReward3, this.rewardRoot, false);
				}
				if (talentRewardNode != null)
				{
					talentRewardNode.gameObject.SetActive(true);
					talentRewardNode.Init();
					talentRewardNode.SetData(talentProgressRewardData);
					talentRewardNode.SetReceivedState(this.curExp >= talentProgressRewardData.exp);
					talentRewardNode.UpdateReceivedStateView();
					this.rewardNodeList.Add(talentRewardNode);
					(talentRewardNode.transform as RectTransform).anchoredPosition = this.rewardPointList[i].anchoredPosition;
				}
				if ((float)this.curExp >= num2 && this.curExp <= talentProgressRewardData.exp)
				{
					float num3 = Mathf.Clamp01(((i == 0) ? this.rewardPointList[i].anchoredPosition.x : (this.rewardPointList[i].anchoredPosition.x - this.rewardPointList[i - 1].anchoredPosition.x)) / this.sliderRectTransform.sizeDelta.x);
					float num4 = Mathf.Clamp01(((float)this.curExp - num2) * 1f / ((float)talentProgressRewardData.exp - num2));
					float num5 = Mathf.Clamp01(num + num3 * num4);
					this.slider.value = num5;
					anchoredPosition.x = this.slider.value * x;
					rectTransform.anchoredPosition = anchoredPosition;
					this.txtExp.text = string.Format("{0}", this.curExp);
				}
				num2 = (float)talentProgressRewardData.exp;
				num = Mathf.Clamp01(this.rewardPointList[i].anchoredPosition.x / this.sliderRectTransform.sizeDelta.x);
			}
			if (this.data.isTheEndProgress && this.data.rewardList.Count > 0 && this.curExp > this.data.rewardList[this.data.rewardList.Count - 1].exp)
			{
				int num6 = this.data.rewardList.Count - 1;
				TalentProgressRewardData talentProgressRewardData2 = this.data.rewardList[num6];
				float num7 = Mathf.Clamp01(((num6 == 0) ? this.rewardPointList[num6].anchoredPosition.x : (this.rewardPointList[num6].anchoredPosition.x - this.rewardPointList[num6 - 1].anchoredPosition.x)) / this.sliderRectTransform.sizeDelta.x);
				float num8 = Mathf.Clamp01(((float)this.curExp - num2) * 1f / ((float)talentProgressRewardData2.exp - num2));
				float num9 = Mathf.Clamp01(num + num7 * num8);
				this.slider.value = num9;
				anchoredPosition.x = this.slider.value * x;
				rectTransform.anchoredPosition = anchoredPosition;
				this.txtExp.text = string.Format("{0}", this.curExp);
			}
		}

		public void PlayProgressAnimation(bool isAnimation, int exp, Action<TalentProgressNode, int> callback)
		{
			if (this == null || base.gameObject == null || !base.isActiveAndEnabled)
			{
				return;
			}
			base.StartCoroutine(this.PlayProgressAnimationImpl(isAnimation, exp, callback));
		}

		private IEnumerator PlayProgressAnimationImpl(bool isAnimation, int exp, Action<TalentProgressNode, int> callback)
		{
			if (this.curExp >= this.data.maxLevel)
			{
				if (exp >= this.data.maxLevel && this.data.isTheEndProgress)
				{
					this.txtExp.text = string.Format("{0}", exp);
				}
				if (callback != null)
				{
					callback(this, 0);
				}
				yield break;
			}
			this.targetExp = Mathf.Clamp(exp, this.data.startLevel, this.data.maxLevel);
			float num = 0.6f;
			if (!isAnimation)
			{
				num = 0f;
			}
			RectTransform rectTransform = this.expNode as RectTransform;
			float x = this.sliderRectTransform.sizeDelta.x;
			float num2 = rectTransform.anchoredPosition.x;
			float num3 = 0f;
			float num4 = (float)this.data.startLevel;
			int num9;
			for (int i = 0; i < this.rewardNodeList.Count; i = num9 + 1)
			{
				if (this.curExp >= this.rewardNodeList[i].data.exp)
				{
					num4 = (float)this.rewardNodeList[i].data.exp;
					num3 = Mathf.Clamp01(this.rewardPointList[i].anchoredPosition.x / this.sliderRectTransform.sizeDelta.x);
				}
				else if (this.curExp < this.rewardNodeList[i].data.exp)
				{
					TalentProgressRewardData talentProgressRewardData = this.rewardNodeList[i].data;
					bool isTriggerReward = this.targetExp >= talentProgressRewardData.exp;
					this.curExp = (isTriggerReward ? this.rewardNodeList[i].data.exp : this.targetExp);
					float num5 = Mathf.Clamp01(((i == 0) ? this.rewardPointList[i].anchoredPosition.x : (this.rewardPointList[i].anchoredPosition.x - this.rewardPointList[i - 1].anchoredPosition.x)) / this.sliderRectTransform.sizeDelta.x);
					float num6 = Mathf.Clamp01(((float)this.curExp - num4) * 1f / ((float)this.rewardNodeList[i].data.exp - num4));
					float num7 = Mathf.Clamp01(num3 + num5 * num6);
					if (isTriggerReward)
					{
						this.rewardNodeList[i].SetReceivedState(true);
					}
					num2 = num7 * x;
					ShortcutExtensions46.DOAnchorPosX(rectTransform, num2, num, false);
					ShortcutExtensions46.DOValue(this.slider, num7, num, false);
					if (isAnimation)
					{
						yield return new WaitForSeconds(num);
					}
					this.txtExp.text = string.Format("{0}", this.curExp);
					if (isTriggerReward)
					{
						this.rewardNodeList[i].UpdateReceivedStateView();
						float num8 = this.rewardNodeList[i].PlayRewardAnimation();
						if (isAnimation)
						{
							yield return new WaitForSeconds(num8);
						}
					}
					if (callback != null)
					{
						callback(this, isTriggerReward ? this.rewardNodeList[i].data.id : 0);
					}
					yield break;
				}
				num9 = i;
			}
			if (callback != null)
			{
				callback(this, 0);
			}
			yield break;
		}

		public int PlayOpenAnimation()
		{
			for (int i = 0; i < this.rewardNodeList.Count; i++)
			{
				this.rewardNodeList[i].StartCoroutine(this.rewardNodeList[i].PlayOpenAnimation(i * 10));
			}
			return this.rewardNodeList.Count * 10;
		}

		public RectTransform rewardRoot;

		public RectTransform sliderRectTransform;

		public Slider slider;

		public Transform expNode;

		public CustomText txtExp;

		public CustomImage imgExp;

		public CustomImage imgFillR;

		public List<RectTransform> rewardPointList = new List<RectTransform>();

		public TalentRewardNode prefabTalentRewardNode1;

		public TalentRewardNode prefabTalentRewardNode2;

		public TalentRewardNode prefabTalentStageReward2;

		public TalentRewardNode prefabTalentStageReward3;

		public TalentProgressData data;

		public List<TalentRewardNode> rewardNodeList = new List<TalentRewardNode>();

		public int targetExp;

		public int curExp;

		public RectTransform sliderBackground;

		public RectTransform imgFilllLeft;

		public RectTransform imgFilllRight;
	}
}
