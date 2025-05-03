using System;
using UnityEngine;
using UnityEngine.UI;

public class ImagePingPong : MonoBehaviour
{
	private void Start()
	{
		this.GoToChangeTarget = base.gameObject.GetComponent<Image>();
	}

	private void Update()
	{
		if (this.OnOff)
		{
			this.testValue = Mathf.PingPong(Time.time, this.time) / this.time;
			this.ChangeImageColor(this.GoToChangeTarget, this.startColor, this.endColor, this.testValue);
		}
	}

	private void ChangeImageColor(Image image, Color startColor, Color endColor, float lerpValue)
	{
		if (image == null)
		{
			return;
		}
		image.color = Color.Lerp(startColor, endColor, lerpValue);
	}

	[Header("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
	[SerializeField]
	public bool OnOff;

	[Header("\ufffd\ufffd\ufffdν\ufffd\ufffd\ufffd\ufffdʱ\ufffd䣬\ufffd\ufffdλ\ufffd\ufffd")]
	[SerializeField]
	public float time;

	[Header("\ufffd\ufffd\u05b5\ufffdĹ۲촰\ufffd\ufffd")]
	public float testValue;

	[Header("\ufffd\ufffd\ufffd\ufffdɫ\ufffd\ufffdImage")]
	public Image GoToChangeTarget;

	[Header("\ufffd\ufffdʼ\ufffd\ufffdɫ")]
	public Color startColor = new Color(1f, 1f, 1f, 1f);

	[Header("\ufffd\ufffd\u05b9\ufffd\ufffdɫ")]
	public Color endColor = new Color(1f, 1f, 1f, 0f);
}
