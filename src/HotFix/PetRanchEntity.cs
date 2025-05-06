using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using Server;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotFix
{
	public class PetRanchEntity : PetEntityBase, IPointerDownHandler, IEventSystemHandler
	{
		public override void Init(PetRanchGroup ranchGroup, ulong rowId)
		{
			if (this._isInit)
			{
				return;
			}
			base.Init(ranchGroup, rowId);
			this._entityType = PetEntityBase.EntityType.Ranch;
			this._parentRT = ranchGroup.petEntityParent;
			base.gameObject.SetActive(true);
		}

		public void InitMapRange(float horMinVal, float horMaxVal, float verMinVal, float verMaxVal, bool isSetPosition = true)
		{
			Vector2 sizeDelta = this.pet.rectTransform.sizeDelta;
			this._horMinVal = horMinVal + sizeDelta.x / 2f;
			this._horMaxVal = horMaxVal - sizeDelta.x / 2f;
			this._verMinVal = verMinVal + sizeDelta.y / 2f;
			this._verMaxVal = verMaxVal - sizeDelta.y / 2f;
			if (isSetPosition)
			{
				this.<InitMapRange>g__SetRandomPos|10_0();
			}
		}

		public override void DeInit()
		{
			base.DeInit();
			this.StopMove();
		}

		public override async Task LoadSpine(PetEntityBase.EntityType type = PetEntityBase.EntityType.None)
		{
			await base.LoadSpine(PetEntityBase.EntityType.Ranch);
			this.PlayMoveAni();
		}

		public override void RefreshQualityImg()
		{
			base.RefreshQualityImg();
		}

		public override void Update()
		{
			base.Update();
			Vector3 localPosition = base.transform.localPosition;
			if (!this.isDrag)
			{
				localPosition.z = (localPosition.y + 1000f) / 20f;
			}
			else
			{
				localPosition.z = localPosition.y - 3000f;
			}
			base.transform.localPosition = localPosition;
		}

		public override void SetData(ulong rowId)
		{
			base.SetData(rowId);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			if (this._ranchGroup && this._ranchGroup.GetDragState() == PetDragController.DragState.None)
			{
				return;
			}
			this.isDrag = true;
			base.PlayInteractAni1();
			this._ranchGroup.OnPetUnitDown();
			this._spine.transform.localPosition -= this._selectZOffset;
			this.StopMove();
		}

		public override void DragFinish()
		{
			base.DragFinish();
			this.isDrag = false;
			this._spine.transform.localPosition += this._selectZOffset;
			int num;
			if (this._ranchGroup.IsInPetSlotAssistArea(this._dragPosition, out num))
			{
				EPetFormationType epetFormationType = num + EPetFormationType.Fight1;
				if (!this._petDataModule.IsDeployPosUnlock(epetFormationType))
				{
					this.ResetState();
					this._ranchGroup.OnPetUnitUp();
					return;
				}
				if (this._petDataModule.IsHavePetInDeployList())
				{
					List<ulong> fightPetRowIds = this._petDataModule.GetFightPetRowIds();
					if (fightPetRowIds[num] > 0UL)
					{
						this._ranchGroup.CreateRanchPet(fightPetRowIds[num]);
					}
					this._ranchGroup.RemoveRanchPet(this.curRowId, true);
				}
				else
				{
					this._ranchGroup.RemoveRanchPet(this.curRowId, true);
				}
				PetUtil.PetFormationChange(this.curRowId, epetFormationType, delegate(bool isOk)
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
			}
			else
			{
				this.ResetState();
			}
			this._ranchGroup.OnPetUnitUp();
		}

		private void ResetState()
		{
			this.PlayMoveAni();
			this.ResetMove();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._seqPool.Clear(false);
		}

		public void Move()
		{
			if (!this._isInit)
			{
				return;
			}
			if (this._lightSeq != null)
			{
				TweenExtensions.Kill(this._lightSeq, false);
				this._lightSeq = null;
			}
			this._lightSeq = this._seqPool.Get();
			Vector2 vector = this.RandomTargetPos();
			float num = this.<Move>g__GetMoveTime|20_1(vector);
			this.<Move>g__CheckRotate|20_2(vector);
			this.PlayMoveAni();
			TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(this._lightSeq, ShortcutExtensions46.DOAnchorPos(this.pet.rectTransform, vector, num, false)), new TweenCallback(base.PlayIdleAni)), Random.Range(1f, 3f)), delegate
			{
				this._lightSeq = null;
				this.Move();
			});
		}

		public void PlayMoveAni()
		{
			try
			{
				PetSpine spine = this._spine;
				if (spine != null)
				{
					spine.Skeleton.SetSlotsToSetupPose();
				}
				PetSpine spine2 = this._spine;
				if (spine2 != null)
				{
					spine2.AnimationState.SetAnimation(0, "Run", true);
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public void StopMove()
		{
			if (this._lightSeq != null)
			{
				TweenExtensions.Kill(this._lightSeq, false);
				this._lightSeq = null;
			}
		}

		public Vector2 RandomTargetPos()
		{
			float num = Random.Range(this._horMinVal, this._horMaxVal);
			float num2 = Random.Range(this._verMinVal, this._verMaxVal);
			return new Vector2(num, num2);
		}

		public void ResetMove()
		{
			this.<ResetMove>g__ResetParent|24_0(false);
			this.<ResetMove>g__SetRaycastTarget|24_1(true);
			this.StopMove();
			this.Move();
		}

		[CompilerGenerated]
		private void <InitMapRange>g__SetRandomPos|10_0()
		{
			if (!this._isInit)
			{
				return;
			}
			Vector2 vector = this.RandomTargetPos();
			this.pet.rectTransform.anchoredPosition = vector;
		}

		[CompilerGenerated]
		private float <Move>g__GetMoveTime|20_1(Vector2 targetPos)
		{
			float num = Mathf.Abs(targetPos.x - this.pet.rectTransform.anchoredPosition.x);
			float num2 = Mathf.Abs(targetPos.y - this.pet.rectTransform.anchoredPosition.y);
			return Mathf.Sqrt(num * num + num2 * num2) / 100f;
		}

		[CompilerGenerated]
		private void <Move>g__CheckRotate|20_2(Vector2 targetPos)
		{
			if (targetPos.x > this.pet.rectTransform.anchoredPosition.x)
			{
				this.pet.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
				return;
			}
			this.pet.rectTransform.localScale = new Vector3(1f, 1f, 1f);
		}

		[CompilerGenerated]
		private void <ResetMove>g__ResetParent|24_0(bool isResetOther)
		{
			base.transform.SetParent(this._parentRT);
			if (isResetOther)
			{
				base.transform.localScale = Vector3.one;
				base.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			}
		}

		[CompilerGenerated]
		private void <ResetMove>g__SetRaycastTarget|24_1(bool isRaycastTarget)
		{
			this.pet.raycastTarget = isRaycastTarget;
		}

		private RectTransform _parentRT;

		private float _horMinVal;

		private float _horMaxVal;

		private float _verMinVal;

		private float _verMaxVal;

		private Sequence _lightSeq;

		private SequencePool _seqPool = new SequencePool();

		private Vector3 _selectZOffset = new Vector3(0f, 0f, 100f);

		private bool isDrag;
	}
}
