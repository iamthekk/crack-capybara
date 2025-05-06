using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class FunctionOpenViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.FunctionDataModule);
			this.Button_Mask.m_onClick = null;
			this.Button_TapToClose.OnClose = null;
			this.AnimatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
		}

		public override void OnOpen(object data)
		{
			FunctionData nextUnlockingData = this.mDataModule.GetNextUnlockingData();
			if (nextUnlockingData == null)
			{
				HLog.LogError("[Function]新功能开启，打开了界面，但是没有开启的新功能！");
				this.OnClickToCloseView();
				return;
			}
			this.mCurrentOpenFunction = nextUnlockingData;
			Function_Function elementById = GameApp.Table.GetManager().GetFunction_FunctionModelInstance().GetElementById(this.mCurrentOpenFunction.ID);
			this.TextTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("9001");
			string text = "??";
			if (!string.IsNullOrEmpty(elementById.nameID))
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			}
			this.TextDes.text = Singleton<LanguageManager>.Instance.GetInfoByID("9002", new object[] { text });
			this.ImageFunctionIcon.SetImage(GameApp.Table.GetAtlasPath(elementById.iconAtlasID), elementById.iconName);
			this.ImageFunctionIcon.gameObject.SetActive(true);
			GameApp.Sound.PlayClip(674, 1f);
			this.MainAnimator.enabled = true;
			this.MainAnimator.Play("Show");
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
			if (this.Button_Mask != null)
			{
				this.Button_Mask.onClick = null;
			}
			if (this.Button_TapToClose != null)
			{
				this.Button_TapToClose.OnClose = null;
			}
			if (this.mFlyIcon != null)
			{
				Object.Destroy(this.mFlyIcon);
			}
		}

		public override void OnDelete()
		{
			this.AnimatorListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickToCloseView()
		{
			if (this.Button_Mask != null)
			{
				this.Button_Mask.onClick = null;
			}
			if (this.Button_TapToClose != null)
			{
				this.Button_TapToClose.OnClose = null;
			}
			this.IconMove();
		}

		public void IconMove()
		{
			if (base.gameObject == null || this.mCurrentOpenFunction == null)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			Function_Function elementById = GameApp.Table.GetManager().GetFunction_FunctionModelInstance().GetElementById(this.mCurrentOpenFunction.ID);
			Transform transform = Singleton<GameFunctionController>.Instance.TryGetFunctionTarget(elementById.flyPos);
			if (transform != null)
			{
				vector = transform.position;
			}
			this.ImageFunctionIcon.gameObject.SetActive(false);
			this.mFlyIcon = Object.Instantiate<GameObject>(this.ImageFunctionIcon.gameObject, base.transform);
			this.mFlyIcon.SetActive(true);
			Transform tficon = this.mFlyIcon.transform;
			tficon.Find("Effect_Root").gameObject.SetActive(false);
			AnimationCurve curves = this.Curves;
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(tficon, vector, 0.5f, false), curves));
			TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(tficon, Vector3.one, 0.5f), curves));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (tficon != null)
				{
					tficon.GetComponent<CustomImage>().enabled = false;
					tficon.Find("Effect_Fly_End").gameObject.SetActive(true);
				}
			});
			TweenSettingsExtensions.AppendInterval(sequence, 0.5f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (this.mFlyIcon != null)
				{
					this.mFlyIcon.SetActiveSafe(true);
				}
				this.IconMoveOver();
			});
		}

		private void IconMoveOver()
		{
			GameApp.View.CloseView(ViewName.FunctionOpenViewModule, delegate
			{
				if (!Singleton<GameFunctionController>.Instance.HasWaitedNewFunction())
				{
					GuideController.Instance.CustomizeStringTrigger("OpenMainUI");
				}
			});
		}

		private void OnAnimatorListen(GameObject obj, string eventstring)
		{
			if (eventstring == "UnlockAniEnd")
			{
				this.OnUnlockAniEnd();
				return;
			}
			if (!(eventstring == "ShowAniEnd"))
			{
				return;
			}
			this.OnShowAniEnd();
		}

		private void OnUnlockAniEnd()
		{
			if (this.Button_Mask != null)
			{
				this.Button_Mask.m_onClick = new Action(this.OnClickToCloseView);
			}
			if (this.Button_TapToClose != null)
			{
				this.Button_TapToClose.OnClose = new Action(this.OnClickToCloseView);
			}
			if (this.mCurrentOpenFunction != null)
			{
				EventArgsFunctionOpen eventArgsFunctionOpen = new EventArgsFunctionOpen();
				eventArgsFunctionOpen.FunctionID = this.mCurrentOpenFunction.ID;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Function_Open, eventArgsFunctionOpen);
			}
		}

		private void OnShowAniEnd()
		{
			if (this.MainAnimator != null)
			{
				this.MainAnimator.enabled = false;
			}
		}

		public CustomButton Button_Mask;

		public TapToCloseCtrl Button_TapToClose;

		public CustomImage ImageFunctionIcon;

		public Animator MainAnimator;

		public AnimatorListen AnimatorListen;

		public CustomText TextTitle;

		public CustomText TextDes;

		public GameObject ObjRoot;

		public AnimationCurve Curves;

		private FunctionDataModule mDataModule;

		private FunctionData mCurrentOpenFunction;

		private SequencePool m_seqPool = new SequencePool();

		private GameObject mFlyIcon;
	}
}
