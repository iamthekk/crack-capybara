using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using HotFix.Client;
using LocalModels.Bean;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleDungeonWinViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.dungeonDataModule = GameApp.Data.GetDataModule(DataName.DungeonDataModule);
		}

		public override void OnOpen(object data)
		{
			this.buttonOK.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.rewardData.Clear();
			if (this.dungeonDataModule.DungeonResponse != null)
			{
				this.rewardData = this.dungeonDataModule.DungeonResponse.CommonData.Reward;
				Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(this.dungeonDataModule.DungeonResponse.DungeonId);
				if (elementById != null)
				{
					this.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.name);
				}
				else
				{
					this.textName.text = "";
				}
			}
			if (this.m_rewardPrefab != null)
			{
				this.m_rewardPrefab.SetActive(false);
			}
			this.CreateRewards();
			this.seqPool.Clear(false);
			this.PlayRewards();
			GameApp.Sound.PlayClip(52, 1f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.buttonOK.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.DestroyRewards();
			this.seqPool.Clear(false);
			this.rewardData.Clear();
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

		private void OnClickClose()
		{
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
					instance.SetData(GameOverType.Win);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
					GameApp.View.CloseView(ViewName.BattleDungeonWinViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		private void SetButtonEnabled(bool isEnabled)
		{
			this.buttonOK.enabled = isEnabled;
		}

		private void CreateRewards()
		{
			if (this.rewardData == null || this.rewardData.Count == 0)
			{
				return;
			}
			if (this.m_rewardParent == null)
			{
				return;
			}
			for (int i = 0; i < this.rewardData.Count; i++)
			{
				RewardDto rewardDto = this.rewardData[i];
				if (this.rewardData != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_rewardPrefab);
					gameObject.SetParentNormal(this.m_rewardParent, false);
					gameObject.SetActive(true);
					UIItem component = gameObject.GetComponent<UIItem>();
					component.SetCountShowType(UIItem.CountShowType.ShowAll);
					component.Init();
					component.SetData(rewardDto.ToPropData());
					component.OnRefresh();
					this.rewardDic[gameObject.GetInstanceID()] = component;
				}
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_rewardParent);
		}

		public void DestroyRewards()
		{
			foreach (KeyValuePair<int, UIItem> keyValuePair in this.rewardDic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.rewardDic.Clear();
		}

		private void PlayRewards()
		{
			foreach (KeyValuePair<int, UIItem> keyValuePair in this.rewardDic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.transform.localScale = Vector3.zero;
				}
			}
			this.rewardBgObj.SetActiveSafe(false);
			this.flagObj.transform.localScale = Vector3.zero;
			this.buttonOK.transform.localScale = Vector3.zero;
			Sequence sequence = this.seqPool.Get();
			float num = 0.3f;
			int num2 = 0;
			float num3 = 1.1f;
			float num4 = 0.15f;
			float num5 = 0.05f;
			TweenSettingsExtensions.AppendInterval(sequence, num);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one * num3, num4)), ShortcutExtensions.DOScale(this.flagObj.transform, Vector3.one, num5));
			TweenSettingsExtensions.AppendInterval(sequence, num4 + num5);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.rewardBgObj.SetActiveSafe(true);
			});
			foreach (KeyValuePair<int, UIItem> keyValuePair2 in this.rewardDic)
			{
				if (!(keyValuePair2.Value == null))
				{
					TweenSettingsExtensions.Join(sequence, this.PlayRewardScale(keyValuePair2.Value, num2));
					num2++;
				}
			}
			if (this.rewardDic.Count > 0)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (this.m_rewardParent != null)
					{
						LayoutRebuilder.MarkLayoutForRebuild(this.m_rewardParent);
					}
				});
				TweenSettingsExtensions.AppendInterval(sequence, num4);
			}
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.buttonOK.transform, Vector3.one * num3, num4)), ShortcutExtensions.DOScale(this.buttonOK.transform, Vector3.one, num5));
		}

		private Sequence PlayRewardScale(UIItem item, int index)
		{
			Sequence sequence = this.seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, (float)index * 0.05f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(item.transform, Vector3.one * 1.1f, 0.15f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(item.transform, Vector3.one, 0.05f));
			return sequence;
		}

		[GameTestMethod("副本", "副本胜利", "", 401)]
		private static void OpenDungeonWin()
		{
			GameApp.View.OpenView(ViewName.BattleDungeonViewModule, null, 1, null, null);
		}

		[SerializeField]
		private CustomButton buttonOK;

		[SerializeField]
		private GameObject flagObj;

		[SerializeField]
		private GameObject rewardBgObj;

		[SerializeField]
		private RectTransform m_rewardParent;

		[SerializeField]
		private GameObject m_rewardPrefab;

		[SerializeField]
		private CustomText textName;

		private DungeonDataModule dungeonDataModule;

		private Dictionary<int, UIItem> rewardDic = new Dictionary<int, UIItem>();

		private SequencePool seqPool = new SequencePool();

		private RepeatedField<RewardDto> rewardData = new RepeatedField<RewardDto>();
	}
}
