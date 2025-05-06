using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
	public class MapMemberControllerBase
	{
		public virtual void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public float GetPlayerMoveSpeed()
		{
			return this.playerMoveSpeed;
		}

		public float GetPlayerOffsetX()
		{
			return this.playerOffsetX;
		}

		protected Vector3 GetScale(string str)
		{
			Vector3 one = Vector3.one;
			if (!string.IsNullOrEmpty(str))
			{
				string[] array = str.Replace("\n", "").Replace(" ", "").Replace("\t", "")
					.Replace("\r", "")
					.Split(',', StringSplitOptions.None);
				if (array.Length > 1)
				{
					float num = float.Parse(array[0]);
					float num2 = float.Parse(array[1]);
					one..ctor(num, num2, 1f);
				}
			}
			return one;
		}

		protected float playerMoveSpeed = 2f;

		protected float playerOffsetX;

		protected List<Vector3> petOffsetList = new List<Vector3>();
	}
}
