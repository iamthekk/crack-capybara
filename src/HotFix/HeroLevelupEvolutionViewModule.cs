using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class HeroLevelupEvolutionViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_heroLevelupDataModule = GameApp.Data.GetDataModule(DataName.HeroLevelUpDataModule);
			Transform transform = this.UI_HeroDetaiShowCamera.transform;
			transform.SetParent(null);
			transform.localScale = Vector3.one;
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			this.m_nameNodeTimer = new Timer();
			this.m_nameNodeTimer.OnInit();
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as HeroLevelupEvolutionViewModule.OpenData;
			if (this.m_openData == null)
			{
				HLog.LogError("HeroLevelupEvolutionViewModule OnOpen data is null");
				return;
			}
			if (this.m_openData.m_cardData == null)
			{
				HLog.LogError("HeroLevelupEvolutionViewModule OnOpen m_cardData is null");
				return;
			}
			this.m_cardData = new CardData();
			this.m_cardData.CloneFrom(this.m_openData.m_cardData);
			if (GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(this.m_cardData.m_memberID) == null)
			{
				HLog.LogError(string.Format("[HeroLevelupEvolutionViewModule]tab[{0}] == null", this.m_cardData));
			}
			IList<HeroLevelup_HeroLevelup> allElements = GameApp.Table.GetManager().GetHeroLevelup_HeroLevelupModelInstance().GetAllElements();
			HeroLevelup_HeroLevelup heroLevelup_HeroLevelup = null;
			HeroLevelup_HeroLevelup heroLevelup_HeroLevelup2 = null;
			for (int i = 0; i < allElements.Count; i++)
			{
				HeroLevelup_HeroLevelup heroLevelup_HeroLevelup3 = allElements[i];
				if (heroLevelup_HeroLevelup3.ID == this.m_openData.m_currentTableID)
				{
					heroLevelup_HeroLevelup2 = heroLevelup_HeroLevelup3;
					break;
				}
				heroLevelup_HeroLevelup = heroLevelup_HeroLevelup3;
			}
			if (heroLevelup_HeroLevelup == null || heroLevelup_HeroLevelup2 == null)
			{
				HLog.LogError(string.Format("HeroLevelupNextEvolutionViewModule OnOpen lastTable= {0}, currentTable={1}", heroLevelup_HeroLevelup != null, heroLevelup_HeroLevelup2 != null));
				return;
			}
			this.m_nameNode.Init();
			this.m_nameNode.SetFromTxt(this.m_heroLevelupDataModule.GetTrainingTitleName(heroLevelup_HeroLevelup, Color.yellow));
			this.m_nameNode.SetToTxt(this.m_heroLevelupDataModule.GetTrainingTitleName(heroLevelup_HeroLevelup2, Color.yellow));
			this.m_nameNode.SetActive(false);
			float num = 0.25f;
			this.m_nameNodeTimer.m_onFinished = delegate(Timer t)
			{
				this.m_nameNode.SetActive(true);
			};
			this.m_nameNodeTimer.Play(num);
			num += 0.2f;
			List<string> list = new List<string>();
			List<MergeAttributeData> list2 = new List<MergeAttributeData>();
			List<MergeAttributeData> list3 = new List<MergeAttributeData>();
			for (int j = 0; j < allElements.Count; j++)
			{
				HeroLevelup_HeroLevelup heroLevelup_HeroLevelup4 = allElements[j];
				if (heroLevelup_HeroLevelup4.ID == heroLevelup_HeroLevelup.ID)
				{
					List<MergeAttributeData> levelUpRewards = this.m_heroLevelupDataModule.GetLevelUpRewards(heroLevelup_HeroLevelup4.ID);
					list3.AddRange(list2);
					list3.AddRange(levelUpRewards);
					break;
				}
				List<MergeAttributeData> levelUpRewards2 = this.m_heroLevelupDataModule.GetLevelUpRewards(heroLevelup_HeroLevelup4.ID);
				list2.AddRange(levelUpRewards2);
			}
			list2 = list2.Merge();
			list3 = list3.Merge();
			for (int k = 0; k < list3.Count; k++)
			{
				list.Add(list3[k].Header);
			}
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(list2, false);
			MemberAttributeData memberAttributeData2 = new MemberAttributeData();
			memberAttributeData2.MergeAttributes(list3, false);
			this.m_attributesNodeTimers = new List<Timer>(list.Count);
			for (int l = 0; l < list.Count; l++)
			{
				string text = list[l];
				long basicAttributeValue = memberAttributeData.GetBasicAttributeValue(text);
				long basicAttributeValue2 = memberAttributeData2.GetBasicAttributeValue(text);
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_attributesNodePrefab, this.m_attributesParent);
				gameObject.transform.SetParent(this.m_attributesParent);
				gameObject.transform.position = this.m_attributesParent.position;
				gameObject.transform.localScale = Vector3.one;
				HeroLevelupAttributeController component = gameObject.GetComponent<HeroLevelupAttributeController>();
				component.Init();
				component.SetNameTxt(Singleton<LanguageManager>.Instance.GetInfoByID(GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(text)
					.LanguageId));
				component.SetFromTxt(DxxTools.FormatNumber(basicAttributeValue));
				component.SetToTxt(DxxTools.FormatNumber(basicAttributeValue2));
				component.gameObject.SetActive(false);
				this.m_attributeNodes[gameObject.GetInstanceID()] = component;
				Timer timer = new Timer(gameObject.GetInstanceID());
				timer.m_onFinished = new Action<Timer>(this.OnNodeTimerFinished);
				timer.Play(num);
				this.m_attributesNodeTimers.Add(timer);
				num += 0.2f;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_attributesParent);
			this.ShowPlayerModel(this.m_openData.m_cardData.m_memberID);
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeMaskBt != null)
			{
				this.m_closeMaskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_nameNodeTimer != null)
			{
				this.m_nameNodeTimer.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			for (int i = 0; i < this.m_attributesNodeTimers.Count; i++)
			{
				this.m_attributesNodeTimers[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeMaskBt != null)
			{
				this.m_closeMaskBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			foreach (KeyValuePair<int, HeroLevelupAttributeController> keyValuePair in this.m_attributeNodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			for (int i = 0; i < this.m_attributesNodeTimers.Count; i++)
			{
				this.m_attributesNodeTimers[i].OnDeInit();
			}
			this.m_attributesNodeTimers.Clear();
			this.m_attributeNodes.Clear();
			this.m_openData = null;
			this.m_cardData = null;
		}

		public override void OnDelete()
		{
			if (this.m_nameNode != null)
			{
				this.m_nameNode.DeInit();
			}
			if (this.m_playerModelShow != null)
			{
				this.m_playerModelShow.OnThisDestroy();
			}
			if (this.m_nameNodeTimer != null)
			{
				this.m_nameNodeTimer.OnDeInit();
			}
			this.m_nameNodeTimer = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.HeroLevelupEvolutionViewModule, null);
		}

		private void OnNodeTimerFinished(Timer obj)
		{
			HeroLevelupAttributeController heroLevelupAttributeController;
			this.m_attributeNodes.TryGetValue(obj.ID, out heroLevelupAttributeController);
			if (heroLevelupAttributeController == null)
			{
				return;
			}
			heroLevelupAttributeController.SetActive(true);
		}

		public async void ShowPlayerModel(int memberID)
		{
			this.m_playerModelShow = UIViewPlayerCamera.Get("HeroLevelupEvolutionViewModule", this.UI_HeroDetaiShowCamera);
			if (this.m_icon != null && this.m_playerModelShow != null)
			{
				Object.DontDestroyOnLoad(this.m_playerModelShow.GObj);
				this.m_icon.gameObject.SetActive(true);
			}
			this.m_playerModelShow.SetCameraTarget(this.m_icon, this.m_icon.rectTransform.rect.size, 1000);
			this.m_playerModelShow.SetOutlineWidth(0.09f);
			this.m_playerModelShow.SetShow(true);
			if (this.m_playerModelShow != null)
			{
				TaskOutValue<GameObject> taskOutValue = new TaskOutValue<GameObject>();
				await this.m_playerModelShow.FindCreatePlayer(taskOutValue, memberID, new Func<GameObject, Task>(this.OnCreateModel));
			}
		}

		public async Task OnCreateModel(GameObject obj)
		{
			if (this.m_playerModelShow != null && !(obj == null))
			{
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.transform.localPosition = Vector3.zero;
				this.m_heroObject = obj;
				ComponentRegister component = obj.GetComponent<ComponentRegister>();
				if (component != null)
				{
					GameObject gameObject = component.GetGameObject("Model");
					if (gameObject != null)
					{
						Animator component2 = gameObject.GetComponent<Animator>();
						if (component2 != null)
						{
							component2.SetTrigger("Win");
						}
					}
				}
				await Task.CompletedTask;
			}
		}

		public HeroLevelupEvolutionViewModule.OpenData m_openData;

		public CardData m_cardData;

		public CustomButton m_closeBt;

		public CustomButton m_closeMaskBt;

		public GameObject UI_HeroDetaiShowCamera;

		public UIViewPlayerCamera m_playerModelShow;

		public RawImage m_icon;

		public HeroLevelupAttributeController m_nameNode;

		public RectTransform m_attributesParent;

		public GameObject m_attributesNodePrefab;

		private GameObject m_heroObject;

		private Dictionary<int, HeroLevelupAttributeController> m_attributeNodes = new Dictionary<int, HeroLevelupAttributeController>();

		public Timer m_nameNodeTimer;

		public List<Timer> m_attributesNodeTimers;

		private HeroLevelUpDataModule m_heroLevelupDataModule;

		public class OpenData
		{
			public int m_currentTableID;

			public CardData m_cardData;
		}
	}
}
