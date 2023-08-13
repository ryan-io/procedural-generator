// ProceduralGeneration

using System;

namespace ProceduralGeneration {
	internal class GeneratorCleaner {
		IActions Actions { get; }

		internal void Clean(IMachine machine, bool alsoRoot = false) {
			machine.InvokeEvent(StateObservableId.ON_CLEAN);
			var owner              = Actions.GetOwner();
			var proceduralConfig = Actions.GetProceduralConfig();
			
			if (alsoRoot)
				new EnsureCleanRootObject().Check(owner);

			Help.ClearConsole();
			Scale.Reset(owner);

			new ConfigCleaner().Clean(proceduralConfig);
			new CleanSpriteShapes().Clean(owner);
			new ColliderGameObjectCleaner().Clean(owner, true);
			new MeshCleaner().Clean(owner);
			new GraphCleaner().Clean();
			new RenderCleaner().Clean(owner);
			new EnsureMapFitsOnStack().Ensure(proceduralConfig);
			new CleanSpriteShapes().Clean(owner);

			Actions.Log(Message.CLEAN_COMPLETE, nameof(Clean));
		}

		internal GeneratorCleaner(IActions actions) {
			Actions = actions;
		}
	}
}