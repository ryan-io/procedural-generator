using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityBCL;
using UnityBCL.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Engine.Procedural.Runtime {
	[Serializable]
	public class ProceduralConfig {
#region NAME

		[field: SerializeField, BoxGroup("Map Name"), HideLabel]
		public string Name { get; set; } = "map_" + Guid.NewGuid();

#endregion

#region REQUIRED_MONOBEHAVIORS

		[field: SerializeField, Required, FoldoutGroup("Required Components", false)]
		public GameObject Pathfinder { get; private set; }

#endregion

#region SERIALIZATION

		[field: SerializeField, FoldoutGroup("Serialization", false)]
		public bool ShouldSerializePathfinding { get; private set; } = true;

		[field: SerializeField, FoldoutGroup("Serialization", false)]
		public bool ShouldSerializeMapPrefab { get; private set; } = true;

		[field: SerializeField, FoldoutGroup("Serialization", false)]
		public bool ShouldSerializeSpriteShape { get; private set; } = true;
		
		[field: SerializeField, FoldoutGroup("Serialization", false)]
		public bool ShouldSerializeColliderCoords { get; private set; } = true;

		[field: SerializeField, InlineEditor(InlineEditorObjectFieldModes.Foldout), ShowIf("@ShouldSerializeMapPrefab"),
		        FoldoutGroup("Serialization", false)]
		public SerializerSetup MapSerializer { get; private set; }

		[field: SerializeField, InlineEditor(InlineEditorObjectFieldModes.Foldout),
		        ShowIf("@ShouldSerializeSpriteShape"), FoldoutGroup("Serialization", false)]
		public SerializerSetup SpriteShapeSerializer { get; private set; }
		
		[field: SerializeField, InlineEditor(InlineEditorObjectFieldModes.Foldout),
		        ShowIf("@ShouldSerializeColliderCoords"), FoldoutGroup("Serialization", false)]
		public SerializerSetup ColliderCoordsSerializer { get; private set; }

#endregion

#region SEEDING

		[field: SerializeField, FoldoutGroup("Seeding", false)]
		public bool UseRandomSeed { get; private set; } = true;

		[field: SerializeField, ShowIf("UseRandomSeed"), FoldoutGroup("Seeding", false)]
		public string Seed { get; internal set; }

		[field: SerializeField, ReadOnly, FoldoutGroup("Seeding", false)]
		public string LastSeed { get; set; }

		[field: SerializeField, ReadOnly, FoldoutGroup("Seeding", false)]
		public int LastIteration { get; set; }

#endregion

#region GENERATION_STATE

		[field: SerializeField, EnumToggleButtons, FoldoutGroup("State", false)]
		public bool IsBuild { get; private set; }

		[field: SerializeField, EnumToggleButtons, FoldoutGroup("State", false)]
		public bool ShouldGenerate { get; set; } = true;

		[field: SerializeField, ShowIf("@!ShouldGenerate"), EnumToggleButtons, FoldoutGroup("State", false)]
		public bool ShouldDeserialize { get; set; } = true;

		[field: SerializeField, ShowIf("@ShouldDeserialize && !ShouldGenerate"), FoldoutGroup("State", false),
		        ValueDropdown(@"GetAllSeedsWrapper")]
		public string NameSeedIteration { get; set; }

#endregion

#region MAP

		void CheckIfEven() {
			if (Columns % 2 != 0) {
				Columns++;
			}

			if (Rows % 2 != 0) {
				Rows++;
			}
		}

		[Tooltip(Message.MAP_WILL_BE_RESIZED)]
		[field: SerializeField, Range(50, Constants.MAP_DIMENSION_LIMIT), OnValueChanged("CheckIfEven"),
		        FoldoutGroup("Generation Settings", false)]
		public int Columns { get; set; } = 100;

		[Tooltip(Message.MAP_WILL_BE_RESIZED)]
		[field: SerializeField, Range(50, Constants.MAP_DIMENSION_LIMIT), OnValueChanged("CheckIfEven"),
		        FoldoutGroup("Generation Settings", false)]
		public int Rows { get; set; } = 100;

		[field: SerializeField, Range(1, 10), FoldoutGroup("Generation Settings", false)]
		public int BorderSize { get; private set; } = 1;

		[field: SerializeField, Range(1, 125), FoldoutGroup("Generation Settings", false)]
		public int SmoothingIterations { get; private set; } = 5;

		[field: SerializeField, Range(10, 1000), FoldoutGroup("Generation Settings", false)]
		public int WallRemovalThreshold { get; private set; } = 50;

		[field: SerializeField, Range(10, 1000), FoldoutGroup("Generation Settings", false)]
		public int RoomRemovalThreshold { get; private set; } = 50;

		[field: SerializeField, Range(1, 4), FoldoutGroup("Generation Settings", false)]
		public int LowerNeighborLimit { get; private set; } = 4;

		[field: SerializeField, Range(4, 8), FoldoutGroup("Generation Settings", false)]
		public int UpperNeighborLimit { get; private set; } = 4;

		[field: SerializeField, PropertyTooltip(Message.PERCENTAGE_WALLS), Range(40, 55),
		        FoldoutGroup("Generation Settings", false)]
		public int WallFillPercentage { get; private set; } = 47;

		[field: SerializeField, MinMaxSlider(1, 12), FoldoutGroup("Generation Settings", false)]
		public Vector2Int CorridorWidth { get; private set; } = new(1, 6);

		[field: SerializeField, FoldoutGroup("Layer Definitions", false)]
		public LayerMask GroundLayerMask { get; private set; }

		[field: SerializeField, FoldoutGroup("Layer Definitions", false)]
		public LayerMask ObstacleLayerMask { get; private set; }

		[field: SerializeField, FoldoutGroup("Layer Definitions", false)]
		public LayerMask BoundaryLayerMask { get; private set; }

		[field: SerializeField, FoldoutGroup("Layer Definitions", false)]
		public LayerMask NavGraphHeightTestLayerMask { get; private set; } = 0;

#endregion

#region TILEMAP

		[field: SerializeField, FoldoutGroup("Tilemaps", false)]

		public bool ShouldCreateTileLabels { get; private set; }

		[field: SerializeField, FoldoutGroup("Tilemaps", false)]
		public bool ShouldGenerateAngles { get; private set; }

		[field: SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.CollapsedFoldout),
		        FoldoutGroup("Tilemaps", false)]
		public TileDictionary TileDictionary { get; private set; } = TileDictionary.GetDefault();

#endregion

#region PATHFINDING

		[field: SerializeField, EnumToggleButtons, FoldoutGroup("Pathfinding", false)]
		public ColliderSolverType GeneratedColliderType { get; private set; } = ColliderSolverType.Edge;

		[field: SerializeField, EnumToggleButtons, FoldoutGroup("Pathfinding", false)]
		public ColliderType NavGraphCollisionType { get; private set; } = ColliderType.Capsule;
		
		[field: SerializeField, FoldoutGroup("Pathfinding", false)]
		[field: Range(0.05f, 5.0f)]
		public float NavGraphCollisionDetectionDiameter { get; private set; } = 0.1f;

		[field: SerializeField, FoldoutGroup("Pathfinding", false)]
		[field: Range(0.5f, 100.0f)]
		[field: ShowIf("@NavGraphCollisionType == ColliderType.Capsule")]
		public float NavGraphCollisionDetectionHeight { get; private set; } = 1.0f;

		[field: SerializeField, Range(.1f, 8), ShowIf("@ErodePathfindingGrid"), FoldoutGroup("Pathfinding", false)]
		public float NavGraphNodeSize { get; private set; } = 0.5f;

#endregion

#region PATHFINDING_EROSION

		[field: SerializeField, FoldoutGroup("Erosion", false)]
		public bool ErodePathfindingGrid { get; private set; } = true;

		[field: SerializeField, Range(.1f, 8), ShowIf("@ErodePathfindingGrid"), FoldoutGroup("Erosion", false)]
		public float ErosionCollisionDiameter { get; private set; } = 1.75f;

		//TODO: is this needed for anything after refactor?
		// [field: SerializeField, ShowIf("@ErodePathfindingGrid")]
		// public LayerMask ErosionObstacleLayerMasks { get; private set; }

		[field: SerializeField, Range(0, 10), ShowIf("@ErodePathfindingGrid"), InfoBox(EROSION_TAG_INFO),
		        FoldoutGroup("Erosion", false)]
		public int NodesToErodeAtBoundaries { get; private set; } = 3;

		[field: SerializeField, Range(0, 18), ShowIf("@ErodePathfindingGrid"), FoldoutGroup("Erosion", false)]
		public int StartingNodeIndexToErode { get; private set; } = 1;

		[Title("Erosion Debugging", horizontalLine: false)]
		[field: SerializeField, ShowIf("@ErodePathfindingGrid"), FoldoutGroup("Erosion", false)]
		public bool DrawNodePositionGizmos { get; private set; }

		[field: SerializeField, ShowIf("@ErodePathfindingGrid"), FoldoutGroup("Erosion", false)]
		public bool DrawTilePositionGizmos { get; private set; }

		[field: SerializeField, ShowIf("@ErodePathfindingGrid"), FoldoutGroup("Erosion", false)]
		public bool DrawTilePositionShiftedGizmos { get; private set; }

#endregion

#region MAP_COLLIDERS

		[field: SerializeField, EnumToggleButtons, FoldoutGroup("Collisions", false)]
		public ColliderSolverType SolverType { get; private set; } = ColliderSolverType.Edge;

#region BOX_COLLIDER_SETTINGS

		[field: SerializeField, ShowIf("@IsBox"), Required, FoldoutGroup("Collisions", false)]
		public AssetReference BoxColliderPrefab { get; private set; }

		[field: SerializeField, ShowIf("@IsBox"), Range(0.1f, 1.5f), FoldoutGroup("Collisions", false)]
		public float BoxColliderSkinWidth { get; private set; } = 0.1f;

#endregion

#region EDGE_COLLIDER_SETTINGS

		[field: SerializeField, ShowIf("@IsEdge"), Range(0.1f, 1f), FoldoutGroup("Collisions", false)]
		public float EdgeColliderRadius { get; private set; } = 0.5f;

		[field: SerializeField, ShowIf("@IsEdge"), FoldoutGroup("Collisions", false)]
		public Vector2 EdgeColliderOffset { get; private set; } = new(0.5f, -0.5f);

#endregion

#region PRIMITIVE_COLLIDER_SETTINGS

		[field: SerializeField, ShowIf("@IsPrimitive"), Range(0.1f, 1f), FoldoutGroup("Collisions", false)]
		public float PrimitiveColliderRadius { get; private set; } = 0.1f;

		[field: SerializeField, Range(0.1f, 1f), ShowIf("@IsPrimitive"), FoldoutGroup("Collisions", false)]
		public float PrimitiveColliderSkinWidth { get; private set; } = 0.5f;
		
		[field: SerializeField, FoldoutGroup("Collisions", false), ShowIf("@IsPrimitive")]
		[field: Range(0.1f, 5.0f)]
		public float NodeCullDistance { get; private set; } = .5f;

#endregion

		[field: SerializeField, FoldoutGroup("Collisions", false)]
		public List<GraphColliderCutter> ColliderCutters { get; private set; } = new();

		bool IsBox       => SolverType == ColliderSolverType.Box;
		bool IsEdge      => SolverType == ColliderSolverType.Edge;
		bool IsPrimitive => SolverType == ColliderSolverType.PrimitiveCombo;

#endregion

#region EVENTS

		[field: SerializeField, DictionaryDrawerSettings(IsReadOnly = true), FoldoutGroup("Events", false)]
		public EventDictionary SerializedEvents { get; private set; } = new() {
			{ ProcessStep.Cleaning, default },
			{ ProcessStep.Initializing, default },
			{ ProcessStep.Running, default },
			{ ProcessStep.Completing, default },
			{ ProcessStep.Disposing, default },
			{ ProcessStep.Error, default },
		};

#endregion

#region DEBUGGING

		[field: SerializeField, EnumToggleButtons, FoldoutGroup("Debugging", false)]
		public bool DrawDebugGizmos { get; private set; }

#endregion

#region SCOPE_CONSTANTS

		const string EROSION_TAG_INFO =
			"Be aware: make sure to adjust the traversable tags in your AI's 'Seeker' component. " +
			"Typically, the tag you want to make 'untraversable' is = Erosion Iterations - 1";

#endregion

#region HELPERS

		internal void PopulateTileDictionary() {
			TileDictionary = TileDictionary.GetDefault();
		}

		internal void FindGraphColliderCuttersInScene() =>
			ColliderCutters = new ObjectFinder().FindGraphColliderCuttersInScene(ColliderCutters);

		internal void FindPathfinderInScene() =>
			Pathfinder = new ObjectFinder().FindPathfinderInScene(Pathfinder);

		bool ShouldShowDeserialize => !ShouldGenerate;

		IEnumerable GetAllSeedsWrapper() {
			var seeds = GeneratorSerializer.GetAllSeeds();
			var allSeedsWrapper = seeds.ToList();
			
			if (allSeedsWrapper.IsEmptyOrNull()) {
				NameSeedIteration = string.Empty;
				return Enumerable.Empty<string>();
			}

			return allSeedsWrapper;
		}

#endregion
	}
}