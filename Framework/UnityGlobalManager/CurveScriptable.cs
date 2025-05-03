using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UnityGlobalManager
{
	public class CurveScriptable : ScriptableObject
	{
		public AnimationCurve GetAnimationCurve(int id)
		{
			if (this.DicCurves == null)
			{
				this.LoadAsset();
			}
			AnimationCurve animationCurve;
			this.DicCurves.TryGetValue(id, out animationCurve);
			return animationCurve;
		}

		private void LoadAsset()
		{
			this.DicCurves = new Dictionary<int, AnimationCurve>();
			for (int i = 0; i < this.m_curves.Length; i++)
			{
				CurveScriptable.CurveData curveData = this.m_curves[i];
				this.DicCurves[curveData.m_id] = curveData.m_curve;
			}
		}

		[SerializeField]
		private CurveScriptable.CurveData[] m_curves;

		private Dictionary<int, AnimationCurve> DicCurves;

		[Serializable]
		public class CurveData
		{
			public int m_id;

			public AnimationCurve m_curve;
		}
	}
}
