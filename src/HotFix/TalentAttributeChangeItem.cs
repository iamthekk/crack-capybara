using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class TalentAttributeChangeItem : MonoBehaviour
	{
		public void SetData(long oldValue, long newValue, long addValue)
		{
			this.txtValueOld.text = DxxTools.FormatNumber(oldValue);
			this.txtValueNew.text = DxxTools.FormatNumber(newValue);
			this.txtValueAdd.text = "+" + DxxTools.FormatNumber(addValue);
		}

		public CustomText txtValueOld;

		public CustomText txtValueNew;

		public CustomText txtValueAdd;
	}
}
