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
		[field: SerializeField, EnumToggleButtons, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public Ppu Ppu { get; set; } = Ppu.Sixteen;

		[field: SerializeField, Range(0.1f, 1.0f), TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public float ScaleModifier { get; set; } = 1;
		
		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public LayerMask SortingLayer { get; private set; } 

		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public int OrderInLayer { get; private set; } = 35;
		
		[field: SerializeField, Range(20.0f, 80.0f), TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public float CornerThreshold { get; set; } = 45f;

		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public bool IsSplineAdaptive { get; set; }

		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow), 
            EnumToggleButtons, LabelText("Open/Closed")]
		Toggle IsOpenEndedToggle = Toggle.Yes;
		public bool IsOpenEnded => IsOpenEndedToggle == Toggle.Yes;

		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow), 
            EnumToggleButtons, LabelText("Set Tangents")]
		Toggle SetTangentsToggle = Toggle.Yes;
		public bool EnableTangents => SetTangentsToggle == Toggle.Yes;

		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow), 
            EnumToggleButtons, LabelText("Use Fill Tessellation")]
		Toggle FillTessellationToggle = Toggle.Yes;
		
		public bool FillTessellation => FillTessellationToggle == Toggle.Yes;

		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow), EnumToggleButtons]
		public ShapeTangentMode SplineTangentMode { get; set; } = ShapeTangentMode.Continuous;
  
		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		[ShowIf("@TangentModeIsLinear"), EnumToggleButtons, LabelText("Simplify Segments")]
		Toggle SimplifySegmentsToggle = Toggle.Yes;
		public bool ShouldSimplifySegments => SimplifySegmentsToggle == Toggle.Yes;

		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		[ShowIf("@_displayTangentCoords")]
		public GameObject TextMeshPrefab { get; set; }
		
		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public SpriteShape Profile { get; private set; }
		
		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public Material EdgeMaterial { get; private set; }
		
		[field: SerializeField, TabGroup("SpriteShape Boundary", TabLayouting = TabLayouting.MultiRow)]
		public Material FillMaterial { get; private set; }

		IEnumerable _ppuList = new ValueDropdownList<Ppu>() {
			{ "8", Ppu.Eight },
			{ "16", Ppu.Sixteen },
			{ "32", Ppu.ThirtyTwo },
			{ "64", Ppu.SixtyFour },
			{ "100", Ppu.OneHundred },
			{ "128", Ppu.OneTwentyEight },
		};
		bool TangentModeIsLinear => SplineTangentMode == ShapeTangentMode.Linear;
	}
}