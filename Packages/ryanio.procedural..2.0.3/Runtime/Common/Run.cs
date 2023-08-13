// ProceduralGeneration

namespace ProceduralGeneration {
	internal class Run {
		IActions Actions { get; }

		internal void Generation(IMachine machine) {
			machine.InvokeEvent(StateObservableId.ON_RUN);
			
			var generationRouter = new GenerationRouter(Actions, new StandardProcess(Actions));
			generationRouter.Run();
			
			SetStateComplete(machine);
		}

		internal void Deserialization(IMachine machine) {
			machine.InvokeEvent(StateObservableId.ON_RUN);

			var deserializeRouter = new DeserializationRouter(Actions);
			deserializeRouter.ValidateAndDeserialize(Actions.GetDeserializationName(), Actions.GetColliderGameObject());
			
			SetStateComplete(machine);
		}
		
		void SetStateComplete(IMachine machine) {
			machine.InvokeEvent(StateObservableId.ON_COMPLETE);
		}

		public Run(IActions actions) {
			Actions = actions;
		}
	}
}