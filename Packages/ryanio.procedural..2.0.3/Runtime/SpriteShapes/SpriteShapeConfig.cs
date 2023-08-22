// Engine.Procedural

using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityEngine;
using UnityEngine.U2D;

namespace ProceduralGeneration {
	[Serializable]
	public class SpriteShapeConfig {
		[field: SerializeField, EnumToggleButtons, FoldoutGroup("Sprite Shape Boundary", false)]
		public Ppu Ppu { get; set; } = Ppu.Sixteen;

		[field: SerializeField, Range(0.1f, 1.0f), FoldoutGroup("Sprite Shape Boundary", false)]
		public float ScaleModifier { get; set; } = 1;
		
		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public LayerMask SortingLayer { get; private set; } 

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public int OrderInLayer { get; private set; } = 35;
		
		[field: SerializeField, Range(20.0f, 80.0f), FoldoutGroup("Sprite Shape Boundary", false)]
		public float CornerThreshold { get; set; } = 45f;

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public bool IsSplineAdaptive { get; set; }

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public bool IsOpenEnded { get; private set; } = true;

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public bool EnableTangents { get; private set; } = true;

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public bool FillTessellation { get; private set; } = true;

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public ShapeTangentMode SplineTangentMode { get; set; } = ShapeTangentMode.Continuous;
  
		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		[ShowIf("@TangentModeIsLinear")]
		public bool ShouldSimplifySegments { get; set; } = true;

		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		[ShowIf("@_displayTangentCoords")]
		public GameObject TextMeshPrefab { get; set; }
		
		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public SpriteShape Profile { get; private set; }
		
		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public Material EdgeMaterial { get; private set; }
		
		[field: SerializeField, FoldoutGroup("Sprite Shape Boundary", false)]
		public Material FillMaterial { get; private set; }

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