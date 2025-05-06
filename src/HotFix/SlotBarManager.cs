using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class SlotBarManager : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.imgClone.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
		}

		public void SetViewModule(SlotMachineViewModule viewModule, int index)
		{
			this.m_index = index;
			this.slotMachineViewModule = viewModule;
		}

		public int ResultSymbolIndex
		{
			get
			{
				return this.resultSymbolIndex;
			}
		}

		public void SetResultSymbol(int symbolIndex)
		{
			this.resultSymbolIndex = symbolIndex;
		}

		public void SetUpSymbols(List<int> symbols)
		{
			this.numberOfSymbols = symbols.Count;
			this.regularSymbolElements = new Transform[this.numberOfSymbols];
			this.blurSymbolElements = new Transform[this.numberOfSymbols];
			this.slotSymbols = symbols;
			for (int i = 0; i < this.numberOfSymbols; i++)
			{
				int num = symbols[i];
				ChapterMiniGame_slotReward elementById = GameApp.Table.GetManager().GetChapterMiniGame_slotRewardModelInstance().GetElementById(num);
				CustomImage customImage = Object.Instantiate<CustomImage>(this.imgClone, this.regularGroup.transform, false);
				customImage.gameObject.SetActive(true);
				customImage.SetImage(elementById.atlasRegular, elementById.iconRegular);
				this.regularSymbolElements[i] = customImage.transform;
				CustomImage customImage2 = Object.Instantiate<CustomImage>(this.imgClone, this.blurGroup.transform, false);
				customImage2.gameObject.SetActive(true);
				customImage2.SetImage(elementById.atlasBlur, elementById.iconBlur);
				this.blurSymbolElements[i] = customImage2.transform;
			}
			this.ShowRegular();
		}

		public void ActivateSpin(float delayTime)
		{
			base.StartCoroutine(this.ActivateSpinWithDelay(delayTime));
		}

		public void EndSpin(string animName, float delayTime)
		{
			base.StartCoroutine(this.EndSpinImpl(animName, delayTime));
		}

		private IEnumerator EndSpinImpl(string animName, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			this.endAnimName = animName;
			this.spinning = false;
			this.endSpinning = true;
			yield break;
		}

		private IEnumerator ActivateSpinWithDelay(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			this.ActivateSpin();
			yield break;
		}

		public void ActivateSpin()
		{
			if (this.m_index == 2)
			{
				GameApp.Sound.PlayClip(61, 1f);
			}
			else
			{
				GameApp.Sound.PlayClip(60, 1f);
			}
			this.spinning = true;
			this.endSpinning = false;
			this.endAnimating = false;
			this.ShowBlur();
		}

		public void SetSymbolByIndex(int symbolIndex)
		{
			this.resultSymbolIndex = symbolIndex;
			this.spinAmount = (float)symbolIndex / (float)this.numberOfSymbols % 1f;
			float num = this.spinAmount * (float)this.numberOfSymbols;
			this.UpdateSpinSymbolsPosition(num);
		}

		private void ShowBlur()
		{
			this.isBlured = true;
			this.blurGroup.SetActive(true);
			this.regularGroup.SetActive(false);
		}

		private void ShowRegular()
		{
			this.isBlured = false;
			this.blurGroup.SetActive(false);
			this.regularGroup.SetActive(true);
		}

		private void Update()
		{
			if (this.spinning)
			{
				this.spinAmount = (this.spinAmount + this.speedFactor * this.speedBet * Time.deltaTime) % 1f;
				float num = this.spinAmount * (float)this.numberOfSymbols;
				this.UpdateSpinSymbolsPosition(num);
				return;
			}
			if (!this.endSpinning)
			{
				if (this.endAnimating)
				{
					float num2 = (this.spinAmount + this.additiveAnimAmount) * (float)this.numberOfSymbols;
					this.UpdateSpinSymbolsPosition(num2);
					if (this.isBlured)
					{
						this.ShowRegular();
					}
				}
				return;
			}
			float num3 = this.spinAmount + this.speedFactor * this.speedBet * Time.deltaTime;
			this.spinAmount = num3 % 1f;
			float num4 = this.spinAmount * (float)this.numberOfSymbols;
			if (!this.readyToStop && (num3 > 1f || num4 < (float)this.resultSymbolIndex))
			{
				this.readyToStop = true;
			}
			if (this.readyToStop && num4 >= (float)this.resultSymbolIndex)
			{
				num4 = (float)this.resultSymbolIndex;
				this.spinAmount = num4 / (float)this.numberOfSymbols;
				this.UpdateSpinSymbolsPosition(num4);
				float animationLength = this.GetAnimationLength(this.animator, this.endAnimName);
				this.animator.Play(this.endAnimName, 0, 1f / animationLength * Time.deltaTime);
				DelayCall.Instance.CallOnce((int)(animationLength * 1000f), new DelayCall.CallAction(this.OnAnimationEnded));
				this.endSpinning = false;
				this.readyToStop = false;
				this.endAnimating = true;
				return;
			}
			this.UpdateSpinSymbolsPosition(num4);
		}

		private void UpdateSpinSymbolsPosition(float spinValue)
		{
			for (int i = 0; i < this.numberOfSymbols; i++)
			{
				float num = ((float)i - spinValue) % (float)this.numberOfSymbols * 1.1f;
				if (num > (float)this.numberOfSymbols / 2.2f)
				{
					num -= (float)this.numberOfSymbols * 1.1f;
				}
				else if (num < -(float)this.numberOfSymbols / 2.2f)
				{
					num += (float)this.numberOfSymbols * 1.1f;
				}
				float num2 = 166f;
				this.blurSymbolElements[i].localPosition = Vector3.up * (num * num2);
				this.regularSymbolElements[i].localPosition = Vector3.up * (num * num2);
				this.regularSymbolElements[i].name = string.Format("index_{0}", i);
			}
		}

		public void OnAnimationEnded()
		{
			if (this == null)
			{
				return;
			}
			this.endAnimating = false;
			this.animator.Play("None");
			this.ShowRegular();
			this.UpdateSpinSymbolsPosition((float)this.resultSymbolIndex);
			this.slotMachineViewModule.SlotBarAnimComplete(this.barID);
		}

		public float GetAnimationLength(Animator animator, string animationName)
		{
			RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
			if (runtimeAnimatorController == null)
			{
				return 0f;
			}
			foreach (AnimationClip animationClip in runtimeAnimatorController.animationClips)
			{
				if (animationClip.name == animationName)
				{
					return animationClip.length;
				}
			}
			return 0f;
		}

		public Animator animator;

		public CustomImage imgClone;

		public Action<int, int> OnImageExitReel;

		public int barID;

		private int numberOfSymbols;

		public GameObject regularGroup;

		public GameObject blurGroup;

		public Transform[] regularSymbolElements;

		public Transform[] blurSymbolElements;

		public List<int> slotSymbols;

		private bool spinning;

		private bool endSpinning;

		private bool readyToStop;

		private bool endAnimating;

		private int resultSymbolIndex;

		private string endAnimName;

		private bool isBlured;

		private float speedFactor = 3f;

		public float speedBet = 1f;

		public float spinAmount;

		public float additiveAnimAmount;

		private int m_index;

		private SlotMachineViewModule slotMachineViewModule;
	}
}
