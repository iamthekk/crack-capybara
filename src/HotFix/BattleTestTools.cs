using System;
using System.Collections.Generic;
using Framework;
using HotFix;
using Server;
using UnityEngine;

public class BattleTestTools : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(103))
		{
			this.AddMainRoleSkill();
		}
		if (Input.GetKeyDown(104))
		{
			this.ClearMainRoleSkill();
		}
		if (Input.GetKeyDown(98))
		{
			this.AddMainPetSkill();
		}
		if (Input.GetKeyDown(110))
		{
			this.ClearMainPetSkill();
		}
		if (Input.GetKeyDown(114))
		{
			this.RefreshEnemyIDs();
		}
		if (Input.GetKeyDown(116))
		{
			this.AddEnemysSkill();
		}
		if (Input.GetKeyDown(121))
		{
			this.ClearEnemysSkill();
		}
		if (Input.GetKeyDown(286))
		{
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					GameApp.State.ActiveState(StateName.BattleTestState);
				});
			});
		}
	}

	private void AddMainRoleSkill()
	{
		if (this.MainAddSkills == null || this.MainAddSkills.Count == 0)
		{
			return;
		}
		List<int> mainRoleAddSkill = Config.MainRoleAddSkill;
		mainRoleAddSkill.Clear();
		mainRoleAddSkill.AddRange(this.MainAddSkills);
	}

	private void ClearMainRoleSkill()
	{
		Config.MainRoleAddSkill.Clear();
	}

	private void AddMainPetSkill()
	{
		if (this.PetAddSkills == null || this.PetAddSkills.Count == 0)
		{
			return;
		}
		List<int> petAddSkill = Config.PetAddSkill;
		petAddSkill.Clear();
		petAddSkill.AddRange(this.PetAddSkills);
	}

	private void ClearMainPetSkill()
	{
		Config.PetAddSkill.Clear();
	}

	private void AddEnemysSkill()
	{
		if (this.EnemysAddSkills == null || this.EnemysAddSkills.Count == 0)
		{
			return;
		}
		List<int> enemysAddSkill = Config.EnemysAddSkill;
		enemysAddSkill.Clear();
		enemysAddSkill.AddRange(this.EnemysAddSkills);
	}

	private void ClearEnemysSkill()
	{
		Config.EnemysAddSkill.Clear();
	}

	private void RefreshEnemyIDs()
	{
		if (this.EnemyMemberIDs == null || this.EnemyMemberIDs.Count == 0)
		{
			return;
		}
		List<int> inspectorEnemyMmbers = Config.InspectorEnemyMmbers;
		inspectorEnemyMmbers.Clear();
		inspectorEnemyMmbers.AddRange(this.EnemyMemberIDs);
	}

	[Space(10f)]
	[Header("主角额外附加Skill ID列表(G & H)")]
	public List<int> MainAddSkills;

	[Header("主角宠物额外附加Skill ID列表(B & N)")]
	public List<int> PetAddSkills;

	[Space(50f)]
	[Header("所有敌人Member ID列表(R)")]
	public List<int> EnemyMemberIDs;

	[Header("所有敌人额外附加Skill ID列表(T & Y)")]
	public List<int> EnemysAddSkills;
}
