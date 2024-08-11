using LocoSim.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HauntedValley
{
	internal class FlickerFuseController : SimComponent
	{
		bool[] connected = { false, false, false };
		FuseReference powerFuse;
		Fuse headlightFuse;
		Fuse cablightFuse;
		Fuse dashFuse;
		float[] timeTillToggle = { 0.0f, 0.0f, 0.0f };
		public FlickerFuseController(FlickerFuseDefinition def) : base(def.ID)
		{
			headlightFuse = base.AddFuse(def.headlightFuse);
			cablightFuse = base.AddFuse(def.cablightFuse);
			dashFuse = base.AddFuse(def.dsahFuse);
			powerFuse = base.AddFuseReference(def.powerFuseId);
		}

		public override void Tick(float delta)
		{
			timeTillToggle = timeTillToggle.Select(val => val - delta).ToArray();
			for (byte i = 0; i  < timeTillToggle.Length; i++)
			{
				CheckTimeAndToggleConnection(timeTillToggle[i], i);
			}
			UpdateFuse();
		}

		private void CheckTimeAndToggleConnection(float time, byte index)
		{
			if (time <= 0.0f)
			{
				connected[index] = !connected[index];
				if (connected[index])
				{
					timeTillToggle[index] = UnityEngine.Random.Range(0.01f, 4.5f);
				} else
				{
					timeTillToggle[index] = UnityEngine.Random.Range(0.02f, 1.5f);
				}
			}
		}

		private void UpdateFuse()
		{
			bool mainState = powerFuse.State;
			if (headlightFuse.State != (connected[0] && mainState))
			{
				headlightFuse.ChangeState(connected[0] && mainState);
			}
			if (cablightFuse.State != (connected[1] && mainState))
			{
				cablightFuse.ChangeState(connected[1] && mainState);
			}
			if (dashFuse.State != (connected[2] && mainState))
			{
				dashFuse.ChangeState(connected[2] && mainState);
			}
		}
	}
}
