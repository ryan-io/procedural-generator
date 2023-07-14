﻿using UnityEngine;

namespace Engine.Procedural {
	public class DebugPoissonMono : MonoBehaviour {
		[SerializeField] DebugPoissonSpawner spawner;

		void Start() {
			if (spawner == null) return;
			spawner.Spawn(transform.position);
		}
	}
}