using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Logic;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollIntCtrl<T> : ScrollRectBase where T : Component
{
	public void SetShowWidthScale(float scale)
	{
		this.ShowWidth = (float)Screen.width * scale;
	}

	protected override void Awake()
	{
		base.Awake();
		this.offsetx = (base.transform as RectTransform).sizeDelta.x / 2f;
		this.BeginDrag = new Action<PointerEventData>(this.OnDragBegin);
		this.Drag = new Action<PointerEventData>(this.OnDrags);
		this.EndDrag = new Action<PointerEventData>(this.OnDragEnd);
	}

	public void InitOnce()
	{
		this.mObjPool = LocalUnityObjctPool.Create(base.gameObject);
		this.mObjPool.CreateCache<T>(this.copyItem);
		this.itemWidth = (this.copyItem.transform as RectTransform).sizeDelta.x;
	}

	public void SetScale(float min, float max)
	{
		this.minScale = min;
		this.maxScale = max;
	}

	public void SetShowCount(int showCount)
	{
		this.showCount = showCount;
	}

	public void Init(int count)
	{
		if (!this.bInit)
		{
			this.InitOnce();
			this.bInit = true;
		}
		this.mCurrentIndex = 0;
		this.mObjPool.Collect<T>();
		base.horizontalNormalizedPosition = 0f;
		base.velocity = Vector2.zero;
		this.lastscrollpos = -1f;
		this.lastspeed = 0f;
		this.mList.Clear();
		this.count = count;
		this.allWidth = (float)(count - 1) * this.itemWidth;
		base.content.sizeDelta = new Vector2(this.allWidth + this.offsetx * 2f, base.content.sizeDelta.y);
		for (int i = 0; i < count; i++)
		{
			ScrollIntCtrl<T>.ScrollData scrollData = new ScrollIntCtrl<T>.ScrollData(i, default(T));
			scrollData.maxScale = this.maxScale;
			scrollData.minScale = this.minScale;
			this.mList.Add(scrollData);
			if (i < this.showCount)
			{
				T t = this.mObjPool.DeQueue<T>();
				if (this.OnUpdateOne != null)
				{
					this.OnUpdateOne(i, t);
				}
				this.UpdateOne(i, t);
			}
			else
			{
				this.UpdateOne(i, default(T));
			}
		}
		this.UpdateSize();
		if (this.mCurrentIndex < this.mList.Count && this.OnScrollEnd != null)
		{
			this.OnScrollEnd(this.mCurrentIndex, this.mList[this.mCurrentIndex].one);
		}
	}

	private void UpdateOne(int i, T one)
	{
		ScrollIntCtrl<T>.ScrollData scrollData = this.mList[i];
		scrollData.Refresh(i, one);
		if (one != null)
		{
			one.transform.SetParent(this.mScrollChild);
			one.transform.localPosition = Vector3.zero;
			one.transform.localEulerAngles = Vector3.zero;
			one.transform.localScale = Vector3.one;
			(one.transform as RectTransform).anchoredPosition = new Vector2((float)i * this.itemWidth + this.offsetx, 0f);
		}
		if (this.count > 1)
		{
			scrollData.normalize = (float)i / ((float)this.count - 1f);
			scrollData.normalize_range = this.itemWidth / (this.allWidth + 0f);
			return;
		}
		if (this.count == 1)
		{
			scrollData.normalize = 0f;
			scrollData.normalize_range = 0f;
		}
	}

	public void DeInit()
	{
		if (this.mObjPool != null)
		{
			this.mObjPool.Collect<T>();
		}
		this.isOnUpdate = false;
		if (this.seq != null)
		{
			TweenExtensions.Kill(this.seq, false);
		}
	}

	private void OnDragBegin(PointerEventData eventData)
	{
		if (this.OnBeginDragEvent != null)
		{
			this.OnBeginDragEvent();
		}
		this.isOnUpdate = false;
	}

	private void OnDrags(PointerEventData eventData)
	{
		this.UpdateSize();
	}

	private void OnDragEnd(PointerEventData eventData)
	{
		base.velocity *= this.Speed;
		this.isOnUpdate = true;
	}

	protected override void OnUpdate()
	{
		if (this.isOnUpdate)
		{
			this.UpdateScroll();
			this.lastspeed = base.velocity.x;
		}
	}

	private void UpdateScroll()
	{
		this.UpdateSize();
		this.UpdateInfinity();
	}

	private void UpdateSize()
	{
		this.lastscrollpos = Utility.Math.Abs(base.content.anchoredPosition.x / this.allWidth);
		this.lastscrollpos = Mathf.Clamp01(this.lastscrollpos);
		float num = 0f;
		int i = 0;
		int num2 = this.mList.Count;
		while (i < num2)
		{
			ScrollIntCtrl<T>.ScrollData scrollData = this.mList[i];
			if (Mathf.Abs((this.lastscrollpos - scrollData.normalize) * this.allWidth) > this.ShowWidth)
			{
				if (scrollData.one != null)
				{
					this.mObjPool.EnQueue<T>(scrollData.one.gameObject);
					scrollData.Miss();
				}
			}
			else if (scrollData.one == null)
			{
				this.UpdateOne(i, this.mObjPool.DeQueue<T>());
				if (this.OnUpdateOne != null)
				{
					this.OnUpdateOne(i, scrollData.one);
				}
			}
			float num3 = scrollData.UpdateScale(this.lastscrollpos);
			if (num < num3)
			{
				num = num3;
				this.mCurrentIndex = i;
			}
			i++;
		}
		if (this.mCurrentIndex < this.mList.Count)
		{
			this.mList[this.mCurrentIndex].SetFront();
			if (this.OnUpdateSize != null)
			{
				this.OnUpdateSize(this.mCurrentIndex, this.mList[this.mCurrentIndex].one);
			}
		}
	}

	private void UpdateInfinity()
	{
		if (Mathf.Abs(base.velocity.x - this.lastspeed) < 50f && this.mCurrentIndex < this.mList.Count)
		{
			float horizontalNormalizedPosition = base.horizontalNormalizedPosition;
			base.horizontalNormalizedPosition = this.mList[this.mCurrentIndex].normalize;
			float num = (base.horizontalNormalizedPosition - horizontalNormalizedPosition) * this.allWidth;
			this.mScrollChild.transform.localPosition = new Vector3(num, 0f, 0f);
			this.isOnUpdate = false;
			Tweener tweener = TweenSettingsExtensions.OnUpdate<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mScrollChild, (float)((num > 0f) ? (-10) : 10), 0.2f, false), new TweenCallback(this.UpdateSize));
			Tweener tweener2 = TweenSettingsExtensions.OnUpdate<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mScrollChild, 0f, 0.2f, false), new TweenCallback(this.UpdateSize));
			this.seq = TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(DOTween.Sequence(), tweener), tweener2);
			if (this.OnScrollEnd != null)
			{
				this.OnScrollEnd(this.mCurrentIndex, this.mList[this.mCurrentIndex].one);
			}
		}
	}

	public void GotoInt(int index, bool playanimation = false)
	{
		if (index >= this.mList.Count || index < 0)
		{
			return;
		}
		if (!playanimation)
		{
			this.mCurrentIndex = index;
			float num = -this.mList[index].normalize * this.allWidth;
			base.horizontalNormalizedPosition = num;
			this.lastscrollpos = -1f;
			base.content.localPosition = new Vector3(num, 0f, 0f);
			this.UpdateSize();
			if (this.OnScrollEnd != null)
			{
				this.OnScrollEnd(index, this.mList[index].one);
				return;
			}
		}
		else
		{
			this.mGotoIntIndex = index;
			float posx = -this.mList[this.mCurrentIndex].normalize * this.allWidth;
			base.content.localPosition = new Vector3(posx, 0f, 0f);
			float normalize = this.mList[this.mGotoIntIndex].normalize;
			float nextxx = -normalize * this.allWidth;
			Vector3 localPosition = base.content.localPosition;
			float starth = this.mList[this.mCurrentIndex].normalize;
			float endh = this.mList[this.mGotoIntIndex].normalize;
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnUpdate<Tweener>(ShortcutExtensions.DOLocalMoveX(base.content, nextxx, 0.5f, false), delegate
			{
				this.lastscrollpos = -1f;
				float num2 = Utility.Math.Abs((this.content.localPosition.x - posx) / (nextxx - posx));
				this.horizontalNormalizedPosition = (endh - starth) * num2 + starth;
				this.UpdateSize();
			}), 6), delegate
			{
				this.mCurrentIndex = this.mGotoIntIndex;
				if (this.OnScrollEnd != null)
				{
					this.OnScrollEnd(this.mCurrentIndex, this.mList[this.mCurrentIndex].one);
				}
			});
		}
	}

	public GameObject copyItem;

	public Transform mScrollChild;

	[Header("滚动加速系数")]
	public float Speed = 3f;

	public Action<int, T> OnUpdateOne;

	public Action<int, T> OnUpdateSize;

	public Action<int, T> OnScrollEnd;

	public Action OnBeginDragEvent;

	private float ShowWidth = (float)Screen.width;

	public float maxScale = 1.5f;

	public float minScale = 1f;

	private bool bInit;

	private int showCount = 10;

	private int count = 40;

	private float allWidth;

	private float itemWidth;

	private float offsetx;

	private float lastscrollpos;

	private float lastspeed;

	private int mCurrentIndex;

	private LocalUnityObjctPool mObjPool;

	private List<ScrollIntCtrl<T>.ScrollData> mList = new List<ScrollIntCtrl<T>.ScrollData>();

	private Sequence seq;

	private int mGotoIntIndex;

	private bool isOnUpdate;

	public class ScrollData
	{
		public ScrollData(int index, T one)
		{
			this.Refresh(index, one);
		}

		public void Refresh(int index, T one)
		{
			this.index = index;
			this.one = one;
			if (one)
			{
				this.transform = one.transform as RectTransform;
			}
		}

		public void Miss()
		{
			this.one = default(T);
			this.transform = null;
		}

		public float UpdateScale(float normalizepos)
		{
			if (!this.one || !this.transform)
			{
				return 0f;
			}
			if (this.normalize_range > 0f)
			{
				this.scale = Mathf.Abs(this.normalize - normalizepos) / this.normalize_range;
				this.scale = Mathf.Clamp01(this.scale);
			}
			else
			{
				this.scale = 0f;
			}
			this.scalex = this.maxScale - this.scale * (this.maxScale - this.minScale);
			if (this.transform.localScale.x == this.scalex)
			{
				return this.scalex;
			}
			this.transform.localScale = Vector3.one * this.scalex;
			return this.scalex;
		}

		public void SetFront()
		{
			this.transform.SetAsLastSibling();
		}

		public float maxScale;

		public float minScale;

		public T one;

		public RectTransform transform;

		public int index;

		public float normalize;

		public float normalize_range;

		private float scale;

		private float scalex;
	}
}
