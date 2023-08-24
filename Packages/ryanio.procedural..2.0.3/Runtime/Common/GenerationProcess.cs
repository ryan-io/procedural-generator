// ProceduralGeneration

using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	/// <summary>
	///  This is where the code gets opinionated. ORDER MATTERS.
	///  Reference StandardProcess.cs for an example of how to implement a process.
	///		1) Fill the map with walls (FillMapSolver)
	///		2) Smooth the map (SmoothMapSolver)
	///		3) Remove regions (RegionRemovalSolver)
	///		4) Set tiles (TileSetterSolver)
	///		5) Create mesh (MeshSolver)
	///		6) Build navigation (NavigationSolver)
	///		
	/// </summary>
	internal abstract class GenerationProcess {
		protected IActions Actions { get; }

		internal abstract MapData Run(Span2D<int> map);

		protected GenerationProcess(IActions actions) {
			Actions = actions;
		}
	}
}