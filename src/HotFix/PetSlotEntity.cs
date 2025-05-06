using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Server;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotFix
{
	public class PetSlotEntity : PetEntityBase, IPointerDownHandler, IEventSystemHandler
	{
		public override void Init(PetRanchGroup ranchGroup, ulong rowId)
		{
			this.petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			if (this._isInit)
			{
				return;
			}
			base.Init(ranchGroup, rowId);
			this._entityType = PetEntityBase.EntityType.Slot;
		}

		public void SetSlotType(EPetFormationType type, int assistSlotIdx, Action click)
		{
			this._click = click;
			this.SetSlotType(type, assistSlotIdx);
		}

		public void SetSlotType(EPetFormationType type, int assistSlotIdx)
		{
			this._curSlotType = type;
			this._curAssistSlotIdx = assistSlotIdx;
			this.ResetPos();
		}

		public override void SetData(ulong rowId)
		{
			base.SetData(rowId);
			this.uiSpineModelItem.gameObject.SetActive(rowId > 0UL);
		}

		public override async Task LoadSpine(PetEntityBase.EntityType type = PetEntityBase.EntityType.None)
		{
			await base.LoadSpine(PetEntityBase.EntityType.Slot);
			Vector3 localScale = this.uiSpineModelItem.transform.localScale;
			this.uiSpineModelItem.transform.localScale = localScale;
			base.PlayIdleAni();
		}

		public override void RefreshQualityImg()
		{
			base.RefreshQualityImg();
			this.qualityImg.rectTransform.anchoredPosition = Vector2.zero;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
		}

		public override void Update()
		{
			base.Update();
		}

		public override void DragFinish()
		{
			base.DragFinish();
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvas, this._dragPosition, GameApp.View.UICamera, ref vector);
			int num;
			if (this._ranchGroup.IsInPetSlotAssistArea(this._dragPosition, out num))
			{
				EPetFormationType epetFormationType = ((num == 0) ? EPetFormationType.Fight1 : EPetFormationType.Fight2);
				bool flag = this._petDataModule.IsDeployPosUnlock(epetFormationType);
				if (this._curAssistSlotIdx == num)
				{
					Action click = this._click;
					if (click != null)
					{
						click();
					}
				}
				else
				{
					if (!flag)
					{
						this.<DragFinish>g__ResetState|14_0();
						return;
					}
					if (this.petDataModule.IsHavePetInDeployList())
					{
						PetUtil.PetFormationChange(this.curRowId, epetFormationType, delegate(bool isOk)
						{
							if (this._ranchGroup == null)
							{
								return;
							}
							this._ranchGroup.RefreshSlots();
						});
					}
					else
					{
						PetUtil.PetFormationChange(this.curRowId, epetFormationType, delegate(bool isOk)
						{
							if (this._ranchGroup == null)
							{
								return;
							}
							this._ranchGroup.RefreshSlots();
						});
					}
				}
				this.<DragFinish>g__ResetState|14_0();
				return;
			}
			if (this._ranchGroup.IsInGroundArea(vector))
			{
				ulong curRowId = this.curRowId;
				this._ranchGroup.CreateRanchPet(curRowId);
				PetUtil.PetFormationChange(0UL, this._curSlotType, delegate(bool isOk)
				{
					if (this._ranchGroup == null)
					{
						return;
					}
					if (isOk)
					{
						this._ranchGroup.RefreshSlots();
						return;
					}
					this._ranchGroup.RefreshSlots();
					this._ranchGroup.RefreshRanch();
				});
				this.SetData(0UL);
				this.SetSlotType(this._curSlotType, this._curAssistSlotIdx);
				this.<DragFinish>g__ResetState|14_0();
				return;
			}
			this.<DragFinish>g__ResetState|14_0();
		}

		private void ResetPos()
		{
			if (this._ranchGroup == null)
			{
				return;
			}
			EPetFormationType curSlotType = this._curSlotType;
			if (curSlotType - EPetFormationType.Fight1 <= 2)
			{
				this.pet.transform.position = this._ranchGroup.GetAssistSlotPos(this._curAssistSlotIdx);
			}
		}

		public void SetQualityActive(bool active)
		{
			this.qualityImg.gameObject.SetActiveSafe(this.curRowId > 0UL && active);
		}

		public void SetCanvasGroupRaycastState(bool isActive)
		{
			this.cg.blocksRaycasts = isActive;
		}

		public void ResetSpineParent()
		{
			if (this._spine != null)
			{
				this._spine.transform.SetParent(base.transform);
			}
		}

		public void AddGuideButton()
		{
		}

		public void DestroyGuideButton()
		{
		}

		[CompilerGenerated]
		private void <DragFinish>g__ResetState|14_0()
		{
			this.ResetPos();
			base.PlayIdleAni();
		}

		[Header("子类对象")]
		public CanvasGroup cg;

		public Transform guideParent;

		private EPetFormationType _curSlotType = EPetFormationType.Fight1;

		private int _curAssistSlotIdx;

		private Action _click;

		private PetDataModule petDataModule;
	}
}
