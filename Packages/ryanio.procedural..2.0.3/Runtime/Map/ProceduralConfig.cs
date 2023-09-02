using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Pathfinding;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityBCL;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProceduralGeneration {
	[Serializable]
	public class ProceduralConfig {
		const float LABEL_WIDTH = 225f;

#region SETUP

		[field: SerializeField, TabGroup("Setup", TabLayouting = TabLayouting.MultiRow), LabelText("Map Name/Id"),
		        LabelWidth(LABEL_WIDTH), Title("State")]
		public string Name { get; set; } = "map_" + Guid.NewGuid();

		[field: SerializeField, LabelText("Is Build"), EnumToggleButtons,
		        TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		Toggle IsBuildToggle { get; set; } = Toggle.No;

		public bool IsBuild => IsBuildToggle == Toggle.Yes;

		[field: SerializeField, LabelText("Run"), EnumToggleButtons,
		        TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		RunState RunState { get; set; } = RunState.CreateMap;

		public bool ShouldGenerate    => RunState == RunState.CreateMap;
		public bool ShouldDeserialize => RunState == RunState.DeserializeMap;

		[field: SerializeField, EnumToggleButtons, LabelText("Use Random Seed"), Title("Seed"), TabGroup("Setup",
			        TabLayouting =
				        TabLayouting.MultiRow)]
		Toggle UseRandomSeedToggle { get; set; } = Toggle.Yes;

		public bool UseRandomSeed => UseRandomSeedToggle == Toggle.Yes;

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("UseRandomSeed"),
		        TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public string Seed { get; internal set; }

		[field: SerializeField, Sirenix.OdinInspector.ReadOnly, TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public string LastSeed { get; set; }

		[field: SerializeField, Sirenix.OdinInspector.ReadOnly, TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public int LastIteration { get; set; }

		[field: SerializeField, Title("Monobehaviors"), HorizontalLine, Sirenix.OdinInspector.Required, TabGroup(
			        "Setup", TabLayouting =
				        TabLayouting.MultiRow), LabelText("Pathfinder"), LabelWidth(LABEL_WIDTH), SceneObjectsOnly]
		public GameObject Pathfinder { get; set; }
		
		[TabGroup("Monobehaviors", TabLayouting = TabLayouting.MultiRow), Sirenix.OdinInspector.ShowIf("@IsPathfinderNull"),
		 Sirenix.OdinInspector.Button(ButtonSizes.Large, Stretch = false, ButtonAlignment = 1),
		 GUIColor(154 / 255f, 208f / 255, 254f / 255, 1f)]
		void FindPathfinder() => FindPathfinderInScene();

#endregion

#region SERIALIZATION

		[field: SerializeField, Title("Iteration Tracker"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow),
		        ValueDropdown(@"GetAllSeedsWrapper")]
		public string NameSeedIteration { get; set; }

		[field: SerializeField, Title("Serialization"), EnumToggleButtons,
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow), LabelText("Mesh")]
		Toggle SerializeMeshToggle { get; set; } = Toggle.Yes;

		public bool ShouldSerializeMesh => SerializeMeshToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons,
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow), LabelText("Pathfinding")]
		Toggle SerializePathfindingToggle { get; set; } = Toggle.Yes;

		public bool ShouldSerializePathfinding => SerializePathfindingToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons, LabelText("Map Prefab"), TabGroup("Serialize & Deserialize",
			        TabLayouting = TabLayouting.MultiRow)]
		Toggle SerializeMapPrefabToggle { get; set; } = Toggle.Yes;

		public bool ShouldSerializeMapPrefab => SerializeMapPrefabToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons, LabelText("Sprite Shape"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		Toggle SerializeSpriteShapeToggle { get; set; } = Toggle.Yes;
		public bool ShouldSerializeSpriteShape => SerializeSpriteShapeToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons, LabelText("Colliders"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		Toggle SerializeColliderCoordsToggle { get; set; } = Toggle.Yes;
		public bool ShouldSerializeColliderCoords => SerializeColliderCoordsToggle == Toggle.Yes;

#endregion

#region DESERIALIZATION

		[field: SerializeField, Title("Deserialization"), EnumToggleButtons, LabelText("Mesh"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		Toggle DeserializeMeshToggle { get; set; } = Toggle.Yes;
		public bool ShouldDeserializeMesh => DeserializeMeshToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons, LabelText("Pathfinding"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		Toggle DeserializePathfindingToggle { get; set; } = Toggle.Yes;
		public bool ShouldDeserializePathfinding => DeserializePathfindingToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons, LabelText("Map Prefab"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		Toggle DeserializeMapPrefabToggle { get; set; } = Toggle.Yes;
		public bool ShouldDeserializeMapPrefab => DeserializeMapPrefabToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons, LabelText("Sprite Shape"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		Toggle DeserializeSpriteShapeToggle { get; set; } = Toggle.Yes;
		public bool ShouldDeserializeSpriteShape => DeserializeSpriteShapeToggle == Toggle.Yes;

		[field: SerializeField, EnumToggleButtons, LabelText("Colliders"),
		        TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		Toggle DeserializeColliderCoordsToggle { get; set; } = Toggle.Yes;
		public bool ShouldDeserializeColliderCoords => DeserializeColliderCoordsToggle == Toggle.Yes;

		[TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow),
		 Sirenix.OdinInspector.Button(ButtonSizes.Large, Stretch = false, ButtonAlignment = 1),
		 GUIColor(255 / 255f, 0f / 255, 55f / 255, 1f)]
		void DeleteSelectedSerialized() {
			if (string.IsNullOrWhiteSpace(NameSeedIteration))
				return;

			DirectoryAction.DeleteDirectory(NameSeedIteration, default, default);
			NameSeedIteration = Help.GetAllSeeds().FirstOrDefault();
		}

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
		[field: SerializeField, Range(50, Constants.MAP_DIMENSION_LIMIT),
		        Sirenix.OdinInspector.OnValueChanged("CheckIfEven"),
		        TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int Columns { get; set; } = 100;

		[Tooltip(Message.MAP_WILL_BE_RESIZED)]
		[field: SerializeField, Range(50, Constants.MAP_DIMENSION_LIMIT),
		        Sirenix.OdinInspector.OnValueChanged("CheckIfEven"),
		        TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int Rows { get; set; } = 100;

		[field: SerializeField, Range(1, 10), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int Scale { get; private set; } = 1;

		[field: SerializeField, Range(1, 10), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int BorderSize { get; private set; } = 1;

		[field: SerializeField, Range(1, 125), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int SmoothingIterations { get; private set; } = 5;

		[field: SerializeField, Range(10, 1000), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int WallRemovalThreshold { get; private set; } = 50;

		[field: SerializeField, Range(10, 1000), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int RoomRemovalThreshold { get; private set; } = 50;

		[field: SerializeField, Range(1, 4), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int LowerNeighborLimit { get; private set; } = 4;

		[field: SerializeField, Range(4, 8), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int UpperNeighborLimit { get; private set; } = 4;

		[field: SerializeField, PropertyTooltip(Message.PERCENTAGE_WALLS), Range(40, 55),
		        TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int WallFillPercentage { get; private set; } = 47;

		[field: SerializeField, Sirenix.OdinInspector.MinMaxSlider(1, 12),
		        TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public Vector2Int CorridorWidth { get; private set; } = new(1, 6);

		[field: SerializeField, TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public LayerMask GroundLayerMask { get; private set; }

		[field: SerializeField, TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public LayerMask ObstacleLayerMask { get; private set; }

		[field: SerializeField, TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public LayerMask BoundaryLayerMask { get; private set; }

		[field: SerializeField, TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public Material MeshMaterial { get; private set; }
        
#endregion

#region TILES

		
		[field: SerializeField,EnumToggleButtons, TabGroup("Tiles", TabLayouting = TabLayouting.MultiRow), LabelText("Render Tilemaps")]
		Toggle ShouldRenderTilesToggle { get; set; } = Toggle.Yes;
		public bool ShouldRenderTiles => ShouldRenderTilesToggle == Toggle.Yes;
		
		[field: SerializeField,EnumToggleButtons, TabGroup("Tiles", TabLayouting = TabLayouting.MultiRow), LabelText("Create Tile Labels")]
		Toggle CreateTileLabelsToggle { get; set; } = Toggle.Yes;
		public bool ShouldCreateTileLabels => CreateTileLabelsToggle == Toggle.Yes;

		[field: SerializeField,EnumToggleButtons, TabGroup("Tiles", TabLayouting = TabLayouting.MultiRow), LabelText("Create Tile Angles")]
		Toggle CreateTileAnglesToggle { get; set; } = Toggle.Yes;
		public bool ShouldGenerateAngles => CreateTileAnglesToggle == Toggle.Yes;

		[field: SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine),
		        TabGroup("Tiles", TabLayouting = TabLayouting.MultiRow), Title("Tilemaps")]
		public TileDictionary TileDictionary { get; private set; } = TileDictionary.GetDefault();

#endregion

#region PATHFINDING

		[field: SerializeField, EnumToggleButtons, Title("Astar"),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public ColliderSolverType GeneratedColliderType { get; private set; } = ColliderSolverType.Edge;

		[field: SerializeField, EnumToggleButtons, TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public ColliderType NavGraphCollisionType { get; private set; } = ColliderType.Capsule;

		[field: SerializeField, TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		[field: Range(0.05f, 5.0f)]
		public float NavGraphCollisionDetectionDiameter { get; private set; } = 0.1f;

		[field: SerializeField, TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		[field: Range(0.5f, 100.0f)]
		[field: Sirenix.OdinInspector.ShowIf("@NavGraphCollisionType == ColliderType.Capsule")]
		public float NavGraphCollisionDetectionHeight { get; private set; } = 1.0f;

		[field: SerializeField, Range(.1f, 8), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public float NavGraphNodeSize { get; private set; } = 0.5f;

		[field: SerializeField, EnumToggleButtons, Title("Erosion"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow), 
            LabelText("Erode Pathfinding Grid")]
		Toggle ErodePathfindingGridToggle { get; set; } = Toggle.Yes;
		public bool ErodePathfindingGrid  => ErodePathfindingGridToggle == Toggle.Yes;

		[field: SerializeField, Range(.1f, 8), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public float ErosionCollisionDiameter { get; private set; } = 1.75f;

		[field: SerializeField, Range(0, 10), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"),
		        Sirenix.OdinInspector.InfoBox(EROSION_TAG_INFO),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public int NodesToErodeAtBoundaries { get; private set; } = 3;

		[field: SerializeField, Range(0, 18), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public int StartingNodeIndexToErode { get; private set; } = 1;

		[Title("Erosion Debugging", horizontalLine: false)]
		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), EnumToggleButtons, LabelText("Draw Node Positions"),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		Toggle DrawNodePositionGizmosToggle { get; set; } = Toggle.Yes;
		public bool DrawNodePositionGizmos => DrawNodePositionGizmosToggle == Toggle.No;

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), EnumToggleButtons, LabelText("Draw Node Positions Shifted"),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		Toggle DrawTilePositionGizmosToggle { get; set; } = Toggle.Yes;
		public bool DrawTilePositionGizmos => DrawTilePositionGizmosToggle == Toggle.No;

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), TabGroup("Pathfinding",
			        TabLayouting = TabLayouting.MultiRow), EnumToggleButtons, LabelText("Draw Node Positions Shifted")]
		Toggle DrawNodePositionShiftedGizmosToggle { get; set; } = Toggle.Yes;
		public bool DrawTilePositionShiftedGizmos => DrawNodePositionShiftedGizmosToggle == Toggle.No;

#endregion

#region COLLIDERS

		[field: SerializeField, EnumToggleButtons, TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow),
		        Title("Solver")]
		public ColliderSolverType SolverType { get; private set; } = ColliderSolverType.Edge;

#region BOX_COLLIDER_SETTINGS

		[field: SerializeField, Title("Box Solver"), Sirenix.OdinInspector.ShowIf("@IsBox"),
		        Sirenix.OdinInspector.Required, TabGroup
			        ("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public AssetReference BoxColliderPrefab { get; private set; }

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@IsBox"), Range(0.1f, 1.5f),
		        TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float BoxColliderSkinWidth { get; private set; } = 0.1f;

#endregion

#region EDGE_COLLIDER_SETTINGS

		[field: SerializeField, Title("Edge Solver"), Sirenix.OdinInspector.ShowIf("@IsEdge"), Range(0.1f, 1f),
		        TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float EdgeColliderRadius { get; private set; } = 0.5f;

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@IsEdge"),
		        TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public Vector2 EdgeColliderOffset { get; private set; } = new(0.5f, -0.5f);

#endregion

#region PRIMITIVE_COLLIDER_SETTINGS

		[field: SerializeField, Title("Primitive Solver"), Sirenix.OdinInspector.ShowIf("@IsPrimitive"),
		        Range(0.1f, 1f), TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float PrimitiveColliderRadius { get; private set; } = 0.1f;

		[field: SerializeField, Range(0.1f, 1f), Sirenix.OdinInspector.ShowIf("@IsPrimitive"),
		        TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float PrimitiveColliderSkinWidth { get; private set; } = 0.5f;

		[field: SerializeField, TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow),
		        Sirenix.OdinInspector.ShowIf("@IsPrimitive")]
		[field: Range(0.1f, 5.0f)]
		public float NodeCullDistance { get; private set; } = .5f;

#endregion

		[field: SerializeField, Title("Cutters"), TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public List<GraphColliderCutter> ColliderCutters { get; private set; } = new();

		bool IsBox       => SolverType == ColliderSolverType.Box;
		bool IsEdge      => SolverType == ColliderSolverType.Edge;
		bool IsPrimitive => SolverType == ColliderSolverType.PrimitiveCombo;


		[TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow),
		 Sirenix.OdinInspector.Button(ButtonSizes.Large, Stretch = false, ButtonAlignment = 1),
		 GUIColor(154 / 255f, 208f / 255, 254f / 255, 1f)]
		void FindGraphCutters() => FindGraphColliderCuttersInScene();

#endregion

#region EVENTS

		[field: SerializeField, DictionaryDrawerSettings(IsReadOnly = true, DisplayMode = DictionaryDisplayOptions
			       .OneLine, KeyLabel = "EventId", ValueLabel = "Subscribers"), TabGroup("Events", TabLayouting =
			        TabLayouting.MultiRow)]
		public EventDictionary SerializedEvents { get; private set; } = new() {
			{ ProcessStep.Cleaning, default },
			{ ProcessStep.Initializing, default },
			{ ProcessStep.Running, default },
			{ ProcessStep.Completing, default },
			{ ProcessStep.Disposing, default },
			{ ProcessStep.Error, default },
		};

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

		IEnumerable GetAllSeedsWrapper() {
			var seeds           = Help.GetAllSeeds();
			var allSeedsWrapper = seeds.ToList();

			if (allSeedsWrapper.IsEmptyOrNull()) {
				NameSeedIteration = string.Empty;
				return Enumerable.Empty<string>();
			}

			return allSeedsWrapper;
		}

#endregion

		bool IsPathfinderNull => !Pathfinder;

	}
}