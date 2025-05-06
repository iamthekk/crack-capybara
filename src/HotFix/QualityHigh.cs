using System;
using UnityEngine;

namespace HotFix
{
	public class QualityHigh : QualityBase
	{
		protected override void OnEnter()
		{
			QualitySettings.SetQualityLevel(0, true);
			Application.targetFrameRate = 60;
		}

		protected override void OnExit()
		{
		}

		protected override QualityManager.QualityWidthType getWidthType()
		{
			return QualityManager.QualityWidthType.e1080;
		}
	}
}
