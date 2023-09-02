// Algorthims

namespace ProceduralGeneration {
	public struct Triangle {
		public   int   vertexIndexA;
		public   int   vertexIndexB;
		public   int   vertexIndexC;
		readonly int[] _vertices;

		public Triangle(int a, int b, int c) {
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;

			_vertices    = new int[3];
			_vertices[0] = a;
			_vertices[1] = b;
			_vertices[2] = c;
		}

		public int this[int i] => _vertices[i];

		public bool Contains(int vertexIndex) =>
			vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
	}
}