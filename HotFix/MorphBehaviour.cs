using System;
using Framework.Logic.Component;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class MorphBehaviour : MonoBehaviour
	{
		public void Init(GameObject handWeaponNode, int morphId)
		{
			this.bodyAnimator = new SpineAnimator(this.bodyAnimation);
			this.handAnimator = new SpineAnimator(this.handAnimation);
			this.handWeaponNode = handWeaponNode;
			this.morphId = morphId;
			this.boneFollower = handWeaponNode.GetComponent<BoneFollower>();
			this.boneFollowHelper = handWeaponNode.GetComponent<BoneFollowHelper>();
		}

		public void ChangeAnimator(SpineAnimator body, SpineAnimator hand)
		{
			this.bodyAnimator = body;
			this.handAnimator = hand;
		}

		public void DeInit()
		{
			this.bodyAnimator = null;
			this.handAnimator = null;
			this.handWeaponNode = null;
			this.morphId = 0;
			this.boneFollower = null;
			this.boneFollowHelper = null;
		}

		public void Show()
		{
			base.gameObject.ChangeLayer(0);
			this.boneFollower.skeletonRenderer = this.handAnimation;
			this.boneFollowHelper.AutoSet();
		}

		public void Hide()
		{
			base.gameObject.ChangeLayer(6);
		}

		public SkeletonAnimation bodyAnimation;

		public SkeletonAnimation handAnimation;

		[NonSerialized]
		public SpineAnimator bodyAnimator;

		[NonSerialized]
		public SpineAnimator handAnimator;

		[NonSerialized]
		public GameObject handWeaponNode;

		[NonSerialized]
		public int morphId;

		[NonSerialized]
		public BoneFollower boneFollower;

		[NonSerialized]
		public BoneFollowHelper boneFollowHelper;
	}
}
