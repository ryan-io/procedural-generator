using System;
using System.Collections.Generic;
using BCL;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Engine.Procedural.Poisson_Spawning {
	[RequireComponent(typeof(Poisson))]
	[ExecuteInEditMode]
	public class PoissonSpawner : MonoBehaviour {
		//[SerializeField]  string             identifier = "Poisson Spawner";
		[SerializeField] PoissonSpawnerData data;
		TextMeshPro                         _label;
		[ShowInInspector] [ReadOnly] int    _numberOfPrefabsThatWillBeSpawned;
		IPoisson               _objectHandler;

		GameObject _start;

		Vector3 Center => transform.position;

		Vector3 Size {
			get {
				var size = data.regionSize;
				return new Vector3(size.x, size.y, 0f);
			}
		}

		void Update() {
			if (Application.isPlaying)
				return;

			_numberOfPrefabsThatWillBeSpawned = PoissonPointGenerator.GeneratePoints(
				data.defaultRadius, data.regionSize,
				data.samplesBeforeRejection).Count;
		}

		void OnDrawGizmos() {
			OnGizmosCheck();

			if (data.enableGizmos == Toggle.No)
				return;

			Gizmos.DrawWireCube(Center, Size);
		}

		void SetStart() {
			var size = data.regionSize;
			_start                         = new GameObject("PoissonStart");
			_start.transform.parent        = transform;
			_start.transform.localPosition = new Vector3(-size.x / 2f, -size.y / 2f, 0f);
		}

		void OnGizmosCheck() {
			if (data.enableGizmos      == Toggle.Yes && _label == null) DrawLabel();
			else if (data.enableGizmos == Toggle.No  && _label != null) RemoveLabel();
		}

		void DrawLabel() {
			// _label = global::Utility.Omni.CreateWorldTextTMP(identifier, gameObject.transform, 12, Color.red,
			// 	TextAlignmentOptions
			// 	   .Center, new Vector3(0f, 0f, 10f));
		}

		void RemoveLabel() {
			if (_label == null) return;
			if (_label.transform != null) {
				if (Application.isPlaying) Destroy(_label.gameObject);
				else DestroyImmediate(_label.gameObject);
			}
		}
#if UNITY_EDITOR
		[Button]
		public void Spawn() {
			Despawn();
			DrawLabel();
			SetStart();

			var weightedObjects = ObjectHandler.GetWeightedRandom(data.weightTable);
			var bounds = Bounds.GetBounds(data.weightTable);
			var spawnPoints = PoissonPointGenerator.GeneratePoints(
				data.defaultRadius, data.regionSize,
				data.samplesBeforeRejection);
			var objects =
				ObjectHandler.SpawnObjects(spawnPoints, weightedObjects, _start.transform);

			if (data.mods == null || data.mods.Count < 1) throw new Exception(CantSpawnObjects());
			ApplyMods(objects);
		}

		void ApplyMods(IEnumerable<GameObject> objects) {
			foreach (var obj in objects) PoissonModProcessor.Process(obj.transform, data.mods);
		}

		static string CantSpawnObjects() =>
			"Please debug the mods associated with this Poisson Spawner. The collection" +
			" is either null. No objects will be spawned.";

		[Button]
		public void Despawn() {
			foreach (var tr in GetComponentsInChildren<Transform>()) {
				if (tr == transform) continue;
				if (tr != null) {
					if (Application.isPlaying) Destroy(tr.gameObject);
					else DestroyImmediate(tr.gameObject);
				}
			}
		}

		[Button]
		// void TestWeightedItemSystem() {
		// 	var test = new WeightedRandom<GameObject>();
		// 	test.Add(data.objects[1],   20.0);
		// 	test.Add(data.objects[0],  5.0);
		//
		// 	for (int i = 0; i < 2000; i++) {
		// 		Debug.Msg(test.GetRandom());
		// 	}
		// }
#endif

		public IPoisson ObjectHandler => _objectHandler ??= GetComponent<Poisson>();
	}
}