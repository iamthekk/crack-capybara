using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HotFix
{
	public class PetRanchGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PetDataModule_ShowIdsChange, new HandlerEvent(this.OnEventShowIdsChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetDrawData, new HandlerEvent(this.OnEventPetDrawDataChange));
			this.guideHand.gameObject.SetActive(false);
			this.petSelectGroup.Init();
			this.petSelectGroup.OnHide();
			this.InitSlot();
			this.CalculateGroundSize();
			this.MapSlotData();
			this.buttonHelp.onClick.AddListener(new UnityAction(this.OnClickHelp));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PetDataModule_ShowIdsChange, new HandlerEvent(this.OnEventShowIdsChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetDrawData, new HandlerEvent(this.OnEventPetDrawDataChange));
			foreach (PetRanchEntity petRanchEntity in this._curRanchPetDict.Values)
			{
				petRanchEntity.DeInit();
			}
			this._curRanchPetDict.Clear();
			this.ReleasePetRanchEntityPool();
			this.buttonHelp.onClick.RemoveListener(new UnityAction(this.OnClickHelp));
		}

		public void OnShow()
		{
			base.SetActive(true);
			this.RefreshRanch();
			if (base.isActiveAndEnabled)
			{
				if (this._coroutineCheckWeakGuide != null)
				{
					base.StopCoroutine(this._coroutineCheckWeakGuide);
					this._coroutineCheckWeakGuide = null;
				}
				this._coroutineCheckWeakGuide = base.StartCoroutine(this.CheckWeakGuide());
			}
		}

		public void OnHide()
		{
			base.SetActive(false);
		}

		private void OnEventShowIdsChange(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshRanch();
		}

		private void OnEventPetDrawDataChange(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshSlots();
		}

		public void RefreshSlots()
		{
			for (int i = 0; i < this.deployPetSlotList.Count; i++)
			{
				this.deployPetSlotList[i].RefreshPetData();
			}
			this.CheckHideHand();
		}

		private void InitSlot()
		{
			for (int i = 0; i < this.deployPetSlotList.Count; i++)
			{
				this.deployPetSlotList[i].Init(i, this);
			}
		}

		private void MapSlotData()
		{
			List<ulong> fightPetRowIds = this.petDataModule.GetFightPetRowIds();
			if (fightPetRowIds != null)
			{
				for (int i = 0; i < fightPetRowIds.Count; i++)
				{
					if (fightPetRowIds[i] > 0UL)
					{
						this.deployPetSlotList[i].SetPetRowId(fightPetRowIds[i]);
					}
					else
					{
						this.deployPetSlotList[i].ResetPetRowId(true);
					}
				}
			}
		}

		private void CalculateGroundSize()
		{
			Vector2 offsetMin = this.ranchAreaImg.rectTransform.offsetMin;
			Vector2 offsetMax = this.ranchAreaImg.rectTransform.offsetMax;
			Vector2 vector = (offsetMin + offsetMax) * 0.5f;
			Rect rect = this.ranchAreaImg.rectTransform.rect;
			float width = rect.width;
			float height = rect.height;
			this._horMinVal = vector.x - width / 2f;
			this._horMaxVal = vector.x + width / 2f;
			this._verMinVal = vector.y - height / 2f;
			this._verMaxVal = vector.y + height / 2f;
		}

		private void ReleasePetRanchEntityPool()
		{
			if (this._petRanchEntityPool.Count > 0)
			{
				foreach (PetRanchEntity petRanchEntity in this._petRanchEntityPool)
				{
					if (petRanchEntity)
					{
						Object.Destroy(petRanchEntity.gameObject);
					}
				}
				this._petRanchEntityPool.Clear();
			}
		}

		public void RemoveRanchPet(ulong rowId, bool isRemove = true)
		{
			PetRanchEntity petRanchEntity;
			if (this._curRanchPetDict.TryGetValue(rowId, out petRanchEntity))
			{
				if (isRemove)
				{
					this._curRanchPetDict.Remove(rowId);
				}
				this.ReturnPetRanchEntityToPool(petRanchEntity);
			}
		}

		public void RemoveAllPetUnit()
		{
			if (this._curRanchPetDict.Count > 0)
			{
				foreach (KeyValuePair<ulong, PetRanchEntity> keyValuePair in this._curRanchPetDict)
				{
					this.RemoveRanchPet(keyValuePair.Value.curRowId, false);
				}
				this._curRanchPetDict.Clear();
			}
		}

		public void RefreshRanch()
		{
			List<ulong> showPetRowIds = this.petDataModule.GetShowPetRowIds();
			List<ulong> list = new List<ulong>();
			foreach (ulong num in this._curRanchPetDict.Keys)
			{
				if (!showPetRowIds.Contains(num))
				{
					list.Add(num);
				}
			}
			for (int i = 0; i < showPetRowIds.Count; i++)
			{
				PetData petData = this.petDataModule.GetPetData(showPetRowIds[i]);
				if (petData != null && petData.formationType != EPetFormationType.Idle)
				{
					list.Add(showPetRowIds[i]);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				PetRanchEntity petRanchEntity;
				if (this._curRanchPetDict.TryGetValue(list[j], out petRanchEntity))
				{
					this.RemoveRanchPet(list[j], true);
				}
			}
			for (int k = 0; k < showPetRowIds.Count; k++)
			{
				ulong num2 = showPetRowIds[k];
				PetData petData2 = this.petDataModule.GetPetData(showPetRowIds[k]);
				if (petData2 != null && petData2.formationType == EPetFormationType.Idle)
				{
					this.CreateRanchPet(num2);
				}
			}
		}

		public void CreateRanchPet(ulong rowId)
		{
			if (rowId <= 0UL)
			{
				return;
			}
			if (this._curRanchPetDict.ContainsKey(rowId))
			{
				return;
			}
			PetRanchEntity petRanchEntityFromPool = this.GetPetRanchEntityFromPool();
			petRanchEntityFromPool.Init(this, rowId);
			petRanchEntityFromPool.InitMapRange(this._horMinVal, this._horMaxVal, this._verMinVal, this._verMaxVal, true);
			petRanchEntityFromPool.Move();
			if (this._curRanchPetDict.ContainsKey(rowId))
			{
				PetRanchEntity petRanchEntity = this._curRanchPetDict[rowId];
				if (petRanchEntity)
				{
					Object.Destroy(petRanchEntity.gameObject);
				}
			}
			this._curRanchPetDict[rowId] = petRanchEntityFromPool;
		}

		private PetRanchEntity GetPetRanchEntityFromPool()
		{
			PetRanchEntity petRanchEntity;
			if (this._petRanchEntityPool.Count > 0)
			{
				petRanchEntity = this._petRanchEntityPool[0];
				petRanchEntity.transform.SetParent(this.petEntityParent, true);
				petRanchEntity.transform.localScale = Vector3.one;
				this._petRanchEntityPool.RemoveAt(0);
			}
			else
			{
				petRanchEntity = Object.Instantiate<PetRanchEntity>(this.petRanchEntityClone, this.petEntityParent, true);
				petRanchEntity.transform.localScale = Vector3.one;
			}
			return petRanchEntity;
		}

		private void ReturnPetRanchEntityToPool(PetRanchEntity petRanchEntity)
		{
			if (petRanchEntity)
			{
				petRanchEntity.DeInit();
				petRanchEntity.transform.SetParent(this.poolParentTf);
				petRanchEntity.transform.localScale = Vector3.one;
				petRanchEntity.gameObject.SetActiveSafe(false);
				this._petRanchEntityPool.Add(petRanchEntity);
			}
		}

		internal Vector3 GetAssistSlotPos(int idx)
		{
			return this.deployPetSlotList[idx].idlePosRT.transform.position;
		}

		public bool IsInPetSlotAssistArea(PointerEventData eventData, out int idx)
		{
			bool flag = false;
			idx = -1;
			for (int i = 0; i < this.deployPetSlotList.Count; i++)
			{
				if (this.deployPetSlotList[i].IsInRect(eventData.position))
				{
					flag = true;
					idx = i;
				}
			}
			return flag;
		}

		public bool IsInPetSlotAssistArea(Vector2 pos, out int idx)
		{
			bool flag = false;
			idx = int.MinValue;
			for (int i = 0; i < this.deployPetSlotList.Count; i++)
			{
				if (this.deployPetSlotList[i].IsInRect(pos))
				{
					flag = true;
					idx = i;
				}
			}
			return flag;
		}

		public void SaveFightSlotRowId(ulong rowId)
		{
			List<ulong> list;
			PetUtil.GetFightPetRowIds(rowId, out list);
		}

		public bool IsInGroundArea(Vector2 pos)
		{
			return pos.x > this._horMinVal && pos.x < this._horMaxVal && pos.y > this._verMinVal && pos.y < this._verMaxVal;
		}

		public void OnPetUnitDown()
		{
			this.RefreshFightPetSlotLayer(EPetFormationType.Fight1);
			this.RefreshFightPetSlotLayer(EPetFormationType.Fight2);
			this.RefreshFightPetSlotLayer(EPetFormationType.Fight3);
			this.petEntityParent.SetParent(this.dynamicUIGroup);
		}

		public void OnPetUnitUp()
		{
			this.CloseDynamicUIGroup();
			this.petEntityParent.SetParent(this.petSlotEntityGroup);
		}

		public PetDragController.DragState GetDragState()
		{
			return this.dragController.GetDragState();
		}

		public void SetDragPos(Vector2 eventDataPosition)
		{
			this.dragController.dragPos = eventDataPosition;
		}

		public void StartDragRanchPet(PetEntityBase petRanchEntity)
		{
			this.dragController.StartDragRanchPet((PetRanchEntity)petRanchEntity);
		}

		public Vector2 GetDragPos()
		{
			return this.dragController.dragPos;
		}

		public void StartDragSlotPet(PetEntityBase petSlotEntity)
		{
			this.dragController.StartDragSlotPet((PetSlotEntity)petSlotEntity);
		}

		public void ShowSelectPetGroup(EPetFormationType formationType, ulong rowId)
		{
			this.petSelectGroup.OnShow(formationType, rowId);
			this.RefreshFightPetSlotLayer(formationType);
			this.CheckHideHand();
		}

		private void RefreshFightPetSlotLayer(EPetFormationType formationType)
		{
			switch (formationType)
			{
			case EPetFormationType.Fight1:
				this.deployUIGroup.deploySlot[0].transform.SetParent(this.dynamicUIGroup);
				this.deployPetSlotList[0].petSlotEntity.transform.SetParent(this.dynamicUIGroup);
				this.deployUIGroup.board.transform.SetParent(this.dynamicUIGroup);
				this.deployUIGroup.board.transform.SetAsLastSibling();
				break;
			case EPetFormationType.Fight2:
				this.deployUIGroup.deploySlot[1].transform.SetParent(this.dynamicUIGroup);
				this.deployPetSlotList[1].petSlotEntity.transform.SetParent(this.dynamicUIGroup);
				this.deployUIGroup.board.transform.SetParent(this.dynamicUIGroup);
				this.deployUIGroup.board.transform.SetAsLastSibling();
				break;
			case EPetFormationType.Fight3:
				this.deployUIGroup.deploySlot[2].transform.SetParent(this.dynamicUIGroup);
				this.deployPetSlotList[2].petSlotEntity.transform.SetParent(this.dynamicUIGroup);
				this.deployUIGroup.board.transform.SetParent(this.dynamicUIGroup);
				this.deployUIGroup.board.transform.SetAsLastSibling();
				break;
			}
			this.dynamicUIGroup.gameObject.SetActive(true);
		}

		public void CloseDynamicUIGroup()
		{
			this.ResetRanchGroupUIParent();
			this.dynamicUIGroup.gameObject.SetActive(false);
		}

		private void ResetRanchGroupUIParent()
		{
			this.deployUIGroup.transform.SetParent(this.assistGroupRTParent);
			this.deployUIGroup.ResetUIParent();
			this.deployPetSlotList.ForEach(delegate(PetSlot p)
			{
				p.petSlotEntity.transform.SetParent(this.petSlotEntityGroup.transform);
			});
		}

		private void Update()
		{
			this.frameCounter++;
			if (this.frameCounter >= this.sortIntervalFrame)
			{
				this.frameCounter = 0;
				this.SortPetRanchLayer();
			}
		}

		private void SortPetRanchLayer()
		{
			List<PetRanchEntity> list = this._curRanchPetDict.Values.ToList<PetRanchEntity>();
			list.Sort((PetRanchEntity a, PetRanchEntity b) => b.transform.localPosition.z.CompareTo(a.transform.localPosition.z));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetSiblingIndex(i);
			}
		}

		private void OnClickHelp()
		{
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("pet_help");
			GameApp.View.ShowStringTip(infoByID);
		}

		private void CheckHideHand()
		{
			if (this.petDataModule.m_petDataDict.Count == 0)
			{
				this.guideHand.gameObject.SetActive(false);
				return;
			}
			if (this.petSelectGroup != null && this.petSelectGroup.isActiveAndEnabled)
			{
				this.guideHand.gameObject.SetActive(false);
				return;
			}
			for (int i = 0; i < this.deployPetSlotList.Count; i++)
			{
				EPetFormationType epetFormationType = i + EPetFormationType.Fight1;
				bool flag = this.petDataModule.IsDeployPosUnlock(epetFormationType);
				bool flag2 = this.petDataModule.IsDeploy(epetFormationType);
				if (this.curHandInIndex == i && (!flag || flag2))
				{
					this.guideHand.gameObject.SetActive(false);
					return;
				}
			}
		}

		private IEnumerator CheckWeakGuide()
		{
			for (;;)
			{
				yield return new WaitForSeconds(3f);
				if (!(this.petSelectGroup != null) || !this.petSelectGroup.isActiveAndEnabled)
				{
					bool flag = false;
					for (int i = 0; i < this.deployPetSlotList.Count; i++)
					{
						EPetFormationType epetFormationType = i + EPetFormationType.Fight1;
						bool flag2 = this.petDataModule.IsDeployPosUnlock(epetFormationType);
						bool flag3 = this.petDataModule.IsDeploy(epetFormationType);
						if (flag2 && !flag3 && this.petDataModule.m_petDataDict.Count > 0)
						{
							this.guideHand.gameObject.SetActive(true);
							this.guideHand.transform.position = this.deployPetSlotList[i].goPlus.transform.position;
							Vector3 localPosition = this.guideHand.transform.localPosition;
							localPosition.y += 100f;
							this.guideHand.transform.localPosition = localPosition;
							flag = true;
							this.curHandInIndex = i;
							break;
						}
					}
					if (!flag)
					{
						this.guideHand.gameObject.SetActive(false);
					}
				}
			}
			yield break;
		}

		public Transform guideHand;

		public PetSelectGroup petSelectGroup;

		public List<PetSlot> deployPetSlotList;

		public Image ranchAreaImg;

		public PetRanchEntity petRanchEntityClone;

		public RectTransform petSlotEntityGroup;

		public RectTransform petEntityParent;

		public Transform mask;

		public RectTransform upLayerGroup;

		public RectTransform upLayerGroupParent;

		[Space(10f)]
		public PetDeployGroup deployUIGroup;

		public RectTransform assistGroupRTParent;

		public Transform poolParentTf;

		public PetDragController dragController;

		public CustomButton buttonHelp;

		[Header("动态层级Group")]
		public Transform dynamicUIGroup;

		private Dictionary<ulong, PetRanchEntity> _curRanchPetDict = new Dictionary<ulong, PetRanchEntity>();

		private List<PetRanchEntity> _petRanchEntityPool = new List<PetRanchEntity>();

		private float _horMinVal;

		private float _horMaxVal;

		private float _verMinVal;

		private float _verMaxVal;

		private PetDataModule petDataModule;

		private Coroutine _coroutineCheckWeakGuide;

		private int sortIntervalFrame = 6;

		private int frameCounter;

		private int curHandInIndex = -1;
	}
}
