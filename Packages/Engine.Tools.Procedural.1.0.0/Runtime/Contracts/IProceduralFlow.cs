namespace Engine.Procedural {
	public interface IProceduralFlow {
		ProceduralExitHandler ExitHandler       { get; }
		ProceduralCoreConfig  CoreConfiguration { get; }
		void                  Cancel();
	}
}