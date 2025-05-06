using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Habby.Mail.Data;
using LocalModels.Bean;
using Proto.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class MailInfoViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.MailInfoViewModule;
		}

		public override void OnCreate(object data)
		{
			if (this.m_rewardNodePrefab != null)
			{
				this.m_rewardNodePrefab.SetActive(false);
			}
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as MailInfoViewModule.OpenData;
			this.OnRefreshUI();
			this.m_popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.m_awardBt.onClick.AddListener(new UnityAction(this.OnClickAwardBt));
			this.m_okBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_popCommon.OnClick = null;
			this.m_awardBt.onClick.RemoveAllListeners();
			this.m_okBt.onClick.RemoveAllListeners();
			this.DestroyRewards();
			this.m_openData = null;
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_MailData_ReceiveAwardsData, new HandlerEvent(this.OnEventReceiveAwardData));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_ReceiveAwardsData, new HandlerEvent(this.OnEventReceiveAwardData));
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(this.GetName(), null);
		}

		private void OnClickAwardBt()
		{
			if (this.m_openData == null)
			{
				return;
			}
			if (this.m_openData.m_mailData == null)
			{
				return;
			}
			string mailId = this.m_openData.m_mailData.mailId;
			string[] array = new string[] { mailId };
			GameApp.Mail.GetManager().Control.SendGetMailReward(array, delegate(List<MailRewardObject> resps, CommonData commonData)
			{
				for (int i = 0; i < resps.Count; i++)
				{
					MailRewardObject mailRewardObject = resps[i];
					if (mailRewardObject != null)
					{
						EventMailReceiveAwards instance = Singleton<EventMailReceiveAwards>.Instance;
						instance.SetData(mailRewardObject);
						GameApp.Event.DispatchNow(this, 226, instance);
					}
				}
				GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(999999), null, null, null, null, resps);
				GameApp.SDK.Analyze.Track_Mail(resps);
				GameApp.Event.DispatchNow(this, 229, null);
				DxxTools.UI.OpenRewardCommon(commonData.Reward, null, true);
				if (commonData != null && commonData.Reward != null)
				{
					bool flag = false;
					for (int j = 0; j < commonData.Reward.Count; j++)
					{
						RewardDto rewardDto = commonData.Reward[j];
						if (rewardDto != null)
						{
							Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId);
							if (elementById != null && elementById.itemType == 46)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_ShopInfo_DataPull, null);
					}
				}
				GameApp.Data.GetDataModule(DataName.LoginDataModule).EmailSetUserInfo(commonData.UserInfoDto);
				RedPointController.Instance.ReCalc("Main.Mail", true);
			}, true);
		}

		private void OnEventReceiveAwardData(object sender, int type, BaseEventArgs eventargs)
		{
			if (!(eventargs is EventMailReceiveAwards))
			{
				return;
			}
			if (this.m_openData == null)
			{
				return;
			}
			if (this.m_openData.m_mailData == null)
			{
				return;
			}
			MailData mailData = GameApp.Data.GetDataModule(DataName.MailDataModule).GetMailData(this.m_openData.m_mailData.mailId);
			if (mailData == null)
			{
				return;
			}
			this.m_openData.m_mailData = mailData;
			this.OnRefreshUI();
		}

		public void OnRefreshUI()
		{
			if (this.m_openData == null)
			{
				return;
			}
			if (this.m_openData.m_mailData == null)
			{
				return;
			}
			MailData mailData = this.m_openData.m_mailData;
			if (this.m_titleTxt != null)
			{
				this.m_titleTxt.text = mailData.mailTitle;
			}
			if (this.m_infoTxt != null)
			{
				this.m_infoTxt.text = MailDataModule.GetFinalContent(mailData.mailContent);
			}
			if (this.m_sendTxt != null)
			{
				this.m_sendTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("164");
			}
			if (this.m_endTimeTxt != null)
			{
				long expireAt = mailData.GetExpireAt();
				long localUTC = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC;
				long num = expireAt - localUTC;
				this.m_endTimeTxt.text = Singleton<LanguageManager>.Instance.GetEndTime(num);
			}
			this.DestroyRewards();
			bool flag = mailData.IsHaveReward();
			bool flag2 = false;
			if (this.m_rewards != null)
			{
				this.m_rewards.SetActive(flag);
			}
			if (flag)
			{
				flag2 = !mailData.isReward;
				this.CreateRewards();
			}
			if (this.m_awardBt != null)
			{
				this.m_awardBt.gameObject.SetActive(flag2);
			}
			if (this.m_okBt != null)
			{
				this.m_okBt.gameObject.SetActive(!flag2);
			}
			if (this.m_content != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_content);
			}
			if (this.m_infoTxtElement != null)
			{
				this.m_infoTxtElement.minHeight = this.m_scroll.sizeDelta.y - 170f;
			}
			if (this.m_scrollContent != null)
			{
				LayoutRebuilder.MarkLayoutForRebuild(this.m_scrollContent);
			}
			if (this.m_rewardsContent != null)
			{
				LayoutRebuilder.MarkLayoutForRebuild(this.m_rewardsContent);
			}
			if (this.m_content != null)
			{
				LayoutRebuilder.MarkLayoutForRebuild(this.m_content);
			}
		}

		private void CreateRewards()
		{
			if (this.m_rewardNodePrefab == null)
			{
				return;
			}
			for (int i = 0; i < this.m_openData.m_mailData.rewards.Length; i++)
			{
				MailReward mailReward = this.m_openData.m_mailData.rewards[i];
				if (mailReward != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_rewardNodePrefab);
					gameObject.transform.SetParentNormal(this.m_rewardsContent, false);
					MailInfoRewardNode component = gameObject.GetComponent<MailInfoRewardNode>();
					component.SetActive(true);
					component.Init();
					component.RefreshUI(mailReward, this.m_openData.m_mailData.isReward, i);
					this.m_nodes[gameObject.GetInstanceID()] = component;
				}
			}
		}

		private void DestroyRewards()
		{
			foreach (KeyValuePair<int, MailInfoRewardNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_nodes.Clear();
		}

		public UIPopCommon m_popCommon;

		public CustomText m_titleTxt;

		public RectTransform m_content;

		public RectTransform m_scroll;

		public RectTransform m_scrollContent;

		public TMP_Text m_infoTxt;

		public LayoutElement m_infoTxtElement;

		public CustomText m_sendTxt;

		public CustomText m_endTimeTxt;

		public GameObject m_rewards;

		public RectTransform m_rewardsContent;

		public GameObject m_rewardNodePrefab;

		public GameObject m_buttons;

		public CustomButton m_awardBt;

		public CustomButton m_okBt;

		public Dictionary<int, MailInfoRewardNode> m_nodes = new Dictionary<int, MailInfoRewardNode>();

		public MailInfoViewModule.OpenData m_openData;

		public class OpenData
		{
			public MailData m_mailData;
		}
	}
}
