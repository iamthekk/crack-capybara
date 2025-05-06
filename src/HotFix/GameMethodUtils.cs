using System;
using UnityEngine;

namespace HotFix
{
	public class GameMethodUtils
	{
		public static float Distance(Vector3 a, Vector3 b, float aRadius = 0f, float bRadius = 0f)
		{
			float num = (a.x - b.x) / GameMethodUtils.m_mapOffset.x;
			float num2 = (a.y - b.y) / GameMethodUtils.m_mapOffset.y;
			float num3 = (a.z - b.z) / GameMethodUtils.m_mapOffset.z;
			return (float)Math.Sqrt((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3) - aRadius - bRadius;
		}

		public static Vector3 TransformPoint(Transform transform, Vector3 position)
		{
			Vector3 vector;
			vector..ctor(position.x * GameMethodUtils.m_mapOffset.x, position.y * GameMethodUtils.m_mapOffset.y, position.z * GameMethodUtils.m_mapOffset.z);
			return transform.TransformPoint(vector);
		}

		public static float DistanceZ(Vector3 a, Vector3 b)
		{
			return Math.Abs((a.z - b.z) / GameMethodUtils.m_mapOffset.z);
		}

		public static float DistanceX(Vector3 a, Vector3 b)
		{
			return Math.Abs((a.x - b.x) / GameMethodUtils.m_mapOffset.x);
		}

		public static Vector3 MultiplyOffset(Vector3 a)
		{
			float num = a.x * GameMethodUtils.m_mapOffset.x;
			float num2 = a.y * GameMethodUtils.m_mapOffset.y;
			float num3 = a.z * GameMethodUtils.m_mapOffset.z;
			return new Vector3(num, num2, num3);
		}

		public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
		{
			if (maxDistanceDelta == 0f)
			{
				return current;
			}
			float num = target.x - current.x;
			float num2 = target.y - current.y;
			float num3 = target.z - current.z;
			float num4 = (float)((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3);
			if ((double)num4 == 0.0 || ((double)maxDistanceDelta >= 0.0 && (double)num4 <= (double)maxDistanceDelta * (double)maxDistanceDelta))
			{
				return target;
			}
			num *= GameMethodUtils.m_mapOffset.x;
			num2 *= GameMethodUtils.m_mapOffset.y;
			num3 *= GameMethodUtils.m_mapOffset.z;
			float num5 = (float)Math.Sqrt((double)num4);
			return new Vector3(current.x + num / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
		}

		public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
		{
			t = Mathf.Clamp01(t);
			float num = a.x * GameMethodUtils.m_mapOffset.x;
			float num2 = a.y * GameMethodUtils.m_mapOffset.y;
			float num3 = a.z * GameMethodUtils.m_mapOffset.z;
			float num4 = b.x * GameMethodUtils.m_mapOffset.x;
			float num5 = b.y * GameMethodUtils.m_mapOffset.y;
			float num6 = b.z * GameMethodUtils.m_mapOffset.z;
			return new Vector3(num + (num4 - num) * t, num2 + (num5 - num2) * t, num3 + (num6 - num3) * t);
		}

		public static Vector3 m_mapOffset = new Vector3(1f, 1f, 1f);
	}
}
