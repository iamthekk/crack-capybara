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
	public class UnlockSkillViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.cloneNode.SetActiveSafe(false);
			this.buttonContinue.onClick.AddListener(new UnityAction(this.OnClickContinue));
			this.animatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationEvent));
			this.skillEffectObj.SetActiveSafe(false);
			this.starEffectObj.SetActiveSafe(false);
			this.flashEffectObj.SetActiveSafe(false);
			this.unlockSkillItem.Init();
			string text = Singleton<LanguageManager>.Instance.GetInfoByID("unlcok_skill_info");
			while (text.Length != 0)
			{
				string text2;
				string text3;
				this.SplitString(text, '[', false, out text2, out text3);
				if (!string.IsNullOrEmpty(text2))
				{
					this.infoList.Add(text2);
				}
				this.SplitString(text3, ']', true, out text2, out text3);
				if (!string.IsNullOrEmpty(text2))
				{
					this.infoList.Add(text2);
				}
				text = text3;
			}
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.skillBuildData = data as GameEventSkillBuildData;
			if (this.skillBuildData == null)
			{
				return;
			}
			this.unlockSkillItem.Refresh(this.skillBuildData);
			this.CreateInfo();
			GameApp.Sound.PlayClip(66, 1f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
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
			int num = 0;
			GameObject gameObject = Object.Instantiate<GameObject>(this.layoutNode);
			gameObject.SetParentNormal(this.infoChild, false);
			UnlockInfoLayoutItem unlockInfoLayoutItem = gameObject.GetComponent<UnlockInfoLayoutItem>();
			unlockInfoLayoutItem.Init();
			unlockInfoLayoutItem.Refresh(num);
			this.infoItemList.Add(unlockInfoLayoutItem);
			for (int i = 0; i < this.infoList.Count; i++)
			{
				string text = this.infoList[i];
				if (text.Equals("[SKILL]"))
				{
					int[] compose = this.skillBuildData.config.compose;
					for (int j = 0; j < compose.Length; j++)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(this.skillNode);
						gameObject2.SetParentNormal(gameObject, false);
						UnlockInfoSkillItem component = gameObject2.GetComponent<UnlockInfoSkillItem>();
						component.Init();
						component.Refresh(compose[j]);
						this.infoItemList.Add(component);
					}
				}
				else if (text.Equals("[STAR]"))
				{
					for (int k = 0; k < 3; k++)
					{
						GameObject gameObject3 = Object.Instantiate<GameObject>(this.starNode);
						gameObject3.SetParentNormal(gameObject, false);
						UnlockInfoStarItem component2 = gameObject3.GetComponent<UnlockInfoStarItem>();
						component2.Init();
						component2.Refresh(k);
						this.infoItemList.Add(component2);
					}
				}
				else if (text.Equals("[NL]"))
				{
					num++;
					gameObject = Object.Instantiate<GameObject>(this.layoutNode);
					gameObject.SetParentNormal(this.infoChild, false);
					unlockInfoLayoutItem = gameObject.GetComponent<UnlockInfoLayoutItem>();
					unlockInfoLayoutItem.Init();
					unlockInfoLayoutItem.Refresh(num);
					this.infoItemList.Add(unlockInfoLayoutItem);
				}
				else
				{
					GameObject gameObject4 = Object.Instantiate<GameObject>(this.textNode);
					gameObject4.SetParentNormal(gameObject, false);
					UnlockInfoTextItem component3 = gameObject4.GetComponent<UnlockInfoTextItem>();
					component3.Init();
					component3.Refresh(text);
					this.infoItemList.Add(component3);
				}
			}
		}

		private void OnClickContinue()
		{
			if (this.isPlayAni)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.UnlockSkillViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, null);
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
	}
}
