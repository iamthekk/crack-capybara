using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class ChapterPowerfulEnemyViewModule : BaseViewModule
	{
		[GameTestMethod("Battle", "ChapterPowerfulEnemyViewModule", "", 0)]
		private static void OpenChapterPowerfulEnemyViewModule()
		{
			ChapterPowerfulEnemyViewModule.OpenData openData = new ChapterPowerfulEnemyViewModule.OpenData(ChapterPowerfulEnemyViewModule.StyleType.Boss, null);
			GameApp.View.OpenView(ViewName.ChapterPowerfulEnemyViewModule, openData, 1, null, null);
		}

		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.rootPoint.transform.localScale = Vector3.one;
			this.openData = data as ChapterPowerfulEnemyViewModule.OpenData;
			if (this.openData == null)
			{
				this.CloseSelf();
				return;
			}
			if (this.openData.StyleType == ChapterPowerfulEnemyViewModule.StyleType.Normal)
			{
				this.animator.gameObject.SetActiveSafe(false);
				this.normalAni.gameObject.SetActiveSafe(true);
				this.DoPlayNormal();
				return;
			}
			this.animator.gameObject.SetActiveSafe(true);
			this.normalAni.gameObject.SetActiveSafe(false);
			this.DoPlay();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.sequencePool.Clear(false);
			Action onComplete = this.openData.OnComplete;
			if (onComplete == null)
			{
				return;
			}
			onComplete();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void DoPlay()
		{
			this.bossIcon.SetActive(this.openData.StyleType == ChapterPowerfulEnemyViewModule.StyleType.Boss);
			this.eliteIcon.SetActive(this.openData.StyleType == ChapterPowerfulEnemyViewModule.StyleType.Elite);
			ChapterPowerfulEnemyViewModule.StyleType styleType = this.openData.StyleType;
			if (styleType != ChapterPowerfulEnemyViewModule.StyleType.Boss)
			{
				if (styleType == ChapterPowerfulEnemyViewModule.StyleType.Elite)
				{
					GameApp.Sound.PlayClip(672, 1f);
					this.infoText.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapterPowerfulEnemy_Elite");
				}
			}
			else
			{
				GameApp.Sound.PlayClip(673, 1f);
				this.infoText.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapterPowerfulEnemy_Boss");
			}
			this.animator.SetTrigger(ChapterPowerfulEnemyViewModule.PlayAnimKey);
			this.sequencePool.Clear(false);
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 1.4f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScaleY(this.rootPoint, 0f, 0.12f));
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.CloseSelf));
		}

		private void DoPlayNormal()
		{
			this.infoTextNormal.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapterPowerfulEnemy_Normal");
			this.normalAni.SetTrigger(ChapterPowerfulEnemyViewModule.EnemyAnimKey);
			this.sequencePool.Clear(false);
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 1.4f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScaleY(this.rootPoint, 0f, 0.12f));
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.CloseSelf));
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.ChapterPowerfulEnemyViewModule, null);
		}

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private GameObject bossIcon;

		[SerializeField]
		private GameObject eliteIcon;

		[SerializeField]
		private CustomText infoText;

		[SerializeField]
		private Transform rootPoint;

		[SerializeField]
		private Animator normalAni;

		[SerializeField]
		private CustomText infoTextNormal;

		private readonly SequencePool sequencePool = new SequencePool();

		private ChapterPowerfulEnemyViewModule.OpenData openData;

		private static readonly int PlayAnimKey = Animator.StringToHash("Play");

		private static readonly int EnemyAnimKey = Animator.StringToHash("EnemyAnim");

		public enum StyleType
		{
			Boss,
			Elite,
			Normal
		}

		public class OpenData
		{
			public ChapterPowerfulEnemyViewModule.StyleType StyleType { get; private set; }

			public Action OnComplete { get; private set; }

			public OpenData(ChapterPowerfulEnemyViewModule.StyleType styleType, Action onComplete)
			{
				this.StyleType = styleType;
				this.OnComplete = onComplete;
			}
		}
	}
}
