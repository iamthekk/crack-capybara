using System;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class DontDestory : MonoBehaviour
	{
		private void Awake()
		{
			Object.DontDestroyOnLoad(base.transform.gameObject);
		}
	}
}
