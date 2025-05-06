using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class CMemberFactory
	{
		public Dictionary<int, CMemberBase> GetMembers
		{
			get
			{
				return this.m_allMembers;
			}
		}

		public Dictionary<int, CMemberBase> GetBattleMembers
		{
			get
			{
				return this.m_battleMembers;
			}
		}

		public CMemberBase MainMember
		{
			get
			{
				if (this._mainMember == null)
				{
					this._mainMember = this.GetMainMember(MemberCamp.Friendly);
				}
				return this._mainMember;
			}
		}

		public async Task OnInit(BattleClientControllerBase controller)
		{
			this.m_controller = controller;
			await Task.CompletedTask;
		}

		public async Task OnDeInit()
		{
			await this.RemoveAllMember();
			this._mainMember = null;
			await PoolManager.Instance.OnClear();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.list.Clear();
			this.list.AddRange(this.m_allMembers.Values);
			foreach (CMemberBase cmemberBase in this.list)
			{
				if (cmemberBase != null)
				{
					cmemberBase.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public CMemberBase GetMainMember(MemberCamp camp = MemberCamp.Friendly)
		{
			foreach (CMemberBase cmemberBase in this.m_allMembers.Values)
			{
				if (cmemberBase.m_memberData.IsMainMember && cmemberBase.m_memberData.Camp == camp)
				{
					return cmemberBase;
				}
			}
			return null;
		}

		public List<CMemberBase> GetPetMembers()
		{
			List<CMemberBase> list = new List<CMemberBase>();
			foreach (CMemberBase cmemberBase in this.m_allMembers.Values)
			{
				if (cmemberBase.m_memberData.RoleType == MemberRoleType.Pet)
				{
					list.Add(cmemberBase);
				}
			}
			return list;
		}

		public async Task CreateMember(Vector3 position, CMemberData clientMemberData, ClientPointData pointData, Transform parent)
		{
			ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(clientMemberData.m_modelId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("CreateMember .. GameArt_memberModel is Error ..modelID is {0}", clientMemberData.m_modelId));
			}
			else
			{
				string path = elementById.path;
				await PoolManager.Instance.CheckPrefab(path);
				GameObject gameObject = PoolManager.Instance.Out(path, position, pointData.m_rotation, parent);
				gameObject.transform.localScale = pointData.m_scale;
				gameObject.SetActive(true);
				CMemberBase member = new CMemberBase();
				member.SetMemberFactory(this);
				member.SetMemberData(clientMemberData);
				member.SetGameObject(gameObject);
				await member.OnInit();
				this.m_allMembers.Add(clientMemberData.InstanceID, member);
			}
		}

		public async Task CreateMemberLocalTrans(CMemberData clientMemberData, Transform parent)
		{
			ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(clientMemberData.m_modelId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("CreateMember .. GameArt_memberModel is Error ..modelID is {0}", clientMemberData.m_modelId));
			}
			else
			{
				string path = elementById.path;
				await PoolManager.Instance.CheckPrefab(path);
				GameObject obj = PoolManager.Instance.Out(path, Vector3.zero, Quaternion.identity, parent);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
				obj.transform.localRotation = Quaternion.identity;
				obj.SetActive(false);
				CMemberBase member = new CMemberBase();
				member.SetMemberFactory(this);
				member.SetMemberData(clientMemberData);
				member.SetGameObject(obj);
				await member.OnInit();
				obj.SetActive(true);
				member.SetAlpha(0f);
				this.m_allMembers.Add(clientMemberData.InstanceID, member);
			}
		}

		public async Task RemoveAllMember()
		{
			List<CMemberBase> list = this.m_allMembers.Values.ToList<CMemberBase>();
			List<Task> list2 = new List<Task>();
			for (int i = 0; i < list.Count; i++)
			{
				CMemberBase cmemberBase = list[i];
				list2.Add(this.RemoveMember(cmemberBase));
			}
			await Task.WhenAll(list2);
			this.m_allMembers.Clear();
		}

		public async Task RemoveMember(CMemberBase member)
		{
			PoolManager.Instance.Put(member.m_gameObject);
			this.m_allMembers.Remove(member.InstanceID);
			await member.OnDeInit();
		}

		public CMemberBase GetMember(int instanceId)
		{
			CMemberBase cmemberBase;
			if (this.m_allMembers.TryGetValue(instanceId, out cmemberBase))
			{
				return cmemberBase;
			}
			return null;
		}

		public CMemberBase GetMemberByMemberId(int id)
		{
			foreach (CMemberBase cmemberBase in this.m_allMembers.Values)
			{
				if (cmemberBase.m_memberData.m_id == id)
				{
					return cmemberBase;
				}
			}
			return null;
		}

		public List<CMemberBase> GetMembersByCamp(MemberCamp camp)
		{
			List<CMemberBase> list = new List<CMemberBase>();
			List<CMemberBase> list2 = this.m_allMembers.Values.ToList<CMemberBase>();
			for (int i = 0; i < list2.Count; i++)
			{
				CMemberBase cmemberBase = list2[i];
				if (cmemberBase.m_memberData.Camp == camp)
				{
					list.Add(cmemberBase);
				}
			}
			return list;
		}

		public void RefreshMemberCardData(List<BattleChapterMemberData> list)
		{
			this.m_battleMembers.Clear();
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				CMemberBase member = this.GetMember(list[i].id);
				if (member != null)
				{
					member.RefreshCardData(list[i].skillIds, list[i].attributeData, list[i].hp, list[i].recharge, list[i].legacyPower, list[i].isUsedRevive);
					this.m_battleMembers.Add(member.InstanceID, member);
				}
			}
		}

		public void RefreshMemberCardData(List<CardData> cardDatas)
		{
			this.m_battleMembers.Clear();
			if (cardDatas == null)
			{
				return;
			}
			for (int i = 0; i < cardDatas.Count; i++)
			{
				CMemberBase member = this.GetMember(cardDatas[i].m_instanceID);
				if (member != null)
				{
					member.m_memberData.cardData.CloneFrom(cardDatas[i]);
					this.m_battleMembers.Add(member.InstanceID, member);
				}
			}
		}

		public void OnPause(bool pause)
		{
			List<CMemberBase> list = this.m_allMembers.Values.ToList<CMemberBase>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].OnPause(pause);
			}
		}

		public void RefreshBattleAllHp()
		{
			EventArgCrossArenaHpInfo instance = Singleton<EventArgCrossArenaHpInfo>.Instance;
			FP fp;
			FP fp2;
			this.GetHpByCamp(MemberCamp.Friendly, out fp, out fp2);
			FP fp3;
			FP fp4;
			this.GetHpByCamp(MemberCamp.Enemy, out fp3, out fp4);
			instance.SetData(fp, fp2, fp3, fp4);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_CrossArenaHpInfo, instance);
		}

		private void GetHpByCamp(MemberCamp camp, out FP curHp, out FP maxHp)
		{
			curHp = FP._0;
			maxHp = FP._0;
			List<CMemberBase> membersByCamp = this.GetMembersByCamp(camp);
			for (int i = 0; i < membersByCamp.Count; i++)
			{
				CMemberBase cmemberBase = membersByCamp[i];
				if (cmemberBase.m_memberData.cardData.m_isMainMember)
				{
					curHp = cmemberBase.m_memberData.m_curHp;
					maxHp = cmemberBase.m_memberData.m_maxHp;
				}
			}
		}

		public void OnGameStart(BattleReportData_GameStart gameStartData)
		{
			for (int i = 0; i < gameStartData.m_members.Count; i++)
			{
				GameStartMemberData gameStartMemberData = gameStartData.m_members[i];
				CMemberBase member = this.GetMember(gameStartMemberData.m_instanceId);
				if (member != null)
				{
					member.OnGameStart(gameStartMemberData);
					member.m_memberData.SetMaxHp(gameStartMemberData.m_maxHp);
					member.m_memberData.SetCurHp(gameStartMemberData.m_curHp);
					member.m_memberData.SetAttack(gameStartMemberData.m_attack);
					member.m_memberData.SetDefense(gameStartMemberData.m_defense);
					member.m_memberData.SetMaxRecharge(gameStartMemberData.m_maxRecharge);
					member.m_memberData.SetCurRecharge(gameStartMemberData.m_curRecharge);
					foreach (KeyValuePair<int, FP> keyValuePair in gameStartMemberData.m_maxLegacyPower)
					{
						member.m_memberData.SetMaxLegacyPower(keyValuePair.Key, keyValuePair.Value);
					}
					foreach (KeyValuePair<int, FP> keyValuePair2 in gameStartMemberData.m_curLegacyPower)
					{
						member.m_memberData.SetCurLegacyPower(keyValuePair2.Key, keyValuePair2.Value);
					}
					member.m_memberData.SetCurShield(FP._0, false, false);
					member.m_memberData.SetIsUsedRevive(gameStartMemberData.m_isUsedRevive);
					member.RefreshHpHUD();
					member.RefreshShieldHUD();
					member.RefreshRechargeHUD();
					member.RefreshLegacyPowerHUD(0);
					member.ShowHpHUD(true);
					member.RefreshPlayerAttrUI();
				}
			}
		}

		public void OnGameOver(BattleReportData_GameOver gameOverData)
		{
			for (int i = 0; i < gameOverData.m_resultData.m_members.Count; i++)
			{
				OutResultMemberData outResultMemberData = gameOverData.m_resultData.m_members[i];
				CMemberBase member = this.GetMember(outResultMemberData.m_memberInstanceID);
				if (member != null)
				{
					member.m_memberData.SetMaxHp(outResultMemberData.m_maxHp);
					member.m_memberData.SetCurHp(outResultMemberData.m_curHp);
					member.m_memberData.SetAttack(outResultMemberData.m_attack);
					member.m_memberData.SetDefense(outResultMemberData.m_defense);
					member.m_memberData.SetCurShield(FP._0, false, false);
					member.OnGameOver(gameOverData.m_resultData.m_isWin);
					member.RemoveAllBuff();
					member.HideShieldEffect();
					member.RefreshPlayerAttrUI();
				}
			}
		}

		public BattleClientControllerBase m_controller;

		private Dictionary<int, CMemberBase> m_allMembers = new Dictionary<int, CMemberBase>();

		private Dictionary<int, CMemberBase> m_battleMembers = new Dictionary<int, CMemberBase>();

		private CMemberBase _mainMember;

		private List<CMemberBase> list = new List<CMemberBase>();
	}
}
