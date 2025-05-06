using System;
using System.Collections;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class WorldBossAnimView : BaseViewModule
	{
		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnCreate(object data)
		{
		}

		public override void OnDelete()
		{
		}

		public override void OnOpen(object data)
		{
			this._openData = (WorldBossAnimView.OpenData)data;
			this._timer = 0f;
			int groupType = this._openData.GroupType;
			int rankLevel = this._openData.RankLevel;
			int lastRank = this._openData.LastRank;
			int lastRankLevel = this._openData.LastRankLevel;
			WorldBoss_Subsection worldBoss_Subsection = null;
			string text;
			if (lastRank > 0)
			{
				worldBoss_Subsection = GameApp.Table.GetManager().GetWorldBoss_Subsection(lastRankLevel);
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(worldBoss_Subsection.languageId);
				text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_LastRound_GroupRank", new object[] { infoByID, lastRank });
			}
			else
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_LastRound_NotChallenge");
			}
			WorldBoss_Subsection worldBoss_Subsection2 = GameApp.Table.GetManager().GetWorldBoss_Subsection(rankLevel);
			int rankLevel2 = worldBoss_Subsection2.RankLevel;
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(worldBoss_Subsection2.languageId);
			if (this._flyTrans != null)
			{
				this._flyTrans.position = this._startPos;
				this._flyTrans.sizeDelta = this._startScale;
			}
			if (groupType == 1)
			{
				this.SetGroupObject.SetActiveSafe(true);
				this.SameGroupObject.SetActiveSafe(false);
				this.ChangeGroupObject.SetActiveSafe(false);
				this._flyTrans = this.SetImageTrans;
				this._animTime = 1.5f;
				int atlasName = worldBoss_Subsection2.atlasName;
				this.SetImage.SetImage(worldBoss_Subsection2.atlasName, worldBoss_Subsection2.atlasId);
				this.SetImageMask(this.SetMaskImage, rankLevel2, atlasName);
				this.SetImageWhite(this.SetWImage, rankLevel2, atlasName);
				this.SetChangeText.text = text;
				this.SetRangeText.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_CurrentRound_Group", new object[] { infoByID2 });
				this.animator.Play(string.Format("Anim_UIFX_RankChange_SetRank_{0}", rankLevel2));
			}
			else if (groupType == 2)
			{
				this.SetGroupObject.SetActiveSafe(false);
				this.SameGroupObject.SetActiveSafe(true);
				this.ChangeGroupObject.SetActiveSafe(false);
				this._flyTrans = this.SameImageTrans;
				this.SameRangeUpText.text = text;
				this.SameRangeDownText.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_CurrentRound_Group", new object[] { infoByID2 });
				int atlasName2 = worldBoss_Subsection2.atlasName;
				this.SameImage.SetImage(atlasName2, worldBoss_Subsection2.atlasId);
				this.SetImageMask(this.SameMaskImage, rankLevel2, atlasName2);
				this.animator.Play(string.Format("Anim_UIFX_RankChange_SameRank_{0}", rankLevel2));
				this._animTime = 1.5f;
			}
			else if (groupType == 3 || groupType == 4)
			{
				this.SetGroupObject.SetActiveSafe(false);
				this.SameGroupObject.SetActiveSafe(false);
				this.ChangeGroupObject.SetActiveSafe(true);
				this._animTime = 2f;
				this._flyTime = 0.4f;
				this._flyTrans = this.ChangeImageTrans;
				this.ChangeLastText.text = text;
				this.ChangeCurText.text = Singleton<LanguageManager>.Instance.GetInfoByID((groupType == 3) ? "WorldBoss_Rank_PromotionUpped" : "WorldBoss_Rank_PromotionDowned", new object[] { infoByID2 });
				this.ChangeLastImage.enabled = worldBoss_Subsection != null;
				if (worldBoss_Subsection != null)
				{
					this.ChangeLastImage.SetImage(worldBoss_Subsection.atlasName, worldBoss_Subsection.atlasId);
				}
				int atlasName3 = worldBoss_Subsection2.atlasName;
				this.ChangeCurImage.SetImage(atlasName3, worldBoss_Subsection2.atlasId);
				this.SetImageMask(this.ChangeMaskImage, rankLevel2, atlasName3);
				this.SetImageWhite(this.ChangeWImage, rankLevel2, atlasName3);
				this.ArrowImage.color = ((groupType == 3) ? this.UpgradeColor : this.DowngradeColor);
				if (groupType == 3)
				{
					this.animator.Play("Anim_UIFX_RankChange_ChangeRank_1to2");
				}
				else
				{
					this.animator.Play("Anim_UIFX_RankChange_ChangeRank_2to1");
				}
			}
			if (this._flyTrans != null)
			{
				this._startPos = this._flyTrans.position;
				this._startScale = this._flyTrans.sizeDelta;
			}
			this._btnCoroutine = base.StartCoroutine(this.SetButtonCoroutine());
		}

		private void SetImageMask(CustomImage customImage, int level, int atlas)
		{
			string text = "icon_level_01_Mask";
			if (level >= 11)
			{
				text = "icon_level_11_Mask";
			}
			else if (level >= 8)
			{
				text = "icon_level_08_Mask";
			}
			else if (level >= 5)
			{
				text = "icon_level_05_Mask";
			}
			else if (level >= 2)
			{
				text = "icon_level_02_Mask";
			}
			customImage.SetImage(atlas, text);
		}

		private void SetImageWhite(CustomImage customImage, int level, int atlas)
		{
			string text = "icon_level_01_W";
			if (level >= 11)
			{
				text = "icon_level_11_W";
			}
			else if (level >= 8)
			{
				text = "icon_level_08_W";
			}
			else if (level >= 5)
			{
				text = "icon_level_05_W";
			}
			else if (level >= 2)
			{
				text = "icon_level_02_W";
			}
			customImage.SetImage(atlas, text);
		}

		private IEnumerator SetButtonCoroutine()
		{
			yield return new WaitForSeconds(this._animTime);
			this.TapToCloseCtrl.gameObject.SetActiveSafe(true);
			this.TapToCloseCtrl.OnClose = delegate
			{
				this.TapToCloseCtrl.gameObject.SetActiveSafe(false);
				this._closeCoroutine = base.StartCoroutine(this.CloseAnimCoroutine());
			};
			yield break;
		}

		private IEnumerator CloseAnimCoroutine()
		{
			if (this._flyTrans != null)
			{
				AnimationCurve jumpCurve = GameApp.UnityGlobal.Curve.GetAnimationCurve(100005);
				while (this._flyTime > this._timer)
				{
					this._timer += Time.deltaTime;
					Vector3 vector = Vector3.Lerp(this._startPos, this._openData.FlyTarget, this._timer / this._flyTime);
					vector.x += jumpCurve.Evaluate(this._timer / this._flyTime) * 0.03f;
					this._flyTrans.position = vector;
					this._flyTrans.sizeDelta = Vector3.Lerp(this._startScale, this._openData.ScaleTarget, this._timer / this._flyTime);
					yield return new WaitForEndOfFrame();
				}
				jumpCurve = null;
			}
			GameApp.View.CloseView(ViewName.WorldBossAnimViewModule, null);
			yield break;
		}

		public override void OnClose()
		{
			if (this._btnCoroutine != null)
			{
				base.StopCoroutine(this._btnCoroutine);
			}
			if (this._closeCoroutine != null)
			{
				base.StopCoroutine(this._closeCoroutine);
			}
			this._openData.OnClose();
			this._openData = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public Animator animator;

		public GameObject SetGroupObject;

		public GameObject SameGroupObject;

		public GameObject ChangeGroupObject;

		public RectTransform SetImageTrans;

		public CustomImage SetImage;

		public CustomImage SetMaskImage;

		public CustomImage SetWImage;

		public CustomText SetRangeText;

		public CustomText SetChangeText;

		public CustomText SameRangeUpText;

		public CustomText SameRangeDownText;

		public RectTransform SameImageTrans;

		public CustomImage SameImage;

		public CustomImage SameMaskImage;

		public RectTransform ChangeImageTrans;

		public CustomText ChangeLastText;

		public CustomText ChangeCurText;

		public CustomImage ChangeLastImage;

		public CustomImage ChangeCurImage;

		public CustomImage ChangeMaskImage;

		public CustomImage ChangeWImage;

		public CustomImage ArrowImage;

		public Color UpgradeColor;

		public Color DowngradeColor;

		[Header("资源")]
		public SpriteRegister spriteRegister;

		private float _animTime;

		private float _flyTime = 0.5f;

		private RectTransform _flyTrans;

		private Coroutine _btnCoroutine;

		private Coroutine _closeCoroutine;

		public TapToCloseCtrl TapToCloseCtrl;

		private WorldBossAnimView.OpenData _openData;

		private float _timer;

		private Vector3 _startPos;

		private Vector3 _startScale;

		public class OpenData
		{
			public Action OnClose;

			public Vector3 FlyTarget;

			public Vector2 ScaleTarget;

			public int GroupType;

			public int RankLevel;

			public int LastRank;

			public int LastRankLevel;
		}
	}
}
