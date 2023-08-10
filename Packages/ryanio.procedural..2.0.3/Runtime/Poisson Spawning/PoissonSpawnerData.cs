using System;
using System.Collections.Generic;
using BCL;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralGeneration {
	[Serializable]
	public class PoissonSpawnerData {
		[VerticalGroup("Prefabs")] [HorizontalGroup("Prefabs/H1")] [SerializeField]
		public GameObjectWeightTable weightTable;

		[SerializeField] public List<PoissonMod> mods;

		[VerticalGroup("Parameters")] [HorizontalGroup("Parameters/H1")] [SerializeField] [Range(0.5f, 1000f)]
		public int defaultRadius = 1;

		[SerializeField] [MinMaxSlider(10, 500, true)]
		public Vector2Int regionSize = new(15, 15);

		[SerializeField] [Range(2, 100)] public int samplesBeforeRejection = 30;

		[VerticalGroup("Debug")] [HorizontalGroup("Debug/H1")] [SerializeField] [EnumToggleButtons]
		public Toggle enableGizmos;
	}
}