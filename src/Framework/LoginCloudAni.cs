using System;
using UnityEngine;

public class LoginCloudAni : MonoBehaviour
{
	private void Start()
	{
		if (this.cloudTrans.Length != 0)
		{
			this.imageWidth = this.cloudTrans[0].rect.width;
		}
	}

	private void Update()
	{
		for (int i = 0; i < this.cloudTrans.Length; i++)
		{
			if (this.cloudTrans[i].anchoredPosition.x < -(this.imageWidth + this.cloudTrans[0].rect.width / 2f))
			{
				float num = float.MinValue;
				for (int j = 0; j < this.cloudTrans.Length; j++)
				{
					if (this.cloudTrans[j].anchoredPosition.x > num)
					{
						num = this.cloudTrans[j].anchoredPosition.x;
					}
				}
				this.cloudTrans[i].anchoredPosition = new Vector2(num + this.imageWidth, this.cloudTrans[i].anchoredPosition.y);
			}
			this.cloudTrans[i].anchoredPosition += Vector2.left * this.speed * Time.deltaTime;
		}
	}

	public RectTransform[] cloudTrans;

	public float speed = 100f;

	private float imageWidth;
}
