// ProceduralGeneration

namespace ProceduralGeneration {
	internal class Run {
		IActions Actions { get; }
		IMachine Machine { get; }

		internal MapData Generation() {
			var process          = new StandardProcess(Actions);
			var generationRouter = new GenerationRouter(Actions, process);
			
			Machine.RegisterEvent(StateObservableId.ON_DISPOSE, process.Dispose);
			
			return generationRouter.Run();
		}

		internal void Serialization() {
			var ctxCreator = new ContextCreator(Actions);
			
			var router = new SerializationRouter(
				ctxCreator.GetNewSerializationRouterCtx(),
				ctxCreator.GetNewSerializationRoute(), 
				Actions );

			router.Run(Actions.GetMapName(), Actions.GetCoordinates());
		}

		internal void Deserialization() {
			var ctxCreator = new ContextCreator(Actions);
			
			var router = new DeserializationRouter(
				ctxCreator.GetNewDeserializationRoute(), 
				Actions);
			
			router.Run(Actions.GetDeserializationName(), Actions.GetColliderGameObject());
		}
		
		public Run(IActions actions, IMachine machine) {
			Actions = actions;
			Machine = machine;
		}
	}
}