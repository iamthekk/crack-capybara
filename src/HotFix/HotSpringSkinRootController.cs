using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;
using Spine.Unity;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class HotSpringSkinRootController : CustomBehaviour
	{
		protected override void OnInit()
		{
			SkeletonAnimation component = this.HotSpringSkinSpineObj.GetComponent<SkeletonAnimation>();
			if (component != null)
			{
				this.m_hotSpringAni = new SpineAnimator(component);
				this.m_hotSpringAni.PlayAni(0, "01", true, null, null);
				this.m_hotSpringAni.PlayAni(2, "03", true, null, null);
			}
			SkeletonAnimation component2 = this.HotSpringSkinSpineObj1.GetComponent<SkeletonAnimation>();
			if (component2 != null)
			{
				this.m_hotSpringAni1 = new SpineAnimator(component2);
				this.m_hotSpringAni1.PlayAni(1, "02", true, null, null);
			}
			this.SceneCamera = base.GetComponentInChildren<Camera>();
			this.CameraRatio();
			this.OnCreatePlayer();
		}

		private void CameraRatio()
		{
			if (this.SceneCamera != null)
			{
				this.SceneCamera.orthographicSize *= Singleton<QualityManager>.Instance.GetDefaultCameraRatio(false);
			}
		}

		private async Task OnCreatePlayer()
		{
			TaskOutValue<MainMemberModeItemData> outPlayer = new TaskOutValue<MainMemberModeItemData>();
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			ClothesData clothesData = new ClothesData(Singleton<GameConfig>.Instance.ClothesDefaultHeadId, Singleton<GameConfig>.Instance.ClothesDefaultBodyId, Singleton<GameConfig>.Instance.ClothesDefaultAccessoryId);
			int weaponId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId();
			await ModelUtils.CreatePlayerMember(dataModule.MainCardData.m_memberID, weaponId, this.PlayerSlot, clothesData.GetSkinDatas(), outPlayer);
			if (outPlayer.Value != null)
			{
				this.mainMemberData = outPlayer.Value;
			}
			if (this.mainMemberData != null && this.mainMemberData.MainMemberSpinePlayer != null)
			{
				HeroDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
				if (dataModule2 != null)
				{
					GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(dataModule2.MainCardData.m_memberID);
					if (elementById != null)
					{
						ArtMember_member elementById2 = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(elementById.modelID);
						if (elementById2 != null)
						{
							this.BoWenPlayer.transform.localScale = new Vector3(elementById2.hotSpringScale, elementById2.hotSpringScale, elementById2.hotSpringScale);
							this.mainMemberData.MainMemberSpinePlayer.transform.localScale = new Vector3(elementById2.hotSpringModelScale, elementById2.hotSpringModelScale, elementById2.hotSpringModelScale);
						}
					}
				}
				this.mainMemberData.MainMemberSpinePlayer.PlayAnimation("Idle_Water");
			}
			this.OnCreatePets();
		}

		private async Task OnCreatePets()
		{
			if (this.mainMemberData != null && !(this.mainMemberData.MainMemberSpinePlayer == null))
			{
				MainMemberSpinePlayer mainMemberSpinePlayer = this.mainMemberData.MainMemberSpinePlayer;
				int minLayer = mainMemberSpinePlayer.GetMinOrderLayer();
				int maxLayer = mainMemberSpinePlayer.GetMaxOrderLayer();
				PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
				List<CardData> petCards = dataModule.GetFightPetCardData();
				this.BoWenPet1.SetActiveSafe(false);
				this.BoWenPet2.SetActiveSafe(false);
				this.BoWenPet3.SetActiveSafe(false);
				this.petList.Clear();
				this.petObjs.Clear();
				for (int i = 0; i < petCards.Count; i++)
				{
					CardData cardData = petCards[i];
					GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
					ArtMember_member modelTable = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(elementById.modelID);
					Vector3 scale = new Vector3(modelTable.hotSpringModelScale, modelTable.hotSpringModelScale, modelTable.hotSpringModelScale);
					AsyncOperationHandle<GameObject> handler = GameApp.Resources.LoadAssetAsync<GameObject>(modelTable.path);
					await handler.Task;
					if (handler.Result != null)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(handler.Result);
						this.petObjs.Add(gameObject);
						Transform transform = null;
						GameObject gameObject2 = null;
						if (petCards[i].m_posIndex == MemberPos.Two)
						{
							transform = this.PetSlot1.transform;
							gameObject2 = this.BoWenPet1;
							this.BoWenPet1.SetActiveSafe(true);
						}
						else if (petCards[i].m_posIndex == MemberPos.Three)
						{
							transform = this.PetSlot2.transform;
							gameObject2 = this.BoWenPet2;
							this.BoWenPet2.SetActiveSafe(true);
						}
						else if (petCards[i].m_posIndex == MemberPos.Four)
						{
							transform = this.PetSlot3.transform;
							gameObject2 = this.BoWenPet3;
							this.BoWenPet3.SetActiveSafe(true);
						}
						if (transform != null && gameObject2 != null)
						{
							gameObject.SetParentNormal(transform, false);
							gameObject2.transform.localScale = new Vector3(modelTable.hotSpringScale, modelTable.hotSpringScale, modelTable.hotSpringScale);
						}
						gameObject.transform.localPosition = Vector3.zero;
						NormalSpinePlayerPlayer normalSpinePlayerPlayer = gameObject.GetComponentInChildren<NormalSpinePlayerPlayer>(true);
						ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
						if (component)
						{
							component.GetGameObject("SpineRoot").transform.localScale = scale;
						}
						if (normalSpinePlayerPlayer == null)
						{
							normalSpinePlayerPlayer = gameObject.AddComponent<NormalSpinePlayerPlayer>();
						}
						if (normalSpinePlayerPlayer != null)
						{
							normalSpinePlayerPlayer.Init(gameObject.GetComponent<ComponentRegister>());
							normalSpinePlayerPlayer.PlayAnimation("Idle_Water");
							if (petCards[i].m_posIndex == MemberPos.Two || petCards[i].m_posIndex == MemberPos.Three)
							{
								normalSpinePlayerPlayer.SetOrderLayer(maxLayer + 5);
							}
							else
							{
								normalSpinePlayerPlayer.SetOrderLayer(minLayer - 5);
							}
							this.petList.Add(normalSpinePlayerPlayer);
						}
						else
						{
							HLog.LogError("Not found NormalSpinePlayerPlayer in " + modelTable.path);
						}
					}
					modelTable = null;
					scale = default(Vector3);
					handler = default(AsyncOperationHandle<GameObject>);
				}
			}
		}

		protected Vector3 GetScale(string str)
		{
			Vector3 one = Vector3.one;
			if (!string.IsNullOrEmpty(str))
			{
				string[] array = str.Replace("\n", "").Replace(" ", "").Replace("\t", "")
					.Replace("\r", "")
					.Split(',', StringSplitOptions.None);
				if (array.Length > 1)
				{
					float num = float.Parse(array[0]);
					float num2 = float.Parse(array[1]);
					one..ctor(num, num2, 1f);
				}
			}
			return one;
		}

		public async void PetFormationChanged()
		{
			for (int i = 0; i < this.petObjs.Count; i++)
			{
				GameObject gameObject = this.petObjs[i];
				if (gameObject != null)
				{
					Object.DestroyImmediate(gameObject);
				}
			}
			this.petObjs.Clear();
			await this.OnCreatePets();
		}

		public void PlayerClothesChange()
		{
		}

		protected override void OnDeInit()
		{
		}

		public GameObject PlayerSlot;

		public GameObject HotSpringSkinSpineObj;

		public GameObject HotSpringSkinSpineObj1;

		private MainMemberModeItemData mainMemberData;

		protected List<NormalSpinePlayerPlayer> petList = new List<NormalSpinePlayerPlayer>();

		protected List<GameObject> petObjs = new List<GameObject>();

		[Header("Spine")]
		private SpineAnimator m_hotSpringAni;

		private SpineAnimator m_hotSpringAni1;

		[Header("占位")]
		public GameObject PetSlot1;

		public GameObject PetSlot2;

		public GameObject PetSlot3;

		[Header("水波纹特效")]
		public GameObject BoWenPlayer;

		public GameObject BoWenPet1;

		public GameObject BoWenPet2;

		public GameObject BoWenPet3;

		private Camera SceneCamera;
	}
}
