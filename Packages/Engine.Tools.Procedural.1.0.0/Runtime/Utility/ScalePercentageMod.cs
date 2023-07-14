using Engine.Procedural.Poisson_Spawning;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Engine.Procedural {
	[CreateAssetMenu(fileName = "ScalePercent", menuName = "Utlity/Poisson Spawn Mods/Scale Percent")]
	public sealed class ScalePercentageMod : PoissonMod {
		[SerializeField] [MinMaxSlider(-50, 50, true)]
		Vector2Int scalePercentRange;

		public override void Process(Transform tr) {
			var scale = Random.Range(scalePercentRange.x, scalePercentRange.y);
			scale         /= Mathf.CeilToInt(scale / 100f);
			tr.localScale *= scale;
		}
	}
}