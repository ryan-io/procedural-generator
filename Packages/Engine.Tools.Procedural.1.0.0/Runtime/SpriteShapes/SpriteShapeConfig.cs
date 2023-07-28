// Engine.Procedural

using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;

namespace Engine.Procedural {
	[Serializable]
	public class SpriteShapeConfig {
		[field: SerializeField, Required, FoldoutGroup("Sprite Shape Boundary", false)]
		[ReadOnly, ShowInInspector]
		public BorderShapeData SerializedData { get; set; }

		[field: SerializeField, Required, FoldoutGroup("Sprite Shape Boundary", false)]
		public GameObject ControllerPrefab { get; set; }

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public GameObject FillPrefab { get; set; }

		[field: SerializeField, EnumToggleButtons, FoldoutGroup("Sprite Shape Boundary", false)]
		public Ppu Ppu { get; set; } = Ppu.Sixteen;

		[field: SerializeField, Range(0.1f, 1.0f), FoldoutGroup("Sprite Shape Boundary", false)]
		public float ScaleModifier { get; set; } = 1;

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public bool IsSplineAdaptive { get; set; }

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false), ValueDropdown("@_ppuList")]
		public ShapeTangentMode SplineTangentMode { get; set; } = ShapeTangentMode.Continuous;
  
		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		[ShowIf("@TangentModeIsLinear")]
		public bool ShouldSimplifySegments { get; set; } = true;

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		[ShowIf("@_displayTangentCoords")]
		public GameObject TextMeshPrefab { get; set; }

		IEnumerable _ppuList = new ValueDropdownList<Ppu>() {
			{ "8", Ppu.Eight },
			{ "16", Ppu.Sixteen },
			{ "32", Ppu.ThirtyTwo },
			{ "64", Ppu.SixtyFour },
			{ "100", Ppu.OneHundred },
			{ "128", Ppu.OneTwentyEight },
		};
	}
}