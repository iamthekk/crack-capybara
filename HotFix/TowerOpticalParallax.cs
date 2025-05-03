using System;
using System.Collections.Generic;
using Framework.Logic;
using UnityEngine;

public class TowerOpticalParallax : MonoBehaviour
{
	private void Start()
	{
		this.lastPosition = this.followTarget.anchoredPosition;
		foreach (RectTransform rectTransform in this.controlTargets)
		{
			this.controlTargetsInitPos.Add(rectTransform.anchoredPosition);
		}
	}

	private void Update()
	{
		Vector2 anchoredPosition = this.followTarget.anchoredPosition;
		Vector2 vector = anchoredPosition - this.lastPosition;
		this.lastPosition = anchoredPosition;
		foreach (RectTransform rectTransform in this.controlTargets)
		{
			Utility.UI.MoveUIInScreen(rectTransform, vector * this.parallax, this.loopPosPadding);
		}
	}

	public void ResetPosition()
	{
		if (this.controlTargetsInitPos.Count != this.controlTargets.Count)
		{
			return;
		}
		for (int i = 0; i < this.controlTargets.Count; i++)
		{
			this.controlTargets[i].anchoredPosition = this.controlTargetsInitPos[i];
		}
		this.lastPosition = this.followTarget.anchoredPosition;
	}

	[SerializeField]
	private RectTransform followTarget;

	[Range(0f, 1f)]
	[SerializeField]
	private float parallax = 0.8f;

	[SerializeField]
	private List<RectTransform> controlTargets = new List<RectTransform>();

	[SerializeField]
	private Vector2 loopPosPadding = new Vector2(10f, 10f);

	private readonly List<Vector2> controlTargetsInitPos = new List<Vector2>();

	private Vector2 lastPosition;
}
