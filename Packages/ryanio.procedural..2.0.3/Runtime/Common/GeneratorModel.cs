// ProceduralGeneration

using BCL;

namespace ProceduralGeneration {
	internal class GeneratorModel {
		internal TileHashset      TileHashset { get; private set; }
		internal StopWatchWrapper StopWatch   { get; private set; }
		
		
		internal GeneratorModel() {
			TileHashset = new TileHashset();
			StopWatch   = new StopWatchWrapper(true);	
		}
	}
}