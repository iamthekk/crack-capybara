using System;
using Framework;
using Framework.Logic;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class CommonFundNodeBottom : CommonFundNodeBase
	{
		protected override void OnInit()
		{
			this.buttonBox.onClick.AddListener(new UnityAction(this.OnClickFinalReward));
		}

		protected override void OnDeInit()
		{
			this.buttonBox.onClick.RemoveListener(new UnityAction(this.OnClickFinalReward));
		}

		public void SetData(CommonFundUIData data, int index, int atlasId, string icon, string tipLanguageId, Action onGetFinal)
		{
			this.mData = data;
			this.mIndex = index;
			this.OnGetFinalReward = onGetFinal;
			string atlasPath = GameApp.Table.GetAtlasPath(atlasId);
			this.imageIcon.SetImage(atlasPath, icon);
			this.textTip.text = Singleton<LanguageManager>.Instance.GetInfoByID(tipLanguageId, new object[]
			{
				this.mData.Score,
				this.mData.LoopRewardLimit
			});
		}

		public void SetStatus(int curScore, int getFinalNum, bool hasBuy)
		{
			this.CurrentScore = curScore;
			this.GetFinalNum = getFinalNum;
			this.HasBuy = hasBuy;
		}

		public void Refresh()
		{
			if (this.mData == null || base.gameObject == null)
			{
				return;
			}
			int previousScore = this.mData.PreviousScore;
			int num = 0;
			bool flag = this.GetFinalNum == this.mData.LoopRewardLimit;
			if (this.CurrentScore >= previousScore + this.mData.Score * this.mData.LoopRewardLimit)
			{
				flag = true;
			}
			if (this.CurrentScore - previousScore > 0)
			{
				num = (this.CurrentScore - previousScore) % this.mData.Score;
			}
			int score = this.mData.Score;
			this.sliderProgress.value = (flag ? 1f : Utility.Math.Clamp01((float)num / (float)score));
			this.textProgress.text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_finish") : string.Format("{0}/{1}", num, score));
			this.textCount.text = string.Format("({0}/{1})", this.GetFinalNum, this.mData.LoopRewardLimit);
			int num2 = 0;
			if (this.CurrentScore >= previousScore)
			{
				num2 = this.CurrentScore - previousScore - this.mData.Score * this.GetFinalNum;
			}
			bool flag2 = this.HasBuy && this.GetFinalNum < this.mData.LoopRewardLimit && num2 >= this.mData.Score;
			if (!this.HasBuy)
			{
				this.redNode.SetActiveSafe(false);
				this.boxAni.Play("Idle");
				this.imageBox.sprite = this.spriteBoxNormal;
				return;
			}
			this.redNode.SetActiveSafe(flag2);
			if (!flag2)
			{
				this.imageBox.sprite = this.spriteBoxNormal;
				this.boxAni.Play("Idle");
				return;
			}
			this.imageBox.sprite = this.spriteBoxLight;
			this.boxAni.Play("Shake");
		}

		private void OnClickFinalReward()
		{
			Action onGetFinalReward = this.OnGetFinalReward;
			if (onGetFinalReward == null)
			{
				return;
			}
			onGetFinalReward();
		}

		public CustomText textTip;

		public Slider sliderProgress;

		public CustomImage imageIcon;

		public CustomText textProgress;

		public CustomButton buttonBox;

		public CustomText textCount;

		public Image imageBox;

		public GameObject redNode;

		public Animator boxAni;

		public Sprite spriteBoxNormal;

		public Sprite spriteBoxLight;

		private CommonFundUIData mData;

		private int mIndex;

		private int CurrentScore;

		private int GetFinalNum;

		private bool HasBuy;

		private Action OnGetFinalReward;
	}
}
