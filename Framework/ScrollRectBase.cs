using System;
using Framework.Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectBase : ScrollRect, IPointerClickHandler, IEventSystemHandler
{
	public bool UseDrag
	{
		get
		{
			return !this.DragDisableForce && this._usegrag;
		}
		set
		{
			this._usegrag = value;
			if (!value)
			{
				this.bDragging = false;
			}
		}
	}

	private bool bDragging
	{
		get
		{
			return this._dragging;
		}
		set
		{
			this._dragging = value;
			if (this._dragging)
			{
				this.bSendFinish = false;
			}
		}
	}

	private bool bSendFinish
	{
		get
		{
			return this._sendfinish;
		}
		set
		{
			this._sendfinish = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		base.onValueChanged = new ScrollRect.ScrollRectEvent();
		base.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChanged));
	}

	public void RemoveAllListeners()
	{
		if (base.onValueChanged != null)
		{
			base.onValueChanged.RemoveAllListeners();
		}
	}

	protected override void OnEnable()
	{
		this.OnEnableWhole();
	}

	protected override void OnDisable()
	{
		this.OnDisableWhole();
	}

	private void Update()
	{
		if (this.UseWhole)
		{
			this.OnUpdateWhole();
		}
		this.OnUpdateGoto();
		this.OnUpdate();
	}

	protected virtual void OnUpdate()
	{
	}

	public void SetLocks(bool[] locks)
	{
		this.mLocks = locks;
	}

	private int GetNextUnlock(int currentindex, bool left)
	{
		if (this.mLocks != null)
		{
			int num = currentindex;
			int num2 = (left ? (-1) : 1);
			currentindex += num2;
			while (currentindex >= 0 && currentindex < this.mLocks.Length)
			{
				if (!this.mLocks[currentindex])
				{
					return currentindex;
				}
				currentindex += num2;
			}
			return num;
		}
		if (left)
		{
			return currentindex - 1;
		}
		return currentindex + 1;
	}

	private void OnUpdateGoto()
	{
		if (this.bGotoStart)
		{
			if (Utility.Math.Abs(this.mGotoTemp - this.mGotoValue) < 3f)
			{
				this.bGotoStart = false;
			}
			if (base.horizontal)
			{
				this.mGotoTemp = Mathf.Lerp(base.content.anchoredPosition.x, this.mGotoValue, 0.2f);
				base.content.anchoredPosition = new Vector2(this.mGotoTemp, base.content.anchoredPosition.y);
				return;
			}
			this.mGotoTemp = Mathf.Lerp(base.content.anchoredPosition.y, this.mGotoValue, 0.2f);
			base.content.anchoredPosition = new Vector2(base.content.anchoredPosition.x, this.mGotoTemp);
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (!this.bUseScrollEvent)
		{
			return;
		}
		if (!this.UseDrag)
		{
			return;
		}
		this.OnBeginDragInternal(eventData);
	}

	public void OnBeginDragInternal(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		if (this.UseWhole)
		{
			this.OnBeginDragWhole(eventData);
		}
		if (this.BeginDrag != null)
		{
			this.BeginDrag(eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (!this.bUseScrollEvent)
		{
			return;
		}
		if (!this.UseDrag)
		{
			return;
		}
		this.OnDragInternal(eventData);
	}

	public void OnDragInternal(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		if (this.UseWhole)
		{
			this.OnDragWhole(eventData);
		}
		if (this.Drag != null)
		{
			this.Drag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (!this.bUseScrollEvent)
		{
			return;
		}
		if (!this.UseDrag)
		{
			return;
		}
		this.OnEndDragInternal(eventData);
	}

	public void OnEndDragInternal(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		if (this.UseWhole)
		{
			this.OnEndDragWhole(eventData);
		}
		if (this.EndDrag != null)
		{
			this.EndDrag(eventData);
		}
	}

	private void OnValueChanged(Vector2 value)
	{
		this.scrollpercent = value.x;
		if (this.ValueChanged != null)
		{
			this.ValueChanged(value);
		}
	}

	public void Goto(float value, bool playanimation = false)
	{
		if (playanimation)
		{
			if (base.horizontal)
			{
				this.mGotoTemp = base.content.anchoredPosition.x;
			}
			else
			{
				this.mGotoTemp = base.content.anchoredPosition.y;
			}
			this.mGotoValue = value;
			this.bGotoStart = true;
			return;
		}
		if (base.horizontal)
		{
			base.content.anchoredPosition = new Vector2(value, base.content.anchoredPosition.y);
			this.mGotoTemp = base.content.anchoredPosition.x;
			return;
		}
		base.content.anchoredPosition = new Vector2(base.content.anchoredPosition.x, value);
		this.mGotoTemp = base.content.anchoredPosition.y;
	}

	private void OnEnableWhole()
	{
	}

	private void OnDisableWhole()
	{
	}

	public void SetWhole(GridLayoutGroup grid, int count)
	{
		this.UseWhole = true;
		if (base.horizontal)
		{
			this.Whole_PerOne = grid.cellSize.x;
		}
		else
		{
			this.Whole_PerOne = grid.cellSize.y;
		}
		this.Whole_Count = count;
		this.AllWidth = this.Whole_PerOne * (float)(this.Whole_Count - 1);
		base.content.sizeDelta = new Vector2(this.AllWidth, 0f);
	}

	public void SetPage(int page, bool animate, Action onFinish = null)
	{
		this.mPageAniFinish = onFinish;
		this.currentPage = page;
		if (!animate)
		{
			if (base.horizontal)
			{
				this.UpdateScrollEndPos();
				base.horizontalNormalizedPosition = this.scrollendpos;
			}
			if (base.vertical)
			{
				this.UpdateScrollEndPos();
				base.verticalNormalizedPosition = this.scrollendpos;
			}
		}
		else
		{
			this.bSendFinish = false;
			this.bUpdateEnd = false;
		}
		this.UpdateScrollEndPos();
	}

	private void OnBeginDragWhole(PointerEventData eventData)
	{
		this.bDragging = true;
	}

	private void OnDragWhole(PointerEventData eventData)
	{
		this.speed = eventData.delta.x;
	}

	private void OnEndDragWhole(PointerEventData eventData)
	{
		int num = this.currentPage;
		if (Mathf.Abs(this.speed) < this.SpeedCritical)
		{
			int page = this.GetPage();
			if ((this.mLocks != null && page >= 0 && page < this.mLocks.Length && !this.mLocks[page]) || this.mLocks == null)
			{
				this.currentPage = page;
			}
		}
		else
		{
			if (this.speed > 0f)
			{
				this.currentPage = this.GetNextUnlock(this.currentPage, true);
			}
			else
			{
				this.currentPage = this.GetNextUnlock(this.currentPage, false);
			}
			this.currentPage = Mathf.Clamp(this.currentPage, 0, this.Whole_Count - 1);
		}
		this.bUpdateEnd = false;
		this.bDragging = false;
		this.speed = 0f;
		this.UpdateScrollEndPos();
		if (this.EndDragItem != null)
		{
			this.EndDragItem(this.currentPage);
		}
	}

	private void UpdateScrollEndPos()
	{
		this.scrollendpos = this.Whole_PerOne * (float)this.currentPage / this.AllWidth;
	}

	private void OnUpdateWhole()
	{
		if (!this.bDragging && !this.bSendFinish)
		{
			if (base.horizontal)
			{
				base.horizontalNormalizedPosition = Mathf.Lerp(base.horizontalNormalizedPosition, this.scrollendpos, 7f * Time.deltaTime);
				if (Mathf.Abs(base.horizontalNormalizedPosition - this.scrollendpos) * base.content.sizeDelta.x < 2f)
				{
					base.horizontalNormalizedPosition = this.scrollendpos;
					this.bUpdateEnd = true;
				}
			}
			if (base.vertical)
			{
				base.verticalNormalizedPosition = Mathf.Lerp(base.verticalNormalizedPosition, this.scrollendpos, 7f * Time.deltaTime);
				if (Mathf.Abs(base.verticalNormalizedPosition - this.scrollendpos) * base.content.sizeDelta.y < 2f)
				{
					base.verticalNormalizedPosition = this.scrollendpos;
					this.bUpdateEnd = true;
				}
			}
			if (this.bUpdateEnd)
			{
				if (this.mPageAniFinish != null)
				{
					this.mPageAniFinish();
				}
				this.bSendFinish = true;
			}
		}
	}

	private int GetPage()
	{
		return Mathf.Clamp((int)(this.scrollpercent * (float)this.Whole_Count), 0, this.Whole_Count - 1);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.OnClick != null)
		{
			this.OnClick();
		}
	}

	public Action<PointerEventData> BeginDrag;

	public Action<PointerEventData> Drag;

	public Action<PointerEventData> EndDrag;

	public Action OnClick;

	public Action<int> EndDragItem;

	public Action<Vector2> ValueChanged;

	private float scrollpercent;

	public bool UseWhole;

	private bool _usegrag = true;

	public bool DragDisableForce;

	public bool bUseScrollEvent = true;

	public float SpeedCritical = 20f;

	public float Whole_PerOne;

	public int Whole_Count;

	public float AllWidth;

	private bool _dragging;

	private bool _sendfinish;

	private bool bUpdateEnd;

	private float speed;

	private int currentPage;

	private float scrollendpos;

	private Action mPageAniFinish;

	private bool[] mLocks;

	private bool bGotoStart;

	private float mGotoValue;

	private float mGotoTemp;
}
