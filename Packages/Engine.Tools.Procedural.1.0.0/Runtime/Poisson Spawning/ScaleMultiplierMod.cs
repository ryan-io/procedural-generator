using Engine.Procedural.Poisson_Spawning;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Engine.Procedural {
	[CreateAssetMenu(fileName = "ScaleMultiplier", menuName = "Utlity/Poisson Spawn Mods/Scale Multiplier")]
	public sealed class ScaleMultiplierMod : PoissonMod {
		[SerializeField] [MinMaxSlider(0.1f, 50f, true)]
		Vector2 scaleRange;

		public override void Process(Transform tr) {
			var scale = Random.Range(scaleRange.x, scaleRange.y);
			tr.localScale *= scale;
		}
	}
}