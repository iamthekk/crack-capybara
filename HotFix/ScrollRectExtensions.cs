using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public static class ScrollRectExtensions
	{
		public static void MoveCenterToItem(this ScrollRect scrollRect, RectTransform viewport, RectTransform content, RectTransform item, float duration = 0f)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			Vector3 vector = scrollRect.transform.InverseTransformVector(ScrollRectExtensions.ConvertLocalPosToWorldPos(viewport));
			Vector3 vector2 = scrollRect.transform.InverseTransformVector(ScrollRectExtensions.ConvertLocalPosToWorldPos(item)) - vector;
			vector2.z = 0f;
			Vector2 vector3;
			vector3..ctor(vector2.x / (content.rect.width - viewport.rect.width), vector2.y / (content.rect.height - viewport.rect.height));
			Vector2 vector4 = scrollRect.normalizedPosition + vector3;
			vector4..ctor(Mathf.Clamp01(vector4.x), Mathf.Clamp01(vector4.y));
			if (duration > 0f)
			{
				scrollRect.StartCoroutine(scrollRect.MoveNormalizedPosition(scrollRect.normalizedPosition, vector4, duration));
				return;
			}
			scrollRect.normalizedPosition = vector4;
		}

		private static IEnumerator MoveNormalizedPosition(this ScrollRect scrollRect, Vector2 normalizedPositionStart, Vector2 normalizedPositionEnd, float duration)
		{
			float elapsedTime = 0f;
			while (elapsedTime < duration)
			{
				float num = elapsedTime / duration;
				scrollRect.normalizedPosition = Vector2.Lerp(normalizedPositionStart, normalizedPositionEnd, num);
				elapsedTime += Time.fixedDeltaTime;
				yield return null;
			}
			yield break;
		}

		private static Vector3 ConvertLocalPosToWorldPos(RectTransform target)
		{
			Vector3 vector;
			vector..ctor((0.5f - target.pivot.x) * target.rect.size.x, (0.5f - target.pivot.y) * target.rect.size.y, 0f);
			Vector3 vector2 = target.localPosition + vector;
			return target.parent.TransformPoint(vector2);
		}
	}
}
