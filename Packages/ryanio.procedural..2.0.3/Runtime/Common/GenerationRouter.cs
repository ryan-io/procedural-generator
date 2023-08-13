// ProceduralGeneration

using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal class GenerationRouter {
		IActions Actions { get; }

		/// <summary>
		///  ***THIS IS NOT GARBAGE COLLECTED WHILE THE SPAN IS STILL IN CONTEXT***
		///  This is an unsafe method.
		/// </summary>
		internal unsafe void Run() {
			// the distance between each row in the span; should always be 0
			const int pitch = 0;

			var dimensions     = Actions.GetMapDimensions();
			var primaryPointer = stackalloc int[dimensions.Rows * dimensions.Columns];
			var map            = new Span2D<int>(primaryPointer, dimensions.Rows, dimensions.Columns, pitch);

			_process.Run(map);
		}

		public GenerationRouter(IActions actions, GenerationProcess process) {
			Actions  = actions;
			_process = process;
		}

		readonly GenerationProcess _process;
	}
}