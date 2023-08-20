// ProceduralGeneration

using System;

namespace ProceduralGeneration {
	internal class GeneratorCleaner {
		IActions Actions { get; }

		internal void Clean(ProceduralConfig config, bool alsoRoot = false) {
			var owner              = Actions.GetOwner();

			if (alsoRoot)
				new EnsureCleanRootObject().Check(owner);

			Help.ClearConsole();
			Scale.Reset(owner);

			new ConfigCleaner().Clean(config);
			new CleanSpriteShapes().Clean(owner);
			new ColliderGameObjectCleaner().Clean(owner, true);
			new MeshCleaner().Clean(owner);
			new GraphCleaner().Clean();
			new RenderCleaner().Clean(owner);
			new EnsureMapFitsOnStack().Ensure(config);
			new CleanSpriteShapes().Clean(owner);

			Actions.Log(Message.CLEAN_COMPLETE, nameof(Clean));
		}

		internal GeneratorCleaner(IActions actions) {
			Actions = actions;
		}
	}
}