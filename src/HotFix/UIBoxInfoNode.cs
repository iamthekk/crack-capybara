using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIBoxInfoNode : CustomBehaviour
	{
		public UIBoxInfoViewModule.UIBoxInfoNodeType NodeType { get; set; }

		public RectTransform ItemParent { get; set; }

		public void SetView(bool isShow)
		{
			base.gameObject.SetActive(isShow);
		}

		public void SetArrowPos(Vector3 targetPos, bool isSetArrowPos = false)
		{
			if (this.Image_Arrow != null && isSetArrowPos)
			{
				this.Image_Arrow.transform.position = new Vector3(targetPos.x, this.Image_Arrow.transform.position.y, this.Image_Arrow.transform.position.z);
			}
		}

		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.SetView(false);
		}

		public GameObject Image_Arrow;
	}
}
