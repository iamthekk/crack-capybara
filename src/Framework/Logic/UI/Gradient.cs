using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	[AddComponentMenu("UI/Effects/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.mVertexList == null)
			{
				this.mVertexList = new List<UIVertex>();
			}
			vh.GetUIVertexStream(this.mVertexList);
			this.ApplyGradient(this.mVertexList);
			vh.Clear();
			vh.AddUIVertexTriangleStream(this.mVertexList);
		}

		private void ApplyGradient(List<UIVertex> vertexList)
		{
			for (int i = 0; i < vertexList.Count; i += 6)
			{
				this.ChangeColor(vertexList, i, this.topColor);
				this.ChangeColor(vertexList, i + 1, this.topColor);
				this.ChangeColor(vertexList, i + 2, this.bottomColor);
				this.ChangeColor(vertexList, i + 3, this.bottomColor);
				this.ChangeColor(vertexList, i + 4, this.bottomColor);
				this.ChangeColor(vertexList, i + 5, this.topColor);
			}
		}

		private void ChangeColor(List<UIVertex> verList, int index, Color color)
		{
			UIVertex uivertex = verList[index];
			uivertex.color = color;
			verList[index] = uivertex;
		}

		[SerializeField]
		private Color32 topColor = Color.white;

		[SerializeField]
		private Color32 bottomColor = Color.black;

		private List<UIVertex> mVertexList;
	}
}
