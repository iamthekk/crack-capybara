using System;
using System.Collections;
using UnityEngine;

namespace Framework.Coroutine
{
	public class CoroutionAgent : MonoBehaviour
	{
		public void AddTask(IEnumerator routine)
		{
			base.StartCoroutine(routine);
		}

		public void RemoveTask(IEnumerator routine)
		{
			base.StopCoroutine(routine);
		}

		public void RemoveAllTask()
		{
			base.StopAllCoroutines();
		}
	}
}
