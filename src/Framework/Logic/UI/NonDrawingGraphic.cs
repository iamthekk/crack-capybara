using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	[RequireComponent(typeof(CanvasRenderer))]
	[AddComponentMenu("Layout/Extensions/NonDrawingGraphic")]
	public class NonDrawingGraphic : MaskableGraphic
	{
		public override void SetMaterialDirty()
		{
		}

		public override void SetVerticesDirty()
		{
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}
