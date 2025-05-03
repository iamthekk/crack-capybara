using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("UI/CustomOutLine", 23)]
	public class CustomOutLine : Shadow
	{
		public Text text
		{
			get
			{
				if (this.m_Text == null)
				{
					this.m_Text = base.GetComponent<Text>();
				}
				return this.m_Text;
			}
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = CustomOutLine.ListPool<UIVertex>.Get();
			vh.GetUIVertexStream(list);
			float num = (float)this.text.fontSize * 0.02f;
			this.distanceScale = 1f * num;
			this.effectscale = 2f * num;
			int num2 = (int)(Mathf.Abs(base.effectDistance.y * this.effectscale) * 2f);
			num2 = Mathf.Clamp(num2, 3, num2);
			float num3 = 3.5f / (float)num2;
			int num4 = list.Count * (5 + num2 * 3);
			if (list.Capacity < num4)
			{
				list.Capacity = num4;
			}
			int num5 = 0;
			int num6 = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale, CustomOutLine.distance.y * this.distanceScale - base.effectDistance.y * this.distanceScale);
			num5 = num6;
			num6 = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale - base.effectDistance.x * this.effectscale * 1f, CustomOutLine.distance.y * this.distanceScale);
			num5 = num6;
			num6 = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale + base.effectDistance.x * this.effectscale * 1f, CustomOutLine.distance.y * this.distanceScale);
			num5 = num6;
			num6 = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale + base.effectDistance.x * this.effectscale * 1f, CustomOutLine.distance.y * this.distanceScale - base.effectDistance.y * this.distanceScale);
			num5 = num6;
			num6 = list.Count;
			base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale - base.effectDistance.x * this.effectscale * 1f, CustomOutLine.distance.y * this.distanceScale - base.effectDistance.y * this.distanceScale);
			num5 = num6;
			num6 = list.Count;
			for (int i = 0; i < num2; i++)
			{
				float num7 = (float)i * num3;
				base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale, CustomOutLine.distance.y * this.distanceScale + base.effectDistance.y * this.effectscale * num7);
				num5 = num6;
				num6 = list.Count;
				base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale - base.effectDistance.x * this.effectscale * 1f, CustomOutLine.distance.y * this.distanceScale + base.effectDistance.y * this.effectscale * num7);
				num5 = num6;
				num6 = list.Count;
				base.ApplyShadowZeroAlloc(list, base.effectColor, num5, list.Count, CustomOutLine.distance.x * this.distanceScale + base.effectDistance.x * this.effectscale * 1f, CustomOutLine.distance.y * this.distanceScale + base.effectDistance.y * this.effectscale * num7);
				num5 = num6;
				num6 = list.Count;
			}
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
			CustomOutLine.ListPool<UIVertex>.Release(list);
		}

		private float effectscale = 3f;

		private float distanceScale = 4f;

		private const float xScale = 1f;

		private const float DownHeight = 3.5f;

		private const float DownDensity = 2f;

		private static Vector2 distance = Vector2.zero;

		private Text m_Text;

		private static class ListPool<T>
		{
			private static void Clear(List<T> l)
			{
				l.Clear();
			}

			public static List<T> Get()
			{
				return CustomOutLine.ListPool<T>.s_ListPool.Get();
			}

			public static void Release(List<T> toRelease)
			{
				CustomOutLine.ListPool<T>.s_ListPool.Release(toRelease);
			}

			private static readonly CustomOutLine.ObjectPool<List<T>> s_ListPool = new CustomOutLine.ObjectPool<List<T>>(null, new UnityAction<List<T>>(CustomOutLine.ListPool<T>.Clear));
		}

		private class ObjectPool<T> where T : new()
		{
			public int countAll { get; private set; }

			public int countActive
			{
				get
				{
					return this.countAll - this.countInactive;
				}
			}

			public int countInactive
			{
				get
				{
					return this.m_Stack.Count;
				}
			}

			public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
			{
				this.m_ActionOnGet = actionOnGet;
				this.m_ActionOnRelease = actionOnRelease;
			}

			public T Get()
			{
				T t;
				if (this.m_Stack.Count == 0)
				{
					t = new T();
					int countAll = this.countAll;
					this.countAll = countAll + 1;
				}
				else
				{
					t = this.m_Stack.Pop();
				}
				if (this.m_ActionOnGet != null)
				{
					this.m_ActionOnGet.Invoke(t);
				}
				return t;
			}

			public void Release(T element)
			{
				if (this.m_Stack.Count > 0 && this.m_Stack.Peek() == element)
				{
					HLog.LogError("Internal error. Trying to destroy object that is already released to pool.");
				}
				if (this.m_ActionOnRelease != null)
				{
					this.m_ActionOnRelease.Invoke(element);
				}
				this.m_Stack.Push(element);
			}

			private readonly Stack<T> m_Stack = new Stack<T>();

			private readonly UnityAction<T> m_ActionOnGet;

			private readonly UnityAction<T> m_ActionOnRelease;
		}
	}
}
