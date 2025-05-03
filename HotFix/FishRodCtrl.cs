using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class FishRodCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.spine.Initialize(true);
		}

		protected override void OnDeInit()
		{
		}

		public void Play(FishRodCtrl.SkinType skinType, FishRodCtrl.AnimType animType, bool isLoop = true, Action onFinish = null, float speed = 1f)
		{
			this.SetSkin(skinType);
			this.PlayAni(animType, isLoop, onFinish, speed);
		}

		public void SetSkin(FishRodCtrl.SkinType skinType)
		{
			string text = skinType.ToString();
			this.spine.Skeleton.SetSkin(text);
		}

		private void PlayAni(FishRodCtrl.AnimType animType, bool isLoop, Action callBack, float animationSpeed)
		{
			this.spine.AnimationState.TimeScale = animationSpeed;
			string animName = this.GetAnimName(animType);
			TrackEntry trackEntry2 = this.spine.AnimationState.SetAnimation(0, animName, isLoop);
			this._lastType = animType;
			if (callBack != null)
			{
				trackEntry2.Complete += delegate(TrackEntry trackEntry)
				{
					Action callBack2 = callBack;
					if (callBack2 == null)
					{
						return;
					}
					callBack2();
				};
			}
		}

		private string GetAnimName(FishRodCtrl.AnimType animType)
		{
			string text = animType.ToString();
			return text.Substring(1, text.Length - 1);
		}

		public void ShowName(bool isShow)
		{
			this.rodName.gameObject.SetActive(isShow);
		}

		public void RefreshName(string nameId)
		{
			this.rodName.text = Singleton<LanguageManager>.Instance.GetInfoByID(nameId);
		}

		public void ShowFishStateTip(bool isShow)
		{
			this.fishStateTip.SetActiveSafe(isShow);
		}

		public float smoothTime = 0.2f;

		public SkeletonGraphic spine;

		public CustomText rodName;

		public GameObject fishStateTip;

		private FishRodCtrl.AnimType _lastType;

		public enum SkinType
		{
			None,
			A,
			B,
			C,
			D
		}

		public enum AnimType
		{
			_00,
			_01,
			_02,
			_03,
			_04,
			_05,
			_06,
			_14,
			_15,
			_Z01,
			_Z02
		}
	}
}
