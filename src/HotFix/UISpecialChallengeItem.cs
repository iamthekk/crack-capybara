using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UISpecialChallengeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(string txtName, string txtDes)
		{
			this.textName.text = txtName;
			this.textDes.text = txtDes;
		}

		public CustomText textName;

		public CustomText textDes;
	}
}
