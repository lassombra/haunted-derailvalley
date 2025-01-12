using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HauntedValley
{
	[HarmonyPatch]
	public class LanternPatch
	{
		private static IEnumerable<MethodBase> TargetMethods()
		{
			var methods = (AccessTools.GetTypesFromAssembly(Assembly.GetAssembly(typeof(Resources)))
				.SelectMany(type => type.GetMethods())
				.Where(method => method.ReturnType == typeof(Object) &&
								 method.Name == nameof(Resources.Load) &&
								 method.GetParameters().Length == 1 &&
								 method.GetParameters()[0].ParameterType == typeof(string))
				.Cast<MethodBase>()).ToList();
			return methods;
		}

		private static void Postfix(MethodBase __originalMethod, string path, Object __result)
		{
			if (path.Contains("Lantern"))
			{
				var go = __result as GameObject;
				if (go != null)
				{
					var lantern = go.GetComponent<Lantern>();
					if (lantern != null)
					{
						var flicker = go.GetComponent<LanternFlicker>();
						if (flicker == null)
						{
							flicker = go.AddComponent<LanternFlicker>();
							flicker.Lantern = lantern;
						}
					}
				}
			}
		}
	}
}
