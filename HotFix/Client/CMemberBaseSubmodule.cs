using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.Client
{
	public abstract class CMemberBaseSubmodule
	{
		public CMemberBaseSubmodule(CMemberBase member, int commonTableID, MemberBodyPosType bodyPosType)
		{
			this.m_owner = member;
			this.m_commonTableID = commonTableID;
			this.m_bodyPosType = bodyPosType;
		}

		public async Task Init()
		{
			TaskOutValue<GameObject> outObj = new TaskOutValue<GameObject>();
			if (this.m_commonTableID != 0)
			{
				await this.AddObject(outObj, this.m_commonTableID, this.m_bodyPosType);
				this.m_gameObject = outObj.Value;
				this.m_componentRegister = this.m_gameObject.GetComponent<ComponentRegister>();
				this.m_allGameObjects = ViewTools.GetAllGameObjects(this.m_gameObject);
			}
			this.m_initFinished = true;
			this.OnInit();
		}

		public void Update(float deltaTime, float unscaledDeltaTime, float unscaleDeltaTimePause)
		{
			if (!this.m_initFinished)
			{
				return;
			}
			this.OnUpdate(deltaTime, unscaledDeltaTime, unscaleDeltaTimePause);
		}

		public async Task DeInit()
		{
			await this.OnDeInit();
			this.m_componentRegister = null;
			this.m_allGameObjects.Clear();
			if (this.m_gameObject != null)
			{
				this.RemoveObject(this.m_gameObject);
			}
			this.m_gameObject = null;
			this.m_initFinished = false;
		}

		protected abstract void OnInit();

		protected abstract void OnUpdate(float deltaTime, float unscaledDeltaTime, float unscaleDeltaTimePause);

		protected abstract Task OnDeInit();

		public abstract Task OnReset();

		public virtual void OnRoleRoundEnd(int roundCount = 1)
		{
		}

		protected async Task AddObject(TaskOutValue<GameObject> outObj, int commonTableID, MemberBodyPosType bodyPosType)
		{
			if (outObj == null)
			{
				HLog.LogError(HLog.ToColor("AddObject  outObj == null!!", 3));
			}
			else
			{
				Transform transform = this.m_owner.m_body.GetTransform(bodyPosType);
				ArtBuff_Buff artTable = GameApp.Table.GetManager().GetArtBuff_BuffModelInstance().GetElementById(commonTableID);
				await PoolManager.Instance.CheckPrefab(artTable.path);
				Vector3 vector = Vector3.zero;
				Quaternion quaternion = Quaternion.identity;
				if (transform != null)
				{
					vector = transform.position;
					quaternion = transform.rotation;
				}
				else
				{
					HLog.LogError(string.Format("{0} Not found body {1}", this.m_owner.m_memberData.m_id, bodyPosType));
				}
				GameObject gameObject = PoolManager.Instance.Out(artTable.path, vector, quaternion, null);
				if (transform != null)
				{
					gameObject.transform.SetParentNormal(transform, false);
				}
				outObj.SetValue(gameObject);
			}
		}

		protected void RemoveObject(GameObject obj)
		{
			PoolManager.Instance.Put(obj);
		}

		protected CMemberBase m_owner;

		protected int m_commonTableID;

		protected MemberBodyPosType m_bodyPosType;

		protected GameObject m_gameObject;

		protected List<GameObject> m_allGameObjects = new List<GameObject>();

		protected ComponentRegister m_componentRegister;

		protected bool m_initFinished;
	}
}
