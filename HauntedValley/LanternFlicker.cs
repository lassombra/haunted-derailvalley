using System.Collections;
using UnityEngine;

namespace HauntedValley
{
	public class LanternFlicker : MonoBehaviour
	{
		public Lantern Lantern;
		public void OnEnable()
		{
			StartCoroutine(Flicker());
		}

		private IEnumerator Flicker()
		{
			while (true) {
				// get target in 0.1f increments random range from 0.1f to lantern.wickSize
				while (Lantern.knob == null) yield return new WaitForSeconds(1.0f);
				var target = UnityEngine.Random.Range(0.1f, Lantern.wickSize);
				while (Lantern.wickSize > target) {
					Lantern.knob.SetValue(Lantern.wickSize - 0.1f);
					yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
				}
				target = UnityEngine.Random.Range(Lantern.wickSize, 1.0f);
				while (Lantern.wickSize < target)
				{
					Lantern.knob.SetValue(Lantern.wickSize + 0.1f);
					yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
				}
			}
		}
	}
}
