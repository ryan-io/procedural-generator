// ProceduralGeneration

using System.Threading;
using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal class GenerationRouter {
		IActions Actions { get; }

		/// <summary>
		///  ***THIS IS NOT GARBAGE COLLECTED WHILE THE SPAN IS STILL IN CONTEXT***
		///  This is an unsafe method.
		/// </summary>
		internal MapData Run() {
			// the distance between each row in the span; should always be 0
			const int pitch = 0;

			var  dimensions     = Actions.GetMapDimensions();
			//int* primaryPointer = stackalloc int[dimensions.Rows * dimensions.Columns];
			var  map            = new int[dimensions.Rows,dimensions.Columns];
			//var  map            = new Span2D<int>(primaryPointer, dimensions.Rows, dimensions.Columns, pitch);
			//map.Clear();
			
			var data =_process.Run(ref map);

			// var scanner = new GraphScanner();
			// scanner.Fire(new GraphScanner.Args(AstarPath.active.graphs[0], false), CancellationToken.None);
			
			Actions.Log("'Standard Process' generation complete.", nameof(ProceduralGeneration.Run));
			return data;
		}

		public GenerationRouter(IActions actions, GenerationProcess process) {
			Actions  = actions;
			_process = process;
		}

		readonly GenerationProcess _process;
	}
}