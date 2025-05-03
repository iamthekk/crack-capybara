using System;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Logic.Component
{
	public class AnimatorListen : MonoBehaviour
	{
		public void Listen(string eventParameter)
		{
			this.onListen.Invoke(base.gameObject, eventParameter);
		}

		public static AnimatorListen Get(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			AnimatorListen animatorListen = obj.GetComponent<AnimatorListen>();
			if (animatorListen == null)
			{
				animatorListen = obj.AddComponent<AnimatorListen>();
			}
			return animatorListen;
		}

		public AnimatorListen.ListenEvent onListen = new AnimatorListen.ListenEvent();

		[Serializable]
		public class ListenEvent : UnityEvent<GameObject, string>
		{
		}
	}
}
