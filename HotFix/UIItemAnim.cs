using System;
using Framework.Logic.Modules;
using UnityEngine;

namespace HotFix
{
	public class UIItemAnim : MonoAnimBase
	{
		protected override void SetPercent(float percent)
		{
			float num = Mathf.Lerp(this.fromScale, this.toScale, percent);
			float num2 = Mathf.Lerp(0f, 1f, percent);
			base.transform.localScale = new Vector3(num, num, num);
			GameObjectExpand.GetOrAddComponent<CanvasGroup>(base.gameObject).alpha = num2;
		}

		[SerializeField]
		private float fromScale = 1.1f;

		[SerializeField]
		private float toScale = 1f;
	}
}
