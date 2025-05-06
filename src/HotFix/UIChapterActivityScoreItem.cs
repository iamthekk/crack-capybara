using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIChapterActivityScoreItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int atlasId, string icon, int num)
		{
			string atlasPath = GameApp.Table.GetAtlasPath(atlasId);
			this.imageIcon.SetImage(atlasPath, icon);
			this.textScore.text = string.Format("+{0}", num);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutTrans);
		}

		public RectTransform layoutTrans;

		public CustomImage imageIcon;

		public CustomText textScore;
	}
}
