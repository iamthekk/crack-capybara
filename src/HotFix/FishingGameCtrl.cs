using System;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class FishingGameCtrl : CustomBehaviour
	{
		private FishingRodState RodState
		{
			get
			{
				return this._curRodState;
			}
			set
			{
				this._curRodState = value;
				this.OnRodStateChanged(this._curRodState);
			}
		}

		protected override void OnInit()
		{
			this.tipCtrl.Init();
			this.fishRodCtrl.Init();
			this.fishCtrl.Init();
			this.focusCtrl.Init();
			this._sliderBg = this.slider.transform.GetComponent<RectTransform>();
			this._sliderBgWidth = this._sliderBg.sizeDelta.x;
			this._sliderFill = this.slider.GetChild(1).transform.GetComponent<RectTransform>();
			this._sliderFillWidth = this._sliderFill.sizeDelta.x;
			this.RodState = FishingRodState.Idle;
			this.spinBtn.onClick.AddListener(new UnityAction(this.OnSpinBtnClick));
			this.spinBtn.onDown = delegate
			{
				this._isPressing = true;
				if (this.RodState == FishingRodState.Focus)
				{
					this.FocusStop();
				}
			};
			this.spinBtn.onUp = delegate
			{
				this._isPressing = false;
				if (this.RodState == FishingRodState.Reel)
				{
					this.RodState = FishingRodState.Relax;
				}
			};
		}

		protected override void OnDeInit()
		{
			this.spinBtn.onClick.RemoveListener(new UnityAction(this.OnSpinBtnClick));
			ShortcutExtensions.DOKill(this.handleImgTrans, false);
			this.StopContinuousShake();
			ShortcutExtensions.DOKill(base.transform, false);
			this._seqPool.Clear(false);
			this.tipCtrl.DeInit();
			this.fishRodCtrl.DeInit();
			this.fishCtrl.DeInit();
			this.focusCtrl.DeInit();
		}

		public void SetData(Fishing_fishing data, int rodId, int seed)
		{
			this._fishBaseConfig = data;
			this.randomSeed = seed;
			Fishing_fishRod elementById = GameApp.Table.GetManager().GetFishing_fishRodModelInstance().GetElementById(rodId);
			if (elementById == null)
			{
				HLog.LogError("fish rod config is null");
				return;
			}
			this.SetRodConfig(elementById);
			this.RefreshUI();
		}

		private void SetRodConfig(Fishing_fishRod config)
		{
			this._rodConfig = config;
			this.rodStrength = (float)this._rodConfig.strength;
			this.rodSpeed = (float)this._rodConfig.speed;
			this.rodHp = (float)this._rodConfig.hp;
			Fishing_fishRod fishing_fishRod = GameApp.Table.GetManager().GetFishing_fishRodModelInstance().GetAllElements()
				.Last<Fishing_fishRod>();
			float num = (float)config.hp / (float)fishing_fishRod.hp;
			this._sliderBg.sizeDelta = new Vector2(num * this._sliderBgWidth, this._sliderBg.sizeDelta.y);
			this._sliderFill.sizeDelta = new Vector2(num * this._sliderBgWidth - 13f, this._sliderFill.sizeDelta.y);
			this.tensionBar.OutInit();
			this.RodState = FishingRodState.Idle;
		}

		private void SetFishConfig(FishData data)
		{
			this.fishData = data;
			this.fishStrength = (float)this.fishData.Config.strength;
			this.fishSpeed = (float)this.fishData.Config.speed;
			this.fishBasicDamage = (float)this.fishData.Config.initialDamage;
			this._basicDamageDecreasePerSecond = this.fishBasicDamage / this.fishBasicDamageEffectTime;
			this._basicDamageDecreaseTimer = this.fishBasicDamageEffectTime;
		}

		private void DelayPlaySound(int id, float delayTime)
		{
			Sequence sequence = this._seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, delayTime);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				GameApp.Sound.PlayClip(id, 1f);
			});
		}

		private async void PlaySoundNew(int id)
		{
			Sound_sound elementById = GameApp.Table.GetManager().GetSound_soundModelInstance().GetElementById(id);
			if (elementById != null && elementById.volume > 0f)
			{
				this.audioPath = elementById.path;
				await GameApp.Sound.PlaySoundEffectNew(this.audioPath, elementById.volume);
			}
		}

		private FishRodCtrl.SkinType GetRodSkinType()
		{
			if (this._rodConfig == null)
			{
				return FishRodCtrl.SkinType.A;
			}
			return (FishRodCtrl.SkinType)this._rodConfig.type;
		}

		private float GetStruggleAnimSpeed()
		{
			float num = 1f;
			if (this.fishCtrl.GetFishState() == FishCtrl.FishState.Struggle)
			{
				num = this.struggleAnimSpeed;
			}
			return num;
		}

		private bool IsStruggle()
		{
			return this.fishCtrl.GetFishState() != FishCtrl.FishState.Tired;
		}

		private void ResetSelf()
		{
			this.focusCtrl.ResetIndicator();
			this._rodCurHp = this.rodHp;
			this._curFishDist = (float)this._fishBaseConfig.distanceDefault;
			this._successDist = 0f;
			this._failedDist = (float)this._fishBaseConfig.distanceFail;
			this.tensionBar.Value = 0f;
		}

		public void ResetState()
		{
			this.RodState = FishingRodState.Idle;
		}

		private void FishingFailed(int type)
		{
			this._failedType = type;
			this.RodState = FishingRodState.Failed;
		}

		private void StartContinuousShake(int type)
		{
			float num;
			float num2;
			int num3;
			if (type == 1)
			{
				num = this.shakeTime;
				num2 = this.shakeStrength;
				num3 = this.shakeCount;
			}
			else if (type == 2)
			{
				num = this.shakeTime2;
				num2 = this.shakeStrength2;
				num3 = this.shakeCount2;
			}
			else
			{
				num = this.shakeTime3;
				num2 = this.shakeStrength3;
				num3 = this.shakeCount3;
			}
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetUpdate<Tweener>(ShortcutExtensions.DOShakePosition(this.spinRoot, num, num2, num3, 90f, false, false), true), delegate
			{
				this.StartContinuousShake(type);
			});
		}

		private void StopContinuousShake()
		{
			ShortcutExtensions.DOKill(this.spinRoot, false);
			this.spinRoot.localPosition = this._startSpinRootPos;
		}

		private void OnRodStateChanged(FishingRodState state)
		{
			FishRodCtrl.SkinType rodSkinType = this.GetRodSkinType();
			switch (state)
			{
			case FishingRodState.Idle:
				this._startSpinRootPos = this.spinRoot.localPosition;
				this.handleImgTrans.rotation = Quaternion.Euler(this._handleRotation);
				this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._00, true, null, 1f);
				this.ShowOrHideUI(true);
				this.focusCtrl.gameObject.SetActive(false);
				this.tensionBar.gameObject.SetActiveSafe(false);
				this.StopContinuousShake();
				this.handImg.color = this.normalColor;
				this.spinImg.color = this.normalColor;
				this._isStartVibration = true;
				break;
			case FishingRodState.Focus:
				this.ShowOrHideUI(false);
				break;
			case FishingRodState.Throwing:
				this.handImg.color = this.transparentColor;
				this.spinImg.color = this.transparentColor;
				ObservableExtensions.Subscribe<long>(Observable.Timer(TimeSpan.FromSeconds((double)this.throwStartTime)), delegate(long _)
				{
					ShortcutExtensions.DOLocalRotate(this.handleImgTrans, new Vector3(0f, 0f, (float)(this.spinCircleCount * 360)), this.cirCleTime, 1);
				});
				this.DelayPlaySound(4105, 0.2f);
				this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._01, false, delegate
				{
					FishData fishData = Singleton<GameEventController>.Instance.GetFishingFactory().RandomFish(this._eval, this.randomSeed);
					this.SetFishConfig(fishData);
					this.RodState = FishingRodState.Wait;
					this.RefreshUI();
				}, this.throwSpeed);
				break;
			case FishingRodState.Wait:
				this._timer = Random.Range(this.miniBiteTime, this.maxBiteTime);
				this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._02, true, null, 1f);
				break;
			case FishingRodState.Bite:
				DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Middle);
				this.handImg.color = this.normalColor;
				this.spinImg.color = this.normalColor;
				this.StartContinuousShake(1);
				this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._03, false, delegate
				{
					this.tensionBar.gameObject.SetActiveSafe(true);
					this.fishCtrl.StartBite(this.fishData);
					this.RodState = FishingRodState.Relax;
					this._isStartVibration = false;
				}, 1f);
				break;
			case FishingRodState.Relax:
				if (!this._isStartVibration)
				{
					FishCtrl.FishState fishState = this.fishCtrl.GetFishState();
					if (fishState == FishCtrl.FishState.StruggleViolently)
					{
						this.StartContinuousShake(2);
					}
					if (fishState == FishCtrl.FishState.Struggle)
					{
						this.StartContinuousShake(3);
					}
				}
				if (this.IsStruggle())
				{
					this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._14, true, null, this.GetStruggleAnimSpeed());
				}
				else
				{
					this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._15, true, null, 1f);
				}
				break;
			case FishingRodState.Reel:
				GameApp.Sound.StopSoundEffect(this.audioPath);
				this._audioPlayTimer = 0f;
				this.StopContinuousShake();
				if (this.IsStruggle())
				{
					this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._04, true, null, this.GetStruggleAnimSpeed());
				}
				else
				{
					this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._05, true, null, 1f);
				}
				break;
			case FishingRodState.Success:
				GameApp.Sound.StopSoundEffect(this.audioPath);
				this.DelayPlaySound(4107, 0.2f);
				this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._Z01, false, delegate
				{
					this.ShowFishingResult(true);
				}, 1f);
				break;
			case FishingRodState.Failed:
				GameApp.Sound.StopSoundEffect(this.audioPath);
				GameApp.Sound.PlayClip(4108, 1f);
				this.fishRodCtrl.Play(rodSkinType, FishRodCtrl.AnimType._Z02, false, delegate
				{
					this.ShowFishingResult(false);
				}, 1f);
				break;
			case FishingRodState.End:
				this.StopContinuousShake();
				break;
			}
			this.fishRodCtrl.ShowFishStateTip(state == FishingRodState.Reel || state == FishingRodState.Relax);
		}

		private void FixedUpdate()
		{
			if (this.RodState == FishingRodState.Idle)
			{
				return;
			}
			if (this.RodState == FishingRodState.Relax && this._isPressing && this._basicDamageDecreaseTimer <= 0f)
			{
				this.RodState = FishingRodState.Reel;
			}
			if (this.RodState == FishingRodState.Reel)
			{
				this.fishCtrl.RodStateLeadToUpdate(Time.fixedDeltaTime);
			}
			if (this.RodState == FishingRodState.Reel || this.RodState == FishingRodState.Relax)
			{
				this.UpdateFishingValue(Time.fixedDeltaTime, this.fishCtrl.GetFishState());
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.RodState == FishingRodState.Idle)
			{
				return;
			}
			if (this.RodState == FishingRodState.Wait)
			{
				this._timer -= deltaTime;
				if (this._timer < 0f)
				{
					this.RodState = FishingRodState.Bite;
				}
			}
			if (this.RodState == FishingRodState.Reel || this.RodState == FishingRodState.Relax)
			{
				if (this._curFishDist <= this._successDist)
				{
					this.RodState = FishingRodState.Success;
					this.fishCtrl.EndBite();
				}
				else if (this._curFishDist >= this._failedDist)
				{
					this.FishingFailed(1);
					this.fishCtrl.EndBite();
				}
				if (this._rodCurHp <= 0f)
				{
					this._persistenceTimer -= deltaTime;
					if (this._persistenceTimer <= 0f)
					{
						this.FishingFailed(2);
						this.fishCtrl.EndBite();
					}
				}
				else
				{
					this._persistenceTimer = this.persistenceTime;
				}
				this.fishCtrl.SetDistance(Math.Round((double)this._curFishDist));
				this.fishCtrl.SetPos(this._curFishDist / (float)this._fishBaseConfig.distanceFail);
			}
		}

		private void LateUpdate()
		{
			if (this.RodState == FishingRodState.Reel || this.RodState == FishingRodState.Relax)
			{
				float num = 1f - this._rodCurHp / this.rodHp;
				if (num >= 0.96f)
				{
					this.tensionBar.Value = Mathf.Clamp(num, 0f, 1f);
					return;
				}
				float num2 = num + this.barUp;
				float num3 = Mathf.Lerp(num + this.barDown, num2, Mathf.PingPong(Time.time, this.barUp * 2f) - this.barUp);
				this.tensionBar.Value = Mathf.Clamp(num3, 0f, 1f);
			}
		}

		private void UpdateFishingValue(float deltaTime, FishCtrl.FishState fishState)
		{
			if (this._lastFishState != fishState)
			{
				this.RodState = this._curRodState;
				this._lastFishState = fishState;
			}
			float num = 0f;
			float num2 = 1f;
			float num3 = 1f;
			if (fishState == FishCtrl.FishState.StruggleViolently)
			{
				num2 = this.fishCtrl.strongSpeedRate;
				num3 = this.fishCtrl.strongStrengthRate;
			}
			else if (fishState == FishCtrl.FishState.Tired)
			{
				num2 = this.fishCtrl.tiredSpeedRate;
				num3 = this.fishCtrl.tiredStrengthRate;
			}
			if (this._basicDamageDecreaseTimer > 0f)
			{
				this._basicDamageDecreaseTimer -= deltaTime;
				this._rodCurHp -= Mathf.Max(0f, this._basicDamageDecreasePerSecond * deltaTime);
			}
			if (this.RodState == FishingRodState.Relax)
			{
				if (this._basicDamageDecreaseTimer <= 0f)
				{
					this._rodCurHp += (float)this._rodConfig.hpRestore * deltaTime;
				}
				num = -(this.fishSpeed * num2) * deltaTime * this.rate;
			}
			else if (this.RodState == FishingRodState.Reel)
			{
				if (fishState == FishCtrl.FishState.Tired)
				{
					this._rodCurHp += (float)this._rodConfig.hpRestore * this.tiredRecoverRate * deltaTime;
					num = (float)this._rodConfig.tiresSpeed * deltaTime * this.rate;
				}
				else
				{
					this._rodCurHp -= Mathf.Max(0f, this.fishStrength * num3 - this.rodStrength) * deltaTime * this.rate;
					num = (this.rodSpeed - this.fishSpeed * num2) * deltaTime * this.rate;
				}
			}
			this._curFishDist -= num;
			if (this._basicDamageDecreaseTimer <= 0f)
			{
				this._rodCurHp = Mathf.Clamp(this._rodCurHp, 0f, this.rodHp - this.fishBasicDamage);
			}
			float num4 = num * this.handleSpeed;
			this.handleImgTrans.Rotate(0f, 0f, -num4);
			this._audioPlayTimer -= deltaTime;
			float num5 = Mathf.Abs(num4);
			if (num5 != 0f && this._audioPlayTimer < 0f)
			{
				if (num4 > 0f)
				{
					this._audioPlayTimer = this.handleAudioSpeed / num5;
					GameApp.Sound.PlayClip(4106, 1f);
				}
				else if (num4 < 0f)
				{
					this._audioPlayTimer = 5f;
					this.PlaySoundNew(4109);
				}
			}
			if (fishState != FishCtrl.FishState.Tired && this.RodState == FishingRodState.Reel && 1f - this._rodCurHp / this.rodHp > 0.9f)
			{
				this._timer -= deltaTime;
				if (this._timer <= 0f)
				{
					this._timer = 0.1f;
					DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Middle);
				}
			}
		}

		private void ShowOrHideUI(bool isShow)
		{
			this.mask.gameObject.SetActive(!isShow);
			this.fishRodCtrl.ShowName(isShow);
		}

		private void RefreshUI()
		{
			if (this._rodConfig != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(this._rodConfig.atlas);
				this.rodImg.SetImage(atlasPath, this._rodConfig.icon);
				this.fishRodCtrl.RefreshName(this._rodConfig.nameId);
				GameEventFishingFactory fishingFactory = Singleton<GameEventController>.Instance.GetFishingFactory();
				this.textBait.text = fishingFactory.baitNum.ToString();
			}
		}

		private void ShowFishingResult(bool result)
		{
			this.RodState = FishingRodState.End;
			FishingResultData fishingResultData = new FishingResultData(result, this.fishData, this._failedType);
			GameApp.View.OpenView(ViewName.FishingResultViewModule, fishingResultData, 1, null, null);
		}

		public void StartRotate()
		{
			if (this.RodState != FishingRodState.Idle)
			{
				return;
			}
			this.ResetSelf();
			this.focusCtrl.RandomRotate();
			this.RodState = FishingRodState.Focus;
		}

		private void FocusStop()
		{
			this._eval = this.focusCtrl.GetEval();
			if (this._eval != FishingEval.Normal)
			{
				this.tipCtrl.ShowThrowResultTips(this._eval);
			}
			this.focusCtrl.gameObject.SetActive(false);
			this.RodState = FishingRodState.Throwing;
		}

		private void OnSpinBtnClick()
		{
			if (this.RodState == FishingRodState.Idle)
			{
				if (Singleton<GameEventController>.Instance.GetFishingFactory().UseBait())
				{
					this.StartRotate();
					this.RefreshUI();
					return;
				}
				EventArgsString instance = Singleton<EventArgsString>.Instance;
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishing_NoBait");
				instance.SetData(infoByID);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, instance);
			}
		}

		public Transform mask;

		[Header("张力条")]
		public ProgressTextCtrl tensionBar;

		public Transform slider;

		public CustomImage rodImg;

		public float barDown = -0.1f;

		public float barUp = 0.1f;

		[Header("鱼线轮")]
		public Transform spinRoot;

		public CustomText textBait;

		public CustomButton spinBtn;

		public Transform handleImgTrans;

		public Image handImg;

		public Image spinImg;

		public Color normalColor;

		public Color transparentColor;

		public float handleSpeed = 1f;

		public float handleAudioSpeed = 1f;

		[Header("控制器")]
		public FishingTipCtrl tipCtrl;

		public FishRodCtrl fishRodCtrl;

		public FishCtrl fishCtrl;

		public FishingFocusCtrl focusCtrl;

		[Header("按钮的震动")]
		public float shakeStrength = 5f;

		public float shakeTime = 5f;

		public int shakeCount = 5;

		[Header("有力的震动")]
		public float shakeStrength2 = 5f;

		public float shakeTime2 = 5f;

		public int shakeCount2 = 5;

		[Header("挣扎的震动")]
		public float shakeStrength3 = 5f;

		public float shakeTime3 = 5f;

		public int shakeCount3 = 5;

		[Header("甩杆转动圈数")]
		public int spinCircleCount = 3;

		public float throwStartTime = 0.3f;

		public float cirCleTime = 1f;

		public float throwSpeed = 0.5f;

		public float struggleAnimSpeed = 0.5f;

		[Header("中鱼时间配置")]
		public float miniBiteTime = 1f;

		public float maxBiteTime = 2f;

		[Header("调试拉扯部分")]
		public float rodStrength;

		public float rodSpeed;

		public float rodHp;

		public float tiredRecoverRate = 0.1f;

		public float fishStrength;

		public float fishSpeed;

		[Header("鱼的基础值")]
		public float fishBasicDamage;

		public float fishBasicDamageEffectTime = 0.5f;

		public float rate = 0.01f;

		public float persistenceTime = 0.5f;

		private FishingEval _eval;

		private float _persistenceTimer;

		private float _basicDamageDecreasePerSecond;

		private float _basicDamageDecreaseTimer;

		private Vector3 _startSpinRootPos = Vector3.zero;

		private FishingRodState _curRodState;

		private float _timer;

		private Fishing_fishRod _rodConfig;

		private FishData fishData;

		private float _rodCurHp;

		private float _curFishDist;

		private float _lastDist;

		private float _successDist;

		private float _failedDist = 200f;

		private bool _isPressing;

		private Fishing_fishing _fishBaseConfig;

		private RectTransform _sliderBg;

		private RectTransform _sliderFill;

		private float _sliderBgWidth;

		private float _sliderFillWidth;

		private FishCtrl.FishState _lastFishState;

		private float _audioPlayTimer;

		private readonly Vector3 _handleRotation = Vector3.zero;

		private int _failedType = 1;

		private readonly SequencePool _seqPool = new SequencePool();

		private int randomSeed;

		private string audioPath;

		private bool _isStartVibration = true;
	}
}
