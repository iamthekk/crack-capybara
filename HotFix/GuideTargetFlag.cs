using System;
using UnityEngine;

namespace HotFix
{
	public class GuideTargetFlag : MonoBehaviour
	{
		private void OnDestroy()
		{
			if (!GuideController.IsNull)
			{
				GuideController.Instance.DelTarget(this.TargetKey);
			}
		}

		public static GuideTargetFlag Get(GameObject obj, string key)
		{
			if (obj == null)
			{
				return null;
			}
			GuideTargetFlag guideTargetFlag = obj.GetComponent<GuideTargetFlag>();
			if (guideTargetFlag == null)
			{
				guideTargetFlag = obj.AddComponent<GuideTargetFlag>();
			}
			if (guideTargetFlag != null)
			{
				guideTargetFlag.TargetKey = key;
			}
			return guideTargetFlag;
		}

		public string TargetKey;
	}
}
