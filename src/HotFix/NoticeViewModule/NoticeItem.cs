using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.NoticeViewModule
{
	public class NoticeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Obj_NoticeContentItem.SetActiveSafe(false);
			this.Obj_BannerBottom.SetActiveSafe(false);
			this.Button_Down.m_onClick = new Action(this.OnClickButtonDown);
			this.Button_Up.m_onClick = new Action(this.OnClickButtonUp);
			this.Button_Banner.m_onClick = new Action(this.OnClickBanner);
		}

		protected override void OnDeInit()
		{
			this.Button_Down.m_onClick = null;
			this.Button_Up.m_onClick = null;
			this.Button_Banner.m_onClick = null;
			for (int i = 0; i < this.m_noticeContentItemList.Count; i++)
			{
				this.m_noticeContentItemList[i].DeInit();
			}
			this.m_tweener = null;
		}

		public void OnClose()
		{
			base.gameObject.SetActiveSafe(false);
			this.Obj_Bottom.SetActiveSafe(false);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform as RectTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform.parent as RectTransform);
			this.ShowArrowBtn(false);
		}

		private void OnClickButtonDown()
		{
			if (this.m_noticeInfo == null)
			{
				return;
			}
			this.OnClickDownDetail();
		}

		private void OnClickDownDetail()
		{
			this.ShowArrowBtn(true);
			if (this.m_noticeInfo.isNew)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				Singleton<NoticeManager>.Instance.DoRequestNoticeRead(dataModule.userId.ToString(), this.m_noticeInfo.announcementId, delegate
				{
					this.m_noticeInfo.isNew = false;
					this.m_noticeInfo.readed = true;
					this.Obj_New.SetActiveSafe(false);
					base.StartCoroutine(this.ShowContentDetail(true));
				});
				return;
			}
			base.StartCoroutine(this.ShowContentDetail(true));
		}

		private void OnClickButtonUp()
		{
			this.ShowArrowBtn(false);
			base.StartCoroutine(this.ShowContentDetail(false));
		}

		private void OnClickBanner()
		{
			if (this.Button_Down.gameObject.activeSelf)
			{
				this.OnClickDownDetail();
				return;
			}
			this.ShowArrowBtn(false);
			base.StartCoroutine(this.ShowContentDetail(false));
		}

		private void ShowArrowBtn(bool up)
		{
			this.Button_Down.gameObject.SetActive(!up);
			this.Button_Up.gameObject.SetActive(up);
		}

		private IEnumerator ShowContentDetail(bool isShow)
		{
			this.Obj_Bottom.SetActiveSafe(isShow);
			if (isShow)
			{
				for (int i = 0; i < this.m_dataList.Count; i++)
				{
					this.m_noticeContentItemList[i].SetData(this.m_dataList[i], i + 1, this.m_dataList.Count);
				}
				yield return new WaitForEndOfFrame();
				if (this.m_tweener != null)
				{
					TweenExtensions.Kill(this.m_tweener, false);
				}
				NoticeShowEventArgs noticeShowEventArgs = new NoticeShowEventArgs();
				noticeShowEventArgs.Index = this.m_index;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Notice_Show, noticeShowEventArgs);
			}
			yield break;
		}

		public void SetData(NoticeInfoData noticeInfo, int index)
		{
			if (noticeInfo == null)
			{
				return;
			}
			this.m_index = index;
			this.m_noticeInfo = noticeInfo;
			this.Obj_New.SetActiveSafe(noticeInfo.isNew);
			this.Text_Title.text = noticeInfo.title;
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(Singleton<NoticeManager>.Instance.GetImage(noticeInfo.BannerInfoData.url, this.Image_Banner));
			}
			this.m_dataList.Clear();
			string[] array = Regex.Split(noticeInfo.content, this.pattern);
			for (int i = 0; i < array.Length; i++)
			{
				this.m_dataList.Add(array[i]);
			}
			if (this.m_noticeContentItemList.Count > this.m_dataList.Count)
			{
				for (int j = 0; j < this.m_noticeContentItemList.Count; j++)
				{
					if (j < this.m_dataList.Count)
					{
						this.m_noticeContentItemList[j].gameObject.SetActiveSafe(true);
					}
					else
					{
						this.m_noticeContentItemList[j].gameObject.SetActiveSafe(false);
					}
				}
			}
			else
			{
				int num = this.m_dataList.Count - this.m_noticeContentItemList.Count;
				for (int k = 0; k < num; k++)
				{
					NoticeContentItem component = Object.Instantiate<GameObject>(this.Obj_NoticeContentItem, this.Obj_Bottom.transform).GetComponent<NoticeContentItem>();
					if (component != null)
					{
						component.Init();
						component.gameObject.SetActiveSafe(true);
						this.m_noticeContentItemList.Add(component);
					}
				}
			}
			this.Obj_Bottom.SetActiveSafe(false);
			this.ShowArrowBtn(false);
		}

		[Header("Banner")]
		public CustomImage Image_Banner;

		public CustomButton Button_Banner;

		public CustomText Text_Title;

		public CustomText Text_BannerTitle;

		public CustomText Text_BannerContent;

		public GameObject Obj_New;

		public CustomButton Button_Down;

		public CustomButton Button_Up;

		public GameObject Obj_BannerBottom;

		[Header("公告内容")]
		public GameObject Obj_Bottom;

		public GameObject Obj_NoticeContentItem;

		private List<string> m_dataList = new List<string>();

		private List<NoticeContentItem> m_noticeContentItemList = new List<NoticeContentItem>();

		private string pattern = "\\[line\\]";

		public int scrollHeight = 500;

		public int minScrollHeight = 80;

		private NoticeInfoData m_noticeInfo;

		private Tweener m_tweener;

		private int m_index;
	}
}
