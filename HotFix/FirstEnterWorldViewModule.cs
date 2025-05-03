using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class FirstEnterWorldViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_button.onClick.AddListener(new UnityAction(this.OnClickNextBt));
			this.m_cloudListen = this.m_cloudAni.GetComponent<AnimatorListen>();
			this.m_cloudListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			this.m_pageAnis.ForEach(delegate(Animator pageAni)
			{
				AnimatorListen animatorListen;
				if (pageAni != null && pageAni.TryGetComponent<AnimatorListen>(ref animatorListen))
				{
					animatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
				}
			});
		}

		public override void OnDelete()
		{
			this.m_button.onClick.RemoveListener(new UnityAction(this.OnClickNextBt));
			this.m_cloudListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			foreach (Animator animator in this.m_pageAnis)
			{
				AnimatorListen animatorListen;
				if (animator != null && animator.TryGetComponent<AnimatorListen>(ref animatorListen))
				{
					animatorListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
				}
			}
		}

		public override void OnOpen(object data)
		{
			this.m_tapContinue.SetActiveSafe(false);
			this.changeState(FirstEnterWorldViewModule.EState.eIdle);
			this.m_currentPage = 1;
			this.playCurrentPage();
		}

		public override void OnClose()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickNextBt()
		{
			this.m_tapContinue.SetActiveSafe(false);
			FirstEnterWorldViewModule.EState state = this.m_state;
			if (state == FirstEnterWorldViewModule.EState.eRunning)
			{
				this.m_pageAnis[this.m_currentPage].Play("pageRun", 0, 1f);
				return;
			}
			if (state != FirstEnterWorldViewModule.EState.ePageFinish)
			{
				return;
			}
			this.m_currentPage++;
			if (this.m_currentPage > 4)
			{
				this.m_button.enabled = false;
				this.EnterGame();
				return;
			}
			this.m_cloudAni.SetTrigger("Close");
		}

		private void EnterGame()
		{
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					GameApp.View.CloseView(ViewName.FirstEnterWorldViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		private void OnAnimatorListen(GameObject obj, string listenName)
		{
			if (string.IsNullOrEmpty(listenName))
			{
				return;
			}
			if (listenName == "PageEnd1" || listenName == "PageEnd2" || listenName == "PageEnd3" || listenName == "PageEnd4")
			{
				int.Parse(listenName.Substring(7, 1));
				this.changeState(FirstEnterWorldViewModule.EState.ePageFinish);
				this.m_tapContinue.SetActiveSafe(true);
				return;
			}
			if (!(listenName == "CloudClose"))
			{
				return;
			}
			this.playCurrentPage();
		}

		private void refreshInfo()
		{
			this.Text_Info.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(100100 + this.m_currentPage);
		}

		private void playCurrentPage()
		{
			this.changeState(FirstEnterWorldViewModule.EState.eRunning);
			for (int i = 1; i <= 4; i++)
			{
				this.m_pageParents[i].SetActiveSafe(false);
			}
			this.m_pageParents[this.m_currentPage].SetActiveSafe(true);
			this.refreshInfo();
		}

		private void changeState(FirstEnterWorldViewModule.EState state)
		{
			this.m_state = state;
		}

		private const int MaxPage = 4;

		[SerializeField]
		private CustomText Text_Info;

		[SerializeField]
		private GameObject m_tapContinue;

		[SerializeField]
		private CustomButton m_button;

		[SerializeField]
		private Animator m_cloudAni;

		[SerializeField]
		private List<Animator> m_pageAnis = new List<Animator>();

		[SerializeField]
		private List<GameObject> m_pageParents = new List<GameObject>();

		private AnimatorListen m_cloudListen;

		private FirstEnterWorldViewModule.EState m_state;

		private int m_currentPage = 1;

		private enum EState
		{
			eIdle,
			eRunning,
			ePageFinish
		}
	}
}
