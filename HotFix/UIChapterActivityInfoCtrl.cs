using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIChapterActivityInfoCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			Transform transform = this.cameraObj.transform;
			this.uiModelShowCameraParent = transform.parent;
			this.rawImage.gameObject.SetActive(false);
			for (int i = 0; i < this.itemParent.transform.childCount; i++)
			{
				UIChapterActivityScoreItem component = this.itemParent.transform.GetChild(i).gameObject.GetComponent<UIChapterActivityScoreItem>();
				component.Init();
				this.scoreItems.Add(component);
			}
		}

		protected override void OnDeInit()
		{
			if (this.uiModelShowCameraParent != null)
			{
				this.cameraObj.transform.SetParent(this.uiModelShowCameraParent);
			}
			for (int i = 0; i < this.scoreItems.Count; i++)
			{
				this.scoreItems[i].DeInit();
			}
			this.scoreItems.Clear();
		}

		public void OnShow()
		{
			Transform transform = this.cameraObj.transform;
			transform.SetParent(null);
			transform.localScale = Vector3.one;
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}

		public void OnHide()
		{
			if (this.uiModelShowCameraParent != null)
			{
				this.cameraObj.transform.SetParent(this.uiModelShowCameraParent);
			}
		}

		public void ShowModel(ChapterActivityNormalData activityData)
		{
			this.ShowPlayerModel(activityData);
		}

		private async void ShowPlayerModel(ChapterActivityNormalData activityData)
		{
			this.modelShow = UIViewPlayerCamera.Get("ChapterActivityNormalViewModule", this.cameraObj);
			if (this.rawImage != null && this.modelShow != null)
			{
				Object.DontDestroyOnLoad(this.modelShow.GObj);
				this.modelShow.SetCameraTarget(this.rawImage, this.rawImage.rectTransform.rect.size, 1000);
				this.modelShow.SetOutlineWidth(0.09f);
				this.modelShow.SetShow(true);
				int[] modelArr = activityData.Config.modelShow;
				int[] scoreShow = activityData.Config.scoreShow;
				for (int j = 0; j < this.lineParent.transform.childCount; j++)
				{
					GameObject gameObject = this.lineParent.transform.GetChild(j).gameObject;
					if (!(gameObject == null))
					{
						if (j < modelArr.Length - 1)
						{
							gameObject.SetActiveSafe(true);
						}
						else
						{
							gameObject.SetActiveSafe(false);
						}
					}
				}
				for (int k = 0; k < this.scoreItems.Count; k++)
				{
					if (k < scoreShow.Length)
					{
						this.scoreItems[k].gameObject.SetActiveSafe(true);
						this.scoreItems[k].SetData(activityData.ScoreAtlasId, activityData.ScoreIcon, scoreShow[k]);
					}
					else
					{
						this.scoreItems[k].gameObject.SetActiveSafe(false);
					}
				}
				this.rawImage.gameObject.SetActive(false);
				for (int i = 0; i < modelArr.Length; i++)
				{
					await this.CreateModel(modelArr[i], this.modelParent, modelArr.Length, i);
				}
				this.rawImage.gameObject.SetActive(true);
				modelArr = null;
			}
		}

		private Task CreateModel(int id, GameObject parent, int totalModel, int index)
		{
			UIChapterActivityInfoCtrl.<CreateModel>d__19 <CreateModel>d__;
			<CreateModel>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateModel>d__.<>4__this = this;
			<CreateModel>d__.id = id;
			<CreateModel>d__.parent = parent;
			<CreateModel>d__.totalModel = totalModel;
			<CreateModel>d__.index = index;
			<CreateModel>d__.<>1__state = -1;
			<CreateModel>d__.<>t__builder.Start<UIChapterActivityInfoCtrl.<CreateModel>d__19>(ref <CreateModel>d__);
			return <CreateModel>d__.<>t__builder.Task;
		}

		public GameObject cameraObj;

		public GameObject modelParent;

		public RawImage rawImage;

		public GameObject lineParent;

		public GameObject itemParent;

		public HorizontalLayoutGroup layoutGroup;

		private Transform uiModelShowCameraParent;

		private UIViewPlayerCamera modelShow;

		private List<UIChapterActivityScoreItem> scoreItems = new List<UIChapterActivityScoreItem>();

		private const float TWO_OFFSET_X = 2.3f;

		private const float THREE_OFFSET_X = 3.2f;

		private const float NORMAL_SPACING = 200f;

		private const float TWO_SPACING = 330f;
	}
}
