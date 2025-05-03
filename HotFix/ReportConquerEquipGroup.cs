using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Proto.Common;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ReportConquerEquipGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			Transform transform = this.UI_HeroDetaiShowCamera.transform;
			transform.SetParent(null);
			transform.localScale = Vector3.one;
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			for (int i = 0; i < 6; i++)
			{
				EquipType equipType = i + EquipType.Weapon;
				ReportConquerEquipNode reportConquerEquipNode = this.m_equipNodes[i];
				reportConquerEquipNode.SetData(equipType);
				reportConquerEquipNode.Init();
				this.m_nodes[equipType] = reportConquerEquipNode;
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_playerModelShow != null)
			{
				this.m_playerModelShow.OnThisDestroy();
			}
			this.m_playerModelShow = null;
			this.m_heroObject = null;
			this.m_icon = null;
			foreach (KeyValuePair<EquipType, ReportConquerEquipNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
		}

		private async void ShowPlayerModel(int memberID)
		{
			this.m_playerModelShow = UIViewPlayerCamera.Get("PlayerInformation.PlayerInformationEquipNode", this.UI_HeroDetaiShowCamera);
			if (this.m_icon != null && this.m_playerModelShow != null)
			{
				Object.DontDestroyOnLoad(this.m_playerModelShow.GObj);
				this.m_icon.gameObject.SetActive(true);
			}
			this.m_playerModelShow.SetCameraTarget(this.m_icon, this.m_icon.rectTransform.rect.size, 1000);
			this.m_playerModelShow.SetOutlineWidth(0.09f);
			this.m_playerModelShow.SetShow(true);
			if (this.m_playerModelShow != null)
			{
				TaskOutValue<GameObject> taskOutValue = new TaskOutValue<GameObject>();
				await this.m_playerModelShow.FindCreatePlayer(taskOutValue, memberID, new Func<GameObject, Task>(this.OnCreateModel));
			}
		}

		private async Task OnCreateModel(GameObject gameObject)
		{
			if (this.m_playerModelShow != null && !(gameObject == null))
			{
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.transform.localPosition = Vector3.zero;
				this.m_heroObject = gameObject;
				ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
				if (component != null)
				{
					GameObject gameObject2 = component.GetGameObject("Model");
					if (gameObject2 != null)
					{
						Animator component2 = gameObject2.GetComponent<Animator>();
						if (component2 != null)
						{
							component2.SetTrigger("Win");
						}
					}
				}
				await Task.CompletedTask;
			}
		}

		public void RefreshUI(BattleUserDto userDto)
		{
			if (userDto == null)
			{
				return;
			}
			Dictionary<long, HeroDto> dictionary = new Dictionary<long, HeroDto>();
			for (int i = 0; i < userDto.Heros.Count; i++)
			{
				HeroDto heroDto = userDto.Heros[i];
				if (heroDto != null)
				{
					dictionary[(long)heroDto.RowId] = heroDto;
				}
			}
			HeroDto heroDto2;
			dictionary.TryGetValue(userDto.ActorRowId, out heroDto2);
			this.ShowPlayerModel((int)heroDto2.HeroId);
			Dictionary<int, EquipData> dictionary2 = new Dictionary<int, EquipData>();
			for (int j = 0; j < userDto.Equips.Count; j++)
			{
				EquipmentDto equipmentDto = userDto.Equips[j];
				if (equipmentDto != null)
				{
					Equip_equip elementById = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipmentDto.EquipId);
					if (elementById != null)
					{
						EquipData equipData = equipmentDto.ToEquipData();
						equipData.SetEquipData(equipmentDto);
						dictionary2[elementById.Type] = equipData;
					}
				}
			}
			foreach (KeyValuePair<EquipType, ReportConquerEquipNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					EquipData equipData2;
					dictionary2.TryGetValue((int)keyValuePair.Value.m_equipType, out equipData2);
					keyValuePair.Value.RefreshData(equipData2);
				}
			}
		}

		public GameObject UI_HeroDetaiShowCamera;

		public UIViewPlayerCamera m_playerModelShow;

		[HideInInspector]
		public GameObject m_heroObject;

		public RawImage m_icon;

		public const int m_equipUpBgCount = 6;

		public List<ReportConquerEquipNode> m_equipNodes;

		public Dictionary<EquipType, ReportConquerEquipNode> m_nodes = new Dictionary<EquipType, ReportConquerEquipNode>(6);
	}
}
