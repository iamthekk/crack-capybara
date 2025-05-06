using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class RoundSelectSkillViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.copySkillItem.gameObject.SetActiveSafe(false);
			this.copyRoundItem.gameObject.SetActiveSafe(false);
			this.buttonSelect.onClick.AddListener(new UnityAction(this.OnClickSelect));
			this.buttonLearnedSkill.onClick.AddListener(new UnityAction(this.OnClickLearnedSkill));
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			RoundSelectSkillViewModule.OpenData openData = data as RoundSelectSkillViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null)
			{
				this.CloseSelf();
				return;
			}
			this.roundSkillDic.Clear();
			this.roundSeedList.Clear();
			XRandom xrandom = new XRandom(this.mOpenData.Seed);
			for (int i = 0; i < this.mOpenData.TotalRound; i++)
			{
				int num = xrandom.NextInt();
				this.roundSeedList.Add(num);
				UIRoundItem uiroundItem;
				if (i < this.roundItems.Count)
				{
					uiroundItem = this.roundItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyRoundItem);
					gameObject.SetParentNormal(this.roundParent, false);
					uiroundItem = gameObject.GetComponent<UIRoundItem>();
					uiroundItem.Init();
					this.roundItems.Add(uiroundItem);
				}
				uiroundItem.gameObject.SetActiveSafe(true);
				uiroundItem.SetData(i + 1);
			}
			for (int j = 0; j < this.mOpenData.RandomSkillNum; j++)
			{
				if (j < this.skillItems.Count)
				{
					UIRoundSelectSkillItem uiroundSelectSkillItem = this.skillItems[j];
				}
				else
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.copySkillItem);
					gameObject2.SetParentNormal(this.skillParent, false);
					UIRoundSelectSkillItem uiroundSelectSkillItem = gameObject2.GetComponent<UIRoundSelectSkillItem>();
					uiroundSelectSkillItem.Init();
					this.skillItems.Add(uiroundSelectSkillItem);
				}
			}
			this.currentRound.UpdateVariable(1);
			this.RandomSkill();
			this.RefreshInfo();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			for (int i = 0; i < this.roundItems.Count; i++)
			{
				this.roundItems[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < this.skillItems.Count; j++)
			{
				this.skillItems[j].gameObject.SetActiveSafe(false);
			}
			this.selectSkills.Clear();
			this.roundSkillDic.Clear();
		}

		public override void OnDelete()
		{
			this.buttonSelect.onClick.RemoveListener(new UnityAction(this.OnClickSelect));
			this.buttonLearnedSkill.onClick.RemoveListener(new UnityAction(this.OnClickLearnedSkill));
			for (int i = 0; i < this.roundItems.Count; i++)
			{
				this.roundItems[i].DeInit();
			}
			this.roundItems.Clear();
			for (int j = 0; j < this.skillItems.Count; j++)
			{
				this.skillItems[j].DeInit();
			}
			this.skillItems.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void RefreshInfo()
		{
			this.textSelect.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiroundselectskill_select", new object[]
			{
				this.selectSkills.Count,
				this.mOpenData.SelectSkillNum
			});
			for (int i = 0; i < this.roundItems.Count; i++)
			{
				this.roundItems[i].Refresh(this.currentRound.mVariable);
			}
		}

		private void RandomSkill()
		{
			this.popAni.Clear();
			int num = 0;
			int num2 = this.currentRound.mVariable - 1;
			if (num2 >= 0 && num2 < this.roundSeedList.Count)
			{
				num = this.roundSeedList[num2];
			}
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(this.mOpenData.SourceType, this.mOpenData.RandomSkillNum, num);
			if (randomSkillList.Count > 0)
			{
				GameApp.Sound.PlayClip(GameEventSkillBuildData.GetQualitySoundId(randomSkillList[0].quality), 1f);
			}
			for (int i = 0; i < this.skillItems.Count; i++)
			{
				UIRoundSelectSkillItem uiroundSelectSkillItem = this.skillItems[i];
				if (i < randomSkillList.Count)
				{
					this.popAni.AddData(new PopAnimationSequence.Data
					{
						transform = uiroundSelectSkillItem.transform,
						itemFinish = new Action<GameObject>(this.OnItemAniFinish)
					});
					uiroundSelectSkillItem.gameObject.SetActiveSafe(true);
					uiroundSelectSkillItem.SetData(randomSkillList[i], new Action<GameEventSkillBuildData>(this.OnSelectSkill));
					uiroundSelectSkillItem.Refresh(this.selectSkills);
				}
			}
			GameTGATools.Ins.AddStageClickTempSkillShow(randomSkillList);
			this.popAni.AddData(new PopAnimationSequence.Data
			{
				transform = this.buttonSelect.transform
			});
			this.popAni.Play();
		}

		private void OnSelectSkill(GameEventSkillBuildData data)
		{
			if (this.selectSkills.Contains(data))
			{
				this.selectSkills.Remove(data);
			}
			else if (this.selectSkills.Count < this.mOpenData.SelectSkillNum)
			{
				this.selectSkills.Add(data);
			}
			for (int i = 0; i < this.skillItems.Count; i++)
			{
				this.skillItems[i].Refresh(this.selectSkills);
			}
			this.RefreshInfo();
		}

		private void CloseSelf()
		{
			EventArgSelectSkillEnd instance = Singleton<EventArgSelectSkillEnd>.Instance;
			instance.SetData(null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CloseSelectSkill, instance);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, null);
			GameApp.View.CloseView(ViewName.RoundSelectSkillViewModule, null);
		}

		private void OnClickSelect()
		{
			if (!this.currentRound.IsDataValid())
			{
				Action<Dictionary<int, int[]>> onSaveSkillAction = this.mOpenData.OnSaveSkillAction;
				if (onSaveSkillAction != null)
				{
					onSaveSkillAction(this.roundSkillDic);
				}
				this.CloseSelf();
				return;
			}
			if (this.selectSkills.Count < this.mOpenData.SelectSkillNum)
			{
				int num = this.mOpenData.SelectSkillNum - this.selectSkills.Count;
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uiroundselectskill_select_tip", new object[] { num });
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			int[] array = new int[this.selectSkills.Count];
			for (int i = 0; i < this.selectSkills.Count; i++)
			{
				array[i] = this.selectSkills[i].id;
				Singleton<GameEventController>.Instance.SelectSkill(this.selectSkills[i], false);
			}
			GameTGATools.Ins.AddStageClickTempSkillSelect(this.selectSkills, true);
			this.roundSkillDic.Add(this.currentRound.mVariable, array);
			this.selectSkills.Clear();
			if (!this.currentRound.IsDataValid() || this.currentRound.mVariable >= this.mOpenData.TotalRound)
			{
				Action<Dictionary<int, int[]>> onSaveSkillAction2 = this.mOpenData.OnSaveSkillAction;
				if (onSaveSkillAction2 != null)
				{
					onSaveSkillAction2(this.roundSkillDic);
				}
				this.CloseSelf();
				return;
			}
			this.currentRound.UpdateVariable(this.currentRound.mVariable + 1);
			this.RandomSkill();
			this.RefreshInfo();
		}

		private void OnClickLearnedSkill()
		{
			GameApp.View.OpenView(ViewName.LearnedSkillsViewModule, null, 1, null, null);
		}

		private void OnItemAniFinish(GameObject obj)
		{
			UIRoundSelectSkillItem component = obj.GetComponent<UIRoundSelectSkillItem>();
			if (component)
			{
				component.PlayEffect();
			}
		}

		private void PlayPopAni()
		{
			Sequence sequence = DOTween.Sequence();
			for (int i = 0; i < this.skillItems.Count; i++)
			{
				Transform trans = this.skillItems[i].transform;
				Sequence sequence2 = DOTween.Sequence();
				TweenSettingsExtensions.AppendCallback(sequence2, delegate
				{
					trans.localScale = Vector3.one * 0.5f;
				});
				TweenSettingsExtensions.Append(sequence2, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(trans, Vector3.one, 0.15f), 27));
				TweenSettingsExtensions.AppendCallback(sequence2, delegate
				{
					this.OnItemAniFinish(trans.gameObject);
				});
				if (i == 0)
				{
					TweenSettingsExtensions.Join(sequence, sequence2);
				}
				else
				{
					TweenSettingsExtensions.AppendInterval(sequence, 0.01f);
					TweenSettingsExtensions.Join(sequence, sequence2);
				}
			}
		}

		public GameObject skillParent;

		public GameObject roundParent;

		public GameObject copySkillItem;

		public GameObject copyRoundItem;

		public CustomText textSelect;

		public CustomButton buttonSelect;

		public CustomButton buttonLearnedSkill;

		public PopAnimationSequence popAni;

		private RoundSelectSkillViewModule.OpenData mOpenData;

		private List<int> roundSeedList = new List<int>();

		private SecureVariable currentRound = new SecureVariable();

		private List<UIRoundItem> roundItems = new List<UIRoundItem>();

		private List<UIRoundSelectSkillItem> skillItems = new List<UIRoundSelectSkillItem>();

		private List<GameEventSkillBuildData> selectSkills = new List<GameEventSkillBuildData>();

		private Dictionary<int, int[]> roundSkillDic = new Dictionary<int, int[]>();

		public class OpenData
		{
			public int Seed;

			public SkillBuildSourceType SourceType;

			public int TotalRound;

			public int RandomSkillNum;

			public int SelectSkillNum;

			public Action<Dictionary<int, int[]>> OnSaveSkillAction;
		}
	}
}
