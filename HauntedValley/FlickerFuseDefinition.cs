using DV.Simulation.Controllers;
using LocoSim.Definitions;
using LocoSim.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HauntedValley
{
	public class FlickerFuseDefinition : SimComponentDefinition
	{
		public string powerFuseId;

		public FuseDefinition headlightFuse = new FuseDefinition("HEADLIGHT", false);
		public FuseDefinition cablightFuse = new FuseDefinition("CABLIGHT", false);
		public FuseDefinition dsahFuse = new FuseDefinition("DASH", false);

		public override SimComponent InstantiateImplementation()
		{
			return new FlickerFuseController(this);
		}
	}
}
