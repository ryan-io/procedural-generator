// ProceduralGeneration

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityBCL;
using UnityEngine;

namespace ProceduralGeneration {
	/// <summary>
	/// Represents a class responsible for processing boundary points.
	/// </summary>
	internal class BoundaryPointProcessor {
		/// <summary>
		/// Gets the dictionary of processed points.
		/// </summary>
		/// <value>
		/// The dictionary where the key is an integer and the value is a list of Vector2 points.
		/// </value>
		public Dictionary<int, List<Vector2>> ProcessedPoints { get; } = new();

		/// <summary>
		/// Processes the given outline by removing points that do not meet a specific condition.
		/// </summary>
		/// <param name="outlineIndex">The index of the outline.</param>
		/// <param name="outline">The outline to be processed.</param>
		[CanBeNull]
		public List<Vector2> Process(int outlineIndex, ref List<int> outline) {
			if (outline.Count < _cutOffPoints) {
				return Enumerable.Empty<Vector2>().ToList();
			}

			if (ProcessedPoints.ContainsKey(outlineIndex))
				throw new Exception("The outline has already been processed.");
			
			var workingList = new List<Vector2>();

			for (var i = 0; i < outline.Count; i++) 
				AddNewPoint(ref outline, ref workingList, i);

			var workingListCopy = new List<Vector2>(workingList);

			// i may need to start at 2
			for (var i = 2; i < workingList.Count; i++) 
				DetermineColinearity(ref workingList, ref workingListCopy, i);

			ProcessedPoints.Add(outlineIndex, workingListCopy);
			
			return ProcessedPoints[outlineIndex];
		}

		/// <summary>
		/// Adds a new point to the working list if it doesn't already exist.
		/// </summary>
		/// <param name="outline">The outline list.</param>
		/// <param name="workingList">The working list.</param>
		/// <param name="index">The index of the outline list to retrieve the new point from.</param>
		/// <remarks>
		/// This method retrieves a point from the outline list using the specified index.
		/// It then checks if the point already exists in the working list. If the point is
		/// not found in the working list, it is added to the list.
		/// </remarks>
		void AddNewPoint(ref List<int> outline, ref List<Vector2> workingList, int index) {
			var point = new Vector2(_meshVertices[outline[index]].x, _meshVertices[outline[index]].y);
			
			if (!workingList.Contains(point))
				workingList.Add(point);
		}

		/// <summary>
		/// Determines if three points are colinear and removes the middle point if it is.
		/// </summary>
		/// <param name="workingList">The original list of points.</param>
		/// <param name="workingListCopy">A copy of the original list of points.</param>
		/// <param name="i">The index of the current point in the list.</param>
		/// <remarks>
		/// This method checks if the three points at indices (i-2), (i-1), and (i) in the workingList are colinear.
		/// If they are colinear, the middle point (p2) is removed from the workingListCopy.
		/// </remarks>
		static void DetermineColinearity(ref List<Vector2> workingList, ref List<Vector2> workingListCopy, int i) {
			var p1 = workingList[i - 2];
			var p2 = workingList[i - 1];
			var p3 = workingList[i];

			if (!VectorF.IsColinear(p1, p2, p3))
				return;

			if (workingListCopy.Contains(p2))
				workingListCopy.Remove(p2);
		}

		/// <summary>
		/// Initializes a new instance of the BoundaryPointProcessor class.
		/// </summary>
		/// <param name="meshVertices">The list of vertices that make up the mesh.</param>
		/// <param name="cutOffPoints">The maximum number of boundary points to consider (default is 10).</param>
		public BoundaryPointProcessor(ref List<Vector3> meshVertices, int cutOffPoints = 10) {
			_meshVertices = meshVertices;
			_cutOffPoints = cutOffPoints;
		}

		readonly int                    _cutOffPoints;
		readonly IReadOnlyList<Vector3> _meshVertices;
	}
}