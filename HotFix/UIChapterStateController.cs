using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIChapterStateController : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Show(false, true);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void Show(bool value, bool isPlayAnimation = true)
		{
			if (value)
			{
				if (isPlayAnimation)
				{
					this.m_child.anchoredPosition = new Vector2(-400f, 0f);
					this.m_animator.SetTrigger("Show");
					return;
				}
				this.m_child.anchoredPosition = new Vector2(0f, 0f);
				this.m_animator.SetTrigger("ShowIdle");
				return;
			}
			else
			{
				if (isPlayAnimation)
				{
					this.m_child.anchoredPosition = new Vector2(0f, 0f);
					this.m_animator.SetTrigger("Hide");
					return;
				}
				this.m_child.anchoredPosition = new Vector2(-400f, 0f);
				this.m_animator.SetTrigger("HideIdle");
				return;
			}
		}

		public void SetDay(int current, int max)
		{
			this.Text_Day.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_88", new object[] { current, max });
		}

		public void SetChapterName(string name)
		{
			this.Text_ChapterName.text = name;
		}

		public RectTransform m_child;

		public Animator m_animator;

		public CustomText Text_Day;

		public CustomText Text_ChapterName;

		private const string AnimationName_Show = "Show";

		private const string AnimationName_ShowIdle = "ShowIdle";

		private const string AnimationName_Hide = "Hide";

		private const string AnimationName_HideIdle = "HideIdle";
	}
}
