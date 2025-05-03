﻿using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMountModelNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.modelItem.Init();
			this.buttonPrevious.onClick.AddListener(new UnityAction(this.OnClickPrevious));
			this.buttonNext.onClick.AddListener(new UnityAction(this.OnClickNext));
			this.buttonRide.onClick.AddListener(new UnityAction(this.OnClickRide));
			this.copyStarItem.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.modelItem.DeInit();
			this.buttonPrevious.onClick.RemoveListener(new UnityAction(this.OnClickPrevious));
			this.buttonNext.onClick.RemoveListener(new UnityAction(this.OnClickNext));
			this.buttonRide.onClick.RemoveListener(new UnityAction(this.OnClickRide));
			for (int i = 0; i < this.starItems.Count; i++)
			{
				this.starItems[i].DeInit();
			}
			this.starItems.Clear();
		}

		public void OnShow()
		{
			this.modelItem.OnShow();
		}

		public void OnHide()
		{
			this.modelItem.OnHide(false);
		}

		public void SetData(Action onPrevious, Action onNext, Action onRide)
		{
			this.OnPrevious = onPrevious;
			this.OnNext = onNext;
			this.OnRide = onRide;
		}

		public void Refresh(string tName, int memberId, bool isRide, int rideType)
		{
			this.modelItem.ShowMountModel("UIMountModelNodeCtrl", memberId);
			this.textName.text = tName;
			this.buttonRide.gameObject.SetActiveSafe(isRide);
			if (rideType >= 0)
			{
				this.textRide.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_ride");
			}
			else
			{
				this.textRide.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_cancel_ride");
			}
			string mountRideRed = PlayerPrefsKeys.GetMountRideRed();
			this.Com_RideRedNode.gameObject.SetActiveSafe(isRide && rideType >= 0 && string.IsNullOrEmpty(mountRideRed));
		}

		public void RefreshStage(int stage)
		{
			this.stageObj.SetActiveSafe(true);
			this.starObj.SetActiveSafe(false);
			this.textStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_stage", new object[] { stage });
		}

		public void RefreshStar(int curStar, int maxStar)
		{
			this.stageObj.SetActiveSafe(false);
			this.starObj.SetActiveSafe(true);
			for (int i = 0; i < this.starItems.Count; i++)
			{
				this.starItems[i].SetActive(false);
			}
			for (int j = 0; j < maxStar; j++)
			{
				UIStarItem uistarItem;
				if (j < this.starItems.Count)
				{
					uistarItem = this.starItems[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyStarItem.gameObject);
					gameObject.SetParentNormal(this.starLayout, false);
					uistarItem = gameObject.GetComponent<UIStarItem>();
					this.starItems.Add(uistarItem);
					uistarItem.Init();
				}
				uistarItem.SetActive(true);
				uistarItem.SetData(j < curStar);
			}
		}

		private void OnClickPrevious()
		{
			Action onPrevious = this.OnPrevious;
			if (onPrevious == null)
			{
				return;
			}
			onPrevious();
		}

		private void OnClickNext()
		{
			Action onNext = this.OnNext;
			if (onNext == null)
			{
				return;
			}
			onNext();
		}

		private void OnClickRide()
		{
			Action onRide = this.OnRide;
			if (onRide == null)
			{
				return;
			}
			onRide();
		}

		public void ShowPreviousButton(bool isShow)
		{
			this.buttonPrevious.gameObject.SetActiveSafe(isShow);
		}

		public void ShowNextButton(bool isShow)
		{
			this.buttonNext.gameObject.SetActiveSafe(isShow);
		}

		public UIModelItem modelItem;

		public CustomText textName;

		public CustomText textStage;

		public CustomButton buttonPrevious;

		public CustomButton buttonNext;

		public CustomButton buttonRide;

		public CustomText textRide;

		public GameObject starLayout;

		public UIStarItem copyStarItem;

		public GameObject stageObj;

		public GameObject starObj;

		public RedNodeOneCtrl Com_RideRedNode;

		private Action OnPrevious;

		private Action OnNext;

		private Action OnRide;

		private List<UIStarItem> starItems = new List<UIStarItem>();
	}
}
