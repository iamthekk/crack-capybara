using System;
using UnityEngine;

public class MaterialManager
{
	public static void SetMaterialColor(Renderer renderer, string name, Color color)
	{
		renderer.GetPropertyBlock(MaterialManager.prop);
		MaterialManager.prop.SetColor(name, color);
		renderer.SetPropertyBlock(MaterialManager.prop);
		MaterialManager.prop.Clear();
	}

	public static void SetMaterialFloat(Renderer renderer, string name, float value)
	{
		renderer.GetPropertyBlock(MaterialManager.prop);
		MaterialManager.prop.SetFloat(name, value);
		renderer.SetPropertyBlock(MaterialManager.prop);
		MaterialManager.prop.Clear();
	}

	public static Color GetMaterialColor(Renderer renderer, string name)
	{
		return renderer.sharedMaterial.GetColor(name);
	}

	public static float GetMaterialFloat(Renderer renderer, string name)
	{
		return renderer.sharedMaterial.GetFloat(name);
	}

	public static string GetMaterialTag(Renderer renderer, string name, bool searchFallbacks = false)
	{
		return renderer.sharedMaterial.GetTag(name, searchFallbacks);
	}

	public static void SetMaterialTag(Renderer renderer, string name, string value)
	{
		renderer.sharedMaterial.SetOverrideTag(name, value);
	}

	public static bool IsTransparentMaterial(Renderer renderer)
	{
		return MaterialManager.GetMaterialTag(renderer, "RenderType", false) == "Transparent";
	}

	public static void SetMaterialKeyWord(Renderer renderer, string name, bool value)
	{
		if (value)
		{
			renderer.sharedMaterial.EnableKeyword(name);
			return;
		}
		renderer.sharedMaterial.DisableKeyword(name);
	}

	public static bool GetMaterialKeyWord(Renderer renderer, string name)
	{
		return renderer.sharedMaterial.IsKeywordEnabled(name);
	}

	private static MaterialPropertyBlock prop = new MaterialPropertyBlock();
}
