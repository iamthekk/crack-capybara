using System;
using Spine.Unity;
using UnityEngine;

namespace Framework.Logic.Component
{
	[RequireComponent(typeof(BoneFollower))]
	public class BoneFollowHelper : MonoBehaviour
	{
		public void AutoSet()
		{
			Transform parent = base.transform.parent;
			BoneFollower component = base.GetComponent<BoneFollower>();
			if (parent == null || this.boneName == null || component == null)
			{
				return;
			}
			int childCount = parent.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				SkeletonAnimation component2 = parent.GetChild(i).GetComponent<SkeletonAnimation>();
				if (component2 != null)
				{
					component.SkeletonRenderer = component2;
					break;
				}
			}
			component.boneName = this.boneName;
			component.Initialize();
		}

		public string boneName = string.Empty;
	}
}
