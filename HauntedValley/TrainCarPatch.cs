using DV;
using DV.Simulation.Cars;
using DV.Simulation.Ports;
using DV.ThingTypes;
using HarmonyLib;
using System.Linq;
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
			var existingFlicker = prefab.GetComponentInChildren<FlickerFuseController>();
			if (existingFlicker != null)
			{
				return;
			}
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
		}
	}
}
