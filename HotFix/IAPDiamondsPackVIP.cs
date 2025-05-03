using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class IAPDiamondsPackVIP : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mVIPDataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			this.ButtonVIP.m_onClick = new Action(this.OnOpenVIPView);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd.SetData(FlyItemModel.Default, CurrencyType.VIPExp, new List<Vector3> { this.TextVIPLv.transform.position });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CurrecyVIPEXP_RollBack, new HandlerEvent(this.OnRollBackVIPShow));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CurrecyVIPExp_Update, new HandlerEvent(this.OnVIPExpUpdate));
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CurrecyVIPEXP_RollBack, new HandlerEvent(this.OnRollBackVIPShow));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CurrecyVIPExp_Update, new HandlerEvent(this.OnVIPExpUpdate));
		}

		private void OnOpenVIPView()
		{
			GameApp.View.OpenView(ViewName.VIPViewModule, null, 1, null, null);
		}

		public void OnOpen()
		{
			this.m_seqPool.Clear(false);
			this.OnRefreshOnOpen();
		}

		public void OnClose()
		{
			this.m_seqPool.Clear(false);
		}

		private void OnRefreshOnOpen()
		{
			int vipLevel = this.mVIPDataModule.VipLevel;
			int num = 1;
			int vipExp = this.mVIPDataModule.VipExp;
			VIPDataModule.VIPDatas currentVIPDatas = this.mVIPDataModule.GetCurrentVIPDatas();
			VIPDataModule.VIPDatas vipdatas = this.mVIPDataModule.GetVIPDatas(vipLevel + 1);
			int num2;
			if (vipdatas != null)
			{
				if (currentVIPDatas != null)
				{
					num2 = currentVIPDatas.m_exp;
				}
				else
				{
					num2 = 0;
				}
				num = vipdatas.m_exp;
			}
			else
			{
				VIPDataModule.VIPDatas vipdatas2 = this.mVIPDataModule.GetVIPDatas(vipLevel - 1);
				if (vipdatas2 != null)
				{
					num2 = vipdatas2.m_exp;
				}
				else
				{
					num2 = 0;
				}
				if (currentVIPDatas != null)
				{
					num = currentVIPDatas.m_exp;
				}
			}
			float num3 = (float)(vipExp - num2) * 1f / (float)(num - num2);
			this.SliderExp.minValue = 0f;
			this.SliderExp.maxValue = 1f;
			this.SliderExp.value = num3;
			if (vipExp >= num)
			{
				this.TextExp.text = Singleton<LanguageManager>.Instance.GetInfoByID("8812");
			}
			else
			{
				this.TextExp.text = string.Format("{0}/{1}", vipExp, num);
			}
			this.SetVIPLevelShow(vipLevel);
		}

		private void SetVIPLevelShow(int level)
		{
			this.TextVIPLv.text = Singleton<LanguageManager>.Instance.GetInfoByID("8804", new object[] { level.ToString() });
		}

		private int GetVIPByExp(int exp)
		{
			IList<Vip_vip> allElements = GameApp.Table.GetManager().GetVip_vipModelInstance().GetAllElements();
			int num = 0;
			for (int i = 0; i < allElements.Count; i++)
			{
				Vip_vip vip_vip = allElements[i];
				if (vip_vip.Exp > exp)
				{
					break;
				}
				num = vip_vip.id;
			}
			return num;
		}

		private int GetCurrentLevelExp(int currentLevel)
		{
			int num;
			if (currentLevel <= 0)
			{
				num = 0;
			}
			else
			{
				num = this.GetLevelExp(currentLevel);
			}
			return num;
		}

		private int GetNextLevelExp(int currentLevel)
		{
			int count = GameApp.Table.GetManager().GetVip_vipModelInstance().GetAllElements()
				.Count;
			int num = currentLevel + 1;
			if (num > count)
			{
				num = currentLevel;
			}
			return this.GetLevelExp(num);
		}

		private int GetLevelExp(int currentLevel)
		{
			Vip_vip elementById = GameApp.Table.GetManager().GetVip_vipModelInstance().GetElementById(currentLevel);
			int num;
			if (elementById == null)
			{
				HLog.LogError(string.Format("PurchaseShopDiamonds_VIPNode GetLevelExp table is null! id={0}", currentLevel));
				num = 0;
			}
			else
			{
				num = elementById.Exp;
			}
			return num;
		}

		private void RefreshVIPExpShow(int showexp)
		{
			int vipbyExp = this.GetVIPByExp(showexp);
			int currentLevelExp = this.GetCurrentLevelExp(vipbyExp);
			int nextLevelExp = this.GetNextLevelExp(vipbyExp);
			int num = ((showexp > nextLevelExp) ? nextLevelExp : showexp);
			float num2;
			if (nextLevelExp <= currentLevelExp)
			{
				num2 = 1f;
			}
			else
			{
				num2 = (float)(num - currentLevelExp) * 1f / (float)(nextLevelExp - currentLevelExp);
			}
			string text = string.Format("{0}/{1}", num, nextLevelExp);
			this.SetVIPLevelShow(vipbyExp);
			if (vipbyExp != this.m_oldShowVIPLevel)
			{
				this.m_seqPool.Clear(false);
				this.PlayLevelChange();
			}
			if (this.SliderExp != null)
			{
				this.SliderExp.value = num2;
			}
			if (this.TextExp != null)
			{
				this.TextExp.text = text;
			}
			this.m_curShowVIPLevel = vipbyExp;
			this.m_curShowVIPExp = showexp;
			this.m_curShowVIPExpMax = nextLevelExp;
		}

		private void PlayLevelChange()
		{
			Sequence sequence = this.m_seqPool.Get();
			this.RTFVIPLevel.localScale = Vector3.one;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.RTFVIPLevel, 1.2f, 0.15f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.RTFVIPLevel, 1f, 0.15f));
		}

		private void OnRollBackVIPShow(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsVIPExpRollBack eventArgsVIPExpRollBack = eventArgs as EventArgsVIPExpRollBack;
			if (eventArgsVIPExpRollBack == null)
			{
				return;
			}
			this.RefreshVIPExpShow(eventArgsVIPExpRollBack.RollBackVIPExp);
			this.m_oldShowVIPLevel = this.m_curShowVIPLevel;
			this.m_oldVIPLevel = this.m_oldShowVIPLevel;
		}

		private void OnVIPExpUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsInt eventArgsInt = eventArgs as EventArgsInt;
			if (eventArgsInt == null)
			{
				return;
			}
			this.RefreshVIPExpShow(eventArgsInt.m_count);
		}

		public RectTransform RTFVIPLevel;

		public CustomText TextVIPLv;

		public Slider SliderExp;

		public CustomText TextExp;

		public CustomButton ButtonVIP;

		private VIPDataModule mVIPDataModule;

		private SequencePool m_seqPool = new SequencePool();

		private int m_oldVIPLevel;

		private int m_oldShowVIPLevel;

		private int m_curShowVIPLevel;

		private int m_curShowVIPExp;

		private int m_curShowVIPExpMax;
	}
}
