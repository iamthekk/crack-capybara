using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class CollectionAttributeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void PlayEffect()
		{
			this.animator.Play("Show");
		}

		public void SetData(string key, string desc, string offsetValue)
		{
			this.attributeKey = key;
			if (!string.IsNullOrEmpty(offsetValue))
			{
				this.txtAttribute.text = desc + offsetValue;
				return;
			}
			this.txtAttribute.text = desc;
		}

		public CustomText txtAttribute;

		public Animator animator;

		[NonSerialized]
		public string attributeKey;
	}
}
