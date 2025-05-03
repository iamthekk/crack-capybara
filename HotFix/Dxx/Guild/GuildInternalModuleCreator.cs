using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	internal class GuildInternalModuleCreator
	{
		internal bool CreateGuildModules(GuildSDKManager sdk, GuildInitConfig config, List<IGuildModule> modulelist)
		{
			List<GuildInternalModuleCreator.CreateModuleCache> list = new List<GuildInternalModuleCreator.CreateModuleCache>();
			Type typeFromHandle = typeof(GuildInternalModuleAttribute);
			foreach (Type type in base.GetType().Assembly.GetTypes())
			{
				if (!(type == null) && type.IsDefined(typeFromHandle, false))
				{
					object[] customAttributes = type.GetCustomAttributes(false);
					for (int j = 0; j < customAttributes.Length; j++)
					{
						GuildInternalModuleAttribute guildInternalModuleAttribute = customAttributes[j] as GuildInternalModuleAttribute;
						if (guildInternalModuleAttribute != null)
						{
							list.Add(new GuildInternalModuleCreator.CreateModuleCache
							{
								Index = guildInternalModuleAttribute.Index,
								Attr = guildInternalModuleAttribute,
								Type = type
							});
						}
					}
				}
			}
			list.Sort(new Comparison<GuildInternalModuleCreator.CreateModuleCache>(GuildInternalModuleCreator.CreateModuleCache.Sort));
			for (int k = 0; k < list.Count; k++)
			{
				IGuildModule guildModule = Activator.CreateInstance(list[k].Type) as IGuildModule;
				if (guildModule == null)
				{
					HLog.LogError("create module fail of type " + list[k].Type.FullName);
				}
				else if (sdk.InternalRegModule(guildModule.ModuleName, guildModule))
				{
					modulelist.Add(guildModule);
				}
			}
			return true;
		}

		private class CreateModuleCache
		{
			public static int Sort(GuildInternalModuleCreator.CreateModuleCache x, GuildInternalModuleCreator.CreateModuleCache y)
			{
				return x.Index.CompareTo(y.Index);
			}

			public int Index;

			public GuildInternalModuleAttribute Attr;

			public Type Type;
		}
	}
}
