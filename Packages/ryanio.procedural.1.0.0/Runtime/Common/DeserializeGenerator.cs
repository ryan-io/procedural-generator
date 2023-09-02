// ProceduralGeneration

namespace ProceduralGeneration {
	internal class DeserializationService {
		IActions Actions { get; }
		
		public void Run(IActions actions) {
			// run
		}

		public DeserializationService(IActions actions) {
			Actions = actions;
		}
	}
}