// ProceduralGeneration

using System.Collections.Generic;
using System.Linq;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	internal readonly struct Convert {
		internal IReadOnlyDictionary<int, List<SerializableVector3>> DictionaryVector3ToSerializedVector3(
			IReadOnlyDictionary<int, List<Vector3>> col) {
			var output = new Dictionary<int, List<SerializableVector3>>();
			var index  = col.Keys.First();

			foreach (var corner in col) {
				var serializedList = corner.Value.AsSerialized().ToList();
				output.Add(index, serializedList);
				index++;
			}

			// for (var i = index; i < col.Count; i++) {
			// 	output[i] = col[i].AsSerialized().ToList();
			// }

			return output;
		}

		internal IReadOnlyDictionary<int, List<SerializableVector2>> DictionaryVector2ToSerializedVector2(
			IReadOnlyDictionary<int, List<Vector2>> col) {
			var output = new Dictionary<int, List<SerializableVector2>>();
			var index  = col.Keys.First();

			foreach (var corner in col) {
				var serializedList = corner.Value.AsSerialized().ToList();
				output.Add(index, serializedList);
				index++;
			}

			// for (var i = index; i < col.Count; i++) {
			// 	output[i] = col[i].AsSerialized().ToList();
			// }

			return output;
		}
		
		/*	BEFORE REFACTOR
		 * internal Dictionary<int, List<SerializableVector3>> GetBoundaryCoords() {
			if (SpriteBoundaryCoords.IsEmptyOrNull())
				return default;

			var dict  = new Dictionary<int, List<SerializableVector3>>();
			var index = SpriteBoundaryCoords.Keys.First();

			foreach (var corner in SpriteBoundaryCoords) {
				var serializedList = corner.Value.AsSerialized().ToList();
				dict.Add(index, serializedList);
				index++;
			}

			return dict;
		}
		 */
	}
}