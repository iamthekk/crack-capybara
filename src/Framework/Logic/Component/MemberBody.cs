using System;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class MemberBody : MonoBehaviour
	{
		public void SetRoot(Transform root)
		{
			this.m_root = root;
		}

		public Transform GetTransform(MemberBodyPosType posType)
		{
			Transform transform = this.m_center;
			switch (posType)
			{
			case MemberBodyPosType.Foot:
				transform = this.m_foot;
				break;
			case MemberBodyPosType.Pelvis:
				transform = this.m_pelvis;
				break;
			case MemberBodyPosType.Center:
				transform = this.m_center;
				break;
			case MemberBodyPosType.LeftHand:
				transform = this.m_leftHand;
				break;
			case MemberBodyPosType.RightHand:
				transform = this.m_rightHand;
				break;
			case MemberBodyPosType.Head:
				transform = this.m_head;
				break;
			case MemberBodyPosType.Health:
				transform = this.m_health;
				break;
			case MemberBodyPosType.HeadTop:
				transform = this.m_headTop;
				break;
			case MemberBodyPosType.RootParent:
				transform = base.transform;
				break;
			case MemberBodyPosType.PointNpc1:
				transform = this.m_pointNpc1;
				break;
			case MemberBodyPosType.PointNpc2:
				transform = this.m_pointNpc2;
				break;
			case MemberBodyPosType.PointNpc3:
				transform = this.m_pointNpc3;
				break;
			default:
				switch (posType)
				{
				case MemberBodyPosType.WeaponLeftCenter:
					if (this.m_weaponLeftBody != null)
					{
						transform = this.m_weaponLeftBody.GetTransform(WeaponBodyPosType.Center);
					}
					break;
				case MemberBodyPosType.WeaponLeftLocalCenter:
					if (this.m_weaponLeftBody != null)
					{
						transform = this.m_weaponLeftBody.GetTransform(WeaponBodyPosType.LocalCenter);
					}
					break;
				case MemberBodyPosType.WeaponLeftHead:
					if (this.m_weaponLeftBody != null)
					{
						transform = this.m_weaponLeftBody.GetTransform(WeaponBodyPosType.Head);
					}
					break;
				case MemberBodyPosType.WeaponLeftTail:
					if (this.m_weaponLeftBody != null)
					{
						transform = this.m_weaponLeftBody.GetTransform(WeaponBodyPosType.Tail);
					}
					break;
				default:
					switch (posType)
					{
					case MemberBodyPosType.WeaponRightCenter:
						if (this.m_weaponRightBody != null)
						{
							transform = this.m_weaponRightBody.GetTransform(WeaponBodyPosType.Center);
						}
						break;
					case MemberBodyPosType.WeaponRightLocalCenter:
						if (this.m_weaponRightBody != null)
						{
							transform = this.m_weaponRightBody.GetTransform(WeaponBodyPosType.LocalCenter);
						}
						break;
					case MemberBodyPosType.WeaponRightHead:
						if (this.m_weaponRightBody != null)
						{
							transform = this.m_weaponRightBody.GetTransform(WeaponBodyPosType.Head);
						}
						break;
					case MemberBodyPosType.WeaponRightTail:
						if (this.m_weaponRightBody != null)
						{
							transform = this.m_weaponRightBody.GetTransform(WeaponBodyPosType.Tail);
						}
						break;
					}
					break;
				}
				break;
			}
			return transform;
		}

		public Transform m_center;

		public Transform m_pelvis;

		public Transform m_foot;

		public Transform m_leftHand;

		public Transform m_rightHand;

		public Transform m_head;

		public Transform m_health;

		public Transform m_headTop;

		public SkinnedMeshRenderer m_bodyMesh;

		public WeaponBody m_weaponLeftBody;

		public WeaponBody m_weaponRightBody;

		private Transform m_root;

		public Transform m_pointNpc1;

		public Transform m_pointNpc2;

		public Transform m_pointNpc3;
	}
}
