// ProceduralGeneration

using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	/// <summary>
	///  This is where the code gets opinionated. ORDER MATTERS.
	///  Reference StandardProcess.cs for an example of how to implement a process.
	///		1) Fill the map with walls (FillMapSolver)
	///		2) Smooth the map (SmoothMapSolver)
	/// </summary>
	internal abstract class GenerationProcess {
		protected IActions Actions { get; }

		internal abstract void Run(Span2D<int> map);

		protected GenerationProcess(IActions actions) {
			Actions = actions;
		}
	}
}