// ProceduralGeneration

namespace ProceduralGeneration {
	internal class Run {
		IActions Actions { get; }

		internal void Generation(IMachine machine) {
			machine.InvokeEvent(StateObservableId.ON_RUN);
			
			var generationRouter = new GenerationRouter(Actions, new StandardProcess(Actions));
			generationRouter.Run();
		}

		internal void Serialization(IMachine machine) {
			var ctxCreator = new ContextCreator(Actions);
			
			var router = new SerializationRouter(
				ctxCreator.GetNewSerializationRouterCtx(),
				ctxCreator.GetNewSerializationRoute(), 
				Actions );

			Actions.GetCoordinates();
			router.Run(Actions.GetMapName(), Actions.GetCoordinates());
		}

		internal void Deserialization(IMachine machine) {
			machine.InvokeEvent(StateObservableId.ON_RUN);

			var deserializeRouter = new DeserializationRouter(Actions);
			deserializeRouter.ValidateAndDeserialize(Actions.GetDeserializationName(), Actions.GetColliderGameObject());
		}
		
		public Run(IActions actions) {
			Actions = actions;
		}
	}
}