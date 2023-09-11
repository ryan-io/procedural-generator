// ProceduralGeneration

using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct PopulateColliderPointsJob : IJob, IDisposable {
	[NativeDisableParallelForRestriction] public NativeList<float2> Output;
	[ReadOnly]                            public NativeList<float2> Vertices;
	[ReadOnly]                            public NativeList<int>    Outline;

	public bool IsDisposed;

	public void Execute() {
		for (var i = 0; i < Outline.Length; i++) {
			Output.Add(new float2(Vertices[Outline[i]].x, Vertices[Outline[i]].y));
		}
	}

	public void Dispose() {
		if (IsDisposed)
			return;

		IsDisposed = true;
		Vertices.Dispose();
		Outline.Dispose();
		Output.Dispose();
	}
}

[BurstCompile]
public struct DetermineColinearityJob : IJob, IDisposable {
	[ReadOnly]                            public NativeList<float2> PointsToCheck;
	[NativeDisableParallelForRestriction] public NativeList<float2> Processed;

	public bool IsDisposed;

	public void Execute() {
		for (var index = 0; index < PointsToCheck.Length; index++) {
			if (index == 0 || index == 1) {
				Processed.Add(PointsToCheck[index]);
				continue;
			}

			var p1 = PointsToCheck[index - 2];
			var p2 = PointsToCheck[index - 1];
			var p3 = PointsToCheck[index];

			if (IsColinear(p1, p2, p3)) {
				var removalIndex = Processed.IndexOf(p2);
				if (removalIndex != -1)
					Processed.RemoveAt(removalIndex);
			}

			// var p2Index = Processed.IndexOf(p2);
			//
			// if (p2Index == -1) {
			// 	Processed.Add(p2);
			// }
		}
	}

	public void Dispose() {
		if (IsDisposed)
			return;

		IsDisposed = true;
		PointsToCheck.Dispose();
		Processed.Dispose();
	}

	static bool IsColinear(float2 point1, float2 point2, float2 point3, float fPError = 0.0005f) {
		var left  = (point2.x - point1.x) * (point3.y - point2.y);
		var right = (point2.y - point1.y) * (point3.x - point2.x);

		return math.abs(left - right) < fPError;
	}
}