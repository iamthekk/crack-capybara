using System;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TalentStateUpgradeSkin : MonoBehaviour
	{
		public void SetShowSkin(int current)
		{
			this.Skin_Current.SetCameraVisible(current == 1);
			this.Skin_Next.SetCameraVisible(current != 1);
		}

		public void AniEnd()
		{
			UnityEvent onAniEnd = this.OnAniEnd;
			if (onAniEnd == null)
			{
				return;
			}
			onAniEnd.Invoke();
		}

		public UIModelItem Skin_Current;

		public UIModelItem Skin_Next;

		public UnityEvent OnAniEnd;
	}
}
