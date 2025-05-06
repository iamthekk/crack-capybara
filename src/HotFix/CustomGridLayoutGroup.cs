﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	[AddComponentMenu("Layout/CustomGridLayoutGroup")]
	public class CustomGridLayoutGroup : GridLayoutGroup
	{
		public override void SetLayoutHorizontal()
		{
			this.SetCellsAlongAxis(0);
		}

		public override void SetLayoutVertical()
		{
			this.SetCellsAlongAxis(1);
		}

		private void SetCellsAlongAxis(int axis)
		{
			int count = base.rectChildren.Count;
			if (axis == 0)
			{
				for (int i = 0; i < count; i++)
				{
					RectTransform rectTransform = base.rectChildren[i];
					this.m_Tracker.Add(this, rectTransform, 16134);
					rectTransform.anchorMin = Vector2.up;
					rectTransform.anchorMax = Vector2.up;
					rectTransform.sizeDelta = base.cellSize;
				}
				return;
			}
			float x = base.rectTransform.rect.size.x;
			float y = base.rectTransform.rect.size.y;
			int num = 1;
			int num2 = 1;
			if (this.m_Constraint == 1)
			{
				num = this.m_ConstraintCount;
				if (count > num)
				{
					num2 = count / num + ((count % num > 0) ? 1 : 0);
				}
			}
			else if (this.m_Constraint == 2)
			{
				num2 = this.m_ConstraintCount;
				if (count > num2)
				{
					num = count / num2 + ((count % num2 > 0) ? 1 : 0);
				}
			}
			else
			{
				if (base.cellSize.x + base.spacing.x <= 0f)
				{
					num = int.MaxValue;
				}
				else
				{
					num = Mathf.Max(1, Mathf.FloorToInt((x - (float)base.padding.horizontal + base.spacing.x + 0.001f) / (base.cellSize.x + base.spacing.x)));
				}
				if (base.cellSize.y + base.spacing.y <= 0f)
				{
					num2 = int.MaxValue;
				}
				else
				{
					num2 = Mathf.Max(1, Mathf.FloorToInt((y - (float)base.padding.vertical + base.spacing.y + 0.001f) / (base.cellSize.y + base.spacing.y)));
				}
			}
			int num3 = base.startCorner % 2;
			int num4 = base.startCorner / 2;
			int num5;
			int num6;
			int num7;
			if (base.startAxis == null)
			{
				num5 = num;
				num6 = Mathf.Clamp(num, 1, count);
				num7 = Mathf.Clamp(num2, 1, Mathf.CeilToInt((float)count / (float)num5));
			}
			else
			{
				num5 = num2;
				num7 = Mathf.Clamp(num2, 1, count);
				num6 = Mathf.Clamp(num, 1, Mathf.CeilToInt((float)count / (float)num5));
			}
			int num8 = count % num5;
			Vector2 vector;
			vector..ctor((float)num6 * base.cellSize.x + (float)(num6 - 1) * base.spacing.x, (float)num7 * base.cellSize.y + (float)(num7 - 1) * base.spacing.y);
			Vector2 vector2;
			vector2..ctor(base.GetStartOffset(0, vector.x), base.GetStartOffset(1, vector.y));
			int num9 = ((num8 == 0) ? num5 : num8);
			int num10 = ((base.startAxis == null) ? num9 : num6);
			int num11 = ((base.startAxis == 1) ? num9 : num7);
			Vector2 vector3;
			vector3..ctor((float)num10 * base.cellSize.x + (float)(num10 - 1) * base.spacing.x, (float)num11 * base.cellSize.y + (float)(num11 - 1) * base.spacing.y);
			Vector2 vector4;
			vector4..ctor(base.GetStartOffset(0, vector3.x), base.GetStartOffset(1, vector3.y));
			for (int j = 0; j < count; j++)
			{
				Vector2 vector5 = ((j + 1 > count - num9) ? vector4 : vector2);
				int num12;
				int num13;
				if (base.startAxis == null)
				{
					num12 = j % num5;
					num13 = j / num5;
				}
				else
				{
					num12 = j / num5;
					num13 = j % num5;
				}
				if (num3 == 1)
				{
					num12 = num6 - 1 - num12;
				}
				if (num4 == 1)
				{
					num13 = num7 - 1 - num13;
				}
				base.SetChildAlongAxis(base.rectChildren[j], 0, vector5.x + (base.cellSize[0] + base.spacing[0]) * (float)num12, base.cellSize[0]);
				base.SetChildAlongAxis(base.rectChildren[j], 1, vector5.y + (base.cellSize[1] + base.spacing[1]) * (float)num13, base.cellSize[1]);
			}
		}
	}
}
