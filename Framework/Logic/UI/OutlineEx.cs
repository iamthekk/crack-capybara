using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class OutlineEx : BaseMeshEffect
	{
		private static Material GetMat()
		{
			if (OutlineEx._mat != null)
			{
				return OutlineEx._mat;
			}
			OutlineEx._mat = new Material(Shader.Find("TSF Shaders/UI/OutlineEx"));
			return OutlineEx._mat;
		}

		protected override void Awake()
		{
			this.m_text = base.GetComponent<Text>();
			if (this.m_text == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				base.graphic.material == Graphic.defaultGraphicMaterial;
			}
			this.m_outlineWidth = this.GetOutlineWidth();
		}

		protected virtual float GetOutlineWidth()
		{
			return 10f;
		}

		protected virtual Vector4 GetMin()
		{
			return this.m_min;
		}

		protected virtual Vector4 GetMax()
		{
			return this.m_max;
		}

		private void SetCanvas()
		{
			if (base.graphic && base.graphic.canvas)
			{
				AdditionalCanvasShaderChannels additionalShaderChannels = base.graphic.canvas.additionalShaderChannels;
				AdditionalCanvasShaderChannels additionalCanvasShaderChannels = 1;
				if ((additionalShaderChannels & additionalCanvasShaderChannels) != additionalCanvasShaderChannels)
				{
					base.graphic.canvas.additionalShaderChannels |= additionalCanvasShaderChannels;
				}
				additionalCanvasShaderChannels = 2;
				if ((additionalShaderChannels & additionalCanvasShaderChannels) != additionalCanvasShaderChannels)
				{
					base.graphic.canvas.additionalShaderChannels |= additionalCanvasShaderChannels;
				}
				additionalCanvasShaderChannels = 4;
				if ((additionalShaderChannels & additionalCanvasShaderChannels) != additionalCanvasShaderChannels)
				{
					base.graphic.canvas.additionalShaderChannels |= additionalCanvasShaderChannels;
				}
				additionalCanvasShaderChannels = 16;
				if ((additionalShaderChannels & additionalCanvasShaderChannels) != additionalCanvasShaderChannels)
				{
					base.graphic.canvas.additionalShaderChannels |= additionalCanvasShaderChannels;
				}
				additionalCanvasShaderChannels = 8;
				if ((additionalShaderChannels & additionalCanvasShaderChannels) != additionalCanvasShaderChannels)
				{
					base.graphic.canvas.additionalShaderChannels |= additionalCanvasShaderChannels;
				}
			}
		}

		private void refreshOffset()
		{
			if (this.m_text == null)
			{
				this.m_text = base.gameObject.GetComponent<Text>();
			}
			if (this.m_text == null)
			{
				return;
			}
			float num = ((float)(this.m_text.fontSize - OutlineEx.m_minFont) + 0f) / (float)(OutlineEx.m_maxFont - OutlineEx.m_minFont);
			this.m_offset = Vector4.Lerp(this.GetMin(), this.GetMax(), num);
			this.m_offset *= (float)Screen.currentResolution.width / 1080f;
		}

		private void _Refresh()
		{
			base.graphic.SetAllDirty();
			base.graphic.SetMaterialDirty();
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.m_isSeting)
			{
				this.SetCanvas();
				this.m_isSeting = true;
			}
			vh.GetUIVertexStream(OutlineEx.m_VetexList);
			this.refreshOffset();
			this._ProcessVertices();
			vh.Clear();
			vh.AddUIVertexTriangleStream(OutlineEx.m_VetexList);
		}

		private void _ProcessVertices()
		{
			int i = 0;
			int num = OutlineEx.m_VetexList.Count - 3;
			while (i <= num)
			{
				UIVertex uivertex = OutlineEx.m_VetexList[i];
				UIVertex uivertex2 = OutlineEx.m_VetexList[i + 1];
				UIVertex uivertex3 = OutlineEx.m_VetexList[i + 2];
				float num2 = OutlineEx._Min(uivertex.position.x, uivertex2.position.x, uivertex3.position.x);
				float num3 = OutlineEx._Min(uivertex.position.y, uivertex2.position.y, uivertex3.position.y);
				float num4 = OutlineEx._Max(uivertex.position.x, uivertex2.position.x, uivertex3.position.x);
				float num5 = OutlineEx._Max(uivertex.position.y, uivertex2.position.y, uivertex3.position.y);
				Vector2 vector = new Vector2(num2 + num4, num3 + num5) * 0.5f;
				Vector2 vector2 = uivertex3.position - uivertex2.position;
				Vector2 vector3 = uivertex3.uv0 - uivertex2.uv0;
				if ((vector2.x == 0f && vector3.x == 0f) || (vector2.y == 0f && vector3.y == 0f))
				{
					this.accept = 0f;
				}
				else
				{
					this.accept = 2f;
				}
				Vector2 vector4 = uivertex.position;
				Vector2 vector5 = uivertex2.position;
				Vector2 vector6 = uivertex3.position;
				Vector2 vector7;
				Vector2 vector8;
				Vector2 vector9;
				Vector2 vector10;
				if (Mathf.Abs(Vector2.Dot((vector5 - vector4).normalized, Vector2.right)) > Mathf.Abs(Vector2.Dot((vector6 - vector5).normalized, Vector2.right)))
				{
					vector7 = vector5 - vector4;
					vector8 = vector6 - vector5;
					vector9 = uivertex2.uv0 - uivertex.uv0;
					vector10 = uivertex3.uv0 - uivertex2.uv0;
				}
				else
				{
					vector7 = vector6 - vector5;
					vector8 = vector5 - vector4;
					vector9 = uivertex3.uv0 - uivertex2.uv0;
					vector10 = uivertex2.uv0 - uivertex.uv0;
				}
				Vector2 vector11 = OutlineEx._Min(uivertex.uv0, uivertex2.uv0, uivertex3.uv0);
				Vector2 vector12 = OutlineEx._Max(uivertex.uv0, uivertex2.uv0, uivertex3.uv0);
				uivertex = OutlineEx._SetNewPosAndUV(uivertex, this.m_outlineWidth, vector, vector7, vector8, vector9, vector10, vector11, vector12);
				uivertex.uv3 = new Vector2(this.m_outlineWidth, this.accept);
				uivertex.tangent = this.m_offset;
				uivertex2 = OutlineEx._SetNewPosAndUV(uivertex2, this.m_outlineWidth, vector, vector7, vector8, vector9, vector10, vector11, vector12);
				uivertex2.uv3 = new Vector2(this.m_outlineWidth, this.accept);
				uivertex2.tangent = this.m_offset;
				uivertex3 = OutlineEx._SetNewPosAndUV(uivertex3, this.m_outlineWidth, vector, vector7, vector8, vector9, vector10, vector11, vector12);
				uivertex3.uv3 = new Vector2(this.m_outlineWidth, this.accept);
				uivertex3.tangent = this.m_offset;
				OutlineEx.m_VetexList[i] = uivertex;
				OutlineEx.m_VetexList[i + 1] = uivertex2;
				OutlineEx.m_VetexList[i + 2] = uivertex3;
				i += 3;
			}
		}

		private static UIVertex _SetNewPosAndUV(UIVertex pVertex, float pOutLineWidth, Vector2 pPosCenter, Vector2 pTriangleX, Vector2 pTriangleY, Vector2 pUVX, Vector2 pUVY, Vector2 pUVOriginMin, Vector2 pUVOriginMax)
		{
			Vector3 position = pVertex.position;
			float num = ((position.x > pPosCenter.x) ? pOutLineWidth : (-pOutLineWidth));
			float num2 = ((position.y > pPosCenter.y) ? pOutLineWidth : (-pOutLineWidth));
			position.x += num;
			position.y += num2;
			pVertex.position = position;
			Vector2 vector = pVertex.uv0;
			vector += pUVX / pTriangleX.magnitude * num * (float)((Vector2.Dot(pTriangleX, Vector2.right) > 0f) ? 1 : (-1));
			vector += pUVY / pTriangleY.magnitude * num2 * (float)((Vector2.Dot(pTriangleY, Vector2.up) > 0f) ? 1 : (-1));
			pVertex.uv0 = vector;
			pVertex.uv1 = pUVOriginMin;
			pVertex.uv2 = pUVOriginMax;
			return pVertex;
		}

		private static float _Min(float pA, float pB, float pC)
		{
			return Mathf.Min(Mathf.Min(pA, pB), pC);
		}

		private static float _Max(float pA, float pB, float pC)
		{
			return Mathf.Max(Mathf.Max(pA, pB), pC);
		}

		private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC)
		{
			return new Vector2(OutlineEx._Min(pA.x, pB.x, pC.x), OutlineEx._Min(pA.y, pB.y, pC.y));
		}

		private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC)
		{
			return new Vector2(OutlineEx._Max(pA.x, pB.x, pC.x), OutlineEx._Max(pA.y, pB.y, pC.y));
		}

		protected float m_outlineWidth;

		private static List<UIVertex> m_VetexList = new List<UIVertex>();

		private static Material _mat = null;

		private static int m_minFont = 25;

		private static int m_maxFont = 100;

		private Vector4 m_min = new Vector4(0f, 0f, 0.5f, 0f);

		private Vector4 m_max = new Vector4(0.2f, 0.13f, 3f, 0f);

		public Vector4 m_offset = Vector4.zero;

		public Text m_text;

		private bool isRfresh;

		private bool m_isSeting;

		private float accept;
	}
}
