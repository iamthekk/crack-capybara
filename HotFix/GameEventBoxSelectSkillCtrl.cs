using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class GameEventBoxSelectSkillCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.skillItemObj.SetActiveSafe(false);
			this.textSelectTip.gameObject.SetActiveSafe(false);
			this.buttonLearnedSkills.gameObject.SetActive(false);
			for (int i = 0; i < this.skillGroup.transform.childCount; i++)
			{
				this.nodes.Add(this.skillGroup.transform.GetChild(i).gameObject);
			}
			this.buttonLearnedSkills.onClick.AddListener(new UnityAction(this.OnClickLearnedSkill));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.buttonLearnedSkills.onClick.RemoveListener(new UnityAction(this.OnClickLearnedSkill));
			for (int i = 0; i < this.selectSkillItems.Count; i++)
			{
				this.selectSkillItems[i].DeInit();
			}
			this.selectSkillItems.Clear();
			this.nodes.Clear();
			this.sequencePool.Clear(false);
			this.selectSkill = null;
		}

		public void Refresh(List<GameEventSkillBuildData> skills, Action<GameEventSkillBuildData> onSelectSkill)
		{
			if (skills == null)
			{
				return;
			}
			if (skills.Count > this.nodes.Count)
			{
				HLog.LogError("技能数量和父节点数量不一致!");
				return;
			}
			for (int i = 0; i < skills.Count; i++)
			{
				UIGameEventSelectSkillItem uigameEventSelectSkillItem;
				if (i < this.selectSkillItems.Count)
				{
					uigameEventSelectSkillItem = this.selectSkillItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.skillItemObj);
					gameObject.transform.SetParentNormal(this.nodes[i], false);
					uigameEventSelectSkillItem = gameObject.GetComponent<UIGameEventSelectSkillItem>();
					uigameEventSelectSkillItem.Init();
					this.selectSkillItems.Add(uigameEventSelectSkillItem);
				}
				uigameEventSelectSkillItem.gameObject.SetActiveSafe(true);
				uigameEventSelectSkillItem.Refresh(skills[i], onSelectSkill);
			}
			this.textSelectTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("UISelectSkill_76");
		}

		public void PlayAni(Vector3 bornPos, Action onFinish)
		{
			Sequence seq = this.sequencePool.Get();
			this.textSelectTip.gameObject.SetActiveSafe(true);
			Color color = this.textSelectTip.color;
			color.a = 0f;
			this.textSelectTip.color = color;
			this.buttonLearnedSkills.gameObject.SetActive(true);
			this.buttonLearnedSkills.transform.localScale = Vector3.zero;
			this.imageTitle.alpha = 0f;
			for (int i = 0; i < this.selectSkillItems.Count; i++)
			{
				this.selectSkillItems[i].transform.position = bornPos;
				this.selectSkillItems[i].transform.localScale = Vector3.zero;
			}
			for (int j = 0; j < this.selectSkillItems.Count; j++)
			{
				GameObject obj = this.selectSkillItems[j].gameObject;
				TweenSettingsExtensions.AppendInterval(seq, 0.07f);
				TweenSettingsExtensions.AppendCallback(seq, delegate
				{
					TweenSettingsExtensions.Append(seq, this.SkillItemAni(obj));
				});
			}
			TweenSettingsExtensions.Append(seq, ShortcutExtensions46.DOFade(this.imageTitle, 1f, 0.15f));
			TweenSettingsExtensions.Append(seq, ShortcutExtensions46.DOFade(this.textSelectTip, 1f, 0.15f));
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(seq, ShortcutExtensions.DOScale(this.buttonLearnedSkills.transform, Vector3.one * 1.05f, 0.15f)), ShortcutExtensions.DOScale(this.buttonLearnedSkills.transform, Vector3.one, 0.1f));
			TweenSettingsExtensions.AppendCallback(seq, delegate
			{
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		private Sequence SkillItemAni(GameObject itemObj)
		{
			Sequence sequence = DOTween.Sequence();
			float num = 0.3f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMove(itemObj.transform, Vector3.zero, num, false));
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(itemObj.transform, 1.1f, num)), ShortcutExtensions.DOScale(itemObj.transform, 1f, num / 2f));
			return sequence;
		}

		private void OnClickLearnedSkill()
		{
			GameApp.View.OpenView(ViewName.LearnedSkillsViewModule, null, 1, null, null);
		}

		public Transform skillGroup;

		public GameObject skillItemObj;

		public CustomText textSelectTip;

		public CanvasGroup imageTitle;

		public CustomButton buttonLearnedSkills;

		private List<UIGameEventSelectSkillItem> selectSkillItems = new List<UIGameEventSelectSkillItem>();

		private List<GameObject> nodes = new List<GameObject>();

		private SequencePool sequencePool = new SequencePool();

		private GameEventSkillBuildData selectSkill;
	}
}
