using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class CustomTextHelper : BaseMeshEffect
	{
		public float lineHeight
		{
			get
			{
				return this._lineHeight;
			}
			set
			{
				this._lineHeight = value;
				this._lineHeightHalf = value * 0.5f;
			}
		}

		protected override void Awake()
		{
			this.text = base.GetComponent<Text>();
			if (this.text == null)
			{
				return;
			}
			this.text.RegisterDirtyMaterialCallback(new UnityAction(this.OnFontMaterialChanged));
		}

		private void OnFontMaterialChanged()
		{
			this.text.font.RequestCharactersInTexture("*", this.text.fontSize, this.text.fontStyle);
		}

		protected override void OnDestroy()
		{
			this.text.UnregisterDirtyMaterialCallback(new UnityAction(this.OnFontMaterialChanged));
			base.OnDestroy();
		}

		private Vector2 GetUnderlineCharUV()
		{
			CharacterInfo characterInfo;
			if (this.text.font.GetCharacterInfo('*', ref characterInfo, this.text.fontSize, this.text.fontStyle))
			{
				return (characterInfo.uvBottomLeft + characterInfo.uvBottomRight + characterInfo.uvTopLeft + characterInfo.uvTopRight) * 0.25f;
			}
			return Vector2.zero;
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive() || (!this.useUnderline && !this.useAlign && !this.useGradientColor))
			{
				return;
			}
			if (this.text.rectTransform.rect.size.x <= 0f || this.text.rectTransform.rect.size.y <= 0f)
			{
				return;
			}
			this.characters = this.text.cachedTextGenerator.GetCharactersArray();
			this.lines = this.text.cachedTextGenerator.GetLinesArray();
			if (this.useUnderline && this.ignoreBreakSign)
			{
				this.textChars = this.text.text.ToCharArray();
			}
			this.characterCountVisible = this.text.cachedTextGenerator.characterCountVisible;
			this.stream = new List<UIVertex>();
			vh.GetUIVertexStream(this.stream);
			vh.Clear();
			if (this.useAlign || this.useGradientColor)
			{
				int num = 0;
				for (int i = 0; i < this.stream.Count; i += 6)
				{
					int num2 = i / 6;
					float num3 = 0f;
					float num4 = 0f;
					if (this.useAlign)
					{
						int charInLineIndex = this.GetCharInLineIndex(num2);
						if (this.lines[charInLineIndex].startCharIdx == num2)
						{
							num = 0;
						}
						num3 = this.wordSpace * (float)num;
						num4 = this.lineSpace * (float)charInLineIndex;
						num++;
					}
					for (int j = 0; j < 6; j++)
					{
						if (this.useGradientColor)
						{
							this.SetGradientColors(i, j);
						}
						if (this.useAlign)
						{
							this.DoAlign(i, j, num3, num4);
						}
					}
				}
			}
			if (this.useUnderline)
			{
				if (!this.useCustomLineIndexArray)
				{
					this.DrawAllLinesLine(vh);
				}
				else
				{
					this.DrawCustomLine(vh);
				}
			}
			vh.AddUIVertexTriangleStream(this.stream);
		}

		private void SetGradientColors(int i, int j)
		{
			if (this.gradientColors == null)
			{
				this.gradientColors = new Color[6];
			}
			this.gradientColors[0] = this.gradientColor1;
			this.gradientColors[1] = this.gradientColor2;
			this.gradientColors[2] = this.gradientColor4;
			this.gradientColors[3] = this.gradientColor4;
			this.gradientColors[4] = this.gradientColor3;
			this.gradientColors[5] = this.gradientColor1;
			UIVertex uivertex = this.stream[i + j];
			uivertex.color = this.gradientColors[j];
			this.stream[i + j] = uivertex;
		}

		private void DoAlign(int i, int j, float offx, float offy)
		{
			UIVertex uivertex = this.stream[i + j];
			Vector3 position = uivertex.position;
			position.x += offx;
			position.y += offy;
			uivertex.position = position;
			this.stream[i + j] = uivertex;
		}

		private int GetCharInLineIndex(int charIndex)
		{
			int num = this.lines.Length - 1;
			for (int i = 0; i < num; i++)
			{
				UILineInfo uilineInfo = this.lines[i];
				if (charIndex >= uilineInfo.startCharIdx && charIndex < this.lines[i + 1].startCharIdx)
				{
					return i;
				}
			}
			if (charIndex >= this.lines[num].startCharIdx && charIndex < this.characters.Length)
			{
				return num;
			}
			return -1;
		}

		private void DrawAllLinesLine(VertexHelper vh)
		{
			Vector2 underlineCharUV = this.GetUnderlineCharUV();
			int i = 0;
			while (i < this.lines.Length)
			{
				UILineInfo uilineInfo = this.lines[i];
				int num;
				if (i + 1 < this.lines.Length)
				{
					int startCharIdx = this.lines[i + 1].startCharIdx;
					if (startCharIdx != this.lines[i].startCharIdx)
					{
						num = startCharIdx - 1;
						goto IL_006C;
					}
				}
				else if (this.characterCountVisible != 0)
				{
					num = this.characterCountVisible - 1;
					goto IL_006C;
				}
				IL_0098:
				i++;
				continue;
				IL_006C:
				float lineBottomY = this.GetLineBottomY(i);
				float num2 = (float)uilineInfo.startCharIdx * this.wordSpace;
				this.AddUnderlineVertTriangle(vh, uilineInfo.startCharIdx, num, num2, lineBottomY, underlineCharUV);
				goto IL_0098;
			}
		}

		private void DrawCustomLine(VertexHelper vh)
		{
			Vector2 underlineCharUV = this.GetUnderlineCharUV();
			int num = this.characterCountVisible - 1;
			this.text.text.ToCharArray();
			for (int i = 0; i < this.customLineIndexArray.Length; i++)
			{
				Vector2Int vector2Int = this.customLineIndexArray[i];
				int num2 = vector2Int[0];
				int num3 = vector2Int[1];
				if (num3 < num2)
				{
					num2 = vector2Int[1];
					num3 = vector2Int[0];
				}
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (num3 > num)
				{
					num3 = num;
				}
				if (num2 < this.characterCountVisible)
				{
					int charInLineIndex = this.GetCharInLineIndex(num2);
					int charInLineIndex2 = this.GetCharInLineIndex(num3);
					if (charInLineIndex != charInLineIndex2)
					{
						for (int j = charInLineIndex; j <= charInLineIndex2; j++)
						{
							float lineBottomY = this.GetLineBottomY(j);
							int num4 = num2;
							int num5 = num3;
							if (j == charInLineIndex)
							{
								num5 = this.lines[j + 1].startCharIdx - 1;
							}
							else if (j == charInLineIndex2)
							{
								num4 = this.lines[j].startCharIdx;
							}
							else
							{
								num4 = this.lines[j].startCharIdx;
								num5 = this.lines[j + 1].startCharIdx - 1;
							}
							float num6 = (float)this.lines[j].startCharIdx * this.wordSpace;
							this.AddUnderlineVertTriangle(vh, num4, num5, num6, lineBottomY, underlineCharUV);
						}
					}
					else
					{
						float lineBottomY2 = this.GetLineBottomY(charInLineIndex);
						float num7 = (float)this.lines[charInLineIndex].startCharIdx * this.wordSpace;
						this.AddUnderlineVertTriangle(vh, num2, num3, num7, lineBottomY2, underlineCharUV);
					}
				}
			}
		}

		private float GetLineBottomY(int lineIndex)
		{
			UILineInfo uilineInfo = this.lines[lineIndex];
			float num = uilineInfo.topY - (this.lineAlignToMiddle ? ((float)uilineInfo.height * 0.5f) : ((float)uilineInfo.height)) - this.lineOffset;
			num /= this.text.pixelsPerUnit;
			if (this.useAlign)
			{
				num += (float)lineIndex * this.lineSpace;
			}
			return num;
		}

		private Vector2 GetCharCursorPos(int charIdx, float firstCharOff)
		{
			Vector2 vector = this.characters[charIdx].cursorPos;
			vector /= this.text.pixelsPerUnit;
			Transform transform = base.transform;
			if (this.useAlign)
			{
				vector.x += this.wordSpace * (float)charIdx - firstCharOff;
			}
			return vector;
		}

		private void AddUnderlineVertTriangle(VertexHelper vh, int startIndex, int endIndex, float firstCharOff, float bottomY, Vector2 uv0)
		{
			if (this.ignoreBreakSign && this.textChars[endIndex] == '\n')
			{
				endIndex--;
			}
			if (endIndex < startIndex)
			{
				return;
			}
			Vector3 vector;
			vector..ctor(this.GetCharCursorPos(startIndex, firstCharOff).x, bottomY + (this.lineHeightJustify ? this._lineHeightHalf : 0f), 0f);
			Vector3 vector2;
			vector2..ctor(vector.x, vector.y - this.lineHeight, 0f);
			Vector3 vector3;
			vector3..ctor(this.GetCharCursorPos(endIndex, firstCharOff).x + this.characters[endIndex].charWidth / this.text.pixelsPerUnit, vector2.y, 0f);
			Vector3 vector4;
			vector4..ctor(vector3.x, vector.y, 0f);
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = this.lineColor;
			simpleVert.uv0 = uv0;
			simpleVert.position = vector;
			this.underlineUIVertexs[0] = simpleVert;
			this.underlineUIVertexs[3] = simpleVert;
			simpleVert.position = vector2;
			this.underlineUIVertexs[5] = simpleVert;
			simpleVert.position = vector3;
			this.underlineUIVertexs[2] = simpleVert;
			this.underlineUIVertexs[4] = simpleVert;
			simpleVert.position = vector4;
			this.underlineUIVertexs[1] = simpleVert;
			this.stream.AddRange(this.underlineUIVertexs);
		}

		[Header("是否使用渐变色")]
		public bool useGradientColor;

		[Header("左上颜色,右上颜色,左下颜色,右下颜色")]
		public Color32 gradientColor1 = Color.white;

		public Color32 gradientColor2 = Color.white;

		public Color32 gradientColor3 = Color.white;

		public Color32 gradientColor4 = Color.white;

		[Header("是否开启对齐")]
		public bool useAlign;

		[Header("字间距")]
		public float wordSpace;

		[Header("行间距")]
		public float lineSpace;

		[Header("是否显示下划线")]
		public bool useUnderline;

		[Header("下划线是否忽略换行符")]
		public bool ignoreBreakSign = true;

		[Header("下划线宽度")]
		[SerializeField]
		[Range(0f, 100f)]
		private float _lineHeight = 1.5f;

		private float _lineHeightHalf = 0.75f;

		[Header("行高是否两端扩展,否则向下扩展")]
		public bool lineHeightJustify = true;

		public float lineOffset;

		public Color32 lineColor = Color.white;

		[Header("横线对齐到行的上下中间")]
		public bool lineAlignToMiddle;

		[Header("是否使用customLineIndexArray来作为显示线的起止依据,否则是全文字段显示")]
		public bool useCustomLineIndexArray;

		public Vector2Int[] customLineIndexArray = new Vector2Int[0];

		private Text text;

		private UICharInfo[] characters;

		private UILineInfo[] lines;

		private Color[] gradientColors;

		private char[] textChars;

		private List<UIVertex> stream;

		private int characterCountVisible;

		private UIVertex[] underlineUIVertexs = new UIVertex[6];
	}
}
