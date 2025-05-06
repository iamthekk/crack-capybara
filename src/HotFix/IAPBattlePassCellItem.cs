using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class IAPBattlePassCellItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Item.Init();
		}

		protected override void OnDeInit()
		{
			this.Item.DeInit();
		}

		public void SetData(PropData propData)
		{
			this.Item.SetData(propData);
		}

		public void OnRefresh()
		{
			this.Item.OnRefresh();
		}

		public void SetBattlePassState(bool ismask, bool islock, bool isgetted, bool isCanCollect)
		{
			if (this.Obj_Mask)
			{
				this.Obj_Mask.SetActive(ismask);
			}
			if (this.Obj_Lock)
			{
				this.Obj_Lock.SetActive(islock);
			}
			if (this.Obj_Getted)
			{
				this.Obj_Getted.SetActive(isgetted);
			}
			if (this.Obj_CanCollect)
			{
				this.Obj_CanCollect.SetActive(isCanCollect);
			}
			if (this.redNode)
			{
				this.redNode.SetActive(isCanCollect);
			}
		}

		public UIItem Item;

		public GameObject Obj_Lock;

		public GameObject Obj_Mask;

		public GameObject Obj_Getted;

		public GameObject Obj_CanCollect;

		public GameObject redNode;
	}
}
