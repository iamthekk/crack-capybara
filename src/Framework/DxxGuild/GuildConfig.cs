using System;
using UnityEngine;

namespace Framework.DxxGuild
{
	public class GuildConfig : MonoBehaviour
	{
		public bool IsDebug
		{
			get
			{
				return this.DebugType == GuildConfig.EDebugType.Debug;
			}
		}

		public bool IsEnable
		{
			get
			{
				return this.DebugType < GuildConfig.EDebugType.Disable;
			}
		}

		[Header("调试模式")]
		public GuildConfig.EDebugType DebugType;

		public enum EDebugType
		{
			Release,
			Debug,
			Disable = 15
		}
	}
}
