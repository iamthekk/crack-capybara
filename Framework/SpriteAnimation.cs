using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.Reset();
	}

	private void Update()
	{
		if (this.isPause)
		{
			return;
		}
		if (this.sprite == null || this.spriteList.Count <= 1)
		{
			return;
		}
		this.currentTime += Time.deltaTime;
		if (this.currentTime - this.startTime >= this.interval)
		{
			this.index++;
			if (this.index >= this.spriteList.Count)
			{
				this.index = 0;
				Action action = this.onAniFinish;
				if (action != null)
				{
					action();
				}
			}
			this.sprite.sprite = this.spriteList[this.index];
			this.startTime = Time.time;
			this.currentTime = this.startTime;
		}
	}

	public void Reset()
	{
		this.index = 0;
		if (this.sprite != null && this.spriteList.Count > 0)
		{
			this.sprite.sprite = this.spriteList[this.index];
			this.startTime = Time.time;
			this.currentTime = this.startTime;
		}
	}

	public void SetFinishAction(Action onFinish)
	{
		this.onAniFinish = onFinish;
	}

	public void SetPause(bool pause)
	{
		this.isPause = pause;
	}

	public List<Sprite> spriteList;

	public float interval = 0.1f;

	private SpriteRenderer sprite;

	private int index;

	private float startTime;

	private float currentTime;

	private Action onAniFinish;

	private bool isPause;
}
