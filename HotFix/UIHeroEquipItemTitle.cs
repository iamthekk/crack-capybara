using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIHeroEquipItemTitle : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshData(string info)
		{
			if (this.m_line == null)
			{
				return;
			}
			this.m_line.SetText(info);
		}

		[SerializeField]
		private LineTextCtrl m_line;
	}
}
