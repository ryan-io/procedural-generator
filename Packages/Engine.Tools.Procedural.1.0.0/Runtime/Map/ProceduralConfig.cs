using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Sirenix.OdinInspector;
using Standalone.Serialization;
using UnityBCL;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Engine.Procedural.Runtime {
	[Serializable]
	public class ProceduralConfig {
#region NAME

		[field: SerializeField, Title("Name")] public string Name { get; set; } = "map_" + Guid.NewGuid();

#endregion

#region DESERIALIZATION

		[field: SerializeField, Title("Deserialization"), ValueDropdown(@"GetAllSeedsWrapper")] 
		public string NameSeedIteration { get; private set; }
		
		IEnumerable GetAllSeedsWrapper() 
			=> SeedInfoSerializer == null ? Enumerable.Empty<string>() : ProceduralSerializer.GetAllSeeds(SeedInfoSerializer);

#endregion

#region REQUIRED_MONOBEHAVIORS

		[field: SerializeField, Title("Components", titleAlignment: TitleAlignments.Left), Required]
		public GameObject Pathfinder { get; private set; }

		[field: SerializeField, Required] public MeshFilter MeshFilter { get; private set; }
		
		[field: SerializeField, Required, Title("Tilemaps", horizontalLine: false)] public Grid Grid { get; private set; }
		
		[field: SerializeField, Required]
		public GameObject TilemapContainer { get; private set; }

		[field: SerializeField, Title("Serializers", horizontalLine: false)]
		public bool ShouldSerializeSeed { get; private set; } = true;

		[field: SerializeField] public bool ShouldSerializePathfinding { get; private set; } = true;

		[field: SerializeField] public bool ShouldSerializeMapPrefab { get; private set; } = true;
		
		[field: SerializeField, InlineEditor(InlineEditorObjectFieldModes.Foldout), ShowIf("@ShouldSerializeSeed")]
		public SerializerSetup SeedInfoSerializer { get; private set; }

		[field: SerializeField, InlineEditor(InlineEditorObjectFieldModes.Foldout), ShowIf("@ShouldSerializePathfinding")]
		public SerializerSetup PathfindingSerializer { get; private set; }
		
		[field: SerializeField, InlineEditor(InlineEditorObjectFieldModes.Foldout), ShowIf("@ShouldSerializeMapPrefab")]
		public SerializerSetup MapSerializer { get; private set; }
		
		[field: SerializeField, Title("Optional", horizontalLine: false)] 
		public List<GraphColliderCutter> ColliderCutters { get; private set; } = new();


#region FINDING_OBJECTS

		[Button, ShowIf("@ShouldShowFindCutters")]
		void FindGraphColliderCuttersInScene() =>
			ColliderCutters = new ObjectFinder().FindGraphColliderCuttersInScene(ColliderCutters);

		[Button, ShowIf("@ShouldShowFindPathfinder")]
		void FindPathfinderInScene() =>
			Pathfinder = new ObjectFinder().FindPathfinderInScene(Pathfinder);

		bool ShouldShowFindCutters    => ColliderCutters.IsEmptyOrNull();
		bool ShouldShowFindPathfinder => !Pathfinder;

#endregion

#endregion

#region SEEDING

		[field: SerializeField, Title("Seeding")]
		public bool UseRandomSeed { get; private set; } = true;

		[field: SerializeField, ShowIf("UseRandomSeed")]
		public string Seed { get; internal set; }

		[field: SerializeField, ReadOnly] public string LastSeed { get; set; }

		[field: SerializeField, ReadOnly] public int LastIteration { get; set; }

#endregion

#region GENERATION_STATE

		[field: SerializeField, EnumToggleButtons, Title("Generation State")]
		public bool IsBuild { get; private set; }

		[field: SerializeField]
		[field: EnumToggleButtons]
		public bool ShouldGenerate { get; set; } = true;

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
		[field: SerializeField, Title("Map Settings"), Range(50, Constants.MAP_DIMENSION_LIMIT), OnValueChanged("CheckIfEven")]
		public int Columns { get; set; } = 100;

		[Tooltip(Message.MAP_WILL_BE_RESIZED)]
		[field: SerializeField, Range(50, Constants.MAP_DIMENSION_LIMIT), OnValueChanged("CheckIfEven")]
		public int Rows { get; set; } = 100;

		[field: SerializeField, Range(1, 10)] public int BorderSize { get; private set; } = 1;

		[field: SerializeField, Title("Procedural"), Range(1, 125)]
		public int SmoothingIterations { get; private set; } = 5;

		[field: SerializeField, Range(10, 1000)]
		public int WallRemovalThreshold { get; private set; } = 50;

		[field: SerializeField, Range(10, 1000)]
		public int RoomRemovalThreshold { get; private set; } = 50;

		[field: SerializeField, Range(1, 4)] public int LowerNeighborLimit { get; private set; } = 4;
		[field: SerializeField, Range(4, 8)] public int UpperNeighborLimit { get; private set; } = 4;

		[field: SerializeField, PropertyTooltip(Message.PERCENTAGE_WALLS), Range(40, 55)]
		public int WallFillPercentage { get; private set; } = 47;

		[field: SerializeField, MinMaxSlider(1, 12)]
		public Vector2Int CorridorWidth { get; private set; } = new(1, 6);

		[field: SerializeField] public LayerMask GroundLayerMask { get; private set; }

		[field: SerializeField] public LayerMask ObstacleLayerMask { get; private set; }

		[field: SerializeField] public LayerMask BoundaryLayerMask { get; private set; }

#endregion

#region TILEMAP

		[field: SerializeField, Title("Tilemap Settings")]

		public bool ShouldCreateTileLabels { get; private set; }

		[field: SerializeField] public bool ShouldGenerateAngles { get; private set; }
		
		[field: SerializeField] public TileMapDictionary TileMapDictionary { get; private set; }

		[field: SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
		public TileDictionary TileDictionary { get; private set; } = TileDictionary.GetDefault();

		[Button]
		void PopulateTileDictionary() {
			TileDictionary = TileDictionary.GetDefault();
		}

#endregion

#region PATHFINDING

		[field: SerializeField, EnumToggleButtons, Title("Pathfinding Settings")]
		public ColliderSolverType GeneratedColliderType { get; private set; } = ColliderSolverType.Edge;

		[field: SerializeField, EnumToggleButtons]
		public ColliderType NavGraphCollisionType { get; private set; } = ColliderType.Capsule;

		[field: SerializeField]
		[field: Range(0.05f, 5.0f)]
		public float NavGraphCollisionDetectionDiameter { get; private set; } = 0.1f;

		[field: SerializeField]
		[field: Range(0.5f, 100.0f)]
		[field: ShowIf("@NavGraphCollisionType == ColliderType.Capsule")]
		public float NavGraphCollisionDetectionHeight { get; private set; } = 1.0f;

		[field: SerializeField, Range(.1f, 8), ShowIf("@ErodePathfindingGrid")]
		public float NavGraphNodeSize { get; private set; } = 0.5f;

		[field: SerializeField] public LayerMask NavGraphHeightTestLayerMask { get; private set; } = 0;

#endregion

#region PATHFINDING_EROSION	

		[field: SerializeField, Title("Erosion Settings")]
		public bool ErodePathfindingGrid { get; private set; } = true;

		[field: SerializeField, Range(.1f, 8), ShowIf("@ErodePathfindingGrid")]
		public float ErosionCollisionDiameter { get; private set; } = 1.75f;

		//TODO: is this needed for anything after refactor?
		// [field: SerializeField, ShowIf("@ErodePathfindingGrid")]
		// public LayerMask ErosionObstacleLayerMasks { get; private set; }

		[field: SerializeField, Range(0, 10), ShowIf("@ErodePathfindingGrid"), InfoBox(EROSION_TAG_INFO)]
		public int NodesToErodeAtBoundaries { get; private set; } = 3;

		[field: SerializeField, Range(0, 18), ShowIf("@ErodePathfindingGrid")]
		public int StartingNodeIndexToErode { get; private set; } = 1;

		[Title("Erosion Debugging", horizontalLine: false)]
		[field: SerializeField, ShowIf("@ErodePathfindingGrid")]
		public bool DrawNodePositionGizmos { get; private set; }

		[field: SerializeField, ShowIf("@ErodePathfindingGrid")]
		public bool DrawTilePositionGizmos { get; private set; }

		[field: SerializeField, ShowIf("@ErodePathfindingGrid")]
		public bool DrawTilePositionShiftedGizmos { get; private set; }

#endregion

#region MAP_COLLIDERS

		[Title("Map Collider Settings")]
		[field: SerializeField, EnumToggleButtons]
		public ColliderSolverType SolverType { get; private set; } = ColliderSolverType.Edge;

#region BOX_COLLIDER_SETTINGS

		[field: SerializeField, ShowIf("@IsBox"), Required]
		public AssetReference BoxColliderPrefab { get; private set; }

		[field: SerializeField, ShowIf("@IsBox"), Range(0.1f, 1.5f)]
		public float BoxColliderSkinWidth { get; private set; } = 0.1f;

#endregion

#region EDGE_COLLIDER_SETTINGS

		[field: SerializeField, ShowIf("@IsEdge"), Range(0.1f, 1f)]
		public float EdgeColliderRadius { get; private set; } = 0.5f;

		[field: SerializeField, ShowIf("@IsEdge")]
		public Vector2 EdgeColliderOffset { get; private set; } = new(0.5f, -0.5f);

#endregion

#region PRIMITIVE_COLLIDER_SETTINGS

		[field: SerializeField, ShowIf("@IsPrimitive"), Range(0.1f, 1f)]
		public float PrimitiveColliderRadius { get; private set; } = 0.1f;

		[field: SerializeField, Range(0.1f, 1f), ShowIf("@IsPrimitive")]
		public float PrimitiveColliderSkinWidth { get; private set; } = 0.5f;

#endregion

		bool IsBox       => SolverType == ColliderSolverType.Box;
		bool IsEdge      => SolverType == ColliderSolverType.Edge;
		bool IsPrimitive => SolverType == ColliderSolverType.PrimitiveCombo;

#endregion

#region EVENTS

		[field: SerializeField, BoxGroup("0", false), DictionaryDrawerSettings(IsReadOnly = true), Title("Events")]
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

		[field: SerializeField, EnumToggleButtons, Title("Debugging")]
		public bool DrawDebugGizmos { get; private set; }

#endregion

#region SCOPE_CONSTANTS

		const string EROSION_TAG_INFO =
			"Be aware: make sure to adjust the traversable tags in your AI's 'Seeker' component. " +
			"Typically, the tag you want to make 'untraversable' is = Erosion Iterations - 1";

#endregion
	}
}