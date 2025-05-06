using System;
using System.Collections.Generic;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class LessHPTrueDamage : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.LessHpCrit;
		}

		protected override void OnInit()
		{
			base.RefreshTargetPositionLessHp(base.target.transform.position);
			this.aniEndCounter = 0;
			for (int i = 0; i < this.nodeListens.Count; i++)
			{
				this.nodeListens[i].onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
			this.m_upAnimatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnUpAnimatorListen));
			HoverLongData hoverLongData = this.hoverData as HoverLongData;
			if (hoverLongData == null)
			{
				return;
			}
			this.count = -hoverLongData.param;
			this.Show();
			this.isInit = true;
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			for (int i = 0; i < this.nodeListens.Count; i++)
			{
				this.nodeListens[i].onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
			this.m_upAnimatorListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnUpAnimatorListen));
		}

		protected override void OnUpdateImpl(float deltaTime, float unscaleDeltaTime)
		{
		}

		private void Show()
		{
			long num = Utility.Math.Abs(this.count);
			char[] array = ((this.count < 0L) ? ("-" + DxxTools.FormatNumber(num)) : DxxTools.FormatNumber(num)).ToCharArray();
			this.aniCount = array.Length;
			for (int i = 0; i < this.nodeTxts.Count; i++)
			{
				if (i < this.aniCount)
				{
					this.nodeTxts[i].transform.parent.gameObject.SetActive(true);
					this.nodeTxts[i].text = array[i].ToString();
				}
				else
				{
					this.nodeTxts[i].transform.parent.gameObject.SetActive(false);
				}
			}
			this.damageTextRoot.GetComponent<ContentSizeFitter>().enabled = true;
			for (int j = 0; j < this.layoutGroups.Count; j++)
			{
				this.layoutGroups[j].enabled = true;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.damageTextRoot as RectTransform);
			this.damageTextRoot.GetComponent<ContentSizeFitter>().enabled = false;
			for (int k = 0; k < this.layoutGroups.Count; k++)
			{
				this.layoutGroups[k].enabled = false;
			}
			HorizontalLayoutGroup[] componentsInChildren = base.transform.GetComponentsInChildren<HorizontalLayoutGroup>(true);
			for (int l = 0; l < componentsInChildren.Length; l++)
			{
				componentsInChildren[l].enabled = false;
			}
			if (this.nodeAnimators.Count > 0 && this.nodeAnimators[0] != null)
			{
				this.nodeAnimators[0].SetTrigger("NodeRun");
				this.curAniIndex = 0;
			}
		}

		private void OnAnimatorListen(GameObject obj, string arg)
		{
			if (arg == "Next")
			{
				this.curAniIndex++;
				if (this.curAniIndex < this.aniCount && this.curAniIndex < this.nodeAnimators.Count && this.nodeAnimators[this.curAniIndex] != null)
				{
					this.nodeAnimators[this.curAniIndex].SetTrigger("NodeRun");
					return;
				}
			}
			else if (arg == "End")
			{
				this.aniEndCounter++;
				if (this.aniEndCounter >= this.aniCount)
				{
					this.m_upAnimator.SetTrigger("Up");
				}
			}
		}

		private void OnUpAnimatorListen(GameObject obj, string arg)
		{
			if (arg.Equals("UpEnd"))
			{
				base.RemoveHover();
			}
		}

		public List<HorizontalLayoutGroup> layoutGroups = new List<HorizontalLayoutGroup>();

		public Transform damageTextRoot;

		public List<CustomText> nodeTxts = new List<CustomText>();

		public List<Animator> nodeAnimators = new List<Animator>();

		public List<AnimatorListen> nodeListens = new List<AnimatorListen>();

		public Animator m_upAnimator;

		public AnimatorListen m_upAnimatorListen;

		private const string RunAni = "NodeRun";

		private const string UpAnimationName = "Up";

		private int aniEndCounter;

		private int curAniIndex;

		private int aniCount;

		private long count;

		private bool isInit;
	}
}
