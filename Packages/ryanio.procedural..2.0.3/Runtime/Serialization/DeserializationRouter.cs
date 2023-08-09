// ProceduralGeneration

using System;
using BCL;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Engine.Procedural.Runtime {
	public class DeserializationRouter {
		GameObject            Owner                           { get; }
		GeneratorDeserializer Deserializer                    { get; }
		StopWatchWrapper      StopWatch                       { get; }
		bool                  ShouldDeserializePathfinding    { get; }
		bool                  ShouldDeserializeMapPrefab      { get; }
		bool                  ShouldDeserializeSpriteShape    { get; }
		bool                  ShouldDeserializeColliderCoords { get; }

		public void ValidateAndDeserialize(string nameSeedIteration, GameObject generatedColliderObject) {
			try {
				Help.ValidateNameIsSerialized(nameSeedIteration);
				var directories = DirectoryAction.GetMapDirectories(nameSeedIteration);

				if (ShouldDeserializeMapPrefab)
					DeserializeMap(nameSeedIteration, directories);

				if (ShouldDeserializePathfinding)
					DeserializeAstar(directories);

				if (ShouldDeserializeSpriteShape)
					DeserializeSpriteShape(nameSeedIteration, directories);

				if (ShouldDeserializeColliderCoords)
					DeserializeColliderCoords(nameSeedIteration, directories, generatedColliderObject);
			}
			catch (Exception e) {
				GenLogging.Instance.LogWithTimeStamp(
					LogLevel.Error,
					StopWatch.TimeElapsed,
					$"Error deserializing map: {nameSeedIteration}: {e.Message}",
					"DeserializationRouter.ValidateAndDeserialize");
			}
		}

		void DeserializeMap(string nameSeedIteration, (string raw, string full) directories) {
			var obj = Deserializer.DeserializeMapPrefab(nameSeedIteration, directories);

			if (obj) {
				Object.Instantiate(obj, Owner.transform, true);
				var grid = Owner.GetComponentInChildren<Grid>();
				grid.gameObject.transform.localPosition = Vector3.zero;
				var tools = new GeneratorTools(_config, grid, default);
				tools.SetOriginWrtMap(Owner);
			}
		}

		void DeserializeAstar((string raw, string full) directories) {
			Deserializer.DeserializeAstar(_config.NameSeedIteration, directories.full);
		}

		void DeserializeSpriteShape(string nameSeedIteration, (string raw, string full) directories) {
			var positions = Deserializer.DeserializeVector3(
				nameSeedIteration, Constants.SPRITE_SHAPE_SAVE_PREFIX, directories);

			var solver = new SpriteShapeBorderSolver(_spriteShapeConfig, Owner);
			solver.GenerateProceduralBorder(positions, _config.NameSeedIteration);
		}

		void DeserializeColliderCoords(string nameSeedIteration, (string raw, string full) directories, GameObject go) {
			var colPos = Deserializer.DeserializeVector3(
				nameSeedIteration, Constants.COLLIDER_COORDS_SAVE_PREFIX, directories);

			var colSolver = new SerializedPrimitiveCollisionSolver(_config, go);
			colSolver.CreateColliderFromDict(colPos);
		}

		public DeserializationRouter(
			GameObject procGenOwner,
			ProceduralConfig config,
			SpriteShapeConfig spriteShapeConfig,
			StopWatchWrapper stopwatch) {
			Deserializer                    = new GeneratorDeserializer(config, stopwatch);
			Owner                           = procGenOwner;
			ShouldDeserializePathfinding    = config.ShouldDeserializePathfinding;
			ShouldDeserializeMapPrefab      = config.ShouldDeserializeMapPrefab;
			ShouldDeserializeSpriteShape    = config.ShouldDeserializeSpriteShape;
			ShouldDeserializeColliderCoords = config.ShouldDeserializeColliderCoords;
			StopWatch                       = stopwatch;
			_config                         = config;
			_spriteShapeConfig              = spriteShapeConfig;
		}

		readonly ProceduralConfig  _config;
		readonly SpriteShapeConfig _spriteShapeConfig;
	}
}