// ProceduralGeneration

namespace ProceduralGeneration {
	public readonly struct Dimensions {
		// height
		public int Rows { get; }
		
		// width
		public int Columns { get; }

		public Dimensions(int rows, int columns) {
			Rows    = rows;
			Columns = columns;
		}
	}
}