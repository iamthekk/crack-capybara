using System;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HotFix
{
	public class PetEntityBase : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		public virtual void Init(PetRanchGroup ranchGroup, ulong rowId)
		{
			this._isInit = true;
			this._petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this._ranchGroup = ranchGroup;
			this._canvas = PetViewModule.PetUIPanelRT;
			this._sizeDelta = this.pet.rectTransform.sizeDelta;
			this._reduceScaleVal.Set(this._sizeDelta.x * this._reduceScaleRatioVal, this._sizeDelta.y * this._reduceScaleRatioVal);
			this._normalScaleVal.Set(this._sizeDelta.x, this._sizeDelta.y);
			this.uiSpineModelItem.Init();
			this.SetData(rowId);
		}

		public virtual void SetData(ulong rowId)
		{
			this.curRowId = rowId;
			this._petData = this._petDataModule.GetPetData(this.curRowId);
			this.txt.text = this.curRowId.ToString();
			this.LoadSpine(PetEntityBase.EntityType.None);
			this.RefreshQualityImg();
		}

		public virtual void DeInit()
		{
			this._isInit = false;
			this._entityType = PetEntityBase.EntityType.None;
			this.uiSpineModelItem.DeInit();
		}

		public virtual async Task LoadSpine(PetEntityBase.EntityType type = PetEntityBase.EntityType.None)
		{
			if (type != PetEntityBase.EntityType.None)
			{
				if (this._petData != null)
				{
					int memberId = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this._petData.petId)
						.memberId;
					ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(memberId);
					this.uiSpineModelItem.SetScale(elementById.uiScale);
					await this.uiSpineModelItem.ShowModel(memberId, 0, "Idle", true);
					Vector3 vector;
					vector..ctor(-0.5f, 0.5f, 0.5f);
					this.uiSpineModelItem.transform.localScale = vector;
					SkeletonGraphic component = this.uiSpineModelItem.modelShow.GetComponent<SkeletonGraphic>();
					this._spine = component.gameObject.AddComponent<PetSpine>();
				}
			}
		}

		public virtual void RefreshQualityImg()
		{
			if (this._petData == null)
			{
				return;
			}
			Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(this._petData.quality);
			this.qualityImg.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasId), elementById.imgBottomCircle);
		}

		public void PlayIdleAni()
		{
			if (this._spine == null)
			{
				return;
			}
			this._spine.Skeleton.SetSlotsToSetupPose();
			this._spine.AnimationState.SetAnimation(0, "Idle", true);
		}

		protected void PlayInteractAni1()
		{
			if (this._spine == null)
			{
				return;
			}
			try
			{
				this._spine.AnimationState.End -= new AnimationState.TrackEntryDelegate(this.PlayInteractAni2);
				this._spine.AnimationState.SetAnimation(0, "Interact_1", false);
				this._spine.AnimationState.End += new AnimationState.TrackEntryDelegate(this.PlayInteractAni2);
			}
			catch (Exception ex)
			{
				this._spine.AnimationState.SetAnimation(0, "Idle", true);
				HLog.LogException(ex);
			}
		}

		private void PlayInteractAni2(TrackEntry trackEntry)
		{
			if (this._spine == null)
			{
				return;
			}
			try
			{
				this._spine.AnimationState.End -= new AnimationState.TrackEntryDelegate(this.PlayInteractAni2);
				this._spine.AnimationState.SetAnimation(0, "Interact_2", true);
			}
			catch (Exception ex)
			{
				this._spine.AnimationState.SetAnimation(0, "Idle", true);
				HLog.LogException(ex);
			}
		}

		public void PlayMove()
		{
			if (this._spine == null)
			{
				return;
			}
			try
			{
				this._spine.AnimationState.SetAnimation(0, "Run", true);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		protected Vector2 GetMouseUguiPos()
		{
			return this._mouseUguiPos;
		}

		public virtual void OnPointerDown(PointerEventData eventData)
		{
			if (this._ranchGroup && this._ranchGroup.GetDragState() != PetDragController.DragState.None)
			{
				return;
			}
			this._isDrag = true;
			this.PlayInteractAni1();
			this._dragAcc = 0;
			Vector2 position = eventData.position;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvas, position, eventData.enterEventCamera, ref this._mouseUguiPos))
			{
				this._offset = this.pet.rectTransform.anchoredPosition - this._mouseUguiPos;
			}
			if (this._ranchGroup)
			{
				this._ranchGroup.SetDragPos(eventData.position);
				if (this._entityType == PetEntityBase.EntityType.Ranch)
				{
					this._ranchGroup.StartDragRanchPet(this);
				}
				else if (this._entityType == PetEntityBase.EntityType.Slot)
				{
					this._ranchGroup.StartDragSlotPet(this);
				}
			}
			this.PlayMove();
			base.transform.SetAsLastSibling();
			this.OnReduceScaleChange();
		}

		public virtual void DragFinish()
		{
			if (this._spine != null)
			{
				this._spine.AnimationState.End -= new AnimationState.TrackEntryDelegate(this.PlayInteractAni2);
			}
			this._isDrag = false;
			this._offset = Vector2.zero;
			this.OnNormalScaleChange();
		}

		public virtual void Update()
		{
			if (this._isDrag && this._ranchGroup)
			{
				Vector2 dragPos = this._ranchGroup.GetDragPos();
				Vector2 vector;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvas, dragPos, GameApp.View.UICamera, ref vector))
				{
					this._dragPosition = dragPos;
					Vector2 vector2 = this._offset + vector;
					this.pet.rectTransform.anchoredPosition = vector2;
				}
			}
		}

		public void OnReduceScaleChange()
		{
			this.pet.rectTransform.sizeDelta = this._reduceScaleVal;
		}

		public void OnNormalScaleChange()
		{
			this.pet.rectTransform.sizeDelta = this._normalScaleVal;
		}

		protected virtual void OnDestroy()
		{
		}

		public UISpineModelItem uiSpineModelItem;

		public Image pet;

		public CustomText txt;

		public CustomImage qualityImg;

		[HideInInspector]
		public ulong curRowId;

		protected PetEntityBase.EntityType _entityType;

		protected PetDataModule _petDataModule;

		protected RectTransform _canvas;

		protected float _reduceScaleRatioVal = 0.8f;

		protected Vector2 _reduceScaleVal = Vector2.zero;

		protected Vector2 _normalScaleVal = Vector2.zero;

		protected Vector2 _offset = default(Vector3);

		protected Vector2 _sizeDelta;

		protected PetRanchGroup _ranchGroup;

		protected PetData _petData;

		protected bool _isDrag;

		protected Vector2 _dragPosition;

		protected int _dragAcc;

		protected PetSpine _spine;

		protected bool _isInit;

		private Vector2 _mouseUguiPos;

		public enum EntityType
		{
			None,
			Ranch,
			Slot
		}
	}
}
