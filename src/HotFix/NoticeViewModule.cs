using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix;
using HotFix.NoticeViewModule;
using UnityEngine;

public class NoticeViewModule : BaseViewModule
{
	public override void OnCreate(object data)
	{
	}

	private void OnClickClose()
	{
		GameApp.View.CloseView(ViewName.NoticeViewModule, null);
	}

	public override void OnOpen(object data)
	{
		this.Button_Close.m_onClick = new Action(this.OnClickClose);
		this.Button_Mask.m_onClick = new Action(this.OnClickClose);
		this.Text_TitleDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("notice_title_desc");
		this.Obj_NoticeItem.SetActiveSafe(false);
		this.Obj_NetWork.SetActiveSafe(true);
		this.Text_Empty.SetActiveSafe(false);
		Singleton<NoticeManager>.Instance.DoRequestNotice(GameApp.Data.GetDataModule(DataName.LoginDataModule).userId.ToString(), delegate
		{
			this.m_tempDataList.Clear();
			List<NoticeInfoData> noticeInfoDataList = Singleton<NoticeManager>.Instance.GetNoticeInfoDataList();
			for (int i = 0; i < noticeInfoDataList.Count; i++)
			{
				if (noticeInfoDataList[i].isValid())
				{
					this.m_tempDataList.Add(noticeInfoDataList[i]);
				}
			}
			if (this.m_coroutine != null)
			{
				base.StopCoroutine(this.m_coroutine);
			}
			this.m_coroutine = base.StartCoroutine(this.LoadAllImage());
		});
	}

	private IEnumerator LoadAllImage()
	{
		int num;
		for (int i = 0; i < this.m_tempDataList.Count; i = num + 1)
		{
			if (this.m_tempDataList[i].BannerInfoData != null)
			{
				yield return base.StartCoroutine(Singleton<NoticeManager>.Instance.GetImage(this.m_tempDataList[i].BannerInfoData.url, null));
			}
			num = i;
		}
		this.Obj_NetWork.SetActiveSafe(false);
		this.OnRefreshView();
		yield break;
	}

	private void OnRefreshView()
	{
		this.Text_Empty.SetActiveSafe(this.m_tempDataList.Count <= 0);
		this.ScrollView_Content.SetActiveSafe(this.m_tempDataList.Count > 0);
		if (this.m_noticeItemList.Count < this.m_tempDataList.Count)
		{
			int num = this.m_tempDataList.Count - this.m_noticeItemList.Count;
			for (int i = 0; i < num; i++)
			{
				NoticeItem component = Object.Instantiate<GameObject>(this.Obj_NoticeItem, this.Obj_NoticeItemContent.transform).GetComponent<NoticeItem>();
				if (component != null)
				{
					component.Init();
					component.gameObject.SetActiveSafe(true);
					this.m_noticeItemList.Add(component);
				}
			}
		}
		for (int j = 0; j < this.m_noticeItemList.Count; j++)
		{
			if (j >= this.m_tempDataList.Count)
			{
				this.m_noticeItemList[j].gameObject.SetActiveSafe(false);
			}
			else
			{
				this.m_noticeItemList[j].gameObject.SetActiveSafe(true);
				this.m_noticeItemList[j].SetData(this.m_tempDataList[j], j);
			}
		}
	}

	public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
	{
	}

	public override void OnClose()
	{
		this.Button_Close.m_onClick = null;
		this.Button_Mask.m_onClick = null;
		for (int i = 0; i < this.m_noticeItemList.Count; i++)
		{
			this.m_noticeItemList[i].OnClose();
		}
		if (this.m_tweener != null)
		{
			TweenExtensions.Kill(this.m_tweener, false);
		}
		if (this.m_coroutine != null)
		{
			base.StopCoroutine(this.m_coroutine);
		}
	}

	public override void OnDelete()
	{
		for (int i = 0; i < this.m_noticeItemList.Count; i++)
		{
			this.m_noticeItemList[i].DeInit();
		}
	}

	private void OnShowNoticeEvent(object sender, int type, BaseEventArgs eventargs)
	{
		if (eventargs == null)
		{
			return;
		}
		NoticeShowEventArgs noticeShowEventArgs = (NoticeShowEventArgs)eventargs;
		if (noticeShowEventArgs.Index == -1)
		{
			return;
		}
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < this.m_noticeItemList.Count; i++)
		{
			if (i < noticeShowEventArgs.Index)
			{
				num2++;
				num += this.m_noticeItemList[i].GetComponent<RectTransform>().sizeDelta.y;
			}
		}
		num += (float)num2 * this.m_itemSpace;
		if (this.m_tweener != null)
		{
			TweenExtensions.Kill(this.m_tweener, false);
		}
		this.m_tweener = ShortcutExtensions46.DOAnchorPos(this.Obj_NoticeItemContent.GetComponent<RectTransform>(), new Vector2(0f, num), 0.2f, false);
	}

	public override void RegisterEvents(EventSystemManager manager)
	{
		GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Notice_Show, new HandlerEvent(this.OnShowNoticeEvent));
	}

	public override void UnRegisterEvents(EventSystemManager manager)
	{
		GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Notice_Show, new HandlerEvent(this.OnShowNoticeEvent));
	}

	public CustomText Text_TitleDesc;

	public GameObject ScrollView_Content;

	public GameObject Obj_NoticeItemContent;

	public GameObject Text_Empty;

	public CustomButton Button_Close;

	public CustomButton Button_Mask;

	public GameObject Obj_NoticeItem;

	public GameObject Obj_NetWork;

	private Tweener m_tweener;

	private float m_itemSpace = 24f;

	private List<NoticeItem> m_noticeItemList = new List<NoticeItem>();

	private List<NoticeInfoData> m_tempDataList = new List<NoticeInfoData>();

	private Coroutine m_coroutine;
}
