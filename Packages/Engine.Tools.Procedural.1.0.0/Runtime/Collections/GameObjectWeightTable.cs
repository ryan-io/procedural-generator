using System;
using UnityBCL;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Engine.Procedural {
	[Serializable]
	public class GameObjectWeightTable : SerializedDictionary<GameObject, double> {
	}
}