using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIChapterRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.maskObj.SetActiveSafe(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void Refresh(ChapterRewardData data)
		{
			if (data == null)
			{
				return;
			}
			this.textTarget.text = (data.IsNeedPass ? Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_PassText") : Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_Day", new object[] { data.stage }));
			this.textChapter.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_Chapter", new object[] { data.chapterId });
		}

		public void SetMask(bool isShow)
		{
			this.maskObj.SetActiveSafe(isShow);
		}

		public CustomText textTarget;

		public CustomText textChapter;

		public GameObject maskObj;
	}
}
