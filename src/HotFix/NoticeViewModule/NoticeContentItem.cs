using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.NoticeViewModule
{
	public class NoticeContentItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Text_Title.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(string content, int index, int totalCount)
		{
			this.Obj_Line.SetActiveSafe(index != totalCount);
			this.Text_Content.text = content;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.Obj_Line.transform.parent as RectTransform);
		}

		public CustomText Text_Title;

		public CustomText Text_Content;

		public GameObject Obj_Line;
	}
}
