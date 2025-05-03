using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIEquipMergeSkillNode : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void SetTxt(string info)
		{
			if (this.m_text == null)
			{
				return;
			}
			this.m_text.text = info;
		}

		public void CalcTxtLineAndHeight()
		{
			float preferredHeight = this.m_text.preferredHeight;
			base.rectTransform.sizeDelta = new Vector2(base.rectTransform.sizeDelta.x, preferredHeight);
		}

		[SerializeField]
		private CustomText m_text;
	}
}
