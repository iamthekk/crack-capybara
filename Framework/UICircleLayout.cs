using System;
using UnityEngine;

public class UICircleLayout : MonoBehaviour
{
	[ContextMenu("自动排列圆形")]
	public void SetCycleLayout()
	{
		RectTransform component = base.GetComponent<RectTransform>();
		float num = (float)(360 / base.transform.childCount);
		for (int i = 0; i < base.transform.childCount; i++)
		{
			float num2 = ((float)this.StartAngle - (float)i * num) * 0.0174532924f;
			float num3 = (float)this.Radius * Mathf.Cos(num2);
			float num4 = (float)this.Radius * Mathf.Sin(num2);
			Vector2 vector;
			vector..ctor(num3, num4);
			Transform child = component.GetChild(i);
			child.localPosition = vector;
			if (this.IsAutoRotateSelfToCenter)
			{
				Vector2 vector2 = -vector;
				Vector2 down = Vector2.down;
				float num5 = Vector2.SignedAngle(vector2, down);
				child.localRotation = Quaternion.Euler(0f, 0f, -num5);
			}
		}
	}

	private void Update()
	{
		if (this.IsRotateLayout)
		{
			base.transform.Rotate(Vector3.forward, this.RotationSpeed * Time.deltaTime);
		}
	}

	[Tooltip("圆形半径")]
	[Header("点击组件右上角自动排列圆形")]
	public int Radius = 300;

	[Tooltip("圆的正上方为第一个Item,圆的90度位置，逆时针排列")]
	public int StartAngle = 90;

	[Tooltip("自动旋转item指向中心点")]
	public bool IsAutoRotateSelfToCenter = true;

	[Tooltip("是否旋转组件")]
	public bool IsRotateLayout;

	[Tooltip("旋转速度（度/秒）")]
	public float RotationSpeed = 30f;
}
