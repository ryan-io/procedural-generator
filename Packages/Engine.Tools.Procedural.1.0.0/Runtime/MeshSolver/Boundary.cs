namespace Engine.Procedural {
	public readonly struct Boundary {
		public Boundary(float right, float left, float up, float down) {
			Right = right;
			Left  = left;
			Up    = up;
			Down  = down;
		}

		public float Right { get; }
		public float Left  { get; }
		public float Up    { get; }
		public float Down  { get; }
	}
}