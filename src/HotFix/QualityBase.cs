using System;
using Framework.Logic;
using UnityEngine;

namespace HotFix
{
	public abstract class QualityBase
	{
		public void Enter()
		{
			int num;
			switch (this.getWidthType())
			{
			case QualityManager.QualityWidthType.e480:
				num = 480;
				break;
			case QualityManager.QualityWidthType.e720:
				num = 720;
				break;
			case QualityManager.QualityWidthType.e1080:
				num = 1080;
				break;
			default:
				num = 720;
				break;
			}
			Vector2Int sizeByWidth = this.GetSizeByWidth(num);
			Screen.SetResolution(sizeByWidth.x, sizeByWidth.y, true);
			this.OnEnter();
		}

		protected abstract void OnEnter();

		public void Exit()
		{
			this.OnExit();
		}

		protected abstract void OnExit();

		protected abstract QualityManager.QualityWidthType getWidthType();

		protected Vector2Int GetSizeByWidth(int res)
		{
			int num = Utility.UI.ScreenHeight;
			int num2 = Utility.UI.ScreenWidth;
			num = (int)((float)num * ((float)res + 0f) / (float)num2);
			num2 = res;
			if (Utility.UI.ScreenWidth < num2 || Utility.UI.ScreenHeight < num)
			{
				num2 = Utility.UI.ScreenWidth;
				num = Utility.UI.ScreenHeight;
			}
			return new Vector2Int(num2, num);
		}
	}
}
