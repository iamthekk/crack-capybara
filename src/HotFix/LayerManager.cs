using System;
using UnityEngine;

namespace HotFix
{
	public class LayerManager
	{
		public static int Default = LayerMask.NameToLayer("Default");

		public static int UI = LayerMask.NameToLayer("UI");

		public static int PutPoint = LayerMask.NameToLayer("PutPoint");

		public static int Rect = LayerMask.NameToLayer("Rect");

		public static int Map = LayerMask.NameToLayer("Map");

		public static int Member = LayerMask.NameToLayer("Member");

		public static int Bullet = LayerMask.NameToLayer("Bullet");

		public static int Skill = LayerMask.NameToLayer("Skill");

		public static int Buff = LayerMask.NameToLayer("Buff");

		public static int GameMask = LayerMask.NameToLayer("GameMask");

		public static int Door = LayerMask.NameToLayer("Door");

		public static int DoorKey = LayerMask.NameToLayer("DoorKey");

		public static int Collect = LayerMask.NameToLayer("Collect");

		public static int BattleHighlight = LayerMask.NameToLayer("BattleHighlight");

		public static int Layer_UI = 1 << LayerManager.UI;

		public static int Layer_Map = 1 << LayerManager.Map;

		public static int Layer_PutPoint = 1 << LayerManager.PutPoint;

		public static int Layer_Rect = 1 << LayerManager.Rect;

		public static int Layer_Member = 1 << LayerManager.Member;

		public static int Layer_Bullet = 1 << LayerManager.Bullet;

		public static int Layer_Skill = 1 << LayerManager.Skill;

		public static int Layer_Buff = 1 << LayerManager.Buff;

		public static int Layer_GameMask = 1 << LayerManager.GameMask;

		public static int Layer_Door = 1 << LayerManager.Door;

		public static int Layer_DoorKey = 1 << LayerManager.DoorKey;

		public static int Layer_Collect = 1 << LayerManager.Collect;

		public static int Layer_BattleHighlight = 1 << LayerManager.BattleHighlight;
	}
}
