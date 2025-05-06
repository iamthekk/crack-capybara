using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainHero_HeroCenterGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_tipController.Init();
			if (this.m_Prefab_PlayerModelShow != null)
			{
				Transform transform = this.m_Prefab_PlayerModelShow.transform;
				this.m_Prefab_PlayerModelShowParent = transform.parent;
				transform.SetParent(null);
				transform.localScale = Vector3.one;
				transform.position = Vector3.zero;
				transform.rotation = Quaternion.identity;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.m_playerStateMachine != null)
			{
				this.m_playerStateMachine.OnUpdate(deltaTime);
			}
			float num = Mathf.Lerp(0.5f, 1f, Mathf.PingPong(Time.time, 1f));
			for (int i = 0; i < this.m_fgs.Count; i++)
			{
				CustomImage customImage = this.m_fgs[i];
				customImage.color = new Color(customImage.color.r, customImage.color.g, customImage.color.b, num);
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_Prefab_PlayerModelShow != null && this.m_Prefab_PlayerModelShowParent != null)
			{
				this.m_Prefab_PlayerModelShow.transform.SetParent(this.m_Prefab_PlayerModelShowParent);
			}
			if (this.PlayerModelShow != null)
			{
				this.PlayerModelShow.Clear(false);
			}
			if (this.m_tipController != null)
			{
				this.m_tipController.DeInit();
			}
			this.PlayerModelShow = null;
			this.m_headTrans = null;
			this.m_heroAnimator = null;
			if (this.m_playerStateMachine != null)
			{
				this.m_playerStateMachine.UnAllRegisterState();
			}
			this.m_playerStateMachine = null;
			foreach (KeyValuePair<int, GameObject> keyValuePair in this.m_progressOpenObjects)
			{
				if (!(keyValuePair.Value == null))
				{
					Object.Destroy(keyValuePair.Value);
				}
			}
			this.m_progressOpenObjects.Clear();
		}

		public void SetProgress(int progresss)
		{
			if (this.m_fgs == null || this.m_fgs.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.m_fgs.Count; i++)
			{
				this.m_fgs[i].gameObject.SetActive(i <= progresss);
			}
			this.m_currentProgress = progresss;
		}

		public void SetOpenProgress(int progress)
		{
			if (progress <= this.m_currentProgress)
			{
				this.SetProgress(progress);
				return;
			}
			if (this.m_fgs == null || this.m_fgs.Count == 0)
			{
				return;
			}
			if (progress >= this.m_fgs.Count)
			{
				return;
			}
			for (int i = this.m_currentProgress + 1; i <= progress; i++)
			{
				GameObject gameObject = this.m_fgs[i].gameObject;
				GameObject obj = Object.Instantiate<GameObject>(gameObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
				obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
				CustomImage component = obj.GetComponent<CustomImage>();
				this.m_progressOpenObjects[obj.GetInstanceID()] = obj;
				component.color = new Color(component.color.r, component.color.g, component.color.b, 1f);
				obj.SetActive(true);
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(component.rectTransform, Vector3.one, 0.5f), 28), delegate
				{
					if (obj == null)
					{
						return;
					}
					this.m_progressOpenObjects.Remove(obj.GetInstanceID());
					this.SetProgress(progress);
					Object.Destroy(obj);
				});
			}
		}

		public async void ShowPlayerModel(int memberID)
		{
			this.PlayerModelShow = UIViewPlayerCamera.Get("MainView.MainFitnessCenter", this.m_Prefab_PlayerModelShow);
			if (this.m_PlayerModelShow_Image != null && this.PlayerModelShow != null)
			{
				Object.DontDestroyOnLoad(this.PlayerModelShow.GObj);
				this.m_PlayerModelShow_Image.gameObject.SetActive(true);
			}
			this.PlayerModelShow.SetCameraTarget(this.m_PlayerModelShow_Image, this.m_PlayerModelShow_Image.rectTransform.rect.size, 1000);
			this.PlayerModelShow.SetOutlineWidth(0.09f);
			this.PlayerModelShow.SetShow(true);
			if (this.PlayerModelShow != null)
			{
				TaskOutValue<GameObject> taskOutValue = new TaskOutValue<GameObject>();
				await this.PlayerModelShow.FindCreatePlayer(taskOutValue, memberID, new Func<GameObject, Task>(this.OnCreateModel));
				if (this.m_slowLiuhanObj != null)
				{
					this.m_slowLiuhanObj.gameObject.transform.position = this.m_headTrans.position + new Vector3(0f, 1.5f, -4f);
					this.m_slowLiuhanObj.gameObject.transform.localScale = this.PlayerModelShow.TFPlayerParent.localScale;
				}
				if (this.m_quickLiuhanObj != null)
				{
					this.m_quickLiuhanObj.gameObject.transform.position = this.m_headTrans.position + new Vector3(0f, 1.5f, -4f);
					this.m_quickLiuhanObj.gameObject.transform.localScale = this.PlayerModelShow.TFPlayerParent.localScale;
				}
				this.m_playerStateMachine = new StateMachine();
				StateDefault stateDefault = new StateDefault(0, delegate(StateDefault enter)
				{
					if (this.m_slowLiuhanObj != null)
					{
						this.m_slowLiuhanObj.gameObject.SetActive(true);
					}
					if (this.m_quickLiuhanObj != null)
					{
						this.m_quickLiuhanObj.gameObject.SetActive(false);
					}
					if (this.m_heroAnimator != null)
					{
						this.m_heroAnimator.speed = 1f;
					}
				}, null, null);
				StateTimer stateTimer = new StateTimer(1, 1f, delegate(StateTimer enter)
				{
					if (this.m_slowLiuhanObj != null)
					{
						this.m_slowLiuhanObj.gameObject.SetActive(false);
					}
					if (this.m_quickLiuhanObj != null)
					{
						this.m_quickLiuhanObj.gameObject.SetActive(true);
					}
					if (this.m_levelUpParticle != null)
					{
						this.m_levelUpParticle.Play();
					}
					if (this.m_heroAnimator != null)
					{
						this.m_heroAnimator.speed = 2f;
					}
				}, null, delegate(StateTimer finished)
				{
					if (this.m_playerStateMachine != null)
					{
						this.m_playerStateMachine.ActiveState(0);
					}
				});
				this.m_playerStateMachine.RegisterState(stateDefault);
				this.m_playerStateMachine.RegisterState(stateTimer);
				this.m_playerStateMachine.ActiveState(0);
			}
		}

		public async Task OnCreateModel(GameObject obj)
		{
			if (this.PlayerModelShow != null && !(obj == null))
			{
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.transform.localPosition = Vector3.zero;
				MemberBody component = obj.GetComponent<MemberBody>();
				if (component != null)
				{
					if (component.m_weaponRightBody != null)
					{
						Vector3 eulerAngles = component.m_weaponRightBody.gameObject.transform.localRotation.eulerAngles;
						component.m_weaponRightBody.gameObject.transform.localRotation = Quaternion.Euler(-36f, eulerAngles.y, eulerAngles.z);
					}
					this.m_headTrans = component.m_head;
				}
				ComponentRegister component2 = obj.GetComponent<ComponentRegister>();
				if (component2 != null)
				{
					GameObject gameObject = component2.GetGameObject("Model");
					if (gameObject != null)
					{
						this.m_heroAnimator = gameObject.GetComponent<Animator>();
						if (this.m_heroAnimator != null)
						{
							this.m_heroAnimator.SetTrigger("Fitness");
						}
					}
				}
				await Task.CompletedTask;
			}
		}

		public void GotoHeroUpState()
		{
			if (this.m_playerStateMachine == null)
			{
				return;
			}
			this.m_playerStateMachine.ActiveState(1);
		}

		public void AddTip(string info)
		{
			if (this.m_tipController == null)
			{
				return;
			}
			this.m_tipController.AddNode(info);
		}

		[SerializeField]
		private RawImage m_PlayerModelShow_Image;

		[SerializeField]
		private GameObject m_slowLiuhanObj;

		[SerializeField]
		private GameObject m_quickLiuhanObj;

		[SerializeField]
		private ParticleSystem m_levelUpParticle;

		[SerializeField]
		public UITipController m_tipController;

		[SerializeField]
		private List<CustomImage> m_fgs;

		[SerializeField]
		private GameObject m_Prefab_PlayerModelShow;

		private UIViewPlayerCamera PlayerModelShow;

		private Transform m_headTrans;

		private Animator m_heroAnimator;

		private StateMachine m_playerStateMachine;

		private Dictionary<int, GameObject> m_progressOpenObjects = new Dictionary<int, GameObject>();

		private int m_currentProgress = -1;

		private Transform m_Prefab_PlayerModelShowParent;
	}
}
