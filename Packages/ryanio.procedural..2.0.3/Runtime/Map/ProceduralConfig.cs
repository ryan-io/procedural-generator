using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Pathfinding;
using Sirenix.OdinInspector;
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
		
		[field: SerializeField, TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public bool IsBuild { get; private set; }

		[field: SerializeField, TabGroup("Setup", TabLayouting = TabLayouting.MultiRow), Sirenix.OdinInspector.OnValueChanged("ResetShouldDeserialize")]
		public bool ShouldGenerate { get; set; } = true;

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@!ShouldGenerate"), TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldDeserialize { get; set; } = true;
		
		[field: SerializeField, Title("Seed"), TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public bool UseRandomSeed { get; private set; } = true;

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("UseRandomSeed"), TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public string Seed { get; internal set; }

		[field: SerializeField, Sirenix.OdinInspector.ReadOnly, TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public string LastSeed { get; set; }

		[field: SerializeField, Sirenix.OdinInspector.ReadOnly, TabGroup("Setup", TabLayouting = TabLayouting.MultiRow)]
		public int LastIteration { get; set; }

		[field: SerializeField, Title("Monobehaviors"), HorizontalLine, Sirenix.OdinInspector.Required,TabGroup("Setup", TabLayouting = 
            TabLayouting.MultiRow), LabelText("Pathfinder"),LabelWidth(LABEL_WIDTH), SceneObjectsOnly]
		public GameObject Pathfinder { get; private set; }

#endregion

#region SERIALIZATION

		[field: SerializeField,Title("Iteration Tracker"), TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow), 
		        ValueDropdown(@"GetAllSeedsWrapper")]
		public string NameSeedIteration { get; set; }
		
		[field: SerializeField,Title("Serialization"), TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldSerializePathfinding { get; private set; } = true;

		[field: SerializeField, TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldSerializeMapPrefab { get; private set; } = true;

		[field: SerializeField, TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldSerializeSpriteShape { get; private set; } = true;
		
		[field: SerializeField, TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldSerializeColliderCoords { get; private set; } = true;

#endregion

#region DESERIALIZATION
		
		[field: SerializeField, Title("Deserialization"), TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldDeserializePathfinding { get; private set; } = true;

		[field: SerializeField, TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldDeserializeMapPrefab { get; private set; } = true;

		[field: SerializeField, TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldDeserializeSpriteShape { get; private set; } = true;
		
		[field: SerializeField, TabGroup("Serialize & Deserialize", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldDeserializeColliderCoords { get; private set; } = true;
		
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
		[field: SerializeField, Range(50, Constants.MAP_DIMENSION_LIMIT), Sirenix.OdinInspector.OnValueChanged("CheckIfEven"),
		        TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public int Columns { get; set; } = 100;

		[Tooltip(Message.MAP_WILL_BE_RESIZED)]
		[field: SerializeField, Range(50, Constants.MAP_DIMENSION_LIMIT), Sirenix.OdinInspector.OnValueChanged("CheckIfEven"),
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

		[field: SerializeField, Sirenix.OdinInspector.MinMaxSlider(1, 12), TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public Vector2Int CorridorWidth { get; private set; } = new(1, 6);

		[field: SerializeField, TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public LayerMask GroundLayerMask { get; private set; }

		[field: SerializeField, TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public LayerMask ObstacleLayerMask { get; private set; }

		[field: SerializeField, TabGroup("Map", TabLayouting = TabLayouting.MultiRow)]
		public LayerMask BoundaryLayerMask { get; private set; }

#endregion

#region TILES

		[field: SerializeField, TabGroup("Tiles", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldCreateTileLabels { get; private set; }

		[field: SerializeField, TabGroup("Tiles", TabLayouting = TabLayouting.MultiRow)]
		public bool ShouldGenerateAngles { get; private set; }

		[field: SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine),
		        TabGroup("Tiles", TabLayouting = TabLayouting.MultiRow), Title("Tilemaps")]
		public TileDictionary TileDictionary { get; private set; } = TileDictionary.GetDefault();

#endregion

#region PATHFINDING

		[field: SerializeField, EnumToggleButtons, Title("Astar"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
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

		[field: SerializeField, Range(.1f, 8), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public float NavGraphNodeSize { get; private set; } = 0.5f;

		[field: SerializeField, Title("Erosion"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public bool ErodePathfindingGrid { get; private set; } = true;

		[field: SerializeField, Range(.1f, 8), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public float ErosionCollisionDiameter { get; private set; } = 1.75f;

		[field: SerializeField, Range(0, 10), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), Sirenix.OdinInspector.InfoBox(EROSION_TAG_INFO),
		        TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public int NodesToErodeAtBoundaries { get; private set; } = 3;

		[field: SerializeField, Range(0, 18), Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public int StartingNodeIndexToErode { get; private set; } = 1;

		[Title("Erosion Debugging", horizontalLine: false)]
		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public bool DrawNodePositionGizmos { get; private set; }

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow)]
		public bool DrawTilePositionGizmos { get; private set; }

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@ErodePathfindingGrid"), TabGroup("Pathfinding", 
            TabLayouting = TabLayouting.MultiRow)]
		public bool DrawTilePositionShiftedGizmos { get; private set; }
		
		[TabGroup("Pathfinding", TabLayouting = TabLayouting.MultiRow),
		 Sirenix.OdinInspector.Button(ButtonSizes.Large, Stretch = false, ButtonAlignment = 1), 
		 GUIColor(154 / 255f, 208f / 255, 254f      / 255,  1f)]
		void FindPathfinder() => FindPathfinderInScene();

#endregion

#region COLLIDERS

		[field: SerializeField, EnumToggleButtons, TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow), Title("Solver")]
		public ColliderSolverType SolverType { get; private set; } = ColliderSolverType.Edge;

#region BOX_COLLIDER_SETTINGS

		[field: SerializeField, Title("Box Solver"), Sirenix.OdinInspector.ShowIf("@IsBox"), Sirenix.OdinInspector.Required, TabGroup
            ("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public AssetReference BoxColliderPrefab { get; private set; }

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@IsBox"), Range(0.1f, 1.5f), TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float BoxColliderSkinWidth { get; private set; } = 0.1f;

#endregion

#region EDGE_COLLIDER_SETTINGS

		[field: SerializeField,Title("Edge Solver"), Sirenix.OdinInspector.ShowIf("@IsEdge"), Range(0.1f, 1f), TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float EdgeColliderRadius { get; private set; } = 0.5f;

		[field: SerializeField, Sirenix.OdinInspector.ShowIf("@IsEdge"), TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public Vector2 EdgeColliderOffset { get; private set; } = new(0.5f, -0.5f);

#endregion

#region PRIMITIVE_COLLIDER_SETTINGS

		[field: SerializeField,Title("Primitive Solver"), Sirenix.OdinInspector.ShowIf("@IsPrimitive"), Range(0.1f, 1f), TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float PrimitiveColliderRadius { get; private set; } = 0.1f;

		[field: SerializeField, Range(0.1f, 1f), Sirenix.OdinInspector.ShowIf("@IsPrimitive"), TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow)]
		public float PrimitiveColliderSkinWidth { get; private set; } = 0.5f;
		
		[field: SerializeField, TabGroup("Colliders", TabLayouting = TabLayouting.MultiRow), Sirenix.OdinInspector.ShowIf("@IsPrimitive")]
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
            .OneLine, KeyLabel = "EventId", ValueLabel = "Subscribers"),  TabGroup("Events", TabLayouting = 
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
			var seeds = Help.GetAllSeeds();
			var allSeedsWrapper = seeds.ToList();
			
			if (allSeedsWrapper.IsEmptyOrNull()) {
				NameSeedIteration = string.Empty;
				return Enumerable.Empty<string>();
			}

			return allSeedsWrapper;
		}

		void ResetShouldDeserialize() {
			ShouldDeserialize = false;
		}
		
#endregion
	}
}