// Engine.Procedural

using System;
using System.Collections.Generic;

namespace Engine.Procedural.Runtime {
	internal class ValueTupleIntComparer : EqualityComparer<(int, int)> {
		public override bool Equals(ValueTuple<int, int> tuple1, ValueTuple<int, int> tuple2) {
			return tuple1.Item1 == tuple2.Item1 &&
			       tuple1.Item2 == tuple2.Item2;
		}

		public override int GetHashCode(ValueTuple<int, int> x) {
			return (x.Item1, x.Item2).GetHashCode();
		}
	}
}