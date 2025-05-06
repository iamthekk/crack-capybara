using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class GameEventSlotSkillViewModule : BaseViewModule
	{
		private float Speed
		{
			get
			{
				return (float)this.SkillGridHeight / 0.1f;
			}
		}

		public override void OnCreate(object data)
		{
			this.rewardPageGo.SetActiveSafe(false);
			this.slotPageGo.SetActiveSafe(false);
			this.imageIconNode.SetActiveSafe(false);
			this.cloneNode.SetActiveSafe(false);
			this.buttonContinue.onClick.AddListener(new UnityAction(this.OnClickContinue));
			this.animatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationEvent));
			this.skillEffectObj.SetActiveSafe(false);
			this.starEffectObj.SetActiveSafe(false);
			this.flashEffectObj.SetActiveSafe(false);
			this.unlockSkillItem.SetData(new Action(this.OnClickContinue));
			this.unlockSkillItem.Init();
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as GameEventSlotSkillViewModule.OpenData;
			if (this.openData == null)
			{
				return;
			}
			this.skillBuildData = this.openData.skillBuild;
			if (this.openData.noSlot)
			{
				this.EndSkillSlot();
				return;
			}
			this.StartSlot();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isSkillSlot)
			{
				Vector3 localPosition = this.skillLayout.transform.localPosition;
				localPosition.y -= this.currentSpeed * deltaTime;
				if (localPosition.y <= 0f)
				{
					if (!this.isResetIcon)
					{
						this.isResetIcon = true;
						this.skillIconList[this.skillIconList.Count - 1].sprite = this.skillIconList[0].sprite;
					}
					localPosition.y = (float)(this.SkillGridHeight * 3);
				}
				this.skillLayout.transform.localPosition = localPosition;
				if (this.isStopSkillSlot)
				{
					if (this.endY > localPosition.y)
					{
						return;
					}
					float num = Mathf.Abs(this.endY - localPosition.y) + (float)this.SkillGridHeight;
					float num2 = Mathf.Abs((float)this.SkillGridHeight / num);
					num2 = Mathf.Clamp01(num2);
					this.currentSpeed = (1f - num2) * this.Speed;
					if (this.currentSpeed <= 0.1f)
					{
						this.EndSkillSlot();
					}
				}
			}
		}

		public override void OnClose()
		{
			if (this.openData != null && this.openData.viewCloseCallback != null)
			{
				this.openData.viewCloseCallback();
			}
		}

		public override void OnDelete()
		{
			this.unlockSkillItem.DeInit();
			this.effectStar.Clear();
			this.buttonContinue.onClick.RemoveListener(new UnityAction(this.OnClickContinue));
			for (int i = 0; i < this.infoItemList.Count; i++)
			{
				this.infoItemList[i].DeInit();
			}
			this.infoItemList.Clear();
			this.infoList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void StartSlot()
		{
			this.rewardPageGo.SetActiveSafe(false);
			this.slotPageGo.SetActiveSafe(true);
			GameApp.Sound.PlayClip(682, 1f);
			this.iconObj.SetActiveSafe(false);
			this.textSkillInfo.text = "";
			this.skillSlotObj.SetActiveSafe(true);
			this.CreateSkillSlot();
			this.PlaySkillAni();
		}

		private void PlaySkillAni()
		{
			Sequence sequence = this.sequencePool.Get();
			this.skillSlotObj.transform.localScale = Vector3.zero;
			Color color = this.imageMask.color;
			float a = color.a;
			color.a = 0f;
			this.imageMask.color = color;
			color = this.textSkillInfo.color;
			color.a = 0f;
			this.textSkillInfo.color = color;
			float num = 0.2f;
			float num2 = 0.1f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.skillSlotObj.transform, Vector3.one * 1.2f, num));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.skillSlotObj.transform, Vector3.one, num2));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.imageMask, a, num2));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textSkillInfo, 1f, num2));
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.StartSkillSlot));
		}

		private void StartSkillSlot()
		{
			this.currentSpeed = this.Speed;
			this.isResetIcon = false;
			this.isStopSkillSlot = false;
			this.isSkillSlot = true;
			DelayCall.Instance.CallOnce(500, delegate
			{
				this.isStopSkillSlot = true;
			});
		}

		private void EndSkillSlot()
		{
			this.isSkillSlot = false;
			Vector3 localPosition = this.skillLayout.transform.localPosition;
			localPosition.y = this.endY;
			this.skillLayout.transform.localPosition = localPosition;
			Sequence sequence = this.sequencePool.Get();
			float num = 0.1f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOMove(this.skillSlotObj.transform, this.unlockSkillItem.transform.position, num, false));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.skillSlotObj.transform, Vector3.one * 3f, num));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.skillSlotObj.SetActiveSafe(false);
				this.rewardPageGo.SetActiveSafe(true);
				this.slotPageGo.SetActiveSafe(false);
				this.RewardSkillPlay();
			});
		}

		private void CreateSkillSlot()
		{
			SkillBuildSourceType skillBuildSourceType = SkillBuildSourceType.SlotNormal;
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(skillBuildSourceType, 3, this.openData.seed);
			bool flag = false;
			int num = 0;
			for (int i = 0; i < randomSkillList.Count; i++)
			{
				if (this.openData.skillBuild.id == randomSkillList[i].id)
				{
					num = i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int num2 = Random.Range(0, randomSkillList.Count);
				randomSkillList[num2] = this.openData.skillBuild;
				num = num2;
			}
			for (int j = 0; j < randomSkillList.Count; j++)
			{
				CustomImage customImage = this.CreateIcon();
				customImage.SetImage(randomSkillList[j].skillAtlas, randomSkillList[j].skillIcon);
				this.skillIconList.Add(customImage);
			}
			CustomImage customImage2 = this.CreateIcon();
			this.skillIconList.Add(customImage2);
			this.endY = (float)(this.SkillGridHeight * num);
			Vector3 localPosition = this.skillLayout.transform.localPosition;
			localPosition.y = (float)(this.SkillGridHeight * 3);
			this.skillLayout.transform.localPosition = localPosition;
		}

		private CustomImage CreateIcon()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.imageIconNode);
			gameObject.transform.SetParentNormal(this.skillLayout.transform, false);
			gameObject.gameObject.SetActiveSafe(true);
			return gameObject.GetComponent<CustomImage>();
		}

		private void RewardSkillPlay()
		{
			if (this.skillBuildData == null)
			{
				return;
			}
			this.unlockSkillItem.Refresh(this.skillBuildData);
			this.CreateInfo();
			GameApp.Sound.PlayClip(66, 1f);
		}

		private void SplitString(string str, char op, bool includeOp, out string left, out string right)
		{
			if (str.Contains(op.ToString()))
			{
				int num = str.IndexOf(op);
				left = str.Substring(0, includeOp ? (num + 1) : num);
				right = str.Substring(includeOp ? (num + 1) : num);
				return;
			}
			left = str;
			right = "";
		}

		private void CreateInfo()
		{
		}

		private void OnClickContinue()
		{
			if (this.isPlayAni)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.GameEventSlotSkillViewModule, null);
			Singleton<GameEventController>.Instance.SelectSkill(this.skillBuildData, false);
		}

		private void OnAnimationEvent(GameObject obj, string param)
		{
			if (param.Equals("StartEffectAni"))
			{
				this.skillEffectObj.SetActiveSafe(true);
				return;
			}
			if (param.Equals("StartEffectFlashAni"))
			{
				this.flashEffectObj.SetActiveSafe(true);
				return;
			}
			if (param.Equals("StartEffectStar"))
			{
				this.starEffectObj.SetActiveSafe(true);
				this.effectStar.Play();
				return;
			}
			if (param.Equals("Finish"))
			{
				this.isPlayAni = false;
				return;
			}
			if (param.Equals("Start"))
			{
				this.isPlayAni = true;
			}
		}

		[GameTestMethod("事件小游戏", "技能老虎机", "", 101)]
		private static void OpenSkillSlot()
		{
			List<GameEventSkillBuildData> randomSkillList = Singleton<GameEventController>.Instance.GetRandomSkillList(SkillBuildSourceType.Normal, 1, 100);
			GameEventSlotSkillViewModule.OpenData openData = new GameEventSlotSkillViewModule.OpenData();
			openData.skillBuild = randomSkillList[0];
			openData.seed = 100;
			GameApp.View.OpenView(ViewName.GameEventSlotSkillViewModule, openData, 1, null, null);
		}

		public GameEventSlotSkillViewModule.OpenData openData;

		public GameObject rewardPageGo;

		public GameObject slotPageGo;

		public GameObject infoChild;

		public GameObject cloneNode;

		public GameObject layoutNode;

		public GameObject textNode;

		public GameObject skillNode;

		public GameObject starNode;

		public CustomButton buttonContinue;

		public AnimatorListen animatorListen;

		public Animator effectAni;

		public Animator flashAni;

		public ParticleSystem effectStar;

		public GameObject skillEffectObj;

		public GameObject flashEffectObj;

		public GameObject starEffectObj;

		public UnlockSkillItem unlockSkillItem;

		private List<string> infoList = new List<string>();

		private List<CustomBehaviour> infoItemList = new List<CustomBehaviour>();

		private GameEventSkillBuildData skillBuildData;

		private bool isPlayAni;

		private const string SKILL_KEY = "[SKILL]";

		private const string STAR_KEY = "[STAR]";

		private const string NEWLINE_KEY = "[NL]";

		private const int MAX_STAR = 3;

		public GameObject iconObj;

		public CustomText textSkillInfo;

		public GameObject skillSlotObj;

		public Image imageMask;

		public GameObject imageIconNode;

		public VerticalLayoutGroup skillLayout;

		private SequencePool sequencePool = new SequencePool();

		private float currentSpeed;

		private int SkillGridHeight = 190;

		private float endY;

		private bool isSkillSlot;

		private bool isStopSkillSlot;

		private bool isResetIcon;

		private List<CustomImage> skillIconList = new List<CustomImage>();

		private const int SKILL_SLOT_STOP_TIME = 500;

		private const int SHOW_SKILLS = 3;

		private const float RESET_Y = 0f;

		public class OpenData
		{
			public GameEventSkillBuildData skillBuild;

			public Action viewCloseCallback;

			public int seed;

			public bool noSlot;
		}
	}
}
