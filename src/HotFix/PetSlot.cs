using System;
using Framework;
using Framework.Logic.UI;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PetSlot : MonoBehaviour
	{
		public void Init(int assistSlotIdx, PetRanchGroup ranchGroup)
		{
			this._petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this._assistSlotIdx = assistSlotIdx;
			if (this.redNode != null)
			{
				this.redNode.SetType(240);
			}
			this.petSlotEntity.Init(ranchGroup, 0UL);
			this.petSlotEntity.SetSlotType(this.ePetSlotType, assistSlotIdx, new Action(this.OnBtnClick));
			this.SetBlockerState(false);
			this.RefreshPetData();
			this.RefreshRedNode(null);
		}

		public bool IsHavePetUnit()
		{
			return this._curRowId != 0UL;
		}

		public void RefreshPetData()
		{
			EPetFormationType epetFormationType = this.ePetSlotType;
			if (epetFormationType - EPetFormationType.Fight1 <= 2)
			{
				ulong num = this._petDataModule.GetFightPetRowIds()[this._assistSlotIdx];
				this.SetPetRowId(num);
			}
		}

		public void ResetPetRowId(bool isRefreshRed = true)
		{
			this._curRowId = 0UL;
			this.petSlotEntity.SetData(this._curRowId);
			this.petSlotEntity.SetSlotType(this.ePetSlotType, this._assistSlotIdx);
			this.petSlotEntity.SetQualityActive(this._curRowId > 0UL);
			if (isRefreshRed)
			{
				this.RefreshRedNode(null);
			}
		}

		public void SetPetRowId(ulong rowId)
		{
			this.ResetPetRowId(false);
			this._curRowId = rowId;
			PetData petData = this._petDataModule.GetPetData(rowId);
			this.petSlotEntity.SetData(this._curRowId);
			this.petSlotEntity.SetSlotType(this.ePetSlotType, this._assistSlotIdx);
			this.petSlotEntity.SetQualityActive(this._curRowId > 0UL);
			this.RefreshAddTagActive(petData == null);
			this.RefreshRedNode(null);
		}

		private void RefreshAddTagActive(bool isShow)
		{
			bool flag = this._petDataModule.IsDeployPosUnlock(this.ePetSlotType);
			this.goLock.gameObject.SetActiveSafe(!flag);
			if (flag)
			{
				this.goPlus.SetActiveSafe(isShow);
				return;
			}
			this.goPlus.SetActiveSafe(false);
		}

		private void RefreshRedNode(PetData petData = null)
		{
			this.redNode == null;
		}

		public ulong GetPetRowId()
		{
			return this._curRowId;
		}

		public bool IsInRect(Vector2 pos)
		{
			return RectTransformUtility.RectangleContainsScreenPoint(this.validMoveAreaRT, pos, GameApp.View.UICamera);
		}

		public void SetBlockerState(bool state)
		{
			this.petSlotEntity.SetCanvasGroupRaycastState(!state);
		}

		internal void AddListener()
		{
		}

		internal void RemoveListener()
		{
		}

		private void OnBtnClick()
		{
			if (this._petDataModule.IsDeployPosUnlock(this.ePetSlotType))
			{
				this.petRanchGroup.ShowSelectPetGroup(this.ePetSlotType, this._curRowId);
				this.IsHavePetUnit();
				return;
			}
			if (this.ePetSlotType == EPetFormationType.Fight1)
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(62));
				return;
			}
			if (this.ePetSlotType == EPetFormationType.Fight2)
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(63));
				return;
			}
			if (this.ePetSlotType == EPetFormationType.Fight3)
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(64));
			}
		}

		public PetRanchGroup petRanchGroup;

		public EPetFormationType ePetSlotType;

		public PetSlotEntity petSlotEntity;

		public GameObject goPlus;

		public GameObject goLock;

		public RectTransform validMoveAreaRT;

		public RectTransform idlePosRT;

		[Header("可空对象")]
		public RedNodeOneCtrl redNode;

		private int _assistSlotIdx;

		private ulong _curRowId;

		private PetDataModule _petDataModule;
	}
}
