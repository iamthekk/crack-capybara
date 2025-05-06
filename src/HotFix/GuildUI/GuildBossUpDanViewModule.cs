using System;
using System.Collections;
using System.Text;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildBossUpDanViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			if (data == null)
			{
				return;
			}
			this._openData = (GuildBossUpDanViewModule.OpenData)data;
			if (this._openData == null)
			{
				return;
			}
			this._timer = 0f;
			int lastRank = this._openData.LastRank;
			int rank = this._openData.Rank;
			int guildDan = this._openData.GuildDan;
			string guildBossSeason = PlayerPrefsKeys.GetGuildBossSeason();
			string guildBossDan = PlayerPrefsKeys.GetGuildBossDan();
			if (string.IsNullOrEmpty(guildBossDan))
			{
				GuildProxy.UI.CloseUIGuildBossUpDan();
				return;
			}
			int num = -1;
			long userId = GameApp.Data.GetDataModule(DataName.LoginDataModule).userId;
			string[] array = guildBossDan.Split('|', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				if (long.Parse(array[i].Split('_', StringSplitOptions.None)[0]) == userId)
				{
					num = int.Parse(array[i].Split('_', StringSplitOptions.None)[1]);
					break;
				}
			}
			if (num == -1)
			{
				GuildProxy.UI.CloseUIGuildBossUpDan();
				return;
			}
			this.m_stringBuilder.Clear();
			GuildBOSS_guildSubection guildBossDanTable = GuildProxy.Table.GetGuildBossDanTable(guildDan);
			if (GuildProxy.Table.GetGuildBossDanTable(num) == null || guildBossDanTable == null)
			{
				GuildProxy.UI.CloseUIGuildBossUpDan();
				return;
			}
			if (this._flyTrans != null)
			{
				this._flyTrans.position = this._startPos;
				this._flyTrans.sizeDelta = this._startScale;
			}
			if (guildDan == num)
			{
				this.SetGroupObject.SetActiveSafe(false);
				this.SameGroupObject.SetActiveSafe(true);
				this.ChangeGroupObject.SetActiveSafe(false);
				if (lastRank > 0)
				{
					this.SameRankChangeText.text = GuildProxy.Language.GetInfoByID2("guild_boss_16", GuildProxy.Language.GetInfoByID(guildBossDanTable.languageId), lastRank);
				}
				else
				{
					this.SameRankChangeText.text = GuildProxy.Language.GetInfoByID("WorldBoss_LastRound_NotChallenge");
				}
				this.SameRankSetText.text = GuildProxy.Language.GetInfoByID1("guild_boss_12", guildBossDanTable.languageId);
				this._flyTrans = this.SameRankCurDanRect;
				this.SameRankCurDan.SetData(guildDan);
				this.SetImageMask(this.SameMaskImage, guildDan);
				this.animator.Play("Anim_UIFX_RankChange_SameRank_1");
				this._animTime = 1.5f;
			}
			else if (guildDan < num)
			{
				this.SetGroupObject.SetActiveSafe(false);
				this.SameGroupObject.SetActiveSafe(false);
				this.ChangeGroupObject.SetActiveSafe(true);
				this._animTime = 2f;
				this._flyTime = 0.4f;
				this._flyTrans = this.ChangeRankCurDanRect;
				this.ChangeChangeText.text = GuildProxy.Language.GetInfoByID1("guild_boss_14", GuildProxy.Language.GetInfoByID(guildBossDanTable.languageId));
				this.ChangeCurText.text = GuildProxy.Language.GetInfoByID1("guild_boss_12", guildBossDanTable.languageId);
				this.ChangeRankLastDan.SetData(num);
				this.ChangeRankCurDan.SetData(guildDan);
				this.SetImageMask(this.ChangeMaskImage, guildDan);
				this.SetImageWhite(this.ChangeWhiteImage, guildDan);
				this.animator.Play("Anim_UIFX_RankChange_ChangeRank_2to1");
				this.ChangeArrowImage.color = this.DowngradeColor;
			}
			else if (guildDan > num)
			{
				this.SetGroupObject.SetActiveSafe(false);
				this.SameGroupObject.SetActiveSafe(false);
				this.ChangeGroupObject.SetActiveSafe(true);
				this._animTime = 2f;
				this._flyTime = 0.4f;
				this._flyTrans = this.ChangeRankCurDanRect;
				this.ChangeChangeText.text = GuildProxy.Language.GetInfoByID1("guild_boss_15", GuildProxy.Language.GetInfoByID(guildBossDanTable.languageId));
				this.ChangeCurText.text = GuildProxy.Language.GetInfoByID1("guild_boss_12", guildBossDanTable.languageId);
				this.ChangeRankLastDan.SetData(num);
				this.ChangeRankCurDan.SetData(guildDan);
				this.SetImageMask(this.ChangeMaskImage, guildDan);
				this.SetImageWhite(this.ChangeWhiteImage, guildDan);
				this.animator.Play("Anim_UIFX_RankChange_ChangeRank_1to2");
				this.ChangeArrowImage.color = this.UpgradeColor;
			}
			string[] array2 = guildBossSeason.Split('|', StringSplitOptions.None);
			for (int j = 0; j < array2.Length; j++)
			{
				if (long.Parse(array2[j].Split('_', StringSplitOptions.None)[0]) == userId)
				{
					if (j == array2.Length - 1)
					{
						this.m_stringBuilder.Append(string.Format("{0}_{1}", userId, this._openData.GuildSeason));
					}
					else
					{
						this.m_stringBuilder.Append(string.Format("{0}_{1}|", userId, this._openData.GuildSeason));
					}
				}
				else if (j == array2.Length - 1)
				{
					this.m_stringBuilder.Append(HLog.StringBuilder(new string[] { array2[j] }));
				}
				else
				{
					this.m_stringBuilder.Append(HLog.StringBuilder(array2[j], "|"));
				}
			}
			PlayerPrefsKeys.SetGuildBossSeason(this.m_stringBuilder.ToString());
			this.m_stringBuilder.Clear();
			string[] array3 = guildBossDan.Split('|', StringSplitOptions.None);
			for (int k = 0; k < array3.Length; k++)
			{
				if (long.Parse(array3[k].Split('_', StringSplitOptions.None)[0]) == userId)
				{
					if (k == array3.Length - 1)
					{
						this.m_stringBuilder.Append(string.Format("{0}_{1}", userId, this._openData.GuildDan));
					}
					else
					{
						this.m_stringBuilder.Append(string.Format("{0}_{1}|", userId, this._openData.GuildDan));
					}
				}
				else if (k == array3.Length - 1)
				{
					this.m_stringBuilder.Append(HLog.StringBuilder(new string[] { array3[k] }));
				}
				else
				{
					this.m_stringBuilder.Append(HLog.StringBuilder(array3[k], "|"));
				}
			}
			PlayerPrefsKeys.SetGuildBossDan(this.m_stringBuilder.ToString());
			if (this._flyTrans != null)
			{
				this._startPos = this._flyTrans.position;
				this._startScale = this._flyTrans.sizeDelta;
			}
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnOpenTapToCloseCtrl));
			DelayCall.Instance.CallOnce((int)this._animTime * 1000, new DelayCall.CallAction(this.OnOpenTapToCloseCtrl));
		}

		private void OnOpenTapToCloseCtrl()
		{
			this.TapToCloseCtrl.gameObject.SetActiveSafe(true);
			this.TapToCloseCtrl.OnClose = delegate
			{
				this.TapToCloseCtrl.gameObject.SetActiveSafe(false);
				this._closeCoroutine = base.StartCoroutine(this.CloseAnimCoroutine());
			};
		}

		private void SetImageMask(CustomImage customImage, int dan)
		{
			GuildBOSS_guildSubection guildBossDanTable = GuildProxy.Table.GetGuildBossDanTable(dan);
			if (guildBossDanTable == null)
			{
				return;
			}
			customImage.SetImage(guildBossDanTable.atlasId, string.Format("{0}_Mask", guildBossDanTable.atlasName));
		}

		private void SetImageWhite(CustomImage customImage, int dan)
		{
			GuildBOSS_guildSubection guildBossDanTable = GuildProxy.Table.GetGuildBossDanTable(dan);
			if (guildBossDanTable == null)
			{
				return;
			}
			customImage.SetImage(guildBossDanTable.atlasId, string.Format("{0}_W", guildBossDanTable.atlasName));
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
			GuildProxy.UI.CloseUIGuildBossUpDan();
			yield break;
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			if (this._btnCoroutine != null)
			{
				base.StopCoroutine(this._btnCoroutine);
			}
			if (this._closeCoroutine != null)
			{
				base.StopCoroutine(this._closeCoroutine);
			}
			GuildBossUpDanViewModule.OpenData openData = this._openData;
			if (openData != null)
			{
				Action onClose = openData.OnClose;
				if (onClose != null)
				{
					onClose();
				}
			}
			this._openData = null;
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OnOpenTapToCloseCtrl));
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnViewUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
		}

		public Animator animator;

		[Header("SetGroup")]
		public GameObject SetGroupObject;

		public CustomText SetGroupRangeText;

		public CustomText SetGroupChangeText;

		public RectTransform SetBackTrans;

		public CustomImage SetBackImage;

		public CustomImage SetMaskImage;

		public CustomImage SetWhiteImage;

		[Header("changeGroup")]
		public GameObject ChangeGroupObject;

		public RectTransform ChangeRankCurDanRect;

		public CustomText ChangeChangeText;

		public CustomText ChangeCurText;

		public UIGuildDan ChangeRankCurDan;

		public UIGuildDan ChangeRankLastDan;

		public CustomImage ChangeMaskImage;

		public CustomImage ChangeWhiteImage;

		public CustomImage ChangeArrowImage;

		[Header("sameGroup")]
		public GameObject SameGroupObject;

		public CustomText SameRankChangeText;

		public CustomText SameRankSetText;

		public RectTransform SameRankCurDanRect;

		public UIGuildDan SameRankCurDan;

		public CustomImage SameMaskImage;

		public Color UpgradeColor;

		public Color DowngradeColor;

		private float _animTime;

		private float _flyTime = 0.5f;

		private RectTransform _flyTrans;

		private Coroutine _btnCoroutine;

		private Coroutine _closeCoroutine;

		public TapToCloseCtrl TapToCloseCtrl;

		private StringBuilder m_stringBuilder = new StringBuilder();

		private GuildBossUpDanViewModule.OpenData _openData;

		private float _timer;

		private Vector3 _startPos;

		private Vector3 _startScale;

		public class OpenData
		{
			public Action OnClose;

			public Vector3 FlyTarget;

			public Vector2 ScaleTarget;

			public int Rank;

			public int LastRank;

			public int GuildDan;

			public int GuildSeason;
		}
	}
}
