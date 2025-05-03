using System;
using System.Collections;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentRewardNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.talentReward.Init();
			if (this.btnIcon != null)
			{
				this.btnIcon.m_onClick = new Action(this.OnBtnIconClick);
			}
		}

		protected override void OnDeInit()
		{
			this.talentReward.DeInit();
			if (this.btnIcon != null)
			{
				this.btnIcon.m_onClick = null;
			}
		}

		public void SetData(TalentProgressRewardData data)
		{
			this.data = data;
			if (data.evolution > 0)
			{
				TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(data.evolution);
				this.receivedGo.SetActive(false);
				this.imgCheckmark.SetActive(false);
				if (elementById != null)
				{
					this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.stepLanguageId);
				}
				else
				{
					this.txtDesc.text = string.Format("{0}", this.data.exp);
				}
			}
			else
			{
				this.txtDesc.text = string.Format("{0}", this.data.exp);
			}
			if (data.rewardType == 1 || data.atlasId > 0)
			{
				this.rewardGo.SetActive(true);
				this.imgCheckmark.SetActive(true);
				this.talentReward.SetData(data.id);
				return;
			}
			this.rewardGo.SetActive(false);
			this.imgCheckmark.SetActive(false);
		}

		public void SetReceivedState(bool isReceived)
		{
			this.isReceived = isReceived;
		}

		public void UpdateReceivedStateView()
		{
			this.receivedGo.SetActive(this.isReceived);
			this.imgCheckmark.SetActive(this.isReceived);
		}

		public IEnumerator PlayOpenAnimation(int delayFrame)
		{
			while (delayFrame > 0)
			{
				int num = delayFrame;
				delayFrame = num - 1;
				yield return 0;
			}
			this.animator.Play("Open");
			yield break;
		}

		public float PlayRewardAnimation()
		{
			GameApp.Sound.PlayClip(648, 1f);
			float num;
			if (this.data.rewardShowType == 2 || this.data.rewardShowType == 3)
			{
				this.animator.Play("Stage");
				num = DxxTools.Animator.GetAnimationLength(this.animator, "Stage");
			}
			else
			{
				this.animator.Play("Reward");
				num = DxxTools.Animator.GetAnimationLength(this.animator, "Reward");
			}
			return num;
		}

		private void OnBtnIconClick()
		{
			if (this.data.rewardShowType == 2 || this.data.rewardShowType == 3)
			{
				TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.data.evolution);
				TalentRewardTipViewModule.OpenData openData = new TalentRewardTipViewModule.OpenData();
				openData.title = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.stepLanguageId);
				openData.desc = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.desc);
				openData.clickPos = base.transform.position;
				RectTransform rectTransform = this.btnIcon.transform as RectTransform;
				openData.offsetY = rectTransform.rect.height * rectTransform.localScale.y * 0.5f + 10f;
				GameApp.View.OpenView(ViewName.TalentRewardTipViewModule, openData, 1, null, null);
			}
		}

		public CustomButton btnIcon;

		public Animator animator;

		public GameObject rewardGo;

		public TalentReward talentReward;

		public GameObject receivedGo;

		public GameObject imgCheckmark;

		public CustomText txtDesc;

		[NonSerialized]
		public bool isReceived;

		public TalentProgressRewardData data;
	}
}
