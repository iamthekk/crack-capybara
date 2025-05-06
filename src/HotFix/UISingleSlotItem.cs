using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UISingleSlotItem : CustomBehaviour
	{
		public SingleSlotData Data
		{
			get
			{
				return this.mData;
			}
		}

		protected override void OnInit()
		{
			if (this.ObjEffect != null)
			{
				this.ObjEffect.SetActive(false);
			}
			this.AnimListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimListen));
		}

		protected override void OnDeInit()
		{
			this.AnimListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimListen));
		}

		public void SetData(SingleSlotData data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData == null)
			{
				return;
			}
			this.SkillObj.SetActiveSafe(false);
			this.PlayObj.SetActiveSafe(false);
			this.NothingObj.SetActiveSafe(false);
			switch (this.mData.rewardType)
			{
			case SingleSlotRewardType.Nothing:
				this.NothingObj.SetActiveSafe(true);
				this.ImageBG.sprite = this.spriteRegister.GetSprite("nothing");
				this.TextNothing.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.mData.nameId);
				return;
			case SingleSlotRewardType.ANGEL_SCORE_ID:
				this.SkillObj.SetActiveSafe(true);
				this.ImageBG.sprite = this.spriteRegister.GetSprite("skill");
				this.ImageIcon.sprite = this.spriteRegister.GetSprite("angel");
				this.TextSkillName.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.mData.nameId);
				return;
			case SingleSlotRewardType.DEMON_SCORE_ID:
				this.SkillObj.SetActiveSafe(true);
				this.ImageBG.sprite = this.spriteRegister.GetSprite("skill");
				this.ImageIcon.sprite = this.spriteRegister.GetSprite("demon");
				this.TextSkillName.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.mData.nameId);
				return;
			case SingleSlotRewardType.PLAY_NUM:
				this.PlayObj.SetActiveSafe(true);
				this.ImageBG.sprite = this.spriteRegister.GetSprite("playNum");
				this.TextPlay.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.mData.nameId);
				this.TextPlayNum.text = string.Format("+{0}", this.mData.rewardNum);
				return;
			default:
				return;
			}
		}

		public void AutoScale(Vector3 centerPos)
		{
		}

		public void PlayCombineAnimation(Action callback)
		{
			this.mOnPlayOver = callback;
			this.Anim.Play("show");
			float num = (float)this.Anim.GetCurrentAnimatorClipInfo(0).Length;
			DelayCall.Instance.CallOnce((int)(num * 1000f), delegate
			{
				Action callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2();
			});
		}

		private void OnAnimListen(GameObject obj, string str)
		{
			str == "Finish";
		}

		public void ResetAnim()
		{
		}

		[Header("动画")]
		public Animator Anim;

		public AnimatorListen AnimListen;

		public GameObject ObjEffect;

		[Header("UI元素")]
		public Image ImageBG;

		public SpriteRegister spriteRegister;

		public GameObject SkillObj;

		public CustomImage ImageIcon;

		public CustomText TextSkillName;

		public GameObject PlayObj;

		public CustomText TextPlay;

		public CustomText TextPlayNum;

		public GameObject NothingObj;

		public CustomText TextNothing;

		public RectTransform RTFScaleRoot;

		private SingleSlotData mData;

		private int mShowCount;

		private int mTotalCount;

		private int mEachCount;

		private Action mOnPlayOver;
	}
}
