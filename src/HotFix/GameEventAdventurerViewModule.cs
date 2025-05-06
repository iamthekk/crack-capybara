using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace HotFix
{
	public class GameEventAdventurerViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 940;
		}

		public override void OnCreate(object data)
		{
			this.buttonGet.onClick.AddListener(new UnityAction(this.OnGetGift));
			this.spineModelItem.Init();
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as GameEventAdventurerViewModule.OpenData;
			if (this.openData == null)
			{
				GameApp.View.CloseView(ViewName.GameEventAdventurerViewModule, null);
				return;
			}
			GameApp.Sound.PlayClip(59, 1f);
			this.surpriseId = this.openData.surpriseId;
			this.attributeBuild = Singleton<GameEventController>.Instance.RandomAttributeBuildId(this.openData.seed);
			this.attParam = this.attributeBuild.attData.ToNodeAttParam();
			this.Refresh();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.sequencePool.Clear(false);
		}

		public override void OnDelete()
		{
			this.buttonGet.onClick.RemoveListener(new UnityAction(this.OnGetGift));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public async void Refresh()
		{
			Chapter_surpriseBuild elementById = GameApp.Table.GetManager().GetChapter_surpriseBuildModelInstance().GetElementById(this.surpriseId);
			if (elementById != null)
			{
				if (elementById.modelId > 0)
				{
					await this.spineModelItem.ShowModel(elementById.modelId, "Idle", true);
				}
				else
				{
					await this.spineModelItem.ShowMemberModel(elementById.memberId, "Idle", true);
				}
				this.spineModelItem.SetAnimationTimeScale(0.5f);
				string atlasPath = GameApp.Table.GetAtlasPath(this.attributeBuild.Config.atlasId);
				this.imageIcon.SetImage(atlasPath, this.attributeBuild.Config.icon);
				this.textAttribute.text = this.GetDes();
				this.PlayOpenAni();
			}
		}

		private void OnGetGift()
		{
			Singleton<GameEventController>.Instance.MergerAttribute(new List<NodeAttParam> { this.attParam });
			GameApp.View.CloseView(ViewName.GameEventAdventurerViewModule, null);
			EventArgSelectSurprise eventArgSelectSurprise = new EventArgSelectSurprise();
			eventArgSelectSurprise.SetData(GameEventPushType.CloseAdventurer, this.attParam);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, eventArgSelectSurprise);
		}

		private string GetDes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			GameEventAttType attType = this.attParam.attType;
			if (attType != GameEventAttType.AttackPercent)
			{
				if (attType != GameEventAttType.HPMaxPercent)
				{
					if (attType == GameEventAttType.DefencePercent)
					{
						string text2 = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_37");
						text = string.Format("{0} +{1}%", text2, this.attParam.baseCount);
					}
				}
				else
				{
					string text2 = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_18");
					text = string.Format("{0} +{1}%", text2, this.attParam.baseCount);
				}
			}
			else
			{
				string text2 = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_16");
				text = string.Format("{0} +{1}%", text2, this.attParam.baseCount);
			}
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.Append(text);
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		private void PlayOpenAni()
		{
			Sequence sequence = this.sequencePool.Get();
			this.downObj.transform.localPosition = new Vector3(0f, 500f, 0f);
			Color color = this.imageLight.color;
			color.a = 0f;
			this.imageLight.color = color;
			this.textTip.transform.localScale = Vector3.zero;
			this.attrObj.transform.localScale = Vector3.zero;
			this.buttonGet.transform.localScale = Vector3.zero;
			float num = 0.2f;
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveY(this.downObj.transform, 0f, num, false)), delegate
			{
				this.downObj.transform.localPosition = Vector3.zero;
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.imageLight, 1f, num)), delegate
			{
				color = this.imageLight.color;
				color.a = 1f;
				this.imageLight.color = color;
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.textTip.transform, Vector3.one, num), 22)), delegate
			{
				this.textTip.transform.localScale = Vector3.one;
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.attrObj.transform, Vector3.one, num), 22)), delegate
			{
				this.attrObj.transform.localScale = Vector3.one;
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.buttonGet.transform, Vector3.one, num), 22)), delegate
			{
				this.buttonGet.transform.localScale = Vector3.one;
			});
		}

		public UISpineModelItem spineModelItem;

		public CustomImage imageIcon;

		public CustomText textAttribute;

		public CustomButton buttonGet;

		public Image imageLight;

		public GameObject downObj;

		public GameObject textTip;

		public GameObject attrObj;

		private int surpriseId;

		private GameEventAttributesFactory.AttributeBuild attributeBuild;

		private NodeAttParam attParam;

		private GameEventAdventurerViewModule.OpenData openData;

		private List<SpriteAtlas> atlasList = new List<SpriteAtlas>();

		private SequencePool sequencePool = new SequencePool();

		public class OpenData
		{
			public int surpriseId;

			public int seed;
		}
	}
}
