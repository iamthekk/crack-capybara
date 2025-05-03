using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.U2D;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Sliced Filled Image", 11)]
public class SlicedFilledImage : MaskableGraphic, ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter
{
	public Sprite sprite
	{
		get
		{
			return this.m_Sprite;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
			{
				this.SetAllDirty();
				this.TrackImage();
			}
		}
	}

	public SlicedFilledImage.FillDirection fillDirection
	{
		get
		{
			return this.m_FillDirection;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetStruct<SlicedFilledImage.FillDirection>(ref this.m_FillDirection, value))
			{
				this.SetVerticesDirty();
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return this.m_FillAmount;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetStruct<float>(ref this.m_FillAmount, Mathf.Clamp01(value)))
			{
				this.SetVerticesDirty();
			}
		}
	}

	public bool fillCenter
	{
		get
		{
			return this.m_FillCenter;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetStruct<bool>(ref this.m_FillCenter, value))
			{
				this.SetVerticesDirty();
			}
		}
	}

	public float pixelsPerUnitMultiplier
	{
		get
		{
			return this.m_PixelsPerUnitMultiplier;
		}
		set
		{
			this.m_PixelsPerUnitMultiplier = Mathf.Max(0.01f, value);
		}
	}

	public float pixelsPerUnit
	{
		get
		{
			float num = 100f;
			if (this.activeSprite)
			{
				num = this.activeSprite.pixelsPerUnit;
			}
			float num2 = 100f;
			if (base.canvas)
			{
				num2 = base.canvas.referencePixelsPerUnit;
			}
			return this.m_PixelsPerUnitMultiplier * num / num2;
		}
	}

	public Sprite overrideSprite
	{
		get
		{
			return this.activeSprite;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
			{
				this.SetAllDirty();
				this.TrackImage();
			}
		}
	}

	private Sprite activeSprite
	{
		get
		{
			return this.m_Sprite;
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (this.activeSprite != null)
			{
				return this.activeSprite.texture;
			}
			if (!(this.material != null) || !(this.material.mainTexture != null))
			{
				return Graphic.s_WhiteTexture;
			}
			return this.material.mainTexture;
		}
	}

	public bool hasBorder
	{
		get
		{
			return this.activeSprite != null && this.activeSprite.border.sqrMagnitude > 0f;
		}
	}

	public override Material material
	{
		get
		{
			if (this.m_Material != null)
			{
				return this.m_Material;
			}
			if (this.activeSprite && this.activeSprite.associatedAlphaSplitTexture != null)
			{
				return Image.defaultETC1GraphicMaterial;
			}
			return this.defaultMaterial;
		}
		set
		{
			base.material = value;
		}
	}

	public float alphaHitTestMinimumThreshold { get; set; }

	protected SlicedFilledImage()
	{
		base.useLegacyMeshGeneration = false;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		this.TrackImage();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_Tracked)
		{
			this.UnTrackImage();
		}
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		if (this.activeSprite == null)
		{
			base.OnPopulateMesh(vh);
			return;
		}
		this.GenerateSlicedFilledSprite(vh);
	}

	protected override void UpdateMaterial()
	{
		base.UpdateMaterial();
		if (this.activeSprite == null)
		{
			base.canvasRenderer.SetAlphaTexture(null);
			return;
		}
		Texture2D associatedAlphaSplitTexture = this.activeSprite.associatedAlphaSplitTexture;
		if (associatedAlphaSplitTexture != null)
		{
			base.canvasRenderer.SetAlphaTexture(associatedAlphaSplitTexture);
		}
	}

	private void GenerateSlicedFilledSprite(VertexHelper vh)
	{
		vh.Clear();
		if (this.m_FillAmount < 0.001f)
		{
			return;
		}
		Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
		Rect rect = ((this.m_FillDirection != SlicedFilledImage.FillDirection.RightExtendWidth) ? pixelAdjustedRect : new Rect(pixelAdjustedRect.position, new Vector2(this.m_OriginalWidth * this.m_FillAmount, pixelAdjustedRect.height)));
		Vector4 outerUV = DataUtility.GetOuterUV(this.activeSprite);
		Vector4 vector = DataUtility.GetPadding(this.activeSprite);
		float num = ((this.m_FillDirection == SlicedFilledImage.FillDirection.RightExtendWidth) ? 1f : this.m_FillAmount);
		if (!this.hasBorder)
		{
			Vector2 size = this.activeSprite.rect.size;
			int num2 = Mathf.RoundToInt(size.x);
			int num3 = Mathf.RoundToInt(size.y);
			Vector4 vector2;
			vector2..ctor(rect.x + rect.width * (vector.x / (float)num2), rect.y + rect.height * (vector.y / (float)num3), rect.x + rect.width * (((float)num2 - vector.z) / (float)num2), rect.y + rect.height * (((float)num3 - vector.w) / (float)num3));
			this.GenerateFilledSprite(vh, vector2, outerUV, num);
			return;
		}
		Vector4 innerUV = DataUtility.GetInnerUV(this.activeSprite);
		Vector4 vector3 = Vector4.zero;
		if (this.m_FillDirection != SlicedFilledImage.FillDirection.RightExtendWidth)
		{
			vector3 = this.GetAdjustedBorders(this.activeSprite.border / this.pixelsPerUnit, rect);
		}
		else if (rect.width <= this.activeSprite.border.x)
		{
			vector3..ctor(rect.width, this.activeSprite.border.y, 0f, this.activeSprite.border.w);
		}
		else if (rect.width > this.activeSprite.border.x && rect.width <= this.activeSprite.border.x + this.activeSprite.border.z)
		{
			vector3..ctor(this.activeSprite.border.x, this.activeSprite.border.y, rect.width - this.activeSprite.border.x, this.activeSprite.border.w);
		}
		else
		{
			vector3 = this.activeSprite.border;
		}
		vector /= this.pixelsPerUnit;
		SlicedFilledImage.s_SlicedVertices[0] = new Vector2(vector.x, vector.y);
		SlicedFilledImage.s_SlicedVertices[3] = new Vector2(rect.width - vector.z, rect.height - vector.w);
		SlicedFilledImage.s_SlicedVertices[1].x = vector3.x;
		SlicedFilledImage.s_SlicedVertices[1].y = vector3.y;
		SlicedFilledImage.s_SlicedVertices[2].x = rect.width - vector3.z;
		SlicedFilledImage.s_SlicedVertices[2].y = rect.height - vector3.w;
		for (int i = 0; i < 4; i++)
		{
			Vector2[] array = SlicedFilledImage.s_SlicedVertices;
			int num4 = i;
			array[num4].x = array[num4].x + rect.x;
			Vector2[] array2 = SlicedFilledImage.s_SlicedVertices;
			int num5 = i;
			array2[num5].y = array2[num5].y + rect.y;
		}
		SlicedFilledImage.s_SlicedUVs[0] = new Vector2(outerUV.x, outerUV.y);
		SlicedFilledImage.s_SlicedUVs[1] = new Vector2(innerUV.x, innerUV.y);
		SlicedFilledImage.s_SlicedUVs[2] = new Vector2(innerUV.z, innerUV.w);
		SlicedFilledImage.s_SlicedUVs[3] = new Vector2(outerUV.z, outerUV.w);
		float num6;
		float num8;
		if (this.m_FillDirection == SlicedFilledImage.FillDirection.Left || this.m_FillDirection == SlicedFilledImage.FillDirection.Right || this.m_FillDirection == SlicedFilledImage.FillDirection.RightExtendWidth)
		{
			num6 = SlicedFilledImage.s_SlicedVertices[0].x;
			float num7 = SlicedFilledImage.s_SlicedVertices[3].x - SlicedFilledImage.s_SlicedVertices[0].x;
			num8 = ((num7 > 0f) ? (1f / num7) : 1f);
		}
		else
		{
			num6 = SlicedFilledImage.s_SlicedVertices[0].y;
			float num9 = SlicedFilledImage.s_SlicedVertices[3].y - SlicedFilledImage.s_SlicedVertices[0].y;
			num8 = ((num9 > 0f) ? (1f / num9) : 1f);
		}
		for (int j = 0; j < 3; j++)
		{
			int num10 = j + 1;
			for (int k = 0; k < 3; k++)
			{
				if (this.m_FillCenter || j != 1 || k != 1)
				{
					int num11 = k + 1;
					float num12;
					float num13;
					switch (this.m_FillDirection)
					{
					case SlicedFilledImage.FillDirection.Right:
					case SlicedFilledImage.FillDirection.RightExtendWidth:
						num12 = (SlicedFilledImage.s_SlicedVertices[j].x - num6) * num8;
						num13 = (SlicedFilledImage.s_SlicedVertices[num10].x - num6) * num8;
						break;
					case SlicedFilledImage.FillDirection.Left:
						num12 = 1f - (SlicedFilledImage.s_SlicedVertices[num10].x - num6) * num8;
						num13 = 1f - (SlicedFilledImage.s_SlicedVertices[j].x - num6) * num8;
						break;
					case SlicedFilledImage.FillDirection.Up:
						num12 = (SlicedFilledImage.s_SlicedVertices[k].y - num6) * num8;
						num13 = (SlicedFilledImage.s_SlicedVertices[num11].y - num6) * num8;
						break;
					case SlicedFilledImage.FillDirection.Down:
						num12 = 1f - (SlicedFilledImage.s_SlicedVertices[num11].y - num6) * num8;
						num13 = 1f - (SlicedFilledImage.s_SlicedVertices[k].y - num6) * num8;
						break;
					default:
						num13 = (num12 = 0f);
						break;
					}
					if (num12 < num)
					{
						Vector4 vector4;
						vector4..ctor(SlicedFilledImage.s_SlicedVertices[j].x, SlicedFilledImage.s_SlicedVertices[k].y, SlicedFilledImage.s_SlicedVertices[num10].x, SlicedFilledImage.s_SlicedVertices[num11].y);
						Vector4 vector5;
						vector5..ctor(SlicedFilledImage.s_SlicedUVs[j].x, SlicedFilledImage.s_SlicedUVs[k].y, SlicedFilledImage.s_SlicedUVs[num10].x, SlicedFilledImage.s_SlicedUVs[num11].y);
						float num14 = (num - num12) / (num13 - num12);
						this.GenerateFilledSprite(vh, vector4, vector5, num14);
					}
				}
			}
		}
	}

	private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
	{
		Rect rect = base.rectTransform.rect;
		for (int i = 0; i <= 1; i++)
		{
			if (rect.size[i] != 0f)
			{
				float num = adjustedRect.size[i] / rect.size[i];
				ref Vector4 ptr = ref border;
				int num2 = i;
				ptr[num2] *= num;
				ptr = ref border;
				num2 = i + 2;
				ptr[num2] *= num;
			}
			float num3 = border[i] + border[i + 2];
			if (adjustedRect.size[i] < num3 && num3 != 0f)
			{
				float num = adjustedRect.size[i] / num3;
				ref Vector4 ptr = ref border;
				int num2 = i;
				ptr[num2] *= num;
				ptr = ref border;
				num2 = i + 2;
				ptr[num2] *= num;
			}
		}
		return border;
	}

	private void GenerateFilledSprite(VertexHelper vh, Vector4 vertices, Vector4 uvs, float fillAmount)
	{
		if (this.m_FillAmount < 0.001f)
		{
			return;
		}
		float num = uvs.x;
		float num2 = uvs.y;
		float num3 = uvs.z;
		float num4 = uvs.w;
		if (fillAmount < 1f)
		{
			if (this.m_FillDirection == SlicedFilledImage.FillDirection.Left || this.m_FillDirection == SlicedFilledImage.FillDirection.Right || this.m_FillDirection == SlicedFilledImage.FillDirection.RightExtendWidth)
			{
				if (this.m_FillDirection == SlicedFilledImage.FillDirection.Left)
				{
					vertices.x = vertices.z - (vertices.z - vertices.x) * fillAmount;
					num = num3 - (num3 - num) * fillAmount;
				}
				else
				{
					vertices.z = vertices.x + (vertices.z - vertices.x) * fillAmount;
					num3 = num + (num3 - num) * fillAmount;
				}
			}
			else if (this.m_FillDirection == SlicedFilledImage.FillDirection.Down)
			{
				vertices.y = vertices.w - (vertices.w - vertices.y) * fillAmount;
				num2 = num4 - (num4 - num2) * fillAmount;
			}
			else
			{
				vertices.w = vertices.y + (vertices.w - vertices.y) * fillAmount;
				num4 = num2 + (num4 - num2) * fillAmount;
			}
		}
		SlicedFilledImage.s_Vertices[0] = new Vector3(vertices.x, vertices.y);
		SlicedFilledImage.s_Vertices[1] = new Vector3(vertices.x, vertices.w);
		SlicedFilledImage.s_Vertices[2] = new Vector3(vertices.z, vertices.w);
		SlicedFilledImage.s_Vertices[3] = new Vector3(vertices.z, vertices.y);
		SlicedFilledImage.s_UVs[0] = new Vector2(num, num2);
		SlicedFilledImage.s_UVs[1] = new Vector2(num, num4);
		SlicedFilledImage.s_UVs[2] = new Vector2(num3, num4);
		SlicedFilledImage.s_UVs[3] = new Vector2(num3, num2);
		int currentVertCount = vh.currentVertCount;
		for (int i = 0; i < 4; i++)
		{
			vh.AddVert(SlicedFilledImage.s_Vertices[i], this.color, SlicedFilledImage.s_UVs[i]);
		}
		vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
	}

	int ILayoutElement.layoutPriority
	{
		get
		{
			return 0;
		}
	}

	float ILayoutElement.minWidth
	{
		get
		{
			return 0f;
		}
	}

	float ILayoutElement.minHeight
	{
		get
		{
			return 0f;
		}
	}

	float ILayoutElement.flexibleWidth
	{
		get
		{
			return -1f;
		}
	}

	float ILayoutElement.flexibleHeight
	{
		get
		{
			return -1f;
		}
	}

	float ILayoutElement.preferredWidth
	{
		get
		{
			if (this.activeSprite == null)
			{
				return 0f;
			}
			return DataUtility.GetMinSize(this.activeSprite).x / this.pixelsPerUnit;
		}
	}

	float ILayoutElement.preferredHeight
	{
		get
		{
			if (this.activeSprite == null)
			{
				return 0f;
			}
			return DataUtility.GetMinSize(this.activeSprite).y / this.pixelsPerUnit;
		}
	}

	void ILayoutElement.CalculateLayoutInputHorizontal()
	{
	}

	void ILayoutElement.CalculateLayoutInputVertical()
	{
	}

	bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		if (this.alphaHitTestMinimumThreshold <= 0f)
		{
			return true;
		}
		if (this.alphaHitTestMinimumThreshold > 1f)
		{
			return false;
		}
		if (this.activeSprite == null)
		{
			return true;
		}
		Vector2 vector;
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, ref vector))
		{
			return false;
		}
		Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
		vector.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
		vector.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
		Rect rect = this.activeSprite.rect;
		Vector4 border = this.activeSprite.border;
		Vector4 adjustedBorders = this.GetAdjustedBorders(border / this.pixelsPerUnit, pixelAdjustedRect);
		for (int i = 0; i < 2; i++)
		{
			if (vector[i] > adjustedBorders[i])
			{
				if (pixelAdjustedRect.size[i] - vector[i] <= adjustedBorders[i + 2])
				{
					ref Vector2 ptr = ref vector;
					int num = i;
					ptr[num] -= pixelAdjustedRect.size[i] - rect.size[i];
				}
				else
				{
					float num2 = Mathf.InverseLerp(adjustedBorders[i], pixelAdjustedRect.size[i] - adjustedBorders[i + 2], vector[i]);
					vector[i] = Mathf.Lerp(border[i], rect.size[i] - border[i + 2], num2);
				}
			}
		}
		Rect textureRect = this.activeSprite.textureRect;
		Vector2 vector2;
		vector2..ctor(vector.x / textureRect.width, vector.y / textureRect.height);
		float num3 = Mathf.Lerp(textureRect.x, textureRect.xMax, vector2.x) / (float)this.activeSprite.texture.width;
		float num4 = Mathf.Lerp(textureRect.y, textureRect.yMax, vector2.y) / (float)this.activeSprite.texture.height;
		switch (this.m_FillDirection)
		{
		case SlicedFilledImage.FillDirection.Right:
			if (num3 > this.m_FillAmount)
			{
				return false;
			}
			break;
		case SlicedFilledImage.FillDirection.Left:
			if (1f - num3 > this.m_FillAmount)
			{
				return false;
			}
			break;
		case SlicedFilledImage.FillDirection.Up:
			if (num4 > this.m_FillAmount)
			{
				return false;
			}
			break;
		case SlicedFilledImage.FillDirection.Down:
			if (1f - num4 > this.m_FillAmount)
			{
				return false;
			}
			break;
		}
		bool flag;
		try
		{
			flag = this.activeSprite.texture.GetPixelBilinear(num3, num4).a >= this.alphaHitTestMinimumThreshold;
		}
		catch (UnityException ex)
		{
			HLog.LogException(ex);
			flag = true;
		}
		return flag;
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		this.m_FillAmount = Mathf.Clamp01(this.m_FillAmount);
	}

	private void TrackImage()
	{
		if (this.activeSprite != null && this.activeSprite.texture == null)
		{
			if (!SlicedFilledImage.s_Initialized)
			{
				SpriteAtlasManager.atlasRegistered += SlicedFilledImage.RebuildImage;
				SlicedFilledImage.s_Initialized = true;
			}
			SlicedFilledImage.m_TrackedTexturelessImages.Add(this);
			this.m_Tracked = true;
		}
	}

	private void UnTrackImage()
	{
		SlicedFilledImage.m_TrackedTexturelessImages.Remove(this);
		this.m_Tracked = false;
	}

	private static void RebuildImage(SpriteAtlas spriteAtlas)
	{
		for (int i = SlicedFilledImage.m_TrackedTexturelessImages.Count - 1; i >= 0; i--)
		{
			SlicedFilledImage slicedFilledImage = SlicedFilledImage.m_TrackedTexturelessImages[i];
			if (spriteAtlas.CanBindTo(slicedFilledImage.activeSprite))
			{
				slicedFilledImage.SetAllDirty();
				SlicedFilledImage.m_TrackedTexturelessImages.RemoveAt(i);
			}
		}
	}

	private static readonly Vector3[] s_Vertices = new Vector3[4];

	private static readonly Vector2[] s_UVs = new Vector2[4];

	private static readonly Vector2[] s_SlicedVertices = new Vector2[4];

	private static readonly Vector2[] s_SlicedUVs = new Vector2[4];

	[SerializeField]
	private Sprite m_Sprite;

	[SerializeField]
	private SlicedFilledImage.FillDirection m_FillDirection;

	[Range(0f, 1f)]
	[SerializeField]
	private float m_FillAmount = 1f;

	[SerializeField]
	private bool m_FillCenter = true;

	[SerializeField]
	private float m_PixelsPerUnitMultiplier = 1f;

	[SerializeField]
	private float m_OriginalWidth;

	[NonSerialized]
	private Sprite m_OverrideSprite;

	private bool m_Tracked;

	private static List<SlicedFilledImage> m_TrackedTexturelessImages = new List<SlicedFilledImage>();

	private static bool s_Initialized;

	private static class SetPropertyUtility
	{
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}
	}

	public enum FillDirection
	{
		Right,
		Left,
		Up,
		Down,
		RightExtendWidth
	}
}
