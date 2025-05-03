using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Logic;
using Framework.Logic.Component;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class ClientPointController
	{
		public ClientPointController()
		{
			this.m_friendlyInitPoints = new ClientPointData[5];
			this.m_FriendPoints = new ClientPointData[5];
			this.m_EnemyPoints = new ClientPointData[5];
		}

		public async Task OnInit(GameObject goPointController)
		{
			ComponentRegister component = goPointController.GetComponent<ComponentRegister>();
			this.m_FriendlyInitPointsParent = component.GetGameObject("FriendlyInitPoints");
			this.m_FriendPointsParent = component.GetGameObject("FriendlyPoints");
			this.m_EnemyPointsParent = component.GetGameObject("EnemyPoints");
			await Task.WhenAll(new List<Task>
			{
				this.InitPointFriendly(),
				this.InitPointEnemy()
			});
		}

		public async Task OnDeInit()
		{
			await Task.CompletedTask;
		}

		private async Task InitPointFriendly()
		{
			ComponentRegister component = this.m_FriendlyInitPointsParent.GetComponent<ComponentRegister>();
			List<Task> list = new List<Task>();
			for (int i = 0; i < this.m_friendlyInitPoints.Length; i++)
			{
				int num = i;
				string text = string.Format("Point_{0}", num + 1);
				ClientPointData clientPointData = new ClientPointData(component.GetGameObject(text), MemberCamp.Friendly, (MemberPos)num);
				this.m_friendlyInitPoints[i] = clientPointData;
				Task task = clientPointData.OnInit();
				list.Add(task);
			}
			ComponentRegister component2 = this.m_FriendPointsParent.GetComponent<ComponentRegister>();
			for (int j = 0; j < this.m_FriendPoints.Length; j++)
			{
				int num2 = j;
				string text2 = string.Format("Point_{0}", num2 + 1);
				ClientPointData clientPointData2 = new ClientPointData(component2.GetGameObject(text2), MemberCamp.Friendly, (MemberPos)num2);
				this.m_FriendPoints[j] = clientPointData2;
				Task task2 = clientPointData2.OnInit();
				list.Add(task2);
			}
			await Task.WhenAll(list);
		}

		private async Task InitPointEnemy()
		{
			ComponentRegister component = this.m_EnemyPointsParent.GetComponent<ComponentRegister>();
			List<Task> list = new List<Task>();
			for (int i = 0; i < this.m_EnemyPoints.Length; i++)
			{
				int num = i;
				string text = string.Format("Point_{0}", num + 1);
				ClientPointData clientPointData = new ClientPointData(component.GetGameObject(text), MemberCamp.Friendly, (MemberPos)num);
				this.m_EnemyPoints[i] = clientPointData;
				Task task = clientPointData.OnInit();
				list.Add(task);
			}
			await Task.WhenAll(list);
		}

		public ClientPointData GetPointByIndex(MemberCamp camp, MemberPos index)
		{
			if (camp == MemberCamp.Friendly)
			{
				for (int i = 0; i < this.m_FriendPoints.Length; i++)
				{
					ClientPointData clientPointData = this.m_FriendPoints[i];
					if (clientPointData.m_posIndex == index)
					{
						return clientPointData;
					}
				}
			}
			else if (camp == MemberCamp.Enemy)
			{
				for (int j = 0; j < this.m_EnemyPoints.Length; j++)
				{
					ClientPointData clientPointData2 = this.m_EnemyPoints[j];
					if (clientPointData2.m_posIndex == index)
					{
						return clientPointData2;
					}
				}
			}
			HLog.LogError(HLog.ToColor("Function:GetPointByIndex is error.", 3));
			return null;
		}

		public void CreateNextPoint()
		{
			float num = Utility.Math.Random(0f, 2f);
			if (EventMemberController.Instance != null)
			{
				Vector3 currentPos = EventMemberController.Instance.GetCurrentPos();
				this.m_FriendPointsParent.transform.position = new Vector3(currentPos.x + GameConfig.GameBattle_FriendPoint_Distance + num, currentPos.y, 0f);
				this.m_EnemyPointsParent.transform.position = new Vector3(currentPos.x + GameConfig.GameBattle_EnemyPoint_Distance + num, currentPos.y, 0f);
				EventMemberController.Instance.SetEnemyMovePosition(this.m_EnemyPointsParent.transform.position);
			}
		}

		public void CreateWavePoint()
		{
			if (EventMemberController.Instance != null)
			{
				Vector3 currentPos = EventMemberController.Instance.GetCurrentPos();
				this.m_EnemyPointsParent.transform.position = new Vector3(currentPos.x + GameConfig.GameBattle_WavePoint_Distance, currentPos.y, 0f);
				EventMemberController.Instance.SetEnemyMovePosition(this.m_EnemyPointsParent.transform.position);
			}
		}

		private const string FriendlyInitPointsParent = "FriendlyInitPoints";

		private const string FriendlyPointsParent = "FriendlyPoints";

		private const string EnemyPointsParent = "EnemyPoints";

		private const string FriendlyStartPointKey = "Point_{0}";

		private GameObject m_FriendlyInitPointsParent;

		private ClientPointData[] m_friendlyInitPoints;

		private GameObject m_FriendPointsParent;

		private ClientPointData[] m_FriendPoints;

		private GameObject m_EnemyPointsParent;

		private ClientPointData[] m_EnemyPoints;
	}
}
