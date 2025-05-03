using System;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;

namespace HotFix
{
	public class DeepLinkDataModule : IDataModule
	{
		public int GetName()
		{
			return 170;
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

		public bool ParseDeepLinkData()
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(GameApp.DeepLink.DeepLinkParam))
			{
				DeepLinkEnum deepLinkEnum;
				if (Enum.TryParse<DeepLinkEnum>(GameApp.DeepLink.DeepLinkParam, true, out deepLinkEnum))
				{
					flag = this.JumpToDeepLinkPanel(deepLinkEnum);
				}
				else
				{
					HLog.LogError("DeepLink数据 ---The given value is not a valid enum name " + GameApp.DeepLink.DeepLinkParam);
				}
			}
			return flag;
		}

		private bool JumpToDeepLinkPanel(DeepLinkEnum linkEnum)
		{
			bool flag = false;
			switch (linkEnum)
			{
			case DeepLinkEnum.towerChallengePanel:
				flag = Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.Tower, null, true);
				if (flag)
				{
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.Tower, null);
				}
				break;
			case DeepLinkEnum.dungeonDivePanel:
				flag = Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.RogueDungeon, null, true);
				if (flag)
				{
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.RogueDungeon, null);
				}
				break;
			case DeepLinkEnum.arenaPanel:
				flag = Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.CrossArena, null, true);
				if (flag)
				{
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.CrossArena, null);
				}
				break;
			case DeepLinkEnum.newPlayerCarnivalPanel:
				flag = Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.SevenDayCarnival, null, true);
				if (flag)
				{
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.SevenDayCarnival, null);
				}
				break;
			case DeepLinkEnum.equipPanel:
				flag = Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.MainEquip, null, true);
				if (flag)
				{
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainEquip, null);
				}
				break;
			case DeepLinkEnum.petPanel:
				flag = Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.MainEquip_PetList, null, true);
				if (flag)
				{
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainEquip_PetList, null);
				}
				break;
			case DeepLinkEnum.mountPanel:
				flag = Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.Mount, null, true);
				if (flag)
				{
					Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.Mount, null);
				}
				break;
			}
			return flag;
		}
	}
}
