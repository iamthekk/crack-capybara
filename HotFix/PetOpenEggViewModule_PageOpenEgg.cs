using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using MoreMountains.Feedbacks;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class PetOpenEggViewModule_PageOpenEgg : MonoBehaviour
	{
		public async void OnInit(int petBoxType, List<PetDto> petList, List<ulong> newPetRowIds)
		{
			this.petList = petList;
			this.newPetRowIds = newPetRowIds;
			this.freeEgg.SetActive(false);
			this.diamondEgg.SetActive(false);
			this.btnTapClose.gameObject.SetActive(false);
			this.btnSkip.gameObject.SetActive(false);
			if (petBoxType == 11)
			{
				this.curOpenAnim = this.OpenEggPetAnim;
				this.curCloseAnim = this.CloseEggPetAnim;
				this.curEgg = this.freeEgg;
			}
			else
			{
				this.curOpenAnim = this.OpenEggDiamondAnim;
				this.curCloseAnim = this.CloseEggDiamondAnim;
				this.curEgg = this.diamondEgg;
			}
			this.curEgg.SetActive(true);
			this.curEgg.transform.Find("Root").GetComponent<Animator>().ResetTrigger("Idle");
			await this.PopView.OnInit();
			foreach (MMFeedback mmfeedback in this.curOpenAnim.Feedbacks)
			{
				if (mmfeedback.Label == "开蛋动画")
				{
					this.FeelOpenEggPetAnimator = mmfeedback as MMFeedbackAnimation;
				}
				else if (mmfeedback.Label == "角色显示弹窗")
				{
					this.FeelSetActivePopUpReward = mmfeedback as MMFeedbackSetActive;
				}
				else if (mmfeedback.Label == "显示弹窗动画")
				{
					this.FeelPopUpRewardAnimator = mmfeedback as MMFeedbackAnimation;
				}
				else if (mmfeedback.Label == "结束事件")
				{
					this.FeelFinishEvents = mmfeedback as MMFeedbackEvents;
				}
			}
		}

		public void OnDeInit()
		{
		}

		public void PlayOpenEggsAnimation(Action callback)
		{
			this.btnTapClose.m_onClick = callback;
			this.btnSkip.m_onClick = callback;
			this.btnTapClose.gameObject.SetActive(false);
			this.btnSkip.gameObject.SetActive(true);
			this.PlayAnimation();
		}

		private void PlayAnimation()
		{
			this.petDto = this.petList[this.playIndex];
			this.isNew = this.newPetRowIds != null && this.newPetRowIds.Contains(this.petDto.RowId);
			this.InitNextCardInfo();
			this.PlayOpenEggAnim();
		}

		private void PlayOpenEggAnim()
		{
			this.curOpenAnim.PlayFeedbacks();
		}

		private void PlayCloseEggAnim()
		{
			this.curCloseAnim.PlayFeedbacks();
		}

		private void InitNextCardInfo()
		{
			int num = int.Parse(GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.petDto.ConfigId)
				.itemTypeParam[0]);
			int quality = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(num)
				.quality;
			this.OpenEggShowSSS.gameObject.SetActive(false);
			this.OpenEggShowSS.gameObject.SetActive(false);
			this.OpenEggShowS.gameObject.SetActive(false);
			this.OpenEggShowA.gameObject.SetActive(false);
			this.OpenEggShowB.gameObject.SetActive(false);
			this.OpenEggShowC.gameObject.SetActive(false);
			switch (quality)
			{
			case 1:
				this.FeelOpenEggPetAnimator.ResetTriggerName("ElematesD");
				this.FeelSetActivePopUpReward.Timing.InitialDelay = this.CDelayActiveTime;
				this.OpenEggShowC.gameObject.SetActive(true);
				break;
			case 2:
				this.FeelOpenEggPetAnimator.ResetTriggerName("ElematesC");
				this.FeelSetActivePopUpReward.Timing.InitialDelay = this.CDelayActiveTime;
				this.OpenEggShowC.gameObject.SetActive(true);
				break;
			case 3:
				this.FeelOpenEggPetAnimator.ResetTriggerName("ElematesB");
				this.FeelSetActivePopUpReward.Timing.InitialDelay = this.BDelayActiveTime;
				this.OpenEggShowB.gameObject.SetActive(true);
				break;
			case 4:
				this.FeelOpenEggPetAnimator.ResetTriggerName("ElematesA");
				this.FeelSetActivePopUpReward.Timing.InitialDelay = this.ADelayActiveTime;
				this.OpenEggShowA.gameObject.SetActive(true);
				break;
			case 5:
				this.FeelOpenEggPetAnimator.ResetTriggerName("ElematesS");
				this.FeelSetActivePopUpReward.Timing.InitialDelay = this.SDelayActiveTime;
				this.OpenEggShowS.gameObject.SetActive(true);
				break;
			case 6:
				this.FeelOpenEggPetAnimator.ResetTriggerName("ElematesSS");
				this.FeelSetActivePopUpReward.Timing.InitialDelay = this.SSDelayActiveTime;
				this.OpenEggShowSS.gameObject.SetActive(true);
				break;
			}
			this.RectTransformGroupInfo.gameObject.SetActive(true);
			this.PopView.InitPet(num, quality, this.isNew);
			this.FeelPopUpRewardAnimator.Timing.InitialDelay = this.FeelSetActivePopUpReward.Timing.InitialDelay;
			this.FeelPopUpRewardAnimator.ResetTriggerName("FirstTime");
			this.FeelFinishEvents.Timing.InitialDelay = this.NewPetDelayTime;
		}

		public void OpenEggFinish()
		{
			GameApp.Sound.PlayClip(662, 1f);
			if (this.playIndex + 1 >= this.petList.Count)
			{
				this.btnTapClose.gameObject.SetActive(true);
				this.btnSkip.gameObject.SetActive(false);
				return;
			}
			this.PlayCloseEggAnim();
		}

		public void CloseEggFinish()
		{
			this.playIndex++;
			this.PlayAnimation();
		}

		[Header("Button")]
		public CustomButton btnSkip;

		public CustomButton btnTapClose;

		[Header("Egg")]
		public GameObject freeEgg;

		public GameObject diamondEgg;

		[Header("开蛋相关配置：")]
		protected MMFeedbackAnimation FeelOpenEggPetAnimator;

		protected MMFeedbackSetActive FeelSetActivePopUpReward;

		protected MMFeedbackAnimation FeelPopUpRewardAnimator;

		protected MMFeedbackEvents FeelFinishEvents;

		[Space(5f)]
		[Header("延迟一定事件显示奖励：")]
		public float SSSDelayActiveTime;

		public float SSDelayActiveTime;

		public float SDelayActiveTime;

		public float ADelayActiveTime;

		public float BDelayActiveTime;

		public float CDelayActiveTime;

		[Space(5f)]
		[Header("首次或重复获取延迟事件：")]
		public float RepPetDelayTime = 0.6f;

		public float NewPetDelayTime = 1f;

		[Space(5f)]
		[Header("不同奖励对应的特效：")]
		public RectTransform OpenEggShowSSS;

		public RectTransform OpenEggShowSS;

		public RectTransform OpenEggShowS;

		public RectTransform OpenEggShowA;

		public RectTransform OpenEggShowB;

		public RectTransform OpenEggShowC;

		[Space(5f)]
		[Header("Feel 动画流程：")]
		public MMFeedbacks OpenEggPetAnim;

		public MMFeedbacks CloseEggPetAnim;

		public MMFeedbacks OpenEggDiamondAnim;

		public MMFeedbacks CloseEggDiamondAnim;

		[Space(5f)]
		public RectTransform RectTransformGroupInfo;

		public UIElematisPopRewardView PopView;

		private GameObject curEgg;

		private MMFeedbacks curOpenAnim;

		private MMFeedbacks curCloseAnim;

		private List<PetDto> petList;

		private List<ulong> newPetRowIds;

		private int playIndex;

		private PetDto petDto;

		private bool isNew;
	}
}
