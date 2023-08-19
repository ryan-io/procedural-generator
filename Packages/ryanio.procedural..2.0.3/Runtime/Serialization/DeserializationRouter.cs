// ProceduralGeneration

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	public class DeserializationRouter {
		IActions              Actions      { get; }
		GeneratorDeserializer Deserializer { get; }

		public void ValidateAndDeserialize(string nameSeedIteration, GameObject generatedColliderObject) {
			try {
				Help.ValidateNameIsSerialized(nameSeedIteration);
				var directories = DirectoryAction.GetMapDirectories(nameSeedIteration);
				var config      = Actions.GetProceduralConfig();

				if (config.ShouldDeserializeMapPrefab)
					DeserializeMap(nameSeedIteration, directories);

				if (config.ShouldDeserializePathfinding)
					DeserializeAstar(directories);

				if (config.ShouldDeserializeSpriteShape)
					DeserializeSpriteShape(nameSeedIteration, directories);

				if (config.ShouldDeserializeColliderCoords)
					DeserializeColliderCoords(nameSeedIteration, directories, generatedColliderObject);
			}
			catch (Exception e) {
				Actions.LogError(
					nameSeedIteration             +
					Constants.SPACE               +
					Message.DESERIALIZATION_ERROR +
					e.Message,
					nameof(ValidateAndDeserialize));
			}
		}

		void DeserializeMap(string nameSeedIteration, (string raw, string full) directories) {
			var obj    = Deserializer.DeserializeMapPrefab(nameSeedIteration, directories);
			var owner  = Actions.GetOwner();

			if (obj) {
				Object.Instantiate(obj, owner.transform, true);
				var grid = owner.GetComponentInChildren<Grid>();
				grid.gameObject.transform.localPosition = Vector3.zero;
				var tools = new GeneratorTools(Actions.GetProceduralConfig(), grid, default);
				tools.SetOriginWrtMap(owner);
			}
		}

		void DeserializeAstar((string raw, string full) directories) {
			Deserializer.DeserializeAstar(Actions.GetDeserializationName(), directories.full);
		}

		void DeserializeSpriteShape(string nameSeedIteration, (string raw, string full) directories) {
			var positions = Deserializer.DeserializeVector3(
				nameSeedIteration, Constants.SPRITE_SHAPE_SAVE_PREFIX, directories);

			var solver = new SpriteShapeBorderSolver(Actions.GetSpriteShapeConfig(), Actions.GetOwner());
			solver.Generate(positions, Actions.GetDeserializationName());
		}

		void DeserializeColliderCoords(string nameSeedIteration, (string raw, string full) directories, GameObject go) {
			var colPos = Deserializer.DeserializeVector3(
				nameSeedIteration, Constants.COLLIDER_COORDS_SAVE_PREFIX, directories);

			var colSolver = new SerializedPrimitiveCollisionSolver(Actions.GetProceduralConfig(), go);
			colSolver.CreateColliderFromDict(colPos);
		}

		public DeserializationRouter(IActions actions) {
			Actions      = actions;
			Deserializer = new GeneratorDeserializer(actions.GetProceduralConfig());
		}
	}
}