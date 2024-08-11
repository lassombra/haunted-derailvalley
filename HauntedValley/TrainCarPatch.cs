using DV;
using DV.Simulation.Cars;
using DV.Simulation.Controllers;
using DV.Simulation.Ports;
using DV.ThingTypes;
using HarmonyLib;
using HauntedValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HauntedValley
{
	[HarmonyPatch(typeof(CarSpawner), "Awake")]
	internal class TrainCarPatch
	{
		static void Prefix()
		{
			Globals.G.Types.Liveries.ForEach(Type =>
			{
				var prefab = Type.prefab;
				if (prefab.GetComponentInChildren<HeadlightsMainController>() != null && prefab.GetComponentInChildren<FlickerFuseController>() == null)
				{
					PatchFlicker(Type, prefab);
				}
			});
		}

		private static void PatchFlicker(TrainCarLivery Type, GameObject prefab)
		{
			var flickerController = prefab.AddComponent<FlickerFuseDefinition>();
			flickerController.ID = "headlightFlicker";
			var headLightController = prefab.GetComponentInChildren<HeadlightsMainController>();
			flickerController.powerFuseId = headLightController.powerFuseId;
			headLightController.powerFuseId = flickerController.ID + "." + flickerController.headlightFuse.id;
			var cabLights = prefab.GetComponentInChildren<CabLightsController>();
			var connections = prefab.GetComponentInChildren<SimController>()
				.connectionsDefinition;
			connections.executionOrder = connections.executionOrder.AddItem(flickerController).ToArray();
			if (cabLights != null)
			{
				cabLights.powerFuseId = flickerController.ID + "." + flickerController.cablightFuse.id;
			}
			var lampPorts = Type.interiorPrefab?.GetComponentsInChildren<LampPortReader>();
			if (lampPorts != null && lampPorts.Length > 0)
			{
				foreach (var entry in lampPorts)
				{
					if (entry != null)
					{
						entry.fuseId = flickerController.ID + "." + flickerController.dsahFuse.id;
					}
				}
			}
		}
	}
}
