using CommunityToolkit.HighPerformance;

namespace ProceduralGeneration {
	internal abstract class SmoothMapSolver {
		internal abstract void Smooth(ref int[,] map, Dimensions dimensions);
	}
}

/*
 // In theory we should check for Vector256.IsHardwareAccelerated and dispatch
    // accordingly, in practice here we don't to keep the code simple.
    var vInts = MemoryMarshal.Cast<int, Vector256<int>>(data);

    var compareValue = Vector256.Create(value);
    var vectorLength = Vector256<int>.Count;

    // Batch <8 x int> per loop
    for (var i = 0; i < vInts.Length; i++)
    {
        var result = Vector256.Equals(vInts[i], compareValue);
        if (result == Vector256<int>.Zero) continue;

        for (var k = 0; k < vectorLength; k++)
            if (result.GetElement(k) != 0)
                return i * vectorLength + k;
    }

    // Scalar process of the remaining
    for (var i = vInts.Length * vectorLength; i < data.Length; i++)
        if (data[i] == value)
            return i;

    return -1;
*/