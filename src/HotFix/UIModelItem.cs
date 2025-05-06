using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIModelItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			Transform transform = this.cameraObj.transform;
			this.uiModelShowCameraParent = transform.parent;
			this.rawImage.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
			if (this.uiModelShowCameraParent != null)
			{
				this.cameraObj.transform.SetParent(this.uiModelShowCameraParent);
				this.cameraObj.transform.localPosition = Vector3.zero;
				this.cameraObj.transform.localScale = Vector3.one;
				this.cameraObj.transform.localEulerAngles = Vector3.zero;
			}
			this.ClearModel();
		}

		public void SetCameraVisible(bool visible)
		{
			this.cameraObj.SetActiveSafe(visible);
		}

		public bool IsCameraShow
		{
			get
			{
				return this.cameraObj.activeSelf;
			}
		}

		public void OnShow()
		{
			Transform transform = this.cameraObj.transform;
			transform.SetParent(null);
			transform.localScale = Vector3.one;
			transform.rotation = Quaternion.identity;
		}

		public void OnHide(bool hideImage = false)
		{
			if (this.uiModelShowCameraParent != null)
			{
				this.cameraObj.transform.SetParent(this.uiModelShowCameraParent);
				this.cameraObj.transform.localPosition = Vector3.zero;
				this.cameraObj.transform.localScale = Vector3.one;
				this.cameraObj.transform.localEulerAngles = Vector3.zero;
			}
			if (hideImage)
			{
				this.rawImage.gameObject.SetActive(false);
			}
		}

		public async void ShowSelfPlayerModel(string cameraKey, bool isCreateMount = true)
		{
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			MountDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.MountDataModule);
			int num = (isCreateMount ? dataModule2.GetMountMemberId(null) : 0);
			Dictionary<SkinType, SkinData> skinDatas = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData.GetSkinDatas();
			int weaponId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId();
			this.ShowPlayerModel(cameraKey, dataModule.MainCardData.m_memberID, weaponId, skinDatas, num);
		}

		public async void ShowPlayerModel(string cameraKey, int playerMemberId, int weaponId, Dictionary<SkinType, SkinData> skinDatas, int mountId)
		{
			this.ClearModel();
			this.modelShow = UIViewPlayerCamera.Get(cameraKey, this.cameraObj);
			if (this.rawImage != null && this.modelShow != null)
			{
				Object.DontDestroyOnLoad(this.modelShow.GObj);
				this.modelShow.SetCameraTarget(this.rawImage, this.rawImage.rectTransform.rect.size, 1000);
				this.modelShow.SetOutlineWidth(0.09f);
				this.modelShow.SetShow(true);
				this.rawImage.gameObject.SetActive(false);
				TaskOutValue<MainMemberModeItemData> outPlayer = new TaskOutValue<MainMemberModeItemData>();
				await ModelUtils.CreatePlayerMember(playerMemberId, weaponId, this.modelParent, skinDatas, outPlayer);
				if (outPlayer.Value != null)
				{
					this.mainMemberData = outPlayer.Value;
				}
				if (this.mainMemberData != null && mountId > 0)
				{
					TaskOutValue<MountMemberModeItemData> outMount = new TaskOutValue<MountMemberModeItemData>();
					await ModelUtils.CreatePlayerMount(this.mainMemberData, mountId, outMount);
					if (outMount.Value != null)
					{
						this.mountMemberData = outMount.Value;
					}
					outMount = null;
				}
				this.rawImage.gameObject.SetActive(true);
				outPlayer = null;
			}
		}

		public async void ShowMountModel(string cameraKey, int mountMemberId)
		{
			this.ClearModel();
			this.modelShow = UIViewPlayerCamera.Get(cameraKey, this.cameraObj);
			if (this.rawImage != null && this.modelShow != null)
			{
				Object.DontDestroyOnLoad(this.modelShow.GObj);
				this.modelShow.SetCameraTarget(this.rawImage, this.rawImage.rectTransform.rect.size, 1000);
				this.modelShow.SetOutlineWidth(0.09f);
				this.modelShow.SetShow(true);
				this.rawImage.gameObject.SetActive(false);
				TaskOutValue<MountMemberModeItemData> outMount = new TaskOutValue<MountMemberModeItemData>();
				await ModelUtils.CreateMountModel(mountMemberId, this.modelParent, outMount);
				if (outMount.Value != null)
				{
					this.mountMemberData = outMount.Value;
				}
				this.rawImage.gameObject.SetActive(true);
				outMount = null;
			}
		}

		public bool RefreshPlayerSkins(Dictionary<SkinType, SkinData> skinDatas = null)
		{
			if (this.mainMemberData != null)
			{
				this.mainMemberData.MainMemberSpinePlayer.Refresh(skinDatas);
				this.rawImage.gameObject.SetActive(true);
				return true;
			}
			return false;
		}

		public async void MountChanged()
		{
			if (this.mainMemberData != null)
			{
				if (this.mainMemberData.MainMemberSpineRoot.transform.parent != this.mainMemberData.MainMemberShakeRoot.transform)
				{
					this.mainMemberData.MainMemberSpineRoot.transform.SetParent(this.mainMemberData.MainMemberShakeRoot.transform);
					this.mainMemberData.MainMemberSpineRoot.transform.localPosition = Vector3.zero;
				}
				if (this.mountMemberData != null && this.mountMemberData.MountMemberRoot != null)
				{
					this.mountMemberData.MountSpinePlayer = null;
					Object.Destroy(this.mountMemberData.MountMemberRoot);
				}
				TaskOutValue<MountMemberModeItemData> outMount = new TaskOutValue<MountMemberModeItemData>();
				await ModelUtils.CreateSelfPlayerMount(this.mainMemberData, outMount);
				if (outMount.Value != null)
				{
					this.mountMemberData = outMount.Value;
				}
			}
		}

		public void ClearModel()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.modelParent.transform.childCount; i++)
			{
				GameObject gameObject = this.modelParent.transform.GetChild(i).gameObject;
				list.Add(gameObject);
			}
			for (int j = 0; j < list.Count; j++)
			{
				Object.Destroy(list[j]);
			}
			list.Clear();
			this.mainMemberData = null;
			this.mountMemberData = null;
		}

		public GameObject cameraObj;

		public GameObject modelParent;

		public RawImage rawImage;

		private Transform uiModelShowCameraParent;

		private UIViewPlayerCamera modelShow;

		private MainMemberModeItemData mainMemberData;

		private MountMemberModeItemData mountMemberData;
	}
}
