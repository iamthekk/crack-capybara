using System;
using Framework.Logic.Component;
using TMPro;

namespace HotFix
{
	public class UIPrivilegeCardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(string text, int index)
		{
			this.textMesh.text = text;
		}

		public TextMeshProUGUI textMesh;
	}
}
