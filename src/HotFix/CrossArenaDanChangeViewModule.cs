using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class CrossArenaDanChangeViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.ButtonMask.m_onClick = null;
			this.TapToClose.OnClose = null;
			this.txtColorList.Clear();
			this.imgColorList.Clear();
			for (int i = 0; i < 6; i++)
			{
				Color white = Color.white;
				Color white2 = Color.white;
				if (i == 0)
				{
					ColorUtility.TryParseHtmlString("#83ae89", ref white);
					ColorUtility.TryParseHtmlString("#33af7c", ref white2);
				}
				else if (i == 1)
				{
					ColorUtility.TryParseHtmlString("#c9c6de", ref white);
					ColorUtility.TryParseHtmlString("#405ecc", ref white2);
				}
				else if (i == 2)
				{
					ColorUtility.TryParseHtmlString("#f4dd90", ref white);
					ColorUtility.TryParseHtmlString("#be9936", ref white2);
				}
				else if (i == 3)
				{
					ColorUtility.TryParseHtmlString("#ff8d41", ref white);
					ColorUtility.TryParseHtmlString("#ed622b", ref white2);
				}
				else if (i == 4)
				{
					ColorUtility.TryParseHtmlString("#ec6064", ref white);
					ColorUtility.TryParseHtmlString("#e62525", ref white2);
				}
				else if (i == 5)
				{
					ColorUtility.TryParseHtmlString("#a070f0", ref white);
					ColorUtility.TryParseHtmlString("#811ff0", ref white2);
				}
				this.txtColorList.Add(white);
				this.imgColorList.Add(white2);
			}
		}

		public override void OnOpen(object data)
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.ButtonMask.m_onClick = null;
			this.TapToClose.OnClose = null;
			this.RefreshUI();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_CrossArena_SaveCurrentDanToLocalCache, null);
			this.Anim.Play("open");
			this.mDelayAllowClose = 0.800000011920929;
		}

		private void RefreshUI()
		{
			int num = Mathf.Clamp(this.mDataModule.Dan, 0, this.txtColorList.Count - 1);
			this.TextDan.text = CrossArenaDataModule.GetCrossArenaDanName(this.mDataModule.Dan);
			this.TextDan.color = this.txtColorList[num];
			this.imgDan.sprite = this.imgDanIconList[num];
			this.imgLightEff.color = this.imgColorList[num];
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mDelayAllowClose > 0.0)
			{
				this.mDelayAllowClose -= (double)deltaTime;
				if (this.mDelayAllowClose <= 0.0)
				{
					this.ButtonMask.m_onClick = new Action(this.OnClickCloseThis);
					this.TapToClose.OnClose = new Action(this.OnClickCloseThis);
				}
			}
		}

		public override void OnClose()
		{
			this.mDelayAllowClose = 0.800000011920929;
			this.ButtonMask.m_onClick = null;
			this.TapToClose.OnClose = null;
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickCloseThis()
		{
			GameApp.View.CloseView(ViewName.CrossArenaDanChangeViewModule, null);
		}

		[GameTestMethod("界面", "段位变更显示", "", 0)]
		private static void DanChangeShow()
		{
			GameApp.Data.GetDataModule(DataName.CrossArenaDataModule).Dan = Random.Range(1, 6);
			GameApp.View.OpenView(ViewName.CrossArenaDanChangeViewModule, null, 1, null, null);
		}

		public CustomButton ButtonMask;

		public TapToCloseCtrl TapToClose;

		public CustomText TextDan;

		public CustomImage imgDan;

		public CustomImage imgLightEff;

		public Animator Anim;

		public List<Sprite> imgDanIconList = new List<Sprite>();

		private List<Color> txtColorList = new List<Color>();

		private List<Color> imgColorList = new List<Color>();

		private CrossArenaDataModule mDataModule;

		private double mDelayAllowClose = 0.800000011920929;
	}
}
