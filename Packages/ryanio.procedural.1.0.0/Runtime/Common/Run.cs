// ProceduralGeneration

namespace ProceduralGeneration {
	internal class Run {
		IActions Actions { get; }

		internal void Generation() {
			var generationRouter = new GenerationRouter(Actions, new StandardProcess(Actions));
			generationRouter.Run();
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
		
		public Run(IActions actions) {
			Actions = actions;
		}
	}
}