// ProceduralGeneration

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProceduralGeneration {
	internal class DeserializationRouter {
		IActions              Actions                         { get; }
		GeneratorDeserializer Deserializer                    { get; }
		bool                  ShouldDeserializePathfinding    { get; }
		bool                  ShouldDeserializeMapPrefab      { get; }
		bool                  ShouldDeserializeSpriteShape    { get; }
		bool                  ShouldDeserializeColliderCoords { get; }
		bool                  ShouldDeserializeMesh           { get; }

		internal void Run(string nameSeedIteration, GameObject generatedColliderObject) {
			try {
				Help.ValidateNameIsSerialized(nameSeedIteration);
				var directories = DirectoryAction.GetMapDirectories(nameSeedIteration);

				if (ShouldDeserializeMesh)
					DeserializeMesh(nameSeedIteration, directories);

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
				Actions.LogError(
					nameSeedIteration             +
					Constants.SPACE               +
					Message.DESERIALIZATION_ERROR +
					e.Message,
					nameof(Run));
			}
		}

		void DeserializeMesh(string nameSeedIteration, (string raw, string full) directories) {
			var mesh = Deserializer.DeserializeMesh(nameSeedIteration, Constants.MESH_SAVE_PREFIX, directories);

			if (mesh == null)
				return;

			var rendering = new MeshRendering(Actions.GetOwner(), default);
			rendering.Render(mesh, nameSeedIteration);
		}

		void DeserializeMap(string nameSeedIteration, (string raw, string full) directories) {
			var obj        = Deserializer.DeserializeMapPrefab(nameSeedIteration, directories);
			var owner      = Actions.GetOwner();
			var ctxCreator = new ContextCreator(Actions);

			if (obj) {
				Object.Instantiate(obj, owner.transform, true);
				var grid = owner.GetComponentInChildren<Grid>();
				grid.gameObject.transform.localPosition = Vector3.zero;
				var tools = new GeneratorTools(ctxCreator.GetNewGeneratorToolsCtx());
				tools.SetOriginWrtMap(owner);
			}
		}

		void DeserializeAstar((string raw, string full) directories) {
			Deserializer.DeserializeAstar(Actions.GetDeserializationName(), directories.full);
		}

		void DeserializeSpriteShape(string nameSeedIteration, (string raw, string full) directories) {
			var coords = Deserializer.DeserializeVector3(
				nameSeedIteration, Constants.SPRITE_SHAPE_SAVE_PREFIX, directories);

			if (coords == default)
				return;

			var ctxCreator = new ContextCreator(Actions);
			var ctx        = ctxCreator.GetNewSpriteShapeBorderCtx(coords);

			ProceduralService.GetSpriteShapeBorderSolver(() => new SpriteShapeBorderSolver(ctx)).Generate();
		}

		void DeserializeColliderCoords(string nameSeedIteration, (string raw, string full) directories, GameObject go) {
			var coords = Deserializer.DeserializeVector3(
				nameSeedIteration, Constants.COLLIDER_COORDS_SAVE_PREFIX, directories);

			if (coords == default)
				return;

			var ctxCreator = new ContextCreator(Actions);

			var colSolver =
				new SerializedPrimitiveCollisionSolver(ctxCreator.GetNewSerializedPrimitiveCollisionSolverCtx());
			colSolver.CreateColliderFromDict(coords);
		}

		internal DeserializationRouter(DeserializationRoute route, IActions actions) {
			Deserializer                    = new GeneratorDeserializer(actions);
			Actions                         = actions;
			ShouldDeserializePathfinding    = route.ShouldDeserializePathfinding;
			ShouldDeserializeColliderCoords = route.ShouldDeserializeColliderCoords;
			ShouldDeserializeMapPrefab      = route.ShouldDeserializeMapPrefab;
			ShouldDeserializeSpriteShape    = route.ShouldDeserializeSpriteShape;
			ShouldDeserializeMesh           = route.ShouldDeserializeMesh;
		}
	}
}