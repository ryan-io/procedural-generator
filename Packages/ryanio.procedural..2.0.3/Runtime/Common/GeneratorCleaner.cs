// ProceduralGeneration

namespace ProceduralGeneration {
	internal class GeneratorCleaner {
		IOwner Owner { get; }
		
		internal void Clean(bool alsoRoot = false) {
			if (alsoRoot)
				new EnsureCleanRootObject().Check(Owner.Go);
			
			Scale.Reset(Owner.Go);
		}

		internal GeneratorCleaner(IOwner owner) {
			Owner = owner;
		}
	}
}