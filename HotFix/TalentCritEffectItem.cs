using System;
using UnityEngine;

namespace HotFix
{
	public class TalentCritEffectItem : MonoBehaviour
	{
		private void Awake()
		{
			this.crit2.gameObject.SetActive(false);
			this.crit5.gameObject.SetActive(false);
			this.effect2.SetActive(false);
			this.effect5.SetActive(false);
		}

		public void PlayCritAnimation(int talentLevelUpCritType)
		{
			if (talentLevelUpCritType == 2)
			{
				this.crit5.gameObject.SetActive(true);
				this.effect5.SetActive(true);
			}
			else
			{
				this.crit2.gameObject.SetActive(true);
				this.effect2.SetActive(true);
			}
			string text = "Show";
			this.animator.Play(text);
			float animationLength = DxxTools.Animator.GetAnimationLength(this.animator, text);
			DelayCall.Instance.CallOnce((int)(animationLength * 1000f) + 100, delegate
			{
				if (this != null && base.gameObject != null)
				{
					Object.Destroy(base.gameObject);
				}
			});
		}

		public Animator animator;

		public CanvasGroup canvasGroup;

		public Transform crit2;

		public Transform crit5;

		public GameObject effect2;

		public GameObject effect5;
	}
}
