using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Framework.Logic.AssetReference
{
	[Serializable]
	public class AssetReferenceScriptableObject : AssetReferenceT<ScriptableObject>
	{
		public AssetReferenceScriptableObject(string guid)
			: base(guid)
		{
		}
	}
}
