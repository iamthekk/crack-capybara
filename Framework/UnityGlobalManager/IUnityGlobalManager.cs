using System;
using UnityEngine;

namespace Framework.UnityGlobalManager
{
	public interface IUnityGlobalManager
	{
		CurveScriptable GetCurve();

		GameObject GetGlobalGameObject(string path);

		void Load(Action finished);

		void UnLoad(Action finished);
	}
}
