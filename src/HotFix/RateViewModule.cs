using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class RateViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.Button_NormalClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.Button_GoodClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.Button_BadClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			for (int i = 0; i < this.HeartItems_Normal.Count; i++)
			{
				this.HeartItems_Normal[i].Init(i + 1, new Action<int>(this.OnClickHeartNormal));
			}
			this.Button_Rate.onClick.AddListener(new UnityAction(this.OnClickRate));
			this.Button_Feeadback.onClick.AddListener(new UnityAction(this.OnClickFeedback));
		}

		public override void OnOpen(object data)
		{
			this.Panel_Normal.gameObject.SetActive(true);
			base.StartCoroutine(this.DelayShowCloseBtn(this.Button_NormalClose, 2f));
			this.Panel_Good.gameObject.SetActive(false);
			this.Panel_Bad.gameObject.SetActive(false);
			this.SetHeartCount(this.HeartItems_Normal, 0);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			base.StopAllCoroutines();
		}

		public override void OnDelete()
		{
			this.Button_Feeadback.onClick.RemoveListener(new UnityAction(this.OnClickFeedback));
			this.Button_Rate.onClick.RemoveListener(new UnityAction(this.OnClickRate));
			for (int i = 0; i < this.HeartItems_Normal.Count; i++)
			{
				this.HeartItems_Normal[i].Clear();
			}
			this.Button_NormalClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.Button_GoodClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.Button_BadClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void SetHeartCount(List<RateViewModule_HeartItem> heartItems, int count)
		{
			for (int i = 0; i < heartItems.Count; i++)
			{
				heartItems[i].SetShow(i < count);
			}
		}

		private void OnClickClose()
		{
			if (this._isPlayNormal)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.RateViewModule, null);
			PlayerPrefsKeys.SaveIsRateShow(true);
		}

		private void OnClickHeartNormal(int index)
		{
			if (this._isPlayNormal)
			{
				return;
			}
			base.StartCoroutine(this.OnClickNormalIE(index));
		}

		private IEnumerator OnClickNormalIE(int index)
		{
			this._isPlayNormal = true;
			this.SetHeartCount(this.HeartItems_Normal, index);
			yield return new WaitForSeconds(0.5f);
			this.Panel_Normal.gameObject.SetActive(false);
			if (index <= 3)
			{
				this.Panel_Good.gameObject.SetActive(false);
				this.Panel_Bad.gameObject.SetActive(true);
			}
			else
			{
				this.Panel_Good.gameObject.SetActive(true);
				base.StartCoroutine(this.DelayShowCloseBtn(this.Button_GoodClose, 2f));
				this.Panel_Bad.gameObject.SetActive(false);
				this.SetHeartCount(this.HeartItems_Good, index);
				this.Text_GoodInfo.ChangeLanguageID("rate_good_info_google");
			}
			this._isPlayNormal = false;
			yield break;
		}

		private IEnumerator DelayShowCloseBtn(Button btn, float delay = 2f)
		{
			btn.gameObject.SetActive(false);
			yield return new WaitForSeconds(delay);
			btn.gameObject.SetActive(true);
			yield break;
		}

		private void OnClickRate()
		{
			GameApp.SDK.Rate.OpenRate();
			GameApp.View.CloseView(ViewName.RateViewModule, null);
			PlayerPrefsKeys.SaveIsRateShow(true);
		}

		private void OnClickFeedback()
		{
			string text = "capybarago@habby.com";
			Application.OpenURL("mailto:" + text);
			GameApp.View.CloseView(ViewName.RateViewModule, null);
			PlayerPrefsKeys.SaveIsRateShow(true);
		}

		public GameObject Panel_Normal;

		public GameObject Panel_Good;

		public GameObject Panel_Bad;

		public List<RateViewModule_HeartItem> HeartItems_Normal;

		public List<RateViewModule_HeartItem> HeartItems_Good;

		public CustomLanguageText Text_GoodInfo;

		public Button Button_Rate;

		public Button Button_Feeadback;

		public Button Button_NormalClose;

		public Button Button_GoodClose;

		public Button Button_BadClose;

		private bool _isPlayNormal;
	}
}
