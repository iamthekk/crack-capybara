using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAreaMask : MonoBehaviour, ICanvasRaycastFilter
{
	public bool IsAreaOutOfView { get; private set; }

	private void Awake()
	{
		if (this.Image == null)
		{
			this.Image = base.GetComponent<Image>();
		}
		this.Image.material = new Material(Shader.Find("UI/UIGuideMask"));
		this.SetDefault();
	}

	private void SetDefault()
	{
		if (this.Image != null)
		{
			RectTransform rectTransform = this.Image.rectTransform;
			rectTransform.pivot = new Vector2(0f, 0f);
			RectTransform rectTransform2 = rectTransform.parent as RectTransform;
			if (rectTransform2 != null)
			{
				rectTransform2.pivot = new Vector2(0f, 0f);
				rectTransform.anchoredPosition = Vector2.zero;
			}
		}
		this.AlwaysCheckArea = true;
	}

	private void OnEnable()
	{
		if (this.TargetArea != null)
		{
			this.CalcClickRect();
		}
	}

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		Vector2 vector;
		vector..ctor(sp.x / (float)Screen.width, sp.y / (float)Screen.height);
		if (this.ViewRect.Contains(vector))
		{
			Input.GetKey(306);
			return false;
		}
		Input.GetKey(306);
		return true;
	}

	public void CalcClickRect()
	{
		if (this.TargetArea == null)
		{
			HLog.LogError("[ImageAreaMask]TargetArea can't be null.");
			return;
		}
		if (this.Image == null)
		{
			return;
		}
		Vector3 vector = this.TargetArea.localToWorldMatrix.MultiplyPoint(this.TargetArea.rect.center);
		this.mLastCenter = this.TargetArea.position;
		vector = this.Image.rectTransform.parent.worldToLocalMatrix.MultiplyPoint(vector);
		this.AreaRect.size = this.TargetArea.rect.size + new Vector2(10f, 10f);
		this.AreaRect.center = vector;
		float width = this.Image.rectTransform.rect.width;
		float num = 0f;
		float height = this.Image.rectTransform.rect.height;
		float num2 = 0f;
		Vector4 vector2;
		vector2.x = (this.AreaRect.xMin + num) / width;
		vector2.z = (this.AreaRect.xMax + num) / width;
		vector2.y = (this.AreaRect.yMin + num2) / height;
		vector2.w = (this.AreaRect.yMax + num2) / height;
		this.ViewRect.xMin = vector2.x;
		this.ViewRect.xMax = vector2.z;
		this.ViewRect.yMin = vector2.y;
		this.ViewRect.yMax = vector2.w;
		this.Image.material.SetVector("_AreaMask", vector2);
		bool flag = vector2.x < 1f && vector2.x < vector2.z && vector2.z > 0f && vector2.y < 1f && vector2.y < vector2.w && vector2.w > 0f;
		this.IsAreaOutOfView = !flag;
		if (!flag)
		{
			Action onAreaOutOfView = this.OnAreaOutOfView;
			if (onAreaOutOfView == null)
			{
				return;
			}
			onAreaOutOfView();
			return;
		}
		else
		{
			Action onTargetPosChange = this.OnTargetPosChange;
			if (onTargetPosChange == null)
			{
				return;
			}
			onTargetPosChange();
			return;
		}
	}

	private void Update()
	{
		if (this.AlwaysCheckArea && this.TargetArea != null)
		{
			if (!this.TargetArea.gameObject.activeInHierarchy)
			{
				if (this.OnHideInHierarchy != null)
				{
					this.OnHideInHierarchy();
				}
			}
			else
			{
				Vector3 vector = this.mLastCenter;
				if (Vector3.Distance(this.mLastCenter, this.TargetArea.position) > 0.005f)
				{
					this.CalcClickRect();
				}
			}
		}
		if (this.TargetArea == null && this.OnHideInHierarchy != null)
		{
			this.OnHideInHierarchy();
		}
	}

	public Image Image;

	public RectTransform TargetArea;

	public Rect AreaRect;

	[HideInInspector]
	public Rect ViewRect;

	[HideInInspector]
	public bool AlwaysCheckArea = true;

	private Vector3 mLastCenter = new Vector3(-1.23f, -1.12f, -1.11f);

	public Action OnAreaOutOfView;

	public Action OnHideInHierarchy;

	public Action OnTargetPosChange;
}
