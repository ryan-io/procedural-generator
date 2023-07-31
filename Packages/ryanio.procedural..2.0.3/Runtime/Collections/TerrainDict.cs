using System;
using System.Collections.Generic;
using UnityBCL;
using UnityEngine;

namespace Engine.Procedural.Runtime {
	[Serializable]
	public class TerrainContainer : SerializedDictionary<Terrain, Sprite> {
		public Sprite GetGrass() => GetTerrain(Terrain.Grass);

		public Sprite GetWall() => GetTerrain(Terrain.TestWall);

		public Sprite GetDirt() => GetTerrain(Terrain.Dirt);

		public Sprite GetTerrain(Terrain terrain) {
			try {
				var hasTerrainSprite = TryGetValue(Terrain.Grass, out var sprite);
				if (hasTerrainSprite) return sprite;
				throw new KeyNotFoundException();
			}
			catch (KeyNotFoundException e) {
				Debug.Log("There was no key found from the input value." + e.Message);
				throw;
			}
			catch (NullReferenceException e) {
				Debug.Log("The terrain dictionary was null or empty. Please create a new, populated dictionary.");
				Debug.Log(e.Message);
				throw;
			}
		}
	}
}