using System;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class PetOpenEggItemAniEvent : MonoBehaviour
	{
		public void OnGreenEvent()
		{
			GameApp.Sound.PlayClip(666, 1f);
		}

		public void OnBlueEvent()
		{
			GameApp.Sound.PlayClip(667, 1f);
		}

		public void OnPurpleEvent()
		{
			GameApp.Sound.PlayClip(668, 1f);
		}

		public void OnOrangeEvent()
		{
			GameApp.Sound.PlayClip(669, 1f);
		}

		public void OnRedEvent()
		{
			GameApp.Sound.PlayClip(670, 1f);
		}

		public void OnEndEvent()
		{
			GameApp.Sound.PlayClip(675, 1f);
		}
	}
}
