// ProceduralGeneration

using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralGeneration {
	internal class ColliderPointProcessor : IDisposable {
		bool IsDisposed { get; set; }
		
		Dictionary<int, List<Vector2>> _output = new();

		//PopulateColliderPointsJob _populatePointsJob = new();
		DetermineColinearityJob   _colinearityJob    = new();

		// JobHandle _populatePointsJobHandle = new();
		// JobHandle _colinearityJobHandle    = new();
		
		HashSet<NativeList<float2>> _v2Hash  = new();
		HashSet<NativeList<int>>     _intHash = new();

		int OutlineId { get; set; } = 0;

		const int BATCH_LOOP = 64;

		internal Dictionary<int, List<Vector2>> Process(List<List<int>> outlines, List<float2> vertices) {
			try {
				foreach (var outline in outlines) {
					if (outline.Count <= 10)
						continue;

					var points = new NativeList<float2 >(outline.Count, Allocator.Persistent);

					for (var i = 0; i < outline.Count; i++) {
						points.Add(new float2(vertices[outline[i]].x, vertices[outline[i]].y));
					}
					
					var processed = new NativeList<float2 >(outline.Count, Allocator.Persistent);
					processed.CopyFrom(points);
					//DeterminePoints(outline, vertices);

					// while (true) {
					// 	if (_populatePointsJobHandle.IsCompleted)
					// 		break;  
					// }  

					//_populatePointsJobHandle.Complete();

					_colinearityJob.PointsToCheck = points;
					_colinearityJob.Processed     = processed;
					//_colinearityJobHandle =
					_colinearityJob.Run();
					//DetermineColinearity(points);
					//
					// while (true) {
					// 	if (_colinearityJobHandle.IsCompleted)
					// 		break;
					// }

					//_colinearityJobHandle.Complete();
					//_colinearityJob.Run();
					_output[OutlineId] = GetManagedList(_colinearityJob.Processed);

					OutlineId++;
				}
			}
			catch (Exception e) {
				Debug.LogError(e.Message);
				Dispose();
			}

			return _output;
		}

		List<Vector2> GetManagedList(NativeList<float2> colinearityJobProcessed) {
			var list = new List<Vector2>();

			foreach (var point in colinearityJobProcessed) {
				list.Add(point);
			}

			return list;
		}

		void DeterminePoints(IReadOnlyList<int> outline, IReadOnlyList<float2> vertices) {
			// _populatePointsJob.Vertices = GetVerticesNative(vertices);
			// _populatePointsJob.Outline  = GetOutlinesNative(outline);
			// _populatePointsJob.Output   = GetOutputListNative(outline.Count);
			// _populatePointsJob.Run();
		}

		void DetermineColinearity(NativeList<float2> populatePointsOutput) {
			var processed = GetOutputListNative(populatePointsOutput.Capacity);

			_colinearityJob.PointsToCheck = populatePointsOutput;
			_colinearityJob.Processed     = processed;
			//_colinearityJobHandle =
				_colinearityJob.Run();
		}

		NativeList<float2> GetVerticesNative(IReadOnlyList<float2> vertices) {
			var verticesNative = new NativeList<float2>(vertices.Count, Allocator.Persistent);

			for (var i = 0; i < vertices.Count; i++) {
				verticesNative.Add(vertices[i]);
			}

			_v2Hash.Add(verticesNative);
			
			return verticesNative;
		}

		NativeList<int> GetOutlinesNative(IReadOnlyList<int> outline) {
			var outlineNative = new NativeList<int>(outline.Count, Allocator.Persistent);

			for (var i = 0; i < outline.Count; i++) {
				outlineNative.Add(outline[i]);
			}

			_intHash.Add(outlineNative);
			
			return outlineNative;
		}

		NativeList<float2> GetOutputListNative(int outlineCount) {
			var nA =  new NativeList<float2>(outlineCount, Allocator.Persistent);
			
			_v2Hash.Add(nA);

			return nA;
		}

		NativeList<float2> GetOutputListNativeFilled(NativeList<float2> nA) {
			var newArray = new NativeList<float2>(nA.Length, Allocator.Persistent);
			newArray.CopyFrom(nA);
			_v2Hash.Add(newArray);
			return newArray;
		}

		public void Dispose() {
			if (IsDisposed)
				return;  

			IsDisposed = true;

			foreach (var v2Hash in _v2Hash) {
				v2Hash.Dispose();
			}

			foreach (var intHash in _intHash) {
				intHash.Dispose();
			}
            
			// _populatePointsJob.Dispose();
			// _colinearityJob.Dispose();
		}
	}
}