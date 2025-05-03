using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.Modules;
using HotFix.Client;
using UnityEngine;

namespace HotFix
{
	public static class ModelUtils
	{
		public static void PlayerRideMount(GameObject mountObj, GameObject mountPlayerPoint, GameObject playerShakeRoot, GameObject playerSpineRoot)
		{
			if (mountObj == null || mountPlayerPoint == null || playerShakeRoot == null || playerSpineRoot == null)
			{
				return;
			}
			mountObj.transform.SetParentNormal(playerShakeRoot, false);
			GameObjectExpand.SetLayer(mountObj, playerShakeRoot.layer, true);
			Vector3 localScale = playerSpineRoot.transform.localScale;
			playerSpineRoot.transform.SetParentNormal(mountPlayerPoint, false);
			playerSpineRoot.transform.localScale = localScale;
		}

		public static void CreateBattlePlayerMountModel(CMemberBase player)
		{
			int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(null);
			if (mountMemberId == 0)
			{
				return;
			}
			ModelUtils.CreateBattleMountModel(player, mountMemberId);
		}

		public static void CreateBattleMountModel(CMemberBase player, int enemyMountType, int enemyMountId)
		{
			int num = MountDataModule.CheckMountMemberId(enemyMountType, enemyMountId);
			if (num == 0)
			{
				return;
			}
			ModelUtils.CreateBattleMountModel(player, num);
		}

		private static void CreateBattleMountModel(CMemberBase cMember, int mountMemberId)
		{
			ModelUtils.<CreateBattleMountModel>d__3 <CreateBattleMountModel>d__;
			<CreateBattleMountModel>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CreateBattleMountModel>d__.cMember = cMember;
			<CreateBattleMountModel>d__.mountMemberId = mountMemberId;
			<CreateBattleMountModel>d__.<>1__state = -1;
			<CreateBattleMountModel>d__.<>t__builder.Start<ModelUtils.<CreateBattleMountModel>d__3>(ref <CreateBattleMountModel>d__);
		}

		public static async Task CreatePlayerMember(GameObject modelParent, TaskOutValue<MainMemberModeItemData> outPlayer)
		{
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			Dictionary<SkinType, SkinData> skinDatas = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData.GetSkinDatas();
			int weaponId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId();
			await ModelUtils.CreatePlayerMember(dataModule.MainCardData.m_memberID, weaponId, modelParent, skinDatas, outPlayer);
		}

		public static Task CreatePlayerMember(int playerMemberId, int weaponId, GameObject modelParent, Dictionary<SkinType, SkinData> skinDatas, TaskOutValue<MainMemberModeItemData> outPlayer)
		{
			ModelUtils.<CreatePlayerMember>d__5 <CreatePlayerMember>d__;
			<CreatePlayerMember>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreatePlayerMember>d__.playerMemberId = playerMemberId;
			<CreatePlayerMember>d__.weaponId = weaponId;
			<CreatePlayerMember>d__.modelParent = modelParent;
			<CreatePlayerMember>d__.skinDatas = skinDatas;
			<CreatePlayerMember>d__.outPlayer = outPlayer;
			<CreatePlayerMember>d__.<>1__state = -1;
			<CreatePlayerMember>d__.<>t__builder.Start<ModelUtils.<CreatePlayerMember>d__5>(ref <CreatePlayerMember>d__);
			return <CreatePlayerMember>d__.<>t__builder.Task;
		}

		public static async Task CreateSelfPlayerMount(MainMemberModeItemData mainMemberData, TaskOutValue<MountMemberModeItemData> outMount)
		{
			if (mainMemberData != null)
			{
				int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(null);
				if (mountMemberId != 0)
				{
					await ModelUtils.CreatePlayerMount(mainMemberData, mountMemberId, outMount);
				}
			}
		}

		public static async Task CreatePlayerMount(MainMemberModeItemData mainMemberData, int mountId, TaskOutValue<MountMemberModeItemData> outMount)
		{
			if (mainMemberData != null)
			{
				if (mountId != 0)
				{
					await ModelUtils.CreateMountModel(mountId, null, outMount);
					if (outMount.Value != null && outMount.Value.MountSpinePlayer != null)
					{
						ComponentRegister component = outMount.Value.MountMemberRoot.GetComponent<ComponentRegister>();
						if (component)
						{
							GameObject gameObject = component.GetGameObject("Point");
							ModelUtils.PlayerRideMount(outMount.Value.MountMemberRoot, gameObject, mainMemberData.MainMemberShakeRoot, mainMemberData.MainMemberSpineRoot);
							if (mainMemberData.MainMemberSpinePlayer != null)
							{
								int weaponLayer = mainMemberData.MainMemberSpinePlayer.GetWeaponLayer();
								int maxOrderLayer = mainMemberData.MainMemberSpinePlayer.GetMaxOrderLayer();
								outMount.Value.MountSpinePlayer.SetBackOrderLayer(weaponLayer + 1);
								outMount.Value.MountSpinePlayer.SetFrontOrderLayer(maxOrderLayer + 1);
							}
						}
					}
				}
			}
		}

		public static Task CreateMountModel(int mountMemberId, GameObject parent, TaskOutValue<MountMemberModeItemData> outMount)
		{
			ModelUtils.<CreateMountModel>d__8 <CreateMountModel>d__;
			<CreateMountModel>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateMountModel>d__.mountMemberId = mountMemberId;
			<CreateMountModel>d__.parent = parent;
			<CreateMountModel>d__.outMount = outMount;
			<CreateMountModel>d__.<>1__state = -1;
			<CreateMountModel>d__.<>t__builder.Start<ModelUtils.<CreateMountModel>d__8>(ref <CreateMountModel>d__);
			return <CreateMountModel>d__.<>t__builder.Task;
		}

		public static Vector3 GetMemberScale(string str)
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
	}
}
