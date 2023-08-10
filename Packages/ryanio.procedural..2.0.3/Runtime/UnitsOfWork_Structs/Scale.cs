using UnityEngine;

namespace ProceduralGeneration {
	public static class Scale {
		/// <summary>
		///  Scales the owner to the given size.
		/// </summary>
		/// <param name="owner">GameObject with ProceduralGenerator component</param>
		/// <param name="size">Size to scale to</param>
		public static void Current(GameObject owner, float size) {
			size = ValidateSize(size);

			owner.transform.localScale = new Vector3(size, size, size);
		}

		/// <summary>
		///   Scales the owner and all of its children to the given size.
		/// </summary>
		/// <param name="owner">GameObject with ProceduralGenerator component</param>
		/// <param name="size">Size to scale to</param>
		public static void CurrentNested(GameObject owner, float size) {
			size = ValidateSize(size);

			var count = owner.transform.childCount;

			for (var i = 0; i < count; i++) {
				var child = owner.transform.GetChild(i);
				child.localScale = new Vector3(size, size, size);
			}
		}

		static float ValidateSize(float size) {
			if (size < MIN_SCALE)
				size = MIN_SCALE;

			size = Mathf.Abs(size);
			return size;
		}

		const float MIN_SCALE = 0.1F;
	}
}