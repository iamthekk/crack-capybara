using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI
{
	public class RedNodeGuildCtrl : MonoBehaviour
	{
		private void Update()
		{
			this.CheckGuild();
		}

		public void Register(RedNodeOneCtrl redNodeOneCtrl)
		{
			if (redNodeOneCtrl == null)
			{
				return;
			}
			this.redNodePool.Add(redNodeOneCtrl);
		}

		public void UnRegister(RedNodeOneCtrl redNodeOneCtrl)
		{
			this.redNodePool.Remove(redNodeOneCtrl);
		}

		private void CheckGuild()
		{
			if (GameApp.View == null || GameApp.View.UICamera == null)
			{
				return;
			}
			Camera uicamera = GameApp.View.UICamera;
			this.viewPort.GetWorldCorners(this.containerInScreenUiCorners);
			Vector3 vector = uicamera.WorldToScreenPoint(this.containerInScreenUiCorners[1]);
			Vector3 vector2 = uicamera.WorldToScreenPoint(this.containerInScreenUiCorners[3]);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			foreach (RedNodeOneCtrl redNodeOneCtrl in this.redNodePool)
			{
				if (flag && flag2 && flag3 && flag4)
				{
					break;
				}
				if (redNodeOneCtrl.IsShowing)
				{
					Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(uicamera, redNodeOneCtrl.transform.position);
					if (!flag)
					{
						flag = vector3.x < vector.x;
					}
					if (!flag2)
					{
						flag2 = vector3.x > vector2.x;
					}
					if (!flag3)
					{
						flag3 = vector3.y > vector.y;
					}
					if (!flag4)
					{
						flag4 = vector3.y < vector2.y;
					}
				}
			}
			this.SetGuildLeftOrRight(flag, flag2);
			this.SetGuildUpOrDown(flag3, flag4);
		}

		private void SetGuildUpOrDown(bool isUp, bool isDown)
		{
			this.SetSafeActive(this.guildUpObj, isUp);
			this.SetSafeActive(this.guildDownObj, isDown);
		}

		private void SetGuildLeftOrRight(bool isLeft, bool isRight)
		{
			this.SetSafeActive(this.guildLeftObj, isLeft);
			this.SetSafeActive(this.guildRightObj, isRight);
		}

		private void SetSafeActive(GameObject targetObj, bool isActive)
		{
			if (targetObj == null)
			{
				return;
			}
			targetObj.SetActive(isActive);
		}

		[SerializeField]
		private RectTransform viewPort;

		[SerializeField]
		private GameObject guildUpObj;

		[SerializeField]
		private GameObject guildDownObj;

		[SerializeField]
		private GameObject guildLeftObj;

		[SerializeField]
		private GameObject guildRightObj;

		[SerializeField]
		private List<RedNodeOneCtrl> redNodePool;

		private readonly Vector3[] containerInScreenUiCorners = new Vector3[4];
	}
}
