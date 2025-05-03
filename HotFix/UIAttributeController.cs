using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIAttributeController : CustomBehaviour
	{
		public bool IsLevelUpAni { get; private set; }

		protected override void OnInit()
		{
			this.chapterDataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.Button_HP.onClick.AddListener(new UnityAction(this.OnClickHP));
			this.Button_Attack.onClick.AddListener(new UnityAction(this.OnClickAttack));
			this.Button_Defence.onClick.AddListener(new UnityAction(this.OnClickDefence));
			this.Button_Gold.onClick.AddListener(new UnityAction(this.OnClickGold));
			this.Button_Exp.onClick.AddListener(new UnityAction(this.OnClickExp));
			this.Button_Day.onClick.AddListener(new UnityAction(this.OnClickDay));
			this.HpAnima = new AttributeHpAnim();
			this.HpAnima.Init(this.tranHpInfo, this.Text_HP);
			this.HpPercentAnima = new AttributeAnim();
			this.HpPercentAnima.Init(this.tranHpInfo, this.Text_HpPercent, true);
			this.AttackAnim = new AttributeAnim();
			this.AttackAnim.Init(this.tranAttackInfo, this.Text_Attack, false);
			this.DefenseAnim = new AttributeAnim();
			this.DefenseAnim.Init(this.tranDefenseInfo, this.Text_Defence, false);
			this.GoldAnim = new AttributeAnim();
			this.GoldAnim.Init(this.tranGoldInfo, this.Text_Gold, false);
			this.hpPercentAni.Play("Idle");
			GameApp.Event.RegisterEvent(372, new HandlerEvent(this.OnEventFlyAttributes));
			this.effectItem.SetActiveSafe(false);
			this.CreateNodeEffects();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			AttributeHpAnim hpAnima = this.HpAnima;
			if (hpAnima != null)
			{
				hpAnima.OnUpdate(deltaTime);
			}
			AttributeAnim hpPercentAnima = this.HpPercentAnima;
			if (hpPercentAnima != null)
			{
				hpPercentAnima.OnUpdate(deltaTime);
			}
			AttributeAnim attackAnim = this.AttackAnim;
			if (attackAnim != null)
			{
				attackAnim.OnUpdate(deltaTime);
			}
			AttributeAnim defenseAnim = this.DefenseAnim;
			if (defenseAnim != null)
			{
				defenseAnim.OnUpdate(deltaTime);
			}
			AttributeAnim goldAnim = this.GoldAnim;
			if (goldAnim == null)
			{
				return;
			}
			goldAnim.OnUpdate(deltaTime);
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(372, new HandlerEvent(this.OnEventFlyAttributes));
			this.Button_HP.onClick.RemoveListener(new UnityAction(this.OnClickHP));
			this.Button_Attack.onClick.RemoveListener(new UnityAction(this.OnClickAttack));
			this.Button_Defence.onClick.RemoveListener(new UnityAction(this.OnClickDefence));
			this.Button_Gold.onClick.RemoveListener(new UnityAction(this.OnClickGold));
			this.Button_Exp.onClick.RemoveListener(new UnityAction(this.OnClickExp));
			this.Button_Day.onClick.RemoveListener(new UnityAction(this.OnClickDay));
			this.DeleteFlyItems();
			foreach (ParticleSystem particleSystem in this.effDic.Values)
			{
				if (particleSystem)
				{
					Object.Destroy(particleSystem.gameObject);
				}
			}
			this.effDic.Clear();
		}

		public void SetHP(long current, long max, bool useAnimation)
		{
			double num = (double)current / (double)max * 100.0;
			if (max > 0L)
			{
				this.m_sliderHP.value = (float)current / ((float)max + 0f);
			}
			this.HpAnima.SetHp(current, max, useAnimation);
			if (num <= 30.0)
			{
				this.HpAnima.SetTextColor(this.GetColorByType(UIAttributeController.AttributeColorType.Red));
				return;
			}
			this.HpAnima.SetTextColor(Color.white);
		}

		public void SetHpPercent(long current)
		{
			this.HpPercentAnima.SetValue(current);
			if (current <= 30L)
			{
				this.hpPercentAni.Play("TweenAlpha");
				return;
			}
			this.hpPercentAni.Play("Idle");
		}

		public void SetAttack(long value)
		{
			this.AttackAnim.SetValue(value);
		}

		public void SetAttackColor(UIAttributeController.AttributeColorType colorType)
		{
			Color colorByType = this.GetColorByType(colorType);
			this.AttackAnim.SetTextColor(colorByType);
		}

		public void SetDefence(long value)
		{
			this.DefenseAnim.SetValue(value);
		}

		public void SetDefenceColor(UIAttributeController.AttributeColorType colorType)
		{
			Color colorByType = this.GetColorByType(colorType);
			this.DefenseAnim.SetTextColor(colorByType);
		}

		public void SetGold(int value)
		{
			this.GoldAnim.SetValue((long)value);
			Color color;
			if (value <= 50)
			{
				color = this.GetColorByType(UIAttributeController.AttributeColorType.Red);
			}
			else
			{
				color = this.GetColorByType(UIAttributeController.AttributeColorType.Normal);
			}
			this.GoldAnim.SetTextColor(color);
		}

		public void SetExp(int exp, int nextExp, int lv)
		{
			this.newExp = exp;
			this.newNextExp = nextExp;
			this.newLv = lv;
			if (this.playerLv == 0)
			{
				this.playerLv = lv;
				float num = Utility.Math.Clamp01((float)exp / (float)nextExp);
				this.m_sliderExp.value = num;
				this.SetLevel(this.playerLv);
				return;
			}
			this.CheckLevelUp();
		}

		private void CheckLevelUp()
		{
			this.IsLevelUpAni = true;
			if (this.playerLv < this.newLv)
			{
				this.playerLv++;
				this.PlayExpAnim(1f, delegate
				{
					this.m_sliderExp.value = 0f;
					Singleton<GameEventController>.Instance.LevelUpRecoverHp(this.playerLv);
					this.ShowSelectSkill(this.playerLv);
				});
				return;
			}
			float num = (float)this.newExp / (float)this.newNextExp;
			this.PlayExpAnim(num, delegate
			{
				this.IsLevelUpAni = false;
			});
		}

		private void PlayExpAnim(float to, Action onFinish)
		{
			Sequence expSeq = DOTween.Sequence();
			float p = this.m_sliderExp.value;
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.OnUpdate<Sequence>(TweenSettingsExtensions.Append(expSeq, DOTween.To(() => p, delegate(float x)
			{
				p = x;
			}, to, 0.5f)), delegate
			{
				this.m_sliderExp.value = p;
			}), delegate
			{
				this.m_sliderExp.value = to;
				Action onFinish2 = onFinish;
				if (onFinish2 != null)
				{
					onFinish2();
				}
				TweenExtensions.Kill(expSeq, false);
				this.SetLevel(this.playerLv);
			});
		}

		private void SetLevel(int level)
		{
			if (level == Singleton<GameEventController>.Instance.PlayerData.MaxExpLevel)
			{
				this.Text_Level.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_144");
				return;
			}
			this.Text_Level.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { level });
		}

		private void ShowSelectSkill(int lv)
		{
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 0.5f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Chapter)
				{
					SelectSkillViewModule.OpenData openData = new SelectSkillViewModule.OpenData();
					openData.type = SelectSkillViewModule.SelectSkillType.LevelUp;
					openData.sourceType = SkillBuildSourceType.Normal;
					openData.randomNum = 3;
					openData.selectNum = 1;
					openData.callBack = new Action(this.CheckLevelUp);
					openData.randomSeed = Singleton<GameEventController>.Instance.GetLevelUpSkillSeed(lv);
					GameApp.View.OpenView(ViewName.SelectSkillViewModule, openData, 1, null, null);
				}
			});
		}

		public void SetDay(int day)
		{
			this.Text_Day.text = ((day == 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_57") : Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_15", new object[] { day }));
		}

		private Color GetColorByType(UIAttributeController.AttributeColorType colorType)
		{
			Color white = Color.white;
			if (colorType != UIAttributeController.AttributeColorType.Green)
			{
				if (colorType == UIAttributeController.AttributeColorType.Red)
				{
					ColorUtility.TryParseHtmlString("#FF604D", ref white);
				}
			}
			else
			{
				ColorUtility.TryParseHtmlString("#D3F24E", ref white);
			}
			return white;
		}

		private void OnClickHP()
		{
			this.ShowTip("UIGameEvent_17", "UIGameEvent_120", this.Button_HP.transform.position);
		}

		private void OnClickAttack()
		{
			this.ShowTip("UIGameEvent_121", "UIGameEvent_122", this.Button_Attack.transform.position);
		}

		private void OnClickDefence()
		{
			this.ShowTip("UIGameEvent_123", "UIGameEvent_124", this.Button_Defence.transform.position);
		}

		private void OnClickGold()
		{
			this.ShowTip("UIGameEvent_125", "UIGameEvent_126", this.Button_Gold.transform.position);
		}

		private void OnClickExp()
		{
			this.ShowTip("UIGameEvent_127", "UIGameEvent_128", this.Button_Exp.transform.position);
		}

		private void OnClickDay()
		{
			this.ShowTip("UIGameEvent_150", "UIGameEvent_151", this.Button_Day.transform.position);
		}

		private void ShowTip(string nameLanguageId, string infoLanguageId, Vector3 position)
		{
			new InfoTipViewModule.InfoTipData
			{
				m_name = Singleton<LanguageManager>.Instance.GetInfoByID(nameLanguageId),
				m_info = Singleton<LanguageManager>.Instance.GetInfoByID(infoLanguageId),
				m_position = position,
				m_offsetY = 226f
			}.Open();
		}

		private void ShowHp()
		{
			Color color = this.Text_HP.color;
			color.a = 0f;
			this.Text_HP.color = color;
			color = this.Text_HpPercent.color;
			color.a = 1f;
			this.Text_HpPercent.color = color;
			this.m_seqPool.Clear(false);
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.Text_HpPercent, 0f, 0.5f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.Text_HP, 1f, 0.5f));
		}

		private void ShowHpPercent()
		{
			Color color = this.Text_HP.color;
			color.a = 1f;
			this.Text_HP.color = color;
			color = this.Text_HpPercent.color;
			color.a = 0f;
			this.Text_HpPercent.color = color;
			this.m_seqPool.Clear(false);
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.Text_HP, 0f, 0.5f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.Text_HpPercent, 1f, 0.5f));
		}

		private void OnEventFlyAttributes(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFlyAttributes eventArgsFlyAttributes = eventArgs as EventArgsFlyAttributes;
			if (eventArgsFlyAttributes != null && eventArgsFlyAttributes.flyItems != null)
			{
				this.FlyItems(eventArgsFlyAttributes.flyItems);
			}
		}

		private void CreateNodeEffects()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.effectItem);
			gameObject.SetParentNormal(this.m_goldNode, false);
			this.effDic.Add(GameEventAttType.Food, gameObject.GetComponent<ParticleSystem>());
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.effectItem);
			gameObject2.SetParentNormal(this.m_expNode, false);
			this.effDic.Add(GameEventAttType.Exp, gameObject2.GetComponent<ParticleSystem>());
			GameObject gameObject3 = Object.Instantiate<GameObject>(this.effectItem);
			gameObject3.SetParentNormal(this.m_atkNode, false);
			this.effDic.Add(GameEventAttType.AttackPercent, gameObject3.GetComponent<ParticleSystem>());
			GameObject gameObject4 = Object.Instantiate<GameObject>(this.effectItem);
			gameObject4.SetParentNormal(this.m_defNode, false);
			this.effDic.Add(GameEventAttType.DefencePercent, gameObject4.GetComponent<ParticleSystem>());
			GameObject gameObject5 = Object.Instantiate<GameObject>(this.effectItem);
			gameObject5.SetParentNormal(this.m_hpNode, false);
			this.effDic.Add(GameEventAttType.HPMaxPercent, gameObject5.GetComponent<ParticleSystem>());
			this.effDic.Add(GameEventAttType.RecoverHpRate, gameObject5.GetComponent<ParticleSystem>());
		}

		private void FlyItems(Dictionary<GameEventAttType, GameObject> flyItems)
		{
			this.DeleteFlyItems();
			int num = 0;
			foreach (GameEventAttType gameEventAttType in flyItems.Keys)
			{
				GameObject gameObject = null;
				switch (gameEventAttType)
				{
				case GameEventAttType.AttackPercent:
					gameObject = this.m_atkNode;
					break;
				case GameEventAttType.RecoverHpRate:
				case GameEventAttType.HPMaxPercent:
					gameObject = this.m_hpNode;
					break;
				case GameEventAttType.Exp:
					gameObject = this.m_expNode;
					break;
				case GameEventAttType.Food:
					gameObject = this.m_goldNode;
					break;
				case GameEventAttType.DefencePercent:
					gameObject = this.m_defNode;
					break;
				}
				if (gameObject != null)
				{
					GameObject gameObject2 = flyItems[gameEventAttType];
					GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2);
					gameObject3.SetParentNormal(this.flyItemParent.transform, false);
					gameObject3.transform.position = gameObject2.transform.position;
					this.Fly(gameEventAttType, gameObject3, gameObject, (float)num * 0.25f);
				}
				num++;
			}
			if (flyItems.Count == 0)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
			}
		}

		private void Fly(GameEventAttType type, GameObject obj, GameObject target, float delay)
		{
			Sequence sequence = DOTween.Sequence();
			Vector3 vector = obj.transform.position + new Vector3(0.2f, 0.2f, 0f);
			TweenSettingsExtensions.AppendInterval(sequence, delay);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOMove(obj.transform, vector, 0.2f, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(obj.transform, Vector3.one * 3f, 0.2f));
			TweenSettingsExtensions.AppendInterval(sequence, 0.15f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(obj.transform, Vector3.one * 2.5f, 0.05f));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(obj.transform, target.transform.position, 0.2f, false), 8));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(obj.transform, Vector3.one, 0.2f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				obj.SetActiveSafe(false);
				this.waitDelete.Add(obj);
				ParticleSystem particleSystem;
				if (this.effDic.TryGetValue(type, out particleSystem))
				{
					particleSystem.gameObject.SetActiveSafe(true);
					particleSystem.Play();
				}
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
			});
		}

		private void DeleteFlyItems()
		{
			for (int i = 0; i < this.waitDelete.Count; i++)
			{
				Object.Destroy(this.waitDelete[i]);
			}
			this.waitDelete.Clear();
		}

		public const float TipOffset = 226f;

		public CustomButton Button_HP;

		public CustomButton Button_Attack;

		public CustomButton Button_Defence;

		public CustomButton Button_Gold;

		public CustomButton Button_Exp;

		public CustomButton Button_Day;

		public Slider m_sliderHP;

		public CustomText Text_HP;

		public CustomText Text_Attack;

		public CustomText Text_Defence;

		public CustomText Text_Gold;

		public CustomText Text_HpPercent;

		public CustomText Text_MaxLevel;

		public CustomText Text_Day;

		public GameObject m_goldNode;

		public GameObject m_expNode;

		public GameObject m_atkNode;

		public GameObject m_defNode;

		public GameObject m_hpNode;

		public Transform tranHpInfo;

		public Transform tranAttackInfo;

		public Transform tranDefenseInfo;

		public Transform tranGoldInfo;

		public Transform tranDayInfo;

		public Slider m_sliderExp;

		public CustomText Text_Level;

		public GameObject flyItemParent;

		public GameObject effectItem;

		public Animator hpPercentAni;

		private AttributeHpAnim HpAnima;

		private AttributeAnim HpPercentAnima;

		private AttributeAnim AttackAnim;

		private AttributeAnim DefenseAnim;

		private AttributeAnim GoldAnim;

		private SequencePool m_seqPool = new SequencePool();

		private List<GameObject> waitDelete = new List<GameObject>();

		private Dictionary<GameEventAttType, ParticleSystem> effDic = new Dictionary<GameEventAttType, ParticleSystem>();

		private int playerLv;

		private int newLv;

		private int newExp;

		private int newNextExp;

		private ChapterDataModule chapterDataModule;

		private const float DelayTime = 1f;

		private const float FadeTime = 0.5f;

		public enum AttributeColorType
		{
			Normal,
			Green,
			Red
		}
	}
}
