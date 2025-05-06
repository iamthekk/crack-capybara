using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Common;
using Proto.User;

namespace HotFix
{
	public class ClothesDataModule : IDataModule
	{
		public int GetName()
		{
			return 161;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public ClothesData SelfClothesData { get; private set; }

		public SceneSkinData SelfSceneSkinData { get; private set; }

		public ClothesData BattleLeftRoleClothesData
		{
			get
			{
				if (this.battleLeftRoleClothesData == null)
				{
					return this.SelfClothesData;
				}
				return this.battleLeftRoleClothesData;
			}
		}

		public ClothesData BattleRightRoleClothesData
		{
			get
			{
				if (this.battleRightRoleClothesData == null)
				{
					this.battleRightRoleClothesData = new ClothesData();
				}
				return this.battleRightRoleClothesData;
			}
		}

		public void UpdateSelfClothesData(UserInfoDto userInfo, bool isLogin = false)
		{
			if (this.SelfClothesData == null)
			{
				this.SelfClothesData = new ClothesData((int)userInfo.SkinHeaddressId, (int)userInfo.SkinBodyId, (int)userInfo.SkinAccessoryId);
				if (!isLogin)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_ClothesData_SelfClothesChanged, null);
				}
			}
			else if ((long)this.SelfClothesData.HeadId != (long)((ulong)userInfo.SkinHeaddressId) || (long)this.SelfClothesData.BodyId != (long)((ulong)userInfo.SkinBodyId) || (long)this.SelfClothesData.AccessoryId != (long)((ulong)userInfo.SkinAccessoryId))
			{
				this.SelfClothesData.FreshData((int)userInfo.SkinHeaddressId, (int)userInfo.SkinBodyId, (int)userInfo.SkinAccessoryId);
				if (!isLogin)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_ClothesData_SelfClothesChanged, null);
				}
			}
			this.OnUpdateSceneSkinData((int)userInfo.BackGround, isLogin);
		}

		public void UpdateBattleClothesData(bool isLeft, int headId, int bodyId, int accessoryId)
		{
			if (isLeft)
			{
				if (this.battleLeftRoleClothesData == null)
				{
					this.battleLeftRoleClothesData = new ClothesData(headId, bodyId, accessoryId);
					return;
				}
				this.battleLeftRoleClothesData.FreshData(headId, bodyId, accessoryId);
				return;
			}
			else
			{
				if (this.battleRightRoleClothesData == null)
				{
					this.battleRightRoleClothesData = new ClothesData(headId, bodyId, accessoryId);
					return;
				}
				this.battleRightRoleClothesData.FreshData(headId, bodyId, accessoryId);
				return;
			}
		}

		public void ClearBattleClothesData(bool isLeft)
		{
			if (isLeft)
			{
				this.battleLeftRoleClothesData = null;
				return;
			}
			this.battleRightRoleClothesData = null;
		}

		public void OnUpdateSceneSkinData(int skinId, bool isLogin = false)
		{
			if (this.SelfSceneSkinData == null)
			{
				this.SelfSceneSkinData = new SceneSkinData(skinId);
			}
			else
			{
				this.SelfSceneSkinData.OnUpdateSkinId(skinId);
			}
			if (!isLogin)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Change_SceneSkin, null);
			}
		}

		public ClothesData CopySelfClothesData()
		{
			if (this.SelfClothesData == null)
			{
				return null;
			}
			return new ClothesData(this.SelfClothesData.HeadId, this.SelfClothesData.BodyId, this.SelfClothesData.AccessoryId);
		}

		public static ClothesData GetPlayerClothesData(PlayerInfoDto playerInfo)
		{
			return new ClothesData((int)playerInfo.SkinHeaddressId, (int)playerInfo.SkinBodyId, (int)playerInfo.SkinAccessoryId);
		}

		public static ClothesData GetPlayerClothesData(UserRankInfoSimpleDto playerInfo)
		{
			return new ClothesData((int)playerInfo.SkinHeaddressId, (int)playerInfo.SkinBodyId, (int)playerInfo.SkinAccessoryId);
		}

		public static ClothesData GetPlayerClothesData(int headId, int bodyId, int accessoryId)
		{
			return new ClothesData(headId, bodyId, accessoryId);
		}

		public void PushUIModelItem(UIModelItem uiModel, Action onUIModelActive)
		{
			if (uiModel == null || this.uiModelItems.Contains(uiModel))
			{
				return;
			}
			if (onUIModelActive != null)
			{
				this.onUIModelActiveDic[uiModel] = onUIModelActive;
			}
			if (this.uiModelItems.Count > 0)
			{
				this.uiModelItems.Peek().SetCameraVisible(false);
			}
			this.uiModelItems.Push(uiModel);
			uiModel.SetCameraVisible(true);
			if (onUIModelActive != null)
			{
				onUIModelActive();
			}
		}

		public void PopUIModelItem(UIModelItem uiModel)
		{
			if (uiModel == null || this.uiModelItems.Count < 1 || this.uiModelItems.Peek() != uiModel)
			{
				return;
			}
			uiModel.SetCameraVisible(false);
			this.uiModelItems.Pop();
			this.onUIModelActiveDic.Remove(uiModel);
			if (this.uiModelItems.Count > 0)
			{
				UIModelItem uimodelItem = this.uiModelItems.Peek();
				uimodelItem.SetCameraVisible(true);
				Action action;
				if (this.onUIModelActiveDic.TryGetValue(uimodelItem, out action) && action != null)
				{
					action();
				}
			}
		}

		private ClothesData battleLeftRoleClothesData;

		private ClothesData battleRightRoleClothesData;

		private Stack<UIModelItem> uiModelItems = new Stack<UIModelItem>();

		private Dictionary<UIModelItem, Action> onUIModelActiveDic = new Dictionary<UIModelItem, Action>();
	}
}
